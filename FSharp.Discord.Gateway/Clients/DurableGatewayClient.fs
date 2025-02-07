namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Threading.Tasks

type IGatewayConnectionClientFactory =
    abstract CreateClient:
        identify: IdentifySendEvent ->
        handler: (string -> Task<unit>) ->
        IGatewayConnectionClient

type IDurableGatewayClient =
    abstract Connect:
        gatewayUrl: string ->
        identify: IdentifySendEvent ->
        handler: (string -> Task<unit>) ->
        Task<GatewayCloseEventCode option>

    abstract RequestGuildMembers:
        guildId: string ->
        query: string option ->
        limit: int ->
        presences: bool option ->
        userIds: string list option ->
        nonce: string option ->
        Task<bool>

    abstract RequestSoundboardSounds:
        guildIds: string list ->
        Task<bool>

    abstract UpdateVoiceState:
        guildId: string ->
        channelId: string option ->
        selfMute: bool ->
        selfDeaf: bool ->
        Task<bool>

    abstract UpdatePresence:
        since: int option ->
        activities: Activity list option ->
        status: Status ->
        afk: bool option ->
        Task<bool>

type DurableGatewayClient (connectionFactory: IGatewayConnectionClientFactory) =
    member val GatewayConnectionClient: IGatewayConnectionClient option = None with get, set

    interface IDurableGatewayClient with
        member this.Connect gatewayUrl identify handler = task {
            let mutable disconnectReason = ReconnectableGatewayDisconnect.Reconnect
            let mutable closeCode: GatewayCloseEventCode option option = None

            while Option.isNone closeCode do
                let client = connectionFactory.CreateClient identify handler
                this.GatewayConnectionClient <- Some client
            
                let! disconnect =
                    match disconnectReason with
                    | ReconnectableGatewayDisconnect.Reconnect -> client.Connect gatewayUrl
                    | ReconnectableGatewayDisconnect.Resume resumeGateawyUrl -> client.Resume resumeGateawyUrl

                match disconnect with
                | Error code -> closeCode <- Some code
                | Ok reason -> disconnectReason <- reason

            return closeCode |> Option.bind id
        }

        member this.RequestGuildMembers guildId query limit presences userIds nonce = task {
            match this.GatewayConnectionClient with
            | None ->
                return false

            | Some client when not client.Connected ->
                return false

            | Some client -> 
                do! client.RequestGuildMembers guildId query limit presences userIds nonce
                return true
        }
        
        member this.RequestSoundboardSounds guildIds = task {
            match this.GatewayConnectionClient with
            | None ->
                return false

            | Some client when not client.Connected ->
                return false

            | Some client -> 
                do! client.RequestSoundboardSounds guildIds
                return true
        }

        member this.UpdateVoiceState guildId channelId selfMute selfDeaf = task {
            match this.GatewayConnectionClient with
            | None ->
                return false

            | Some client when not client.Connected ->
                return false

            | Some client -> 
                do! client.UpdateVoiceState guildId channelId selfMute selfDeaf
                return true
        }

        member this.UpdatePresence since activities status afk = task {
            match this.GatewayConnectionClient with
            | None ->
                return false

            | Some client when not client.Connected ->
                return false

            | Some client -> 
                do! client.UpdatePresence since activities status afk
                return true
        }

    interface IDisposable with
        member _.Dispose () = ()
