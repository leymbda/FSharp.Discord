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

    | Connect of AsyncEitherMsg<Uri option, ThreadSafeWebSocket.ThreadSafeWebSocket>
    | Reconnect of AsyncPerformMsg<Uri option, ThreadSafeWebSocket.ThreadSafeWebSocket>
    | Disconnect of AsyncPerformMsg<unit, unit>

    // TODO: Handle websocket action success/failure
    // TODO: Handle sending and receiving gateway events
    // TODO: Msg to detect for termination

let private connect model resumeUri = async {
    let uri = resumeUri |> Option.defaultValue model.GatewayUri

    let ws = new ClientWebSocket() // TODO: Partially apply to allow testing with a mock socket
    do! ws.ConnectAsync(uri, CancellationToken.None) |> Async.AwaitTask
    return ThreadSafeWebSocket.createFromWebSocket ws
}

let init (gatewayUri, identify, handler) =
    Model.zero gatewayUri identify handler, Cmd.none

let update msg model =
    match model.Lifecycle, model.Heartbeat, msg with
    // Connect
    | None, None, Msg.Connect (AsyncEitherMsg.Either resumeUri) ->
        let lifecycle, cmd = Lifecycle.init model.Identify

        { model with Lifecycle = Some lifecycle },
        Cmd.batch [
            Cmd.map Msg.Lifecycle cmd
            AsyncEitherMsg.toCmd (connect model) resumeUri Msg.Connect
        ]

    | None, None, Msg.Connect (AsyncEitherMsg.Success socket) ->
        model, Cmd.none // TODO: Implement

    | None, None, Msg.Connect (AsyncEitherMsg.Failure exn) ->
        eprintfn "%A" exn
        model, Cmd.none // TODO: Implement

    // Reconnect
    | _, _, Msg.Reconnect (AsyncPerformMsg.Perform resumeUri) ->
        model, Cmd.none // TODO: Trigger disconnect then reconnect
        
    | _, _, Msg.Reconnect (AsyncPerformMsg.Success socket) ->
        model, Cmd.none // TODO: Implement

    // TODO: Reconnect should instead call disconnect then connect...
    // TODO: If above, disconnect needs to know whether to or not to terminate

    // Disconnect
    | _, _, Msg.Disconnect (AsyncPerformMsg.Perform _) ->
        model, Cmd.none // TODO: Implement
        
    | _, _, Msg.Disconnect (AsyncPerformMsg.Success _) ->
        model, Cmd.none // TODO: Implement

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

        { model with Lifecycle = Some updated }, Cmd.map Msg.Lifecycle cmd
        
        // TODO: Send event
        
    // Lifecycle requested restart
    | Some lifecycle, Some heartbeat, Msg.Lifecycle ((Lifecycle.Msg.Restart resumeData) as msg) ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated }, Cmd.map Msg.Lifecycle cmd
        
        // TODO: Handle resume or reconnect based on `resumeData`
        // TODO: Handle restarting new heartbeat
        
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
    
    // Heartbeat requested stop
    | _, Some (Heartbeat.State.Active _ as heartbeat), Msg.Heartbeat (Heartbeat.Msg.Stop as msg) ->
        let updated, cmd = Heartbeat.update msg heartbeat

        { model with Heartbeat = Some updated },
        Cmd.batch [
            Cmd.map Msg.Heartbeat cmd
            Cmd.ofMsg (Msg.Disconnect (AsyncPerformMsg.Perform ()))
        ]

        // TODO: Handle heartbeat stop

    // Catch remaining heartbeat messages
    | _, Some heartbeat, Msg.Heartbeat msg ->
        let updated, cmd = Heartbeat.update msg heartbeat

        { model with Heartbeat = Some updated }, Cmd.map Msg.Heartbeat cmd

    // Catch invalid messages
    | lifecycle, heartbeat, msg ->
        eprintfn "Attempted to call msg %A in invate state %A %A" msg lifecycle heartbeat
        model, Cmd.ofMsg (Msg.Disconnect (AsyncPerformMsg.Perform ()))

let subscribe model: Sub<Msg> =
    [] // TODO: Implement all necessary subscriptions
