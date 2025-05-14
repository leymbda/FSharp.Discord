module FSharp.Discord.Gateway.Gateway

open Elmish
open FSharp.Control.Websockets
open FSharp.Discord.Types
open System
open System.Net.WebSockets
open System.Threading
open Thoth.Json.Net

type Handler = GatewayReceiveEvent -> Async<unit>

type GatewayState = {
    Sequence: int option
    HeartbeatInterval: int option
    HeartbeatAcked: bool
    HeartbeatNextDue: DateTime option
    ResumeGatewayUrl: string option
    SessionId: string option
}

module GatewayState =
    let zero () =
        {
            Sequence = None
            HeartbeatInterval = None
            HeartbeatAcked = true
            HeartbeatNextDue = None
            ResumeGatewayUrl = None
            SessionId = None
        }

type Model = {
    GatewayUri: Uri
    State: GatewayState
    Identify: IdentifySendEvent
    Handler: Handler
    Socket: ThreadSafeWebSocket.ThreadSafeWebSocket option
}

type Msg =
    | Connect of Uri
    | OnConnectSuccess of ThreadSafeWebSocket.ThreadSafeWebSocket
    | OnConnectError of exn

    | Reconnect of resumable: bool

    | Disconnect
    | OnDisconnect

    | Send of GatewaySendEvent
    | OnSendError of exn

    | Receive of GatewayReceiveEvent
    | OnReceiveError of exn

let connect uri = async {
    let ws = new ClientWebSocket()
    do! ws.ConnectAsync(uri, CancellationToken.None) |> Async.AwaitTask
    return ThreadSafeWebSocket.createFromWebSocket ws // TODO: Partially apply to allow testing with a mock socket
}

let disconnect model status = async {
    match model.Socket with
    | None ->
        return ()

    | Some socket ->
        do! ThreadSafeWebSocket.close socket status "" |> Async.Ignore // TODO: Informative status description
}

let send model event = async {
    match model.Socket with
    | None ->
        return ()

    | Some socket ->
        let content = Encode.toString 0 (GatewaySendEvent.encoder event)
        do! ThreadSafeWebSocket.sendMessageAsUTF8 socket content |> Async.Ignore
}

let init (gatewayUri, identify, handler) =
    {
        GatewayUri = gatewayUri
        State = GatewayState.zero ()
        Identify = identify
        Handler = handler
        Socket = None
    },
    Cmd.ofMsg (Msg.Connect gatewayUri)

let update msg model =
    match msg with
    | Msg.Connect uri ->
        model, Cmd.OfAsync.either connect uri Msg.OnConnectSuccess Msg.OnConnectError

    | Msg.OnConnectSuccess socket ->
        { model with Socket = Some socket }, Cmd.none

    | Msg.OnConnectError _ ->
        model, Cmd.ofMsg Msg.Disconnect

    | Msg.Reconnect resumable ->
        let model, reconnectUri =
            match resumable, model.State.ResumeGatewayUrl, model.State.SessionId, model.State.Sequence with
            | true, Some url, Some _, Some _ ->
                let state = { model.State with HeartbeatAcked = true; HeartbeatInterval = None }
                { model with State = state }, Uri url

            | _ ->
                { model with State = GatewayState.zero () }, model.GatewayUri

        model, Cmd.OfAsync.perform (disconnect model) WebSocketCloseStatus.Empty (fun _ -> Msg.Connect reconnectUri)

    | Msg.Disconnect ->
        model, Cmd.OfAsync.perform (disconnect model) WebSocketCloseStatus.NormalClosure (fun _ -> Msg.OnDisconnect)

    | Msg.OnDisconnect ->
        model, Cmd.none

    | Msg.Send event ->
        model, Cmd.OfAsync.attempt (send model) event Msg.OnSendError

    | Msg.OnSendError _ ->
        model, Cmd.none

    | Msg.Receive event ->
        match event with
        | GatewayReceiveEvent.HELLO data ->
            let sendEvent =
                match model.State.SessionId, model.State.Sequence with
                | Some sessionId, Some sequence ->
                    GatewaySendEvent.RESUME {
                        Token = model.Identify.Token
                        SessionId = sessionId
                        Sequence = sequence
                    }

                | _ ->
                    GatewaySendEvent.IDENTIFY model.Identify

            let state = {
                model.State with
                    HeartbeatInterval = Some data.HeartbeatInterval
                    HeartbeatNextDue = Some (DateTime.UtcNow.AddMilliseconds data.HeartbeatInterval) } // TODO: Remove DateTime.UtcNow side effect

            { model with State = state }, Cmd.ofMsg (Msg.Send sendEvent) // TODO: Initiate heartbeat

        | GatewayReceiveEvent.HEARTBEAT ->
            let sendEvent = GatewaySendEvent.HEARTBEAT model.State.Sequence

            let state = { model.State with HeartbeatAcked = false }
            { model with State = state }, Cmd.ofMsg (Msg.Send sendEvent)

        | GatewayReceiveEvent.HEARTBEAT_ACK ->
            let state = { model.State with HeartbeatAcked = true }
            { model with State = state }, Cmd.none

        | GatewayReceiveEvent.READY (data, sequence) ->
            let state =
                { model.State with
                    ResumeGatewayUrl = Some data.ResumeGatewayUrl
                    SessionId = Some data.SessionId
                    Sequence = Some sequence }

            { model with State = state }, Cmd.none

        | GatewayReceiveEvent.RESUMED ->
            model, Cmd.none

        | GatewayReceiveEvent.RECONNECT ->
            model, Cmd.ofMsg (Msg.Reconnect true)

        | GatewayReceiveEvent.INVALID_SESSION resumable ->
            model, Cmd.ofMsg (Msg.Reconnect resumable)

        | event ->
            model, Cmd.OfAsync.attempt model.Handler event Msg.OnReceiveError

    | Msg.OnReceiveError _ ->
        model, Cmd.none

let subscribe model =
    // TODO: Create subscription to gateway receive events
    // TODO: Create subscription to ws disconnect
    // TODO: Create subscription to heartbeat in model.State.HeartbeatNextDue

    []

let terminate msg =
    msg |> function | Msg.OnDisconnect -> true | _ -> false
