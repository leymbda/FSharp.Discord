namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Text.Json
open System.Threading
open System.Threading.Tasks

type GatewayHandler = string -> Task<unit>

type ResumeData = {
    ResumeGatewayUrl: string
    SessionId: string
    SequenceId: int
}

type OldGatewayState = {
    IdentifyEvent: IdentifySendEvent
    SequenceId: int option
    Interval: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
    ResumeGatewayUrl: string option
    SessionId: string option
}

module OldGatewayState =
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
    | Continue of OldGatewayState
    | Resume of ResumeData
    | Reconnect
    | Disconnect of GatewayCloseEventCode option

type ReconnectableGatewayDisconnect =
    | Resume of ResumeData
    | Reconnect

module Gateway =
    /// Send an identify event.
    let identify payload ct ws = task {
        let event = GatewaySendEvent.IDENTIFY ({ Opcode = GatewayOpcode.IDENTIFY; Data = payload; Sequence = None; EventName = None })
        return! Websocket.write (Json.serializeF event) ct ws
    }

    /// Send a resume event.
    let resume token sessionId sequenceId ct ws = task {
        let payload = { Token = token; SessionId = sessionId; Sequence = sequenceId }
        let event = GatewaySendEvent.RESUME ({ Opcode = GatewayOpcode.RESUME; Data = payload; Sequence = None; EventName = None })
        return! Websocket.write (Json.serializeF event) ct ws
    }

    /// Send a heartbeat event.
    let heartbeat sequenceId ct ws = task {
        let event = GatewaySendEvent.HEARTBEAT ({ Opcode = GatewayOpcode.HEARTBEAT; Data = sequenceId; Sequence = None; EventName = None })
        return! Websocket.write (Json.serializeF event) ct ws    
    }

    /// Send a request guild members event.
    let requestGuildMembers guildId query limit presences userIds nonce ct ws = task {
        let payload = {
            GuildId = guildId
            Query = query
            Limit = limit
            Presences = presences
            UserIds = userIds
            Nonce = nonce
        }

        let event = GatewaySendEvent.REQUEST_GUILD_MEMBERS ({ Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS; Data = payload; Sequence = None; EventName = None })
        return! Websocket.write (Json.serializeF event) ct ws
    }

    /// Send a request soundboard sounds event.
    let requestSoundboardSounds guildIds ct ws = task {
        let payload = { GuildIds = guildIds }
        let event = GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS ({ Opcode = GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS; Data = payload; Sequence = None; EventName = None })
        return! Websocket.write (Json.serializeF event) ct ws
    }

    /// Send an update voice state event.
    let updateVoiceState guildId channelId selfMute selfDeaf ct ws = task {
        let payload = { GuildId = guildId; ChannelId = channelId; SelfMute = selfMute; SelfDeaf = selfDeaf }
        let event = GatewaySendEvent.UPDATE_VOICE_STATE ({ Opcode = GatewayOpcode.VOICE_STATE_UPDATE; Data = payload; Sequence = None; EventName = None })
        return! Websocket.write (Json.serializeF event) ct ws
    }

    /// Send an update presence event.
    let updatePresence since activities status afk ct ws = task {
        let payload: UpdatePresenceSendEvent = { Status = status; Activities = activities; Afk = afk; Since = since }
        let event = GatewaySendEvent.UPDATE_PRESENCE ({ Opcode = GatewayOpcode.PRESENCE_UPDATE; Data = payload; Sequence = None; EventName = None })
        return! Websocket.write (Json.serializeF event) ct ws
    }

    /// Handle a gateway event. Appropriately handles lifecycle events, and uses the provided handler for the rest.
    let handleEvent (event, raw) state (handler: GatewayHandler) ct ws =
        match event, raw with
        | GatewayReceiveEvent.HELLO ev, _ ->
            match state.SessionId, state.SequenceId with
            | Some sessionId, Some sequenceId -> resume state.IdentifyEvent.Token sessionId sequenceId ct ws |> ignore
            | _ -> identify state.IdentifyEvent ct ws |> ignore

            LifecycleResult.Continue { state with Interval = Some ev.Data.HeartbeatInterval }

        | GatewayReceiveEvent.HEARTBEAT _, _ ->
            heartbeat state.SequenceId ct ws |> ignore

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
    
    /// Handle the lifecycle of the gateway connection, including heartbeat.
    let handleLifecycle (event: Task<Result<(GatewayReceiveEvent * string), GatewayCloseEventCode option>>) (timeout: Task) state handler ct ws = task {
        let! winner = Task.WhenAny(event, timeout)

        match winner, state.HeartbeatAcked with
        | winner, false when winner = timeout ->
            match state.ResumeGatewayUrl, state.SessionId, state.SequenceId with
            | Some resumeGatewayUrl, Some sessionId, Some sequenceId -> return LifecycleResult.Resume { ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId; SequenceId = sequenceId }
            | _, _, _ -> return LifecycleResult.Reconnect

        | winner, true when winner = timeout ->
            heartbeat state.SequenceId ct ws |> ignore

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
                return handleEvent (event, raw) state handler ct ws
    }

    /// Start a connection to the gateway. Automatically reconnects on disconnect when appropriate.
    let startConnection identify handler gatewayUrl (resumeData: ResumeData option) ct (ws: IWebsocket) = task {
        let url =
            match resumeData with
            | Some { ResumeGatewayUrl = url } -> url
            | None -> gatewayUrl

        do! ws.ConnectAsync(Uri url, ct)

        let mutable state = OldGatewayState.zero identify resumeData
        let mutable disconnectCause: Result<ReconnectableGatewayDisconnect, GatewayCloseEventCode option> option = None

        while disconnectCause.IsNone do
            let event =
                let mapper = Result.mapError (fun _ -> None) 
                let binder = Result.bind (function
                    | WebsocketResponse.Close code -> Error (Option.map enum<GatewayCloseEventCode> code)
                    | WebsocketResponse.Message message -> Ok (Json.deserializeF<GatewayReceiveEvent> message, message)
                )

                Websocket.readNext ct ws |> Task.map (mapper >> binder)

            let timeout =
                match state.Heartbeat with
                | Some h -> h.Subtract DateTime.UtcNow
                | None -> Timeout.InfiniteTimeSpan
                |> Task.Delay

                // TODO: Implement jitter here

            let! res = handleLifecycle event timeout state handler ct ws
                
            match res with
            | LifecycleResult.Continue newState -> state <- newState
            | LifecycleResult.Resume resumeData -> disconnectCause <- Some (Ok (ReconnectableGatewayDisconnect.Resume resumeData))
            | LifecycleResult.Reconnect -> disconnectCause <- Some (Ok ReconnectableGatewayDisconnect.Reconnect)
            | LifecycleResult.Disconnect code -> disconnectCause <- Some (Error code)

        return disconnectCause.Value
    }

    // TODO: Handle enqueuing send events to ensure only one is sent at a time (add to queue synchronously, then work through as freed up, basically any use of `ignore` in this file)
