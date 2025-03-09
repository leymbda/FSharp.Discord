namespace rec FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Thoth.Json.Net

module ErrorResponse =
    module Property =
        let [<Literal>] Code = "code"
        let [<Literal>] Message = "message"
        let [<Literal>] Errors = "errors"

    let decoder path v =
        Decode.object (fun get -> {
            Code = get |> Get.required Property.Code Decode.Enum.int<JsonErrorCode>
            Message = get |> Get.required Property.Message Decode.string
            Errors = get |> Get.required Property.Errors (Decode.dict Decode.string)
        }) path v

    let encoder (v: ErrorResponse) =
        Encode.object ([]
            |> Encode.required Property.Code Encode.Enum.int v.Code
            |> Encode.required Property.Message Encode.string v.Message
            |> Encode.required Property.Errors (Encode.mapv Encode.string) v.Errors
        )

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
            Id = get |> Get.required Property.Id Decode.string
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            Data = get |> Get.optional Property.Data InteractionData.decoder
            Guild = get |> Get.optional Property.Guild Guild.Partial.decoder
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Channel = get |> Get.optional Property.Channel Channel.Partial.decoder
            ChannelId = get |> Get.optional Property.ChannelId Decode.string
            Member = get |> Get.optional Property.Member GuildMember.decoder
            User = get |> Get.optional Property.User User.decoder
            Token = get |> Get.required Property.Token Decode.string
            Version = get |> Get.required Property.Version Decode.int
            Message = get |> Get.optional Property.Message Message.decoder
            AppPermissions = get |> Get.required Property.AppPermissions Decode.string
            Locale = get |> Get.optional Property.Locale Decode.string
            GuildLocale = get |> Get.optional Property.GuildLocale Decode.string
            Entitlements = get |> Get.required Property.Entitlements (Decode.list Entitlement.decoder)
            AuthorizingIntegrationOwners = get |> Get.required Property.AuthorizingIntegrationOwners (Decode.mapkv ApplicationIntegrationType.fromString ApplicationIntegrationTypeConfiguration.decoder)
            Context = get |> Get.optional Property.Context Decode.Enum.int<InteractionContextType>
        }) path v

    let encoder (v: Interaction) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.Data InteractionData.encoder v.Data
            |> Encode.optional Property.Guild Guild.Partial.encoder v.Guild
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Channel Channel.Partial.encoder v.Channel
            |> Encode.optional Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.Member GuildMember.encoder v.Member
            |> Encode.optional Property.User User.encoder v.User
            |> Encode.required Property.Token Encode.string v.Token
            |> Encode.required Property.Version Encode.int v.Version
            |> Encode.optional Property.Message Message.encoder v.Message
            |> Encode.required Property.AppPermissions Encode.string v.AppPermissions
            |> Encode.optional Property.Locale Encode.string v.Locale
            |> Encode.optional Property.GuildLocale Encode.string v.GuildLocale
            |> Encode.required Property.Entitlements (List.map Entitlement.encoder >> Encode.list) v.Entitlements
            |> Encode.required Property.AuthorizingIntegrationOwners (Encode.mapkv ApplicationIntegrationType.toString ApplicationIntegrationTypeConfiguration.encoder) v.AuthorizingIntegrationOwners
            |> Encode.optional Property.Context Encode.Enum.int v.Context
        )

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
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandType>
            Resolved = get |> Get.optional Property.Resolved ResolvedData.decoder
            Options = get |> Get.optional Property.Options (Decode.list ApplicationCommandInteractionDataOption.decoder)
            GuildId = get |> Get.optional Property.GuildId Decode.string
            TargetId = get |> Get.optional Property.TargetId Decode.string
        }) path v

    let encoder (v: ApplicationCommandData) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.Resolved ResolvedData.encoder v.Resolved
            |> Encode.optional Property.Options (List.map ApplicationCommandInteractionDataOption.encoder >> Encode.list) v.Options
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.TargetId Encode.string v.TargetId
        )

module MessageComponentData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] ComponentType = "component_type"
        let [<Literal>] Values = "values"
        let [<Literal>] Resolved = "resolved"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get |> Get.required Property.CustomId Decode.string
            ComponentType = get |> Get.required Property.ComponentType Decode.Enum.int<ComponentType>
            Values = get |> Get.optional Property.Values (Decode.list SelectMenuOption.decoder)
            Resolved = get |> Get.optional Property.Resolved ResolvedData.decoder
        }) path v

    let encoder (v: MessageComponentData) =
        Encode.object ([]
            |> Encode.required Property.CustomId Encode.string v.CustomId
            |> Encode.required Property.ComponentType Encode.Enum.int v.ComponentType
            |> Encode.optional Property.Values (List.map SelectMenuOption.encoder >> Encode.list) v.Values
            |> Encode.optional Property.Resolved ResolvedData.encoder v.Resolved
        )

module ModalSubmitData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Components = "components"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get |> Get.required Property.CustomId Decode.string
            Components = get |> Get.required Property.Components (Decode.list Component.decoder)
        }) path v

    let encoder (v: ModalSubmitData) =
        Encode.object ([]
            |> Encode.required Property.CustomId Encode.string v.CustomId
            |> Encode.required Property.Components (List.map Component.encoder >> Encode.list) v.Components
        )

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
            Users = get |> Get.optional Property.Users (Decode.dict User.decoder)
            Members = get |> Get.optional Property.Members (Decode.dict GuildMember.Partial.decoder)
            Roles = get |> Get.optional Property.Roles (Decode.dict Role.decoder)
            Channels = get |> Get.optional Property.Channels (Decode.dict Channel.Partial.decoder)
            Messages = get |> Get.optional Property.Messages (Decode.dict Message.Partial.decoder)
            Attachments = get |> Get.optional Property.Attachments (Decode.dict Attachment.decoder)
        }) path v

    let encoder (v: ResolvedData) =
        Encode.object ([]
            |> Encode.optional Property.Users (Encode.mapv User.encoder) v.Users
            |> Encode.optional Property.Members (Encode.mapv GuildMember.Partial.encoder) v.Members
            |> Encode.optional Property.Roles (Encode.mapv Role.encoder) v.Roles
            |> Encode.optional Property.Channels (Encode.mapv Channel.Partial.encoder) v.Channels
            |> Encode.optional Property.Messages (Encode.mapv Message.Partial.encoder) v.Messages
            |> Encode.optional Property.Attachments (Encode.mapv Attachment.encoder) v.Attachments
        )

module ApplicationCommandInteractionDataOption =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Type = "type"
        let [<Literal>] Value = "value"
        let [<Literal>] Options = "options"
        let [<Literal>] Focused = "focused"

    let decoder path v =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandOptionType>
            Value = get |> Get.optional Property.Value ApplicationCommandInteractionDataOptionValue.decoder
            Options = get |> Get.optional Property.Options (Decode.list ApplicationCommandInteractionDataOption.decoder)
            Focused = get |> Get.optional Property.Focused Decode.bool
        }) path v

    let encoder (v: ApplicationCommandInteractionDataOption) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.Value ApplicationCommandInteractionDataOptionValue.encoder v.Value
            |> Encode.optional Property.Options (List.map ApplicationCommandInteractionDataOption.encoder >> Encode.list) v.Options
            |> Encode.optional Property.Focused Encode.bool v.Focused
        )

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
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            Name = get |> Get.required Property.Name Decode.string
            User = get |> Get.required Property.User User.decoder
            Member = get |> Get.optional Property.Member GuildMember.Partial.decoder
        }) path v

    let encoder (v: MessageInteraction) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.User User.encoder v.User
            |> Encode.optional Property.Member GuildMember.Partial.encoder v.Member
        )

module InteractionResponse =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Data = "data"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionCallbackType>
            Data = get |> Get.optional Property.Data InteractionCallbackData.decoder
        }) path v

    let encoder (v: InteractionResponse) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.Data InteractionCallbackData.encoder v.Data
        )

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
            Tts = get |> Get.optional Property.Tts Decode.bool
            Content = get |> Get.optional Property.Content Decode.string
            Embeds = get |> Get.optional Property.Embeds (Decode.list Embed.decoder)
            AllowedMentions = get |> Get.optional Property.AllowedMentions AllowedMentions.decoder
            Flags = get |> Get.optional Property.Flags Decode.int
            Components = get |> Get.optional Property.Components (Decode.list Component.decoder)
            Attachments = get |> Get.optional Property.Attachments (Decode.list Attachment.Partial.decoder)
            Poll = get |> Get.optional Property.Poll Poll.decoder
        }) path v

    let encoder (v: MessageInteractionCallbackData) =
        Encode.object ([]
            |> Encode.optional Property.Tts Encode.bool v.Tts
            |> Encode.optional Property.Content Encode.string v.Content
            |> Encode.optional Property.Embeds (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optional Property.AllowedMentions AllowedMentions.encoder v.AllowedMentions
            |> Encode.optional Property.Flags Encode.int v.Flags
            |> Encode.optional Property.Components (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optional Property.Attachments (List.map Attachment.Partial.encoder >> Encode.list) v.Attachments
            |> Encode.optional Property.Poll Poll.encoder v.Poll
        )

module AutocompleteInteractionCallbackData =
    module Property =
        let [<Literal>] Choices = "choices"

    let decoder path v =
        Decode.object (fun get -> {
            Choices = get |> Get.required Property.Choices (Decode.list ApplicationCommandOptionChoice.decoder)
        }) path v

    let encoder (v: AutocompleteInteractionCallbackData) =
        Encode.object ([]
            |> Encode.required Property.Choices (List.map ApplicationCommandOptionChoice.encoder >> Encode.list) v.Choices
        )

module ModalInteractionCallbackData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Title = "title"
        let [<Literal>] Components = "components"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get |> Get.required Property.CustomId Decode.string
            Title = get |> Get.required Property.Title Decode.string
            Components = get |> Get.required Property.Components (Decode.list Component.decoder)
        }) path v

    let encoder (v: ModalInteractionCallbackData) =
        Encode.object ([]
            |> Encode.required Property.CustomId Encode.string v.CustomId
            |> Encode.required Property.Title Encode.string v.Title
            |> Encode.required Property.Components (List.map Component.encoder >> Encode.list) v.Components
        )

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
            Interaction = get |> Get.required Property.Interaction InteractionCallback.decoder
            Resource = get |> Get.optional Property.Resource InteractionCallbackResource.decoder
        }) path v

    let encoder (v: InteractionCallbackResponse) =
        Encode.object ([]
            |> Encode.required Property.Interaction InteractionCallback.encoder v.Interaction
            |> Encode.optional Property.Resource InteractionCallbackResource.encoder v.Resource
        )

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
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            ActivityInstanceId = get |> Get.optional Property.ActivityInstanceId Decode.string
            ResponseMessageId = get |> Get.optional Property.ResponseMessageId Decode.string
            ResponseMessageLoading = get |> Get.optional Property.ResponseMessageLoading Decode.bool
            ResponseMessageEphemeral = get |> Get.optional Property.ResponseMessageEphemeral Decode.bool
        }) path v

    let encoder (v: InteractionCallback) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.ActivityInstanceId Encode.string v.ActivityInstanceId
            |> Encode.optional Property.ResponseMessageId Encode.string v.ResponseMessageId
            |> Encode.optional Property.ResponseMessageLoading Encode.bool v.ResponseMessageLoading
            |> Encode.optional Property.ResponseMessageEphemeral Encode.bool v.ResponseMessageEphemeral
        )

module InteractionCallbackResource =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] ActivityInstance = "activity_instance"
        let [<Literal>] Message = "message"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionCallbackType>
            ActivityInstance = get |> Get.optional Property.ActivityInstance InteractionCallbackActivityInstance.decoder
            Message = get |> Get.optional Property.Message Message.decoder
        }) path v

    let encoder (v: InteractionCallbackResource) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.ActivityInstance InteractionCallbackActivityInstance.encoder v.ActivityInstance
            |> Encode.optional Property.Message Message.encoder v.Message
        )

module InteractionCallbackActivityInstance =
    module Property =
        let [<Literal>] Id = "id"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
        }) path v

    let encoder (v: InteractionCallbackActivityInstance) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
        )

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
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.optional Property.Type Decode.Enum.int<ApplicationCommandType> |> Option.defaultValue ApplicationCommandType.CHAT_INPUT
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optinull Property.NameLocalizations (Decode.dict Decode.string)
            Description = get |> Get.required Property.Description Decode.string
            DescriptionLocalizations = get |> Get.optinull Property.DescriptionLocalizations (Decode.dict Decode.string)
            Options = get |> Get.optional Property.Options (Decode.list ApplicationCommandOption.decoder)
            DefaultMemberPermissions = get |> Get.nullable Property.DefaultMemberPermissions Decode.string
            Nsfw = get |> Get.optional Property.Nsfw Decode.bool |> Option.defaultValue false
            IntegrationTypes = get |> Get.optional Property.IntegrationTypes (Decode.list Decode.Enum.int<ApplicationIntegrationType>) |> Option.defaultValue [ApplicationIntegrationType.GUILD_INSTALL; ApplicationIntegrationType.USER_INSTALL]
            Contexts = get |> Get.optional Property.Contexts (Decode.list Decode.Enum.int<InteractionContextType>)
            Version = get |> Get.required Property.Version Decode.string
            Handler = get |> Get.optional Property.Handler Decode.Enum.int<ApplicationCommandHandlerType>
        }) path v

    let encoder (v: ApplicationCommand) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.optional Property.Type Encode.Enum.int (Some v.Type)
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optinull Property.NameLocalizations (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optinull Property.DescriptionLocalizations (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional Property.Options (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.nullable Property.DefaultMemberPermissions Encode.string v.DefaultMemberPermissions
            |> Encode.optional Property.Nsfw Encode.bool (Some v.Nsfw)
            |> Encode.required Property.IntegrationTypes (List.map Encode.Enum.int >> Encode.list) v.IntegrationTypes
            |> Encode.optional Property.Contexts (List.map Encode.Enum.int >> Encode.list) v.Contexts
            |> Encode.required Property.Version Encode.string v.Version
            |> Encode.optional Property.Handler Encode.Enum.int v.Handler
        )

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
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandOptionType>
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optinull Property.NameLocalizations (Decode.dict Decode.string)
            Description = get |> Get.required Property.Description Decode.string
            DescriptionLocalizations = get |> Get.optinull Property.DescriptionLocalizations (Decode.dict Decode.string)
            Required = get |> Get.optional Property.Required Decode.bool |> Option.defaultValue false
            Choices = get |> Get.optional Property.Choices (Decode.list ApplicationCommandOptionChoice.decoder)
            Options = get |> Get.optional Property.Options (Decode.list ApplicationCommandOption.decoder)
            ChannelTypes = get |> Get.optional Property.ChannelTypes (Decode.list Decode.Enum.int<ChannelType>)
            MinValue = get |> Get.optional Property.MinValue ApplicationCommandOptionMinValue.decoder
            MaxValue = get |> Get.optional Property.MaxValue ApplicationCommandOptionMaxValue.decoder
            MinLength = get |> Get.optional Property.MinLength Decode.int
            MaxLength = get |> Get.optional Property.MaxLength Decode.int
            Autocomplete = get |> Get.optional Property.Autocomplete Decode.bool
        }) path v

    let encoder (v: ApplicationCommandOption) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optinull Property.NameLocalizations (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optinull Property.DescriptionLocalizations (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional Property.Required Encode.bool (Some v.Required)
            |> Encode.optional Property.Choices (List.map ApplicationCommandOptionChoice.encoder >> Encode.list) v.Choices
            |> Encode.optional Property.Options (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.optional Property.ChannelTypes (List.map Encode.Enum.int >> Encode.list) v.ChannelTypes
            |> Encode.optional Property.MinValue ApplicationCommandOptionMinValue.encoder v.MinValue
            |> Encode.optional Property.MaxValue ApplicationCommandOptionMaxValue.encoder v.MaxValue
            |> Encode.optional Property.MinLength Encode.int v.MinLength
            |> Encode.optional Property.MaxLength Encode.int v.MaxLength
            |> Encode.optional Property.Autocomplete Encode.bool v.Autocomplete
        )

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

module ApplicationCommandOptionChoice =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] NameLocalizations = "name_localizations"
        let [<Literal>] Value = "value"

    let decoder path v =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optinull Property.NameLocalizations (Decode.dict Decode.string)
            Value = get |> Get.required Property.Value ApplicationCommandOptionChoiceValue.decoder
        }) path v

    let encoder (v: ApplicationCommandOptionChoice) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optinull Property.NameLocalizations (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.required Property.Value ApplicationCommandOptionChoiceValue.encoder v.Value
        )

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
            Id = get |> Get.required Property.Id Decode.string
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            Permissions = get |> Get.required Property.Permissions (Decode.list ApplicationCommandPermission.decoder)
        }) path v

    let encoder (v: GuildApplicationCommandPermissions) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Permissions (List.map ApplicationCommandPermission.encoder >> Encode.list) v.Permissions
        )

module ApplicationCommandPermission =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] Permission = "permission"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandPermissionType>
            Permission = get |> Get.required Property.Permission Decode.bool
        }) path v

    let encoder (v: ApplicationCommandPermission) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Permission Encode.bool v.Permission
        )

module ActionRow =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Components = "components"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<ComponentType>
            Components = get |> Get.required Property.Components (Decode.list Component.decoder)
        }) path v

    let encoder (v: ActionRow) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Components (List.map Component.encoder >> Encode.list) v.Components
        )

module Button =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Style = "style"
        let [<Literal>] Label = "label"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] SkuId = "sku_id"
        let [<Literal>] Url = "url"
        let [<Literal>] Disabled = "disabled"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<ComponentType>
            Style = get |> Get.required Property.Style Decode.Enum.int<ButtonStyle>
            Label = get |> Get.optional Property.Label Decode.string
            Emoji = get |> Get.optional Property.Emoji Emoji.Partial.decoder
            CustomId = get |> Get.optional Property.CustomId Decode.string
            SkuId = get |> Get.optional Property.SkuId Decode.string
            Url = get |> Get.optional Property.Url Decode.string
            Disabled = get |> Get.optional Property.Disabled Decode.bool |> Option.defaultValue false
        }) path v

    let encoder (v: Button) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Style Encode.Enum.int v.Style
            |> Encode.optional Property.Label Encode.string v.Label
            |> Encode.optional Property.Emoji Emoji.Partial.encoder v.Emoji
            |> Encode.optional Property.CustomId Encode.string v.CustomId
            |> Encode.optional Property.SkuId Encode.string v.SkuId
            |> Encode.optional Property.Url Encode.string v.Url
            |> Encode.optional Property.Disabled Encode.bool (Some v.Disabled)
        )

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
            Type = get |> Get.required Property.Type Decode.Enum.int<ComponentType>
            CustomId = get |> Get.required Property.CustomId Decode.string
            Options = get |> Get.optional Property.Options (Decode.list SelectMenuOption.decoder)
            ChannelTypes = get |> Get.optional Property.ChannelTypes (Decode.list Decode.Enum.int<ChannelType>)
            Placeholder = get |> Get.optional Property.Placeholder Decode.string
            DefaultValues = get |> Get.optional Property.DefaultValues (Decode.list SelectMenuDefaultValue.decoder)
            MinValues = get |> Get.optional Property.MinValues Decode.int |> Option.defaultValue 1
            MaxValues = get |> Get.optional Property.MaxValues Decode.int |> Option.defaultValue 1
            Disabled = get |> Get.optional Property.Disabled Decode.bool |> Option.defaultValue false
        }) path v

    let encoder (v: SelectMenu) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.CustomId Encode.string v.CustomId
            |> Encode.optional Property.Options (List.map SelectMenuOption.encoder >> Encode.list) v.Options
            |> Encode.optional Property.ChannelTypes (List.map Encode.Enum.int >> Encode.list) v.ChannelTypes
            |> Encode.optional Property.Placeholder Encode.string v.Placeholder
            |> Encode.optional Property.DefaultValues (List.map SelectMenuDefaultValue.encoder >> Encode.list) v.DefaultValues
            |> Encode.optional Property.MinValues Encode.int (Some v.MinValues)
            |> Encode.optional Property.MaxValues Encode.int (Some v.MaxValues)
            |> Encode.optional Property.Disabled Encode.bool (Some v.Disabled)
        )

module SelectMenuOption =
    module Property =
        let [<Literal>] Label = "label"
        let [<Literal>] Value = "value"
        let [<Literal>] Description = "description"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] Default = "default"

    let decoder path v =
        Decode.object (fun get -> {
            Label = get |> Get.required Property.Label Decode.string
            Value = get |> Get.required Property.Value Decode.string
            Description = get |> Get.optional Property.Description Decode.string
            Emoji = get |> Get.optional Property.Emoji Emoji.Partial.decoder
            Default = get |> Get.optional Property.Default Decode.bool
        }) path v

    let encoder (v: SelectMenuOption) =
        Encode.object ([]
            |> Encode.required Property.Label Encode.string v.Label
            |> Encode.required Property.Value Encode.string v.Value
            |> Encode.optional Property.Description Encode.string v.Description
            |> Encode.optional Property.Emoji Emoji.Partial.encoder v.Emoji
            |> Encode.optional Property.Default Encode.bool v.Default
        )

module SelectMenuDefaultValue =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type SelectMenuDefaultValueType.decoder
        }) path v

    let encoder (v: SelectMenuDefaultValue) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type SelectMenuDefaultValueType.encoder v.Type
        )

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
            Type = get |> Get.required Property.Type Decode.Enum.int<ComponentType>
            CustomId = get |> Get.required Property.CustomId Decode.string
            Style = get |> Get.required Property.Style Decode.Enum.int<TextInputStyle>
            Label = get |> Get.required Property.Label Decode.string
            MinLength = get |> Get.optional Property.MinLength Decode.int
            MaxLength = get |> Get.optional Property.MaxLength Decode.int
            Required = get |> Get.optional Property.Required Decode.bool |> Option.defaultValue true
            Value = get |> Get.optional Property.Value Decode.string
            Placeholder = get |> Get.optional Property.Placeholder Decode.string
        }) path v

    let encoder (v: TextInput) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.CustomId Encode.string v.CustomId
            |> Encode.required Property.Style Encode.Enum.int v.Style
            |> Encode.required Property.Label Encode.string v.Label
            |> Encode.optional Property.MinLength Encode.int v.MinLength
            |> Encode.optional Property.MaxLength Encode.int v.MaxLength
            |> Encode.optional Property.Required Encode.bool (Some v.Required)
            |> Encode.optional Property.Value Encode.string v.Value
            |> Encode.optional Property.Placeholder Encode.string v.Placeholder
        )

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
            Total = get |> Get.required Property.Total Decode.int
            Remaining = get |> Get.required Property.Remaining Decode.int
            ResetAfter = get |> Get.required Property.ResetAfter Decode.int
            MaxConcurrency = get |> Get.required Property.MaxConcurrency Decode.int
        }) path v

    let encoder (v: SessionStartLimit) =
        Encode.object ([]
            |> Encode.required Property.Total Encode.int v.Total
            |> Encode.required Property.Remaining Encode.int v.Remaining
            |> Encode.required Property.ResetAfter Encode.int v.ResetAfter
            |> Encode.required Property.MaxConcurrency Encode.int v.MaxConcurrency
        )

module IdentifyConnectionProperties =
    module Property =
        let [<Literal>] OperatingSystem = "os"
        let [<Literal>] Browser = "browser"
        let [<Literal>] Device = "device"

    let decoder path v =
        Decode.object (fun get -> {
            OperatingSystem = get |> Get.required Property.OperatingSystem Decode.string
            Browser = get |> Get.required Property.Browser Decode.string
            Device = get |> Get.required Property.Device Decode.string
        }) path v

    let encoder (v: IdentifyConnectionProperties) =
        Encode.object ([]
            |> Encode.required Property.OperatingSystem Encode.string v.OperatingSystem
            |> Encode.required Property.Browser Encode.string v.Browser
            |> Encode.required Property.Device Encode.string v.Device
        )

module ClientStatus =
    module Property =
        let [<Literal>] Desktop = "desktop"
        let [<Literal>] Mobile = "mobile"
        let [<Literal>] Web = "web"

    let decoder path v =
        Decode.object (fun get -> {
            Desktop = get |> Get.optional Property.Desktop ClientDeviceStatus.decoder
            Mobile = get |> Get.optional Property.Mobile ClientDeviceStatus.decoder
            Web = get |> Get.optional Property.Web ClientDeviceStatus.decoder
        }) path v

    let encoder (v: ClientStatus) =
        Encode.object ([]
            |> Encode.optional Property.Desktop ClientDeviceStatus.encoder v.Desktop
            |> Encode.optional Property.Mobile ClientDeviceStatus.encoder v.Mobile
            |> Encode.optional Property.Web ClientDeviceStatus.encoder v.Web
        )

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
            Name = get |> Get.required Property.Name Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ActivityType>
            Url = get |> Get.optinull Property.Url Decode.string
            CreatedAt = get |> Get.required Property.CreatedAt UnixTimestamp.decoder
            Timestamps = get |> Get.optional Property.Timestamps ActivityTimestamps.decoder
            ApplicationId = get |> Get.optional Property.ApplicationId Decode.string
            Details = get |> Get.optinull Property.Details Decode.string
            State = get |> Get.optinull Property.State Decode.string
            Emoji = get |> Get.optinull Property.Emoji ActivityEmoji.decoder
            Party = get |> Get.optional Property.Party ActivityParty.decoder
            Assets = get |> Get.optional Property.Assets ActivityAssets.decoder
            Secrets = get |> Get.optional Property.Secrets ActivitySecrets.decoder
            Instance = get |> Get.optional Property.Instance Decode.bool
            Flags = get |> Get.optional Property.Flags Decode.int
            Buttons = get |> Get.optional Property.Buttons (Decode.list ActivityButton.decoder)
        }) path v

    let encoder (v: Activity) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optinull Property.Url Encode.string v.Url
            |> Encode.required Property.CreatedAt UnixTimestamp.encoder v.CreatedAt
            |> Encode.optional Property.Timestamps ActivityTimestamps.encoder v.Timestamps
            |> Encode.optional Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optinull Property.Details Encode.string v.Details
            |> Encode.optinull Property.State Encode.string v.State
            |> Encode.optinull Property.Emoji ActivityEmoji.encoder v.Emoji
            |> Encode.optional Property.Party ActivityParty.encoder v.Party
            |> Encode.optional Property.Assets ActivityAssets.encoder v.Assets
            |> Encode.optional Property.Secrets ActivitySecrets.encoder v.Secrets
            |> Encode.optional Property.Instance Encode.bool v.Instance
            |> Encode.optional Property.Flags Encode.int v.Flags
            |> Encode.optional Property.Buttons (List.map ActivityButton.encoder >> Encode.list) v.Buttons
        )

module ActivityTimestamps =
    module Property =
        let [<Literal>] Start = "start"
        let [<Literal>] End = "end"

    let decoder path v =
        Decode.object (fun get -> {
            Start = get |> Get.optional Property.Start UnixTimestamp.decoder
            End = get |> Get.optional Property.End UnixTimestamp.decoder
        }) path v

    let encoder (v: ActivityTimestamps) =
        Encode.object ([]
            |> Encode.optional Property.Start UnixTimestamp.encoder v.Start
            |> Encode.optional Property.End UnixTimestamp.encoder v.End
        )

module ActivityEmoji =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Id = "id"
        let [<Literal>] Animated = "animated"

    let decoder path v =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            Id = get |> Get.optional Property.Id Decode.string
            Animated = get |> Get.optional Property.Animated Decode.bool
        }) path v

    let encoder (v: ActivityEmoji) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optional Property.Id Encode.string v.Id
            |> Encode.optional Property.Animated Encode.bool v.Animated
        )

module ActivityParty =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Size = "size"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.optional Property.Id Decode.string
            Size = get |> Get.optional Property.Size ActivityPartySize.decoder
        }) path v

    let encoder (v: ActivityParty) =
        Encode.object ([]
            |> Encode.optional Property.Id Encode.string v.Id
            |> Encode.optional Property.Size ActivityPartySize.encoder v.Size
        )

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
            LargeImage = get |> Get.optional Property.LargeImage Decode.string
            LargeText = get |> Get.optional Property.LargeText Decode.string
            SmallImage = get |> Get.optional Property.SmallImage Decode.string
            SmallText = get |> Get.optional Property.SmallText Decode.string
        }) path v

    let encoder (v: ActivityAssets) =
        Encode.object ([]
            |> Encode.optional Property.LargeImage Encode.string v.LargeImage
            |> Encode.optional Property.LargeText Encode.string v.LargeText
            |> Encode.optional Property.SmallImage Encode.string v.SmallImage
            |> Encode.optional Property.SmallText Encode.string v.SmallText
        )

module ActivitySecrets =
    module Property =
        let [<Literal>] Join = "join"
        let [<Literal>] Spectate = "spectate"
        let [<Literal>] Match = "match"

    let decoder path v =
        Decode.object (fun get -> {
            Join = get |> Get.optional Property.Join Decode.string
            Spectate = get |> Get.optional Property.Spectate Decode.string
            Match = get |> Get.optional Property.Match Decode.string
        }) path v

    let encoder (v: ActivitySecrets) =
        Encode.object ([]
            |> Encode.optional Property.Join Encode.string v.Join
            |> Encode.optional Property.Spectate Encode.string v.Spectate
            |> Encode.optional Property.Match Encode.string v.Match
        )

module ActivityButton =
    module Property =
        let [<Literal>] Label = "label"
        let [<Literal>] Url = "url"

    let decoder path v =
        Decode.object (fun get -> {
            Label = get |> Get.required Property.Label Decode.string
            Url = get |> Get.required Property.Url Decode.string
        }) path v

    let encoder (v: ActivityButton) =
        Encode.object ([]
            |> Encode.required Property.Label Encode.string v.Label
            |> Encode.required Property.Url Encode.string v.Url
        )

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
            OAuth2InstallParams = get |> Get.optional Property.OAuth2InstallParams InstallParams.decoder
        }) path v

    let encoder (v: ApplicationIntegrationTypeConfiguration) =
        Encode.object ([]
            |> Encode.optional Property.OAuth2InstallParams InstallParams.encoder v.OAuth2InstallParams
        )

module InstallParams =
    module Property =
        let [<Literal>] Scopes = "scopes"
        let [<Literal>] Permissions = "permissions"

    let decoder path v =
        Decode.object (fun get -> {
            Scopes = get |> Get.required Property.Scopes (Decode.list OAuthScope.decoder)
            Permissions = get |> Get.required Property.Permissions Decode.string
        }) path v

    let encoder (v: InstallParams) =
        Encode.object ([]
            |> Encode.required Property.Scopes (List.map OAuthScope.encoder >> Encode.list) v.Scopes
            |> Encode.required Property.Permissions Encode.string v.Permissions
        )

module ActivityInstance =
    module Property =
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] InstanceId = "instance_id"
        let [<Literal>] LaunchId = "launch_id"
        let [<Literal>] Location = "location"
        let [<Literal>] Users = "users"

    let decoder path v =
        Decode.object (fun get -> {
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            InstanceId = get |> Get.required Property.InstanceId Decode.string
            LaunchId = get |> Get.required Property.LaunchId Decode.string
            Location = get |> Get.required Property.Location ActivityLocation.decoder
            Users = get |> Get.required Property.Users (Decode.list Decode.string)
        }) path v

    let encoder (v: ActivityInstance) =
        Encode.object ([]
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.required Property.InstanceId Encode.string v.InstanceId
            |> Encode.required Property.LaunchId Encode.string v.LaunchId
            |> Encode.required Property.Location ActivityLocation.encoder v.Location
            |> Encode.required Property.Users (List.map Encode.string >> Encode.list) v.Users
        )

module ActivityLocation =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Kind = "kind"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Kind = get |> Get.required Property.Kind ActivityLocationKind.decoder
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.optinull Property.GuildId Decode.string
        }) path v

    let encoder (v: ActivityLocation) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Kind ActivityLocationKind.encoder v.Kind
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.optinull Property.GuildId Encode.string v.GuildId
        )

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
            ApplicationCommands = get |> Get.required Property.ApplicationCommands (Decode.list ApplicationCommand.decoder)
            AuditLogEntries = get |> Get.required Property.AuditLogEntries (Decode.list AuditLogEntry.decoder)
            AutoModerationRules = get |> Get.required Property.AutoModerationRules (Decode.list AutoModerationRule.decoder)
            GuildScheduledEvents = get |> Get.required Property.GuildScheduledEvents (Decode.list GuildScheduledEvent.decoder)
            Integrations = get |> Get.required Property.Integrations (Decode.list Integration.Partial.decoder)
            Threads = get |> Get.required Property.Threads (Decode.list Channel.decoder)
            Users = get |> Get.required Property.Users (Decode.list User.decoder)
            Webhooks = get |> Get.required Property.Webhooks (Decode.list Webhook.decoder)
        }) path v

    let encoder (v: AuditLog) =
        Encode.object ([]
            |> Encode.required Property.ApplicationCommands (List.map ApplicationCommand.encoder >> Encode.list) v.ApplicationCommands
            |> Encode.required Property.AuditLogEntries (List.map AuditLogEntry.encoder >> Encode.list) v.AuditLogEntries
            |> Encode.required Property.AutoModerationRules (List.map AutoModerationRule.encoder >> Encode.list) v.AutoModerationRules
            |> Encode.required Property.GuildScheduledEvents (List.map GuildScheduledEvent.encoder >> Encode.list) v.GuildScheduledEvents
            |> Encode.required Property.Integrations (List.map Integration.Partial.encoder >> Encode.list) v.Integrations
            |> Encode.required Property.Threads (List.map Channel.encoder >> Encode.list) v.Threads
            |> Encode.required Property.Users (List.map User.encoder >> Encode.list) v.Users
            |> Encode.required Property.Webhooks (List.map Webhook.encoder >> Encode.list) v.Webhooks
        )

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
            IncidentsData = get |> Get.nullable Property.IncidentsData IncidentsData.decoder
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
            |> Encode.nullable Property.IncidentsData IncidentsData.encoder v.IncidentsData
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
                IncidentsData = get |> Get.optinull Property.IncidentsData IncidentsData.decoder
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
            |> Encode.optinull Property.IncidentsData IncidentsData.encoder v.IncidentsData
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
            User = get |> Get.optional Property.User User.decoder
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
            |> Encode.optional Property.User User.encoder v.User
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

module GuildTemplate =
    module Property =
        let [<Literal>] Code = "code"
        let [<Literal>] Name = "name"
        let [<Literal>] Description = "description"
        let [<Literal>] UsageCount = "usage_count"
        let [<Literal>] CreatorId = "creator_id"
        let [<Literal>] Creator = "creator"
        let [<Literal>] CreatedAt = "created_at"
        let [<Literal>] UpdatedAt = "updated_at"
        let [<Literal>] SourceGuildId = "source_guild_id"
        let [<Literal>] SerializedSourceGuild = "serialized_source_guild"
        let [<Literal>] IsDirty = "is_dirty"

    let decoder path v =
        Decode.object (fun get -> {
            Code = get |> Get.required Property.Code Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Description = get |> Get.nullable Property.Description Decode.string
            UsageCount = get |> Get.required Property.UsageCount Decode.int
            CreatorId = get |> Get.required Property.CreatorId Decode.string
            Creator = get |> Get.required Property.Creator User.decoder
            CreatedAt = get |> Get.required Property.CreatedAt Decode.datetimeUtc
            UpdatedAt = get |> Get.required Property.UpdatedAt Decode.datetimeUtc
            SourceGuildId = get |> Get.required Property.SourceGuildId Decode.string
            SerializedSourceGuild = get |> Get.required Property.SerializedSourceGuild Guild.Partial.decoder
            IsDirty = get |> Get.nullable Property.IsDirty Decode.bool
        }) path v

    let encoder (v: GuildTemplate) =
        Encode.object ([]
            |> Encode.required Property.Code Encode.string v.Code
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.nullable Property.Description Encode.string v.Description
            |> Encode.required Property.UsageCount Encode.int v.UsageCount
            |> Encode.required Property.CreatorId Encode.string v.CreatorId
            |> Encode.required Property.Creator User.encoder v.Creator
            |> Encode.required Property.CreatedAt Encode.datetime v.CreatedAt
            |> Encode.required Property.UpdatedAt Encode.datetime v.UpdatedAt
            |> Encode.required Property.SourceGuildId Encode.string v.SourceGuildId
            |> Encode.required Property.SerializedSourceGuild Guild.Partial.encoder v.SerializedSourceGuild
            |> Encode.nullable Property.IsDirty Encode.bool v.IsDirty
        )

module Invite =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Code = "code"
        let [<Literal>] Guild = "guild"
        let [<Literal>] Channel = "channel"
        let [<Literal>] Inviter = "inviter"
        let [<Literal>] TargetType = "target_type"
        let [<Literal>] TargetUser = "target_user"
        let [<Literal>] TargetApplication = "target_application"
        let [<Literal>] ApproximatePresenceCount = "approximate_presence_count"
        let [<Literal>] ApproximateMemberCount = "approximate_member_count"
        let [<Literal>] ExpiresAt = "expires_at"
        let [<Literal>] StageInstance = "stage_instance"
        let [<Literal>] GuildScheduledEvent = "guild_scheduled_event"

    let decoder path v =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<InviteType>
            Code = get |> Get.required Property.Code Decode.string
            Guild = get |> Get.optional Property.Guild Guild.Partial.decoder
            Channel = get |> Get.nullable Property.Channel Channel.Partial.decoder
            Inviter = get |> Get.optional Property.Inviter User.Partial.decoder
            TargetType = get |> Get.optional Property.TargetType Decode.Enum.int<InviteTargetType>
            TargetUser = get |> Get.optional Property.TargetUser User.decoder
            TargetApplication = get |> Get.optional Property.TargetApplication Application.Partial.decoder
            ApproximatePresenceCount = get |> Get.optional Property.ApproximatePresenceCount Decode.int
            ApproximateMemberCount = get |> Get.optional Property.ApproximateMemberCount Decode.int
            ExpiresAt = get |> Get.optinull Property.ExpiresAt Decode.datetimeUtc
            GuildScheduledEvent = get |> Get.optional Property.GuildScheduledEvent GuildScheduledEvent.decoder
        }) path v

    let internal encodeProperties (v: Invite) =
        []
        |> Encode.required Property.Type Encode.Enum.int v.Type
        |> Encode.required Property.Code Encode.string v.Code
        |> Encode.optional Property.Guild Guild.Partial.encoder v.Guild
        |> Encode.nullable Property.Channel Channel.Partial.encoder v.Channel
        |> Encode.optional Property.Inviter User.Partial.encoder v.Inviter
        |> Encode.optional Property.TargetType Encode.Enum.int v.TargetType
        |> Encode.optional Property.TargetUser User.encoder v.TargetUser
        |> Encode.optional Property.TargetApplication Application.Partial.encoder v.TargetApplication
        |> Encode.optional Property.ApproximatePresenceCount Encode.int v.ApproximatePresenceCount
        |> Encode.optional Property.ApproximateMemberCount Encode.int v.ApproximateMemberCount
        |> Encode.optinull Property.ExpiresAt Encode.datetime v.ExpiresAt
        |> Encode.optional Property.GuildScheduledEvent GuildScheduledEvent.encoder v.GuildScheduledEvent

    let encoder (v: Invite) =
        Encode.object (encodeProperties v)

module InviteMetadata =
    module Property =
        let [<Literal>] Uses = "uses"
        let [<Literal>] MaxUses = "max_uses"
        let [<Literal>] MaxAge = "max_age"
        let [<Literal>] Temporary = "temporary"
        let [<Literal>] CreatedAt = "created_at"

    let decoder path v =
        Decode.object (fun get -> {
            Uses = get |> Get.required Property.Uses Decode.int
            MaxUses = get |> Get.required Property.MaxUses Decode.int
            MaxAge = get |> Get.required Property.MaxAge Decode.int
            Temporary = get |> Get.required Property.Temporary Decode.bool
            CreatedAt = get |> Get.required Property.CreatedAt Decode.datetimeUtc
        }) path v

    let internal encodeProperties (v: InviteMetadata) =
        []
        |> Encode.required Property.Uses Encode.int v.Uses
        |> Encode.required Property.MaxUses Encode.int v.MaxUses
        |> Encode.required Property.MaxAge Encode.int v.MaxAge
        |> Encode.required Property.Temporary Encode.bool v.Temporary
        |> Encode.required Property.CreatedAt Encode.datetime v.CreatedAt

    let encoder (v: InviteMetadata) =
        Encode.object (encodeProperties v)

module InviteWithMetadata =
    let decoder path v =
        Decode.object (fun get -> {
            Invite = get |> Get.extract Invite.decoder
            Metadata = get |> Get.extract InviteMetadata.decoder
        }) path v

    let encoder (v: InviteWithMetadata) =
        Encode.object (Invite.encodeProperties v.Invite @ InviteMetadata.encodeProperties v.Metadata)

module Message =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Author = "author"
        let [<Literal>] Content = "content"
        let [<Literal>] Timestamp = "timestamp"
        let [<Literal>] EditedTimestamp = "edited_timestamp"
        let [<Literal>] Tts = "tts"
        let [<Literal>] MentionEveryone = "mention_everyone"
        let [<Literal>] Mentions = "mentions"
        let [<Literal>] MentionRoles = "mention_roles"
        let [<Literal>] MentionChannels = "mention_channels"
        let [<Literal>] Attachments = "attachments"
        let [<Literal>] Embeds = "embeds"
        let [<Literal>] Reactions = "reactions"
        let [<Literal>] Nonce = "nonce"
        let [<Literal>] Pinned = "pinned"
        let [<Literal>] WebhookId = "webhook_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Activity = "activity"
        let [<Literal>] Application = "application"
        let [<Literal>] Flags = "flags"
        let [<Literal>] MessageReference = "message_reference"
        let [<Literal>] MessageSnapshots = "message_snapshots"
        let [<Literal>] ReferencedMessage = "referenced_message"
        let [<Literal>] InteractionMetadata = "interaction_metadata"
        let [<Literal>] Interaction = "interaction"
        let [<Literal>] Thread = "thread"
        let [<Literal>] Components = "components"
        let [<Literal>] StickerItems = "sticker_items"
        let [<Literal>] Position = "position"
        let [<Literal>] RoleSubscriptionData = "role_subscription_data"
        let [<Literal>] Resolved = "resolved"
        let [<Literal>] Poll = "poll"
        let [<Literal>] Call = "call"

    let decoder: Decoder<Message> = raise (System.NotImplementedException())
    let encoder: Encoder<Message> = raise (System.NotImplementedException())

    module Partial =
        let decoder: Decoder<PartialMessage> = raise (System.NotImplementedException())
        let encoder: Encoder<PartialMessage> = raise (System.NotImplementedException())

module MessageActivity =
    module Property =
        let [<Literal>] Tyoe = "type"
        let [<Literal>] PartyId = "party_id"

    let decoder: Decoder<MessageActivity> = raise (System.NotImplementedException())
    let encoder: Encoder<MessageActivity> = raise (System.NotImplementedException())

module ApplicationCommandInteractionMetadata =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] User = "user"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] OriginalResponseMessageId = "original_response_message_id"
        let [<Literal>] TargetUser = "target_user"
        let [<Literal>] TargetMessageId = "target_message_id"

    let decoder: Decoder<ApplicationCommandInteractionMetadata> = raise (System.NotImplementedException())
    let encoder: Encoder<ApplicationCommandInteractionMetadata> = raise (System.NotImplementedException())

module MessageComponentInteractionMetadata =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] User = "user"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] OriginalResponseMessageId = "original_response_message_id"
        let [<Literal>] InteractedMessageId = "interacted_message_id"

    let decoder: Decoder<MessageComponentInteractionMetadata> = raise (System.NotImplementedException())
    let encoder: Encoder<MessageComponentInteractionMetadata> = raise (System.NotImplementedException())

module ModalSubmitInteractionMetadata =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] User = "user"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] OriginalResponseMessageId = "original_response_message_id"
        let [<Literal>] TriggeringInteractionMetadata = "triggering_interaction_metadata"

    let decoder: Decoder<MessageComponentInteractionMetadata> = raise (System.NotImplementedException())
    let encoder: Encoder<MessageComponentInteractionMetadata> = raise (System.NotImplementedException())

module MessageCall =
    module Property =
        let [<Literal>] Participants = "participants"
        let [<Literal>] EndedTimestamp = "ended_timestamp"

    let decoder: Decoder<MessageCall> = raise (System.NotImplementedException())
    let encoder: Encoder<MessageCall> = raise (System.NotImplementedException())

module MessageReference =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] FailIfNotExists = "fail_if_not_exists"

    let decoder: Decoder<MessageReference> = raise (System.NotImplementedException())
    let encoder: Encoder<MessageReference> = raise (System.NotImplementedException())

module MessageSnapshot =
    module Property =
        let [<Literal>] Message = "message"

    let decoder: Decoder<MessageSnapshot> = raise (System.NotImplementedException())
    let encoder: Encoder<MessageSnapshot> = raise (System.NotImplementedException())

module Reaction =
    module Property =
        let [<Literal>] Count = "count"
        let [<Literal>] CountDetails = "count_details"
        let [<Literal>] Me = "me"
        let [<Literal>] MeBurst = "me_burst"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] BurstColors = "burst_colors"

    let decoder: Decoder<Reaction> = raise (System.NotImplementedException())
    let encoder: Encoder<Reaction> = raise (System.NotImplementedException())

module ReactionCountDetails =
    module Property =
        let [<Literal>] Burst = "burst"
        let [<Literal>] Normal = "normal"

    let decoder: Decoder<ReactionCountDetails> = raise (System.NotImplementedException())
    let encoder: Encoder<ReactionCountDetails> = raise (System.NotImplementedException())

module Embed =
    module Property =
        let [<Literal>] Title = "title"
        let [<Literal>] Type = "type"
        let [<Literal>] Description = "description"
        let [<Literal>] Url = "url"
        let [<Literal>] Timestamp = "timestamp"
        let [<Literal>] Color = "color"
        let [<Literal>] Footer = "footer"
        let [<Literal>] Image = "image"
        let [<Literal>] Thumbnail = "thumbnail"
        let [<Literal>] Video = "video"
        let [<Literal>] Provider = "provider"
        let [<Literal>] Author = "author"
        let [<Literal>] Fields = "fields"

    let decoder: Decoder<Embed> = raise (System.NotImplementedException())
    let encoder: Encoder<Embed> = raise (System.NotImplementedException())

module EmbedThumbnail =
    module Property =
        let [<Literal>] Url = "url"
        let [<Literal>] ProxyUrl = "proxy_url"
        let [<Literal>] Height = "height"
        let [<Literal>] Width = "width"

    let decoder: Decoder<EmbedThumbnail> = raise (System.NotImplementedException())
    let encoder: Encoder<EmbedThumbnail> = raise (System.NotImplementedException())

module EmbedVideo =
    module Property =
        let [<Literal>] Url = "url"
        let [<Literal>] ProxyUrl = "proxy_url"
        let [<Literal>] Height = "height"
        let [<Literal>] Width = "width"

    let decoder: Decoder<EmbedVideo> = raise (System.NotImplementedException())
    let encoder: Encoder<EmbedVideo> = raise (System.NotImplementedException())

module EmbedImage =
    module Property =
        let [<Literal>] Url = "url"
        let [<Literal>] ProxyUrl = "proxy_url"
        let [<Literal>] Height = "height"
        let [<Literal>] Width = "width"

    let decoder: Decoder<EmbedImage> = raise (System.NotImplementedException())
    let encoder: Encoder<EmbedImage> = raise (System.NotImplementedException())

module EmbedProvider =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Url = "url"

    let decoder: Decoder<EmbedProvider> = raise (System.NotImplementedException())
    let encoder: Encoder<EmbedProvider> = raise (System.NotImplementedException())

module EmbedAuthor =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Url = "url"
        let [<Literal>] IconUrl = "icon_url"
        let [<Literal>] ProxyIconUrl = "proxy_icon_url"

    let decoder: Decoder<EmbedAuthor> = raise (System.NotImplementedException())
    let encoder: Encoder<EmbedAuthor> = raise (System.NotImplementedException())

module EmbedFooter =
    module Property =
        let [<Literal>] Text = "text"
        let [<Literal>] IconUrl = "icon_url"
        let [<Literal>] ProxyIconUrl = "proxy_icon_url"

    let decoder: Decoder<EmbedFooter> = raise (System.NotImplementedException())
    let encoder: Encoder<EmbedFooter> = raise (System.NotImplementedException())

module EmbedField =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Value = "value"
        let [<Literal>] Inline = "inline"

    let decoder: Decoder<EmbedField> = raise (System.NotImplementedException())
    let encoder: Encoder<EmbedField> = raise (System.NotImplementedException())

module Attachment =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Filename = "filename"
        let [<Literal>] Description = "description"
        let [<Literal>] ContentType = "content_type"
        let [<Literal>] Size = "size"
        let [<Literal>] Url = "url"
        let [<Literal>] ProxyUrl = "proxy_url"
        let [<Literal>] Height = "height"
        let [<Literal>] Width = "width"
        let [<Literal>] Ephemeral = "ephemeral"
        let [<Literal>] DurationSecs = "duration_secs"
        let [<Literal>] Waveform = "waveform"
        let [<Literal>] Flags = "flags"

    let decoder: Decoder<Attachment> = raise (System.NotImplementedException())
    let encoder: Encoder<Attachment> = raise (System.NotImplementedException())

    module Partial =
        let decoder: Decoder<PartialAttachment> = raise (System.NotImplementedException())
        let encoder: Encoder<PartialAttachment> = raise (System.NotImplementedException())

module ChannelMention =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Name = "name"

    let decoder: Decoder<ChannelMention> = raise (System.NotImplementedException())
    let encoder: Encoder<ChannelMention> = raise (System.NotImplementedException())

module AllowedMentions =
    module Property =
        let [<Literal>] Parse = "parse"
        let [<Literal>] Roles = "roles"
        let [<Literal>] Users = "users"
        let [<Literal>] RepliedUser = "replied_user"

    let decoder: Decoder<AllowedMentions> = raise (System.NotImplementedException())
    let encoder: Encoder<AllowedMentions> = raise (System.NotImplementedException())

module RoleSubscriptionData =
    module Property =
        let [<Literal>] RoleSubscriptionListingId = "role_subscription_listing_id"
        let [<Literal>] TierName = "tier_name"
        let [<Literal>] TotalMonthsSubscribed = "total_months_subscribed"
        let [<Literal>] IsRenewal = "is_renewal"

    let decoder: Decoder<RoleSubscriptionData> = raise (System.NotImplementedException())
    let encoder: Encoder<RoleSubscriptionData> = raise (System.NotImplementedException())

module Poll =
    module Property =
        let [<Literal>] Question = "question"
        let [<Literal>] Answers = "answers"
        let [<Literal>] Expiry = "expiry"
        let [<Literal>] AllowMultiselect = "allow_multiselect"
        let [<Literal>] LayoutType = "layout_type"
        let [<Literal>] Results = "results"

    let decoder: Decoder<Poll> = raise (System.NotImplementedException())
    let encoder: Encoder<Poll> = raise (System.NotImplementedException())

module PollMedia =
    module Property =
        let [<Literal>] Text = "text"
        let [<Literal>] Emoji = "emoji"

    let decoder: Decoder<PollMedia> = raise (System.NotImplementedException())
    let encoder: Encoder<PollMedia> = raise (System.NotImplementedException())

module PollAnswer =
    module Property =
        let [<Literal>] AnswerId = "answer_id"
        let [<Literal>] PollMedia = "poll_media"

    let decoder: Decoder<PollAnswer> = raise (System.NotImplementedException())
    let encoder: Encoder<PollAnswer> = raise (System.NotImplementedException())

module PollResults =
    module Property =
        let [<Literal>] IsFinalized = "is_finalized"
        let [<Literal>] AnswerCounts = "answer_counts"

    let decoder: Decoder<PollResults> = raise (System.NotImplementedException())
    let encoder: Encoder<PollResults> = raise (System.NotImplementedException())

module PollAnswerCount =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Count = "count"
        let [<Literal>] MeVoted = "me_voted"

    let decoder: Decoder<PollAnswerCount> = raise (System.NotImplementedException())
    let encoder: Encoder<PollAnswerCount> = raise (System.NotImplementedException())

module Sku =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Name = "name"
        let [<Literal>] Slug = "slug"
        let [<Literal>] Flags = "flags"

    let decoder: Decoder<Sku> = raise (System.NotImplementedException())
    let encoder: Encoder<Sku> = raise (System.NotImplementedException())

module SoundboardSound =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] SoundId = "sound_id"
        let [<Literal>] Volume = "volume"
        let [<Literal>] EmojiId = "emoji_id"
        let [<Literal>] EmojiName = "emoji_name"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Available = "available"
        let [<Literal>] User = "user"

    let decoder: Decoder<SoundboardSound> = raise (System.NotImplementedException())
    let encoder: Encoder<SoundboardSound> = raise (System.NotImplementedException())

module StageInstance =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Topic = "topic"
        let [<Literal>] PrivacyLevel = "privacy_level"
        let [<Literal>] DiscoverableEnabled = "discoverable_enabled"
        let [<Literal>] GuildScheduledEventId = "guild_scheduled_event_id"

    let decoder: Decoder<StageInstance> = raise (System.NotImplementedException())
    let encoder: Encoder<StageInstance> = raise (System.NotImplementedException())

module Sticker =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] PackId = "pack_id"
        let [<Literal>] Name = "name"
        let [<Literal>] Description = "description"
        let [<Literal>] Tags = "tags"
        let [<Literal>] Type = "type"
        let [<Literal>] FormatType = "format_type"
        let [<Literal>] Available = "available"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] User = "user"
        let [<Literal>] SortValue = "sort_value"

    let decoder: Decoder<Sticker> = raise (System.NotImplementedException())
    let encoder: Encoder<Sticker> = raise (System.NotImplementedException())

module StickerItem =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] FormatType = "format_type"

    let decoder: Decoder<StickerItem> = raise (System.NotImplementedException())
    let encoder: Encoder<StickerItem> = raise (System.NotImplementedException())

module StickerPack =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Stickers = "stickers"
        let [<Literal>] Name = "name"
        let [<Literal>] SkuId = "sku_id"
        let [<Literal>] CoverStickerId = "cover_sticker_id"
        let [<Literal>] Description = "description"
        let [<Literal>] BannerAssetId = "banner_asset_id"

    let decoder: Decoder<StickerPack> = raise (System.NotImplementedException())
    let encoder: Encoder<StickerPack> = raise (System.NotImplementedException())

module Subscription =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] SkuIds = "sku_ids"
        let [<Literal>] EntitlementIds = "entitlement_ids"
        let [<Literal>] RenewalSkuIds = "renewal_sku_ids"
        let [<Literal>] CurrentPeriodStart = "current_period_start"
        let [<Literal>] CurrentPeriodEnd = "current_period_end"
        let [<Literal>] Status = "status"
        let [<Literal>] CreatedAt = "created_at"
        let [<Literal>] Country = "country"

    let decoder: Decoder<Subscription> = raise (System.NotImplementedException())
    let encoder: Encoder<Subscription> = raise (System.NotImplementedException())

module User =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Username = "username"
        let [<Literal>] Discriminator = "discriminator"
        let [<Literal>] GlobalName = "global_name"
        let [<Literal>] Avatar = "avatar"
        let [<Literal>] Bot = "bot"
        let [<Literal>] System = "system"
        let [<Literal>] MfaEnabled = "mfa_enabled"
        let [<Literal>] Banner = "banner"
        let [<Literal>] AccentColor = "accent_color"
        let [<Literal>] Locale = "locale"
        let [<Literal>] Verified = "verified"
        let [<Literal>] Email = "email"
        let [<Literal>] Flags = "flags"
        let [<Literal>] PremiumType = "premium_type"
        let [<Literal>] PublicFlags = "public_flags"
        let [<Literal>] AvatarDecorationData = "avatar_decoration_data"

    let decoder: Decoder<User> = raise (System.NotImplementedException())
    let encoder: Encoder<User> = raise (System.NotImplementedException())

    module Partial =
        let decoder: Decoder<PartialUser> = raise (System.NotImplementedException())
        let encoder: Encoder<PartialUser> = raise (System.NotImplementedException())

module AvatarDecorationData =
    module Property =
        let [<Literal>] Asset = "asset"
        let [<Literal>] SkuId = "sku_id"

    let decoder: Decoder<AvatarDecorationData> = raise (System.NotImplementedException())
    let encoder: Encoder<AvatarDecorationData> = raise (System.NotImplementedException())

module Connection =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Type = "type"
        let [<Literal>] Revoked = "revoked"
        let [<Literal>] Integrations = "integrations"
        let [<Literal>] Verified = "verified"
        let [<Literal>] FriendSync = "friend_sync"
        let [<Literal>] ShowActivity = "show_activity"
        let [<Literal>] TwoWayLink = "two_way_link"
        let [<Literal>] Visibility = "visibility"

    let decoder: Decoder<Connection> = raise (System.NotImplementedException())
    let encoder: Encoder<Connection> = raise (System.NotImplementedException())

module ApplicationRoleConnection =
    module Property =
        let [<Literal>] PlatformName = "platform_name"
        let [<Literal>] PlatformUsername = "platform_username"
        let [<Literal>] Metadata = "metadata"

    let decoder: Decoder<ApplicationRoleConnection> = raise (System.NotImplementedException())
    let encoder: Encoder<ApplicationRoleConnection> = raise (System.NotImplementedException())

module VoiceState =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] Member = "member"
        let [<Literal>] SessionId = "session_id"
        let [<Literal>] Deaf = "deaf"
        let [<Literal>] Mute = "mute"
        let [<Literal>] SelfDeaf = "self_deaf"
        let [<Literal>] SelfMute = "self_mute"
        let [<Literal>] SelfStream = "self_stream"
        let [<Literal>] SelfVideo = "self_video"
        let [<Literal>] Suppress = "suppress"
        let [<Literal>] RequestToSpeakTimestamp = "request_to_speak_timestamp"

    let decoder: Decoder<VoiceState> = raise (System.NotImplementedException())
    let encoder: Encoder<VoiceState> = raise (System.NotImplementedException())

module VoiceRegion =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Optimal = "optimal"
        let [<Literal>] Deprecated = "deprecated"
        let [<Literal>] Custom = "custom"

    let decoder: Decoder<VoiceRegion> = raise (System.NotImplementedException())
    let encoder: Encoder<VoiceRegion> = raise (System.NotImplementedException())

module Webhook =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] WebhookType = "webhook_type"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] User = "user"
        let [<Literal>] Name = "name"
        let [<Literal>] Avatar = "avatar"
        let [<Literal>] Token = "token"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] SourceGuild = "source_guild"
        let [<Literal>] SourceChannel = "source_channel"
        let [<Literal>] Url = "url"

    let decoder: Decoder<Webhook> = raise (System.NotImplementedException())
    let encoder: Encoder<Webhook> = raise (System.NotImplementedException())

module Role =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Color = "color"
        let [<Literal>] Hoist = "hoist"
        let [<Literal>] Icon = "icon"
        let [<Literal>] UnicodeEmoji = "unicode_emoji"
        let [<Literal>] Position = "position"
        let [<Literal>] Permissions = "permissions"
        let [<Literal>] Managed = "managed"
        let [<Literal>] Mentionable = "mentionable"
        let [<Literal>] Tags = "tags"
        let [<Literal>] Flags = "flags"

    let decoder: Decoder<Role> = raise (System.NotImplementedException())
    let encoder: Encoder<Role> = raise (System.NotImplementedException())

module RoleTags =
    module Property =
        let [<Literal>] BotId = "bot_id"
        let [<Literal>] IntegrationId = "integration_id"
        let [<Literal>] PremiumSubscriber = "premium_subscriber"
        let [<Literal>] SubscriptionListingId = "subscription_listing_id"
        let [<Literal>] AvailableForPurchase = "available_for_purchase"
        let [<Literal>] GuildConnections = "guild_connections"

    // TODO: These 3 bool properties are the ones that use null or undefined as true or false

    let decoder: Decoder<RoleTags> = raise (System.NotImplementedException())
    let encoder: Encoder<RoleTags> = raise (System.NotImplementedException())

module RateLimitResponse =
    module Property =
        let [<Literal>] Message = "message"
        let [<Literal>] RetryAfter = "retry_after"
        let [<Literal>] Global = "global"
        let [<Literal>] Code = "code"

    let decoder: Decoder<RateLimitResponse> = raise (System.NotImplementedException())
    let encoder: Encoder<RateLimitResponse> = raise (System.NotImplementedException())

module Team =
    module Property =
        let [<Literal>] Icon = "icon"
        let [<Literal>] Id = "id"
        let [<Literal>] Members = "members"
        let [<Literal>] Name = "name"
        let [<Literal>] OwnerUserId = "owner_user_id"

    let decoder: Decoder<Team> = raise (System.NotImplementedException())
    let encoder: Encoder<Team> = raise (System.NotImplementedException())

module TeamMember =
    module Property =
        let [<Literal>] MembershipState = "membership_state"
        let [<Literal>] TeamId = "team_id"
        let [<Literal>] User = "user"
        let [<Literal>] Role = "role"

    let decoder: Decoder<TeamMember> = raise (System.NotImplementedException())
    let encoder: Encoder<TeamMember> = raise (System.NotImplementedException())
