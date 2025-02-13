namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Threading
open System.Threading.Tasks

type DurableGatewaySendEventFailure =
    | NoConnectionClient
    | NotConnected

type IGatewayConnectionClientFactory =
    abstract CreateClient:
        identify: IdentifySendEvent ->
        handler: GatewayHandler ->
        IGatewayConnectionClient

type IDurableGatewayClient =
    abstract Connect:
        gatewayUrl: string ->
        identify: IdentifySendEvent ->
        handler: GatewayHandler ->
        cancellationToken: CancellationToken ->
        Task<GatewayCloseEventCode option>

    abstract RequestGuildMembers:
        guildId: string ->
        query: string option ->
        limit: int ->
        presences: bool option ->
        userIds: string list option ->
        nonce: string option ->
        Task<Result<unit, DurableGatewaySendEventFailure>>

    abstract RequestSoundboardSounds:
        guildIds: string list ->
        Task<Result<unit, DurableGatewaySendEventFailure>>

    abstract UpdateVoiceState:
        guildId: string ->
        channelId: string option ->
        selfMute: bool ->
        selfDeaf: bool ->
        Task<Result<unit, DurableGatewaySendEventFailure>>

    abstract UpdatePresence:
        since: int option ->
        activities: Activity list option ->
        status: Status ->
        afk: bool option ->
        Task<Result<unit, DurableGatewaySendEventFailure>>

type DurableGatewayClient (connectionFactory: IGatewayConnectionClientFactory) =
    member val GatewayConnectionClient: IGatewayConnectionClient option = None with get, set

    interface IDurableGatewayClient with
        member this.Connect gatewayUrl identify handler ct = task {
            let mutable disconnectReason = ReconnectableGatewayDisconnect.Reconnect
            let mutable closeCode: GatewayCloseEventCode option option = None

            while Option.isNone closeCode do
                let client = connectionFactory.CreateClient identify handler
                this.GatewayConnectionClient <- Some client
            
                let! disconnect =
                    match disconnectReason with
                    | ReconnectableGatewayDisconnect.Reconnect -> client.Connect gatewayUrl ct
                    | ReconnectableGatewayDisconnect.Resume resumeData -> client.Resume gatewayUrl resumeData ct

                match disconnect with
                | Error code -> closeCode <- Some code
                | Ok reason -> disconnectReason <- reason

            return closeCode |> Option.bind id
        }

        member this.RequestGuildMembers guildId query limit presences userIds nonce = task {
            match this.GatewayConnectionClient with
            | None ->
                return Error DurableGatewaySendEventFailure.NoConnectionClient

            | Some client when not client.Connected ->
                return Error DurableGatewaySendEventFailure.NotConnected

            | Some client -> 
                do! client.RequestGuildMembers guildId query limit presences userIds nonce
                return Ok ()
        }
        
        member this.RequestSoundboardSounds guildIds = task {
            match this.GatewayConnectionClient with
            | None ->
                return Error DurableGatewaySendEventFailure.NoConnectionClient

            | Some client when not client.Connected ->
                return Error DurableGatewaySendEventFailure.NotConnected

            | Some client -> 
                do! client.RequestSoundboardSounds guildIds
                return Ok ()
        }

        member this.UpdateVoiceState guildId channelId selfMute selfDeaf = task {
            match this.GatewayConnectionClient with
            | None ->
                return Error DurableGatewaySendEventFailure.NoConnectionClient

            | Some client when not client.Connected ->
                return Error DurableGatewaySendEventFailure.NotConnected

            | Some client -> 
                do! client.UpdateVoiceState guildId channelId selfMute selfDeaf
                return Ok ()
        }

        member this.UpdatePresence since activities status afk = task {
            match this.GatewayConnectionClient with
            | None ->
                return Error DurableGatewaySendEventFailure.NoConnectionClient

            | Some client when not client.Connected ->
                return Error DurableGatewaySendEventFailure.NotConnected

            | Some client -> 
                do! client.UpdatePresence since activities status afk
                return Ok ()
        }

    interface IDisposable with
        member _.Dispose () = ()
