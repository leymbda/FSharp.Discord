module FSharp.Discord.Gateway.Lifecycle

open Elmish
open FSharp.Discord.Gateway
open FSharp.Discord.Types

type SessionState = {
    Sequence: int
    SessionId: string
}

type ResumeData = {
    Sequence: int
    SessionId: string
    ResumeGatewayUrl: string
}

type Model = {
    Identify: IdentifySendEvent
    ResumeGatewayUrl: string option
    SessionState: SessionState option
    Started: bool
    Ready: bool
    Stopped: bool
}

module Model =
    let zero identify =
        {
            Identify = identify
            ResumeGatewayUrl = None
            SessionState = None
            Started = false
            Ready = false
            Stopped = false
        }

module State =
    let (|NotStarted|Starting|Active|Stopped|) model =
        match model with
        | { Stopped = true } -> Stopped
        | { Ready = true; SessionState = Some state } -> Active state
        | { Started = true; SessionState = state } -> Starting state
        | { Started = false; Identify = identify; SessionState = state } -> NotStarted (identify, state)

type Msg =
    /// Handle hello lifecycle event.
    | Hello of HelloReceiveEvent

    /// Handle ready lifecycle event.
    | Ready of ReadyReceiveEvent * sequence: int
    
    /// Handle resumed lifecycle event
    | Resumed

    /// Handle reconnect lifecycle event
    | Reconnect

    /// Handle invalid session lifecycle event.
    | InvalidSession of resumable: bool

    /// Request a gateway event be sent.
    | Send of GatewaySendEvent

    /// Request a connection restart.
    | Restart of ResumeData option

    /// Stop the lifecycle.
    | Stop

let init identify =
    Model.zero identify, Cmd.none

let update msg model =
    match model, msg with
    | State.NotStarted (identify, None), Msg.Hello _ ->
        let event = GatewaySendEvent.IDENTIFY identify

        { model with Started = true }, Cmd.ofMsg (Msg.Send event)
        
    | State.NotStarted (identify, Some state), Msg.Hello _ ->
        let event = GatewaySendEvent.RESUME {
            Token = identify.Token
            SessionId = state.SessionId
            Sequence = state.Sequence
        }
        
        { model with Started = true }, Cmd.ofMsg (Msg.Send event)

    | State.Starting None, Msg.Ready (event, sequence) ->
        let state = {
            Sequence = sequence
            SessionId = event.SessionId
        }

        { model with
            ResumeGatewayUrl = Some event.ResumeGatewayUrl
            SessionState = Some state
            Ready = true },
        Cmd.none

    | State.Starting (Some _), Msg.Resumed ->
        { model with Ready = true }, Cmd.none

    | State.Active state, Msg.Reconnect ->
        let resumeData =
            model.ResumeGatewayUrl
            |> Option.map (fun resumeGatewayUrl -> {
                Sequence = state.Sequence
                SessionId = state.SessionId
                ResumeGatewayUrl = resumeGatewayUrl
            })

        { model with Stopped = true }, Cmd.ofMsg (Msg.Restart resumeData)

    | State.Starting (Some state), Msg.InvalidSession resumable
    | State.Active state, Msg.InvalidSession resumable ->
        let resumeData =
            model.ResumeGatewayUrl
            |> Option.filter (fun _ -> resumable)
            |> Option.map (fun resumeGatewayUrl -> {
                Sequence = state.Sequence
                SessionId = state.SessionId
                ResumeGatewayUrl = resumeGatewayUrl
            })

        { model with Stopped = true }, Cmd.ofMsg (Msg.Restart resumeData)

    | _, Msg.Send _ ->
        model, Cmd.none

    | _, Msg.Restart _ ->
        model, Cmd.none

    | _, Msg.Stop ->
        { model with Stopped = true }, Cmd.none

    | state, msg ->
        eprintfn "Attempted to call msg %A in invalid state %A" msg state
        model, Cmd.ofMsg Msg.Stop
