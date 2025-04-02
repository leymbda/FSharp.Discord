[<AutoOpen>]
module FSharp.Discord.Commands.Interactions

open FSharp.Discord.Types

let (|Ping|_|) (interaction: Interaction) =
    match interaction.Type with
    | InteractionType.PING -> Some ()
    | _ -> None

let (|ApplicationCommand|_|) name (interaction: Interaction) =
    match interaction.Type, interaction.Data with
    | InteractionType.APPLICATION_COMMAND, Some (InteractionData.APPLICATION_COMMAND data) ->
        if data.Name = name then Some data
        else None
    | _ -> None

let (|MessageComponent|_|) (comparer: string -> bool) (interaction: Interaction) =
    match interaction.Type, interaction.Data with
    | InteractionType.MESSAGE_COMPONENT, Some (InteractionData.MESSAGE_COMPONENT data) ->
        if comparer data.CustomId then Some data
        else None
    | _ -> None

let (|ApplicationCommandAutocomplete|_|) name (interaction: Interaction) =
    match interaction.Type, interaction.Data with
    | InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE, Some (InteractionData.APPLICATION_COMMAND data) ->
        if data.Name = name then Some data
        else None
    | _ -> None

let (|ModalSubmit|_|) (comparer: string -> bool) (interaction: Interaction) =
    match interaction.Type, interaction.Data with
    | InteractionType.MODAL_SUBMIT, Some (InteractionData.MODAL_SUBMIT data) ->
        if comparer data.CustomId then Some data
        else None
    | _ -> None

// TODO: Change interaction to have author property of either guild member or used for the different contexts since one has to exist
// TODO: Add extra patterns here as necessary
