module FSharp.Discord.Gateway.Gateway

open Elmish
open FSharp.Discord.Gateway
open FSharp.Discord.Types
open System

type Model = {
    Heartbeat: Heartbeat.Model option
    Lifecycle: Lifecycle.Model option
}

module Model =
    let zero () =
        {
            Lifecycle = None
            Heartbeat = None
        }

type Msg =
    | Heartbeat of Heartbeat.Msg
    | Lifecycle of Lifecycle.Msg

    | Connect of IdentifySendEvent
    | Reconnect
    | Disconnect

    // TODO: Handle websocket action success/failure
    // TODO: Handle sending and receiving gateway events

let init () =
    Model.zero (), Cmd.none

let update msg model =
    match model.Lifecycle, model.Heartbeat, msg with
    // Connect
    | None, None, Msg.Connect identify ->
        let lifecycle, cmd = Lifecycle.init identify

        { model with Lifecycle = Some lifecycle }, Cmd.map Msg.Lifecycle cmd

    // Reconnect
    | _, _, Msg.Reconnect ->
        model, Cmd.none // TODO: Implement

    // Disconnect
    | _, _, Msg.Disconnect ->
        model, Cmd.none // TODO: Implement

    // Lifecycle hello start heartbeat
    | Some (Lifecycle.State.Starting _ as lifecycle), None, Msg.Lifecycle (Lifecycle.Msg.Hello data as msg) ->
        let interval = TimeSpan.FromMilliseconds data.HeartbeatInterval

        let heartbeat, hcmd = Heartbeat.init interval
        let updated, lcmd = Lifecycle.update msg lifecycle

        { model with
            Lifecycle = Some updated
            Heartbeat = Some heartbeat },
        Cmd.batch [
            Cmd.map Msg.Lifecycle lcmd
            Cmd.map Msg.Heartbeat hcmd
            Cmd.ofMsg (Msg.Heartbeat Heartbeat.Msg.Start)
        ]

    // Lifecycle requested send gateway event
    | Some lifecycle, _, Msg.Lifecycle ((Lifecycle.Msg.Send event) as msg) ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated }, Cmd.map Msg.Lifecycle cmd
        
        // TODO: Send event
        
    // Lifecycle requested restart
    | Some lifecycle, Some heartbeat, Msg.Lifecycle ((Lifecycle.Msg.Restart resumeData) as msg) ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated }, Cmd.map Msg.Lifecycle cmd
        
        // TODO: Handle resume or reconnect based on `resumeData`
        // TODO: Handle restarting new heartbeat
        
    // Lifecycle requested stop
    | Some lifecycle, _, Msg.Lifecycle (Lifecycle.Msg.Stop as msg) ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated },
        Cmd.batch [
            Cmd.map Msg.Lifecycle cmd
            Cmd.ofMsg Msg.Disconnect
        ]

    // Catch remaining lifecycle messages
    | Some lifecycle, _, Msg.Lifecycle msg ->
        let updated, cmd = Lifecycle.update msg lifecycle

        { model with Lifecycle = Some updated }, Cmd.map Msg.Lifecycle cmd
    
    // Heartbeat requested stop
    | _, Some (Heartbeat.State.Active _ as heartbeat), Msg.Heartbeat (Heartbeat.Msg.Stop as msg) ->
        let updated, cmd = Heartbeat.update msg heartbeat

        { model with Heartbeat = Some updated },
        Cmd.batch [
            Cmd.map Msg.Heartbeat cmd
            Cmd.ofMsg Msg.Disconnect
        ]

        // TODO: Handle heartbeat stop

    // Catch remaining heartbeat messages
    | _, Some heartbeat, Msg.Heartbeat msg ->
        let updated, cmd = Heartbeat.update msg heartbeat

        { model with Heartbeat = Some updated }, Cmd.map Msg.Heartbeat cmd

    // Catch invalid messages
    | lifecycle, heartbeat, msg ->
        eprintfn "Attempted to call msg %A in invate state %A %A" msg lifecycle heartbeat
        model, Cmd.ofMsg Msg.Disconnect

let subscribe model: Sub<Msg> =
    [] // TODO: Implement all necessary subscriptions
