namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Net.WebSockets
open System.Text.Json
open System.Threading
open System.Threading.Tasks

type ReconnectableGatewayDisconnect =
    | Resume of ResumeData
    | Reconnect

type IGatewayConnectionClient =
    abstract Connected: bool with get

    abstract Connect:
        gatewayUrl: string ->
        cancellationToken: CancellationToken ->
        Task<Result<ReconnectableGatewayDisconnect, GatewayCloseEventCode option>>

    abstract Resume:
        gatewayUrl: string ->
        resumeData: ResumeData ->
        cancellationToken: CancellationToken ->
        Task<Result<ReconnectableGatewayDisconnect, GatewayCloseEventCode option>>

    abstract RequestGuildMembers:
        guildId: string ->
        query: string option ->
        limit: int ->
        presences: bool option ->
        userIds: string list option ->
        nonce: string option ->
        Task<unit>

    abstract RequestSoundboardSounds:
        guildIds: string list ->
        Task<unit>

    abstract UpdateVoiceState:
        guildId: string ->
        channelId: string option ->
        selfMute: bool ->
        selfDeaf: bool ->
        Task<unit>

    abstract UpdatePresence:
        since: int option ->
        activities: Activity list option ->
        status: Status ->
        afk: bool option ->
        Task<unit>

type GatewayConnectionClient (identify: IdentifySendEvent, handler: GatewayHandler, ws: ClientWebSocket) =
    member private this.connect (gatewayUrl: string) (resumeData: ResumeData option) ct = task {
        let url =
            match resumeData with
            | Some { ResumeGatewayUrl = url } -> url
            | None -> gatewayUrl

        do! ws.ConnectAsync(Uri url, ct)
        this.Connected <- true

        let mutable state = GatewayState.zero identify resumeData
        let mutable disconnectCause: Result<ReconnectableGatewayDisconnect, GatewayCloseEventCode option> option = None

        while disconnectCause.IsNone do
            let event = Websocket.readNext ws ct |> Task.map (function
                | WebsocketReadResponse.Close code -> Error (Option.map enum<GatewayCloseEventCode> code)
                | WebsocketReadResponse.Message message -> Ok (Json.deserializeF<GatewayReceiveEvent> message, message))

            let timeout =
                match state.Heartbeat with
                | Some h -> h.Subtract DateTime.UtcNow
                | None -> Timeout.InfiniteTimeSpan
                |> Task.Delay

            let! res = Gateway.handle event timeout state handler ws ct
                
            match res with
            | LifecycleResult.Continue newState -> state <- newState
            | LifecycleResult.Resume resumeData -> disconnectCause <- Some (Ok (ReconnectableGatewayDisconnect.Resume resumeData))
            | LifecycleResult.Reconnect -> disconnectCause <- Some (Ok ReconnectableGatewayDisconnect.Reconnect)
            | LifecycleResult.Disconnect code -> disconnectCause <- Some (Error code)

        this.Connected <- false
        return disconnectCause.Value
    }
    
    member val Connected = false with get, set
    
    interface IGatewayConnectionClient with
        member this.Connected = this.Connected // TODO: Test if this behaves as intended

        member this.Connect gatewayUrl ct =
            this.connect gatewayUrl None ct

        member this.Resume gatewayUrl resumeData ct =
            this.connect gatewayUrl (Some resumeData) ct

        member _.RequestGuildMembers guildId query limit presences userIds nonce = task {
            let payload = RequestGuildMembersSendEvent.create(guildId, limit, ?Presences = presences, ?Query = query, ?UserIds = userIds, ?Nonce = nonce)
            let event = GatewaySendEvent.REQUEST_GUILD_MEMBERS (GatewayEventPayload.create(GatewayOpcode.REQUEST_GUILD_MEMBERS, payload))

            do! Gateway.send event ws CancellationToken.None
        }

        member _.RequestSoundboardSounds guildIds = task {
            let payload = RequestSoundboardSoundsSendEvent.create(guildIds)
            let event = GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS (GatewayEventPayload.create(GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS, payload))
            
            do! Gateway.send event ws CancellationToken.None
        }

        member _.UpdateVoiceState guildId channelId selfMute selfDeaf = task {
            let payload = UpdateVoiceStateSendEvent.create(guildId, channelId, selfMute, selfDeaf)
            let event = GatewaySendEvent.UPDATE_VOICE_STATE (GatewayEventPayload.create(GatewayOpcode.VOICE_STATE_UPDATE, payload))

            do! Gateway.send event ws CancellationToken.None
        }

        member _.UpdatePresence since activities status afk = task {
            let payload = UpdatePresenceSendEvent.create(status, ?Activities = activities, ?Afk = afk, ?Since = since)
            let event = GatewaySendEvent.UPDATE_PRESENCE (GatewayEventPayload.create(GatewayOpcode.PRESENCE_UPDATE, payload))

            do! Gateway.send event ws CancellationToken.None
        }

    interface IDisposable with
        member _.Dispose () = () // TODO: Implement (?)
