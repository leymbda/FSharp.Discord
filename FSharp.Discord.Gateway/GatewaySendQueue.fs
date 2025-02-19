module FSharp.Discord.Gateway.GatewaySendQueue

open Elmish

type QueuedSendEvent =
    | Pending of GatewaySendEvent
    | Processing

type Model = {
    Items: QueuedSendEvent list
}

let init () =
    { Items = [] }, Cmd.none

type Msg =
    | Enqueue of GatewaySendEvent
    | StartProcessNext
    | EndProcessNext

let private enqueue model ev =
    { model with Items = model.Items @ [QueuedSendEvent.Pending ev] },
    Cmd.ofMsg Msg.StartProcessNext

let private startProcessNext env model =
    match model.Items |> List.tryHead with
    | Some (QueuedSendEvent.Pending ev) ->
        { model with Items = model.Items |> List.skip 1 |> List.append [QueuedSendEvent.Processing] },
        Cmd.ofEffect (fun dispatch -> (async {
            // TODO: Send event `ev` here (where does the dependency to send gateway events come from?)

            dispatch (Msg.EndProcessNext)
        } |> Async.StartImmediate)) // TODO: Test if this is blocking or not

    | _ -> model, Cmd.none

let private endProcessNext model =
    { model with Items = model.Items |> List.skip 1 },
    Cmd.ofMsg Msg.StartProcessNext

    // TODO: Should this somehow filter for `QueuedSendEvent.Processing` in some way to ensure it doesn't get stuck?

let update env msg model =
    match msg with
    | Msg.Enqueue ev -> enqueue model ev
    | Msg.StartProcessNext -> startProcessNext env model
    | Msg.EndProcessNext -> endProcessNext model

let view model dispatch =
    ()

let program env =
    Program.mkProgram init (update env) view
    |> Program.run
