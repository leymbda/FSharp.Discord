namespace rec FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open System
open Thoth.Json.Net

// TODO: Rewrite serializers using `Get` module helpers. Consider making a CE first
// TODO: Can piped functions be mapped instead (e.g. Option.defaultValue)

module Decode =
    let mapkv (keyMapper: string -> 'a option) (valueDecoder: Decoder<'b>) path v =
        let decoded = Decode.dict valueDecoder path v

        match decoded with
        | Error err -> Error err
        | Ok d ->
            d
            |> Map.toSeq
            |> Seq.fold
                (fun acc cur -> acc |> Result.bind (fun acc ->
                    match keyMapper (fst cur) with
                    | None -> Error (path, BadField("an invalid key", v))
                    | Some k -> Ok (acc |> Seq.append (seq { k, snd cur }))
                ))
                (Ok [])
            |> Result.map (Map.ofSeq)

module Encode =
    /// Append an encoding that is required.
    let required key decoder v list =
        list @ [key, decoder v]

    /// Append an encoding that is optional.
    let optional key decoder v list =
        match v with
        | Some s -> list @ [key, decoder s]
        | None -> list

    /// Append an encoding that is nullable.
    let nullable key decoder v list =
        list @ [key, Encode.option decoder v]

    /// Append an encoding that is optional and nullable.
    let optinull key decoder v list =
        match v with
        | Some s -> list @ [key, Encode.option decoder s]
        | None -> list

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
        
module Get =
    /// Get a required decoded value.
    let required key decoder (get: Decode.IGetters) =
        get.Required.Field key decoder

    /// Get an optional decoded value.
    let optional key decoder (get: Decode.IGetters) =
        get.Optional.Field key decoder

    /// Get a nullable decoded value.
    let nullable key decoder (get: Decode.IGetters) =
        get.Required.Field key (Decode.option decoder)

    /// Get an optional and nullable decoded value.
    let optinull key decoder (get: Decode.IGetters) =
        get.Optional.Raw (Decode.field key (Decode.option decoder))
        
module UnixTimestamp =
    let decoder path v =
        Decode.map (DateTimeOffset.FromUnixTimeMilliseconds >> _.DateTime) Decode.int64 path v

    let encoder (v: DateTime) =
        DateTimeOffset v |> _.ToUnixTimeMilliseconds() |> Encode.int64

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
            Guild = get.Optional.Field Property.Guild Guild.Partial.decoder
            GuildId = get.Optional.Field Property.GuildId Decode.string
            Channel = get.Optional.Field Property.Channel Channel.Partial.decoder
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
            AuthorizingIntegrationOwners = get.Required.Field Property.AuthorizingIntegrationOwners (Decode.mapkv ApplicationIntegrationType.fromString ApplicationIntegrationTypeConfiguration.decoder)
            Context = get.Optional.Field Property.Context Decode.Enum.int<InteractionContextType>
        }) path v

    let encoder (v: Interaction) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.ApplicationId, Encode.string v.ApplicationId
            Property.Type, Encode.Enum.int v.Type
            Property.Data, Encode.option InteractionData.encoder v.Data
            Property.Guild, Encode.option Guild.Partial.encoder v.Guild
            Property.GuildId, Encode.option Encode.string v.GuildId
            Property.Channel, Encode.option Channel.Partial.encoder v.Channel
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
            Property.AuthorizingIntegrationOwners, Encode.mapkv ApplicationIntegrationType.toString ApplicationIntegrationTypeConfiguration.encoder v.AuthorizingIntegrationOwners
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
            Options = get.Optional.Field Property.Options (Decode.list ApplicationCommandInteractionDataOption.decoder)
            GuildId = get.Optional.Field Property.GuildId Decode.string
            TargetId = get.Optional.Field Property.TargetId Decode.string
        }) path v

    let encoder (v: ApplicationCommandData) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Name, Encode.string v.Name
            Property.Type, Encode.Enum.int v.Type
            Property.Resolved, Encode.option ResolvedData.encoder v.Resolved
            Property.Options, Encode.option (List.map ApplicationCommandInteractionDataOption.encoder >> Encode.list) v.Options
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
            Members = get.Optional.Field Property.Members (Decode.dict GuildMember.Partial.decoder)
            Roles = get.Optional.Field Property.Roles (Decode.dict Role.decoder)
            Channels = get.Optional.Field Property.Channels (Decode.dict Channel.Partial.decoder)
            Messages = get.Optional.Field Property.Messages (Decode.dict Message.Partial.decoder)
            Attachments = get.Optional.Field Property.Attachments (Decode.dict Attachment.decoder)
        }) path v

    let encoder (v: ResolvedData) =
        Encode.object [
            Property.Users, Encode.option (Encode.mapv User.encoder) v.Users
            Property.Members, Encode.option (Encode.mapv GuildMember.Partial.encoder) v.Members
            Property.Roles, Encode.option (Encode.mapv Role.encoder) v.Roles
            Property.Channels, Encode.option (Encode.mapv Channel.Partial.encoder) v.Channels
            Property.Messages, Encode.option (Encode.mapv Message.Partial.encoder) v.Messages
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
            Member = get.Optional.Field Property.Member GuildMember.Partial.decoder
        }) path v

    let encoder (v: MessageInteraction) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Type, Encode.Enum.int v.Type
            Property.Name, Encode.string v.Name
            Property.User, User.encoder v.User
            Property.Member, Encode.option GuildMember.Partial.encoder v.Member
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
            Attachments = get.Optional.Field Property.Attachments (Decode.list Attachment.Partial.decoder)
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
            Property.Attachments, Encode.option (List.map Attachment.Partial.encoder >> Encode.list) v.Attachments
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
            Options = get.Optional.Field Property.Options (Decode.list SelectMenuOption.decoder)
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

module SessionStartLimit =
    module Property =
        let [<Literal>] Total = "total"
        let [<Literal>] Remaining = "remaining"
        let [<Literal>] ResetAfter = "reset_after"
        let [<Literal>] MaxConcurrency = "max_concurrency"

    let decoder path v =
        Decode.object (fun get -> {
            Total = get.Required.Field Property.Total Decode.int
            Remaining = get.Required.Field Property.Remaining Decode.int
            ResetAfter = get.Required.Field Property.ResetAfter Decode.int
            MaxConcurrency = get.Required.Field Property.MaxConcurrency Decode.int
        }) path v

    let encoder (v: SessionStartLimit) =
        Encode.object [
            Property.Total, Encode.int v.Total
            Property.Remaining, Encode.int v.Remaining
            Property.ResetAfter, Encode.int v.ResetAfter
            Property.MaxConcurrency, Encode.int v.MaxConcurrency
        ]

module IdentifyConnectionProperties =
    module Property =
        let [<Literal>] OperatingSystem = "os"
        let [<Literal>] Browser = "browser"
        let [<Literal>] Device = "device"

    let decoder path v =
        Decode.object (fun get -> {
            OperatingSystem = get.Required.Field Property.OperatingSystem Decode.string
            Browser = get.Required.Field Property.Browser Decode.string
            Device = get.Required.Field Property.Device Decode.string
        }) path v

    let encoder (v: IdentifyConnectionProperties) =
        Encode.object [
            Property.OperatingSystem, Encode.string v.OperatingSystem
            Property.Browser, Encode.string v.Browser
            Property.Device, Encode.string v.Device
        ]

module ClientStatus =
    module Property =
        let [<Literal>] Desktop = "desktop"
        let [<Literal>] Mobile = "mobile"
        let [<Literal>] Web = "web"

    let decoder path v =
        Decode.object (fun get -> {
            Desktop = get.Optional.Field Property.Desktop ClientDeviceStatus.decoder
            Mobile = get.Optional.Field Property.Mobile ClientDeviceStatus.decoder
            Web = get.Optional.Field Property.Web ClientDeviceStatus.decoder
        }) path v

    let encoder (v: ClientStatus) =
        Encode.object [
            Property.Desktop, Encode.option ClientDeviceStatus.encoder v.Desktop
            Property.Mobile, Encode.option ClientDeviceStatus.encoder v.Mobile
            Property.Web, Encode.option ClientDeviceStatus.encoder v.Web
        ]

module Activity =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Type = "type"
        let [<Literal>] Url = "url"
        let [<Literal>] CreatedAt = "created_at"
        let [<Literal>] Timestamps = "timestamps"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Details = "details"
        let [<Literal>] State = "state"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] Party = "party"
        let [<Literal>] Assets = "assets"
        let [<Literal>] Secrets = "secrets"
        let [<Literal>] Instance = "instance"
        let [<Literal>] Flags = "flags"
        let [<Literal>] Buttons = "buttons"

    let decoder path v =
        Decode.object (fun get -> {
            Name = get.Required.Field Property.Name Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<ActivityType>
            Url = get.Optional.Field Property.Url Decode.string
            CreatedAt = get.Optional.Field Property.CreatedAt UnixTimestamp.decoder
            Timestamps = get.Optional.Field Property.Timestamps ActivityTimestamps.decoder
            ApplicationId = get.Optional.Field Property.ApplicationId Decode.string
            Details = get.Optional.Field Property.Details Decode.string
            State = get.Optional.Field Property.State Decode.string
            Emoji = get.Optional.Field Property.Emoji ActivityEmoji.decoder
            Party = get.Optional.Field Property.Party ActivityParty.decoder
            Assets = get.Optional.Field Property.Assets ActivityAssets.decoder
            Secrets = get.Optional.Field Property.Secrets ActivitySecrets.decoder
            Instance = get.Optional.Field Property.Instance Decode.bool
            Flags = get.Optional.Field Property.Flags Decode.int
            Buttons = get.Optional.Field Property.Buttons (Decode.list ActivityButton.decoder)
        }) path v

    let encoder (v: Activity) =
        Encode.object [
            Property.Name, Encode.string v.Name
            Property.Type, Encode.Enum.int v.Type
            Property.Url, Encode.option Encode.string v.Url
            Property.CreatedAt, Encode.option UnixTimestamp.encoder v.CreatedAt
            Property.Timestamps, Encode.option ActivityTimestamps.encoder v.Timestamps
            Property.ApplicationId, Encode.option Encode.string v.ApplicationId
            Property.Details, Encode.option Encode.string v.Details
            Property.State, Encode.option Encode.string v.State
            Property.Emoji, Encode.option ActivityEmoji.encoder v.Emoji
            Property.Party, Encode.option ActivityParty.encoder v.Party
            Property.Assets, Encode.option ActivityAssets.encoder v.Assets
            Property.Secrets, Encode.option ActivitySecrets.encoder v.Secrets
            Property.Instance, Encode.option Encode.bool v.Instance
            Property.Flags, Encode.option Encode.int v.Flags
            Property.Buttons, Encode.option (List.map ActivityButton.encoder >> Encode.list) v.Buttons
        ]

module ActivityTimestamps =
    module Property =
        let [<Literal>] Start = "start"
        let [<Literal>] End = "end"

    let decoder path v =
        Decode.object (fun get -> {
            Start = get.Optional.Field Property.Start UnixTimestamp.decoder
            End = get.Optional.Field Property.End UnixTimestamp.decoder
        }) path v

    let encoder (v: ActivityTimestamps) =
        Encode.object [
            Property.Start, Encode.option UnixTimestamp.encoder v.Start
            Property.End, Encode.option UnixTimestamp.encoder v.End
        ]

module ActivityEmoji =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Id = "id"
        let [<Literal>] Animated = "animated"

    let decoder path v =
        Decode.object (fun get -> {
            Name = get.Required.Field Property.Name Decode.string
            Id = get.Optional.Field Property.Id Decode.string
            Animated = get.Optional.Field Property.Animated Decode.bool
        }) path v

    let encoder (v: ActivityEmoji) =
        Encode.object [
            Property.Name, Encode.string v.Name
            Property.Id, Encode.option Encode.string v.Id
            Property.Animated, Encode.option Encode.bool v.Animated
        ]

module ActivityParty =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Size = "size"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Optional.Field Property.Id Decode.string
            Size = get.Optional.Field Property.Size ActivityPartySize.decoder
        }) path v

    let encoder (v: ActivityParty) =
        Encode.object [
            Property.Id, Encode.option Encode.string v.Id
            Property.Size, Encode.option ActivityPartySize.encoder v.Size
        ]

module ActivityPartySize =
    let decoder path v =
        match Decode.list Decode.int path v with
        | Error err -> Error err
        | Ok [current; maximum] -> Ok { Current = current; Maximum = maximum }
        | Ok _ -> Error (path, BadType("array of 2 integers", v))

    let encoder (v: ActivityPartySize) =
        (List.map Encode.int >> Encode.list) [v.Current; v.Maximum]

module ActivityAssets =
    module Property =
        let [<Literal>] LargeImage = "large_image"
        let [<Literal>] LargeText = "large_text"
        let [<Literal>] SmallImage = "small_image"
        let [<Literal>] SmallText = "small_text"

    let decoder path v =
        Decode.object (fun get -> {
            LargeImage = get.Optional.Field Property.LargeImage Decode.string
            LargeText = get.Optional.Field Property.LargeText Decode.string
            SmallImage = get.Optional.Field Property.SmallImage Decode.string
            SmallText = get.Optional.Field Property.SmallText Decode.string
        }) path v

    let encoder (v: ActivityAssets) =
        Encode.object [
            Property.LargeImage, Encode.option Encode.string v.LargeImage
            Property.LargeText, Encode.option Encode.string v.LargeText
            Property.SmallImage, Encode.option Encode.string v.SmallImage
            Property.SmallText, Encode.option Encode.string v.SmallText
        ]

module ActivitySecrets =
    module Property =
        let [<Literal>] Join = "join"
        let [<Literal>] Spectate = "spectate"
        let [<Literal>] Match = "match"

    let decoder path v =
        Decode.object (fun get -> {
            Join = get.Optional.Field Property.Join Decode.string
            Spectate = get.Optional.Field Property.Spectate Decode.string
            Match = get.Optional.Field Property.Match Decode.string
        }) path v

    let encoder (v: ActivitySecrets) =
        Encode.object [
            Property.Join, Encode.option Encode.string v.Join
            Property.Spectate, Encode.option Encode.string v.Spectate
            Property.Match, Encode.option Encode.string v.Match
        ]

module ActivityButton =
    module Property =
        let [<Literal>] Label = "label"
        let [<Literal>] Url = "url"

    let decoder path v =
        Decode.object (fun get -> {
            Label = get.Required.Field Property.Label Decode.string
            Url = get.Required.Field Property.Url Decode.string
        }) path v

    let encoder (v: ActivityButton) =
        Encode.object [
            Property.Label, Encode.string v.Label
            Property.Url, Encode.string v.Url
        ]

module WebhookEventPayload =
    module Property =
        let [<Literal>] Version = "version"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Event = "event"

    let decoder eventDataDecoder path v =
        Decode.object (fun get -> {
            Version = get |> Get.required Property.Version Decode.int
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<WebhookPayloadType>
            Event = get |> Get.optional Property.Event (WebhookEventBody.decoder eventDataDecoder)
        }) path v

    let encoder eventDataEncoder (v: WebhookEventPayload<'a>) =
        Encode.object ([]
            |> Encode.required Property.Version Encode.int v.Version
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.required Property.Type Encode.Enum.int<WebhookPayloadType> v.Type
            |> Encode.optional Property.Event (WebhookEventBody.encoder eventDataEncoder) v.Event
        )

module WebhookEventBody =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Timestamp = "timestamp"
        let [<Literal>] Data = "data"

    let decoder dataDecoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type WebhookEventType.decoder
            Timestamp = get |> Get.required Property.Timestamp Decode.datetimeUtc
            Data = get |> Get.optional Property.Data dataDecoder
        }) path v

    let encoder dataEncoder (v: WebhookEventBody<'a>) =
        Encode.object ([]
            |> Encode.required Property.Type WebhookEventType.encoder v.Type
            |> Encode.required Property.Timestamp Encode.datetime v.Timestamp
            |> Encode.optional Property.Data dataEncoder v.Data
        )

module ApplicationAuthorizedEvent =
    module Property =
        let [<Literal>] IntegrationType = "integration_type"
        let [<Literal>] User = "user"
        let [<Literal>] Scopes = "scopes"
        let [<Literal>] Guild = "guild"

    let decoder path v =
        Decode.object (fun get -> {
            IntegrationType = get |> Get.optional Property.IntegrationType Decode.Enum.int<ApplicationIntegrationType>
            User = get |> Get.required Property.User User.decoder
            Scopes = get |> Get.required Property.Scopes (Decode.list OAuthScope.decoder)
            Guild = get |> Get.optional Property.Guild Guild.decoder
        }) path v

    let encoder (v: ApplicationAuthorizedEvent) =
        Encode.object ([]
            |> Encode.optional Property.IntegrationType Encode.Enum.int<ApplicationIntegrationType> v.IntegrationType
            |> Encode.required Property.User User.encoder v.User
            |> Encode.required Property.Scopes (List.map OAuthScope.encoder >> Encode.list) v.Scopes
            |> Encode.optional Property.Guild Guild.encoder v.Guild
        )

module EntitlementCreateEvent =
    let decoder path v: Result<EntitlementCreateEvent, DecoderError> =
        Entitlement.decoder path v

    let encoder (v: EntitlementCreateEvent): JsonValue =
        Entitlement.encoder v

module Application =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Icon = "icon"
        let [<Literal>] Description = "description"
        let [<Literal>] RpcOrigins = "rpc_origins"
        let [<Literal>] BotPublic = "bot_public"
        let [<Literal>] BotRequireCodeGrant = "bot_require_code_grant"
        let [<Literal>] Bot = "bot"
        let [<Literal>] TermsOfServiceUrl = "terms_of_service_url"
        let [<Literal>] PrivacyPolicyUrl = "privacy_policy_url"
        let [<Literal>] Owner = "owner"
        let [<Literal>] VerifyKey = "verify_key"
        let [<Literal>] Team = "team"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Guild = "guild"
        let [<Literal>] PrimarySkuId = "primary_sku_id"
        let [<Literal>] Slug = "slug"
        let [<Literal>] CoverImage = "cover_image"
        let [<Literal>] Flags = "flags"
        let [<Literal>] ApproximateGuildCount = "approximate_guild_count"
        let [<Literal>] ApproximateUserInstallCount = "approximate_user_install_count"
        let [<Literal>] RedirectUris = "redirect_uris"
        let [<Literal>] InteractionsEndpointUrl = "interactions_endpoint_url"
        let [<Literal>] RoleConnectionsVerificationUrl = "role_connections_verification_url"
        let [<Literal>] EventWebhooksUrl = "event_webhooks_url"
        let [<Literal>] EventWebhooksStatus = "event_webhooks_status"
        let [<Literal>] EventWebhooksTypes = "event_webhooks_types"
        let [<Literal>] Tags = "tags"
        let [<Literal>] InstallParams = "install_params"
        let [<Literal>] IntegrationTypesConfig = "integration_types_config"
        let [<Literal>] CustomInstallUrl = "custom_install_url"

    let decoder path v: Result<Application, DecoderError> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Icon = get |> Get.nullable Property.Icon Decode.string
            Description = get |> Get.required Property.Description Decode.string
            RpcOrigins = get |> Get.optional Property.RpcOrigins (Decode.list Decode.string)
            BotPublic = get |> Get.required Property.BotPublic Decode.bool
            BotRequireCodeGrant = get |> Get.required Property.BotRequireCodeGrant Decode.bool
            Bot = get |> Get.optional Property.Bot User.Partial.decoder
            TermsOfServiceUrl = get |> Get.optional Property.TermsOfServiceUrl Decode.string
            PrivacyPolicyUrl = get |> Get.optional Property.PrivacyPolicyUrl Decode.string
            Owner = get |> Get.optional Property.Owner User.Partial.decoder
            VerifyKey = get |> Get.required Property.VerifyKey Decode.string
            Team = get |> Get.nullable Property.Team Team.decoder
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Guild = get |> Get.optional Property.Guild Guild.Partial.decoder
            PrimarySkuId = get |> Get.optional Property.PrimarySkuId Decode.string
            Slug = get |> Get.optional Property.Slug Decode.string
            CoverImage = get |> Get.optional Property.CoverImage Decode.string
            Flags = get |> Get.optional Property.Flags Decode.int
            ApproximateGuildCount = get |> Get.optional Property.ApproximateGuildCount Decode.int
            ApproximateUserInstallCount = get |> Get.optional Property.ApproximateUserInstallCount Decode.int
            RedirectUris = get |> Get.optional Property.RedirectUris (Decode.list Decode.string)
            InteractionsEndpointUrl = get |> Get.optinull Property.InteractionsEndpointUrl Decode.string
            RoleConnectionsVerificationUrl = get |> Get.optinull Property.RoleConnectionsVerificationUrl Decode.string
            EventWebhooksUrl = get |> Get.optinull Property.EventWebhooksUrl Decode.string
            EventWebhooksStatus = get |> Get.required Property.EventWebhooksStatus Decode.Enum.int<WebhookEventStatus>
            EventWebhooksTypes = get |> Get.optional Property.EventWebhooksTypes (Decode.list WebhookEventType.decoder)
            Tags = get |> Get.optional Property.Tags (Decode.list Decode.string)
            InstallParams = get |> Get.optional Property.InstallParams InstallParams.decoder
            IntegrationTypesConfig = get |> Get.optional Property.IntegrationTypesConfig (Decode.mapkv ApplicationIntegrationType.fromString ApplicationIntegrationTypeConfiguration.decoder)
            CustomInstallUrl  = get |> Get.optional Property.CustomInstallUrl Decode.string
        }) path v

    let encoder (v: Application) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.nullable Property.Icon Encode.string v.Icon
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optional Property.RpcOrigins (List.map Encode.string >> Encode.list) v.RpcOrigins
            |> Encode.required Property.BotPublic Encode.bool v.BotPublic
            |> Encode.required Property.BotRequireCodeGrant Encode.bool v.BotRequireCodeGrant
            |> Encode.optional Property.Bot User.Partial.encoder v.Bot
            |> Encode.optional Property.TermsOfServiceUrl Encode.string v.TermsOfServiceUrl
            |> Encode.optional Property.PrivacyPolicyUrl Encode.string v.TermsOfServiceUrl
            |> Encode.optional Property.Owner User.Partial.encoder v.Owner
            |> Encode.required Property.VerifyKey Encode.string v.VerifyKey
            |> Encode.nullable Property.Team Team.encoder v.Team
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Guild Guild.Partial.encoder v.Guild
            |> Encode.optional Property.PrimarySkuId Encode.string v.PrimarySkuId
            |> Encode.optional Property.Slug Encode.string v.Slug
            |> Encode.optional Property.CoverImage Encode.string v.CoverImage
            |> Encode.optional Property.Flags Encode.int v.Flags
            |> Encode.optional Property.ApproximateGuildCount Encode.int v.ApproximateGuildCount
            |> Encode.optional Property.ApproximateUserInstallCount Encode.int v.ApproximateUserInstallCount
            |> Encode.optional Property.RedirectUris (List.map Encode.string >> Encode.list) v.RedirectUris
            |> Encode.optinull Property.InteractionsEndpointUrl Encode.string v.InteractionsEndpointUrl
            |> Encode.optinull Property.RoleConnectionsVerificationUrl Encode.string v.RoleConnectionsVerificationUrl
            |> Encode.optinull Property.EventWebhooksUrl Encode.string v.EventWebhooksUrl
            |> Encode.required Property.EventWebhooksStatus Encode.Enum.int v.EventWebhooksStatus
            |> Encode.optional Property.EventWebhooksTypes (List.map WebhookEventType.encoder >> Encode.list) v.EventWebhooksTypes
            |> Encode.optional Property.Tags (List.map Encode.string >> Encode.list) v.Tags
            |> Encode.optional Property.InstallParams InstallParams.encoder v.InstallParams
            |> Encode.optional Property.IntegrationTypesConfig (Encode.mapkv ApplicationIntegrationType.toString ApplicationIntegrationTypeConfiguration.encoder) v.IntegrationTypesConfig
            |> Encode.optional Property.CustomInstallUrl Encode.string v.CustomInstallUrl
        )

    module Partial =
        let decoder path v: Result<PartialApplication, DecoderError> =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Name = get |> Get.optional Property.Name Decode.string
                Icon = get |> Get.optinull Property.Icon Decode.string
                Description = get |> Get.optional Property.Description Decode.string
                RpcOrigins = get |> Get.optional Property.RpcOrigins (Decode.list Decode.string)
                BotPublic = get |> Get.optional Property.BotPublic Decode.bool
                BotRequireCodeGrant = get |> Get.optional Property.BotRequireCodeGrant Decode.bool
                Bot = get |> Get.optional Property.Bot User.Partial.decoder
                TermsOfServiceUrl = get |> Get.optional Property.TermsOfServiceUrl Decode.string
                PrivacyPolicyUrl = get |> Get.optional Property.PrivacyPolicyUrl Decode.string
                Owner = get |> Get.optional Property.Owner User.Partial.decoder
                VerifyKey = get |> Get.optional Property.VerifyKey Decode.string
                Team = get |> Get.optinull Property.Team Team.decoder
                GuildId = get |> Get.optional Property.GuildId Decode.string
                Guild = get |> Get.optional Property.Guild Guild.Partial.decoder
                PrimarySkuId = get |> Get.optional Property.PrimarySkuId Decode.string
                Slug = get |> Get.optional Property.Slug Decode.string
                CoverImage = get |> Get.optional Property.CoverImage Decode.string
                Flags = get |> Get.optional Property.Flags Decode.int
                ApproximateGuildCount = get |> Get.optional Property.ApproximateGuildCount Decode.int
                ApproximateUserInstallCount = get |> Get.optional Property.ApproximateUserInstallCount Decode.int
                RedirectUris = get |> Get.optional Property.RedirectUris (Decode.list Decode.string)
                InteractionsEndpointUrl = get |> Get.optinull Property.InteractionsEndpointUrl Decode.string
                RoleConnectionsVerificationUrl = get |> Get.optinull Property.RoleConnectionsVerificationUrl Decode.string
                EventWebhooksUrl = get |> Get.optinull Property.EventWebhooksUrl Decode.string
                EventWebhooksStatus = get |> Get.optional Property.EventWebhooksStatus Decode.Enum.int<WebhookEventStatus>
                EventWebhooksTypes = get |> Get.optional Property.EventWebhooksTypes (Decode.list WebhookEventType.decoder)
                Tags = get |> Get.optional Property.Tags (Decode.list Decode.string)
                InstallParams = get |> Get.optional Property.InstallParams InstallParams.decoder
                IntegrationTypesConfig = get |> Get.optional Property.IntegrationTypesConfig (Decode.mapkv ApplicationIntegrationType.fromString ApplicationIntegrationTypeConfiguration.decoder)
                CustomInstallUrl  = get |> Get.optional Property.CustomInstallUrl Decode.string
            }) path v

        let encoder (v: PartialApplication) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.Name Encode.string v.Name
                |> Encode.optinull Property.Icon Encode.string v.Icon
                |> Encode.optional Property.Description Encode.string v.Description
                |> Encode.optional Property.RpcOrigins (List.map Encode.string >> Encode.list) v.RpcOrigins
                |> Encode.optional Property.BotPublic Encode.bool v.BotPublic
                |> Encode.optional Property.BotRequireCodeGrant Encode.bool v.BotRequireCodeGrant
                |> Encode.optional Property.Bot User.Partial.encoder v.Bot
                |> Encode.optional Property.TermsOfServiceUrl Encode.string v.TermsOfServiceUrl
                |> Encode.optional Property.PrivacyPolicyUrl Encode.string v.TermsOfServiceUrl
                |> Encode.optional Property.Owner User.Partial.encoder v.Owner
                |> Encode.optional Property.VerifyKey Encode.string v.VerifyKey
                |> Encode.optinull Property.Team Team.encoder v.Team
                |> Encode.optional Property.GuildId Encode.string v.GuildId
                |> Encode.optional Property.Guild Guild.Partial.encoder v.Guild
                |> Encode.optional Property.PrimarySkuId Encode.string v.PrimarySkuId
                |> Encode.optional Property.Slug Encode.string v.Slug
                |> Encode.optional Property.CoverImage Encode.string v.CoverImage
                |> Encode.optional Property.Flags Encode.int v.Flags
                |> Encode.optional Property.ApproximateGuildCount Encode.int v.ApproximateGuildCount
                |> Encode.optional Property.ApproximateUserInstallCount Encode.int v.ApproximateUserInstallCount
                |> Encode.optional Property.RedirectUris (List.map Encode.string >> Encode.list) v.RedirectUris
                |> Encode.optinull Property.InteractionsEndpointUrl Encode.string v.InteractionsEndpointUrl
                |> Encode.optinull Property.RoleConnectionsVerificationUrl Encode.string v.RoleConnectionsVerificationUrl
                |> Encode.optinull Property.EventWebhooksUrl Encode.string v.EventWebhooksUrl
                |> Encode.optional Property.EventWebhooksStatus Encode.Enum.int v.EventWebhooksStatus
                |> Encode.optional Property.EventWebhooksTypes (List.map WebhookEventType.encoder >> Encode.list) v.EventWebhooksTypes
                |> Encode.optional Property.Tags (List.map Encode.string >> Encode.list) v.Tags
                |> Encode.optional Property.InstallParams InstallParams.encoder v.InstallParams
                |> Encode.optional Property.IntegrationTypesConfig (Encode.mapkv ApplicationIntegrationType.toString ApplicationIntegrationTypeConfiguration.encoder) v.IntegrationTypesConfig
                |> Encode.optional Property.CustomInstallUrl Encode.string v.CustomInstallUrl
            )

module ApplicationIntegrationTypeConfiguration =
    module Property =
        let [<Literal>] OAuth2InstallParams = "oauth2_install_params"

    let decoder path v =
        Decode.object (fun get -> {
            OAuth2InstallParams = get.Optional.Field Property.OAuth2InstallParams InstallParams.decoder
        }) path v

    let encoder (v: ApplicationIntegrationTypeConfiguration) =
        Encode.object [
            Property.OAuth2InstallParams, Encode.option InstallParams.encoder v.OAuth2InstallParams
        ]

module InstallParams =
    module Property =
        let [<Literal>] Scopes = "scopes"
        let [<Literal>] Permissions = "permissions"

    let decoder path v =
        Decode.object (fun get -> {
            Scopes = get.Required.Field Property.Scopes (Decode.list OAuthScope.decoder)
            Permissions = get.Required.Field Property.Permissions Decode.string
        }) path v

    let encoder (v: InstallParams) =
        Encode.object [
            Property.Scopes, (List.map OAuthScope.encoder >> Encode.list) v.Scopes
            Property.Permissions, Encode.string v.Permissions
        ]

module ActivityInstance =
    module Property =
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] InstanceId = "instance_id"
        let [<Literal>] LaunchId = "launch_id"
        let [<Literal>] Location = "location"
        let [<Literal>] Users = "users"

    let decoder path v =
        Decode.object (fun get -> {
            ApplicationId = get.Required.Field Property.ApplicationId Decode.string
            InstanceId = get.Required.Field Property.InstanceId Decode.string
            LaunchId = get.Required.Field Property.LaunchId Decode.string
            Location = get.Required.Field Property.Location ActivityLocation.decoder
            Users = get.Required.Field Property.Users (Decode.list Decode.string)
        }) path v

    let encoder (v: ActivityInstance) =
        Encode.object [
            Property.ApplicationId, Encode.string v.ApplicationId
            Property.InstanceId, Encode.string v.InstanceId
            Property.LaunchId, Encode.string v.LaunchId
            Property.Location, ActivityLocation.encoder v.Location
            Property.Users, (List.map Encode.string >> Encode.list) v.Users
        ]

module ActivityLocation =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Kind = "kind"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Kind = get.Required.Field Property.Kind ActivityLocationKind.decoder
            ChannelId = get.Required.Field Property.ChannelId Decode.string
            GuildId = get.Optional.Field Property.GuildId (Decode.option Decode.string)
        }) path v

    let encoder (v: ActivityLocation) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Kind, ActivityLocationKind.encoder v.Kind
            Property.ChannelId, Encode.string v.ChannelId
            Property.GuildId, Encode.option Encode.string (v.GuildId |> Option.flatten)
        ]

module ApplicationRoleConnectionMetadata =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Key = "key"
        let [<Literal>] Name = "name"
        let [<Literal>] NameLocalizations = "name_localizations"
        let [<Literal>] Description = "description"
        let [<Literal>] DescriptionLocalizations = "description_localizations"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationRoleConnectionMetadataType>
            Key = get |> Get.required Property.Key Decode.string
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optional Property.NameLocalizations (Decode.dict Decode.string)
            Description = get |> Get.required Property.Description Decode.string
            DescriptionLocalizations = get |> Get.optional Property.DescriptionLocalizations (Decode.dict Decode.string)
        }) path v

    let encoder (v: ApplicationRoleConnectionMetadata) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Key Encode.string v.Key
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optional Property.NameLocalizations (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optional Property.DescriptionLocalizations (Encode.mapv Encode.string) v.DescriptionLocalizations
        )

module AuditLog =
    module Property =
        let [<Literal>] ApplicationCommands = "application_commands"
        let [<Literal>] AuditLogEntries = "audit_log_entries"
        let [<Literal>] AutoModerationRules = "auto_moderation_rules"
        let [<Literal>] GuildScheduledEvents = "guild_scheduled_events"
        let [<Literal>] Integrations = "integrations"
        let [<Literal>] Threads = "threads"
        let [<Literal>] Users = "users"
        let [<Literal>] Webhooks = "webhooks"

    let decoder path v =
        Decode.object (fun get -> {
            ApplicationCommands = get.Required.Field Property.ApplicationCommands (Decode.list ApplicationCommand.decoder)
            AuditLogEntries = get.Required.Field Property.AuditLogEntries (Decode.list AuditLogEntry.decoder)
            AutoModerationRules = get.Required.Field Property.AutoModerationRules (Decode.list AutoModerationRule.decoder)
            GuildScheduledEvents = get.Required.Field Property.GuildScheduledEvents (Decode.list GuildScheduledEvent.decoder)
            Integrations = get.Required.Field Property.Integrations (Decode.list Integration.Partial.decoder)
            Threads = get.Required.Field Property.Threads (Decode.list Channel.decoder)
            Users = get.Required.Field Property.Users (Decode.list User.decoder)
            Webhooks = get.Required.Field Property.Webhooks (Decode.list Webhook.decoder)
        }) path v

    let encoder (v: AuditLog) =
        Encode.object [
            Property.ApplicationCommands, (List.map ApplicationCommand.encoder >> Encode.list) v.ApplicationCommands
            Property.AuditLogEntries, (List.map AuditLogEntry.encoder >> Encode.list) v.AuditLogEntries
            Property.AutoModerationRules, (List.map AutoModerationRule.encoder >> Encode.list) v.AutoModerationRules
            Property.GuildScheduledEvents, (List.map GuildScheduledEvent.encoder >> Encode.list) v.GuildScheduledEvents
            Property.Integrations, (List.map Integration.Partial.encoder >> Encode.list) v.Integrations
            Property.Threads, (List.map Channel.encoder >> Encode.list) v.Threads
            Property.Users, (List.map User.encoder >> Encode.list) v.Users
            Property.Webhooks, (List.map Webhook.encoder >> Encode.list) v.Webhooks
        ]

module AuditLogEntry =
    module Property =
        let [<Literal>] TargetId = "target_id"
        let [<Literal>] Changes = "changes"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] Id = "id"
        let [<Literal>] ActionType = "action_type"
        let [<Literal>] Options = "options"
        let [<Literal>] Reason = "reason"

    let decoder path v =
        Decode.object (fun get -> {
            TargetId = get |> Get.nullable Property.TargetId Decode.string
            Changes = get |> Get.optional Property.Changes (Decode.list AuditLogChange.decoder)
            UserId = get |> Get.nullable Property.UserId Decode.string
            Id = get |> Get.required Property.Id Decode.string
            ActionType = get |> Get.required Property.ActionType Decode.Enum.int<AuditLogEventType>
            Options = get |> Get.optional Property.Options AuditLogEntryOptionalInfo.decoder
            Reason = get |> Get.optional Property.Reason Decode.string
        }) path v

    let encoder (v: AuditLogEntry) =
        Encode.object ([]
            |> Encode.nullable Property.TargetId Encode.string v.TargetId
            |> Encode.optional Property.Changes (List.map AuditLogChange.encoder >> Encode.list) v.Changes
            |> Encode.nullable Property.UserId Encode.string v.UserId
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.ActionType Encode.Enum.int v.ActionType
            |> Encode.optional Property.Options AuditLogEntryOptionalInfo.encoder v.Options
            |> Encode.optional Property.Reason Encode.string v.Reason
        )

module AuditLogEntryOptionalInfo =
    module Property =
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] AutoModerationRuleName = "auto_moderation_rule_name"
        let [<Literal>] AutoModerationRuleTriggerType = "auto_moderation_rule_trigger_type"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Count = "count"
        let [<Literal>] DeleteMemberDays = "delete_member_days"
        let [<Literal>] Id = "id"
        let [<Literal>] MembersRemoved = "members_removed"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] RoleName = "role_name"
        let [<Literal>] Type = "type"
        let [<Literal>] IntegrationType = "integration_type"

    let decoder path v =
        Decode.object (fun get -> {
            ApplicationId = get |> Get.optional Property.ApplicationId Decode.string
            AutoModerationRuleName = get |> Get.optional Property.AutoModerationRuleName Decode.string
            AutoModerationRuleTriggerType = get |> Get.optional Property.AutoModerationRuleTriggerType Decode.string
            ChannelId = get |> Get.optional Property.ChannelId Decode.string
            Count = get |> Get.optional Property.Count Decode.string
            DeleteMemberDays = get |> Get.optional Property.DeleteMemberDays Decode.string
            Id = get |> Get.optional Property.Id Decode.string
            MembersRemoved = get |> Get.optional Property.MembersRemoved Decode.string
            MessageId = get |> Get.optional Property.MessageId Decode.string
            RoleName = get |> Get.optional Property.RoleName Decode.string
            Type = get |> Get.optional Property.Type Decode.string
            IntegrationType = get |> Get.optional Property.IntegrationType Decode.string
        }) path v

    let encoder (v: AuditLogEntryOptionalInfo) =
        Encode.object ([]
            |> Encode.optional Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optional Property.AutoModerationRuleName Encode.string v.AutoModerationRuleName
            |> Encode.optional Property.AutoModerationRuleTriggerType Encode.string v.AutoModerationRuleTriggerType
            |> Encode.optional Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.Count Encode.string v.Count
            |> Encode.optional Property.DeleteMemberDays Encode.string v.DeleteMemberDays
            |> Encode.optional Property.Id Encode.string v.Id
            |> Encode.optional Property.MembersRemoved Encode.string v.MembersRemoved
            |> Encode.optional Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.RoleName Encode.string v.RoleName
            |> Encode.optional Property.Type Encode.string v.Type
            |> Encode.optional Property.IntegrationType Encode.string v.IntegrationType
        )

module AuditLogChange =
    module Property =
        let [<Literal>] NewValue = "new_value"
        let [<Literal>] OldValue = "old_value"
        let [<Literal>] Key = "key"

    let decoder path v =
        Decode.object (fun get -> {
            NewValue = None
            OldValue = None
            Key = get |> Get.required Property.Key Decode.string
        }) path v

    let encoder (v: AuditLogChange) =
        Encode.object ([]
            |> Encode.optional Property.NewValue Encode.unit None
            |> Encode.optional Property.OldValue Encode.unit None
            |> Encode.required Property.Key Encode.string v.Key
        )

    // TODO: Fix old and new value serialization to not just be `None` always

module AutoModerationRule =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Name = "name"
        let [<Literal>] CreatorId = "creator_id"
        let [<Literal>] EventType = "event_type"
        let [<Literal>] TriggerType = "trigger_type"
        let [<Literal>] TriggerMetadata = "trigger_metadata"
        let [<Literal>] Actions = "actions"
        let [<Literal>] Enabled = "enabled"
        let [<Literal>] ExemptRoles = "exempt_roles"
        let [<Literal>] ExemptChannels = "exempt_channels"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            Name = get |> Get.required Property.Name Decode.string
            CreatorId = get |> Get.required Property.CreatorId Decode.string
            EventType = get |> Get.required Property.EventType Decode.Enum.int<AutoModerationEventType>
            TriggerType = get |> Get.required Property.TriggerType Decode.Enum.int<AutoModerationTriggerType>
            TriggerMetadata = get |> Get.required Property.TriggerMetadata AutoModerationTriggerMetadata.decoder
            Actions = get |> Get.required Property.Actions (Decode.list AutoModerationAction.decoder)
            Enabled = get |> Get.required Property.Enabled Decode.bool
            ExemptRoles = get |> Get.required Property.ExemptRoles (Decode.list Decode.string)
            ExemptChannels = get |> Get.required Property.ExemptChannels (Decode.list Decode.string)
        }) path v

    let encoder (v: AutoModerationRule) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.CreatorId Encode.string v.CreatorId
            |> Encode.required Property.EventType Encode.Enum.int v.EventType
            |> Encode.required Property.TriggerType Encode.Enum.int v.TriggerType
            |> Encode.required Property.TriggerMetadata AutoModerationTriggerMetadata.encoder v.TriggerMetadata
            |> Encode.required Property.Actions (List.map AutoModerationAction.encoder >> Encode.list) v.Actions
            |> Encode.required Property.Enabled Encode.bool v.Enabled
            |> Encode.required Property.ExemptRoles (List.map Encode.string >> Encode.list) v.ExemptRoles
            |> Encode.required Property.ExemptChannels (List.map Encode.string >> Encode.list) v.ExemptChannels
        )

module AutoModerationTriggerMetadata =
    module Property =
        let [<Literal>] KeywordFilter = "keyword_filter"
        let [<Literal>] RegexPatterns = "regex_patterns"
        let [<Literal>] Presets = "presets"
        let [<Literal>] AllowList = "allow_list"
        let [<Literal>] MentionTotalLimit = "mention_total_limit"
        let [<Literal>] MentionRaidProtectionEnabled = "mention_raid_protection_enabled"

    let decoder path v =
        Decode.object (fun get -> {
            KeywordFilter = get |> Get.optional Property.KeywordFilter (Decode.list Decode.string)
            RegexPatterns = get |> Get.optional Property.RegexPatterns (Decode.list Decode.string)
            Presets = get |> Get.optional Property.Presets (Decode.list Decode.Enum.int<AutoModerationKeywordPreset>)
            AllowList = get |> Get.optional Property.AllowList (Decode.list Decode.string)
            MentionTotalLimit = get |> Get.optional Property.MentionTotalLimit Decode.int
            MentionRaidProtectionEnabled = get |> Get.optional Property.MentionRaidProtectionEnabled Decode.bool
        }) path v

    let encoder (v: AutoModerationTriggerMetadata) =
        Encode.object ([]
            |> Encode.optional Property.KeywordFilter (List.map Encode.string >> Encode.list) v.KeywordFilter
            |> Encode.optional Property.RegexPatterns (List.map Encode.string >> Encode.list) v.RegexPatterns
            |> Encode.optional Property.Presets (List.map Encode.Enum.int >> Encode.list) v.Presets
            |> Encode.optional Property.AllowList (List.map Encode.string >> Encode.list) v.AllowList
            |> Encode.optional Property.MentionTotalLimit Encode.int v.MentionTotalLimit
            |> Encode.optional Property.MentionRaidProtectionEnabled Encode.bool v.MentionRaidProtectionEnabled
        )

module AutoModerationAction =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Metadata = "metadata"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<AutoModerationActionType>
            Metadata = get |> Get.optional Property.Metadata AutoModerationActionMetadata.decoder
        }) path v

    let encoder (v: AutoModerationAction) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.Metadata AutoModerationActionMetadata.encoder v.Metadata
        )

module AutoModerationActionMetadata =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] DurationSeconds = "duration_seconds"
        let [<Literal>] CustomMessage = "custom_message"

    let decoder path v =
        Decode.object (fun get -> {
            ChannelId = get |> Get.optional Property.ChannelId Decode.string
            DurationSeconds = get |> Get.optional Property.DurationSeconds Decode.int
            CustomMessage = get |> Get.optional Property.CustomMessage Decode.string
        }) path v

    let encoder (v: AutoModerationActionMetadata) =
        Encode.object ([]
            |> Encode.optional Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.DurationSeconds Encode.int v.DurationSeconds
            |> Encode.optional Property.CustomMessage Encode.string v.CustomMessage
        )

module Channel =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Position = "position"
        let [<Literal>] PermissionOverwrites = "permission_overwrites"
        let [<Literal>] Name = "name"
        let [<Literal>] Topic = "topic"
        let [<Literal>] Nsfw = "nsfw"
        let [<Literal>] LastMessageId = "last_message_id"
        let [<Literal>] Bitrate = "bitrate"
        let [<Literal>] UserLimit = "user_limit"
        let [<Literal>] RateLimitPerUser = "rate_limit_per_user"
        let [<Literal>] Recipients = "recipients"
        let [<Literal>] Icon = "icon"
        let [<Literal>] OwnerId = "owner_id"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Managed = "managed"
        let [<Literal>] ParentId = "parent_id"
        let [<Literal>] LastPinTimestamp = "last_pin_timestamp"
        let [<Literal>] RtcRegion = "rtc_region"
        let [<Literal>] VideoQualityMode = "video_quality_mode"
        let [<Literal>] MessageCount = "message_count"
        let [<Literal>] MemberCount = "member_count"
        let [<Literal>] ThreadMetadata = "thread_metadata"
        let [<Literal>] Member = "member"
        let [<Literal>] DefaultAutoArchiveDuration = "default_auto_archive_duration"
        let [<Literal>] Permissions = "permissions"
        let [<Literal>] Flags = "flags"
        let [<Literal>] TotalMessagesSent = "total_messages_sent"
        let [<Literal>] AvailableTags = "available_tags"
        let [<Literal>] AppliedTags = "applied_tags"
        let [<Literal>] DefaultReactionEmoji = "default_reaction_emoji"
        let [<Literal>] DefaultThreadRateLimitPerUser = "default_thread_rate_limit_per_user"
        let [<Literal>] DefaultSortOrder = "default_sort_order"
        let [<Literal>] DefaultForumLayout = "default_forum_layout"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ChannelType>
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Position = get |> Get.optional Property.Position Decode.int
            PermissionOverwrites = get |> Get.optional Property.PermissionOverwrites (Decode.list PermissionOverwrite.decoder)
            Name = get |> Get.optinull Property.Name Decode.string
            Topic = get |> Get.optinull Property.Topic Decode.string
            Nsfw = get |> Get.optional Property.Nsfw Decode.bool
            LastMessageId = get |> Get.optinull Property.LastMessageId Decode.string
            Bitrate = get |> Get.optional Property.Bitrate Decode.int
            UserLimit = get |> Get.optional Property.UserLimit Decode.int
            RateLimitPerUser = get |> Get.optional Property.RateLimitPerUser Decode.int
            Recipients = get |> Get.optional Property.Recipients (Decode.list User.decoder)
            Icon = get |> Get.optinull Property.Icon Decode.string
            OwnerId = get |> Get.optional Property.OwnerId Decode.string
            ApplicationId = get |> Get.optional Property.ApplicationId Decode.string
            Managed = get |> Get.optional Property.Managed Decode.bool
            ParentId = get |> Get.optinull Property.ParentId Decode.string
            LastPinTimestamp = get |> Get.optinull Property.LastPinTimestamp Decode.datetimeUtc
            RtcRegion = get |> Get.optinull Property.RtcRegion Decode.string
            VideoQualityMode = get |> Get.optional Property.VideoQualityMode Decode.Enum.int<VideoQualityMode>
            MessageCount = get |> Get.optional Property.MessageCount Decode.int
            MemberCount = get |> Get.optional Property.MemberCount Decode.int
            ThreadMetadata = get |> Get.optional Property.ThreadMetadata ThreadMetadata.decoder
            Member = get |> Get.optional Property.Member ThreadMember.decoder
            DefaultAutoArchiveDuration = get |> Get.optional Property.DefaultAutoArchiveDuration Decode.Enum.int<AutoArchiveDuration>
            Permissions = get |> Get.optional Property.Permissions Decode.string
            Flags = get |> Get.optional Property.Flags Decode.int
            TotalMessagesSent = get |> Get.optional Property.TotalMessagesSent Decode.int
            AvailableTags = get |> Get.optional Property.AvailableTags (Decode.list ForumTag.decoder)
            AppliedTags = get |> Get.optional Property.AppliedTags (Decode.list Decode.string)
            DefaultReactionEmoji = get |> Get.optinull Property.DefaultReactionEmoji DefaultReaction.decoder
            DefaultThreadRateLimitPerUser = get |> Get.optional Property.DefaultThreadRateLimitPerUser Decode.int
            DefaultSortOrder = get |> Get.optinull Property.DefaultSortOrder Decode.Enum.int<ChannelSortOrder>
            DefaultForumLayout = get |> Get.optional Property.DefaultForumLayout Decode.Enum.int<ForumLayout>
        }) path v

    let encoder (v: Channel) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Position Encode.int v.Position
            |> Encode.optional Property.PermissionOverwrites (List.map PermissionOverwrite.encoder >> Encode.list) v.PermissionOverwrites
            |> Encode.optinull Property.Name Encode.string v.Name
            |> Encode.optinull Property.Topic Encode.string v.Topic
            |> Encode.optional Property.Nsfw Encode.bool v.Nsfw
            |> Encode.optinull Property.LastMessageId Encode.string v.LastMessageId
            |> Encode.optional Property.Bitrate Encode.int v.Bitrate
            |> Encode.optional Property.UserLimit Encode.int v.UserLimit
            |> Encode.optional Property.RateLimitPerUser Encode.int v.RateLimitPerUser
            |> Encode.optional Property.Recipients (List.map User.encoder >> Encode.list) v.Recipients
            |> Encode.optinull Property.Icon Encode.string v.Icon
            |> Encode.optional Property.OwnerId Encode.string v.OwnerId
            |> Encode.optional Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optional Property.Managed Encode.bool v.Managed
            |> Encode.optinull Property.ParentId Encode.string v.ParentId
            |> Encode.optinull Property.LastPinTimestamp Encode.datetime v.LastPinTimestamp
            |> Encode.optinull Property.RtcRegion Encode.string v.RtcRegion
            |> Encode.optional Property.VideoQualityMode Encode.Enum.int v.VideoQualityMode
            |> Encode.optional Property.MessageCount Encode.int v.MessageCount
            |> Encode.optional Property.MemberCount Encode.int v.MemberCount
            |> Encode.optional Property.ThreadMetadata ThreadMetadata.encoder v.ThreadMetadata
            |> Encode.optional Property.Member ThreadMember.encoder v.Member
            |> Encode.optional Property.DefaultAutoArchiveDuration Encode.Enum.int v.DefaultAutoArchiveDuration
            |> Encode.optional Property.Permissions Encode.string v.Permissions
            |> Encode.optional Property.Flags Encode.int v.Flags
            |> Encode.optional Property.TotalMessagesSent Encode.int v.TotalMessagesSent
            |> Encode.optional Property.AvailableTags (List.map ForumTag.encoder >> Encode.list) v.AvailableTags
            |> Encode.optional Property.AppliedTags (List.map Encode.string >> Encode.list) v.AppliedTags
            |> Encode.optinull Property.DefaultReactionEmoji DefaultReaction.encoder v.DefaultReactionEmoji
            |> Encode.optional Property.DefaultThreadRateLimitPerUser Encode.int v.DefaultThreadRateLimitPerUser
            |> Encode.optinull Property.DefaultSortOrder Encode.Enum.int v.DefaultSortOrder
            |> Encode.optional Property.DefaultForumLayout Encode.Enum.int v.DefaultForumLayout
        )

    module Partial =
        let decoder path v =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Type = get |> Get.optional Property.Type Decode.Enum.int<ChannelType>
                GuildId = get |> Get.optional Property.GuildId Decode.string
                Position = get |> Get.optional Property.Position Decode.int
                PermissionOverwrites = get |> Get.optional Property.PermissionOverwrites (Decode.list PermissionOverwrite.decoder)
                Name = get |> Get.optinull Property.Name Decode.string
                Topic = get |> Get.optinull Property.Topic Decode.string
                Nsfw = get |> Get.optional Property.Nsfw Decode.bool
                LastMessageId = get |> Get.optinull Property.LastMessageId Decode.string
                Bitrate = get |> Get.optional Property.Bitrate Decode.int
                UserLimit = get |> Get.optional Property.UserLimit Decode.int
                RateLimitPerUser = get |> Get.optional Property.RateLimitPerUser Decode.int
                Recipients = get |> Get.optional Property.Recipients (Decode.list User.decoder)
                Icon = get |> Get.optinull Property.Icon Decode.string
                OwnerId = get |> Get.optional Property.OwnerId Decode.string
                ApplicationId = get |> Get.optional Property.ApplicationId Decode.string
                Managed = get |> Get.optional Property.Managed Decode.bool
                ParentId = get |> Get.optinull Property.ParentId Decode.string
                LastPinTimestamp = get |> Get.optinull Property.LastPinTimestamp Decode.datetimeUtc
                RtcRegion = get |> Get.optinull Property.RtcRegion Decode.string
                VideoQualityMode = get |> Get.optional Property.VideoQualityMode Decode.Enum.int<VideoQualityMode>
                MessageCount = get |> Get.optional Property.MessageCount Decode.int
                MemberCount = get |> Get.optional Property.MemberCount Decode.int
                ThreadMetadata = get |> Get.optional Property.ThreadMetadata ThreadMetadata.decoder
                Member = get |> Get.optional Property.Member ThreadMember.decoder
                DefaultAutoArchiveDuration = get |> Get.optional Property.DefaultAutoArchiveDuration Decode.Enum.int<AutoArchiveDuration>
                Permissions = get |> Get.optional Property.Permissions Decode.string
                Flags = get |> Get.optional Property.Flags Decode.int
                TotalMessagesSent = get |> Get.optional Property.TotalMessagesSent Decode.int
                AvailableTags = get |> Get.optional Property.AvailableTags (Decode.list ForumTag.decoder)
                AppliedTags = get |> Get.optional Property.AppliedTags (Decode.list Decode.string)
                DefaultReactionEmoji = get |> Get.optinull Property.DefaultReactionEmoji DefaultReaction.decoder
                DefaultThreadRateLimitPerUser = get |> Get.optional Property.DefaultThreadRateLimitPerUser Decode.int
                DefaultSortOrder = get |> Get.optinull Property.DefaultSortOrder Decode.Enum.int<ChannelSortOrder>
                DefaultForumLayout = get |> Get.optional Property.DefaultForumLayout Decode.Enum.int<ForumLayout>
            }) path v

        let encoder (v: PartialChannel) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.Type Encode.Enum.int v.Type
                |> Encode.optional Property.GuildId Encode.string v.GuildId
                |> Encode.optional Property.Position Encode.int v.Position
                |> Encode.optional Property.PermissionOverwrites (List.map PermissionOverwrite.encoder >> Encode.list) v.PermissionOverwrites
                |> Encode.optinull Property.Name Encode.string v.Name
                |> Encode.optinull Property.Topic Encode.string v.Topic
                |> Encode.optional Property.Nsfw Encode.bool v.Nsfw
                |> Encode.optinull Property.LastMessageId Encode.string v.LastMessageId
                |> Encode.optional Property.Bitrate Encode.int v.Bitrate
                |> Encode.optional Property.UserLimit Encode.int v.UserLimit
                |> Encode.optional Property.RateLimitPerUser Encode.int v.RateLimitPerUser
                |> Encode.optional Property.Recipients (List.map User.encoder >> Encode.list) v.Recipients
                |> Encode.optinull Property.Icon Encode.string v.Icon
                |> Encode.optional Property.OwnerId Encode.string v.OwnerId
                |> Encode.optional Property.ApplicationId Encode.string v.ApplicationId
                |> Encode.optional Property.Managed Encode.bool v.Managed
                |> Encode.optinull Property.ParentId Encode.string v.ParentId
                |> Encode.optinull Property.LastPinTimestamp Encode.datetime v.LastPinTimestamp
                |> Encode.optinull Property.RtcRegion Encode.string v.RtcRegion
                |> Encode.optional Property.VideoQualityMode Encode.Enum.int v.VideoQualityMode
                |> Encode.optional Property.MessageCount Encode.int v.MessageCount
                |> Encode.optional Property.MemberCount Encode.int v.MemberCount
                |> Encode.optional Property.ThreadMetadata ThreadMetadata.encoder v.ThreadMetadata
                |> Encode.optional Property.Member ThreadMember.encoder v.Member
                |> Encode.optional Property.DefaultAutoArchiveDuration Encode.Enum.int v.DefaultAutoArchiveDuration
                |> Encode.optional Property.Permissions Encode.string v.Permissions
                |> Encode.optional Property.Flags Encode.int v.Flags
                |> Encode.optional Property.TotalMessagesSent Encode.int v.TotalMessagesSent
                |> Encode.optional Property.AvailableTags (List.map ForumTag.encoder >> Encode.list) v.AvailableTags
                |> Encode.optional Property.AppliedTags (List.map Encode.string >> Encode.list) v.AppliedTags
                |> Encode.optinull Property.DefaultReactionEmoji DefaultReaction.encoder v.DefaultReactionEmoji
                |> Encode.optional Property.DefaultThreadRateLimitPerUser Encode.int v.DefaultThreadRateLimitPerUser
                |> Encode.optinull Property.DefaultSortOrder Encode.Enum.int v.DefaultSortOrder
                |> Encode.optional Property.DefaultForumLayout Encode.Enum.int v.DefaultForumLayout
            )

module FollowedChannel =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] WebhookId = "webhook_id"

    let decoder path v =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            WebhookId = get |> Get.required Property.WebhookId Decode.string
        }) path v

    let encoder (v: FollowedChannel) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.WebhookId Encode.string v.WebhookId
        )

module PermissionOverwrite =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] Allow = "allow"
        let [<Literal>] Deny = "deny"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<PermissionOverwriteType>
            Allow = get |> Get.required Property.Allow Decode.string
            Deny = get |> Get.required Property.Deny Decode.string
        }) path v

    let encoder (v: PermissionOverwrite) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Allow Encode.string v.Allow
            |> Encode.required Property.Deny Encode.string v.Deny
        )

    module Partial =
        let decoder path v =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Type = get |> Get.optional Property.Type Decode.Enum.int<PermissionOverwriteType>
                Allow = get |> Get.optional Property.Allow Decode.string
                Deny = get |> Get.optional Property.Deny Decode.string
            }) path v

        let encoder (v: PartialPermissionOverwrite) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.Type Encode.Enum.int v.Type
                |> Encode.optional Property.Allow Encode.string v.Allow
                |> Encode.optional Property.Deny Encode.string v.Deny
            )

module ThreadMetadata =
    module Property =
        let [<Literal>] Archived = "archived"
        let [<Literal>] AutoArchiveDuration = "auto_archive_duration"
        let [<Literal>] ArchiveTimestamp = "archive_timestamp"
        let [<Literal>] Locked = "locked"
        let [<Literal>] Invitable = "invitable"
        let [<Literal>] CreateTimestamp = "create_timestamp"

    let decoder path v =
        Decode.object (fun get -> {
            Archived = get |> Get.required Property.Archived Decode.bool
            AutoArchiveDuration = get |> Get.required Property.AutoArchiveDuration Decode.Enum.int<AutoArchiveDuration>
            ArchiveTimestamp = get |> Get.required Property.ArchiveTimestamp Decode.datetimeUtc
            Locked = get |> Get.required Property.Locked Decode.bool
            Invitable = get |> Get.optional Property.Invitable Decode.bool
            CreateTimestamp = get |> Get.optinull Property.CreateTimestamp Decode.datetimeUtc
        }) path v

    let encoder (v: ThreadMetadata) =
        Encode.object ([]
            |> Encode.required Property.Archived Encode.bool v.Archived
            |> Encode.required Property.AutoArchiveDuration Encode.Enum.int v.AutoArchiveDuration
            |> Encode.required Property.ArchiveTimestamp Encode.datetime v.ArchiveTimestamp
            |> Encode.required Property.Locked Encode.bool v.Locked
            |> Encode.optional Property.Invitable Encode.bool v.Invitable
            |> Encode.optinull Property.CreateTimestamp Encode.datetime v.CreateTimestamp
        )

module ThreadMember =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] JoinTimestamp = "join_timestamp"
        let [<Literal>] Flags = "flags"
        let [<Literal>] Member = "member"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.optional Property.Id Decode.string
            UserId = get |> Get.optional Property.UserId Decode.string
            JoinTimestamp = get |> Get.required Property.JoinTimestamp Decode.datetimeUtc
            Flags = get |> Get.required Property.Flags Decode.int
            Member = get |> Get.optional Property.Member GuildMember.decoder
        }) path v

    let encoder (v: ThreadMember) =
        Encode.object ([]
            |> Encode.optional Property.Id Encode.string v.Id
            |> Encode.optional Property.UserId Encode.string v.UserId
            |> Encode.required Property.JoinTimestamp Encode.datetime v.JoinTimestamp
            |> Encode.required Property.Flags Encode.int v.Flags
            |> Encode.optional Property.Member GuildMember.encoder v.Member
        )

module DefaultReaction =
    module Property =
        let [<Literal>] EmojiId = "emoji_id"
        let [<Literal>] EmojiName = "emoji_name"

    let decoder path v =
        Decode.object (fun get -> {
            EmojiId = get |> Get.nullable Property.EmojiId Decode.string
            EmojiName = get |> Get.nullable Property.EmojiName Decode.string
        }) path v

    let encoder (v: DefaultReaction) =
        Encode.object ([]
            |> Encode.nullable Property.EmojiId Encode.string v.EmojiId
            |> Encode.nullable Property.EmojiName Encode.string v.EmojiName
        )

module ForumTag =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Moderated = "moderated"
        let [<Literal>] EmojiId = "emoji_id"
        let [<Literal>] EmojiName = "emoji_name"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Moderated = get |> Get.required Property.Moderated Decode.bool
            EmojiId = get |> Get.nullable Property.EmojiId Decode.string
            EmojiName = get |> Get.nullable Property.EmojiName Decode.string
        }) path v

    let encoder (v: ForumTag) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Moderated Encode.bool v.Moderated
            |> Encode.nullable Property.EmojiId Encode.string v.EmojiId
            |> Encode.nullable Property.EmojiName Encode.string v.EmojiName
        )

module Emoji =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Roles = "roles"
        let [<Literal>] User = "user"
        let [<Literal>] RequireColons = "require_colons"
        let [<Literal>] Managed = "managed"
        let [<Literal>] Animated = "animated"
        let [<Literal>] Available = "available"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.nullable Property.Id Decode.string
            Name = get |> Get.nullable Property.Name Decode.string
            Roles = get |> Get.optional Property.Roles (Decode.list Decode.string)
            User = get |> Get.optional Property.User User.decoder
            RequireColons = get |> Get.optional Property.RequireColons Decode.bool
            Managed = get |> Get.optional Property.Managed Decode.bool
            Animated = get |> Get.optional Property.Animated Decode.bool
            Available = get |> Get.optional Property.Available Decode.bool
        }) path v

    let encoder (v: Emoji) =
        Encode.object ([]
            |> Encode.nullable Property.Id Encode.string v.Id
            |> Encode.nullable Property.Name Encode.string v.Name
            |> Encode.optional Property.Roles (List.map Encode.string >> Encode.list) v.Roles
            |> Encode.optional Property.User User.encoder v.User
            |> Encode.optional Property.RequireColons Encode.bool v.RequireColons
            |> Encode.optional Property.Managed Encode.bool v.Managed
            |> Encode.optional Property.Animated Encode.bool v.Animated
            |> Encode.optional Property.Available Encode.bool v.Available
        )

    module Partial =
        let decoder path v: Result<PartialEmoji, DecoderError> =
            Decode.object (fun get -> {
                Id = get |> Get.nullable Property.Id Decode.string
                Name = get |> Get.nullable Property.Name Decode.string
                Roles = get |> Get.optional Property.Roles (Decode.list Decode.string)
                User = get |> Get.optional Property.User User.decoder
                RequireColons = get |> Get.optional Property.RequireColons Decode.bool
                Managed = get |> Get.optional Property.Managed Decode.bool
                Animated = get |> Get.optional Property.Animated Decode.bool
                Available = get |> Get.optional Property.Available Decode.bool
            }) path v

        let encoder (v: PartialEmoji) =
            Encode.object ([]
                |> Encode.nullable Property.Id Encode.string v.Id
                |> Encode.nullable Property.Name Encode.string v.Name
                |> Encode.optional Property.Roles (List.map Encode.string >> Encode.list) v.Roles
                |> Encode.optional Property.User User.encoder v.User
                |> Encode.optional Property.RequireColons Encode.bool v.RequireColons
                |> Encode.optional Property.Managed Encode.bool v.Managed
                |> Encode.optional Property.Animated Encode.bool v.Animated
                |> Encode.optional Property.Available Encode.bool v.Available
            )

module Entitlement =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] SkuId = "sku_id"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Deleted = "deleted"
        let [<Literal>] StartsAt = "starts_at"
        let [<Literal>] EndsAt = "ends_at"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Consumed = "consumed"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            SkuId = get |> Get.required Property.SkuId Decode.string
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            UserId = get |> Get.optional Property.UserId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<EntitlementType>
            Deleted = get |> Get.required Property.Deleted Decode.bool
            StartsAt = get |> Get.nullable Property.StartsAt Decode.datetimeUtc
            EndsAt = get |> Get.nullable Property.EndsAt Decode.datetimeUtc
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Consumed = get |> Get.optional Property.Consumed Decode.bool
        }) path v

    let encoder (v: Entitlement) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.SkuId Encode.string v.SkuId
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optional Property.UserId Encode.string v.UserId
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Deleted Encode.bool v.Deleted
            |> Encode.nullable Property.StartsAt Encode.datetime v.StartsAt
            |> Encode.nullable Property.EndsAt Encode.datetime v.EndsAt
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Consumed Encode.bool v.Consumed
        )

module Guild =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Icon = "icon"
        let [<Literal>] IconHash = "icon_hash"
        let [<Literal>] Splash = "splash"
        let [<Literal>] DiscoverySplash = "discovery_splash"
        let [<Literal>] Owner = "owner"
        let [<Literal>] OwnerId = "owner_id"
        let [<Literal>] Permissions = "permissions"
        let [<Literal>] AfkChannelId = "afk_channel_id"
        let [<Literal>] AfkTimeout = "afk_timeout"
        let [<Literal>] WidgetEnabled = "widget_enabled"
        let [<Literal>] WidgetChannelId = "widget_channel_id"
        let [<Literal>] VerificationLevel = "verification_level"
        let [<Literal>] DefaultMessageNotifications = "default_message_notifications"
        let [<Literal>] ExplicitContentFilter = "explicit_content_filter"
        let [<Literal>] Roles = "roles"
        let [<Literal>] Emojis = "emojis"
        let [<Literal>] Features = "features"
        let [<Literal>] MfaLevel = "mfa_level"
        let [<Literal>] ApplicationId = "applicationId"
        let [<Literal>] SystemChannelId = "system_channel_id"
        let [<Literal>] SystemChannelFlags = "system_channel_flags"
        let [<Literal>] RulesChannelId = "rules_channel_id"
        let [<Literal>] MaxPresences = "max_presences"
        let [<Literal>] MaxMembers = "max_members"
        let [<Literal>] VanityUrlCode = "vanity_url_code"
        let [<Literal>] Description = "description"
        let [<Literal>] Banner = "banner"
        let [<Literal>] PremiumTier = "premium_tier"
        let [<Literal>] PremiumSubscriptionCount = "premium_subscription_count"
        let [<Literal>] PreferredLocale = "preferred_locale"
        let [<Literal>] PublicUpdatesChannelId = "public_updates_channel_id"
        let [<Literal>] MaxVideoChannelUsers = "max_video_channel_users"
        let [<Literal>] MaxStageVideoChannelUsers = "max_stage_video_channel_users"
        let [<Literal>] ApproximateMemberCount = "approximate_member_count"
        let [<Literal>] ApproximatePresenceCount = "approximate_presence_count"
        let [<Literal>] WelcomeScreen = "welcome_screen"
        let [<Literal>] NsfwLevel = "nsfw_level"
        let [<Literal>] Stickers = "stickers"
        let [<Literal>] PremiumProgressBarEnabled = "premium_progress_bar_enabled"
        let [<Literal>] SafetyAlertsChannelId = "safety_alerts_channel_id"
        let [<Literal>] IncidentsData = "incidents_data"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Icon = get |> Get.nullable Property.Icon Decode.string
            IconHash = get |> Get.optinull Property.IconHash Decode.string
            Splash = get |> Get.nullable Property.Splash Decode.string
            DiscoverySplash = get |> Get.nullable Property.DiscoverySplash Decode.string
            Owner = get |> Get.optional Property.Owner Decode.bool
            OwnerId = get |> Get.required Property.OwnerId Decode.string
            Permissions = get |> Get.optional Property.Permissions Decode.string
            AfkChannelId = get |> Get.nullable Property.AfkChannelId Decode.string
            AfkTimeout = get |> Get.required Property.AfkTimeout Decode.int
            WidgetEnabled = get |> Get.optional Property.WidgetEnabled Decode.bool
            WidgetChannelId = get |> Get.optinull Property.WidgetChannelId Decode.string
            VerificationLevel = get |> Get.required Property.VerificationLevel Decode.Enum.int<VerificationLevel>
            DefaultMessageNotifications = get |> Get.required Property.DefaultMessageNotifications Decode.Enum.int<MessageNotificationLevel>
            ExplicitContentFilter = get |> Get.required Property.ExplicitContentFilter Decode.Enum.int<ExplicitContentFilterLevel>
            Roles = get |> Get.required Property.Roles (Decode.list Role.decoder)
            Emojis = get |> Get.required Property.Emojis (Decode.list Emoji.decoder)
            Features = get |> Get.required Property.Features (Decode.list GuildFeature.decoder)
            MfaLevel = get |> Get.required Property.MfaLevel Decode.Enum.int<MfaLevel>
            ApplicationId = get |> Get.nullable Property.ApplicationId Decode.string
            SystemChannelId = get |> Get.nullable Property.SystemChannelId Decode.string
            SystemChannelFlags = get |> Get.required Property.SystemChannelFlags Decode.int
            RulesChannelId = get |> Get.nullable Property.RulesChannelId Decode.string
            MaxPresences = get |> Get.optinull Property.MaxPresences Decode.int
            MaxMembers = get |> Get.optional Property.MaxMembers Decode.int
            VanityUrlCode = get |> Get.nullable Property.VanityUrlCode Decode.string
            Description = get |> Get.nullable Property.Description Decode.string
            Banner = get |> Get.nullable Property.Banner Decode.string
            PremiumTier = get |> Get.required Property.PremiumTier Decode.Enum.int<GuildPremiumTier>
            PremiumSubscriptionCount = get |> Get.optional Property.PremiumSubscriptionCount Decode.int
            PreferredLocale = get |> Get.required Property.PreferredLocale Decode.string
            PublicUpdatesChannelId = get |> Get.nullable Property.PublicUpdatesChannelId Decode.string
            MaxVideoChannelUsers = get |> Get.optional Property.MaxVideoChannelUsers Decode.int
            MaxStageVideoChannelUsers = get |> Get.optional Property.MaxStageVideoChannelUsers Decode.int
            ApproximateMemberCount = get |> Get.optional Property.ApproximateMemberCount Decode.int
            ApproximatePresenceCount = get |> Get.optional Property.ApproximatePresenceCount Decode.int
            WelcomeScreen = get |> Get.optional Property.WelcomeScreen WelcomeScreen.decoder
            NsfwLevel = get |> Get.required Property.NsfwLevel Decode.Enum.int<NsfwLevel>
            Stickers = get |> Get.optional Property.Stickers (Decode.list Sticker.decoder)
            PremiumProgressBarEnabled = get |> Get.required Property.PremiumProgressBarEnabled Decode.bool
            SafetyAlertsChannelId = get |> Get.nullable Property.SafetyAlertsChannelId Decode.string
            IncidentsData = get |> Get.nullable Property.IncidentsData IncidentData.decoder
        }) path v

    let encoder (v: Guild) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.nullable Property.Icon Encode.string v.Icon
            |> Encode.optinull Property.IconHash Encode.string v.IconHash
            |> Encode.nullable Property.Splash Encode.string v.Splash
            |> Encode.nullable Property.DiscoverySplash Encode.string v.DiscoverySplash
            |> Encode.optional Property.Owner Encode.bool v.Owner
            |> Encode.required Property.OwnerId Encode.string v.OwnerId
            |> Encode.optional Property.Permissions Encode.string v.Permissions
            |> Encode.nullable Property.AfkChannelId Encode.string v.AfkChannelId
            |> Encode.required Property.AfkTimeout Encode.int v.AfkTimeout
            |> Encode.optional Property.WidgetEnabled Encode.bool v.WidgetEnabled
            |> Encode.optinull Property.WidgetChannelId Encode.string v.WidgetChannelId
            |> Encode.required Property.VerificationLevel Encode.Enum.int v.VerificationLevel
            |> Encode.required Property.DefaultMessageNotifications Encode.Enum.int v.DefaultMessageNotifications
            |> Encode.required Property.ExplicitContentFilter Encode.Enum.int v.ExplicitContentFilter
            |> Encode.required Property.Roles (List.map Role.encoder >> Encode.list) v.Roles
            |> Encode.required Property.Emojis (List.map Emoji.encoder >> Encode.list) v.Emojis
            |> Encode.required Property.Features (List.map GuildFeature.encoder >> Encode.list) v.Features
            |> Encode.required Property.MfaLevel Encode.Enum.int v.MfaLevel
            |> Encode.nullable Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.nullable Property.SystemChannelId Encode.string v.SystemChannelId
            |> Encode.required Property.SystemChannelFlags Encode.int v.SystemChannelFlags
            |> Encode.nullable Property.RulesChannelId Encode.string v.RulesChannelId
            |> Encode.optinull Property.MaxPresences Encode.int v.MaxPresences
            |> Encode.optional Property.MaxMembers Encode.int v.MaxMembers
            |> Encode.nullable Property.VanityUrlCode Encode.string v.VanityUrlCode
            |> Encode.nullable Property.Description Encode.string v.Description
            |> Encode.required Property.PremiumTier Encode.Enum.int v.PremiumTier
            |> Encode.optional Property.PremiumSubscriptionCount Encode.int v.PremiumSubscriptionCount
            |> Encode.required Property.PreferredLocale Encode.string v.PreferredLocale
            |> Encode.nullable Property.PublicUpdatesChannelId Encode.string v.PublicUpdatesChannelId
            |> Encode.optional Property.MaxVideoChannelUsers Encode.int v.MaxVideoChannelUsers
            |> Encode.optional Property.MaxStageVideoChannelUsers Encode.int v.MaxStageVideoChannelUsers
            |> Encode.optional Property.ApproximateMemberCount Encode.int v.ApproximateMemberCount
            |> Encode.optional Property.ApproximatePresenceCount Encode.int v.ApproximatePresenceCount
            |> Encode.optional Property.WelcomeScreen WelcomeScreen.encoder v.WelcomeScreen
            |> Encode.required Property.NsfwLevel Encode.Enum.int v.NsfwLevel
            |> Encode.optional Property.Stickers (List.map Sticker.encoder >> Encode.list) v.Stickers
            |> Encode.required Property.PremiumProgressBarEnabled Encode.bool v.PremiumProgressBarEnabled
            |> Encode.nullable Property.SafetyAlertsChannelId Encode.string v.SafetyAlertsChannelId
            |> Encode.nullable Property.IncidentsData IncidentData.encoder v.IncidentsData
        )

    module Partial =
        let decoder path v =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Name = get |> Get.optional Property.Name Decode.string
                Icon = get |> Get.optinull Property.Icon Decode.string
                IconHash = get |> Get.optinull Property.IconHash Decode.string
                Splash = get |> Get.optinull Property.Splash Decode.string
                DiscoverySplash = get |> Get.optinull Property.DiscoverySplash Decode.string
                Owner = get |> Get.optional Property.Owner Decode.bool
                OwnerId = get |> Get.optional Property.OwnerId Decode.string
                Permissions = get |> Get.optional Property.Permissions Decode.string
                AfkChannelId = get |> Get.optinull Property.AfkChannelId Decode.string
                AfkTimeout = get |> Get.optional Property.AfkTimeout Decode.int
                WidgetEnabled = get |> Get.optional Property.WidgetEnabled Decode.bool
                WidgetChannelId = get |> Get.optinull Property.WidgetChannelId Decode.string
                VerificationLevel = get |> Get.optional Property.VerificationLevel Decode.Enum.int<VerificationLevel>
                DefaultMessageNotifications = get |> Get.optional Property.DefaultMessageNotifications Decode.Enum.int<MessageNotificationLevel>
                ExplicitContentFilter = get |> Get.optional Property.ExplicitContentFilter Decode.Enum.int<ExplicitContentFilterLevel>
                Roles = get |> Get.optional Property.Roles (Decode.list Role.decoder)
                Emojis = get |> Get.optional Property.Emojis (Decode.list Emoji.decoder)
                Features = get |> Get.optional Property.Features (Decode.list GuildFeature.decoder)
                MfaLevel = get |> Get.optional Property.MfaLevel Decode.Enum.int<MfaLevel>
                ApplicationId = get |> Get.optinull Property.ApplicationId Decode.string
                SystemChannelId = get |> Get.optinull Property.SystemChannelId Decode.string
                SystemChannelFlags = get |> Get.optional Property.SystemChannelFlags Decode.int
                RulesChannelId = get |> Get.optinull Property.RulesChannelId Decode.string
                MaxPresences = get |> Get.optinull Property.MaxPresences Decode.int
                MaxMembers = get |> Get.optional Property.MaxMembers Decode.int
                VanityUrlCode = get |> Get.optinull Property.VanityUrlCode Decode.string
                Description = get |> Get.optinull Property.Description Decode.string
                Banner = get |> Get.optinull Property.Banner Decode.string
                PremiumTier = get |> Get.optional Property.PremiumTier Decode.Enum.int<GuildPremiumTier>
                PremiumSubscriptionCount = get |> Get.optional Property.PremiumSubscriptionCount Decode.int
                PreferredLocale = get |> Get.optional Property.PreferredLocale Decode.string
                PublicUpdatesChannelId = get |> Get.optinull Property.PublicUpdatesChannelId Decode.string
                MaxVideoChannelUsers = get |> Get.optional Property.MaxVideoChannelUsers Decode.int
                MaxStageVideoChannelUsers = get |> Get.optional Property.MaxStageVideoChannelUsers Decode.int
                ApproximateMemberCount = get |> Get.optional Property.ApproximateMemberCount Decode.int
                ApproximatePresenceCount = get |> Get.optional Property.ApproximatePresenceCount Decode.int
                WelcomeScreen = get |> Get.optional Property.WelcomeScreen WelcomeScreen.decoder
                NsfwLevel = get |> Get.optional Property.NsfwLevel Decode.Enum.int<NsfwLevel>
                Stickers = get |> Get.optional Property.Stickers (Decode.list Sticker.decoder)
                PremiumProgressBarEnabled = get |> Get.optional Property.PremiumProgressBarEnabled Decode.bool
                SafetyAlertsChannelId = get |> Get.optinull Property.SafetyAlertsChannelId Decode.string
                IncidentsData = get |> Get.optinull Property.IncidentsData IncidentData.decoder
            }) path v

        let encoder (v: PartialGuild) =
            Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.optional Property.Name Encode.string v.Name
            |> Encode.optinull Property.Icon Encode.string v.Icon
            |> Encode.optinull Property.IconHash Encode.string v.IconHash
            |> Encode.optinull Property.Splash Encode.string v.Splash
            |> Encode.optinull Property.DiscoverySplash Encode.string v.DiscoverySplash
            |> Encode.optional Property.Owner Encode.bool v.Owner
            |> Encode.optional Property.OwnerId Encode.string v.OwnerId
            |> Encode.optional Property.Permissions Encode.string v.Permissions
            |> Encode.optinull Property.AfkChannelId Encode.string v.AfkChannelId
            |> Encode.optional Property.AfkTimeout Encode.int v.AfkTimeout
            |> Encode.optional Property.WidgetEnabled Encode.bool v.WidgetEnabled
            |> Encode.optinull Property.WidgetChannelId Encode.string v.WidgetChannelId
            |> Encode.optional Property.VerificationLevel Encode.Enum.int v.VerificationLevel
            |> Encode.optional Property.DefaultMessageNotifications Encode.Enum.int v.DefaultMessageNotifications
            |> Encode.optional Property.ExplicitContentFilter Encode.Enum.int v.ExplicitContentFilter
            |> Encode.optional Property.Roles (List.map Role.encoder >> Encode.list) v.Roles
            |> Encode.optional Property.Emojis (List.map Emoji.encoder >> Encode.list) v.Emojis
            |> Encode.optional Property.Features (List.map GuildFeature.encoder >> Encode.list) v.Features
            |> Encode.optional Property.MfaLevel Encode.Enum.int v.MfaLevel
            |> Encode.optinull Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optinull Property.SystemChannelId Encode.string v.SystemChannelId
            |> Encode.optional Property.SystemChannelFlags Encode.int v.SystemChannelFlags
            |> Encode.optinull Property.RulesChannelId Encode.string v.RulesChannelId
            |> Encode.optinull Property.MaxPresences Encode.int v.MaxPresences
            |> Encode.optional Property.MaxMembers Encode.int v.MaxMembers
            |> Encode.optinull Property.VanityUrlCode Encode.string v.VanityUrlCode
            |> Encode.optinull Property.Description Encode.string v.Description
            |> Encode.optional Property.PremiumTier Encode.Enum.int v.PremiumTier
            |> Encode.optional Property.PremiumSubscriptionCount Encode.int v.PremiumSubscriptionCount
            |> Encode.optional Property.PreferredLocale Encode.string v.PreferredLocale
            |> Encode.optinull Property.PublicUpdatesChannelId Encode.string v.PublicUpdatesChannelId
            |> Encode.optional Property.MaxVideoChannelUsers Encode.int v.MaxVideoChannelUsers
            |> Encode.optional Property.MaxStageVideoChannelUsers Encode.int v.MaxStageVideoChannelUsers
            |> Encode.optional Property.ApproximateMemberCount Encode.int v.ApproximateMemberCount
            |> Encode.optional Property.ApproximatePresenceCount Encode.int v.ApproximatePresenceCount
            |> Encode.optional Property.WelcomeScreen WelcomeScreen.encoder v.WelcomeScreen
            |> Encode.optional Property.NsfwLevel Encode.Enum.int v.NsfwLevel
            |> Encode.optional Property.Stickers (List.map Sticker.encoder >> Encode.list) v.Stickers
            |> Encode.optional Property.PremiumProgressBarEnabled Encode.bool v.PremiumProgressBarEnabled
            |> Encode.optinull Property.SafetyAlertsChannelId Encode.string v.SafetyAlertsChannelId
            |> Encode.optinull Property.IncidentsData IncidentData.encoder v.IncidentsData
            )

module UnavailableGuild =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Unavailable = "unavailable"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Unavailable = get |> Get.required Property.Unavailable Decode.bool
        }) path v

    let encoder (v: UnavailableGuild) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Unavailable Encode.bool v.Unavailable
        )

module GuildPreview =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Icon = "icon"
        let [<Literal>] Splash = "splash"
        let [<Literal>] DiscoverySplash = "discovery_splash"
        let [<Literal>] Emojis = "emojis"
        let [<Literal>] Features = "features"
        let [<Literal>] ApproximateMemberCount = "approximate_member_count"
        let [<Literal>] ApproximatePresenceCount = "approximate_presence_count"
        let [<Literal>] Description = "description"
        let [<Literal>] Stickers = "stickers"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Icon = get |> Get.nullable Property.Icon Decode.string
            Splash = get |> Get.nullable Property.Splash Decode.string
            DiscoverySplash = get |> Get.nullable Property.DiscoverySplash Decode.string
            Emojis = get |> Get.required Property.Emojis (Decode.list Emoji.decoder)
            Features = get |> Get.required Property.Features (Decode.list GuildFeature.decoder)
            ApproximateMemberCount = get |> Get.required Property.ApproximateMemberCount Decode.int
            ApproximatePresenceCount = get |> Get.required Property.ApproximatePresenceCount Decode.int
            Description = get |> Get.nullable Property.Description Decode.string
            Stickers = get |> Get.required Property.Stickers (Decode.list Sticker.decoder)
        }) path v

    let encoder (v: GuildPreview) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.nullable Property.Icon Encode.string v.Icon
            |> Encode.nullable Property.Splash Encode.string v.Splash
            |> Encode.nullable Property.DiscoverySplash Encode.string v.DiscoverySplash
            |> Encode.required Property.Emojis (List.map Emoji.encoder >> Encode.list) v.Emojis
            |> Encode.required Property.Features (List.map GuildFeature.encoder >> Encode.list) v.Features
            |> Encode.required Property.ApproximateMemberCount Encode.int v.ApproximateMemberCount
            |> Encode.required Property.ApproximatePresenceCount Encode.int v.ApproximatePresenceCount
            |> Encode.nullable Property.Description Encode.string v.Description
            |> Encode.required Property.Stickers (List.map Sticker.encoder >> Encode.list) v.Stickers
        )

module GuildWidgetSettings =
    module Property =
        let [<Literal>] Enabled = "enabled"
        let [<Literal>] ChannelId = "channel_id"

    let decoder path v =
        Decode.object (fun get -> {
            Enabled = get |> Get.required Property.Enabled Decode.bool
            ChannelId = get |> Get.nullable Property.ChannelId Decode.string
        }) path v

    let encoder (v: GuildWidgetSettings) =
        Encode.object ([]
            |> Encode.required Property.Enabled Encode.bool v.Enabled
            |> Encode.nullable Property.ChannelId Encode.string v.ChannelId
        )

module GuildWidget =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] InstantInvite = "instant_invite"
        let [<Literal>] Channels = "channels"
        let [<Literal>] Members = "members"
        let [<Literal>] PresenceCount = "presence_count"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            InstantInvite = get |> Get.nullable Property.InstantInvite Decode.string
            Channels = get |> Get.required Property.Channels (Decode.list Channel.Partial.decoder)
            Members = get |> Get.required Property.Members (Decode.list User.Partial.decoder)
            PresenceCount = get |> Get.required Property.PresenceCount Decode.int
        })

    let encoder (v: GuildWidget) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.nullable Property.InstantInvite Encode.string v.InstantInvite
            |> Encode.required Property.Channels (List.map Channel.Partial.encoder >> Encode.list) v.Channels
            |> Encode.required Property.Members (List.map User.Partial.encoder >> Encode.list) v.Members
            |> Encode.required Property.PresenceCount Encode.int v.PresenceCount
        )

module GuildMember =
    module Property =
        let [<Literal>] User = "user"
        let [<Literal>] Nick = "nick"
        let [<Literal>] Avatar = "avatar"
        let [<Literal>] Banner = "banner"
        let [<Literal>] Roles = "roles"
        let [<Literal>] JoinedAt = "joined_at"
        let [<Literal>] PremiumSince = "premium_since"
        let [<Literal>] Deaf = "deaf"
        let [<Literal>] Mute = "mute"
        let [<Literal>] Flags = "flags"
        let [<Literal>] Pending = "pending"
        let [<Literal>] Permissions = "permissions"
        let [<Literal>] CommunicationDisabledUntil = "communication_disabled_until"
        let [<Literal>] AvatarDecorationData = "avatar_decoration_data"

    let decoder path v =
        Decode.object (fun get -> {
            User = get |> Get.required Property.User User.decoder
            Nick = get |> Get.optinull Property.Nick Decode.string
            Avatar = get |> Get.optinull Property.Avatar Decode.string
            Banner = get |> Get.optinull Property.Banner Decode.string
            Roles = get |> Get.required Property.Roles (Decode.list Decode.string)
            JoinedAt = get |> Get.required Property.JoinedAt Decode.datetimeUtc
            PremiumSince = get |> Get.optinull Property.PremiumSince Decode.datetimeUtc
            Deaf = get |> Get.required Property.Deaf Decode.bool
            Mute = get |> Get.required Property.Mute Decode.bool
            Flags = get |> Get.required Property.Flags Decode.int
            Pending = get |> Get.optional Property.Pending Decode.bool
            Permissions = get |> Get.optional Property.Permissions Decode.string
            CommunicationDisabledUntil = get |> Get.optinull Property.CommunicationDisabledUntil Decode.datetimeUtc
            AvatarDecorationData = get |> Get.optinull Property.AvatarDecorationData AvatarDecorationData.decoder
        }) path v

    let encoder (v: GuildMember) =
        Encode.object ([]
            |> Encode.required Property.User User.encoder v.User
            |> Encode.optinull Property.Nick Encode.string v.Nick
            |> Encode.optinull Property.Avatar Encode.string v.Avatar
            |> Encode.optinull Property.Banner Encode.string v.Banner
            |> Encode.required Property.Roles (List.map Encode.string >> Encode.list) v.Roles
            |> Encode.required Property.JoinedAt Encode.datetime v.JoinedAt
            |> Encode.optinull Property.PremiumSince Encode.datetime v.PremiumSince
            |> Encode.required Property.Deaf Encode.bool v.Deaf
            |> Encode.required Property.Mute Encode.bool v.Mute
            |> Encode.required Property.Flags Encode.int v.Flags
            |> Encode.optional Property.Pending Encode.bool v.Pending
            |> Encode.optional Property.Permissions Encode.string v.Permissions
            |> Encode.optinull Property.CommunicationDisabledUntil Encode.datetime v.CommunicationDisabledUntil
            |> Encode.optinull Property.AvatarDecorationData AvatarDecorationData.encoder v.AvatarDecorationData
        )

    module Partial =
        let decoder path v =
            Decode.object (fun get -> {
                User = get |> Get.optional Property.User User.decoder
                Nick = get |> Get.optinull Property.Nick Decode.string
                Avatar = get |> Get.optinull Property.Avatar Decode.string
                Banner = get |> Get.optinull Property.Banner Decode.string
                Roles = get |> Get.optional Property.Roles (Decode.list Decode.string)
                JoinedAt = get |> Get.optional Property.JoinedAt Decode.datetimeUtc
                PremiumSince = get |> Get.optinull Property.PremiumSince Decode.datetimeUtc
                Deaf = get |> Get.optional Property.Deaf Decode.bool
                Mute = get |> Get.optional Property.Mute Decode.bool
                Flags = get |> Get.optional Property.Flags Decode.int
                Pending = get |> Get.optional Property.Pending Decode.bool
                Permissions = get |> Get.optional Property.Permissions Decode.string
                CommunicationDisabledUntil = get |> Get.optinull Property.CommunicationDisabledUntil Decode.datetimeUtc
                AvatarDecorationData = get |> Get.optinull Property.AvatarDecorationData AvatarDecorationData.decoder
            }) path v

        let encoder (v: PartialGuildMember) =
            Encode.object ([]
                |> Encode.optional Property.User User.encoder v.User
                |> Encode.optinull Property.Nick Encode.string v.Nick
                |> Encode.optinull Property.Avatar Encode.string v.Avatar
                |> Encode.optinull Property.Banner Encode.string v.Banner
                |> Encode.optional Property.Roles (List.map Encode.string >> Encode.list) v.Roles
                |> Encode.optional Property.JoinedAt Encode.datetime v.JoinedAt
                |> Encode.optinull Property.PremiumSince Encode.datetime v.PremiumSince
                |> Encode.optional Property.Deaf Encode.bool v.Deaf
                |> Encode.optional Property.Mute Encode.bool v.Mute
                |> Encode.optional Property.Flags Encode.int v.Flags
                |> Encode.optional Property.Pending Encode.bool v.Pending
                |> Encode.optional Property.Permissions Encode.string v.Permissions
                |> Encode.optinull Property.CommunicationDisabledUntil Encode.datetime v.CommunicationDisabledUntil
                |> Encode.optinull Property.AvatarDecorationData AvatarDecorationData.encoder v.AvatarDecorationData
            )

module Integration =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Type = "type"
        let [<Literal>] Enabled = "enabled"
        let [<Literal>] Syncing = "syncing"
        let [<Literal>] RoleId = "role_id"
        let [<Literal>] EnableEmoticons = "enable_emoticons"
        let [<Literal>] ExpireBehavior = "expire_behavior"
        let [<Literal>] ExpireGracePeriod = "expire_grace_period"
        let [<Literal>] User = "user"
        let [<Literal>] Account = "account"
        let [<Literal>] SyncedAt = "synced_at"
        let [<Literal>] SubscriberCount = "subscriber_count"
        let [<Literal>] Revoked = "revoked"
        let [<Literal>] Application = "application"
        let [<Literal>] Scopes = "scopes"

    let decoder path v =
        Decode.object (fun get -> {
            Integration.Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Type = get |> Get.required Property.Type GuildIntegrationType.decoder
            Enabled = get |> Get.required Property.Enabled Decode.bool
            Syncing = get |> Get.optional Property.Syncing Decode.bool
            RoleId = get |> Get.optional Property.RoleId Decode.string
            EnableEmoticons = get |> Get.optional Property.EnableEmoticons Decode.bool
            ExpireBehavior = get |> Get.optional Property.ExpireBehavior Decode.Enum.int<IntegrationExpireBehavior>
            ExpireGracePeriod = get |> Get.optional Property.ExpireGracePeriod Decode.int
            User = get |> Get.optional Property.User User.decoder
            Account = get |> Get.required Property.Account IntegrationAccount.decoder
            SyncedAt = get |> Get.optional Property.SyncedAt Decode.datetimeUtc
            SubscriberCount = get |> Get.optional Property.SubscriberCount Decode.int
            Revoked = get |> Get.optional Property.Revoked Decode.bool
            Application = get |> Get.optional Property.Application IntegrationApplication.decoder
            Scopes = get |> Get.optional Property.Scopes (Decode.list OAuthScope.decoder)
        }) path v

    let encoder (v: Integration) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Type GuildIntegrationType.encoder v.Type
            |> Encode.required Property.Enabled Encode.bool v.Enabled
            |> Encode.optional Property.Syncing Encode.bool v.Syncing
            |> Encode.optional Property.RoleId Encode.string v.RoleId
            |> Encode.optional Property.EnableEmoticons Encode.bool v.EnableEmoticons
            |> Encode.optional Property.ExpireBehavior Encode.Enum.int v.ExpireBehavior
            |> Encode.optional Property.ExpireGracePeriod Encode.int v.ExpireGracePeriod
            |> Encode.optional Property.User User.encoder v.User
            |> Encode.required Property.Account IntegrationAccount.encoder v.Account
            |> Encode.optional Property.SyncedAt Encode.datetime v.SyncedAt
            |> Encode.optional Property.SubscriberCount Encode.int v.SubscriberCount
            |> Encode.optional Property.Revoked Encode.bool v.Revoked
            |> Encode.optional Property.Application IntegrationApplication.encoder v.Application
            |> Encode.optional Property.Scopes (List.map OAuthScope.encoder >> Encode.list) v.Scopes
        )

    module Partial =
        let decoder path v =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Name = get |> Get.optional Property.Name Decode.string
                Type = get |> Get.optional Property.Type GuildIntegrationType.decoder
                Enabled = get |> Get.optional Property.Enabled Decode.bool
                Syncing = get |> Get.optional Property.Syncing Decode.bool
                RoleId = get |> Get.optional Property.RoleId Decode.string
                EnableEmoticons = get |> Get.optional Property.EnableEmoticons Decode.bool
                ExpireBehavior = get |> Get.optional Property.ExpireBehavior Decode.Enum.int<IntegrationExpireBehavior>
                ExpireGracePeriod = get |> Get.optional Property.ExpireGracePeriod Decode.int
                User = get |> Get.optional Property.User User.decoder
                Account = get |> Get.optional Property.Account IntegrationAccount.decoder
                SyncedAt = get |> Get.optional Property.SyncedAt Decode.datetimeUtc
                SubscriberCount = get |> Get.optional Property.SubscriberCount Decode.int
                Revoked = get |> Get.optional Property.Revoked Decode.bool
                Application = get |> Get.optional Property.Application IntegrationApplication.decoder
                Scopes = get |> Get.optional Property.Scopes (Decode.list OAuthScope.decoder)
            }) path v

        let encoder (v: PartialIntegration) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.Name Encode.string v.Name
                |> Encode.optional Property.Type GuildIntegrationType.encoder v.Type
                |> Encode.optional Property.Enabled Encode.bool v.Enabled
                |> Encode.optional Property.Syncing Encode.bool v.Syncing
                |> Encode.optional Property.RoleId Encode.string v.RoleId
                |> Encode.optional Property.EnableEmoticons Encode.bool v.EnableEmoticons
                |> Encode.optional Property.ExpireBehavior Encode.Enum.int v.ExpireBehavior
                |> Encode.optional Property.ExpireGracePeriod Encode.int v.ExpireGracePeriod
                |> Encode.optional Property.User User.encoder v.User
                |> Encode.optional Property.Account IntegrationAccount.encoder v.Account
                |> Encode.optional Property.SyncedAt Encode.datetime v.SyncedAt
                |> Encode.optional Property.SubscriberCount Encode.int v.SubscriberCount
                |> Encode.optional Property.Revoked Encode.bool v.Revoked
                |> Encode.optional Property.Application IntegrationApplication.encoder v.Application
                |> Encode.optional Property.Scopes (List.map OAuthScope.encoder >> Encode.list) v.Scopes
            )

module IntegrationAccount =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
        }) path v

    let encoder (v: IntegrationAccount) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
        )

module IntegrationApplication =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Icon = "icon"
        let [<Literal>] Description = "description"
        let [<Literal>] Summary = "summary"
        let [<Literal>] Bot = "bot"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Icon = get |> Get.nullable Property.Icon Decode.string
            Description = get |> Get.required Property.Description Decode.string
            Bot = get |> Get.optional Property.Bot User.decoder
        }) path v

    let encoder (v: IntegrationApplication) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.nullable Property.Icon Encode.string v.Icon
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optional Property.Bot User.encoder v.Bot
        )

module Ban =
    module Property =
        let [<Literal>] Reason = "reason"
        let [<Literal>] User = "user"

    let decoder path v =
        Decode.object (fun get -> {
            Reason = get |> Get.nullable Property.Reason Decode.string
            User = get |> Get.required Property.User User.decoder
        }) path v
    let encoder (v: Ban) =
        Encode.object ([]
            |> Encode.nullable Property.Reason Encode.string v.Reason
            |> Encode.required Property.User User.encoder v.User
        )

module WelcomeScreen =
    module Property =
        let [<Literal>] Description = "description"
        let [<Literal>] WelcomeChannels = "welcome_channels"

    let decoder path v =
        Decode.object (fun get -> {
            Description = get |> Get.nullable Property.Description Decode.string
            WelcomeChannels = get |> Get.required Property.WelcomeChannels (Decode.list WelcomeScreenChannel.decoder)
        }) path v

    let encoder (v: WelcomeScreen) =
        Encode.object ([]
            |> Encode.nullable Property.Description Encode.string v.Description
            |> Encode.required Property.WelcomeChannels (List.map WelcomeScreenChannel.encoder >> Encode.list) v.WelcomeChannels
        )

module WelcomeScreenChannel =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Description = "description"
        let [<Literal>] EmojiId = "emoji_id"
        let [<Literal>] EmojiName = "emoji_name"

    let decoder path v =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            Description = get |> Get.required Property.Description Decode.string
            EmojiId = get |> Get.nullable Property.EmojiId Decode.string
            EmojiName = get |> Get.nullable Property.EmojiName Decode.string
        }) path v

    let encoder (v: WelcomeScreenChannel) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.nullable Property.EmojiId Encode.string v.EmojiId
            |> Encode.nullable Property.EmojiName Encode.string v.EmojiName
        )

module GuildOnboarding =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Prompts = "prompts"
        let [<Literal>] DefaultChannelIds = "default_channel_ids"
        let [<Literal>] Enabled = "enabled"
        let [<Literal>] Mode = "mode"

    let decoder path v =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Prompts = get |> Get.required Property.Prompts (Decode.list GuildOnboardingPrompt.decoder)
            DefaultChannelIds = get |> Get.required Property.DefaultChannelIds (Decode.list Decode.string)
            Enabled = get |> Get.required Property.Enabled Decode.bool
            Mode = get |> Get.required Property.Mode Decode.Enum.int<OnboardingMode>
        }) path v

    let encoder (v: GuildOnboarding) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Prompts (List.map GuildOnboardingPrompt.encoder >> Encode.list) v.Prompts
            |> Encode.required Property.DefaultChannelIds (List.map Encode.string >> Encode.list) v.DefaultChannelIds
            |> Encode.required Property.Enabled Encode.bool v.Enabled
            |> Encode.required Property.Mode Encode.Enum.int v.Mode
        )

module GuildOnboardingPrompt =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] Options = "options"
        let [<Literal>] Title = "title"
        let [<Literal>] SingleSelect = "single_select"
        let [<Literal>] Required = "required"
        let [<Literal>] InOnboarding = "in_onboarding"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<OnboardingPromptType>
            Options = get |> Get.required Property.Options (Decode.list GuildOnboardingPromptOption.decoder)
            Title = get |> Get.required Property.Title Decode.string
            SingleSelect = get |> Get.required Property.SingleSelect Decode.bool
            Required = get |> Get.required Property.Required Decode.bool
            InOnboarding = get |> Get.required Property.InOnboarding Decode.bool
        }) path v

    let encoder (v: GuildOnboardingPrompt) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Options (List.map GuildOnboardingPromptOption.encoder >> Encode.list) v.Options
            |> Encode.required Property.Title Encode.string v.Title
            |> Encode.required Property.SingleSelect Encode.bool v.SingleSelect
            |> Encode.required Property.Required Encode.bool v.Required
            |> Encode.required Property.InOnboarding Encode.bool v.InOnboarding
        )

module GuildOnboardingPromptOption =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] ChannelIds = "channel_ids"
        let [<Literal>] RoleIds = "role_ids"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] EmojiId = "emoji_id"
        let [<Literal>] EmojiName = "emoji_name"
        let [<Literal>] EmojiAnimated = "emoji_animated"
        let [<Literal>] Title = "title"
        let [<Literal>] Description = "description"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            ChannelIds = get |> Get.required Property.ChannelIds (Decode.list Decode.string)
            RoleIds = get |> Get.required Property.RoleIds (Decode.list Decode.string)
            Emoji = get |> Get.optional Property.Emoji Emoji.decoder
            EmojiId = get |> Get.optional Property.EmojiId Decode.string
            EmojiName = get |> Get.optional Property.EmojiName Decode.string
            EmojiAnimated = get |> Get.optional Property.EmojiAnimated Decode.bool
            Title = get |> Get.required Property.Title Decode.string
            Description = get |> Get.nullable Property.Description Decode.string
        }) path v

    let encoder (v: GuildOnboardingPromptOption) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.ChannelIds (List.map Encode.string >> Encode.list) v.ChannelIds
            |> Encode.required Property.RoleIds (List.map Encode.string >> Encode.list) v.RoleIds
            |> Encode.optional Property.Emoji Emoji.encoder v.Emoji
            |> Encode.optional Property.EmojiId Encode.string v.EmojiId
            |> Encode.optional Property.EmojiName Encode.string v.EmojiName
            |> Encode.optional Property.EmojiAnimated Encode.bool v.EmojiAnimated
            |> Encode.required Property.Title Encode.string v.Title
            |> Encode.nullable Property.Description Encode.string v.Description
        )

module IncidentsData =
    module Property =
        let [<Literal>] InvitesDisabledUntil = "invites_disabled_until"
        let [<Literal>] DmsDisabledUntil = "dms_disabled_until"
        let [<Literal>] DmSpamDetectedAt = "dm_spam_detected_at"
        let [<Literal>] RaidDetectedAt = "raid_detected_at"

    let decoder path v =
        Decode.object (fun get -> {
            InvitesDisabledUntil = get |> Get.nullable Property.InvitesDisabledUntil Decode.datetimeUtc
            DmsDisabledUntil = get |> Get.nullable Property.DmsDisabledUntil Decode.datetimeUtc
            DmSpamDetectedAt = get |> Get.optinull Property.DmSpamDetectedAt Decode.datetimeUtc
            RaidDetectedAt = get |> Get.optinull Property.RaidDetectedAt Decode.datetimeUtc
        }) path v

    let encoder (v: IncidentsData) =
        Encode.object ([]
            |> Encode.nullable Property.InvitesDisabledUntil Encode.datetime v.InvitesDisabledUntil
            |> Encode.nullable Property.DmsDisabledUntil Encode.datetime v.DmsDisabledUntil
            |> Encode.optinull Property.DmSpamDetectedAt Encode.datetime v.DmSpamDetectedAt
            |> Encode.optinull Property.RaidDetectedAt Encode.datetime v.RaidDetectedAt
        )

module GuildScheduledEvent =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] CreatorId = "creator_id"
        let [<Literal>] Name = "name"
        let [<Literal>] Description = "description"
        let [<Literal>] ScheduledStartTime = "scheduled_start_time"
        let [<Literal>] ScheduledEndTime = "scheduled_end_time"
        let [<Literal>] PrivacyLevel = "privacy_level"
        let [<Literal>] Status = "status"
        let [<Literal>] EntityType = "entity_type"
        let [<Literal>] EntityId = "entity_id"
        let [<Literal>] EntityMetadata = "entity_metadata"
        let [<Literal>] Creator = "creator"
        let [<Literal>] UserCount = "user_count"
        let [<Literal>] Image = "image"
        let [<Literal>] RecurrenceRule = "recurrence_rule"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            ChannelId = get |> Get.nullable Property.ChannelId Decode.string
            CreatorId = get |> Get.optinull Property.CreatorId Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Description = get |> Get.optinull Property.Description Decode.string
            ScheduledStartTime = get |> Get.required Property.ScheduledStartTime Decode.datetimeUtc
            ScheduledEndTime = get |> Get.nullable Property.ScheduledEndTime Decode.datetimeUtc
            PrivacyLevel = get |> Get.required Property.PrivacyLevel Decode.Enum.int<PrivacyLevel>
            Status = get |> Get.required Property.Status Decode.Enum.int<EventStatus>
            EntityType = get |> Get.required Property.EntityType Decode.Enum.int<ScheduledEntityType>
            EntityId = get |> Get.nullable Property.EntityId Decode.string
            EntityMetadata = get |> Get.nullable Property.EntityMetadata EntityMetadata.decoder
            Creator = get |> Get.optional Property.Creator User.decoder
            UserCount = get |> Get.optional Property.UserCount Decode.int
            Image = get |> Get.optinull Property.Image Decode.string
            RecurrenceRule = get |> Get.nullable Property.RecurrenceRule RecurrenceRule.decoder
        }) path v

    let encoder (v: GuildScheduledEvent) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.nullable Property.ChannelId Encode.string v.ChannelId
            |> Encode.optinull Property.CreatorId Encode.string v.CreatorId
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optinull Property.Description Encode.string v.Description
            |> Encode.required Property.ScheduledStartTime Encode.datetime v.ScheduledStartTime
            |> Encode.nullable Property.ScheduledEndTime Encode.datetime v.ScheduledEndTime
            |> Encode.required Property.PrivacyLevel Encode.Enum.int v.PrivacyLevel
            |> Encode.required Property.Status Encode.Enum.int v.Status
            |> Encode.required Property.EntityType Encode.Enum.int v.EntityType
            |> Encode.nullable Property.EntityId Encode.string v.EntityId
            |> Encode.nullable Property.EntityMetadata EntityMetadata.encoder v.EntityMetadata
            |> Encode.optional Property.Creator User.encoder v.Creator
            |> Encode.optional Property.UserCount Encode.int v.UserCount
            |> Encode.optinull Property.Image Encode.string v.Image
            |> Encode.nullable Property.RecurrenceRule RecurrenceRule.encoder v.RecurrenceRule
        )

module EntityMetadata =
    module Property =
        let [<Literal>] Location = "location"

    let decoder path v =
        Decode.object (fun get -> {
            Location = get |> Get.optional Property.Location Decode.string
        }) path v

    let encoder (v: EntityMetadata) =
        Encode.object ([]
            |> Encode.optional Property.Location Encode.string v.Location
        )

module GuildScheduledEventUser =
    module Property =
        let [<Literal>] GuildScheduledEventId = "guild_scheduled_event_id"
        let [<Literal>] User = "user"
        let [<Literal>] Member = "member"

    let decoder path v =
        Decode.object (fun get -> {
            GuildScheduledEventId = get |> Get.required Property.GuildScheduledEventId Decode.string
            User = get |> Get.required Property.User User.decoder
            Member = get |> Get.optional Property.Member GuildMember.decoder
        }) path v

    let encoder (v: GuildScheduledEventUser) =
        Encode.object ([]
            |> Encode.required Property.GuildScheduledEventId Encode.string v.GuildScheduledEventId
            |> Encode.required Property.User User.encoder v.User
            |> Encode.optional Property.Member GuildMember.encoder v.Member
        )

module RecurrenceRule =
    module Property =
        let [<Literal>] Start = "start"
        let [<Literal>] End = "end"
        let [<Literal>] Frequency = "frequency"
        let [<Literal>] Interval = "interval"
        let [<Literal>] ByWeekday = "by_weekday"
        let [<Literal>] ByWeekend = "by_weekend"
        let [<Literal>] ByMonth = "by_month"
        let [<Literal>] ByMonthDay = "by_month_day"
        let [<Literal>] ByYearDay = "by_year_day"
        let [<Literal>] Count = "count"

    let decoder path v =
        Decode.object (fun get -> {
            Start = get |> Get.required Property.Start Decode.datetimeUtc
            End = get |> Get.nullable Property.End Decode.datetimeUtc
            Frequency = get |> Get.required Property.Frequency Decode.Enum.int<RecurrenceRuleFrequency>
            Interval = get |> Get.required Property.Interval Decode.int
            ByWeekday = get |> Get.nullable Property.ByWeekday (Decode.list Decode.Enum.int<RecurrenceRuleWeekday>)
            ByWeekend = get |> Get.nullable Property.ByWeekend (Decode.list RecurrenceRuleNWeekday.decoder)
            ByMonth = get |> Get.nullable Property.ByMonth (Decode.list Decode.Enum.int<RecurrenceRuleMonth>)
            ByMonthDay = get |> Get.nullable Property.ByMonthDay (Decode.list Decode.int)
            ByYearDay = get |> Get.nullable Property.ByYearDay (Decode.list Decode.int)
            Count = get |> Get.nullable Property.Count Decode.int
        }) path v

    let encoder (v: RecurrenceRule) =
        Encode.object ([]
            |> Encode.required Property.Start Encode.datetime v.Start
            |> Encode.nullable Property.End Encode.datetime v.End
            |> Encode.required Property.Frequency Encode.Enum.int v.Frequency
            |> Encode.required Property.Interval Encode.int v.Interval
            |> Encode.nullable Property.ByWeekday (List.map Encode.Enum.int >> Encode.list) v.ByWeekday
            |> Encode.nullable Property.ByWeekend (List.map RecurrenceRuleNWeekday.encoder >> Encode.list) v.ByWeekend
            |> Encode.nullable Property.ByMonth (List.map Encode.Enum.int >> Encode.list) v.ByMonth
            |> Encode.nullable Property.ByMonthDay (List.map Encode.int >> Encode.list) v.ByMonthDay
            |> Encode.nullable Property.ByYearDay (List.map Encode.int >> Encode.list) v.ByYearDay
            |> Encode.nullable Property.Count Encode.int v.Count
        )

module RecurrenceRuleNWeekday =
    module Property =
        let [<Literal>] N = "n"
        let [<Literal>] Day = "day"

    let decoder path v =
        Decode.object (fun get -> {
            N = get |> Get.required Property.N Decode.int
            Day = get |> Get.required Property.Day Decode.Enum.int<RecurrenceRuleWeekday>
        }) path v

    let encoder (v: RecurrenceRuleNWeekday) =
        Encode.object ([]
            |> Encode.required Property.N Encode.int v.N
            |> Encode.required Property.Day Encode.Enum.int v.Day
        )
