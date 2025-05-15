module FSharp.Discord.Gateway.GatewayHeartbeat

open Elmish
open System

type State =
    | NotStarted of interval: TimeSpan
    | Active of due: DateTime * interval: TimeSpan * acked: bool
    | Stopped of dead: bool

type Model = {
    Interval: TimeSpan
    Due: DateTime option
    Acked: bool
    Stopped: bool
}

module Model =
    let zero interval =
        {
            Interval = interval
            Due = None
            Acked = false
            Stopped = false
        }

    let getState currentTime model =
        match model with
        | { Due = Some due } when due < currentTime -> State.Stopped true
        | { Stopped = true } -> State.Stopped false
        | { Due = Some due; Interval = interval; Acked = acked } -> State.Active (due, interval, acked)
        | { Due = None; Interval = interval } -> State.NotStarted interval

type Msg =
    /// Start the heartbeat.
    | Start

    /// Reset timeout and request new ack. 
    | Beat

    /// Acknowledge the heartbeat.
    | Ack

    /// Triggered when heartbeat interval is reached without receiving an ack.
    | Timeout
    
    /// Stop the heartbeat.
    | Stop

let init interval =
    Model.zero interval, Cmd.none

let update msg model =
    let currentTime = DateTime.UtcNow // TODO: Extract elsewhere to remove side effect

    match Model.getState currentTime model, msg with
    | State.NotStarted _, Msg.Start ->
        model, Cmd.ofMsg Msg.Beat

    | State.NotStarted interval, Msg.Beat
    | State.Active (_, interval, _), Msg.Beat ->
        { model with Due = Some (currentTime.Add interval); Acked = false }, Cmd.none

    | State.Active _, Msg.Ack ->
        { model with Acked = true }, Cmd.none
        
    | State.Stopped true, Msg.Timeout ->
        model, Cmd.ofMsg Msg.Stop

    | _, Msg.Stop ->
        { model with Stopped = true }, Cmd.none

    | state, msg ->
        eprintfn "Attempted to call msg %A in invalid state %A" msg state
        model, Cmd.ofMsg Msg.Stop

let subscribe model: Sub<Msg> =
    let currentTime = DateTime.UtcNow // TODO: Extract elsewhere to remove side effect

    let timeout (due: DateTime) =
        let timespan = due.Subtract currentTime
        [["timeout"; string due.Ticks], Sub.delay timespan (fun dispatch -> dispatch Msg.Timeout)]

    model.Due
    |> Option.filter (fun _ -> not model.Stopped)
    |> Option.map timeout
    |> Option.defaultValue []
