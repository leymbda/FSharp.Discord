namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Net.WebSockets
open System.Text.Json
open System.Threading
open System.Threading.Tasks

type LifecycleState = {
    IdentifyEvent: IdentifySendEvent
    SequenceId: int option
    Interval: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
    ResumeGatewayUrl: string option
    SessionId: string option
}

module LifecycleState =
    let zero (identifyEvent: IdentifySendEvent) = {
        IdentifyEvent = identifyEvent
        SequenceId = None
        Interval = None
        Heartbeat = None
        HeartbeatAcked = true
        ResumeGatewayUrl = None
        SessionId = None
    }
        
type LifecycleContinuation =
    | Resume of ResumeGatewayUrl: string
    | Reconnect
    | Close of code: GatewayCloseEventCode option

type ConnectState = {
    GatewayUrl: string
    ResumeGatewayUrl: string option
    IdentifyEvent: IdentifySendEvent
}

module ConnectState =
    let zero gatewayUrl identifyEvent = {
        GatewayUrl = gatewayUrl
        ResumeGatewayUrl = None
        IdentifyEvent = identifyEvent
    }

    let getConnectionUrl (state: ConnectState) =
        state.ResumeGatewayUrl
        |> Option.defaultValue state.GatewayUrl
        |> Uri

module Gateway = 
    let send (event: GatewaySendEvent) ws =
        ws |> Websocket.write (Json.serializeF event)

    let rec lifecycle (state: LifecycleState) handler ws (ct: CancellationToken) = task {
        match ct.IsCancellationRequested with
        | true -> return LifecycleContinuation.Close None
        | false ->
            let timeout =
                match state.Heartbeat with
                | Some h -> h.Subtract DateTime.UtcNow
                | None -> Timeout.InfiniteTimeSpan
                |> Task.Delay

            let event =
                ws |> Websocket.readNext ?> function
                | WebsocketReadResponse.Close code -> Error (Option.map enum<GatewayCloseEventCode> code)
                | WebsocketReadResponse.Message message -> Ok (Json.deserializeF<GatewayReceiveEvent> message, message)

            let! winner = Task.WhenAny(timeout, event)

            match winner, state.HeartbeatAcked with
            | winner, false when winner = timeout ->
                match state.ResumeGatewayUrl with
                | Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                | None -> return LifecycleContinuation.Reconnect

            | winner, true when winner = timeout ->
                let sendEvent = GatewayEventPayload.create(GatewayOpcode.HEARTBEAT, state.SequenceId) |> GatewaySendEvent.HEARTBEAT
                ws |> send sendEvent |> ignore

                return! lifecycle { state with HeartbeatAcked = false } handler ws ct
            
            | _ ->
                match event.Result with
                | Error code ->
                    let reconnecting =
                        match code with
                        | Some c -> GatewayCloseEventCode.shouldReconnect c
                        | None -> false

                    match reconnecting, state.ResumeGatewayUrl with
                    | true, Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                    | true, None -> return LifecycleContinuation.Reconnect
                    | false, _ -> return LifecycleContinuation.Close code

                | Ok (GatewayReceiveEvent.HELLO event, _) ->
                    let interval = Some event.Data.HeartbeatInterval

                    let sendEvent =
                        match state.SessionId, state.SequenceId with
                        | Some sessionId, Some sequenceId -> GatewayEventPayload.create(GatewayOpcode.RESUME, ResumeSendEvent.create(state.IdentifyEvent.Token, sessionId, sequenceId)) |> GatewaySendEvent.RESUME
                        | _ -> GatewayEventPayload.create(GatewayOpcode.IDENTIFY, state.IdentifyEvent) |> GatewaySendEvent.IDENTIFY

                    ws |> send sendEvent |> ignore

                    return! lifecycle { state with Interval = interval } handler ws ct

                | Ok (GatewayReceiveEvent.HEARTBEAT _, _) ->
                    let freshHeartbeat = state.Interval |> Option.map (fun i -> DateTime.UtcNow.AddMilliseconds(i))
                
                    let sendEvent = GatewayEventPayload.create(GatewayOpcode.HEARTBEAT, state.SequenceId) |> GatewaySendEvent.HEARTBEAT
                    ws |> send sendEvent |> ignore

                    return! lifecycle { state with Heartbeat = freshHeartbeat } handler ws ct

                | Ok (GatewayReceiveEvent.HEARTBEAT_ACK _, _) ->
                    return! lifecycle { state with HeartbeatAcked = true } handler ws ct

                | Ok (GatewayReceiveEvent.READY event, _) ->
                    let resumeGatewayUrl = Some event.Data.ResumeGatewayUrl
                    let sessionId = Some event.Data.SessionId

                    return! lifecycle { state with ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId } handler ws ct

                | Ok (GatewayReceiveEvent.RESUMED _, _) ->
                    return! lifecycle state handler ws ct

                | Ok (GatewayReceiveEvent.RECONNECT _, _) ->
                    match state.ResumeGatewayUrl with
                    | Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                    | None -> return LifecycleContinuation.Reconnect

                | Ok (GatewayReceiveEvent.INVALID_SESSION event, _) ->
                    match event.Data, state.ResumeGatewayUrl with
                    | true, Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                    | _ -> return LifecycleContinuation.Reconnect

                | Ok (_, event) ->
                    handler event |> ignore

                    match JsonDocument.Parse(event).RootElement.TryGetProperty "s" with
                    | true, t -> return! lifecycle { state with SequenceId = t.GetInt32() |> Some } handler ws ct
                    | _ -> return! lifecycle state handler ws ct
    }

    let rec connect (state: ConnectState) handler (socketRef: ClientWebSocket option ref) (ct: CancellationToken) = task {
        let ws = new ClientWebSocket()
        socketRef.Value <- Some ws
        do! ws.ConnectAsync(ConnectState.getConnectionUrl state, CancellationToken.None)

        let lifecycleState = { LifecycleState.zero state.IdentifyEvent with ResumeGatewayUrl = state.ResumeGatewayUrl }

        match! lifecycle lifecycleState handler ws ct with
        | LifecycleContinuation.Resume resumeGatewayUrl ->
            do! ws.CloseAsync(WebSocketCloseStatus.Empty, "Resuming", CancellationToken.None)
            return! connect { state with ResumeGatewayUrl = Some resumeGatewayUrl } handler socketRef ct

        | LifecycleContinuation.Reconnect ->
            do! ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Reconnecting", CancellationToken.None)
            return! connect { state with ResumeGatewayUrl = None } handler socketRef ct

        | LifecycleContinuation.Close code ->
            do! ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
            return code
    }
