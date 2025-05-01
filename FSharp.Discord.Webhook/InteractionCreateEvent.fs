namespace FSharp.Discord.Webhook

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type InteractionCreateEvent =
    | PING
    | APPLICATION_COMMAND              of ApplicationCommandData
    | MESSAGE_COMPONENT                of MessageComponentData
    | APPLICATION_COMMAND_AUTOCOMPLETE of ApplicationCommandData
    | MODAL_SUBMIT                     of ModalSubmitData

module InteractionCreateEvent =
    let decoder: Decoder<InteractionCreateEvent> =
        Interaction.decoder
        |> Decode.andThen (fun interaction ->
            match interaction.Type, interaction.Data with
            | InteractionType.PING, _ ->
                Decode.succeed InteractionCreateEvent.PING

            | InteractionType.APPLICATION_COMMAND, Some (InteractionData.APPLICATION_COMMAND d) ->
                Decode.succeed (InteractionCreateEvent.APPLICATION_COMMAND d)

            | InteractionType.MESSAGE_COMPONENT, Some (InteractionData.MESSAGE_COMPONENT d) ->
                Decode.succeed (InteractionCreateEvent.MESSAGE_COMPONENT d)

            | InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE, Some (InteractionData.APPLICATION_COMMAND d) ->
                Decode.succeed (InteractionCreateEvent.APPLICATION_COMMAND_AUTOCOMPLETE d)

            | InteractionType.MODAL_SUBMIT, Some (InteractionData.MODAL_SUBMIT d) ->
                Decode.succeed (InteractionCreateEvent.MODAL_SUBMIT d)

            | _ ->
                Decode.fail "Unexpected interaction data received"
        )
        
    let encoder: Encoder<InteractionCreateEvent> = raise (System.NotImplementedException()) // TODO: Include outer data in decoded result to preserve and support encoding then implement encoder
