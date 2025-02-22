namespace rec FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Thoth.Json.Net

module Encode =
    /// Encode a map to an object with a value encoder.
    let mapv (encoder: Encoder<'a>) value =
        Map.map (fun _ v -> encoder v) value
        |> Encode.dict

    /// Encode a map to an object with a key mapper and value encoder.
    let mapkv (mapper: 'a -> string) (encoder: Encoder<'b>) value =
        value
        |> Map.toSeq
        |> Seq.map (fun (k, v) -> mapper k, encoder v)
        |> Map.ofSeq
        |> Encode.dict

module ErrorResponse =
    module Property =
        let [<Literal>] Code = "code"
        let [<Literal>] Message = "message"
        let [<Literal>] Errors = "errors"

    let decoder path v =
        Decode.object (fun get -> {
            Code = get.Required.Field Property.Code Decode.Enum.int<JsonErrorCode>
            Message = get.Required.Field Property.Message Decode.string
            Errors = get.Required.Field Property.Errors (Decode.dict Decode.string)
        }) path v

    let encoder (v: ErrorResponse) =
        Encode.object [
            Property.Code, Encode.Enum.int v.Code
            Property.Message, Encode.string v.Message
            Property.Errors, Encode.mapv Encode.string v.Errors
        ]

module Interaction =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Data = "data"
        let [<Literal>] Guild = "guild"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Channel = "channel"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Member = "member"
        let [<Literal>] User = "user"
        let [<Literal>] Token = "token"
        let [<Literal>] Version = "version"
        let [<Literal>] Message = "message"
        let [<Literal>] AppPermissions = "app_permissions"
        let [<Literal>] Locale = "locale"
        let [<Literal>] GuildLocale = "guild_locale"
        let [<Literal>] Entitlements = "entitlements"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] Context = "context"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            ApplicationId = get.Required.Field Property.ApplicationId Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<InteractionType>
            Data = get.Optional.Field Property.Data InteractionData.decoder
            Guild = get.Optional.Field Property.Guild PartialGuild.decoder
            GuildId = get.Optional.Field Property.GuildId Decode.string
            Channel = get.Optional.Field Property.Channel PartialChannel.decoder
            ChannelId = get.Optional.Field Property.ChannelId Decode.string
            Member = get.Optional.Field Property.Member GuildMember.decoder
            User = get.Optional.Field Property.User User.decoder
            Token = get.Required.Field Property.Token Decode.string
            Version = get.Required.Field Property.Version Decode.int
            Message = get.Optional.Field Property.Message Message.decoder
            AppPermissions = get.Required.Field Property.AppPermissions Decode.string
            Locale = get.Optional.Field Property.Locale Decode.string
            GuildLocale = get.Optional.Field Property.GuildLocale Decode.string
            Entitlements = get.Required.Field Property.Entitlements (Decode.list Entitlement.decoder)
            AuthorizingIntegrationOwners = get.Required.Field Property.AuthorizingIntegrationOwners (Decode.dict ApplicationIntegrationTypeConfiguration.decoder) // TODO: Ensure key is a valid option
            Context = get.Optional.Field Property.Context Decode.Enum.int<InteractionContextType>
        }) path v

    let encoder (v: Interaction) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.ApplicationId, Encode.string v.ApplicationId
            Property.Type, Encode.Enum.int v.Type
            Property.Data, Encode.option InteractionData.encoder v.Data
            Property.Guild, Encode.option Guild.encoder v.Guild
            Property.GuildId, Encode.option Encode.string v.GuildId
            Property.Channel, Encode.option PartialChannel.encoder v.Channel
            Property.ChannelId, Encode.option Encode.string v.ChannelId
            Property.Member, Encode.option GuildMember.encoder v.Member
            Property.User, Encode.option User.encoder v.User
            Property.Token, Encode.string v.Token
            Property.Version, Encode.int v.Version
            Property.Message, Encode.option Message.encoder v.Message
            Property.AppPermissions, Encode.string v.AppPermissions
            Property.Locale, Encode.option Encode.string v.Locale
            Property.GuildLocale, Encode.option Encode.string v.GuildLocale
            Property.Entitlements, (List.map Entitlement.encoder >> Encode.list) v.Entitlements
            Property.AuthorizingIntegrationOwners, Encode.mapv ApplicationIntegrationTypeConfiguration.encoder v.AuthorizingIntegrationOwners
            Property.Context, Encode.option Encode.Enum.int v.Context
        ]

module ApplicationCommandData =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Type = "type"
        let [<Literal>] Resolved = "resolved"
        let [<Literal>] Options = "options"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] TargetId = "target_id"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Name = get.Required.Field Property.Name Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<ApplicationCommandType>
            Resolved = get.Optional.Field Property.Resolved ResolvedData.decoder
            Options = get.Optional.Field Property.Options (Decode.list ApplicationCommandOption.decoder)
            GuildId = get.Optional.Field Property.GuildId Decode.string
            TargetId = get.Optional.Field Property.TargetId Decode.string
        }) path v

    let encoder (v: ApplicationCommandData) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Name, Encode.string v.Name
            Property.Type, Encode.Enum.int v.Type
            Property.Resolved, Encode.option ResolvedData.encoder v.Resolved
            Property.Options, Encode.option (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            Property.GuildId, Encode.option Encode.string v.GuildId
            Property.TargetId, Encode.option Encode.string v.TargetId
        ]

module MessageComponentData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] ComponentType = "component_type"
        let [<Literal>] Values = "values"
        let [<Literal>] Resolved = "resolved"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get.Required.Field Property.CustomId Decode.string
            ComponentType = get.Required.Field Property.ComponentType Decode.Enum.int<ComponentType>
            Values = get.Optional.Field Property.Values (Decode.list SelectMenuOption.decoder)
            Resolved = get.Optional.Field Property.Resolved ResolvedData.decoder
        }) path v

    let encoder (v: MessageComponentData) =
        Encode.object [
            Property.CustomId, Encode.string v.CustomId
            Property.ComponentType, Encode.Enum.int v.ComponentType
            Property.Values, Encode.option (List.map SelectMenuOption.encoder >> Encode.list) v.Values
            Property.Resolved, Encode.option ResolvedData.encoder v.Resolved
        ]

module ModalSubmitData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Components = "components"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get.Required.Field Property.CustomId Decode.string
            Components = get.Required.Field Property.Components (Decode.list Component.decoder)
        }) path v

    let encoder (v: ModalSubmitData) =
        Encode.object [
            Property.CustomId, Encode.string v.CustomId
            Property.Components, (List.map Component.encoder >> Encode.list) v.Components
        ]

module InteractionData =
    let decoder path v =
        Decode.oneOf [
            Decode.map InteractionData.APPLICATION_COMMAND ApplicationCommandData.decoder
            Decode.map InteractionData.MESSAGE_COMPONENT MessageComponentData.decoder
            Decode.map InteractionData.MODAL_SUBMIT ModalSubmitData.decoder
        ] path v

    let encoder (v: InteractionData) =
        match v with
        | InteractionData.APPLICATION_COMMAND data -> ApplicationCommandData.encoder data
        | InteractionData.MESSAGE_COMPONENT data -> MessageComponentData.encoder data
        | InteractionData.MODAL_SUBMIT data -> ModalSubmitData.encoder data

module ResolvedData =
    module Property =
        let [<Literal>] Users = "users"
        let [<Literal>] Members = "members"
        let [<Literal>] Roles = "roles"
        let [<Literal>] Channels = "channels"
        let [<Literal>] Messages = "messages"
        let [<Literal>] Attachments = "attachments"

    let decoder path v =
        Decode.object (fun get -> {
            Users = get.Optional.Field Property.Users (Decode.dict User.decoder)
            Members = get.Optional.Field Property.Members (Decode.dict PartialGuildMember.decoder)
            Roles = get.Optional.Field Property.Roles (Decode.dict Role.decoder)
            Channels = get.Optional.Field Property.Channels (Decode.dict PartialChannel.decoder)
            Messages = get.Optional.Field Property.Messages (Decode.dict PartialMessage.decoder)
            Attachments = get.Optional.Field Property.Attachments (Decode.dict Attachment.decoder)
        }) path v

    let encoder (v: ResolvedData) =
        Encode.object [
            Property.Users, Encode.option (Encode.mapv User.encoder) v.Users
            Property.Members, Encode.option (Encode.mapv PartialGuildMember.encoder) v.Members
            Property.Roles, Encode.option (Encode.mapv Role.encoder) v.Roles
            Property.Channels, Encode.option (Encode.mapv PartialChannel.encoder) v.Channels
            Property.Messages, Encode.option (Encode.mapv PartialMessage.encoder) v.Messages
            Property.Attachments, Encode.option (Encode.mapv Attachment.encoder) v.Attachments
        ]

module ApplicationCommandInteractionDataOption =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Type = "type"
        let [<Literal>] Value = "value"
        let [<Literal>] Options = "options"
        let [<Literal>] Focused = "focused"

    let decoder path v =
        Decode.object (fun get -> {
            Name = get.Required.Field Property.Name Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<ApplicationCommandOptionType>
            Value = get.Optional.Field Property.Value ApplicationCommandInteractionDataOptionValue.decoder
            Options = get.Optional.Field Property.Options (Decode.list ApplicationCommandInteractionDataOption.decoder)
            Focused = get.Optional.Field Property.Focused Decode.bool
        }) path v

    let encoder (v: ApplicationCommandInteractionDataOption) =
        Encode.object [
            Property.Name, Encode.string v.Name
            Property.Type, Encode.Enum.int v.Type
            Property.Value, Encode.option ApplicationCommandInteractionDataOptionValue.encoder v.Value
            Property.Options, Encode.option (List.map ApplicationCommandInteractionDataOption.encoder >> Encode.list) v.Options
            Property.Focused, Encode.option Encode.bool v.Focused
        ]

module ApplicationCommandInteractionDataOptionValue =
    let decoder path v =
        Decode.oneOf [
            Decode.map ApplicationCommandInteractionDataOptionValue.STRING Decode.string
            Decode.map ApplicationCommandInteractionDataOptionValue.INT Decode.int
            Decode.map ApplicationCommandInteractionDataOptionValue.DOUBLE Decode.float
            Decode.map ApplicationCommandInteractionDataOptionValue.BOOL Decode.bool
        ] path v

        // TODO: Test if int will fail decoding if a float is provided
        // TODO: Test to ensure that Decode.oneOf works down the list

    let encoder (v: ApplicationCommandInteractionDataOptionValue) =
        match v with
        | ApplicationCommandInteractionDataOptionValue.STRING data -> Encode.string data
        | ApplicationCommandInteractionDataOptionValue.INT data -> Encode.int data
        | ApplicationCommandInteractionDataOptionValue.DOUBLE data -> Encode.float data
        | ApplicationCommandInteractionDataOptionValue.BOOL data -> Encode.bool data

module MessageInteraction =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] Name = "name"
        let [<Literal>] User = "user"
        let [<Literal>] Member = "member"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<InteractionType>
            Name = get.Required.Field Property.Name Decode.string
            User = get.Required.Field Property.User User.decoder
            Member = get.Optional.Field Property.Member PartialGuildMember.decoder
        }) path v

    let encoder (v: MessageInteraction) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Type, Encode.Enum.int v.Type
            Property.Name, Encode.string v.Name
            Property.User, User.encoder v.User
            Property.Member, Encode.option PartialGuildMember.encoder v.Member
        ]

module InteractionResponse =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Data = "data"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get.Required.Field Property.Type Decode.Enum.int<InteractionCallbackType>
            Data = get.Optional.Field Property.Data InteractionCallbackData.decoder
        }) path v

    let encoder (v: InteractionResponse) =
        Encode.object [
            Property.Type, Encode.Enum.int v.Type
            Property.Data, Encode.option InteractionCallbackData.encoder v.Data
        ]

module MessageInteractionCallbackData =
    module Property =
        let [<Literal>] Tts = "tts"
        let [<Literal>] Content = "content"
        let [<Literal>] Embeds = "embeds"
        let [<Literal>] AllowedMentions = "allowed_mentions"
        let [<Literal>] Flags = "flags"
        let [<Literal>] Components = "components"
        let [<Literal>] Attachments = "attachments"
        let [<Literal>] Poll = "poll"

    let decoder path v =
        Decode.object (fun get -> {
            Tts = get.Optional.Field Property.Tts Decode.bool
            Content = get.Optional.Field Property.Content Decode.string
            Embeds = get.Optional.Field Property.Embeds (Decode.list Embed.decoder)
            AllowedMentions = get.Optional.Field Property.AllowedMentions AllowedMentions.decoder
            Flags = get.Optional.Field Property.Flags Decode.int
            Components = get.Optional.Field Property.Components (Decode.list Component.decoder)
            Attachments = get.Optional.Field Property.Attachments (Decode.list PartialAttachment.decoder)
            Poll = get.Optional.Field Property.Poll Poll.decoder
        }) path v

    let encoder (v: MessageInteractionCallbackData) =
        Encode.object [
            Property.Tts, Encode.option Encode.bool v.Tts
            Property.Content, Encode.option Encode.string v.Content
            Property.Embeds, Encode.option (List.map Embed.encoder >> Encode.list) v.Embeds
            Property.AllowedMentions, Encode.option AllowedMentions.encoder v.AllowedMentions
            Property.Flags, Encode.option Encode.int v.Flags
            Property.Components, Encode.option (List.map Component.encoder >> Encode.list) v.Components
            Property.Attachments, Encode.option (List.map PartialAttachment.encoder >> Encode.list) v.Attachments
            Property.Poll, Encode.option Poll.encoder v.Poll
        ]

module AutocompleteInteractionCallbackData =
    module Property =
        let [<Literal>] Choices = "choices"

    let decoder path v =
        Decode.object (fun get -> {
            Choices = get.Required.Field Property.Choices (Decode.list ApplicationCommandOptionChoice.decoder)
        }) path v

    let encoder (v: AutocompleteInteractionCallbackData) =
        Encode.object [
            Property.Choices, (List.map ApplicationCommandOptionChoice.encoder >> Encode.list) v.Choices
        ]

module ModalInteractionCallbackData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Title = "title"
        let [<Literal>] Components = "components"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get.Required.Field Property.CustomId Decode.string
            Title = get.Required.Field Property.Title Decode.string
            Components = get.Required.Field Property.Components (Decode.list Component.decoder)
        }) path v

    let encoder (v: ModalInteractionCallbackData) =
        Encode.object [
            Property.CustomId, Encode.string v.CustomId
            Property.Title, Encode.string v.Title
            Property.Components, (List.map Component.encoder >> Encode.list) v.Components
        ]

module InteractionCallbackData =
    let decoder path v =
        Decode.oneOf [
            Decode.map InteractionCallbackData.MESSAGE MessageInteractionCallbackData.decoder
            Decode.map InteractionCallbackData.AUTOCOMPLETE AutocompleteInteractionCallbackData.decoder
            Decode.map InteractionCallbackData.MODAL ModalInteractionCallbackData.decoder
        ] path v

    let encoder (v: InteractionCallbackData) =
        match v with
        | InteractionCallbackData.MESSAGE data -> MessageInteractionCallbackData.encoder data
        | InteractionCallbackData.AUTOCOMPLETE data -> AutocompleteInteractionCallbackData.encoder data
        | InteractionCallbackData.MODAL data -> ModalInteractionCallbackData.encoder data

module InteractionCallbackResponse =
    module Property =
        let [<Literal>] Interaction = "interaction"
        let [<Literal>] Resource = "resource"

    let decoder path v =
        Decode.object (fun get -> {
            Interaction = get.Required.Field Property.Interaction InteractionCallback.decoder
            Resource = get.Optional.Field Property.Resource InteractionCallbackResource.decoder
        }) path v

    let encoder (v: InteractionCallbackResponse) =
        Encode.object [
            Property.Interaction, InteractionCallback.encoder v.Interaction
            Property.Resource, Encode.option InteractionCallbackResource.encoder v.Resource
        ]

module InteractionCallback =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] ActivityInstanceId = "activity_instance_id"
        let [<Literal>] ResponseMessageId = "response_message_id"
        let [<Literal>] ResponseMessageLoading = "response_message_loading"
        let [<Literal>] ResponseMessageEphemeral = "response_message_ephemeral"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<InteractionType>
            ActivityInstanceId = get.Optional.Field Property.ActivityInstanceId Decode.string
            ResponseMessageId = get.Optional.Field Property.ResponseMessageId Decode.string
            ResponseMessageLoading = get.Optional.Field Property.ResponseMessageLoading Decode.bool
            ResponseMessageEphemeral = get.Optional.Field Property.ResponseMessageEphemeral Decode.bool
        }) path v

    let encoder (v: InteractionCallback) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Type, Encode.Enum.int v.Type
            Property.ActivityInstanceId, Encode.option Encode.string v.ActivityInstanceId
            Property.ResponseMessageId, Encode.option Encode.string v.ResponseMessageId
            Property.ResponseMessageLoading, Encode.option Encode.bool v.ResponseMessageLoading
            Property.ResponseMessageEphemeral, Encode.option Encode.bool v.ResponseMessageEphemeral
        ]

module InteractionCallbackResource =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] ActivityInstance = "activity_instance"
        let [<Literal>] Message = "message"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get.Required.Field Property.Type Decode.Enum.int<InteractionCallbackType>
            ActivityInstance = get.Optional.Field Property.ActivityInstance InteractionCallbackActivityInstance.decoder
            Message = get.Optional.Field Property.Message Message.decoder
        }) path v

    let encoder (v: InteractionCallbackResource) =
        Encode.object [
            Property.Type, Encode.Enum.int v.Type
            Property.ActivityInstance, Encode.option InteractionCallbackActivityInstance.encoder v.ActivityInstance
            Property.Message, Encode.option Message.encoder v.Message
        ]

module InteractionCallbackActivityInstance =
    module Property =
        let [<Literal>] Id = "id"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
        }) path v

    let encoder (v: InteractionCallbackActivityInstance) =
        Encode.object [
            Property.Id, Encode.string v.Id
        ]

module ApplicationCommand =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Name = "name"
        let [<Literal>] NameLocalizations = "name_localizations"
        let [<Literal>] Description = "description"
        let [<Literal>] DescriptionLocalizations = "description_localizations"
        let [<Literal>] Options = "options"
        let [<Literal>] DefaultMemberPermissions = "default_member_permissions"
        let [<Literal>] Nsfw = "nsfw"
        let [<Literal>] IntegrationTypes = "integration_types"
        let [<Literal>] Contexts = "contexts"
        let [<Literal>] Version = "version"
        let [<Literal>] Handler = "handler"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Type = get.Optional.Field Property.Type Decode.Enum.int<ApplicationCommandType> |> Option.defaultValue ApplicationCommandType.CHAT_INPUT
            ApplicationId = get.Required.Field Property.ApplicationId Decode.string
            GuildId = get.Optional.Field Property.GuildId Decode.string
            Name = get.Required.Field Property.Name Decode.string
            NameLocalizations = get.Optional.Field Property.NameLocalizations (Decode.dict Decode.string)
            Description = get.Required.Field Property.Description Decode.string
            DescriptionLocalizations = get.Optional.Field Property.DescriptionLocalizations (Decode.dict Decode.string)
            Options = get.Optional.Field Property.Options (Decode.list ApplicationCommandOption.decoder)
            DefaultMemberPermissions = get.Optional.Field Property.DefaultMemberPermissions Decode.string
            Nsfw = get.Optional.Field Property.Nsfw Decode.bool |> Option.defaultValue false
            IntegrationTypes = get.Optional.Field Property.IntegrationTypes (Decode.list Decode.Enum.int<ApplicationIntegrationType>) |> Option.defaultValue [ApplicationIntegrationType.GUILD_INSTALL; ApplicationIntegrationType.USER_INSTALL]
            Contexts = get.Optional.Field Property.Contexts (Decode.list Decode.Enum.int<InteractionContextType>)
            Version = get.Required.Field Property.Version Decode.string
            Handler = get.Optional.Field Property.Handler Decode.Enum.int<ApplicationCommandHandlerType>
        }) path v

    let encoder (v: ApplicationCommand) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Type, Encode.Enum.int v.Type
            Property.ApplicationId, Encode.string v.ApplicationId
            Property.GuildId, Encode.option Encode.string v.GuildId
            Property.Name, Encode.string v.Name
            Property.NameLocalizations, Encode.option (Encode.mapv Encode.string) v.NameLocalizations
            Property.Description, Encode.string v.Description
            Property.DescriptionLocalizations, Encode.option (Encode.mapv Encode.string) v.DescriptionLocalizations
            Property.Options, Encode.option (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            Property.DefaultMemberPermissions, Encode.option Encode.string v.DefaultMemberPermissions
            Property.Nsfw, Encode.bool v.Nsfw
            Property.IntegrationTypes, (List.map Encode.Enum.int >> Encode.list) v.IntegrationTypes
            Property.Contexts, Encode.option (List.map Encode.Enum.int >> Encode.list) v.Contexts
            Property.Version, Encode.string v.Version
            Property.Handler, Encode.option Encode.Enum.int v.Handler
        ]

module ApplicationCommandOption =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Name = "name"
        let [<Literal>] NameLocalizations = "name_localizations"
        let [<Literal>] Description = "description"
        let [<Literal>] DescriptionLocalizations = "description_localizations"
        let [<Literal>] Required = "required"
        let [<Literal>] Choices = "choices"
        let [<Literal>] Options = "options"
        let [<Literal>] ChannelTypes = "channel_types"
        let [<Literal>] MinValue = "min_value"
        let [<Literal>] MaxValue = "max_value"
        let [<Literal>] MinLength = "min_length"
        let [<Literal>] MaxLength = "max_length"
        let [<Literal>] Autocomplete = "autocomplete"

    let decoder path v: Result<ApplicationCommandOption, DecoderError> =
        Decode.object (fun get -> {
            Type = get.Required.Field Property.Type Decode.Enum.int<ApplicationCommandOptionType>
            Name = get.Required.Field Property.Name Decode.string
            NameLocalizations = get.Optional.Field Property.NameLocalizations (Decode.dict Decode.string)
            Description = get.Required.Field Property.Description Decode.string
            DescriptionLocalizations = get.Optional.Field Property.DescriptionLocalizations (Decode.dict Decode.string)
            Required = get.Optional.Field Property.Required Decode.bool |> Option.defaultValue false
            Choices = get.Optional.Field Property.Choices (Decode.list ApplicationCommandOptionChoice.decoder)
            Options = get.Optional.Field Property.Options (Decode.list ApplicationCommandOption.decoder)
            ChannelTypes = get.Optional.Field Property.ChannelTypes (Decode.list Decode.Enum.int<ChannelType>)
            MinValue = get.Optional.Field Property.MinValue ApplicationCommandOptionMinValue.decoder
            MaxValue = get.Optional.Field Property.MaxValue ApplicationCommandOptionMaxValue.decoder
            MinLength = get.Optional.Field Property.MinLength Decode.int
            MaxLength = get.Optional.Field Property.MaxLength Decode.int
            Autocomplete = get.Optional.Field Property.Autocomplete Decode.bool
        }) path v

    let encoder (v: ApplicationCommandOption) =
        Encode.object [
            Property.Type, Encode.Enum.int v.Type
            Property.Name, Encode.string v.Name
            Property.NameLocalizations, Encode.option (Encode.mapv Encode.string) v.NameLocalizations
            Property.Description, Encode.string v.Description
            Property.DescriptionLocalizations, Encode.option (Encode.mapv Encode.string) v.DescriptionLocalizations
            Property.Required, Encode.bool v.Required
            Property.Choices, Encode.option (List.map ApplicationCommandOptionChoice.encoder >> Encode.list) v.Choices
            Property.Options, Encode.option (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            Property.ChannelTypes, Encode.option (List.map Encode.Enum.int >> Encode.list) v.ChannelTypes
            Property.MinValue, Encode.option ApplicationCommandOptionMinValue.encoder v.MinValue
            Property.MaxValue, Encode.option ApplicationCommandOptionMaxValue.encoder v.MaxValue
            Property.MinLength, Encode.option Encode.int v.MinLength
            Property.MaxLength, Encode.option Encode.int v.MaxLength
            Property.Autocomplete, Encode.option Encode.bool v.Autocomplete
        ]

module ApplicationCommandOptionMinValue =
    let decoder path v =
        Decode.oneOf [
            Decode.map ApplicationCommandOptionMinValue.INT Decode.int
            Decode.map ApplicationCommandOptionMinValue.DOUBLE Decode.float
        ] path v

    let encoder (v: ApplicationCommandOptionMinValue) =
        match v with
        | ApplicationCommandOptionMinValue.INT data -> Encode.int data
        | ApplicationCommandOptionMinValue.DOUBLE data -> Encode.float data

    // TODO: Ensure min 0, max 6000 (create single DU with this requirement)

module ApplicationCommandOptionMaxValue =
    let decoder path v =
        Decode.oneOf [
            Decode.map ApplicationCommandOptionMaxValue.INT Decode.int
            Decode.map ApplicationCommandOptionMaxValue.DOUBLE Decode.float
        ] path v

    let encoder (v: ApplicationCommandOptionMaxValue) =
        match v with
        | ApplicationCommandOptionMaxValue.INT data -> Encode.int data
        | ApplicationCommandOptionMaxValue.DOUBLE data -> Encode.float data

    // TODO: Ensure min 1, max 6000 (create single DU with this requirement)

module ApplicationCommandOptionChoice =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] NameLocalizations = "name_localizations"
        let [<Literal>] Value = "value"

    let decoder path v =
        Decode.object (fun get -> {
            Name = get.Required.Field Property.Name Decode.string
            NameLocalizations = get.Optional.Field Property.NameLocalizations (Decode.dict Decode.string)
            Value = get.Required.Field Property.Value ApplicationCommandOptionChoiceValue.decoder
        }) path v

    let encoder (v: ApplicationCommandOptionChoice) =
        Encode.object [
            Property.Name, Encode.string v.Name
            Property.NameLocalizations, Encode.option (Encode.mapv Encode.string) v.NameLocalizations
            Property.Value, ApplicationCommandOptionChoiceValue.encoder v.Value
        ]

module ApplicationCommandOptionChoiceValue =
    let decoder path v =
        Decode.oneOf [
            Decode.map ApplicationCommandOptionChoiceValue.STRING Decode.string
            Decode.map ApplicationCommandOptionChoiceValue.INT Decode.int
            Decode.map ApplicationCommandOptionChoiceValue.DOUBLE Decode.float
        ] path v

    let encoder (v: ApplicationCommandOptionChoiceValue) =
        match v with
        | ApplicationCommandOptionChoiceValue.STRING data -> Encode.string data
        | ApplicationCommandOptionChoiceValue.INT data -> Encode.int data
        | ApplicationCommandOptionChoiceValue.DOUBLE data -> Encode.float data

module GuildApplicationCommandPermissions =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Permissions = "permissions"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            ApplicationId = get.Required.Field Property.ApplicationId Decode.string
            GuildId = get.Required.Field Property.GuildId Decode.string
            Permissions = get.Required.Field Property.Permissions (Decode.list ApplicationCommandPermission.decoder)
        }) path v

    let encoder (v: GuildApplicationCommandPermissions) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.ApplicationId, Encode.string v.ApplicationId
            Property.GuildId, Encode.string v.GuildId
            Property.Permissions, (List.map ApplicationCommandPermission.encoder >> Encode.list) v.Permissions
        ]

module ApplicationCommandPermission =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] Permission = "permission"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<ApplicationCommandPermissionType>
            Permission = get.Required.Field Property.Permission Decode.bool
        }) path v

    let encoder (v: ApplicationCommandPermission) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Type, Encode.Enum.int v.Type
            Property.Permission, Encode.bool v.Permission
        ]

module ActionRow =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Components = "components"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get.Required.Field Property.Type Decode.Enum.int<ComponentType>
            Components = get.Required.Field Property.Components (Decode.list Component.decoder)
        }) path v

    let encoder (v: ActionRow) =
        Encode.object [
            Property.Type, Encode.Enum.int v.Type
            Property.Components, (List.map Component.encoder >> Encode.list) v.Components
        ]

module Button =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Style = "style"
        let [<Literal>] Label = "label"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Url = "url"
        let [<Literal>] Disabled = "disabled"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get.Required.Field Property.Type Decode.Enum.int<ComponentType>
            Style = get.Required.Field Property.Style Decode.Enum.int<ButtonStyle>
            Label = get.Required.Field Property.Label Decode.string
            Emoji = get.Optional.Field Property.Emoji Emoji.decoder
            CustomId = get.Optional.Field Property.CustomId Decode.string
            Url = get.Optional.Field Property.Url Decode.string
            Disabled = get.Optional.Field Property.Disabled Decode.bool |> Option.defaultValue false
        }) path v

    let encoder (v: Button) =
        Encode.object [
            Property.Type, Encode.Enum.int v.Type
            Property.Style, Encode.Enum.int v.Style
            Property.Label, Encode.string v.Label
            Property.Emoji, Encode.option Emoji.encoder v.Emoji
            Property.CustomId, Encode.option Encode.string v.CustomId
            Property.Url, Encode.option Encode.string v.Url
            Property.Disabled, Encode.bool v.Disabled
        ]

module SelectMenu =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Options = "options"
        let [<Literal>] ChannelTypes = "channel_types"
        let [<Literal>] Placeholder = "placeholder"
        let [<Literal>] DefaultValues = "default_values"
        let [<Literal>] MinValues = "min_values"
        let [<Literal>] MaxValues = "max_values"
        let [<Literal>] Disabled = "disabled"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get.Required.Field Property.Type Decode.Enum.int<ComponentType>
            CustomId = get.Required.Field Property.CustomId Decode.string
            Options = get.Required.Field Property.Options (Decode.list SelectMenuOption.decoder)
            ChannelTypes = get.Optional.Field Property.ChannelTypes (Decode.list Decode.Enum.int<ChannelType>)
            Placeholder = get.Optional.Field Property.Placeholder Decode.string
            DefaultValues = get.Optional.Field Property.DefaultValues (Decode.list SelectMenuDefaultValue.decoder)
            MinValues = get.Optional.Field Property.MinValues Decode.int |> Option.defaultValue 1
            MaxValues = get.Optional.Field Property.MaxValues Decode.int |> Option.defaultValue 1
            Disabled = get.Optional.Field Property.Disabled Decode.bool |> Option.defaultValue false
        }) path v

    let encoder (v: SelectMenu) =
        Encode.object [
            Property.Type, Encode.Enum.int v.Type
            Property.CustomId, Encode.string v.CustomId
            Property.Options, Encode.option (List.map SelectMenuOption.encoder >> Encode.list) v.Options
            Property.ChannelTypes, Encode.option (List.map Encode.Enum.int >> Encode.list) v.ChannelTypes
            Property.Placeholder, Encode.option Encode.string v.Placeholder
            Property.DefaultValues, Encode.option (List.map SelectMenuDefaultValue.encoder >> Encode.list) v.DefaultValues
            Property.MinValues, Encode.int v.MinValues
            Property.MaxValues, Encode.int v.MaxValues
            Property.Disabled, Encode.bool v.Disabled
        ]

module SelectMenuOption =
    module Property =
        let [<Literal>] Label = "label"
        let [<Literal>] Value = "value"
        let [<Literal>] Description = "description"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] Default = "default"

    let decoder path v =
        Decode.object (fun get -> {
            Label = get.Required.Field Property.Label Decode.string
            Value = get.Required.Field Property.Value Decode.string
            Description = get.Optional.Field Property.Description Decode.string
            Emoji = get.Optional.Field Property.Emoji Emoji.decoder
            Default = get.Optional.Field Property.Default Decode.bool
        }) path v

    let encoder (v: SelectMenuOption) =
        Encode.object [
            Property.Label, Encode.string v.Label
            Property.Value, Encode.string v.Value
            Property.Description, Encode.option Encode.string v.Description
            Property.Emoji, Encode.option Emoji.encoder v.Emoji
            Property.Default, Encode.option Encode.bool v.Default
        ]

module SelectMenuDefaultValue =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Type = get.Required.Field Property.Type SelectMenuDefaultValueType.decoder
        }) path v

    let encoder (v: SelectMenuDefaultValue) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Type, SelectMenuDefaultValueType.encoder v.Type
        ]

module TextInput =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Style = "style"
        let [<Literal>] Label = "label"
        let [<Literal>] MinLength = "min_length"
        let [<Literal>] MaxLength = "max_length"
        let [<Literal>] Required = "required"
        let [<Literal>] Value = "value"
        let [<Literal>] Placeholder = "placeholder"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get.Required.Field Property.Type Decode.Enum.int<ComponentType>
            CustomId = get.Required.Field Property.CustomId Decode.string
            Style = get.Required.Field Property.Style Decode.Enum.int<TextInputStyle>
            Label = get.Required.Field Property.Label Decode.string
            MinLength = get.Optional.Field Property.MinLength Decode.int
            MaxLength = get.Optional.Field Property.MaxLength Decode.int
            Required = get.Optional.Field Property.Required Decode.bool |> Option.defaultValue true
            Value = get.Optional.Field Property.Value Decode.string
            Placeholder = get.Optional.Field Property.Placeholder Decode.string
        }) path v

    let encoder (v: TextInput) =
        Encode.object [
            Property.Type, Encode.Enum.int v.Type
            Property.CustomId, Encode.string v.CustomId
            Property.Style, Encode.Enum.int v.Style
            Property.Label, Encode.string v.Label
            Property.MinLength, Encode.option Encode.int v.MinLength
            Property.MaxLength, Encode.option Encode.int v.MaxLength
            Property.Required, Encode.bool v.Required
            Property.Value, Encode.option Encode.string v.Value
            Property.Placeholder, Encode.option Encode.string v.Placeholder
        ]

module Component =
    let decoder path v =
        Decode.oneOf [
            Decode.map Component.ACTION_ROW ActionRow.decoder
            Decode.map Component.BUTTON Button.decoder
            Decode.map Component.SELECT_MENU SelectMenu.decoder
            Decode.map Component.TEXT_INPUT TextInput.decoder
        ] path v

    let encoder (v: Component) =
        match v with
        | Component.ACTION_ROW data -> ActionRow.encoder data
        | Component.BUTTON data -> Button.encoder data
        | Component.SELECT_MENU data -> SelectMenu.encoder data
        | Component.TEXT_INPUT data -> TextInput.encoder data

// TODO: Can piped functions be mapped instead (e.g. Option.defaultValue)
