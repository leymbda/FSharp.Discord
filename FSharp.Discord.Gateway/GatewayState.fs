module FSharp.Discord.Gateway.GatewayState

open Elmish
open FSharp.Discord.Types
open System

type QueuedSendEvent =
    | Pending of GatewaySendEvent
    | Processing

type Model = {
    // Program state
    SendQueue: QueuedSendEvent list

    // Args
    IdentifyEvent: IdentifySendEvent

    // Lifecycle state
    Interval: int option
    ResumeGatewayUrl: string option
    SessionId: string option
    SequenceId: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
}

let init identifyEvent =
    {
        SendQueue = []
        IdentifyEvent = identifyEvent
        SequenceId = None
        Interval = None
        Heartbeat = None
        HeartbeatAcked = true
        ResumeGatewayUrl = None
        SessionId = None
    },
    Cmd.none

type Msg =
    // Core interactivity
    | Send of GatewaySendEvent
    | Receive of GatewayReceiveEvent

    // Handle send queue
    | Enqueue of GatewaySendEvent
    | StartProcessNext
    | EndProcessNext

    // Send events
    | SendIdentify of GatewayEventPayload<IdentifySendEvent>
    | SendResume of GatewayEventPayload<ResumeSendEvent>
    | SendHeartbeat of GatewayEventPayload<HeartbeatSendEvent>
    | SendRequestGuildMembers of GatewayEventPayload<RequestGuildMembersSendEvent>
    | SendRequestSoundboardSounds of GatewayEventPayload<RequestSoundboardSoundsSendEvent>
    | SendUpdateVoiceState of GatewayEventPayload<UpdateVoiceStateSendEvent>
    | SendUpdatePresence of GatewayEventPayload<UpdatePresenceSendEvent>

    // Receive events
    | ReceiveHello of GatewayEventPayload<HelloReceiveEvent>
    | ReceiveHeartbeat of GatewayEventPayload<HeartbeatReceiveEvent>
    | ReceiveHeartbeatAck of GatewayEventPayload<HeartbeatAckReceiveEvent>
    | ReceiveReady of GatewayEventPayload<ReadyReceiveEvent>
    | ReceiveResumed of GatewayEventPayload<ResumedReceiveEvent>
    | ReceiveReconnect of GatewayEventPayload<ReconnectReceiveEvent>
    | ReceiveInvalidSession of GatewayEventPayload<InvalidSessionReceiveEvent>

let private send model (ev: GatewaySendEvent) =
    match ev with
    | GatewaySendEvent.IDENTIFY ev -> model, Cmd.ofMsg (Msg.SendIdentify ev)
    | GatewaySendEvent.RESUME ev -> model, Cmd.ofMsg (Msg.SendResume ev)
    | GatewaySendEvent.HEARTBEAT ev -> model, Cmd.ofMsg (Msg.SendHeartbeat ev)
    | GatewaySendEvent.REQUEST_GUILD_MEMBERS ev -> model, Cmd.ofMsg (Msg.SendRequestGuildMembers ev)
    | GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS ev -> model, Cmd.ofMsg (Msg.SendRequestSoundboardSounds ev)
    | GatewaySendEvent.UPDATE_VOICE_STATE ev -> model, Cmd.ofMsg (Msg.SendUpdateVoiceState ev)
    | GatewaySendEvent.UPDATE_PRESENCE ev -> model, Cmd.ofMsg (Msg.SendUpdatePresence ev)
    | GatewaySendEvent.UNKNOWN ev -> model, Cmd.none // TODO: Handle unknown events (?)

let private receive model (ev: GatewayReceiveEvent) =
    match ev with
    | GatewayReceiveEvent.HELLO ev -> model, Cmd.ofMsg (Msg.ReceiveHello ev)
    | GatewayReceiveEvent.HEARTBEAT ev -> model, Cmd.ofMsg (Msg.ReceiveHeartbeat ev)
    | GatewayReceiveEvent.HEARTBEAT_ACK ev -> model, Cmd.ofMsg (Msg.ReceiveHeartbeatAck ev)
    | GatewayReceiveEvent.READY ev -> model, Cmd.ofMsg (Msg.ReceiveReady ev)
    | GatewayReceiveEvent.RESUMED ev -> model, Cmd.ofMsg (Msg.ReceiveResumed ev)
    | GatewayReceiveEvent.RECONNECT ev -> model, Cmd.ofMsg (Msg.ReceiveReconnect ev)
    | GatewayReceiveEvent.INVALID_SESSION ev -> model, Cmd.ofMsg (Msg.ReceiveInvalidSession ev)
    | ev -> model, Cmd.none // TODO: Handle other events (pass gateway handler in as arg?)

let private enqueue model ev =
    { model with SendQueue = model.SendQueue @ [QueuedSendEvent.Pending ev] },
    Cmd.ofMsg Msg.StartProcessNext

let private startProcessNext model =
    match model.SendQueue |> List.tryHead with
    | Some (QueuedSendEvent.Pending ev) ->
        { model with SendQueue = model.SendQueue |> List.skip 1 |> List.append [QueuedSendEvent.Processing] },
        Cmd.ofEffect (fun dispatch -> (async {
            // TODO: Send event `ev` here (where does the dependency to send gateway events come from?)

            dispatch (Msg.EndProcessNext)
        } |> Async.StartImmediate)) // TODO: Test if this is blocking or not

    | _ -> model, Cmd.none

let private endProcessNext model =
    { model with SendQueue = model.SendQueue |> List.skip 1 },
    Cmd.ofMsg Msg.StartProcessNext

    // TODO: Should this somehow filter for `QueuedSendEvent.Processing` in some way to ensure it doesn't get stuck?

let private sendIdentify model (ev: GatewayEventPayload<IdentifySendEvent>) =
    model, Cmd.ofMsg (Msg.Enqueue (GatewaySendEvent.IDENTIFY ev))

let private sendResume model (ev: GatewayEventPayload<ResumeSendEvent>) =
    model, Cmd.ofMsg (Msg.Enqueue (GatewaySendEvent.RESUME ev))

let private sendHeartbeat model (ev: GatewayEventPayload<HeartbeatSendEvent>) =
    model, Cmd.ofMsg (Msg.Enqueue (GatewaySendEvent.HEARTBEAT ev))

let private sendRequestGuildMembers model (ev: GatewayEventPayload<RequestGuildMembersSendEvent>) =
    model, Cmd.ofMsg (Msg.Enqueue (GatewaySendEvent.REQUEST_GUILD_MEMBERS ev))

let private sendRequestSoundboardSounds model (ev: GatewayEventPayload<RequestSoundboardSoundsSendEvent>) =
    model, Cmd.ofMsg (Msg.Enqueue (GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS ev))

let private sendUpdateVoiceState model (ev: GatewayEventPayload<UpdateVoiceStateSendEvent>) =
    model, Cmd.ofMsg (Msg.Enqueue (GatewaySendEvent.UPDATE_VOICE_STATE ev))

let private sendUpdatePresence model (ev: GatewayEventPayload<UpdatePresenceSendEvent>) =
    model, Cmd.ofMsg (Msg.Enqueue (GatewaySendEvent.UPDATE_PRESENCE ev))

let private receiveHello model (ev: GatewayEventPayload<HelloReceiveEvent>) =
    match model.SessionId, model.SequenceId with
    | Some sessionId, Some sequenceId ->
        let payload = ResumeSendEvent.create(model.IdentifyEvent.Token, sessionId, sequenceId)

        { model with Interval = Some ev.Data.HeartbeatInterval },
        Cmd.ofMsg (Msg.SendResume (GatewayEventPayload.create(GatewayOpcode.RESUME, payload)))

    | _ ->
        { model with Interval = Some ev.Data.HeartbeatInterval },
        Cmd.ofMsg (Msg.SendIdentify (GatewayEventPayload.create(GatewayOpcode.IDENTIFY, model.IdentifyEvent)))

let private receiveHeartbeat model (ev: GatewayEventPayload<HeartbeatReceiveEvent>) =
    let payload = GatewayEventPayload.create(GatewayOpcode.HEARTBEAT, model.SequenceId)

    { model with Heartbeat = model.Interval |> Option.map DateTime.UtcNow.AddMilliseconds }, // TODO: Remove current time dependency
    Cmd.ofMsg (Msg.SendHeartbeat payload)

let private receiveHeartbeatAck (model: Model) (ev: GatewayEventPayload<HeartbeatAckReceiveEvent>) =
    { model with HeartbeatAcked = true }, Cmd.none

let private receiveReady (model: Model) (ev: GatewayEventPayload<ReadyReceiveEvent>) =
    { model with ResumeGatewayUrl = Some ev.Data.ResumeGatewayUrl; SessionId = Some ev.Data.SessionId }, Cmd.none

let private receiveResumed model (ev: GatewayEventPayload<ResumedReceiveEvent>) =
    model, Cmd.none // TODO: Mark state as active? Same with ready?

let private receiveReconnect model (ev: GatewayEventPayload<ReconnectReceiveEvent>) =
    match model.ResumeGatewayUrl, model.SessionId, model.SequenceId with
    | Some resumeGatewayUrl, Some sessionId, Some sequenceId -> model, Cmd.none // TODO: Resume
    | _ -> model, Cmd.none // TODO: Reconnect

let private receiveInvalidSession model (ev: GatewayEventPayload<InvalidSessionReceiveEvent>) =
    match ev.Data, model.ResumeGatewayUrl, model.SessionId, model.SequenceId with
    | true, Some resumeGatewayUrl, Some sessionId, Some sequenceId -> model, Cmd.none // TODO: Resume
    | _ -> model, Cmd.none // TODO: Reconnect

let update msg model =
    match msg with
    | Msg.Send ev -> send model ev
    | Msg.Receive ev -> receive model ev

    | Msg.Enqueue status -> enqueue model status
    | Msg.StartProcessNext -> startProcessNext model
    | Msg.EndProcessNext -> endProcessNext model

    | Msg.SendIdentify ev -> sendIdentify model ev
    | Msg.SendResume ev -> sendResume model ev
    | Msg.SendHeartbeat ev -> sendHeartbeat model ev
    | Msg.SendRequestGuildMembers ev -> sendRequestGuildMembers model ev
    | Msg.SendRequestSoundboardSounds ev -> sendRequestSoundboardSounds model ev
    | Msg.SendUpdateVoiceState ev -> sendUpdateVoiceState model ev
    | Msg.SendUpdatePresence ev -> sendUpdatePresence model ev

    | Msg.ReceiveHello ev -> receiveHello model ev
    | Msg.ReceiveHeartbeat ev -> receiveHeartbeat model ev
    | Msg.ReceiveHeartbeatAck ev -> receiveHeartbeatAck model ev
    | Msg.ReceiveReady ev -> receiveReady model ev
    | Msg.ReceiveResumed ev -> receiveResumed model ev
    | Msg.ReceiveReconnect ev -> receiveReconnect model ev
    | Msg.ReceiveInvalidSession ev -> receiveInvalidSession model ev

let view model dispatch =
    ()

let program identify =
    Program.mkProgram init update view
    // TODO: Add subscriptions to trigger send and receive messages with the websocket here `|> Program.withSubscription ...` (?)
    |> Program.runWith identify
