module FSharp.Discord.Gateway.GatewayState

open Elmish
open FSharp.Discord.Types
open System

module Cmd =
    let fromAsync (operation: Async<'msg>): Cmd<'msg> =
        let delayedCmd (dispatch: 'msg -> unit): unit =
            let delayedDispatch = async {
                let! msg = operation
                dispatch msg
            }

            Async.StartImmediate delayedDispatch

        Cmd.ofEffect delayedCmd

type SendEventAction =
    | Start
    | Finish

type QueuedSendEvent =
    | Pending of GatewaySendEvent
    | Processing

type Model = {
    SendQueue: QueuedSendEvent list

    IdentifyEvent: IdentifySendEvent
    SequenceId: int option
    Interval: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
    ResumeGatewayUrl: string option
    SessionId: string option    
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
    | Send of SendEventAction
    | SendIdentify of GatewayEventPayload<IdentifySendEvent>
    | SendResume of GatewayEventPayload<ResumeSendEvent>

    | ReceiveHello of GatewayEventPayload<HelloReceiveEvent>
    | ReceiveHeartbeat of GatewayEventPayload<HeartbeatReceiveEvent>
    | ReceiveHeartbeatAck of GatewayEventPayload<HeartbeatAckReceiveEvent>
    | ReceiveReady of GatewayEventPayload<ReadyReceiveEvent>
    | ReceiveResumed of GatewayEventPayload<ResumedReceiveEvent>
    | ReceiveReconnect of GatewayEventPayload<ReconnectReceiveEvent>
    | ReceiveInvalidSession of GatewayEventPayload<InvalidSessionReceiveEvent>

let private send model status =
    match status with
    | SendEventAction.Start ->
        match model.SendQueue |> List.tryHead with
        | Some (QueuedSendEvent.Pending ev) ->
            { model with SendQueue = model.SendQueue |> List.skip 1 |> List.append [QueuedSendEvent.Processing] },
            Cmd.ofEffect (fun dispatch -> (async {
                // TODO: Send next event `ev` here (where does the websocket dependency come from?)
                dispatch (Msg.Send SendEventAction.Finish)
            } |> Async.StartImmediate)) // TODO: Is this blocking? Can it be made not blocking?

        | Some (QueuedSendEvent.Processing)
        | None ->
            model, Cmd.none

    | SendEventAction.Finish ->
        { model with SendQueue = model.SendQueue |> List.skip 1 },
        Cmd.ofMsg (Msg.Send SendEventAction.Start)

let private sendIdentify model (ev: GatewayEventPayload<IdentifySendEvent>) =
    { model with SendQueue = model.SendQueue @ [QueuedSendEvent.Pending (GatewaySendEvent.IDENTIFY ev)] },
    Cmd.ofMsg (Msg.Send SendEventAction.Start)

let private sendResume model (ev: GatewayEventPayload<ResumeSendEvent>) =
    { model with SendQueue = model.SendQueue @ [QueuedSendEvent.Pending (GatewaySendEvent.RESUME ev)] },
    Cmd.ofMsg (Msg.Send SendEventAction.Start)

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
    model, Cmd.none // TODO: Implement

let private receiveHeartbeatAck model (ev: GatewayEventPayload<HeartbeatAckReceiveEvent>) =
    model, Cmd.none // TODO: Implement

let private receiveReady model (ev: GatewayEventPayload<ReadyReceiveEvent>) =
    model, Cmd.none // TODO: Implement

let private receiveResumed model (ev: GatewayEventPayload<ResumedReceiveEvent>) =
    model, Cmd.none // TODO: Implement

let private receiveReconnect model (ev: GatewayEventPayload<ReconnectReceiveEvent>) =
    model, Cmd.none // TODO: Implement

let private receiveInvalidSession model (ev: GatewayEventPayload<InvalidSessionReceiveEvent>) =
    model, Cmd.none // TODO: Implement

let update msg model =
    match msg with
    | Msg.Send status -> send model status
    | Msg.SendIdentify ev -> sendIdentify model ev
    | Msg.SendResume ev -> sendResume model ev

    | Msg.ReceiveHello ev -> receiveHello model ev
    | Msg.ReceiveHeartbeat ev -> receiveHeartbeat model ev
    | Msg.ReceiveHeartbeatAck ev -> receiveHeartbeatAck model ev
    | Msg.ReceiveReady ev -> receiveReady model ev
    | Msg.ReceiveResumed ev -> receiveResumed model ev
    | Msg.ReceiveReconnect ev -> receiveReconnect model ev
    | Msg.ReceiveInvalidSession ev -> receiveInvalidSession model ev

let view model dispatch =
    ()

let program identifyEvent =
    Program.mkProgram init update view
    |> Program.runWith identifyEvent

// TODO: Figure out subscriptions for receiving from the actual websocket
