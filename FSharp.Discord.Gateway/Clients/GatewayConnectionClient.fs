namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Net.WebSockets
open System.Threading
open System.Threading.Tasks

type ReconnectableGatewayDisconnect =
    | Resume of ResumeGatewayUrl: string
    | Reconnect

type IGatewayConnectionClient =
    abstract Connected: bool

    abstract Connect:
        gatewayUrl: string ->
        Task<Result<ReconnectableGatewayDisconnect, GatewayCloseEventCode option>>

    abstract Resume:
        resumeGatewayUrl: string ->
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

type GatewayConnectionClient (identify: IdentifySendEvent, handler: string -> Task<unit>, ws: ClientWebSocket) =
    member val Connected = false with get, set

    interface IGatewayConnectionClient with
        member this.Connected = this.Connected

        member _.Connect gatewayUrl = task {
            return Error None // TODO: Handle fresh connect here
        }

        member _.Resume resumeGatewayUrl = task {
            return Error None // TODO: Handle resuming here
        }

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
        member _.Dispose () = ()
