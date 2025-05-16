module FSharp.Discord.Gateway.Heartbeat

open Elmish
open System

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

module State =
    let (|NotStarted|Active|Stopped|) model =
        match model with
        | { Stopped = true } -> Stopped
        | { Due = Some due; Interval = interval; Acked = acked } -> Active (due, interval, acked)
        | { Due = None; Interval = interval } -> NotStarted interval
        
    let (|Dead|_|) currentTime model =
        match model with
        | Active (due, _, _) when due < currentTime -> Some ()
        | _ -> None

    let (|Alive|_|) currentTime model =
        match model with
        | Active (due, interval, acked) when due >= currentTime -> Some (due, interval, acked)
        | _ -> None

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

    match model, msg with
    | State.NotStarted _, Msg.Start ->
        model, Cmd.ofMsg Msg.Beat

    | State.NotStarted interval, Msg.Beat
    | State.Alive currentTime (_, interval, _), Msg.Beat ->
        { model with Due = Some (currentTime.Add interval); Acked = false }, Cmd.none

    | State.Alive currentTime _, Msg.Ack ->
        { model with Acked = true }, Cmd.none
        
    | State.Dead currentTime _, Msg.Timeout ->
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
