module FSharp.Discord.Gateway.GatewayState

open Elmish
open FSharp.Discord.Types
open System
open System.Text.Json
open System.Threading.Tasks

type Socket (ws: IWebsocket, id) =
    member val private _observers: IObserver<(GatewayReceiveEvent * string)> list = [] with get, set

    member val Id: Guid = id with get

    interface IObservable<(GatewayReceiveEvent * string)> with
        member this.Subscribe observer =
            this._observers <- this._observers @ [observer]
            { new IDisposable with member _.Dispose () = this._observers <- this._observers |> List.except [observer] }

    member this.Connect uri ct = task {
        do! ws.ConnectAsync(uri, ct)

        let mapper = Result.mapError (fun _ -> None)
        let binder = Result.bind (function
            | WebsocketResponse.Close code -> Error (Option.map enum<GatewayCloseEventCode> code)
            | WebsocketResponse.Message message -> Ok (Json.deserializeF<GatewayReceiveEvent> message, message)
        )

        let! res = Websocket.readNext ct ws |> Task.map (mapper >> binder)

        match res with
        | Error _ -> this._observers |> List.iter (_.OnCompleted()) // TODO: Pass actual error code to handle resume/reconnect
        | Ok (ev, raw) -> this._observers |> List.iter (_.OnNext(ev, raw))
    }

// TODO: Clean up above and probably move to a separate file. Should be able to remove IWebsocket as well

type Model = {
    // Args
    IdentifyEvent: IdentifySendEvent
    Handler: GatewayHandler

    // Lifecycle state
    Active: bool
    Interval: int option
    ResumeGatewayUrl: string option
    SessionId: string option
    SequenceId: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool

    // Child Models
    SendQueue: GatewaySendQueue.Model

    // Other state
    Socket: Socket option
}

type Msg =
    | Send of GatewaySendEvent
    | Receive of GatewayReceiveEvent * string

    // TODO: Create message to initialise with connect (and run as cmd on init?)
    // TODO: Figure out how to handle disconnect and termination (probably needs a msg here)

    | Resume
    | Reconnect

    | SetActive of bool
    | Queue of GatewaySendQueue.Msg
    | Ignore

let init (identifyEvent, handler) =
    let sendQueue, sendQueueCmd = GatewaySendQueue.init()

    {
        IdentifyEvent = identifyEvent
        Handler = handler
        Active = false
        Interval = None
        ResumeGatewayUrl = None
        SessionId = None
        SequenceId = None
        Heartbeat = None
        HeartbeatAcked = true
        SendQueue = sendQueue
        Socket = None // TODO: How can this be passed down into the GatewaySendQueue appropriately?
    },
    Cmd.map Msg.Queue sendQueueCmd

let private receive (env: #IGetCurrentTime) model (ev: GatewayReceiveEvent) (raw: string) =
    match ev, raw with
    | GatewayReceiveEvent.HELLO ev, _ ->
        match model.SessionId, model.SequenceId with
        | Some sessionId, Some sequenceId ->
            let event = GatewayEventPayload.create(GatewayOpcode.RESUME, ResumeSendEvent.create(model.IdentifyEvent.Token, sessionId, sequenceId)) |> GatewaySendEvent.RESUME

            { model with Interval = Some ev.Data.HeartbeatInterval },
            Cmd.ofMsg (Msg.Send event)

        | _ ->
            let event = GatewayEventPayload.create(GatewayOpcode.IDENTIFY, model.IdentifyEvent) |> GatewaySendEvent.IDENTIFY

            { model with Interval = Some ev.Data.HeartbeatInterval },
            Cmd.ofMsg (Msg.Send event)

    | GatewayReceiveEvent.HEARTBEAT ev, _ ->
        let event = GatewayEventPayload.create(GatewayOpcode.HEARTBEAT, model.SequenceId) |> GatewaySendEvent.HEARTBEAT

        { model with Heartbeat = model.Interval |> Option.map (env.GetCurrentTime().AddMilliseconds) },
        Cmd.ofMsg (Msg.Send event)

    | GatewayReceiveEvent.HEARTBEAT_ACK ev, _ ->
        { model with HeartbeatAcked = true }, Cmd.none

    | GatewayReceiveEvent.READY ev, _ ->
        { model with ResumeGatewayUrl = Some ev.Data.ResumeGatewayUrl; SessionId = Some ev.Data.SessionId },
        Cmd.ofMsg (Msg.SetActive true)

    | GatewayReceiveEvent.RESUMED _, _ ->
        model, Cmd.ofMsg (Msg.SetActive true)

    | GatewayReceiveEvent.RECONNECT _, _ ->
        match model.ResumeGatewayUrl, model.SessionId, model.SequenceId with
        | Some _, Some _, Some _ -> model, Cmd.ofMsg (Msg.SetActive false) // TODO: Resume
        | _ -> model, Cmd.ofMsg (Msg.SetActive false) // TODO: Reconnect

    | GatewayReceiveEvent.INVALID_SESSION ev, _ ->
        match ev.Data, model.ResumeGatewayUrl, model.SessionId, model.SequenceId with
        | true, Some _, Some _, Some _ -> model, Cmd.ofMsg (Msg.SetActive false) // TODO: Resume
        | _ -> model, Cmd.ofMsg (Msg.SetActive false) // TODO: Reconnect

    | _, raw ->
        model, Cmd.OfAsync.perform (fun r -> model.Handler r |> Async.AwaitTask) raw (fun _ -> Msg.Ignore)

let resume env model =
    model, Cmd.none // TODO: Trigger socket setup and startup with existing model state (do reconnect if invalid state)

let reconnect env model =
    init(model.IdentifyEvent, model.Handler) // TODO: Trigger socket setup and startup

let private setActive model active =
    let opcode =
        match active with
        | false -> CustomGatewayOpcode.INACTIVE
        | true -> CustomGatewayOpcode.ACTIVE

    let ev =
        GatewayReceiveEvent.UNKNOWN (GatewayEventPayload.create (CustomGatewayOpcode.cast opcode, ())),
        $"""{{"op": {CustomGatewayOpcode.toString opcode}}}"""
    
    // TODO: Clean up this (should this even be sending in the same way as a custom event?)
    
    { model with Active = active }, Cmd.ofMsg (Msg.Receive ev)

let update env msg model =
    match msg with
    | Msg.Send ev -> model, Cmd.ofMsg (Msg.Queue (GatewaySendQueue.Msg.Enqueue ev))
    | Msg.Receive (ev, raw) -> receive env model ev raw
    | Msg.Resume -> resume env model
    | Msg.Reconnect -> reconnect env model
    | Msg.SetActive active -> setActive model active
    | Msg.Queue msg' ->
        let res, cmd = GatewaySendQueue.update env msg' model.SendQueue
        { model with SendQueue = res }, Cmd.map Msg.Queue cmd
    | Msg.Ignore -> model, Cmd.none

let view model dispatch =
    ()

let subscribe model =
    match model.Socket with
    | None -> []
    | Some socket -> [
        ["websocket"; socket.Id.ToString()],
        fun dispatch -> socket.Subscribe(fun (ev, raw) -> dispatch (Msg.Receive (ev, raw)));

        // TODO: Handle heartbeat timeout as a second subscription (probably)
    ]

let program env identify handler =
    Task.Run(fun () ->
        Program.mkProgram init (update env) view
        |> Program.withSubscription subscribe
        |> Program.runWith (identify, handler)
    )

    // TODO: Check if this resolves when program ends, otherwise figure out `withSubscription`/`withTermination` type logic

// TODO: Figure out how to notify that the gateway disconencted irrecoverably (likely cant use `Program.runWith` as it starts single threaded loop)
