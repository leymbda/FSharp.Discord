namespace FSharp.Discord.Webhook

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type InteractionCreateEventMetadata = {
    Id: string
    ApplicationId: string
    Guild: PartialGuild option
    GuildId: string option
    Channel: PartialChannel option
    ChannelId: string option
    Author: InteractionAuthor
    Token: string
    Version: int
    Message: Message option
    AppPermissions: Permission list
    Locale: string option
    GuildLocale: string option
    Entitlements: Entitlement list
    AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, string>
    Context: InteractionContextType option
    AttachmentSizeLimit: int
}

type InteractionCreateEvent =
    | PING                             of                          InteractionCreateEventMetadata
    | APPLICATION_COMMAND              of ApplicationCommandData * InteractionCreateEventMetadata
    | MESSAGE_COMPONENT                of MessageComponentData   * InteractionCreateEventMetadata
    | APPLICATION_COMMAND_AUTOCOMPLETE of ApplicationCommandData * InteractionCreateEventMetadata
    | MODAL_SUBMIT                     of ModalSubmitData        * InteractionCreateEventMetadata

module InteractionCreateEvent =
    let decoder: Decoder<InteractionCreateEvent> =
        Interaction.decoder
        |> Decode.andThen (fun interaction ->
            let metadata = {
                Id = interaction.Id
                ApplicationId = interaction.ApplicationId
                Guild = interaction.Guild
                GuildId = interaction.GuildId
                Channel = interaction.Channel
                ChannelId = interaction.ChannelId
                Author = interaction.Author
                Token = interaction.Token
                Version = interaction.Version
                Message = interaction.Message
                AppPermissions = interaction.AppPermissions
                Locale = interaction.Locale
                GuildLocale = interaction.GuildLocale
                Entitlements = interaction.Entitlements
                AuthorizingIntegrationOwners = interaction.AuthorizingIntegrationOwners
                Context = interaction.Context
                AttachmentSizeLimit = interaction.AttachmentSizeLimit
            }

            match interaction.Type, interaction.Data with
            | InteractionType.PING, _ ->
                Decode.succeed (InteractionCreateEvent.PING metadata)

            | InteractionType.APPLICATION_COMMAND, Some (InteractionData.APPLICATION_COMMAND d) ->
                Decode.succeed (InteractionCreateEvent.APPLICATION_COMMAND (d, metadata))

            | InteractionType.MESSAGE_COMPONENT, Some (InteractionData.MESSAGE_COMPONENT d) ->
                Decode.succeed (InteractionCreateEvent.MESSAGE_COMPONENT (d, metadata))

            | InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE, Some (InteractionData.APPLICATION_COMMAND d) ->
                Decode.succeed (InteractionCreateEvent.APPLICATION_COMMAND_AUTOCOMPLETE (d, metadata))

            | InteractionType.MODAL_SUBMIT, Some (InteractionData.MODAL_SUBMIT d) ->
                Decode.succeed (InteractionCreateEvent.MODAL_SUBMIT (d, metadata))

            | _ ->
                Decode.fail "Unexpected interaction data received"
        )
        
    let encoder (v: InteractionCreateEvent) =
        let encodeInteraction type' data metadata =
            {
                Id = metadata.Id
                ApplicationId = metadata.ApplicationId
                Type = type'
                Data = data
                Guild = metadata.Guild
                GuildId = metadata.GuildId
                Channel = metadata.Channel
                ChannelId = metadata.ChannelId
                Author = metadata.Author
                Token = metadata.Token
                Version = metadata.Version
                Message = metadata.Message
                AppPermissions = metadata.AppPermissions
                Locale = metadata.Locale
                GuildLocale = metadata.GuildLocale
                Entitlements = metadata.Entitlements
                AuthorizingIntegrationOwners = metadata.AuthorizingIntegrationOwners
                Context = metadata.Context
                AttachmentSizeLimit = metadata.AttachmentSizeLimit
            }
            |> Interaction.encoder

        match v with
        | InteractionCreateEvent.PING metadata ->
            encodeInteraction InteractionType.PING None metadata

        | InteractionCreateEvent.APPLICATION_COMMAND (d, metadata) ->
            encodeInteraction InteractionType.APPLICATION_COMMAND (Some (InteractionData.APPLICATION_COMMAND d)) metadata

        | InteractionCreateEvent.MESSAGE_COMPONENT (d, metadata) ->
            encodeInteraction InteractionType.MESSAGE_COMPONENT (Some (InteractionData.MESSAGE_COMPONENT d)) metadata

        | InteractionCreateEvent.APPLICATION_COMMAND_AUTOCOMPLETE (d, metadata) ->
            encodeInteraction InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE (Some (InteractionData.APPLICATION_COMMAND d)) metadata

        | InteractionCreateEvent.MODAL_SUBMIT (d, metadata) ->
            encodeInteraction InteractionType.MODAL_SUBMIT (Some (InteractionData.MODAL_SUBMIT d)) metadata
