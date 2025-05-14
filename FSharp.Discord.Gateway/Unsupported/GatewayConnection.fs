namespace FSharp.Discord.Gateway.Unsupported

open FSharp.Discord.Gateway
open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open System.Threading
open System.Threading.Tasks
open Thoth.Json.Net
open WebSocketSharp

type Dispatcher = GatewayReceiveEvent -> unit

type GatewayConnectionState = {
    IdentifyEvent: IdentifySendEvent
    Sequence: int option
    HeartbeatInterval: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
    ResumeGatewayUrl: string option
    SessionId: string option
}

module GatewayConnectionState =
    let create identify = {
        IdentifyEvent = identify
        Sequence = None
        HeartbeatInterval = None
        Heartbeat = None
        HeartbeatAcked = true
        ResumeGatewayUrl = None
        SessionId = None
    }

type GatewayConnection(socket, state) =
    member val Socket: WebSocket = socket with get, set
    member val State: GatewayConnectionState = state with get, set

    member val CancellationTokenSource = new CancellationTokenSource() with get, set
    member val Heartbeat: Task option = None with get, set

    interface IDisposable with
        member this.Dispose() =
            this.Socket :> IDisposable |> _.Dispose()

module GatewayConnection =
    let send event (connection: GatewayConnection) =
        let opcode, data =
            match event with
            | GatewaySendEvent.IDENTIFY d ->
                GatewayOpcode.IDENTIFY, GatewaySendEventData.IDENTIFY d

            | GatewaySendEvent.RESUME d ->
                GatewayOpcode.RESUME, GatewaySendEventData.RESUME d
            
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

    let private onMessage (dispatcher: Dispatcher) (connection: GatewayConnection) = fun (event: GatewayReceiveEvent) ->
        let state = connection.State

        match event with
        | GatewayReceiveEvent.HELLO data ->
            let event =
                match state.SessionId, state.Sequence with
                | Some sessionId, Some sequence ->
                    GatewaySendEvent.RESUME {
                        Token = state.IdentifyEvent.Token
                        SessionId = sessionId
                        Sequence = sequence
                    }

                | _ ->
                    GatewaySendEvent.IDENTIFY state.IdentifyEvent

            send event connection

            connection.State <- { state with HeartbeatInterval = Some data.HeartbeatInterval }

            connection.Heartbeat <- Some (task {
                while not connection.CancellationTokenSource.IsCancellationRequested do
                    if not connection.State.HeartbeatAcked then
                        do! connection.CancellationTokenSource.CancelAsync()
                    else
                        connection.State <- { state with HeartbeatAcked = false }
                        send (GatewaySendEvent.HEARTBEAT state.Sequence) connection

                    do! Task.Delay (data.HeartbeatInterval |> float |> TimeSpan.FromMilliseconds)

                // TODO: Make this not so crude... There's probably a more functional way to do this, may involve
                // switching from this pre-release package to FSharp.Control.Websockets. I fully expect this not to
                // fully work, and I think state.Heartbeat is a remnant of previous code and should be used for the
                // Task.Delay instead. Currently this will ignore immediate heartbeat requests entirely and just do
                // its own thing.
            })

        | GatewayReceiveEvent.HEARTBEAT ->
            let event = GatewaySendEvent.HEARTBEAT state.Sequence
            send event connection
            
            let heartbeat = Option.map (float >> DateTime.UtcNow.AddMilliseconds) state.HeartbeatInterval // TODO: Remove DateTime.UtcNow side effect
            connection.State <- { state with Heartbeat = heartbeat; HeartbeatAcked = false }

        | GatewayReceiveEvent.HEARTBEAT_ACK ->
            connection.State <- { state with HeartbeatAcked = true }

        | GatewayReceiveEvent.READY (data, sequence) ->
            let resumeGatewayUrl = Some data.ResumeGatewayUrl
            let sessionId = Some data.SessionId
            connection.State <- { state with ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId; Sequence = Some sequence }

        | GatewayReceiveEvent.RESUMED ->
            ()

        | GatewayReceiveEvent.RECONNECT ->
            match state.ResumeGatewayUrl, state.SessionId, state.Sequence with
            | Some resumeGatewayUrl, Some sessionId, Some sequenceId -> () // TODO: Trigger resume (close)
            | _ -> () // TODO: Trigger reconnect (close)

        | GatewayReceiveEvent.INVALID_SESSION resumable ->
            match resumable, state.ResumeGatewayUrl, state.SessionId, state.Sequence with
            | true, Some resumeGatewayUrl, Some sessionId, Some sequenceId -> () // TODO: Trigger resume (close)
            | _ -> () // TODO: Trigger reconnect (close)

        | event ->
            dispatcher event

    let private onClose (connection: GatewayConnection) = fun (code: GatewayCloseEventCode) ->
        match GatewayCloseEventCode.shouldReconnect code with
        | true -> () // TODO: Trigger reconnect (close)
        | false -> () // TODO: Abandon connection with attempting a reconnect

    let create gatewayUrl identify initialState dispatcher =
        let state = initialState |> Option.defaultValue (GatewayConnectionState.create identify)
        let url = state.ResumeGatewayUrl |> Option.defaultValue gatewayUrl

        let ws = new WebSocket(url)
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
        connection.CancellationTokenSource <- new CancellationTokenSource()
        connection.Heartbeat <- None

    let close (connection: GatewayConnection) =
        connection.Socket.Close()
        connection.CancellationTokenSource.Cancel()
        connection.Heartbeat <- None
