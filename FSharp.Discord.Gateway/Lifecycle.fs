module FSharp.Discord.Gateway.Lifecycle

open Elmish
open FSharp.Discord.Gateway
open FSharp.Discord.Types

type SessionState = {
    Sequence: int
    SessionId: string
}

type State =
    | NotStarted of SessionState option
    | Starting of SessionState option
    | Active of SessionState
    | Stopped

type Model = {
    SessionState: SessionState option
    Started: bool
    Ready: bool
    Stopped: bool
}

module Model =
    let zero () =
        {
            SessionState = None
            Started = false
            Ready = false
            Stopped = false
        }

    let getState model =
        match model with
        | { Stopped = true } -> State.Stopped
        | { Ready = true; SessionState = Some state } -> State.Active state
        | { Started = true; SessionState = state } -> State.Starting state
        | { Started = false; SessionState = state } -> State.NotStarted state

type Msg =
    /// Handle hello lifecycle event.
    | Hello of IdentifySendEvent

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
    | Restart of SessionState option

    /// Stop the lifecycle.
    | Stop

let init () =
    Model.zero (), Cmd.none

let update msg model =
    match Model.getState model, msg with
    | State.NotStarted None, Msg.Hello identify ->
        let event = GatewaySendEvent.IDENTIFY identify

        { model with Started = true }, Cmd.ofMsg (Msg.Send event)
        
    | State.NotStarted (Some state), Msg.Hello identify ->
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
            SessionState = Some state
            Ready = true },
        Cmd.none

    | State.Starting (Some _), Msg.Resumed ->
        { model with Ready = true }, Cmd.none

    | State.Active state, Msg.Reconnect ->
        { model with Stopped = true }, Cmd.ofMsg (Msg.Restart (Some state))

    | State.Starting (Some state), Msg.InvalidSession resumable
    | State.Active state, Msg.InvalidSession resumable ->
        let resume = if resumable then Some state else None
        { model with Stopped = true }, Cmd.ofMsg (Msg.Restart resume)

    | _, Msg.Send _ ->
        model, Cmd.none

    | _, Msg.Restart _ ->
        model, Cmd.none

    | _, Msg.Stop ->
        { model with Stopped = true }, Cmd.none

    | state, msg ->
        eprintfn "Attempted to call msg %A in invalid state %A" msg state
        model, Cmd.ofMsg Msg.Stop
