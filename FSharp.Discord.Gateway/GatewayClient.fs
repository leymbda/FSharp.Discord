namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open System
open System.Threading
open System.Threading.Tasks

type IGatewayClient =
    inherit IAsyncDisposable

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
        cancellationToken: CancellationToken ->
        Task<Result<unit, WebsocketError>>

    abstract RequestSoundboardSounds:
        guildIds: string list ->
        cancellationToken: CancellationToken ->
        Task<Result<unit, WebsocketError>>

    abstract UpdateVoiceState:
        guildId: string ->
        channelId: string option ->
        selfMute: bool ->
        selfDeaf: bool ->
        cancellationToken: CancellationToken ->
        Task<Result<unit, WebsocketError>>

    abstract UpdatePresence:
        since: int option ->
        activities: Activity list option ->
        status: Status ->
        afk: bool option ->
        cancellationToken: CancellationToken ->
        Task<Result<unit, WebsocketError>>

type GatewayClient (websocketFactory: IWebsocketFactory) =
    member val private _ws: IWebsocket ref = ref (websocketFactory.CreateClient()) with get, set

    interface IGatewayClient with
        member this.Connect gatewayUrl identify handler ct = task {
            let mutable disconnectReason = ReconnectableGatewayDisconnect.Reconnect
            let mutable closeCode: GatewayCloseEventCode option option = None

            while Option.isNone closeCode do
                this._ws.Value <- websocketFactory.CreateClient()
            
                let! disconnect =
                    match disconnectReason with
                    | ReconnectableGatewayDisconnect.Reconnect -> Gateway.connect identify handler gatewayUrl None ct this._ws.Value
                    | ReconnectableGatewayDisconnect.Resume resumeData -> Gateway.connect identify handler gatewayUrl (Some resumeData) ct this._ws.Value

                match disconnect with
                | Error code -> closeCode <- Some code
                | Ok reason -> disconnectReason <- reason

            return closeCode |> Option.bind id
        }

        member this.RequestGuildMembers guildId query limit presences userIds nonce ct = task {
            return! Gateway.requestGuildMembers guildId query limit presences userIds nonce ct this._ws.Value
        }
        
        member this.RequestSoundboardSounds guildIds ct = task {
            return! Gateway.requestSoundboardSounds guildIds ct this._ws.Value
        }

        member this.UpdateVoiceState guildId channelId selfMute selfDeaf ct = task {
            return! Gateway.updateVoiceState guildId channelId selfMute selfDeaf ct this._ws.Value
        }

        member this.UpdatePresence since activities status afk ct = task {
            return! Gateway.updatePresence since activities status afk ct this._ws.Value
        }

    interface IAsyncDisposable with
        member this.DisposeAsync () =
            this._ws.Value.DisposeAsync()

type IGatewayClientFactory =
    abstract CreateClient: unit -> IGatewayClient

type GatewayClientFactory (websocketFactory: IWebsocketFactory) =
    interface IGatewayClientFactory with
        member _.CreateClient () =
            new GatewayClient(websocketFactory)
