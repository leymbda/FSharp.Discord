namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open Thoth.Json.Net
open WebSocketSharp

type Dispatcher = GatewayReceiveEvent -> unit

type GatewayConnectionState = {
    IdentifyEvent: IdentifySendEvent
    Sequence: int option
    Interval: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
    ResumeGatewayUrl: string option
    SessionId: string option
}

module GatewayConnectionState =
    let create identify = {
        IdentifyEvent = identify
        Sequence = None
        Interval = None
        Heartbeat = None
        HeartbeatAcked = true
        ResumeGatewayUrl = None
        SessionId = None
    }

type GatewayConnection(socket, state) =
    member val Socket: WebSocket = socket
    member val State: GatewayConnectionState = state

    interface IDisposable with
        member this.Dispose() =
            this.Socket :> IDisposable |> _.Dispose()

module GatewayConnection =
    let private onMessage (dispatcher: Dispatcher) (connection: GatewayConnection) = fun (event: GatewayReceiveEvent) ->
        // TODO: Handle lifecycle events or forward events to given handler
        raise (NotImplementedException())

    let private onClose (connection: GatewayConnection) = fun (code: GatewayCloseEventCode) ->
        // TODO: Check disconnection reason and either trigger resume/reconnect or close gateway (pass in signal? TBD)
        raise (NotImplementedException())

    let create gatewayUrl identify initialState dispatcher =
        let ws = new WebSocket(gatewayUrl)
        let state = Option.defaultValue (GatewayConnectionState.create identify) initialState
        let conenction = new GatewayConnection(ws, state)

        ws.OnMessage.Add(
            _.Data
            >> Decode.fromString GatewayReceiveEvent.decoder
            >> Result.iter (onMessage dispatcher conenction)
        )

        ws.OnClose.Add(
            _.Code
            >> int
            >> enum<GatewayCloseEventCode>
            >> onClose conenction
        )

        conenction

    let connect (connection: GatewayConnection) =
        connection.Socket.Connect()

    let close (connection: GatewayConnection) =
        connection.Socket.Close()

    let send event (connection: GatewayConnection) =
        let opcode, data =
            match event with
            | GatewaySendEvent.IDENTIFY d ->
                GatewayOpcode.IDENTIFY, GatewaySendEventData.IDENTIFY d

            | GatewaySendEvent.RESUME d ->
                GatewayOpcode.IDENTIFY, GatewaySendEventData.IDENTIFY d
            
            | GatewaySendEvent.HEARTBEAT d ->
                GatewayOpcode.HEARTBEAT, GatewaySendEventData.OPTIONAL_INT d

            | GatewaySendEvent.REQUEST_GUILD_MEMBERS d ->
                GatewayOpcode.REQUEST_GUILD_MEMBERS, GatewaySendEventData.REQUEST_GUILD_MEMBERS d

            | GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS d ->
                GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS, GatewaySendEventData.REQUEST_SOUNDBOARD_SOUNDS d

            | GatewaySendEvent.UPDATE_VOICE_STATE d ->
                GatewayOpcode.VOICE_STATE_UPDATE, GatewaySendEventData.UPDATE_VOICE_STATE d
            
            | GatewaySendEvent.UPDATE_PRESENCE d ->
                GatewayOpcode.PRESENCE_UPDATE, GatewaySendEventData.UPDATE_PRESENCE d

        { Opcode = opcode; Data = data; Sequence = None; EventName = None }
        |> GatewaySendEventPayload.encoder
        |> Encode.toString 0
        |> connection.Socket.Send
