module FSharp.Discord.Gateway.GatewayState

open Elmish
open FSharp.Discord.Types
open System

type Model = {
    // Args
    IdentifyEvent: IdentifySendEvent
    Handler: GatewayHandler

    // Lifecycle state
    Interval: int option
    ResumeGatewayUrl: string option
    SessionId: string option
    SequenceId: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool

    // Child Models
    SendQueue: GatewaySendQueue.Model
}

type Msg =
    | Send of GatewaySendEvent
    | Receive of GatewayReceiveEvent * string
    | Queue of GatewaySendQueue.Msg
    | Ignore

let init (identifyEvent, handler) =
    let sendQueue, sendQueueCmd = GatewaySendQueue.init()
    {
        IdentifyEvent = identifyEvent
        Handler = handler
        SequenceId = None
        Interval = None
        Heartbeat = None
        HeartbeatAcked = true
        ResumeGatewayUrl = None
        SessionId = None
        SendQueue = sendQueue
    },
    Cmd.batch [
        Cmd.map Msg.Queue sendQueueCmd
    ]

let private send model (ev: GatewaySendEvent) =
    model, Cmd.ofMsg (Msg.Queue (GatewaySendQueue.Msg.Enqueue ev))

let private receive model (ev: GatewayReceiveEvent) (raw: string) =
    match ev with
    | GatewayReceiveEvent.HELLO ev ->
        match model.SessionId, model.SequenceId with
        | Some sessionId, Some sequenceId ->
            let event = GatewayEventPayload.create(GatewayOpcode.RESUME, ResumeSendEvent.create(model.IdentifyEvent.Token, sessionId, sequenceId)) |> GatewaySendEvent.RESUME

            { model with Interval = Some ev.Data.HeartbeatInterval },
            Cmd.ofMsg (Msg.Send event)

        | _ ->
            let event = GatewayEventPayload.create(GatewayOpcode.IDENTIFY, model.IdentifyEvent) |> GatewaySendEvent.IDENTIFY

            { model with Interval = Some ev.Data.HeartbeatInterval },
            Cmd.ofMsg (Msg.Send event)

    | GatewayReceiveEvent.HEARTBEAT ev ->
        let event = GatewayEventPayload.create(GatewayOpcode.HEARTBEAT, model.SequenceId) |> GatewaySendEvent.HEARTBEAT

        { model with Heartbeat = model.Interval |> Option.map DateTime.UtcNow.AddMilliseconds }, // TODO: Remove current time dependency
        Cmd.ofMsg (Msg.Send event)

    | GatewayReceiveEvent.HEARTBEAT_ACK ev ->
        { model with HeartbeatAcked = true }, Cmd.none

    | GatewayReceiveEvent.READY ev ->
        { model with ResumeGatewayUrl = Some ev.Data.ResumeGatewayUrl; SessionId = Some ev.Data.SessionId }, Cmd.none

    | GatewayReceiveEvent.RESUMED ev ->
        model, Cmd.none // TODO: Mark state as active? Same with ready?

    | GatewayReceiveEvent.RECONNECT ev ->
        match model.ResumeGatewayUrl, model.SessionId, model.SequenceId with
        | Some resumeGatewayUrl, Some sessionId, Some sequenceId -> model, Cmd.none // TODO: Resume
        | _ -> model, Cmd.none // TODO: Reconnect

    | GatewayReceiveEvent.INVALID_SESSION ev ->
        match ev.Data, model.ResumeGatewayUrl, model.SessionId, model.SequenceId with
        | true, Some resumeGatewayUrl, Some sessionId, Some sequenceId -> model, Cmd.none // TODO: Resume
        | _ -> model, Cmd.none // TODO: Reconnect

    | ev ->
        model, Cmd.OfAsync.perform (fun r -> model.Handler r |> Async.AwaitTask) raw (fun _ -> Msg.Ignore)

let update env msg model =
    match msg with
    | Msg.Send ev -> send model ev
    | Msg.Receive (ev, raw) -> receive model ev raw
    | Msg.Queue msg' ->
        let res, cmd = GatewaySendQueue.update env msg' model.SendQueue
        { model with SendQueue = res }, Cmd.map Msg.Queue cmd
    | Msg.Ignore -> model, Cmd.none

let view model dispatch =
    ()

let program env identify handler =
    Program.mkProgram init (update env) view
    // TODO: Add subscriptions to trigger send and receive messages with the websocket here `|> Program.withSubscription ...` (?)
    |> Program.runWith (identify, handler)
