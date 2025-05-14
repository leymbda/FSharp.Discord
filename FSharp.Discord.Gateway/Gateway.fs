module FSharp.Discord.Gateway.Gateway

open Elmish
open FSharp.Control.Websockets
open FSharp.Discord.Types
open System
open System.Net.WebSockets
open System.Threading
open Thoth.Json.Net

type Handler = GatewayReceiveEvent -> Async<unit>

type Model = {
    GatewayUri: Uri
    Identify: IdentifySendEvent
    Handler: Handler
    Socket: ThreadSafeWebSocket.ThreadSafeWebSocket option
}

type Msg =
    | Connect of Uri
    | OnConnectSuccess of ThreadSafeWebSocket.ThreadSafeWebSocket
    | OnConnectError of exn

    | Disconnect of WebSocketCloseStatus option
    | OnDisconnect

    | Send of GatewaySendEvent
    | OnSendError of exn

    | Receive of GatewayReceiveEvent
    | OnReceiveError of exn

let connect uri = async {
    let ws = new ClientWebSocket()
    do! ws.ConnectAsync(uri, CancellationToken.None) |> Async.AwaitTask
    return ThreadSafeWebSocket.createFromWebSocket ws
}

let disconnect model status = async {
    match model.Socket with
    | None ->
        return ()

    | Some socket ->
        do! ThreadSafeWebSocket.close socket status "TODO: Informative status description" |> Async.Ignore
}

let send model event = async {
    match model.Socket with
    | None ->
        return ()

    | Some socket ->
        let content = Encode.toString 0 (GatewaySendEvent.encoder event)
        do! ThreadSafeWebSocket.sendMessageAsUTF8 socket content |> Async.Ignore
}

let receive model event = async {
    do! model.Handler event // TODO: Handle gateway lifecycle
}

let init (gatewayUri, identify, handler) =
    {
        GatewayUri = gatewayUri
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
        model, Cmd.ofMsg (Msg.Disconnect None)

    | Msg.Disconnect status ->
        let code = status |> Option.defaultValue WebSocketCloseStatus.Empty

        // TODO: Reconnect if allowed, otherwise finish and call OnDisconnect to terminate

        model, Cmd.OfAsync.perform (disconnect model) code (fun _ -> Msg.OnDisconnect)

    | Msg.OnDisconnect ->
        model, Cmd.none

    | Msg.Send event ->
        model, Cmd.OfAsync.attempt (send model) event Msg.OnSendError

    | Msg.OnSendError _ ->
        model, Cmd.none

    | Msg.Receive event ->
        model, Cmd.OfAsync.attempt (receive model) event Msg.OnReceiveError

    | Msg.OnReceiveError _ ->
        model, Cmd.none

let subscribe model =
    // TODO: Create subscription to gateway receive events
    // TODO: Create subscription to ws disconnect

    []

let terminate msg =
    msg |> function | Msg.OnDisconnect -> true | _ -> false
