namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Net.WebSockets
open System.Text.Json
open System.Threading
open System.Threading.Tasks

type GatewayHandler = string -> Task<unit>

type ResumeData = {
    ResumeGatewayUrl: string
    SessionId: string
    SequenceId: int
}

type GatewayState = {
    IdentifyEvent: IdentifySendEvent
    SequenceId: int option
    Interval: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
    ResumeGatewayUrl: string option
    SessionId: string option
}

module GatewayState =
    let connect (identifyEvent: IdentifySendEvent) = {
        IdentifyEvent = identifyEvent
        SequenceId = None
        Interval = None
        Heartbeat = None
        HeartbeatAcked = true
        ResumeGatewayUrl = None
        SessionId = None
    }

    let resume (identifyEvent: IdentifySendEvent) (resumeData: ResumeData) = {
        IdentifyEvent = identifyEvent
        SequenceId = Some resumeData.SequenceId
        Interval = None
        Heartbeat = None
        HeartbeatAcked = true
        ResumeGatewayUrl = Some resumeData.ResumeGatewayUrl
        SessionId = Some resumeData.SessionId
    }

    let zero (identifyEvent: IdentifySendEvent) (resumeData: ResumeData option) =
        match resumeData with
        | Some d -> resume identifyEvent d
        | None -> connect identifyEvent

type LifecycleResult =
    | Continue of GatewayState
    | Resume of ResumeData
    | Reconnect
    | Disconnect of GatewayCloseEventCode option

module Gateway = 
    let send (event: GatewaySendEvent) ws ct =
        Websocket.write (Json.serializeF event) ws ct

    let handleEvent (event: GatewayReceiveEvent) (raw: string) (state: GatewayState) (handler: GatewayHandler) (ws: ClientWebSocket) (ct: CancellationToken) =
        match event, raw with
        | GatewayReceiveEvent.HELLO ev, _ ->
            match state.SessionId, state.SequenceId with
            | Some sessionId, Some sequenceId -> GatewayEventPayload.create(GatewayOpcode.RESUME, ResumeSendEvent.create(state.IdentifyEvent.Token, sessionId, sequenceId)) |> GatewaySendEvent.RESUME
            | _ -> GatewayEventPayload.create(GatewayOpcode.IDENTIFY, state.IdentifyEvent) |> GatewaySendEvent.IDENTIFY
            |> fun sendEvent -> send sendEvent ws ct
            |> ignore

            LifecycleResult.Continue { state with Interval = Some ev.Data.HeartbeatInterval }

        | GatewayReceiveEvent.HEARTBEAT _, _ ->
            GatewayEventPayload.create(GatewayOpcode.HEARTBEAT, state.SequenceId) |> GatewaySendEvent.HEARTBEAT
            |> fun sendEvent -> send sendEvent ws ct
            |> ignore

            LifecycleResult.Continue { state with Heartbeat = state.Interval |> Option.map (fun i -> DateTime.UtcNow.AddMilliseconds(i)) }
        
        | GatewayReceiveEvent.HEARTBEAT_ACK _, _ ->
            LifecycleResult.Continue { state with HeartbeatAcked = true }

        | GatewayReceiveEvent.READY ev, _ ->
            LifecycleResult.Continue { state with ResumeGatewayUrl = Some ev.Data.ResumeGatewayUrl; SessionId = Some ev.Data.SessionId }

        | GatewayReceiveEvent.RESUMED _, _ ->
            LifecycleResult.Continue state

        | GatewayReceiveEvent.RECONNECT _, _ ->
            match state.ResumeGatewayUrl, state.SessionId, state.SequenceId with
            | Some resumeGatewayUrl, Some sessionId, Some sequenceId -> LifecycleResult.Resume { ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId; SequenceId = sequenceId }
            | _ -> LifecycleResult.Reconnect

        | GatewayReceiveEvent.INVALID_SESSION ev, _ ->
            match ev.Data, state.ResumeGatewayUrl, state.SessionId, state.SequenceId with
            | true, Some resumeGatewayUrl, Some sessionId, Some sequenceId -> LifecycleResult.Resume { ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId; SequenceId = sequenceId }
            | _ -> LifecycleResult.Reconnect

        | (_, raw) ->
            handler raw |> ignore
            
            match JsonDocument.Parse(raw).RootElement.TryGetProperty "s" with
            | true, t -> LifecycleResult.Continue { state with SequenceId = Some (t.GetInt32()) }
            | _ -> LifecycleResult.Continue state
    
    let handle
        (event: Task<Result<(GatewayReceiveEvent * string), GatewayCloseEventCode option>>)
        (timeout: Task)
        (state: GatewayState)
        (handler: GatewayHandler)
        (ws: ClientWebSocket)
        (ct: CancellationToken)
        = task {
            let! winner = Task.WhenAny(event, timeout)

            match winner, state.HeartbeatAcked with
            | winner, false when winner = timeout ->
                match state.ResumeGatewayUrl, state.SessionId, state.SequenceId with
                | Some resumeGatewayUrl, Some sessionId, Some sequenceId -> return LifecycleResult.Resume { ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId; SequenceId = sequenceId }
                | _, _, _ -> return LifecycleResult.Reconnect

            | winner, true when winner = timeout ->
                GatewayEventPayload.create (GatewayOpcode.HEARTBEAT, state.SequenceId) |> GatewaySendEvent.HEARTBEAT
                |> fun sendEvent -> send sendEvent ws ct
                |> ignore

                return LifecycleResult.Continue { state with HeartbeatAcked = false }

            | _ ->
                match event.Result with
                | Error code ->
                    let reconnecting =
                        match code with
                        | Some c -> GatewayCloseEventCode.shouldReconnect c
                        | None -> false

                    match reconnecting, state.ResumeGatewayUrl, state.SessionId, state.SequenceId with
                    | true, Some resumeGatewayUrl, Some sessionId, Some sequenceId -> return LifecycleResult.Resume { ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId; SequenceId = sequenceId }
                    | true, _, _, _ -> return LifecycleResult.Reconnect
                    | false, _, _, _ -> return LifecycleResult.Disconnect code

                | Ok (event, raw) ->
                    return handleEvent event raw state handler ws ct
        }

    // TODO: Probably rename above functions to nicer names
    // TODO: Handle jitter (where it should be done TBD)
