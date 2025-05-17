module FSharp.Discord.Gateway.Gateway

open Elmish
open FSharp.Control.Websockets
open FSharp.Discord.Gateway
open FSharp.Discord.Types
open System
open System.Net.WebSockets
open System.Threading

type Handler = GatewayReceiveEvent -> Async<unit>

type Model = {
    GatewayUri: Uri
    Identify: IdentifySendEvent
    Handler: Handler
    Socket: ThreadSafeWebSocket.ThreadSafeWebSocket option
    Heartbeat: Heartbeat.Model option
    Lifecycle: Lifecycle.Model option
}

module Model =
    let zero gatewayUri identify handler =
        {
            GatewayUri = gatewayUri
            Identify = identify
            Handler = handler
            Socket = None
            Lifecycle = None
            Heartbeat = None
        }

type Msg =
    | Heartbeat of Heartbeat.Msg
    | Lifecycle of Lifecycle.Msg

    | Connect of AsyncEitherMsg<unit, ThreadSafeWebSocket.ThreadSafeWebSocket>
    | Reconnect of AsyncEitherMsg<Lifecycle.ResumeData option, ThreadSafeWebSocket.ThreadSafeWebSocket>
    | Disconnect of AsyncPerformMsg<unit, unit>
    | Terminate

    | Receive of GatewayReceiveEvent
    | Handle of AsyncAttemptMsg<GatewayReceiveEvent>
    | Send of AsyncAttemptMsg<GatewaySendEvent>

let private connect model () = async {
    let ws = new ClientWebSocket() // TODO: Partially apply to allow testing with a mock socket
    do! ws.ConnectAsync(model.GatewayUri, CancellationToken.None) |> Async.AwaitTask
    return ThreadSafeWebSocket.createFromWebSocket ws
}

let private reconnect model (resumeData: Lifecycle.ResumeData option) = async {
    // TODO: Disconnect if currently connected

    let uri =
        resumeData
        |> Option.map (fun data -> Uri data.ResumeGatewayUrl)
        |> Option.defaultValue model.GatewayUri

    let ws = new ClientWebSocket() // TODO: Partially apply to allow testing with a mock socket
    do! ws.ConnectAsync(uri, CancellationToken.None) |> Async.AwaitTask
    return ThreadSafeWebSocket.createFromWebSocket ws
}

let private disconnect model () = async {
    return ()

    // TODO: Implement disconnect
}

let private receive model event = async {
    do! model.Handler event

    // TODO: Update handler to handle failures
}

let private send model event = async {
    return ()

    // TODO: Attempt to send event to socket
}

let init (gatewayUri, identify, handler) =
    Model.zero gatewayUri identify handler, Cmd.none

let update msg model =
    let currentTime = DateTime.UtcNow // TODO: Extract elsewhere to remove side effect

    match model.Lifecycle, model.Heartbeat, msg with
    // Connect
    | None, None, Msg.Connect (AsyncEitherMsg.Either _) ->
        let lifecycle, cmd = Lifecycle.init model.Identify None

        { model with Lifecycle = Some lifecycle },
        Cmd.batch [
            Cmd.map Msg.Lifecycle cmd
            AsyncEitherMsg.toCmd (connect model) () Msg.Connect
        ]

    | None, None, Msg.Connect (AsyncEitherMsg.Success socket) ->
        model, Cmd.none // TODO: Implement

    | None, None, Msg.Connect (AsyncEitherMsg.Failure exn) ->
        eprintfn "%A" exn
        model, Cmd.none // TODO: Handle failure

    // Reconnect
    | _, _, Msg.Reconnect (AsyncEitherMsg.Either resumeData) ->
        let state: Lifecycle.SessionState option =
            resumeData
            |> Option.map (fun data -> {
                SessionId = data.SessionId
                Sequence = data.Sequence
            })

        let lifecycle, cmd = Lifecycle.init model.Identify state

        { model with Lifecycle = Some lifecycle },
        Cmd.batch [
            Cmd.map Msg.Lifecycle cmd
            AsyncEitherMsg.toCmd (reconnect model) resumeData Msg.Reconnect
        ]

    | None, None, Msg.Reconnect (AsyncEitherMsg.Success socket) ->
        model, Cmd.none // TODO: Implement

    | None, None, Msg.Reconnect (AsyncEitherMsg.Failure exn) ->
        eprintfn "%A" exn
        model, Cmd.none // TODO: Handle failure

    // Disconnect
    | _, _, Msg.Disconnect (AsyncPerformMsg.Perform _) ->
        model, AsyncPerformMsg.toCmd (disconnect model) () Msg.Disconnect
        
    | _, _, Msg.Disconnect (AsyncPerformMsg.Success _) ->
        model, Cmd.ofMsg Msg.Terminate

    // Terminate
    | _, _, Msg.Terminate ->
        model, Cmd.none

    // Lifecycle gateway receive events
    | Some _, _, Msg.Receive (GatewayReceiveEvent.HELLO data) ->
        model, Cmd.ofMsg (Msg.Lifecycle (Lifecycle.Msg.Hello data))

    | Some _, _, Msg.Receive (GatewayReceiveEvent.READY (data, sequence)) ->
        model, Cmd.ofMsg (Msg.Lifecycle (Lifecycle.Msg.Ready (data, sequence)))

    | Some _, _, Msg.Receive (GatewayReceiveEvent.RESUMED) ->
        model, Cmd.ofMsg (Msg.Lifecycle Lifecycle.Msg.Resumed)

    | Some _, _, Msg.Receive (GatewayReceiveEvent.RECONNECT) ->
        model, Cmd.ofMsg (Msg.Lifecycle Lifecycle.Msg.Reconnect)

    | Some _, _, Msg.Receive (GatewayReceiveEvent.INVALID_SESSION resumable) ->
        model, Cmd.ofMsg (Msg.Lifecycle (Lifecycle.Msg.InvalidSession resumable))

    // Lifecycle hello start heartbeat
    | Some (Lifecycle.State.Starting _ as lifecycle), None, Msg.Lifecycle (Lifecycle.Msg.Hello data as msg) ->
        let interval = TimeSpan.FromMilliseconds data.HeartbeatInterval

        let heartbeat, hcmd = Heartbeat.init interval
        let updated, lcmd = Lifecycle.update msg lifecycle

        { model with
            Lifecycle = Some updated
            Heartbeat = Some heartbeat },
        Cmd.batch [
            Cmd.map Msg.Lifecycle lcmd
            Cmd.map Msg.Heartbeat hcmd
            Cmd.ofMsg (Msg.Heartbeat Heartbeat.Msg.Start)
        ]

    // Lifecycle requested send gateway event
    | Some lifecycle, _, Msg.Lifecycle ((Lifecycle.Msg.Send event) as msg) ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated },
        Cmd.batch [
            Cmd.map Msg.Lifecycle cmd
            Cmd.ofMsg (Msg.Send (AsyncAttemptMsg.Attempt event))
        ]
        
    // Lifecycle requested restart
    | Some lifecycle, _, Msg.Lifecycle ((Lifecycle.Msg.Restart resumeData) as msg) ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated },
        Cmd.batch [
            Cmd.map Msg.Lifecycle cmd
            Cmd.ofMsg (Msg.Reconnect (AsyncEitherMsg.Either resumeData))
        ]
        
    // Lifecycle requested stop
    | Some lifecycle, _, Msg.Lifecycle (Lifecycle.Msg.Stop as msg) ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated },
        Cmd.batch [
            Cmd.map Msg.Lifecycle cmd
            Cmd.ofMsg (Msg.Disconnect (AsyncPerformMsg.Perform ()))
        ]

    // Catch remaining lifecycle messages
    | Some lifecycle, _, Msg.Lifecycle msg ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated }, Cmd.map Msg.Lifecycle cmd
    
    // Heartbeat gateway receive events
    | _, Some _, Msg.Receive (GatewayReceiveEvent.HEARTBEAT) ->
        model, Cmd.ofMsg (Msg.Heartbeat Heartbeat.Msg.Beat)

    | _, Some _, Msg.Receive (GatewayReceiveEvent.HEARTBEAT_ACK) ->
        model, Cmd.ofMsg (Msg.Heartbeat Heartbeat.Msg.Ack)

    // Heartbeat requested stop
    | lifecycle, Some (Heartbeat.State.Active _ as heartbeat), Msg.Heartbeat (Heartbeat.Msg.Stop as msg) ->
        let resumeGatewayUrl =
            lifecycle |> Option.bind _.ResumeGatewayUrl

        let state =
            lifecycle |> Option.bind _.SessionState

        let resumeData: Lifecycle.ResumeData option =
            Option.map2 (fun a b -> a, b) resumeGatewayUrl state
            |> Option.map (fun (resumeGatewayUrl, state) -> {
                ResumeGatewayUrl = resumeGatewayUrl
                SessionId = state.SessionId
                Sequence = state.Sequence
            })

        let updated, cmd = Heartbeat.update msg heartbeat

        { model with Heartbeat = Some updated },
        Cmd.batch [
            Cmd.map Msg.Heartbeat cmd
            Cmd.ofMsg (Msg.Reconnect (AsyncEitherMsg.Either resumeData))
        ]

    // Catch remaining heartbeat messages
    | _, Some heartbeat, Msg.Heartbeat msg ->
        let updated, cmd = Heartbeat.update msg heartbeat

        { model with Heartbeat = Some updated }, Cmd.map Msg.Heartbeat cmd

    // Handle remaining gateway receive events through model handler
    | Some (Lifecycle.State.Active _), Some (Heartbeat.State.Alive currentTime _), Msg.Receive event ->
        model, Cmd.ofMsg (Msg.Handle (AsyncAttemptMsg.Attempt event))

    | Some (Lifecycle.State.Active _), Some (Heartbeat.State.Alive currentTime _), Msg.Handle (AsyncAttemptMsg.Attempt event) ->
        model, AsyncAttemptMsg.toCmd (receive model) event Msg.Handle

    | Some (Lifecycle.State.Active _), Some (Heartbeat.State.Alive currentTime _), Msg.Handle (AsyncAttemptMsg.Failure exn) ->
        eprintfn "%A" exn
        model, Cmd.none // TODO: Handle failure
        
    // Send gateway events
    | _, _, Msg.Send (AsyncAttemptMsg.Attempt event) ->
        model, AsyncAttemptMsg.toCmd (send model) event Msg.Send

    | _, _, Msg.Send (AsyncAttemptMsg.Failure exn) ->
        eprintfn "%A" exn
        model, Cmd.none // TODO: Handle failure

    // Catch invalid messages
    | lifecycle, heartbeat, msg ->
        eprintfn "Attempted to call msg %A in invate state %A %A" msg lifecycle heartbeat
        model, Cmd.ofMsg (Msg.Disconnect (AsyncPerformMsg.Perform ()))

        // TODO: Currently kills conenction on invalid state, likely want to handle gracefully after testing

let subscribe model: Sub<Msg> =
    [] // TODO: Implement all necessary subscriptions

let terminate msg =
    match msg with
    | Msg.Terminate -> true
    | _ -> false
