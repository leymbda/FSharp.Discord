namespace rec FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Thoth.Json.Net

// This and other recursive references to the object(s) being defined will be checked for initialization-soundness at
// runtime through the use of a delayed reference. This is because you are defining one or more recursive objects,
// rather than recursive functions. This warning may be suppressed by using '#nowarn "40"' or '--nowarn:40'.
#nowarn "40"

module InteractionAuthor =
    let decoder: Decoder<InteractionAuthor> =
        Decode.oneOf [
            Decode.map InteractionAuthor.User (Decode.field Interaction.Property.User User.decoder)
            Decode.map InteractionAuthor.GuildMember (Decode.field Interaction.Property.Member GuildMember.decoder)
        ]

    let encodeProperties (v: InteractionAuthor) =
        match v with
        | InteractionAuthor.User u -> [] |> Encode.optional Interaction.Property.User User.encoder (Some u)
        | InteractionAuthor.GuildMember g -> [] |> Encode.optional Interaction.Property.Member GuildMember.encoder (Some g)
        
    let encoder (v: InteractionAuthor) =
        Encode.object (encodeProperties v)

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
        let [<Literal>] AttachmentSizeLimit = "attachment_size_limit"

    let decoder: Decoder<Interaction> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            Data = get |> Get.optional Property.Data InteractionData.decoder
            Guild = get |> Get.optional Property.Guild Guild.Partial.decoder
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Channel = get |> Get.optional Property.Channel Channel.Partial.decoder
            ChannelId = get |> Get.optional Property.ChannelId Decode.string
            Author = get |> Get.extract InteractionAuthor.decoder
            Token = get |> Get.required Property.Token Decode.string
            Version = get |> Get.required Property.Version Decode.int
            Message = get |> Get.optional Property.Message Message.decoder
            AppPermissions = get |> Get.required Property.AppPermissions Decode.bitfieldL<Permission>
            Locale = get |> Get.optional Property.Locale Decode.string
            GuildLocale = get |> Get.optional Property.GuildLocale Decode.string
            Entitlements = get |> Get.required Property.Entitlements (Decode.list Entitlement.decoder)
            AuthorizingIntegrationOwners = get |> Get.required Property.AuthorizingIntegrationOwners (Decode.mapkv (int >> enum<ApplicationIntegrationType> >> Some) Decode.string)
            Context = get |> Get.optional Property.Context Decode.Enum.int<InteractionContextType>
            AttachmentSizeLimit = get |> Get.required Property.AttachmentSizeLimit Decode.int
        })

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
            |> List.append (InteractionAuthor.encodeProperties v.Author)
            |> Encode.required Property.Token Encode.string v.Token
            |> Encode.required Property.Version Encode.int v.Version
            |> Encode.optional Property.Message Message.encoder v.Message
            |> Encode.required Property.AppPermissions Encode.bitfieldL v.AppPermissions
            |> Encode.optional Property.Locale Encode.string v.Locale
            |> Encode.optional Property.GuildLocale Encode.string v.GuildLocale
            |> Encode.required Property.Entitlements (List.map Entitlement.encoder >> Encode.list) v.Entitlements
            |> Encode.required Property.AuthorizingIntegrationOwners (Encode.mapkv (int >> string) Encode.string) v.AuthorizingIntegrationOwners
            |> Encode.optional Property.Context Encode.Enum.int v.Context
            |> Encode.required Property.AttachmentSizeLimit Encode.int v.AttachmentSizeLimit
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

    let decoder: Decoder<ApplicationCommandData> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandType>
            Resolved = get |> Get.optional Property.Resolved ResolvedData.decoder
            Options = get |> Get.optional Property.Options (Decode.list ApplicationCommandInteractionDataOption.decoder)
            GuildId = get |> Get.optional Property.GuildId Decode.string
            TargetId = get |> Get.optional Property.TargetId Decode.string
        })

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

    let decoder: Decoder<MessageComponentData> =
        Decode.object (fun get -> {
            CustomId = get |> Get.required Property.CustomId Decode.string
            ComponentType = get |> Get.required Property.ComponentType Decode.Enum.int<ComponentType>
            Values = get |> Get.optional Property.Values (Decode.list SelectMenuOption.decoder)
            Resolved = get |> Get.optional Property.Resolved ResolvedData.decoder
        })

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

    let decoder: Decoder<ModalSubmitData> =
        Decode.object (fun get -> {
            CustomId = get |> Get.required Property.CustomId Decode.string
            Components = get |> Get.required Property.Components (Decode.list Component.decoder)
        })

    let encoder (v: ModalSubmitData) =
        Encode.object ([]
            |> Encode.required Property.CustomId Encode.string v.CustomId
            |> Encode.required Property.Components (List.map Component.encoder >> Encode.list) v.Components
        )

module InteractionData =
    let decoder: Decoder<InteractionData> =
        Decode.oneOf [
            Decode.map InteractionData.APPLICATION_COMMAND ApplicationCommandData.decoder
            Decode.map InteractionData.MESSAGE_COMPONENT MessageComponentData.decoder
            Decode.map InteractionData.MODAL_SUBMIT ModalSubmitData.decoder
        ]

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

    let decoder: Decoder<ResolvedData> =
        Decode.object (fun get -> {
            Users = get |> Get.optional Property.Users (Decode.dict User.decoder)
            Members = get |> Get.optional Property.Members (Decode.dict GuildMember.Partial.decoder)
            Roles = get |> Get.optional Property.Roles (Decode.dict Role.decoder)
            Channels = get |> Get.optional Property.Channels (Decode.dict Channel.Partial.decoder)
            Messages = get |> Get.optional Property.Messages (Decode.dict Message.Partial.decoder)
            Attachments = get |> Get.optional Property.Attachments (Decode.dict Attachment.decoder)
        })

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

    let decoder: Decoder<ApplicationCommandInteractionDataOption> =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandOptionType>
            Value = get |> Get.optional Property.Value ApplicationCommandInteractionDataOptionValue.decoder
            Options = get |> Get.optional Property.Options (Decode.list decoder)
            Focused = get |> Get.optional Property.Focused Decode.bool
        })

    let encoder (v: ApplicationCommandInteractionDataOption) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.Value ApplicationCommandInteractionDataOptionValue.encoder v.Value
            |> Encode.optional Property.Options (List.map encoder >> Encode.list) v.Options
            |> Encode.optional Property.Focused Encode.bool v.Focused
        )

module ApplicationCommandInteractionDataOptionValue =
    let decoder: Decoder<ApplicationCommandInteractionDataOptionValue> =
        Decode.oneOf [
            Decode.map ApplicationCommandInteractionDataOptionValue.STRING Decode.string
            Decode.map ApplicationCommandInteractionDataOptionValue.INT Decode.int
            Decode.map ApplicationCommandInteractionDataOptionValue.DOUBLE Decode.float
            Decode.map ApplicationCommandInteractionDataOptionValue.BOOL Decode.bool
        ]

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

    let decoder: Decoder<MessageInteraction> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            Name = get |> Get.required Property.Name Decode.string
            User = get |> Get.required Property.User User.decoder
            Member = get |> Get.optional Property.Member GuildMember.Partial.decoder
        })

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

    let decoder: Decoder<InteractionResponse> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionCallbackType>
            Data = get |> Get.optional Property.Data InteractionCallbackData.decoder
        })

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

    let decoder: Decoder<MessageInteractionCallbackData> =
        Decode.object (fun get -> {
            Tts = get |> Get.optional Property.Tts Decode.bool
            Content = get |> Get.optional Property.Content Decode.string
            Embeds = get |> Get.optional Property.Embeds (Decode.list Embed.decoder)
            AllowedMentions = get |> Get.optional Property.AllowedMentions AllowedMentions.decoder
            Flags = get |> Get.optional Property.Flags Decode.bitfield
            Components = get |> Get.optional Property.Components (Decode.list Component.decoder)
            Attachments = get |> Get.optional Property.Attachments (Decode.list Attachment.Partial.decoder)
            Poll = get |> Get.optional Property.Poll Poll.decoder
        })

    let encoder (v: MessageInteractionCallbackData) =
        Encode.object ([]
            |> Encode.optional Property.Tts Encode.bool v.Tts
            |> Encode.optional Property.Content Encode.string v.Content
            |> Encode.optional Property.Embeds (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optional Property.AllowedMentions AllowedMentions.encoder v.AllowedMentions
            |> Encode.optional Property.Flags Encode.bitfield v.Flags
            |> Encode.optional Property.Components (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optional Property.Attachments (List.map Attachment.Partial.encoder >> Encode.list) v.Attachments
            |> Encode.optional Property.Poll Poll.encoder v.Poll
        )

module AutocompleteInteractionCallbackData =
    module Property =
        let [<Literal>] Choices = "choices"

    let decoder: Decoder<AutocompleteInteractionCallbackData> =
        Decode.object (fun get -> {
            Choices = get |> Get.required Property.Choices (Decode.list ApplicationCommandOptionChoice.decoder)
        })

    let encoder (v: AutocompleteInteractionCallbackData) =
        Encode.object ([]
            |> Encode.required Property.Choices (List.map ApplicationCommandOptionChoice.encoder >> Encode.list) v.Choices
        )

module ModalInteractionCallbackData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Title = "title"
        let [<Literal>] Components = "components"

    let decoder: Decoder<ModalInteractionCallbackData> =
        Decode.object (fun get -> {
            CustomId = get |> Get.required Property.CustomId Decode.string
            Title = get |> Get.required Property.Title Decode.string
            Components = get |> Get.required Property.Components (Decode.list Component.decoder)
        })

    let encoder (v: ModalInteractionCallbackData) =
        Encode.object ([]
            |> Encode.required Property.CustomId Encode.string v.CustomId
            |> Encode.required Property.Title Encode.string v.Title
            |> Encode.required Property.Components (List.map Component.encoder >> Encode.list) v.Components
        )

module InteractionCallbackData =
    let decoder: Decoder<InteractionCallbackData> =
        Decode.oneOf [
            Decode.map InteractionCallbackData.MESSAGE MessageInteractionCallbackData.decoder
            Decode.map InteractionCallbackData.AUTOCOMPLETE AutocompleteInteractionCallbackData.decoder
            Decode.map InteractionCallbackData.MODAL ModalInteractionCallbackData.decoder
        ]

    let encoder (v: InteractionCallbackData) =
        match v with
        | InteractionCallbackData.MESSAGE data -> MessageInteractionCallbackData.encoder data
        | InteractionCallbackData.AUTOCOMPLETE data -> AutocompleteInteractionCallbackData.encoder data
        | InteractionCallbackData.MODAL data -> ModalInteractionCallbackData.encoder data

module InteractionCallbackResponse =
    module Property =
        let [<Literal>] Interaction = "interaction"
        let [<Literal>] Resource = "resource"

    let decoder: Decoder<InteractionCallbackResponse> =
        Decode.object (fun get -> {
            Interaction = get |> Get.required Property.Interaction InteractionCallback.decoder
            Resource = get |> Get.optional Property.Resource InteractionCallbackResource.decoder
        })

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

    let decoder: Decoder<InteractionCallback> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            ActivityInstanceId = get |> Get.optional Property.ActivityInstanceId Decode.string
            ResponseMessageId = get |> Get.optional Property.ResponseMessageId Decode.string
            ResponseMessageLoading = get |> Get.optional Property.ResponseMessageLoading Decode.bool
            ResponseMessageEphemeral = get |> Get.optional Property.ResponseMessageEphemeral Decode.bool
        })

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

    let decoder: Decoder<InteractionCallbackResource> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionCallbackType>
            ActivityInstance = get |> Get.optional Property.ActivityInstance InteractionCallbackActivityInstance.decoder
            Message = get |> Get.optional Property.Message Message.decoder
        })

    let encoder (v: InteractionCallbackResource) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.ActivityInstance InteractionCallbackActivityInstance.encoder v.ActivityInstance
            |> Encode.optional Property.Message Message.encoder v.Message
        )

module InteractionCallbackActivityInstance =
    module Property =
        let [<Literal>] Id = "id"

    let decoder: Decoder<InteractionCallbackActivityInstance> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
        })

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
        let [<Literal>] LocalizedName = "localized_name"
        let [<Literal>] Description = "description"
        let [<Literal>] DescriptionLocalizations = "description_localizations"
        let [<Literal>] LocalizedDescription = "localized_description"
        let [<Literal>] Options = "options"
        let [<Literal>] DefaultMemberPermissions = "default_member_permissions"
        let [<Literal>] Nsfw = "nsfw"
        let [<Literal>] IntegrationTypes = "integration_types"
        let [<Literal>] Contexts = "contexts"
        let [<Literal>] Version = "version"
        let [<Literal>] Handler = "handler"

    let decoder: Decoder<ApplicationCommand> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.optional Property.Type Decode.Enum.int<ApplicationCommandType> |> Option.defaultValue ApplicationCommandType.CHAT_INPUT
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optinull Property.NameLocalizations (Decode.dict Decode.string)
            LocalizedName = get |> Get.optional Property.LocalizedName Decode.string
            Description = get |> Get.required Property.Description Decode.string
            DescriptionLocalizations = get |> Get.optinull Property.DescriptionLocalizations (Decode.dict Decode.string)
            LocalizedDescription = get |> Get.optional Property.LocalizedDescription Decode.string
            Options = get |> Get.optional Property.Options (Decode.list ApplicationCommandOption.decoder)
            DefaultMemberPermissions = get |> Get.nullable Property.DefaultMemberPermissions Decode.bitfieldL<Permission>
            Nsfw = get |> Get.optional Property.Nsfw Decode.bool |> Option.defaultValue false
            IntegrationTypes = get |> Get.optional Property.IntegrationTypes (Decode.list Decode.Enum.int<ApplicationIntegrationType>) |> Option.defaultValue [ApplicationIntegrationType.GUILD_INSTALL; ApplicationIntegrationType.USER_INSTALL]
            Contexts = get |> Get.optional Property.Contexts (Decode.list Decode.Enum.int<InteractionContextType>)
            Version = get |> Get.required Property.Version Decode.string
            Handler = get |> Get.optional Property.Handler Decode.Enum.int<ApplicationCommandHandlerType>
        })

    let encoder (v: ApplicationCommand) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.optional Property.Type Encode.Enum.int (Some v.Type)
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optinull Property.NameLocalizations (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional Property.LocalizedName Encode.string v.LocalizedName
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optinull Property.DescriptionLocalizations (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional Property.LocalizedDescription Encode.string v.LocalizedDescription
            |> Encode.optional Property.Options (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.nullable Property.DefaultMemberPermissions Encode.bitfieldL v.DefaultMemberPermissions
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
        let [<Literal>] LocalizedName = "localized_name"
        let [<Literal>] Description = "description"
        let [<Literal>] DescriptionLocalizations = "description_localizations"
        let [<Literal>] LocalizedDescription = "localized_description"
        let [<Literal>] Required = "required"
        let [<Literal>] Choices = "choices"
        let [<Literal>] Options = "options"
        let [<Literal>] ChannelTypes = "channel_types"
        let [<Literal>] MinValue = "min_value"
        let [<Literal>] MaxValue = "max_value"
        let [<Literal>] MinLength = "min_length"
        let [<Literal>] MaxLength = "max_length"
        let [<Literal>] Autocomplete = "autocomplete"

    let decoder: Decoder<ApplicationCommandOption> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandOptionType>
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optinull Property.NameLocalizations (Decode.dict Decode.string)
            LocalizedName = get |> Get.optional Property.LocalizedName Decode.string
            Description = get |> Get.required Property.Description Decode.string
            DescriptionLocalizations = get |> Get.optinull Property.DescriptionLocalizations (Decode.dict Decode.string)
            LocalizedDescription = get |> Get.optional Property.LocalizedDescription Decode.string
            Required = get |> Get.optional Property.Required Decode.bool
            Choices = get |> Get.optional Property.Choices (Decode.list ApplicationCommandOptionChoice.decoder)
            Options = get |> Get.optional Property.Options (Decode.list decoder)
            ChannelTypes = get |> Get.optional Property.ChannelTypes (Decode.list Decode.Enum.int<ChannelType>)
            MinValue = get |> Get.optional Property.MinValue ApplicationCommandOptionMinValue.decoder
            MaxValue = get |> Get.optional Property.MaxValue ApplicationCommandOptionMaxValue.decoder
            MinLength = get |> Get.optional Property.MinLength Decode.int
            MaxLength = get |> Get.optional Property.MaxLength Decode.int
            Autocomplete = get |> Get.optional Property.Autocomplete Decode.bool
        })

    let encoder (v: ApplicationCommandOption) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optinull Property.NameLocalizations (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional Property.LocalizedName Encode.string v.LocalizedName
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optinull Property.DescriptionLocalizations (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional Property.LocalizedDescription Encode.string v.LocalizedDescription
            |> Encode.optional Property.Required Encode.bool v.Required
            |> Encode.optional Property.Choices (List.map ApplicationCommandOptionChoice.encoder >> Encode.list) v.Choices
            |> Encode.optional Property.Options (List.map encoder >> Encode.list) v.Options
            |> Encode.optional Property.ChannelTypes (List.map Encode.Enum.int >> Encode.list) v.ChannelTypes
            |> Encode.optional Property.MinValue ApplicationCommandOptionMinValue.encoder v.MinValue
            |> Encode.optional Property.MaxValue ApplicationCommandOptionMaxValue.encoder v.MaxValue
            |> Encode.optional Property.MinLength Encode.int v.MinLength
            |> Encode.optional Property.MaxLength Encode.int v.MaxLength
            |> Encode.optional Property.Autocomplete Encode.bool v.Autocomplete
        )

module ApplicationCommandOptionMinValue =
    let decoder: Decoder<ApplicationCommandOptionMinValue> =
        Decode.oneOf [
            Decode.map ApplicationCommandOptionMinValue.INT Decode.int
            Decode.map ApplicationCommandOptionMinValue.DOUBLE Decode.float
        ]

    let encoder (v: ApplicationCommandOptionMinValue) =
        match v with
        | ApplicationCommandOptionMinValue.INT data -> Encode.int data
        | ApplicationCommandOptionMinValue.DOUBLE data -> Encode.float data

module ApplicationCommandOptionMaxValue =
    let decoder: Decoder<ApplicationCommandOptionMaxValue> =
        Decode.oneOf [
            Decode.map ApplicationCommandOptionMaxValue.INT Decode.int
            Decode.map ApplicationCommandOptionMaxValue.DOUBLE Decode.float
        ]

    let encoder (v: ApplicationCommandOptionMaxValue) =
        match v with
        | ApplicationCommandOptionMaxValue.INT data -> Encode.int data
        | ApplicationCommandOptionMaxValue.DOUBLE data -> Encode.float data

module ApplicationCommandOptionChoice =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] NameLocalizations = "name_localizations"
        let [<Literal>] Value = "value"

    let decoder: Decoder<ApplicationCommandOptionChoice> =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optinull Property.NameLocalizations (Decode.dict Decode.string)
            Value = get |> Get.required Property.Value ApplicationCommandOptionChoiceValue.decoder
        })

    let encoder (v: ApplicationCommandOptionChoice) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optinull Property.NameLocalizations (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.required Property.Value ApplicationCommandOptionChoiceValue.encoder v.Value
        )

module ApplicationCommandOptionChoiceValue =
    let decoder: Decoder<ApplicationCommandOptionChoiceValue>  =
        Decode.oneOf [
            Decode.map ApplicationCommandOptionChoiceValue.STRING Decode.string
            Decode.map ApplicationCommandOptionChoiceValue.INT Decode.int
            Decode.map ApplicationCommandOptionChoiceValue.DOUBLE Decode.float
        ]

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

    let decoder: Decoder<GuildApplicationCommandPermissions> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            Permissions = get |> Get.required Property.Permissions (Decode.list ApplicationCommandPermission.decoder)
        })

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

    let decoder: Decoder<ApplicationCommandPermission> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationCommandPermissionType>
            Permission = get |> Get.required Property.Permission Decode.bool
        })

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

    let decoder: Decoder<ActionRow> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<ComponentType>
            Components = get |> Get.required Property.Components (Decode.list Component.decoder)
        })

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

    let decoder: Decoder<Button> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<ComponentType>
            Style = get |> Get.required Property.Style Decode.Enum.int<ButtonStyle>
            Label = get |> Get.optional Property.Label Decode.string
            Emoji = get |> Get.optional Property.Emoji Emoji.Partial.decoder
            CustomId = get |> Get.optional Property.CustomId Decode.string
            SkuId = get |> Get.optional Property.SkuId Decode.string
            Url = get |> Get.optional Property.Url Decode.string
            Disabled = get |> Get.optional Property.Disabled Decode.bool |> Option.defaultValue false
        })

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

    let decoder: Decoder<SelectMenu> =
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
        })

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

    let decoder: Decoder<SelectMenuOption> =
        Decode.object (fun get -> {
            Label = get |> Get.required Property.Label Decode.string
            Value = get |> Get.required Property.Value Decode.string
            Description = get |> Get.optional Property.Description Decode.string
            Emoji = get |> Get.optional Property.Emoji Emoji.Partial.decoder
            Default = get |> Get.optional Property.Default Decode.bool
        })

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

    let decoder: Decoder<SelectMenuDefaultValue> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type SelectMenuDefaultValueType.decoder
        })

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

    let decoder: Decoder<TextInput> =
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
        })

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
    let decoder: Decoder<Component> =
        Decode.oneOf [
            Decode.map Component.ACTION_ROW ActionRow.decoder
            Decode.map Component.BUTTON Button.decoder
            Decode.map Component.SELECT_MENU SelectMenu.decoder
            Decode.map Component.TEXT_INPUT TextInput.decoder
        ]

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

    let decoder: Decoder<SessionStartLimit> =
        Decode.object (fun get -> {
            Total = get |> Get.required Property.Total Decode.int
            Remaining = get |> Get.required Property.Remaining Decode.int
            ResetAfter = get |> Get.required Property.ResetAfter Decode.int
            MaxConcurrency = get |> Get.required Property.MaxConcurrency Decode.int
        })

    let encoder (v: SessionStartLimit) =
        Encode.object ([]
            |> Encode.required Property.Total Encode.int v.Total
            |> Encode.required Property.Remaining Encode.int v.Remaining
            |> Encode.required Property.ResetAfter Encode.int v.ResetAfter
            |> Encode.required Property.MaxConcurrency Encode.int v.MaxConcurrency
        )

module GatewayEventPayload =
    module Property =
        let [<Literal>] Opcode = "op"
        let [<Literal>] Data = "d"
        let [<Literal>] Sequence = "s"
        let [<Literal>] EventName = "t"

    let decoder (payloadDecoder: Decoder<'T>): Decoder<GatewayEventPayload<'T>> =
        Decode.object (fun get -> {
            Opcode = get |> Get.required Property.Opcode Decode.Enum.int<GatewayOpcode>
            Data = get |> Get.required Property.Data payloadDecoder
            Sequence = get |> Get.nullable Property.Sequence Decode.int
            EventName = get |> Get.nullable Property.EventName Decode.string
        })

    let encoder (payloadEncoder: Encoder<'T>) (v: GatewayEventPayload<'T>) =
        Encode.object ([]
            |> Encode.required Property.Opcode Encode.Enum.int v.Opcode
            |> Encode.required Property.Data payloadEncoder v.Data
            |> Encode.nullable Property.Sequence Encode.int v.Sequence
            |> Encode.nullable Property.EventName Encode.string v.EventName
        )

module GatewaySendEventPayload =
    let decoder: Decoder<GatewaySendEventPayload> = GatewayEventPayload.decoder GatewaySendEventData.decoder
    let encoder (v: GatewaySendEventPayload) = GatewayEventPayload.encoder GatewaySendEventData.encoder v

module GatewaySendEventData =
    let decoder: Decoder<GatewaySendEventData> =
        Decode.oneOf [
            Decode.map GatewaySendEventData.OPTIONAL_INT (Decode.option Decode.int)
            Decode.map GatewaySendEventData.IDENTIFY IdentifySendEvent.decoder
            Decode.map GatewaySendEventData.RESUME ResumeSendEvent.decoder
            Decode.map GatewaySendEventData.REQUEST_GUILD_MEMBERS RequestGuildMembersSendEvent.decoder
            Decode.map GatewaySendEventData.REQUEST_SOUNDBOARD_SOUNDS RequestSoundboardSoundsSendEvent.decoder
            Decode.map GatewaySendEventData.UPDATE_VOICE_STATE UpdateVoiceStateSendEvent.decoder
            Decode.map GatewaySendEventData.UPDATE_PRESENCE UpdatePresenceSendEvent.decoder
        ]

    let encoder (v: GatewaySendEventData) =
        match v with
        | GatewaySendEventData.OPTIONAL_INT d -> Encode.option Encode.int d
        | GatewaySendEventData.IDENTIFY d -> IdentifySendEvent.encoder d
        | GatewaySendEventData.RESUME d -> ResumeSendEvent.encoder d
        | GatewaySendEventData.REQUEST_GUILD_MEMBERS d -> RequestGuildMembersSendEvent.encoder d
        | GatewaySendEventData.REQUEST_SOUNDBOARD_SOUNDS d -> RequestSoundboardSoundsSendEvent.encoder d
        | GatewaySendEventData.UPDATE_VOICE_STATE d -> UpdateVoiceStateSendEvent.encoder d
        | GatewaySendEventData.UPDATE_PRESENCE d -> UpdatePresenceSendEvent.encoder d

module GatewayReceiveEventPayload =
    let decoder: Decoder<GatewayReceiveEventPayload> = GatewayEventPayload.decoder (Decode.option GatewayReceiveEventData.decoder)
    let encoder (v: GatewayReceiveEventPayload) = GatewayEventPayload.encoder (Encode.option GatewayReceiveEventData.encoder) v

module GatewayReceiveEventData =
    let decoder: Decoder<GatewayReceiveEventData> =
        Decode.oneOf [
            Decode.map GatewayReceiveEventData.BOOLEAN Decode.bool
            Decode.map GatewayReceiveEventData.HELLO HelloReceiveEvent.decoder
            Decode.map GatewayReceiveEventData.READY ReadyReceiveEvent.decoder
        ]

    let encoder (v: GatewayReceiveEventData) =
        match v with
        | GatewayReceiveEventData.BOOLEAN d -> Encode.bool d
        | GatewayReceiveEventData.HELLO d -> HelloReceiveEvent.encoder d
        | GatewayReceiveEventData.READY d -> ReadyReceiveEvent.encoder d

module IdentifySendEvent =
    module Property =
        let [<Literal>] Token = "token"
        let [<Literal>] Properties = "properties"
        let [<Literal>] Compress = "compress"
        let [<Literal>] LargeThreshold = "large_threshold"
        let [<Literal>] Shard = "shard"
        let [<Literal>] Presence = "presence"
        let [<Literal>] Intents = "intents"

    let decoder: Decoder<IdentifySendEvent> =
        Decode.object (fun get -> {
            Token = get |> Get.required Property.Token Decode.string
            Properties = get |> Get.required Property.Properties IdentifyConnectionProperties.decoder
            Compress = get |> Get.optional Property.Compress Decode.bool
            LargeThreshold = get |> Get.optional Property.LargeThreshold Decode.int
            Shard = get |> Get.optional Property.Shard IntPair.decoder
            Presence = get |> Get.optional Property.Presence UpdatePresenceSendEvent.decoder
            Intents = get |> Get.required Property.Intents Decode.int
        })

    let encoder (v: IdentifySendEvent) =
        Encode.object ([]
            |> Encode.required Property.Token Encode.string v.Token
            |> Encode.required Property.Properties IdentifyConnectionProperties.encoder v.Properties
            |> Encode.optional Property.Compress Encode.bool v.Compress
            |> Encode.optional Property.LargeThreshold Encode.int v.LargeThreshold
            |> Encode.optional Property.Shard IntPair.encoder v.Shard
            |> Encode.optional Property.Presence UpdatePresenceSendEvent.encoder v.Presence
            |> Encode.required Property.Intents Encode.int v.Intents
        )

module IdentifyConnectionProperties =
    module Property =
        let [<Literal>] OperatingSystem = "os"
        let [<Literal>] Browser = "browser"
        let [<Literal>] Device = "device"

    let decoder: Decoder<IdentifyConnectionProperties> =
        Decode.object (fun get -> {
            OperatingSystem = get |> Get.required Property.OperatingSystem Decode.string
            Browser = get |> Get.required Property.Browser Decode.string
            Device = get |> Get.required Property.Device Decode.string
        })

    let encoder (v: IdentifyConnectionProperties) =
        Encode.object ([]
            |> Encode.required Property.OperatingSystem Encode.string v.OperatingSystem
            |> Encode.required Property.Browser Encode.string v.Browser
            |> Encode.required Property.Device Encode.string v.Device
        )

module ResumeSendEvent =
    module Property =
        let [<Literal>] Token = "token"
        let [<Literal>] SessionId = "session_id"
        let [<Literal>] Sequence = "seq"

    let decoder: Decoder<ResumeSendEvent> =
        Decode.object (fun get -> {
            Token = get |> Get.required Property.Token Decode.string
            SessionId = get |> Get.required Property.SessionId Decode.string
            Sequence = get |> Get.required Property.Sequence Decode.int
        })

    let encoder (v: ResumeSendEvent) =
        Encode.object ([]
            |> Encode.required Property.Token Encode.string v.Token
            |> Encode.required Property.SessionId Encode.string v.SessionId
            |> Encode.required Property.Sequence Encode.int v.Sequence
        )

module HeartbeatSendEvent =
    let decoder: Decoder<HeartbeatSendEvent> = Decode.option Decode.int
    let encoder: Encoder<HeartbeatSendEvent> = Encode.option Encode.int

module RequestGuildMembersSendEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Query = "query"
        let [<Literal>] Limit = "limit"
        let [<Literal>] Presences = "presences"
        let [<Literal>] UserIds = "user_ids"
        let [<Literal>] Nonce = "nonce"

    let decoder: Decoder<RequestGuildMembersSendEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Query = get |> Get.optional Property.Query Decode.string
            Limit = get |> Get.required Property.Limit Decode.int
            Presences = get |> Get.optional Property.Presences Decode.bool
            UserIds = get |> Get.optional Property.UserIds (Decode.list Decode.string)
            Nonce = get |> Get.optional Property.Nonce Decode.string
        })

    let encoder (v: RequestGuildMembersSendEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Query Encode.string v.Query
            |> Encode.required Property.Limit Encode.int v.Limit
            |> Encode.optional Property.Presences Encode.bool v.Presences
            |> Encode.optional Property.UserIds (List.map Encode.string >> Encode.list) v.UserIds
            |> Encode.optional Property.Nonce Encode.string v.Nonce
        )

module RequestSoundboardSoundsSendEvent =
    module Property =
        let [<Literal>] GuildIds = "guild_ids"

    let decoder: Decoder<RequestSoundboardSoundsSendEvent> =
        Decode.object (fun get -> {
            GuildIds = get |> Get.required Property.GuildIds (Decode.list Decode.string)
        })

    let encoder (v: RequestSoundboardSoundsSendEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildIds (List.map Encode.string >> Encode.list) v.GuildIds
        )

module UpdateVoiceStateSendEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] SelfMute = "self_mute"
        let [<Literal>] SelfDeaf = "self_deaf"

    let decoder: Decoder<UpdateVoiceStateSendEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            ChannelId = get |> Get.nullable Property.ChannelId Decode.string
            SelfMute = get |> Get.required Property.SelfMute Decode.bool
            SelfDeaf = get |> Get.required Property.SelfDeaf Decode.bool
        })

    let encoder (v: UpdateVoiceStateSendEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.nullable Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.SelfMute Encode.bool v.SelfMute
            |> Encode.required Property.SelfDeaf Encode.bool v.SelfDeaf
        )

module UpdatePresenceSendEvent =
    module Property =
        let [<Literal>] Since = "since"
        let [<Literal>] Activities = "activities"
        let [<Literal>] Status = "status"
        let [<Literal>] Afk = "afk"

    let decoder: Decoder<UpdatePresenceSendEvent> =
        Decode.object (fun get -> {
            Since = get |> Get.nullable Property.Since UnixTimestamp.decoder
            Activities = get |> Get.required Property.Activities (Decode.list Activity.decoder)
            Status = get |> Get.required Property.Status Status.decoder
            Afk = get |> Get.required Property.Afk Decode.bool
        })

    let encoder (v: UpdatePresenceSendEvent) =
        Encode.object ([]
            |> Encode.nullable Property.Since UnixTimestamp.encoder v.Since
            |> Encode.required Property.Activities (List.map Activity.encoder >> Encode.list) v.Activities
            |> Encode.required Property.Status Status.encoder v.Status
            |> Encode.required Property.Afk Encode.bool v.Afk
        )

    module Partial =
        let decoder: Decoder<PartialUpdatePresenceSendEvent> =
            Decode.object (fun get -> {
                Since = get |> Get.optinull Property.Since UnixTimestamp.decoder
                Activities = get |> Get.optional Property.Activities (Decode.list Activity.decoder)
                Status = get |> Get.optional Property.Status Status.decoder
                Afk = get |> Get.optional Property.Afk Decode.bool
            })

        let encoder (v: PartialUpdatePresenceSendEvent) =
            Encode.object ([]
                |> Encode.optinull Property.Since UnixTimestamp.encoder v.Since
                |> Encode.optional Property.Activities (List.map Activity.encoder >> Encode.list) v.Activities
                |> Encode.optional Property.Status Status.encoder v.Status
                |> Encode.optional Property.Afk Encode.bool v.Afk
            )

module HeartbeatReceiveEvent =
    let decoder = Decode.nil
    let encoder (_: HeartbeatReceiveEvent) = Encode.nil

module HeartbeatAckReceiveEvent =
    let decoder = Decode.nil
    let encoder (_: HeartbeatReceiveEvent) = Encode.nil

module HelloReceiveEvent =
    module Property =
        let [<Literal>] HeartbeatInterval = "heartbeat_interval"

    let decoder: Decoder<HelloReceiveEvent> =
        Decode.object (fun get -> {
            HeartbeatInterval = get |> Get.required Property.HeartbeatInterval Decode.int
        })

    let encoder (v: HelloReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.HeartbeatInterval Encode.int v.HeartbeatInterval
        )

module ReadyReceiveEvent =
    module Property =
        let [<Literal>] Version = "v"
        let [<Literal>] User = "user"
        let [<Literal>] Guilds = "guilds"
        let [<Literal>] SessionId = "session_id"
        let [<Literal>] ResumeGatewayUrl = "resume_gateway_url"
        let [<Literal>] Shard = "shard"
        let [<Literal>] Application = "application"

    let decoder: Decoder<ReadyReceiveEvent> =
        Decode.object (fun get -> {
            Version = get |> Get.required Property.Version Decode.int
            User = get |> Get.required Property.User User.decoder
            Guilds = get |> Get.required Property.Guilds (Decode.list Guild.Unavailable.decoder)
            SessionId = get |> Get.required Property.SessionId Decode.string
            ResumeGatewayUrl = get |> Get.required Property.ResumeGatewayUrl Decode.string
            Shard = get |> Get.optional Property.Shard IntPair.decoder
            Application = get |> Get.required Property.Application Application.Partial.decoder
        })

    let encoder (v: ReadyReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.Version Encode.int v.Version
            |> Encode.required Property.User User.encoder v.User
            |> Encode.required Property.Guilds (List.map Guild.Unavailable.encoder >> Encode.list) v.Guilds
            |> Encode.required Property.SessionId Encode.string v.SessionId
            |> Encode.required Property.ResumeGatewayUrl Encode.string v.ResumeGatewayUrl
            |> Encode.optional Property.Shard IntPair.encoder v.Shard
            |> Encode.required Property.Application Application.Partial.encoder v.Application
        )

module ResumedReceiveEvent =
    let decoder = Decode.nil
    let encoder (_: HeartbeatReceiveEvent) = Encode.nil

module ReconnectReceiveEvent =
    let decoder = Decode.nil
    let encoder (_: HeartbeatReceiveEvent) = Encode.nil

module InvalidSessionReceiveEvent =
    let decoder: Decoder<InvalidSessionReceiveEvent> = Decode.bool
    let encoder (v: InvalidSessionReceiveEvent) = Encode.bool v

module ApplicationCommandPermissionsUpdateReceiveEvent =
    let decoder: Decoder<ApplicationCommandPermissionsUpdateReceiveEvent> = ApplicationCommandPermission.decoder
    let encoder (v: ApplicationCommandPermissionsUpdateReceiveEvent) = ApplicationCommandPermission.encoder v

module AutoModerationRuleCreateReceiveEvent =
    let decoder: Decoder<AutoModerationRuleCreateReceiveEvent> = AutoModerationRule.decoder
    let encoder (v: AutoModerationRuleCreateReceiveEvent) = AutoModerationRule.encoder v

module AutoModerationRuleUpdateReceiveEvent =
    let decoder: Decoder<AutoModerationRuleUpdateReceiveEvent> = AutoModerationRule.decoder
    let encoder (v: AutoModerationRuleUpdateReceiveEvent) = AutoModerationRule.encoder v

module AutoModerationRuleDeleteReceiveEvent =
    let decoder: Decoder<AutoModerationRuleDeleteReceiveEvent> = AutoModerationRule.decoder
    let encoder (v: AutoModerationRuleDeleteReceiveEvent) = AutoModerationRule.encoder v

module AutoModerationActionExecutionReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Action = "action"
        let [<Literal>] RuleId = "rule_id"
        let [<Literal>] RuleTriggerType = "rule_trigger_type"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] AlertSystemMessageId = "alert_system_message_id"
        let [<Literal>] Content = "content"
        let [<Literal>] MatchedKeyword = "matched_keyword"
        let [<Literal>] MatchedContent = "matched_content"

    let decoder: Decoder<AutoModerationActionExecutionReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Action = get |> Get.required Property.Action AutoModerationAction.decoder
            RuleId = get |> Get.required Property.RuleId Decode.string
            RuleTriggerType = get |> Get.required Property.RuleTriggerType Decode.Enum.int<AutoModerationTriggerType>
            UserId = get |> Get.required Property.UserId Decode.string
            ChannelId = get |> Get.optional Property.ChannelId Decode.string
            MessageId = get |> Get.optional Property.MessageId Decode.string
            AlertSystemMessageId = get |> Get.optional Property.AlertSystemMessageId Decode.string
            Content = get |> Get.optional Property.Content Decode.string
            MatchedKeyword = get |> Get.nullable Property.MatchedKeyword Decode.string
            MatchedContent = get |> Get.optinull Property.MatchedContent Decode.string
        })

    let encoder (v: AutoModerationActionExecutionReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Action AutoModerationAction.encoder v.Action
            |> Encode.required Property.RuleId Encode.string v.RuleId
            |> Encode.required Property.RuleTriggerType Encode.Enum.int v.RuleTriggerType
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.optional Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.AlertSystemMessageId Encode.string v.AlertSystemMessageId
            |> Encode.optional Property.Content Encode.string v.Content
            |> Encode.nullable Property.MatchedKeyword Encode.string v.MatchedKeyword
            |> Encode.optinull Property.MatchedContent Encode.string v.MatchedContent
        )

module ChannelCreateReceiveEvent =
    let decoder: Decoder<ChannelCreateReceiveEvent> = Channel.decoder
    let encoder (v: ChannelCreateReceiveEvent) = Channel.encoder v

module ChannelUpdateReceiveEvent =
    let decoder: Decoder<ChannelUpdateReceiveEvent> = Channel.decoder
    let encoder (v: ChannelUpdateReceiveEvent) = Channel.encoder v

module ChannelDeleteReceiveEvent =
    let decoder: Decoder<ChannelDeleteReceiveEvent> = Channel.decoder
    let encoder (v: ChannelDeleteReceiveEvent) = Channel.encoder v

module ThreadCreateReceiveEvent =
    module Property =
        let [<Literal>] NewlyCreated = "newly_created"
        let [<Literal>] ThreadMember = "thread_member"

    let decoder: Decoder<ThreadCreateReceiveEvent> =
        Decode.object (fun get -> {
            Channel = get |> Get.extract Channel.decoder
            NewlyCreated = get |> Get.optional Property.NewlyCreated Decode.bool
            ThreadMember = get |> Get.optional Property.ThreadMember ThreadMember.decoder
        })

    let encoder (v: ThreadCreateReceiveEvent) =
        Encode.object (
            Channel.encodeProperties v.Channel @ []
            |> Encode.optional Property.NewlyCreated Encode.bool v.NewlyCreated
            |> Encode.optional Property.ThreadMember ThreadMember.encoder v.ThreadMember
        )

module ThreadUpdateReceiveEvent =
    let decoder: Decoder<ThreadUpdateReceiveEvent> = Channel.decoder
    let encoder (v: ThreadUpdateReceiveEvent) = Channel.encoder v

module ThreadDeleteReceiveEvent =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ParentId = "parent_id"
        let [<Literal>] Type = "type"

    let decoder: Decoder<ThreadDeleteReceiveEvent> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            ParentId = get |> Get.optinull Property.ParentId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ChannelType>
        })

    let encoder (v: ThreadDeleteReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optinull Property.ParentId Encode.string v.ParentId
            |> Encode.required Property.Type Encode.Enum.int v.Type
        )

module ThreadListSyncReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelIds = "channel_ids"
        let [<Literal>] Threads = "threads"
        let [<Literal>] Members = "members"

    let decoder: Decoder<ThreadListSyncReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            ChannelIds = get |> Get.optional Property.ChannelIds (Decode.list Decode.string)
            Threads = get |> Get.required Property.Threads (Decode.list Channel.decoder)
            Members = get |> Get.required Property.Members (Decode.list ThreadMember.decoder)
        })

    let encoder (v: ThreadListSyncReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.ChannelIds (List.map Encode.string >> Encode.list) v.ChannelIds
            |> Encode.required Property.Threads (List.map Channel.encoder >> Encode.list) v.Threads
            |> Encode.required Property.Members (List.map ThreadMember.encoder >> Encode.list) v.Members
        )

module ThreadMemberUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<ThreadMemberUpdateReceiveEvent> =
        Decode.object (fun get -> {
            ThreadMember = get |> Get.extract ThreadMember.decoder
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: ThreadMemberUpdateReceiveEvent) =
        Encode.object (
            ThreadMember.encodeProperties v.ThreadMember @ []
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module ThreadMembersUpdateReceiveEvent =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] MemberCount = "member_count"
        let [<Literal>] AddedMembers = "added_members"
        let [<Literal>] RemovedMemberIds = "removed_member_ids"

    let decoder: Decoder<ThreadMembersUpdateReceiveEvent> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            MemberCount = get |> Get.required Property.MemberCount Decode.int
            AddedMembers = get |> Get.optional Property.AddedMembers (Decode.list ThreadMember.decoder)
            RemovedMemberIds = get |> Get.optional Property.RemovedMemberIds (Decode.list Decode.string)
        })

    let encoder (v: ThreadMembersUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.MemberCount Encode.int v.MemberCount
            |> Encode.optional Property.AddedMembers (List.map ThreadMember.encoder >> Encode.list) v.AddedMembers
            |> Encode.optional Property.RemovedMemberIds (List.map Encode.string >> Encode.list) v.RemovedMemberIds
        )

module ChannelPinsUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] LastPinTimestamp = "last_pin_timestamp"

    let decoder: Decoder<ChannelPinsUpdateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.optional Property.GuildId Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            LastPinTimestamp = get |> Get.optinull Property.LastPinTimestamp UnixTimestamp.decoder
        })

    let encoder (v: ChannelPinsUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.optinull Property.LastPinTimestamp UnixTimestamp.encoder v.LastPinTimestamp
        )

module EntitlementCreateReceiveEvent =
    let decoder: Decoder<EntitlementCreateReceiveEvent> = Entitlement.decoder
    let encoder (v: EntitlementCreateReceiveEvent) = Entitlement.encoder v

module EntitlementUpdateReceiveEvent =
    let decoder: Decoder<EntitlementUpdateReceiveEvent> = Entitlement.decoder
    let encoder (v: EntitlementUpdateReceiveEvent) = Entitlement.encoder v

module EntitlementDeleteReceiveEvent =
    let decoder: Decoder<EntitlementDeleteReceiveEvent> = Entitlement.decoder
    let encoder (v: EntitlementDeleteReceiveEvent) = Entitlement.encoder v

module GuildCreateReceiveEvent =
    let decoder: Decoder<GuildCreateReceiveEvent> =
        Decode.oneOf [
            Decode.map GuildCreateReceiveEvent.AvailableGuild GuildCreateReceiveEventAvailableGuild.decoder
            Decode.map GuildCreateReceiveEvent.UnavailableGuild Guild.Unavailable.decoder
        ]

    let encoder (v: GuildCreateReceiveEvent) =
        match v with
        | GuildCreateReceiveEvent.AvailableGuild a -> GuildCreateReceiveEventAvailableGuild.encoder a
        | GuildCreateReceiveEvent.UnavailableGuild u -> Guild.Unavailable.encoder u

module GuildCreateReceiveEventAvailableGuild =
    module Property =
        let [<Literal>] JoinedAt = "joined_at"
        let [<Literal>] Large = "large"
        let [<Literal>] Unavailable = "unavailable"
        let [<Literal>] MemberCount = "member_count"
        let [<Literal>] VoiceStates = "voice_states"
        let [<Literal>] Members = "members"
        let [<Literal>] Channels = "channels"
        let [<Literal>] Threads = "threads"
        let [<Literal>] Presences = "presences"
        let [<Literal>] StageInstances = "stage_instances"
        let [<Literal>] GuildScheduledEvents = "guild_scheduled_events"
        let [<Literal>] SoundboardSounds = "soundboard_sounds"

    let decoder: Decoder<GuildCreateReceiveEventAvailableGuild> =
        Decode.object (fun get -> {
            Guild = get |> Get.extract Guild.decoder
            JoinedAt = get |> Get.required Property.JoinedAt UnixTimestamp.decoder
            Large = get |> Get.required Property.Large Decode.bool
            Unavailable = get |> Get.optional Property.Unavailable Decode.bool |> Option.defaultValue false
            MemberCount = get |> Get.required Property.MemberCount Decode.int
            VoiceStates = get |> Get.required Property.VoiceStates (Decode.list VoiceState.Partial.decoder)
            Members = get |> Get.required Property.Members (Decode.list GuildMember.decoder)
            Channels = get |> Get.required Property.Channels (Decode.list Channel.decoder)
            Threads = get |> Get.required Property.Threads (Decode.list Channel.decoder)
            Presences = get |> Get.required Property.Presences (Decode.list UpdatePresenceSendEvent.Partial.decoder)
            StageInstances = get |> Get.required Property.StageInstances (Decode.list StageInstance.decoder)
            GuildScheduledEvents = get |> Get.required Property.GuildScheduledEvents (Decode.list GuildScheduledEvent.decoder)
            SoundboardSounds = get |> Get.required Property.SoundboardSounds (Decode.list SoundboardSound.decoder)
        })

    let encoder (v: GuildCreateReceiveEventAvailableGuild) =
        Encode.object (Guild.encodeProperties v.Guild
            |> Encode.required Property.JoinedAt UnixTimestamp.encoder v.JoinedAt
            |> Encode.required Property.Large Encode.bool v.Large
            |> Encode.optional Property.Unavailable Encode.bool (v.Unavailable |> function | true -> Some true | false -> None)
            |> Encode.required Property.MemberCount Encode.int v.MemberCount
            |> Encode.required Property.VoiceStates (List.map VoiceState.Partial.encoder >> Encode.list) v.VoiceStates
            |> Encode.required Property.Members (List.map GuildMember.encoder >> Encode.list) v.Members
            |> Encode.required Property.Channels (List.map Channel.encoder >> Encode.list) v.Channels
            |> Encode.required Property.Threads (List.map Channel.encoder >> Encode.list) v.Threads
            |> Encode.required Property.Presences (List.map UpdatePresenceSendEvent.Partial.encoder >> Encode.list) v.Presences
            |> Encode.required Property.StageInstances (List.map StageInstance.encoder >> Encode.list) v.StageInstances
            |> Encode.required Property.GuildScheduledEvents (List.map GuildScheduledEvent.encoder >> Encode.list) v.GuildScheduledEvents
            |> Encode.required Property.SoundboardSounds (List.map SoundboardSound.encoder >> Encode.list) v.SoundboardSounds
        )

module GuildUpdateReceiveEvent =
    let decoder: Decoder<GuildUpdateReceiveEvent> = Guild.decoder
    let encoder (v: GuildUpdateReceiveEvent) = Guild.encoder v

module GuildDeleteReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Unavailable = "unavailable"

    let decoder: Decoder<GuildDeleteReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Unavailable = get |> Get.optional Property.Unavailable Decode.bool
        })

    let encoder (v: GuildDeleteReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Unavailable Encode.bool v.Unavailable
        )

module GuildAuditLogEntryCreateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildAuditLogEntryCreateReceiveEvent> =
        Decode.object (fun get -> {
            AuditLogEntry = get |> Get.extract AuditLogEntry.decoder
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildAuditLogEntryCreateReceiveEvent) =
        Encode.object (
            AuditLogEntry.encodeProperties v.AuditLogEntry
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildBanAddReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] User = "user"

    let decoder: Decoder<GuildBanAddReceiveEvent> =
        Decode.object (fun get -> {
            User = get |> Get.extract User.decoder
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildBanAddReceiveEvent) =
        Encode.object (
            User.encodeProperties v.User
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildBanRemoveReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] User = "user"
        
    let decoder: Decoder<GuildBanRemoveReceiveEvent> =
        Decode.object (fun get -> {
            User = get |> Get.extract User.decoder
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildBanRemoveReceiveEvent) =
        Encode.object (
            User.encodeProperties v.User
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildEmojisUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Emojis = "emojis"

    let decoder: Decoder<GuildEmojisUpdateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Emojis = get |> Get.required Property.Emojis (Decode.list Emoji.decoder)
        })

    let encoder (v: GuildEmojisUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Emojis (List.map Emoji.encoder >> Encode.list) v.Emojis
        )

module GuildStickersUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Stickers = "stickers"

    let decoder: Decoder<GuildStickersUpdateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Stickers = get |> Get.required Property.Stickers (Decode.list Sticker.decoder)
        })

    let encoder (v: GuildStickersUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Stickers (List.map Sticker.encoder >> Encode.list) v.Stickers
        )

module GuildIntegrationsUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildIntegrationsUpdateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildIntegrationsUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildMemberAddReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildMemberAddReceiveEvent> =
        Decode.object (fun get -> {
            GuildMember = get |> Get.extract GuildMember.decoder
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildMemberAddReceiveEvent) =
        Encode.object (
            GuildMember.encodeProperties v.GuildMember
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildMemberRemoveReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] User = "user"

    let decoder: Decoder<GuildMemberRemoveReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            User = get |> Get.required Property.User User.decoder
        })

    let encoder (v: GuildMemberRemoveReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.User User.encoder v.User
        )

module GuildMemberUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Roles = "roles"
        let [<Literal>] User = "user"
        let [<Literal>] Nick = "nick"
        let [<Literal>] Avatar = "avatar"
        let [<Literal>] Banner = "banner"
        let [<Literal>] JoinedAt = "joined_at"
        let [<Literal>] PremiumSince = "premium_since"
        let [<Literal>] Deaf = "deaf"
        let [<Literal>] Mute = "mute"
        let [<Literal>] Pending = "pending"
        let [<Literal>] CommunicationDisabledUntil = "communication_disabled_until"
        let [<Literal>] Flags = "flags"
        let [<Literal>] AvatarDecorationData = "avatar_decoration_data"

    let decoder: Decoder<GuildMemberUpdateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Roles = get |> Get.required Property.Roles (Decode.list Decode.string)
            User = get |> Get.required Property.User User.decoder
            Nick = get |> Get.optinull Property.Nick Decode.string
            Avatar = get |> Get.nullable Property.Avatar Decode.string
            Banner = get |> Get.nullable Property.Banner Decode.string
            JoinedAt = get |> Get.nullable Property.JoinedAt Decode.datetimeUtc
            PremiumSince = get |> Get.optinull Property.PremiumSince Decode.datetimeUtc
            Deaf = get |> Get.optional Property.Deaf Decode.bool
            Mute = get |> Get.optional Property.Mute Decode.bool
            Pending = get |> Get.optional Property.Pending Decode.bool
            CommunicationDisabledUntil = get |> Get.optinull Property.CommunicationDisabledUntil Decode.datetimeUtc
            Flags = get |> Get.optional Property.Flags Decode.bitfield
            AvatarDecorationData = get |> Get.optinull Property.AvatarDecorationData AvatarDecorationData.decoder
        })

    let encoder (v: GuildMemberUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Roles (List.map Encode.string >> Encode.list) v.Roles
            |> Encode.required Property.User User.encoder v.User
            |> Encode.optinull Property.Nick Encode.string v.Nick
            |> Encode.nullable Property.Avatar Encode.string v.Avatar
            |> Encode.nullable Property.Banner Encode.string v.Banner
            |> Encode.nullable Property.JoinedAt Encode.datetime v.JoinedAt
            |> Encode.optinull Property.PremiumSince Encode.datetime v.PremiumSince
            |> Encode.optional Property.Deaf Encode.bool v.Deaf
            |> Encode.optional Property.Mute Encode.bool v.Mute
            |> Encode.optional Property.Pending Encode.bool v.Pending
            |> Encode.optinull Property.CommunicationDisabledUntil Encode.datetime v.CommunicationDisabledUntil
            |> Encode.optional Property.Flags Encode.bitfield v.Flags
            |> Encode.optinull Property.AvatarDecorationData AvatarDecorationData.encoder v.AvatarDecorationData
        )

module GuildMembersChunkReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Members = "members"
        let [<Literal>] ChunkIndex = "chunk_index"
        let [<Literal>] ChunkCount = "chunk_count"
        let [<Literal>] NotFound = "not_found"
        let [<Literal>] Presences = "presences"
        let [<Literal>] Nonce = "nonce"

    let decoder: Decoder<GuildMembersChunkReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Members = get |> Get.required Property.Members (Decode.list GuildMember.decoder)
            ChunkIndex = get |> Get.required Property.ChunkIndex Decode.int
            ChunkCount = get |> Get.required Property.ChunkCount Decode.int
            NotFound = get |> Get.optional Property.NotFound (Decode.list Decode.string)
            Presences = get |> Get.optional Property.Presences (Decode.list UpdatePresenceSendEvent.decoder)
            Nonce = get |> Get.optional Property.Nonce Decode.string
        })

    let encoder (v: GuildMembersChunkReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Members (List.map GuildMember.encoder >> Encode.list) v.Members
            |> Encode.required Property.ChunkIndex Encode.int v.ChunkIndex
            |> Encode.required Property.ChunkCount Encode.int v.ChunkCount
            |> Encode.optional Property.NotFound (List.map Encode.string >> Encode.list) v.NotFound
            |> Encode.optional Property.Presences (List.map UpdatePresenceSendEvent.encoder >> Encode.list) v.Presences
            |> Encode.optional Property.Nonce Encode.string v.Nonce
        )

module GuildRoleCreateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Role = "role"

    let decoder: Decoder<GuildRoleCreateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Role = get |> Get.required Property.Role Role.decoder
        })

    let encoder (v: GuildRoleCreateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Role Role.encoder v.Role
        )

module GuildRoleUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Role = "role"

    let decoder: Decoder<GuildRoleUpdateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Role = get |> Get.required Property.Role Role.decoder
        })

    let encoder (v: GuildRoleUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Role Role.encoder v.Role
        )

module GuildRoleDeleteReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] RoleId = "role_id"

    let decoder: Decoder<GuildRoleDeleteReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            RoleId = get |> Get.required Property.RoleId Decode.string
        })

    let encoder (v: GuildRoleDeleteReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.RoleId Encode.string v.RoleId
        )

module GuildScheduledEventCreateReceiveEvent =
    let decoder: Decoder<GuildScheduledEventCreateReceiveEvent> = GuildScheduledEvent.decoder
    let encoder (v: GuildScheduledEventCreateReceiveEvent) = GuildScheduledEvent.encoder v

module GuildScheduledEventUpdateReceiveEvent =
    let decoder: Decoder<GuildScheduledEventUpdateReceiveEvent> = GuildScheduledEvent.decoder
    let encoder (v: GuildScheduledEventUpdateReceiveEvent) = GuildScheduledEvent.encoder v

module GuildScheduledEventDeleteReceiveEvent =
    let decoder: Decoder<GuildScheduledEventDeleteReceiveEvent> = GuildScheduledEvent.decoder
    let encoder (v: GuildScheduledEventDeleteReceiveEvent) = GuildScheduledEvent.encoder v

module GuildScheduledEventUserAddReceiveEvent =
    module Property =
        let [<Literal>] GuildScheduledEventId = "guild_scheduled_event_id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildScheduledEventUserAddReceiveEvent> =
        Decode.object (fun get -> {
            GuildScheduledEventId = get |> Get.required Property.GuildScheduledEventId Decode.string
            UserId = get |> Get.required Property.UserId Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildScheduledEventUserAddReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildScheduledEventId Encode.string v.GuildScheduledEventId
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildScheduledEventUserRemoveReceiveEvent =
    module Property =
        let [<Literal>] GuildScheduledEventId = "guild_scheduled_event_id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildScheduledEventUserRemoveReceiveEvent> =
        Decode.object (fun get -> {
            GuildScheduledEventId = get |> Get.required Property.GuildScheduledEventId Decode.string
            UserId = get |> Get.required Property.UserId Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildScheduledEventUserRemoveReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildScheduledEventId Encode.string v.GuildScheduledEventId
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildSoundboardSoundCreateReceiveEvent =
    let decoder: Decoder<GuildSoundboardSoundCreateReceiveEvent> = SoundboardSound.decoder
    let encoder (v: GuildSoundboardSoundCreateReceiveEvent) = SoundboardSound.encoder v

module GuildSoundboardSoundUpdateReceiveEvent =
    let decoder: Decoder<GuildSoundboardSoundUpdateReceiveEvent> = SoundboardSound.decoder
    let encoder (v: GuildSoundboardSoundUpdateReceiveEvent) = SoundboardSound.encoder v

module GuildSoundboardSoundDeleteReceiveEvent =
    module Property =
        let [<Literal>] SoundId = "sound_id"
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildSoundboardSoundDeleteReceiveEvent> =
        Decode.object (fun get -> {
            SoundId = get |> Get.required Property.SoundId Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildSoundboardSoundDeleteReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.SoundId Encode.string v.SoundId
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildSoundboardSoundsUpdateReceiveEvent =
    module Property =
        let [<Literal>] SoundboardSounds = "soundboard_sounds"
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildSoundboardSoundsUpdateReceiveEvent> =
        Decode.object (fun get -> {
            SoundboardSounds = get |> Get.required Property.SoundboardSounds (Decode.list SoundboardSound.decoder)
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildSoundboardSoundsUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.SoundboardSounds (List.map SoundboardSound.encoder >> Encode.list) v.SoundboardSounds
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module GuildSoundboardSoundsReceiveEvent =
    module Property =
        let [<Literal>] SoundboardSounds = "soundboard_sounds"
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<GuildSoundboardSoundsReceiveEvent> =
        Decode.object (fun get -> {
            SoundboardSounds = get |> Get.required Property.SoundboardSounds (Decode.list SoundboardSound.decoder)
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: GuildSoundboardSoundsReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.SoundboardSounds (List.map SoundboardSound.encoder >> Encode.list) v.SoundboardSounds
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module IntegrationCreateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<IntegrationCreateReceiveEvent> =
        Decode.object (fun get -> {
            Integration = get |> Get.extract Integration.decoder
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: IntegrationCreateReceiveEvent) =
        Encode.object (
            Integration.encodeProperties v.Integration
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module IntegrationUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<IntegrationUpdateReceiveEvent> =
        Decode.object (fun get -> {
            Integration = get |> Get.extract Integration.decoder
            GuildId = get |> Get.required Property.GuildId Decode.string
        })

    let encoder (v: IntegrationUpdateReceiveEvent) =
        Encode.object (
            Integration.encodeProperties v.Integration
            |> Encode.required Property.GuildId Encode.string v.GuildId
        )

module IntegrationDeleteReceiveEvent =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ApplicationId = "application_id"

    let decoder: Decoder<IntegrationDeleteReceiveEvent> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
        })

    let encoder (v: IntegrationDeleteReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
        )

module InviteCreateReceiveEvent =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Code = "code"
        let [<Literal>] CreatedAt = "created_at"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Inviter = "inviter"
        let [<Literal>] MaxAge = "max_age"
        let [<Literal>] MaxUses = "max_uses"
        let [<Literal>] TargetType = "target_type"
        let [<Literal>] TargetUser = "target_user"
        let [<Literal>] TargetApplication = "target_application"
        let [<Literal>] Temporary = "temporary"
        let [<Literal>] Uses = "uses"
    
    let decoder: Decoder<InviteCreateReceiveEvent> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            Code = get |> Get.required Property.Code Decode.string
            CreatedAt = get |> Get.required Property.CreatedAt UnixTimestamp.decoder
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Inviter = get |> Get.optional Property.Inviter User.decoder
            MaxAge = get |> Get.required Property.MaxAge Decode.int
            MaxUses = get |> Get.required Property.MaxUses Decode.int
            TargetType = get |> Get.optional Property.TargetType Decode.Enum.int<InviteTargetType>
            TargetUser = get |> Get.optional Property.TargetUser User.decoder
            TargetApplication = get |> Get.optional Property.TargetApplication Application.Partial.decoder
            Temporary = get |> Get.required Property.Temporary Decode.bool
            Uses = get |> Get.required Property.Uses Decode.int
        })

    let encoder (v: InviteCreateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.Code Encode.string v.Code
            |> Encode.required Property.CreatedAt UnixTimestamp.encoder v.CreatedAt
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Inviter User.encoder v.Inviter
            |> Encode.required Property.MaxAge Encode.int v.MaxAge
            |> Encode.required Property.MaxUses Encode.int v.MaxUses
            |> Encode.optional Property.TargetType Encode.Enum.int v.TargetType
            |> Encode.optional Property.TargetUser User.encoder v.TargetUser
            |> Encode.optional Property.TargetApplication Application.Partial.encoder v.TargetApplication
            |> Encode.required Property.Temporary Encode.bool v.Temporary
            |> Encode.required Property.Uses Encode.int v.Uses
        )

module InviteDeleteReceiveEvent =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Code = "code"

    let decoder: Decoder<InviteDeleteReceiveEvent> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Code = get |> Get.required Property.Code Decode.string
        })

    let encoder (v: InviteDeleteReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Code Encode.string v.Code
        )

module MessageCreateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Member = "member"
        let [<Literal>] Mentions = "mentions"

    let decoder: Decoder<MessageCreateReceiveEvent> =
        Decode.object (fun get -> {
            Message = get |> Get.extract Message.decoder
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Member = get |> Get.optional Property.Member GuildMember.Partial.decoder
            Mentions = get |> Get.required Property.Mentions (Decode.list MessageCreateReceiveEventMention.decoder)
        })

    let encoder (v: MessageCreateReceiveEvent) =
        Encode.object (
            Message.encodeProperties v.Message
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Member GuildMember.Partial.encoder v.Member
            |> Encode.required Property.Mentions (List.map MessageCreateReceiveEventMention.encoder >> Encode.list) v.Mentions
        )

module MessageCreateReceiveEventMention =
    module Property =
        let [<Literal>] Member = "member"

    let decoder: Decoder<MessageCreateReceiveEventMention> =
        Decode.object (fun get -> {
            User = get |> Get.extract User.decoder
            Member = get |> Get.optional Property.Member GuildMember.Partial.decoder
        })

    let encoder (v: MessageCreateReceiveEventMention) =
        Encode.object (
            User.encodeProperties v.User
            |> Encode.optional Property.Member GuildMember.Partial.encoder v.Member
        )

module MessageUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Member = "member"
        let [<Literal>] Mentions = "mentions"
        
    let decoder: Decoder<MessageUpdateReceiveEvent> =
        Decode.object (fun get -> {
            Message = get |> Get.extract Message.decoder
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Member = get |> Get.optional Property.Member GuildMember.Partial.decoder
            Mentions = get |> Get.required Property.Mentions (Decode.list MessageUpdateReceiveEventMention.decoder)
        })

    let encoder (v: MessageUpdateReceiveEvent) =
        Encode.object (
            Message.encodeProperties v.Message
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Member GuildMember.Partial.encoder v.Member
            |> Encode.required Property.Mentions (List.map MessageUpdateReceiveEventMention.encoder >> Encode.list) v.Mentions
        )

module MessageUpdateReceiveEventMention =
    module Property =
        let [<Literal>] Member = "member"

    let decoder: Decoder<MessageUpdateReceiveEventMention> =
        Decode.object (fun get -> {
            User = get |> Get.extract User.decoder
            Member = get |> Get.optional Property.Member GuildMember.Partial.decoder
        })

    let encoder (v: MessageUpdateReceiveEventMention) =
        Encode.object (
            User.encodeProperties v.User
            |> Encode.optional Property.Member GuildMember.Partial.encoder v.Member
        )

module MessageDeleteReceiveEvent =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<MessageDeleteReceiveEvent> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
        })

    let encoder (v: MessageDeleteReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
        )

module MessageDeleteBulkReceiveEvent =
    module Property =
        let [<Literal>] Ids = "ids"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"
    
    let decoder: Decoder<MessageDeleteBulkReceiveEvent> =
        Decode.object (fun get -> {
            Ids = get |> Get.required Property.Ids (Decode.list Decode.string)
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string            
        })

    let encoder (v: MessageDeleteBulkReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.Ids (List.map Encode.string >> Encode.list) v.Ids
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
        )

module MessageReactionAddReceiveEvent =
    module Property =
        let [<Literal>] UserId = "user_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Member = "member"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] MessageAuthorId = "message_author_id"
        let [<Literal>] Burst = "burst"
        let [<Literal>] BurstColors = "burst_colors"
        let [<Literal>] Type = "type"

    let decoder: Decoder<MessageReactionAddReceiveEvent> =
        Decode.object (fun get -> {
            UserId = get |> Get.required Property.UserId Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            MessageId = get |> Get.required Property.MessageId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Member = get |> Get.optional Property.Member GuildMember.decoder
            Emoji = get |> Get.required Property.Emoji Emoji.Partial.decoder
            MessageAuthorId = get |> Get.optional Property.MessageAuthorId Decode.string
            Burst = get |> Get.required Property.Burst Decode.bool
            BurstColors = get |> Get.optional Property.BurstColors (Decode.list Decode.string)
            Type = get |> Get.required Property.Type Decode.Enum.int<ReactionType>
        })

    let encoder (v: MessageReactionAddReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Member GuildMember.encoder v.Member
            |> Encode.required Property.Emoji Emoji.Partial.encoder v.Emoji
            |> Encode.optional Property.MessageAuthorId Encode.string v.MessageAuthorId
            |> Encode.required Property.Burst Encode.bool v.Burst
            |> Encode.optional Property.BurstColors (List.map Encode.string >> Encode.list) v.BurstColors
            |> Encode.required Property.Type Encode.Enum.int v.Type
        )

module MessageReactionRemoveReceiveEvent =
    module Property =
        let [<Literal>] UserId = "user_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] Burst = "burst"
        let [<Literal>] Type = "type"

    let decoder: Decoder<MessageReactionRemoveReceiveEvent> =
        Decode.object (fun get -> {
            UserId = get |> Get.required Property.UserId Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            MessageId = get |> Get.required Property.MessageId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Emoji = get |> Get.required Property.Emoji Emoji.Partial.decoder
            Burst = get |> Get.required Property.Burst Decode.bool
            Type = get |> Get.required Property.Type Decode.Enum.int<ReactionType>
        })

    let encoder (v: MessageReactionRemoveReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Emoji Emoji.Partial.encoder v.Emoji
            |> Encode.required Property.Burst Encode.bool v.Burst
            |> Encode.required Property.Type Encode.Enum.int v.Type
        )

module MessageReactionRemoveAllReceiveEvent =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] GuildId = "guild_id"

    let decoder: Decoder<MessageReactionRemoveAllReceiveEvent> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            MessageId = get |> Get.required Property.MessageId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
        })

    let encoder (v: MessageReactionRemoveAllReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
        )

module MessageReactionRemoveEmojiReceiveEvent =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] Emoji = "emoji"

    let decoder: Decoder<MessageReactionRemoveEmojiReceiveEvent> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            MessageId = get |> Get.required Property.MessageId Decode.string
            Emoji = get |> Get.required Property.Emoji Emoji.Partial.decoder
        })

    let encoder (v: MessageReactionRemoveEmojiReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.MessageId Encode.string v.MessageId
            |> Encode.required Property.Emoji Emoji.Partial.encoder v.Emoji
        )

module PresenceUpdateReceiveEvent =
    module Property =
        let [<Literal>] User = "user"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Status = "status"
        let [<Literal>] Activities = "activities"
        let [<Literal>] ClientStatus = "client_status"

    let decoder: Decoder<PresenceUpdateReceiveEvent> =
        Decode.object (fun get -> {
            User = get |> Get.optional Property.User User.Partial.decoder
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Status = get |> Get.optional Property.Status Status.decoder
            Activities = get |> Get.optional Property.Activities (Decode.list Activity.decoder)
            ClientStatus = get |> Get.optional Property.ClientStatus ClientStatus.decoder
        })

    let encoder (v: PresenceUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.optional Property.User User.Partial.encoder v.User
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.Status Status.encoder v.Status
            |> Encode.optional Property.Activities (List.map Activity.encoder >> Encode.list) v.Activities
            |> Encode.optional Property.ClientStatus ClientStatus.encoder v.ClientStatus
        )

module ClientStatus =
    module Property =
        let [<Literal>] Desktop = "desktop"
        let [<Literal>] Mobile = "mobile"
        let [<Literal>] Web = "web"

    let decoder: Decoder<ClientStatus> =
        Decode.object (fun get -> {
            Desktop = get |> Get.optional Property.Desktop ClientDeviceStatus.decoder
            Mobile = get |> Get.optional Property.Mobile ClientDeviceStatus.decoder
            Web = get |> Get.optional Property.Web ClientDeviceStatus.decoder
        })

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

    let decoder: Decoder<Activity> =
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
            Flags = get |> Get.optional Property.Flags Decode.bitfield
            Buttons = get |> Get.optional Property.Buttons (Decode.list ActivityButton.decoder)
        })

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
            |> Encode.optional Property.Flags Encode.bitfield v.Flags
            |> Encode.optional Property.Buttons (List.map ActivityButton.encoder >> Encode.list) v.Buttons
        )

module ActivityTimestamps =
    module Property =
        let [<Literal>] Start = "start"
        let [<Literal>] End = "end"

    let decoder: Decoder<ActivityTimestamps> =
        Decode.object (fun get -> {
            Start = get |> Get.optional Property.Start UnixTimestamp.decoder
            End = get |> Get.optional Property.End UnixTimestamp.decoder
        })

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

    let decoder: Decoder<ActivityEmoji> =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            Id = get |> Get.optional Property.Id Decode.string
            Animated = get |> Get.optional Property.Animated Decode.bool
        })

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

    let decoder: Decoder<ActivityParty> =
        Decode.object (fun get -> {
            Id = get |> Get.optional Property.Id Decode.string
            Size = get |> Get.optional Property.Size ActivityPartySize.decoder
        })

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

    let decoder: Decoder<ActivityAssets> =
        Decode.object (fun get -> {
            LargeImage = get |> Get.optional Property.LargeImage Decode.string
            LargeText = get |> Get.optional Property.LargeText Decode.string
            SmallImage = get |> Get.optional Property.SmallImage Decode.string
            SmallText = get |> Get.optional Property.SmallText Decode.string
        })

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

    let decoder: Decoder<ActivitySecrets> =
        Decode.object (fun get -> {
            Join = get |> Get.optional Property.Join Decode.string
            Spectate = get |> Get.optional Property.Spectate Decode.string
            Match = get |> Get.optional Property.Match Decode.string
        })

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

    let decoder: Decoder<ActivityButton> =
        Decode.object (fun get -> {
            Label = get |> Get.required Property.Label Decode.string
            Url = get |> Get.required Property.Url Decode.string
        })

    let encoder (v: ActivityButton) =
        Encode.object ([]
            |> Encode.required Property.Label Encode.string v.Label
            |> Encode.required Property.Url Encode.string v.Url
        )

module TypingStartReceiveEvent =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] Timestamp = "timestamp"
        let [<Literal>] Member = "member"

    let decoder: Decoder<TypingStartReceiveEvent> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            UserId = get |> Get.required Property.UserId Decode.string
            Timestamp = get |> Get.required Property.Timestamp UnixTimestamp.decoder
            Member = get |> Get.optional Property.Member GuildMember.decoder
        })

    let encoder (v: TypingStartReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.Timestamp UnixTimestamp.encoder v.Timestamp
            |> Encode.optional Property.Member GuildMember.encoder v.Member
        )

module UserUpdateReceiveEvent =
    let decoder: Decoder<UserUpdateReceiveEvent> = User.decoder
    let encoder (v: UserUpdateReceiveEvent) = User.encoder v

module VoiceChannelEffectSendReceiveEvent =
    module Property =
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] UserId = "user_id"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] AnimationType = "animation_type"
        let [<Literal>] AnimationId = "animation_id"
        let [<Literal>] SoundId = "sound_id"
        let [<Literal>] SoundVolume = "sound_volume"
        
    let decoder: Decoder<VoiceChannelEffectSendReceiveEvent> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            UserId = get |> Get.required Property.UserId Decode.string
            Emoji = get |> Get.optinull Property.Emoji Emoji.decoder
            AnimationType = get |> Get.optinull Property.AnimationType Decode.Enum.int<AnimationType>
            AnimationId = get |> Get.optional Property.AnimationId Decode.string
            SoundId = get |> Get.optional Property.SoundId Decode.string
            SoundVolume = get |> Get.optional Property.SoundVolume Decode.float
        })

    let encoder (v: VoiceChannelEffectSendReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.optinull Property.Emoji Emoji.encoder v.Emoji
            |> Encode.optinull Property.AnimationType Encode.Enum.int v.AnimationType
            |> Encode.optional Property.AnimationId Encode.string v.AnimationId
            |> Encode.optional Property.SoundId Encode.string v.SoundId
            |> Encode.optional Property.SoundVolume Encode.float v.SoundVolume
        )

module VoiceStateUpdateReceiveEvent =
    let decoder: Decoder<VoiceStateUpdateReceiveEvent> = VoiceState.decoder
    let encoder (v: VoiceStateUpdateReceiveEvent) = VoiceState.encoder v

module VoiceServerUpdateReceiveEvent =
    module Property =
        let [<Literal>] Token = "token"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Endpoint = "endpoint"

    let decoder: Decoder<VoiceServerUpdateReceiveEvent> =
        Decode.object (fun get -> {
            Token = get |> Get.required Property.Token Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            Endpoint = get |> Get.nullable Property.Endpoint Decode.string
        })

    let encoder (v: VoiceServerUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.Token Encode.string v.Token
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.nullable Property.Endpoint Encode.string v.Endpoint
        )

module WebhooksUpdateReceiveEvent =
    module Property =
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"

    let decoder: Decoder<WebhooksUpdateReceiveEvent> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
        })

    let encoder (v: WebhooksUpdateReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
        )

module InteractionCreateReceiveEvent =
    let decoder: Decoder<InteractionCreateReceiveEvent> = Interaction.decoder
    let encoder (v: InteractionCreateReceiveEvent) = Interaction.encoder v

module StageInstanceCreateReceiveEvent =
    let decoder: Decoder<StageInstanceCreateReceiveEvent> = StageInstance.decoder
    let encoder (v: StageInstanceCreateReceiveEvent) = StageInstance.encoder v

module StageInstanceUpdateReceiveEvent =
    let decoder: Decoder<StageInstanceUpdateReceiveEvent> = StageInstance.decoder
    let encoder (v: StageInstanceUpdateReceiveEvent) = StageInstance.encoder v

module StageInstanceDeleteReceiveEvent =
    let decoder: Decoder<StageInstanceDeleteReceiveEvent> = StageInstance.decoder
    let encoder (v: StageInstanceDeleteReceiveEvent) = StageInstance.encoder v

module SubscriptionCreateReceiveEvent =
    let decoder: Decoder<SubscriptionCreateReceiveEvent> = Subscription.decoder
    let encoder (v: SubscriptionCreateReceiveEvent) = Subscription.encoder v

module SubscriptionUpdateReceiveEvent =
    let decoder: Decoder<SubscriptionUpdateReceiveEvent> = Subscription.decoder
    let encoder (v: SubscriptionUpdateReceiveEvent) = Subscription.encoder v

module SubscriptionDeleteReceiveEvent =
    let decoder: Decoder<SubscriptionDeleteReceiveEvent> = Subscription.decoder
    let encoder (v: SubscriptionDeleteReceiveEvent) = Subscription.encoder v

module MessagePollVoteAddReceiveEvent =
    module Property =
        let [<Literal>] UserId = "user_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] AnswerId = "answer_id"

    let decoder: Decoder<MessagePollVoteAddReceiveEvent> =
        Decode.object (fun get -> {
            UserId = get |> Get.required Property.UserId Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            MessageId = get |> Get.required Property.MessageId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            AnswerId = get |> Get.required Property.AnswerId Decode.int
        })

    let encoder (v: MessagePollVoteAddReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.AnswerId Encode.int v.AnswerId
        )

module MessagePollVoteRemoveReceiveEvent =
    module Property =
        let [<Literal>] UserId = "user_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] AnswerId = "answer_id"

    let decoder: Decoder<MessagePollVoteRemoveReceiveEvent> =
        Decode.object (fun get -> {
            UserId = get |> Get.required Property.UserId Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            MessageId = get |> Get.required Property.MessageId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            AnswerId = get |> Get.required Property.AnswerId Decode.int
        })

    let encoder (v: MessagePollVoteRemoveReceiveEvent) =
        Encode.object ([]
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.AnswerId Encode.int v.AnswerId
        )

module WebhookEventPayload =
    module Property =
        let [<Literal>] Version = "version"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Event = "event"

    let decoder: Decoder<WebhookEventPayload> =
        Decode.object (fun get -> {
            Version = get |> Get.required Property.Version Decode.int
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<WebhookPayloadType>
            Event = get |> Get.optional Property.Event WebhookEventBody.decoder
        })

    let encoder (v: WebhookEventPayload) =
        Encode.object ([]
            |> Encode.required Property.Version Encode.int v.Version
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.required Property.Type Encode.Enum.int<WebhookPayloadType> v.Type
            |> Encode.optional Property.Event WebhookEventBody.encoder v.Event
        )

module WebhookEventBody =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] Timestamp = "timestamp"
        let [<Literal>] Data = "data"

    let decoder: Decoder<WebhookEventBody> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type WebhookEventType.decoder
            Timestamp = get |> Get.required Property.Timestamp Decode.datetimeUtc
            Data = get |> Get.optional Property.Data WebhookEventData.decoder
        })

    let encoder (v: WebhookEventBody) =
        Encode.object ([]
            |> Encode.required Property.Type WebhookEventType.encoder v.Type
            |> Encode.required Property.Timestamp Encode.datetime v.Timestamp
            |> Encode.optional Property.Data WebhookEventData.encoder v.Data
        )

module WebhookEventData =
    let decoder: Decoder<WebhookEventData> =
        Decode.oneOf [
            Decode.map WebhookEventData.APPLICATION_AUTHORIZED ApplicationAuthorizedEvent.decoder
            Decode.map WebhookEventData.ENTITLEMENT_CREATE EntitlementCreateEvent.decoder
        ]

    let encoder (v: WebhookEventData) =
        match v with
        | WebhookEventData.APPLICATION_AUTHORIZED d -> ApplicationAuthorizedEvent.encoder d
        | WebhookEventData.ENTITLEMENT_CREATE d -> EntitlementCreateEvent.encoder d

module ApplicationAuthorizedEvent =
    module Property =
        let [<Literal>] IntegrationType = "integration_type"
        let [<Literal>] User = "user"
        let [<Literal>] Scopes = "scopes"
        let [<Literal>] Guild = "guild"

    let decoder: Decoder<ApplicationAuthorizedEvent> =
        Decode.object (fun get -> {
            IntegrationType = get |> Get.optional Property.IntegrationType Decode.Enum.int<ApplicationIntegrationType>
            User = get |> Get.required Property.User User.decoder
            Scopes = get |> Get.required Property.Scopes (Decode.list OAuthScope.decoder)
            Guild = get |> Get.optional Property.Guild Guild.decoder
        })

    let encoder (v: ApplicationAuthorizedEvent) =
        Encode.object ([]
            |> Encode.optional Property.IntegrationType Encode.Enum.int<ApplicationIntegrationType> v.IntegrationType
            |> Encode.required Property.User User.encoder v.User
            |> Encode.required Property.Scopes (List.map OAuthScope.encoder >> Encode.list) v.Scopes
            |> Encode.optional Property.Guild Guild.encoder v.Guild
        )

module EntitlementCreateEvent =
    let decoder: Decoder<EntitlementCreateEvent> = Entitlement.decoder
    let encoder (v: EntitlementCreateEvent) = Entitlement.encoder v

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

    let decoder: Decoder<Application> =
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
            Flags = get |> Get.optional Property.Flags Decode.bitfield
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
        })

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
            |> Encode.optional Property.Flags Encode.bitfield v.Flags
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
        let decoder: Decoder<PartialApplication> =
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
                Flags = get |> Get.optional Property.Flags Decode.bitfield
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
            })

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
                |> Encode.optional Property.Flags Encode.bitfield v.Flags
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

    let decoder: Decoder<ApplicationIntegrationTypeConfiguration> =
        Decode.object (fun get -> {
            OAuth2InstallParams = get |> Get.optional Property.OAuth2InstallParams InstallParams.decoder
        })

    let encoder (v: ApplicationIntegrationTypeConfiguration) =
        Encode.object ([]
            |> Encode.optional Property.OAuth2InstallParams InstallParams.encoder v.OAuth2InstallParams
        )

module InstallParams =
    module Property =
        let [<Literal>] Scopes = "scopes"
        let [<Literal>] Permissions = "permissions"

    let decoder: Decoder<InstallParams> =
        Decode.object (fun get -> {
            Scopes = get |> Get.required Property.Scopes (Decode.list OAuthScope.decoder)
            Permissions = get |> Get.required Property.Permissions Decode.bitfieldL<Permission>
        })

    let encoder (v: InstallParams) =
        Encode.object ([]
            |> Encode.required Property.Scopes (List.map OAuthScope.encoder >> Encode.list) v.Scopes
            |> Encode.required Property.Permissions Encode.bitfieldL v.Permissions
        )

module ActivityInstance =
    module Property =
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] InstanceId = "instance_id"
        let [<Literal>] LaunchId = "launch_id"
        let [<Literal>] Location = "location"
        let [<Literal>] Users = "users"

    let decoder: Decoder<ActivityInstance> =
        Decode.object (fun get -> {
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            InstanceId = get |> Get.required Property.InstanceId Decode.string
            LaunchId = get |> Get.required Property.LaunchId Decode.string
            Location = get |> Get.required Property.Location ActivityLocation.decoder
            Users = get |> Get.required Property.Users (Decode.list Decode.string)
        })

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

    let decoder: Decoder<ActivityLocation> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Kind = get |> Get.required Property.Kind ActivityLocationKind.decoder
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            GuildId = get |> Get.optinull Property.GuildId Decode.string
        })

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

    let decoder: Decoder<ApplicationRoleConnectionMetadata> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<ApplicationRoleConnectionMetadataType>
            Key = get |> Get.required Property.Key Decode.string
            Name = get |> Get.required Property.Name Decode.string
            NameLocalizations = get |> Get.optional Property.NameLocalizations (Decode.dict Decode.string)
            Description = get |> Get.required Property.Description Decode.string
            DescriptionLocalizations = get |> Get.optional Property.DescriptionLocalizations (Decode.dict Decode.string)
        })

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

    let decoder: Decoder<AuditLog> =
        Decode.object (fun get -> {
            ApplicationCommands = get |> Get.required Property.ApplicationCommands (Decode.list ApplicationCommand.decoder)
            AuditLogEntries = get |> Get.required Property.AuditLogEntries (Decode.list AuditLogEntry.decoder)
            AutoModerationRules = get |> Get.required Property.AutoModerationRules (Decode.list AutoModerationRule.decoder)
            GuildScheduledEvents = get |> Get.required Property.GuildScheduledEvents (Decode.list GuildScheduledEvent.decoder)
            Integrations = get |> Get.required Property.Integrations (Decode.list Integration.Partial.decoder)
            Threads = get |> Get.required Property.Threads (Decode.list Channel.decoder)
            Users = get |> Get.required Property.Users (Decode.list User.decoder)
            Webhooks = get |> Get.required Property.Webhooks (Decode.list Webhook.decoder)
        })

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

    let decoder: Decoder<AuditLogEntry> =
        Decode.object (fun get -> {
            TargetId = get |> Get.nullable Property.TargetId Decode.string
            Changes = get |> Get.optional Property.Changes (Decode.list AuditLogChange.decoder)
            UserId = get |> Get.nullable Property.UserId Decode.string
            Id = get |> Get.required Property.Id Decode.string
            ActionType = get |> Get.required Property.ActionType Decode.Enum.int<AuditLogEventType>
            Options = get |> Get.optional Property.Options AuditLogEntryOptionalInfo.decoder
            Reason = get |> Get.optional Property.Reason Decode.string
        })

    let internal encodeProperties (v: AuditLogEntry) =
        []
        |> Encode.nullable Property.TargetId Encode.string v.TargetId
        |> Encode.optional Property.Changes (List.map AuditLogChange.encoder >> Encode.list) v.Changes
        |> Encode.nullable Property.UserId Encode.string v.UserId
        |> Encode.required Property.Id Encode.string v.Id
        |> Encode.required Property.ActionType Encode.Enum.int v.ActionType
        |> Encode.optional Property.Options AuditLogEntryOptionalInfo.encoder v.Options
        |> Encode.optional Property.Reason Encode.string v.Reason

    let encoder (v: AuditLogEntry) =
        Encode.object (encodeProperties v)

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

    let decoder: Decoder<AuditLogEntryOptionalInfo> =
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
        })

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

    let decoder: Decoder<AuditLogChange> =
        Decode.object (fun get -> {
            NewValue = None
            OldValue = None
            Key = get |> Get.required Property.Key Decode.string
        })

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

    let decoder: Decoder<AutoModerationRule> =
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
        })

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

    let decoder: Decoder<AutoModerationTriggerMetadata> =
        Decode.object (fun get -> {
            KeywordFilter = get |> Get.optional Property.KeywordFilter (Decode.list Decode.string)
            RegexPatterns = get |> Get.optional Property.RegexPatterns (Decode.list Decode.string)
            Presets = get |> Get.optional Property.Presets (Decode.list Decode.Enum.int<AutoModerationKeywordPreset>)
            AllowList = get |> Get.optional Property.AllowList (Decode.list Decode.string)
            MentionTotalLimit = get |> Get.optional Property.MentionTotalLimit Decode.int
            MentionRaidProtectionEnabled = get |> Get.optional Property.MentionRaidProtectionEnabled Decode.bool
        })

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

    let decoder: Decoder<AutoModerationAction> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<AutoModerationActionType>
            Metadata = get |> Get.optional Property.Metadata AutoModerationActionMetadata.decoder
        })

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

    let decoder: Decoder<AutoModerationActionMetadata> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.optional Property.ChannelId Decode.string
            DurationSeconds = get |> Get.optional Property.DurationSeconds Decode.int
            CustomMessage = get |> Get.optional Property.CustomMessage Decode.string
        })

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

    let decoder: Decoder<Channel> =
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
            Permissions = get |> Get.optional Property.Permissions Decode.bitfieldL<Permission>
            Flags = get |> Get.optional Property.Flags Decode.bitfield
            TotalMessagesSent = get |> Get.optional Property.TotalMessagesSent Decode.int
            AvailableTags = get |> Get.optional Property.AvailableTags (Decode.list ForumTag.decoder)
            AppliedTags = get |> Get.optional Property.AppliedTags (Decode.list Decode.string)
            DefaultReactionEmoji = get |> Get.optinull Property.DefaultReactionEmoji DefaultReaction.decoder
            DefaultThreadRateLimitPerUser = get |> Get.optional Property.DefaultThreadRateLimitPerUser Decode.int
            DefaultSortOrder = get |> Get.optinull Property.DefaultSortOrder Decode.Enum.int<ChannelSortOrder>
            DefaultForumLayout = get |> Get.optional Property.DefaultForumLayout Decode.Enum.int<ForumLayout>
        })

    let internal encodeProperties (v: Channel) =
        []
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
        |> Encode.optional Property.Permissions Encode.bitfieldL v.Permissions
        |> Encode.optional Property.Flags Encode.bitfield v.Flags
        |> Encode.optional Property.TotalMessagesSent Encode.int v.TotalMessagesSent
        |> Encode.optional Property.AvailableTags (List.map ForumTag.encoder >> Encode.list) v.AvailableTags
        |> Encode.optional Property.AppliedTags (List.map Encode.string >> Encode.list) v.AppliedTags
        |> Encode.optinull Property.DefaultReactionEmoji DefaultReaction.encoder v.DefaultReactionEmoji
        |> Encode.optional Property.DefaultThreadRateLimitPerUser Encode.int v.DefaultThreadRateLimitPerUser
        |> Encode.optinull Property.DefaultSortOrder Encode.Enum.int v.DefaultSortOrder
        |> Encode.optional Property.DefaultForumLayout Encode.Enum.int v.DefaultForumLayout

    let encoder (v: Channel) =
        Encode.object (encodeProperties v)

    module Partial =
        let decoder: Decoder<PartialChannel> =
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
                Permissions = get |> Get.optional Property.Permissions Decode.bitfieldL<Permission>
                Flags = get |> Get.optional Property.Flags Decode.bitfield
                TotalMessagesSent = get |> Get.optional Property.TotalMessagesSent Decode.int
                AvailableTags = get |> Get.optional Property.AvailableTags (Decode.list ForumTag.decoder)
                AppliedTags = get |> Get.optional Property.AppliedTags (Decode.list Decode.string)
                DefaultReactionEmoji = get |> Get.optinull Property.DefaultReactionEmoji DefaultReaction.decoder
                DefaultThreadRateLimitPerUser = get |> Get.optional Property.DefaultThreadRateLimitPerUser Decode.int
                DefaultSortOrder = get |> Get.optinull Property.DefaultSortOrder Decode.Enum.int<ChannelSortOrder>
                DefaultForumLayout = get |> Get.optional Property.DefaultForumLayout Decode.Enum.int<ForumLayout>
            })

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
                |> Encode.optional Property.Permissions Encode.bitfieldL v.Permissions
                |> Encode.optional Property.Flags Encode.bitfield v.Flags
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

    let decoder: Decoder<FollowedChannel> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            WebhookId = get |> Get.required Property.WebhookId Decode.string
        })

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

    let decoder: Decoder<PermissionOverwrite> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<PermissionOverwriteType>
            Allow = get |> Get.required Property.Allow Decode.bitfieldL<Permission>
            Deny = get |> Get.required Property.Deny Decode.bitfieldL<Permission>
        })

    let encoder (v: PermissionOverwrite) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Allow Encode.bitfieldL v.Allow
            |> Encode.required Property.Deny Encode.bitfieldL v.Deny
        )

    module Partial =
        let decoder: Decoder<PartialPermissionOverwrite> =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Type = get |> Get.optional Property.Type Decode.Enum.int<PermissionOverwriteType>
                Allow = get |> Get.optional Property.Allow Decode.bitfieldL<Permission>
                Deny = get |> Get.optional Property.Deny Decode.bitfieldL<Permission>
            })

        let encoder (v: PartialPermissionOverwrite) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.Type Encode.Enum.int v.Type
                |> Encode.optional Property.Allow Encode.bitfieldL v.Allow
                |> Encode.optional Property.Deny Encode.bitfieldL v.Deny
            )

module ThreadMetadata =
    module Property =
        let [<Literal>] Archived = "archived"
        let [<Literal>] AutoArchiveDuration = "auto_archive_duration"
        let [<Literal>] ArchiveTimestamp = "archive_timestamp"
        let [<Literal>] Locked = "locked"
        let [<Literal>] Invitable = "invitable"
        let [<Literal>] CreateTimestamp = "create_timestamp"

    let decoder: Decoder<ThreadMetadata> =
        Decode.object (fun get -> {
            Archived = get |> Get.required Property.Archived Decode.bool
            AutoArchiveDuration = get |> Get.required Property.AutoArchiveDuration Decode.Enum.int<AutoArchiveDuration>
            ArchiveTimestamp = get |> Get.required Property.ArchiveTimestamp Decode.datetimeUtc
            Locked = get |> Get.required Property.Locked Decode.bool
            Invitable = get |> Get.optional Property.Invitable Decode.bool
            CreateTimestamp = get |> Get.optinull Property.CreateTimestamp Decode.datetimeUtc
        })

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

    let decoder: Decoder<ThreadMember> =
        Decode.object (fun get -> {
            Id = get |> Get.optional Property.Id Decode.string
            UserId = get |> Get.optional Property.UserId Decode.string
            JoinTimestamp = get |> Get.required Property.JoinTimestamp Decode.datetimeUtc
            Flags = get |> Get.required Property.Flags Decode.int
            Member = get |> Get.optional Property.Member GuildMember.decoder
        })

    let internal encodeProperties (v: ThreadMember) =
        []
        |> Encode.optional Property.Id Encode.string v.Id
        |> Encode.optional Property.UserId Encode.string v.UserId
        |> Encode.required Property.JoinTimestamp Encode.datetime v.JoinTimestamp
        |> Encode.required Property.Flags Encode.int v.Flags
        |> Encode.optional Property.Member GuildMember.encoder v.Member

    let encoder (v: ThreadMember) =
        Encode.object (encodeProperties v)

module DefaultReaction =
    module Property =
        let [<Literal>] EmojiId = "emoji_id"
        let [<Literal>] EmojiName = "emoji_name"

    let decoder: Decoder<DefaultReaction> =
        Decode.object (fun get -> {
            EmojiId = get |> Get.nullable Property.EmojiId Decode.string
            EmojiName = get |> Get.nullable Property.EmojiName Decode.string
        })

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

    let decoder: Decoder<ForumTag> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Moderated = get |> Get.required Property.Moderated Decode.bool
            EmojiId = get |> Get.nullable Property.EmojiId Decode.string
            EmojiName = get |> Get.nullable Property.EmojiName Decode.string
        })

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

    let decoder: Decoder<Emoji> =
        Decode.object (fun get -> {
            Id = get |> Get.nullable Property.Id Decode.string
            Name = get |> Get.nullable Property.Name Decode.string
            Roles = get |> Get.optional Property.Roles (Decode.list Decode.string)
            User = get |> Get.optional Property.User User.decoder
            RequireColons = get |> Get.optional Property.RequireColons Decode.bool
            Managed = get |> Get.optional Property.Managed Decode.bool
            Animated = get |> Get.optional Property.Animated Decode.bool
            Available = get |> Get.optional Property.Available Decode.bool
        })

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
        let decoder: Decoder<PartialEmoji> =
            Decode.object (fun get -> {
                Id = get |> Get.nullable Property.Id Decode.string
                Name = get |> Get.nullable Property.Name Decode.string
                Roles = get |> Get.optional Property.Roles (Decode.list Decode.string)
                User = get |> Get.optional Property.User User.decoder
                RequireColons = get |> Get.optional Property.RequireColons Decode.bool
                Managed = get |> Get.optional Property.Managed Decode.bool
                Animated = get |> Get.optional Property.Animated Decode.bool
                Available = get |> Get.optional Property.Available Decode.bool
            })

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

    let decoder: Decoder<Entitlement> =
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
        })

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

    let decoder: Decoder<Guild> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Icon = get |> Get.nullable Property.Icon Decode.string
            IconHash = get |> Get.optinull Property.IconHash Decode.string
            Splash = get |> Get.nullable Property.Splash Decode.string
            DiscoverySplash = get |> Get.nullable Property.DiscoverySplash Decode.string
            Owner = get |> Get.optional Property.Owner Decode.bool
            OwnerId = get |> Get.required Property.OwnerId Decode.string
            Permissions = get |> Get.optional Property.Permissions Decode.bitfieldL<Permission>
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
            SystemChannelFlags = get |> Get.required Property.SystemChannelFlags Decode.bitfield
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
        })

    let internal encodeProperties (v: Guild) =
        []
        |> Encode.required Property.Id Encode.string v.Id
        |> Encode.required Property.Name Encode.string v.Name
        |> Encode.nullable Property.Icon Encode.string v.Icon
        |> Encode.optinull Property.IconHash Encode.string v.IconHash
        |> Encode.nullable Property.Splash Encode.string v.Splash
        |> Encode.nullable Property.DiscoverySplash Encode.string v.DiscoverySplash
        |> Encode.optional Property.Owner Encode.bool v.Owner
        |> Encode.required Property.OwnerId Encode.string v.OwnerId
        |> Encode.optional Property.Permissions Encode.bitfieldL v.Permissions
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
        |> Encode.required Property.SystemChannelFlags Encode.bitfield v.SystemChannelFlags
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

    let encoder (v: Guild) =
        Encode.object (encodeProperties v)

    module Partial =
        let decoder: Decoder<PartialGuild> =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Name = get |> Get.optional Property.Name Decode.string
                Icon = get |> Get.optinull Property.Icon Decode.string
                IconHash = get |> Get.optinull Property.IconHash Decode.string
                Splash = get |> Get.optinull Property.Splash Decode.string
                DiscoverySplash = get |> Get.optinull Property.DiscoverySplash Decode.string
                Owner = get |> Get.optional Property.Owner Decode.bool
                OwnerId = get |> Get.optional Property.OwnerId Decode.string
                Permissions = get |> Get.optional Property.Permissions Decode.bitfieldL<Permission>
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
                SystemChannelFlags = get |> Get.optional Property.SystemChannelFlags Decode.bitfield
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
            })

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
            |> Encode.optional Property.Permissions Encode.bitfieldL v.Permissions
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
            |> Encode.optional Property.SystemChannelFlags Encode.bitfield v.SystemChannelFlags
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

    module Unavailable =
        module Property =
            let [<Literal>] Id = "id"
            let [<Literal>] Unavailable = "unavailable"

        let decoder: Decoder<UnavailableGuild> =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Unavailable = get |> Get.required Property.Unavailable Decode.bool
            })

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

    let decoder: Decoder<GuildPreview> =
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
        })

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

    let decoder: Decoder<GuildWidgetSettings> =
        Decode.object (fun get -> {
            Enabled = get |> Get.required Property.Enabled Decode.bool
            ChannelId = get |> Get.nullable Property.ChannelId Decode.string
        })

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

    let decoder: Decoder<GuildWidget> =
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

    let decoder: Decoder<GuildMember> =
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
            Flags = get |> Get.required Property.Flags Decode.bitfield
            Pending = get |> Get.optional Property.Pending Decode.bool
            Permissions = get |> Get.optional Property.Permissions Decode.bitfieldL<Permission>
            CommunicationDisabledUntil = get |> Get.optinull Property.CommunicationDisabledUntil Decode.datetimeUtc
            AvatarDecorationData = get |> Get.optinull Property.AvatarDecorationData AvatarDecorationData.decoder
        })

    let internal encodeProperties (v: GuildMember) =
        []
        |> Encode.optional Property.User User.encoder v.User
        |> Encode.optinull Property.Nick Encode.string v.Nick
        |> Encode.optinull Property.Avatar Encode.string v.Avatar
        |> Encode.optinull Property.Banner Encode.string v.Banner
        |> Encode.required Property.Roles (List.map Encode.string >> Encode.list) v.Roles
        |> Encode.required Property.JoinedAt Encode.datetime v.JoinedAt
        |> Encode.optinull Property.PremiumSince Encode.datetime v.PremiumSince
        |> Encode.required Property.Deaf Encode.bool v.Deaf
        |> Encode.required Property.Mute Encode.bool v.Mute
        |> Encode.required Property.Flags Encode.bitfield v.Flags
        |> Encode.optional Property.Pending Encode.bool v.Pending
        |> Encode.optional Property.Permissions Encode.bitfieldL v.Permissions
        |> Encode.optinull Property.CommunicationDisabledUntil Encode.datetime v.CommunicationDisabledUntil
        |> Encode.optinull Property.AvatarDecorationData AvatarDecorationData.encoder v.AvatarDecorationData

    let encoder (v: GuildMember) =
        Encode.object (encodeProperties v)

    module Partial =
        let decoder: Decoder<PartialGuildMember> =
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
                Flags = get |> Get.optional Property.Flags Decode.bitfield
                Pending = get |> Get.optional Property.Pending Decode.bool
                Permissions = get |> Get.optional Property.Permissions Decode.bitfieldL<Permission>
                CommunicationDisabledUntil = get |> Get.optinull Property.CommunicationDisabledUntil Decode.datetimeUtc
                AvatarDecorationData = get |> Get.optinull Property.AvatarDecorationData AvatarDecorationData.decoder
            })

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
                |> Encode.optional Property.Flags Encode.bitfield v.Flags
                |> Encode.optional Property.Pending Encode.bool v.Pending
                |> Encode.optional Property.Permissions Encode.bitfieldL v.Permissions
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

    let decoder: Decoder<Integration> =
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
        })

    let internal encodeProperties (v: Integration) =
        []
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

    let encoder (v: Integration) =
        Encode.object (encodeProperties v)

    module Partial =
        let decoder: Decoder<PartialIntegration> =
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
            })

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

    let decoder: Decoder<IntegrationAccount> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
        })

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

    let decoder: Decoder<IntegrationApplication> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Icon = get |> Get.nullable Property.Icon Decode.string
            Description = get |> Get.required Property.Description Decode.string
            Bot = get |> Get.optional Property.Bot User.decoder
        })

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

    let decoder: Decoder<Ban> =
        Decode.object (fun get -> {
            Reason = get |> Get.nullable Property.Reason Decode.string
            User = get |> Get.required Property.User User.decoder
        })
        
    let encoder (v: Ban) =
        Encode.object ([]
            |> Encode.nullable Property.Reason Encode.string v.Reason
            |> Encode.required Property.User User.encoder v.User
        )

module WelcomeScreen =
    module Property =
        let [<Literal>] Description = "description"
        let [<Literal>] WelcomeChannels = "welcome_channels"

    let decoder: Decoder<WelcomeScreen> =
        Decode.object (fun get -> {
            Description = get |> Get.nullable Property.Description Decode.string
            WelcomeChannels = get |> Get.required Property.WelcomeChannels (Decode.list WelcomeScreenChannel.decoder)
        })

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

    let decoder: Decoder<WelcomeScreenChannel> =
        Decode.object (fun get -> {
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            Description = get |> Get.required Property.Description Decode.string
            EmojiId = get |> Get.nullable Property.EmojiId Decode.string
            EmojiName = get |> Get.nullable Property.EmojiName Decode.string
        })

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

    let decoder: Decoder<GuildOnboarding> =
        Decode.object (fun get -> {
            GuildId = get |> Get.required Property.GuildId Decode.string
            Prompts = get |> Get.required Property.Prompts (Decode.list GuildOnboardingPrompt.decoder)
            DefaultChannelIds = get |> Get.required Property.DefaultChannelIds (Decode.list Decode.string)
            Enabled = get |> Get.required Property.Enabled Decode.bool
            Mode = get |> Get.required Property.Mode Decode.Enum.int<OnboardingMode>
        })

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

    let decoder: Decoder<GuildOnboardingPrompt> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<OnboardingPromptType>
            Options = get |> Get.required Property.Options (Decode.list GuildOnboardingPromptOption.decoder)
            Title = get |> Get.required Property.Title Decode.string
            SingleSelect = get |> Get.required Property.SingleSelect Decode.bool
            Required = get |> Get.required Property.Required Decode.bool
            InOnboarding = get |> Get.required Property.InOnboarding Decode.bool
        })

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

    let decoder: Decoder<GuildOnboardingPromptOption> =
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
        })

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

    let decoder: Decoder<IncidentsData> =
        Decode.object (fun get -> {
            InvitesDisabledUntil = get |> Get.nullable Property.InvitesDisabledUntil Decode.datetimeUtc
            DmsDisabledUntil = get |> Get.nullable Property.DmsDisabledUntil Decode.datetimeUtc
            DmSpamDetectedAt = get |> Get.optinull Property.DmSpamDetectedAt Decode.datetimeUtc
            RaidDetectedAt = get |> Get.optinull Property.RaidDetectedAt Decode.datetimeUtc
        })

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

    let decoder: Decoder<GuildScheduledEvent> =
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
        })

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

    let decoder: Decoder<EntityMetadata> =
        Decode.object (fun get -> {
            Location = get |> Get.optional Property.Location Decode.string
        })

    let encoder (v: EntityMetadata) =
        Encode.object ([]
            |> Encode.optional Property.Location Encode.string v.Location
        )

module GuildScheduledEventUser =
    module Property =
        let [<Literal>] GuildScheduledEventId = "guild_scheduled_event_id"
        let [<Literal>] User = "user"
        let [<Literal>] Member = "member"

    let decoder: Decoder<GuildScheduledEventUser> =
        Decode.object (fun get -> {
            GuildScheduledEventId = get |> Get.required Property.GuildScheduledEventId Decode.string
            User = get |> Get.required Property.User User.decoder
            Member = get |> Get.optional Property.Member GuildMember.decoder
        })

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

    let decoder: Decoder<RecurrenceRule> =
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
        })

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

    let decoder: Decoder<RecurrenceRuleNWeekday> =
        Decode.object (fun get -> {
            N = get |> Get.required Property.N Decode.int
            Day = get |> Get.required Property.Day Decode.Enum.int<RecurrenceRuleWeekday>
        })

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

    let decoder: Decoder<GuildTemplate> =
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
        })

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

    let decoder: Decoder<Invite> =
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
        })

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

    let decoder: Decoder<InviteMetadata> =
        Decode.object (fun get -> {
            Uses = get |> Get.required Property.Uses Decode.int
            MaxUses = get |> Get.required Property.MaxUses Decode.int
            MaxAge = get |> Get.required Property.MaxAge Decode.int
            Temporary = get |> Get.required Property.Temporary Decode.bool
            CreatedAt = get |> Get.required Property.CreatedAt Decode.datetimeUtc
        })

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
    let decoder: Decoder<InviteWithMetadata> =
        Decode.object (fun get -> {
            Invite = get |> Get.extract Invite.decoder
            Metadata = get |> Get.extract InviteMetadata.decoder
        })

    let encoder (v: InviteWithMetadata) =
        Encode.object (Invite.encodeProperties v.Invite @ InviteMetadata.encodeProperties v.Metadata)

    // TODO: Should invite metadata be separated, or should this be treated like old ExtraFields types?

module Lobby =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Metadata = "metadata"
        let [<Literal>] Members = "members"
        let [<Literal>] LinkedChannel = "linked_channel"

    let decoder: Decoder<Lobby> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            Metadata = get |> Get.nullable Property.Metadata (Decode.dict Decode.string)
            Members = get |> Get.required Property.Members (Decode.list LobbyMember.decoder)
            LinkedChannel = get |> Get.optional Property.LinkedChannel Channel.decoder
        })

    let encoder (v: Lobby) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.nullable Property.Metadata (Encode.mapv Encode.string) v.Metadata
            |> Encode.required Property.Members (List.map LobbyMember.encoder >> Encode.list) v.Members
            |> Encode.optional Property.LinkedChannel Channel.encoder v.LinkedChannel
        )

module LobbyMember =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Metadata = "metadata"
        let [<Literal>] Flags = "flags"

    let decoder: Decoder<LobbyMember> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Metadata = get |> Get.optinull Property.Metadata (Decode.dict Decode.string)
            Flags = get |> Get.optional Property.Flags Decode.bitfield
        })

    let encoder (v: LobbyMember) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.optinull Property.Metadata (Encode.mapv Encode.string) v.Metadata
            |> Encode.optional Property.Flags Encode.bitfield v.Flags
        )

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
        let [<Literal>] ApplicationId = "application_id"
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

    let decoder: Decoder<Message> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            Author = get |> Get.required Property.Author User.decoder
            Content = get |> Get.nullable Property.Content Decode.string
            Timestamp = get |> Get.required Property.Timestamp Decode.datetimeUtc
            EditedTimestamp = get |> Get.nullable Property.EditedTimestamp Decode.datetimeUtc
            Tts = get |> Get.required Property.Tts Decode.bool
            MentionEveryone = get |> Get.required Property.MentionEveryone Decode.bool
            Mentions = get |> Get.required Property.Mentions (Decode.list User.decoder)
            MentionRoles = get |> Get.required Property.MentionRoles (Decode.list Decode.string)
            MentionChannels = get |> Get.optional Property.MentionChannels (Decode.list ChannelMention.decoder)
            Attachments = get |> Get.required Property.Attachments (Decode.list Attachment.decoder)
            Embeds = get |> Get.required Property.Embeds (Decode.list Embed.decoder)
            Reactions = get |> Get.optional Property.Reactions (Decode.list Reaction.decoder)
            Nonce = get |> Get.optional Property.Nonce MessageNonce.decoder
            Pinned = get |> Get.required Property.Pinned Decode.bool
            WebhookId = get |> Get.optional Property.WebhookId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<MessageType>
            Activity = get |> Get.optional Property.Activity MessageActivity.decoder
            Application = get |> Get.optional Property.Application Application.Partial.decoder
            ApplicationId = get |> Get.optional Property.ApplicationId Decode.string
            Flags = get |> Get.optional Property.Flags Decode.bitfield
            MessageReference = get |> Get.optional Property.MessageReference MessageReference.decoder
            MessageSnapshots = get |> Get.optional Property.MessageSnapshots (Decode.list MessageSnapshot.decoder)
            ReferencedMessage = get |> Get.optinull Property.ReferencedMessage decoder
            InteractionMetadata = get |> Get.optional Property.InteractionMetadata MessageInteractionMetadata.decoder
            Interaction = get |> Get.optional Property.Interaction MessageInteraction.decoder
            Thread = get |> Get.optional Property.Thread Channel.decoder
            Components = get |> Get.optional Property.Components (Decode.list Component.decoder)
            StickerItems = get |> Get.optional Property.StickerItems (Decode.list StickerItem.decoder)
            Position = get |> Get.optional Property.Position Decode.int
            RoleSubscriptionData = get |> Get.optional Property.RoleSubscriptionData RoleSubscriptionData.decoder
            Resolved = get |> Get.optional Property.Resolved ResolvedData.decoder
            Poll = get |> Get.optional Property.Poll Poll.decoder
            Call = get |> Get.optional Property.Call MessageCall.decoder
        })

    let internal encodeProperties (v: Message) =
        []
        |> Encode.required Property.Id Encode.string v.Id
        |> Encode.required Property.ChannelId Encode.string v.ChannelId
        |> Encode.required Property.Author User.encoder v.Author
        |> Encode.nullable Property.Content Encode.string v.Content
        |> Encode.required Property.Timestamp Encode.datetime v.Timestamp
        |> Encode.nullable Property.EditedTimestamp Encode.datetime v.EditedTimestamp
        |> Encode.required Property.Tts Encode.bool v.Tts
        |> Encode.required Property.MentionEveryone Encode.bool v.MentionEveryone
        |> Encode.required Property.Mentions (List.map User.encoder >> Encode.list) v.Mentions
        |> Encode.required Property.MentionRoles (List.map Encode.string >> Encode.list) v.MentionRoles
        |> Encode.optional Property.MentionChannels (List.map ChannelMention.encoder >> Encode.list) v.MentionChannels
        |> Encode.required Property.Attachments (List.map Attachment.encoder >> Encode.list) v.Attachments
        |> Encode.required Property.Embeds (List.map Embed.encoder >> Encode.list) v.Embeds
        |> Encode.optional Property.Reactions (List.map Reaction.encoder >> Encode.list) v.Reactions
        |> Encode.optional Property.Nonce MessageNonce.encoder v.Nonce
        |> Encode.required Property.Pinned Encode.bool v.Pinned
        |> Encode.optional Property.WebhookId Encode.string v.WebhookId
        |> Encode.required Property.Type Encode.Enum.int v.Type
        |> Encode.optional Property.Activity MessageActivity.encoder v.Activity
        |> Encode.optional Property.Application Application.Partial.encoder v.Application
        |> Encode.optional Property.ApplicationId Encode.string v.ApplicationId
        |> Encode.optional Property.Flags Encode.bitfield v.Flags
        |> Encode.optional Property.MessageReference MessageReference.encoder v.MessageReference
        |> Encode.optional Property.MessageSnapshots (List.map MessageSnapshot.encoder >> Encode.list) v.MessageSnapshots
        |> Encode.optinull Property.ReferencedMessage encoder v.ReferencedMessage
        |> Encode.optional Property.InteractionMetadata MessageInteractionMetadata.encoder v.InteractionMetadata
        |> Encode.optional Property.Interaction MessageInteraction.encoder v.Interaction
        |> Encode.optional Property.Thread Channel.encoder v.Thread
        |> Encode.optional Property.Components (List.map Component.encoder >> Encode.list) v.Components
        |> Encode.optional Property.StickerItems (List.map StickerItem.encoder >> Encode.list) v.StickerItems
        |> Encode.optional Property.Position Encode.int v.Position
        |> Encode.optional Property.RoleSubscriptionData RoleSubscriptionData.encoder v.RoleSubscriptionData
        |> Encode.optional Property.Resolved ResolvedData.encoder v.Resolved
        |> Encode.optional Property.Poll Poll.encoder v.Poll
        |> Encode.optional Property.Call MessageCall.encoder v.Call

    let encoder (v: Message) =
        Encode.object (encodeProperties v)

    module Partial =
        let decoder: Decoder<PartialMessage> =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                ChannelId = get |> Get.optional Property.ChannelId Decode.string
                Author = get |> Get.optional Property.Author User.decoder
                Content = get |> Get.optinull Property.Content Decode.string
                Timestamp = get |> Get.optional Property.Timestamp Decode.datetimeUtc
                EditedTimestamp = get |> Get.optinull Property.EditedTimestamp Decode.datetimeUtc
                Tts = get |> Get.optional Property.Tts Decode.bool
                MentionEveryone = get |> Get.optional Property.MentionEveryone Decode.bool
                Mentions = get |> Get.optional Property.Mentions (Decode.list User.decoder)
                MentionRoles = get |> Get.optional Property.MentionRoles (Decode.list Decode.string)
                MentionChannels = get |> Get.optional Property.MentionChannels (Decode.list ChannelMention.decoder)
                Attachments = get |> Get.optional Property.Attachments (Decode.list Attachment.decoder)
                Embeds = get |> Get.optional Property.Embeds (Decode.list Embed.decoder)
                Reactions = get |> Get.optional Property.Reactions (Decode.list Reaction.decoder)
                Nonce = get |> Get.optional Property.Nonce MessageNonce.decoder
                Pinned = get |> Get.optional Property.Pinned Decode.bool
                WebhookId = get |> Get.optional Property.WebhookId Decode.string
                Type = get |> Get.optional Property.Type Decode.Enum.int<MessageType>
                Activity = get |> Get.optional Property.Activity MessageActivity.decoder
                Application = get |> Get.optional Property.Application Application.Partial.decoder
                ApplicationId = get |> Get.optional Property.ApplicationId Decode.string
                Flags = get |> Get.optional Property.Flags Decode.bitfield
                MessageReference = get |> Get.optional Property.MessageReference MessageReference.decoder
                MessageSnapshots = get |> Get.optional Property.MessageSnapshots (Decode.list MessageSnapshot.decoder)
                ReferencedMessage = get |> Get.optinull Property.ReferencedMessage Message.decoder
                InteractionMetadata = get |> Get.optional Property.InteractionMetadata MessageInteractionMetadata.decoder
                Interaction = get |> Get.optional Property.Interaction MessageInteraction.decoder
                Thread = get |> Get.optional Property.Thread Channel.decoder
                Components = get |> Get.optional Property.Components (Decode.list Component.decoder)
                StickerItems = get |> Get.optional Property.StickerItems (Decode.list StickerItem.decoder)
                Position = get |> Get.optional Property.Position Decode.int
                RoleSubscriptionData = get |> Get.optional Property.RoleSubscriptionData RoleSubscriptionData.decoder
                Resolved = get |> Get.optional Property.Resolved ResolvedData.decoder
                Poll = get |> Get.optional Property.Poll Poll.decoder
                Call = get |> Get.optional Property.Call MessageCall.decoder
            })
        
        let encoder (v: PartialMessage) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.ChannelId Encode.string v.ChannelId
                |> Encode.optional Property.Author User.encoder v.Author
                |> Encode.optinull Property.Content Encode.string v.Content
                |> Encode.optional Property.Timestamp Encode.datetime v.Timestamp
                |> Encode.optinull Property.EditedTimestamp Encode.datetime v.EditedTimestamp
                |> Encode.optional Property.Tts Encode.bool v.Tts
                |> Encode.optional Property.MentionEveryone Encode.bool v.MentionEveryone
                |> Encode.optional Property.Mentions (List.map User.encoder >> Encode.list) v.Mentions
                |> Encode.optional Property.MentionRoles (List.map Encode.string >> Encode.list) v.MentionRoles
                |> Encode.optional Property.MentionChannels (List.map ChannelMention.encoder >> Encode.list) v.MentionChannels
                |> Encode.optional Property.Attachments (List.map Attachment.encoder >> Encode.list) v.Attachments
                |> Encode.optional Property.Embeds (List.map Embed.encoder >> Encode.list) v.Embeds
                |> Encode.optional Property.Reactions (List.map Reaction.encoder >> Encode.list) v.Reactions
                |> Encode.optional Property.Nonce MessageNonce.encoder v.Nonce
                |> Encode.optional Property.Pinned Encode.bool v.Pinned
                |> Encode.optional Property.WebhookId Encode.string v.WebhookId
                |> Encode.optional Property.Type Encode.Enum.int v.Type
                |> Encode.optional Property.Activity MessageActivity.encoder v.Activity
                |> Encode.optional Property.Application Application.Partial.encoder v.Application
                |> Encode.optional Property.ApplicationId Encode.string v.ApplicationId
                |> Encode.optional Property.Flags Encode.bitfield v.Flags
                |> Encode.optional Property.MessageReference MessageReference.encoder v.MessageReference
                |> Encode.optional Property.MessageSnapshots (List.map MessageSnapshot.encoder >> Encode.list) v.MessageSnapshots
                |> Encode.optinull Property.ReferencedMessage Message.encoder v.ReferencedMessage
                |> Encode.optional Property.InteractionMetadata MessageInteractionMetadata.encoder v.InteractionMetadata
                |> Encode.optional Property.Interaction MessageInteraction.encoder v.Interaction
                |> Encode.optional Property.Thread Channel.encoder v.Thread
                |> Encode.optional Property.Components (List.map Component.encoder >> Encode.list) v.Components
                |> Encode.optional Property.StickerItems (List.map StickerItem.encoder >> Encode.list) v.StickerItems
                |> Encode.optional Property.Position Encode.int v.Position
                |> Encode.optional Property.RoleSubscriptionData RoleSubscriptionData.encoder v.RoleSubscriptionData
                |> Encode.optional Property.Resolved ResolvedData.encoder v.Resolved
                |> Encode.optional Property.Poll Poll.encoder v.Poll
                |> Encode.optional Property.Call MessageCall.encoder v.Call
            )

    module Snapshot =
        let decoder: Decoder<SnapshotPartialMessage> =
            Decode.object (fun get -> {
                Content = get |> Get.nullable Property.Content Decode.string
                Timestamp = get |> Get.required Property.Timestamp Decode.datetimeUtc
                EditedTimestamp = get |> Get.optional Property.EditedTimestamp Decode.datetimeUtc
                Mentions = get |> Get.required Property.Mentions (Decode.list User.decoder)
                MentionRoles = get |> Get.required Property.MentionRoles (Decode.list Decode.string)
                Attachments = get |> Get.required Property.Attachments (Decode.list Attachment.decoder)
                Embeds = get |> Get.required Property.Embeds (Decode.list Embed.decoder)
                Type = get |> Get.required Property.Type Decode.Enum.int<MessageType>
                Flags = get |> Get.optional Property.Flags Decode.bitfield
                Components = get |> Get.optional Property.Components (Decode.list Component.decoder)
                StickerItems = get |> Get.optional Property.StickerItems (Decode.list StickerItem.decoder)
            })
        
        let encoder (v: SnapshotPartialMessage) =
            Encode.object ([]
                |> Encode.nullable Property.Content Encode.string v.Content
                |> Encode.required Property.Timestamp Encode.datetime v.Timestamp
                |> Encode.nullable Property.EditedTimestamp Encode.datetime v.EditedTimestamp
                |> Encode.required Property.Mentions (List.map User.encoder >> Encode.list) v.Mentions
                |> Encode.required Property.MentionRoles (List.map Encode.string >> Encode.list) v.MentionRoles
                |> Encode.required Property.Attachments (List.map Attachment.encoder >> Encode.list) v.Attachments
                |> Encode.required Property.Embeds (List.map Embed.encoder >> Encode.list) v.Embeds
                |> Encode.required Property.Type Encode.Enum.int v.Type
                |> Encode.optional Property.Flags Encode.bitfield v.Flags
                |> Encode.optional Property.Components (List.map Component.encoder >> Encode.list) v.Components
                |> Encode.optional Property.StickerItems (List.map StickerItem.encoder >> Encode.list) v.StickerItems
            )

module MessageNonce =
    let decoder: Decoder<MessageNonce> =
        Decode.oneOf [
            Decode.map MessageNonce.INT Decode.int
            Decode.map MessageNonce.STRING Decode.string
        ]
        
    let encoder (v: MessageNonce) =
        match v with
        | MessageNonce.INT v -> Encode.int v
        | MessageNonce.STRING v -> Encode.string v

module MessageActivity =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] PartyId = "party_id"

    let decoder: Decoder<MessageActivity> =
        Decode.object (fun get -> {
            Type = get |> Get.required Property.Type Decode.Enum.int<MessageActivityType>
            PartyId = get |> Get.optional Property.PartyId Decode.string
        })
    
    let encoder (v: MessageActivity) =
        Encode.object ([]
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optional Property.PartyId Encode.string v.PartyId
        )

module ApplicationCommandInteractionMetadata =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] User = "user"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] OriginalResponseMessageId = "original_response_message_id"
        let [<Literal>] TargetUser = "target_user"
        let [<Literal>] TargetMessageId = "target_message_id"

    let decoder: Decoder<ApplicationCommandInteractionMetadata> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            User = get |> Get.required Property.User User.decoder
            AuthorizingIntegrationOwners = get |> Get.required Property.AuthorizingIntegrationOwners (Decode.mapkv (int >> enum<ApplicationIntegrationType> >> Some) Decode.string)
            OriginalResponseMessageId = get |> Get.optional Property.OriginalResponseMessageId Decode.string
            TargetUser = get |> Get.optional Property.TargetUser User.decoder
            TargetMessageId = get |> Get.optional Property.TargetMessageId Decode.string
        })

    let encoder (v: ApplicationCommandInteractionMetadata) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.User User.encoder v.User
            |> Encode.required Property.AuthorizingIntegrationOwners (Encode.mapkv (int >> string) Encode.string) v.AuthorizingIntegrationOwners
            |> Encode.optional Property.OriginalResponseMessageId Encode.string v.OriginalResponseMessageId
            |> Encode.optional Property.TargetUser User.encoder v.TargetUser
            |> Encode.optional Property.TargetMessageId Encode.string v.TargetMessageId
        )

module MessageComponentInteractionMetadata =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] User = "user"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] OriginalResponseMessageId = "original_response_message_id"
        let [<Literal>] InteractedMessageId = "interacted_message_id"

    let decoder: Decoder<MessageComponentInteractionMetadata> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            User = get |> Get.required Property.User User.decoder
            AuthorizingIntegrationOwners = get |> Get.required Property.AuthorizingIntegrationOwners (Decode.mapkv (int >> enum<ApplicationIntegrationType> >> Some) Decode.string)
            OriginalResponseMessageId = get |> Get.optional Property.OriginalResponseMessageId Decode.string
            InteractedMessageId = get |> Get.required Property.InteractedMessageId Decode.string
        })

    let encoder (v: MessageComponentInteractionMetadata) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.User User.encoder v.User
            |> Encode.required Property.AuthorizingIntegrationOwners (Encode.mapkv (int >> string) Encode.string) v.AuthorizingIntegrationOwners
            |> Encode.optional Property.OriginalResponseMessageId Encode.string v.OriginalResponseMessageId
            |> Encode.required Property.InteractedMessageId Encode.string v.InteractedMessageId
        )

module ModalSubmitInteractionMetadata =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] User = "user"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] OriginalResponseMessageId = "original_response_message_id"
        let [<Literal>] TriggeringInteractionMetadata = "triggering_interaction_metadata"

    let decoder: Decoder<ModalSubmitInteractionMetadata> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<InteractionType>
            User = get |> Get.required Property.User User.decoder
            AuthorizingIntegrationOwners = get |> Get.required Property.AuthorizingIntegrationOwners (Decode.mapkv (int >> enum<ApplicationIntegrationType> >> Some) Decode.string)
            OriginalResponseMessageId = get |> Get.optional Property.OriginalResponseMessageId Decode.string
            TriggeringInteractionMetadata = get |> Get.required Property.TriggeringInteractionMetadata MessageInteractionMetadata.decoder
        })

    let encoder (v: ModalSubmitInteractionMetadata) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.User User.encoder v.User
            |> Encode.required Property.AuthorizingIntegrationOwners (Encode.mapkv (int >> string) Encode.string) v.AuthorizingIntegrationOwners
            |> Encode.optional Property.OriginalResponseMessageId Encode.string v.OriginalResponseMessageId
            |> Encode.required Property.TriggeringInteractionMetadata MessageInteractionMetadata.encoder v.TriggeringInteractionMetadata
        )

module MessageInteractionMetadata =
    let decoder: Decoder<MessageInteractionMetadata> =
        Decode.oneOf [
            Decode.map MessageInteractionMetadata.APPLICATION_COMMAND ApplicationCommandInteractionMetadata.decoder
            Decode.map MessageInteractionMetadata.MESSAGE_COMPONENT MessageComponentInteractionMetadata.decoder
            Decode.map MessageInteractionMetadata.MODAL_SUBMIT ModalSubmitInteractionMetadata.decoder
        ]

    let encoder (v: MessageInteractionMetadata) =
        match v with
        | MessageInteractionMetadata.APPLICATION_COMMAND v -> ApplicationCommandInteractionMetadata.encoder v
        | MessageInteractionMetadata.MESSAGE_COMPONENT v -> MessageComponentInteractionMetadata.encoder v
        | MessageInteractionMetadata.MODAL_SUBMIT v -> ModalSubmitInteractionMetadata.encoder v

module MessageCall =
    module Property =
        let [<Literal>] Participants = "participants"
        let [<Literal>] EndedTimestamp = "ended_timestamp"

    let decoder: Decoder<MessageCall> =
        Decode.object (fun get -> {
            Participants = get |> Get.required Property.Participants (Decode.list Decode.string)
            EndedTimestamp = get |> Get.optinull Property.EndedTimestamp Decode.datetimeUtc
        })

    let encoder (v: MessageCall) =
        Encode.object ([]
            |> Encode.required Property.Participants (List.map Encode.string >> Encode.list) v.Participants
            |> Encode.optinull Property.EndedTimestamp Encode.datetime v.EndedTimestamp
        )

module MessageReference =
    module Property =
        let [<Literal>] Type = "type"
        let [<Literal>] MessageId = "message_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] FailIfNotExists = "fail_if_not_exists"

    let decoder: Decoder<MessageReference> =
        Decode.object (fun get -> {
            Type = get |> Get.optional Property.Type Decode.Enum.int<MessageReferenceType> |> Option.defaultValue MessageReferenceType.DEFAULT
            MessageId = get |> Get.optional Property.MessageId Decode.string
            ChannelId = get |> Get.optional Property.ChannelId Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            FailIfNotExists = get |> Get.optional Property.FailIfNotExists Decode.bool
        })

    let encoder (v: MessageReference) =
        Encode.object ([]
            |> Encode.optional Property.Type Encode.Enum.int (Some v.Type)
            |> Encode.optional Property.MessageId Encode.string v.MessageId
            |> Encode.optional Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.FailIfNotExists Encode.bool v.FailIfNotExists
        )

module MessageSnapshot =
    module Property =
        let [<Literal>] Message = "message"

    let decoder: Decoder<MessageSnapshot> =
        Decode.object (fun get -> {
            Message = get |> Get.required Property.Message Message.Snapshot.decoder
        })

    let encoder (v: MessageSnapshot) =
        Encode.object ([]
            |> Encode.required Property.Message Message.Snapshot.encoder v.Message
        )

module Reaction =
    module Property =
        let [<Literal>] Count = "count"
        let [<Literal>] CountDetails = "count_details"
        let [<Literal>] Me = "me"
        let [<Literal>] MeBurst = "me_burst"
        let [<Literal>] Emoji = "emoji"
        let [<Literal>] BurstColors = "burst_colors"

    let decoder: Decoder<Reaction> =
        Decode.object (fun get -> {
            Count = get |> Get.required Property.Count Decode.int
            CountDetails = get |> Get.required Property.CountDetails ReactionCountDetails.decoder
            Me = get |> Get.required Property.Me Decode.bool
            MeBurst = get |> Get.required Property.MeBurst Decode.bool
            Emoji = get |> Get.required Property.Emoji Emoji.Partial.decoder
            BurstColors = get |> Get.required Property.BurstColors (Decode.list Decode.int)
        })

    let encoder (v: Reaction) =
        Encode.object ([]
            |> Encode.required Property.Count Encode.int v.Count
            |> Encode.required Property.CountDetails ReactionCountDetails.encoder v.CountDetails
            |> Encode.required Property.Me Encode.bool v.Me
            |> Encode.required Property.MeBurst Encode.bool v.MeBurst
            |> Encode.required Property.Emoji Emoji.Partial.encoder v.Emoji
            |> Encode.required Property.BurstColors (List.map Encode.int >> Encode.list) v.BurstColors
        )

module ReactionCountDetails =
    module Property =
        let [<Literal>] Burst = "burst"
        let [<Literal>] Normal = "normal"

    let decoder: Decoder<ReactionCountDetails> =
        Decode.object (fun get -> {
            Burst = get |> Get.required Property.Burst Decode.int
            Normal = get |> Get.required Property.Normal Decode.int
        })

    let encoder (v: ReactionCountDetails) =
        Encode.object ([]
            |> Encode.required Property.Burst Encode.int v.Burst
            |> Encode.required Property.Normal Encode.int v.Normal
        )

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

    let decoder: Decoder<Embed> =
        Decode.object (fun get -> {
            Title = get |> Get.optional Property.Title Decode.string
            Type = get |> Get.optional Property.Type EmbedType.decoder
            Description = get |> Get.optional Property.Description Decode.string
            Url = get |> Get.optional Property.Url Decode.string
            Timestamp = get |> Get.optional Property.Timestamp Decode.datetimeUtc
            Color = get |> Get.optional Property.Color Decode.int
            Footer = get |> Get.optional Property.Footer EmbedFooter.decoder
            Image = get |> Get.optional Property.Image EmbedImage.decoder
            Thumbnail = get |> Get.optional Property.Thumbnail EmbedThumbnail.decoder
            Video = get |> Get.optional Property.Video EmbedVideo.decoder
            Provider = get |> Get.optional Property.Provider EmbedProvider.decoder
            Author = get |> Get.optional Property.Author EmbedAuthor.decoder
            Fields = get |> Get.optional Property.Fields (Decode.list EmbedField.decoder)
        })

    let encoder (v: Embed) =
        Encode.object ([]
            |> Encode.optional Property.Title Encode.string v.Title
            |> Encode.optional Property.Type EmbedType.encoder v.Type
            |> Encode.optional Property.Description Encode.string v.Description
            |> Encode.optional Property.Url Encode.string v.Url
            |> Encode.optional Property.Timestamp Encode.datetime v.Timestamp
            |> Encode.optional Property.Color Encode.int v.Color
            |> Encode.optional Property.Footer EmbedFooter.encoder v.Footer
            |> Encode.optional Property.Image EmbedImage.encoder v.Image
            |> Encode.optional Property.Thumbnail EmbedThumbnail.encoder v.Thumbnail
            |> Encode.optional Property.Video EmbedVideo.encoder v.Video
            |> Encode.optional Property.Provider EmbedProvider.encoder v.Provider
            |> Encode.optional Property.Author EmbedAuthor.encoder v.Author
            |> Encode.optional Property.Fields (List.map EmbedField.encoder >> Encode.list) v.Fields
        )

module EmbedThumbnail =
    module Property =
        let [<Literal>] Url = "url"
        let [<Literal>] ProxyUrl = "proxy_url"
        let [<Literal>] Height = "height"
        let [<Literal>] Width = "width"

    let decoder: Decoder<EmbedThumbnail> =
        Decode.object (fun get -> {
            Url = get |> Get.required Property.Url Decode.string
            ProxyUrl = get |> Get.optional Property.ProxyUrl Decode.string
            Height = get |> Get.optional Property.Height Decode.int
            Width = get |> Get.optional Property.Width Decode.int
        })

    let encoder (v: EmbedThumbnail) =
        Encode.object ([]
            |> Encode.required Property.Url Encode.string v.Url
            |> Encode.optional Property.ProxyUrl Encode.string v.ProxyUrl
            |> Encode.optional Property.Height Encode.int v.Height
            |> Encode.optional Property.Width Encode.int v.Width
        )

module EmbedVideo =
    module Property =
        let [<Literal>] Url = "url"
        let [<Literal>] ProxyUrl = "proxy_url"
        let [<Literal>] Height = "height"
        let [<Literal>] Width = "width"

    let decoder: Decoder<EmbedVideo> =
        Decode.object (fun get -> {
            Url = get |> Get.optional Property.Url Decode.string
            ProxyUrl = get |> Get.optional Property.ProxyUrl Decode.string
            Height = get |> Get.optional Property.Height Decode.int
            Width = get |> Get.optional Property.Width Decode.int
        })

    let encoder (v: EmbedVideo) =
        Encode.object ([]
            |> Encode.optional Property.Url Encode.string v.Url
            |> Encode.optional Property.ProxyUrl Encode.string v.ProxyUrl
            |> Encode.optional Property.Height Encode.int v.Height
            |> Encode.optional Property.Width Encode.int v.Width
        )

module EmbedImage =
    module Property =
        let [<Literal>] Url = "url"
        let [<Literal>] ProxyUrl = "proxy_url"
        let [<Literal>] Height = "height"
        let [<Literal>] Width = "width"

    let decoder: Decoder<EmbedImage> =
        Decode.object (fun get -> {
            Url = get |> Get.required Property.Url Decode.string
            ProxyUrl = get |> Get.optional Property.ProxyUrl Decode.string
            Height = get |> Get.optional Property.Height Decode.int
            Width = get |> Get.optional Property.Width Decode.int
        })

    let encoder (v: EmbedImage) =
        Encode.object ([]
            |> Encode.required Property.Url Encode.string v.Url
            |> Encode.optional Property.ProxyUrl Encode.string v.ProxyUrl
            |> Encode.optional Property.Height Encode.int v.Height
            |> Encode.optional Property.Width Encode.int v.Width
        )

module EmbedProvider =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Url = "url"

    let decoder: Decoder<EmbedProvider> =
        Decode.object (fun get -> {
            Name = get |> Get.optional Property.Name Decode.string
            Url = get |> Get.optional Property.Url Decode.string
        })
        
    let encoder (v: EmbedProvider) =
        Encode.object ([]
            |> Encode.optional Property.Name Encode.string v.Name
            |> Encode.optional Property.Url Encode.string v.Url
        )

module EmbedAuthor =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Url = "url"
        let [<Literal>] IconUrl = "icon_url"
        let [<Literal>] ProxyIconUrl = "proxy_icon_url"

    let decoder: Decoder<EmbedAuthor> =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            Url = get |> Get.optional Property.Url Decode.string
            IconUrl = get |> Get.optional Property.IconUrl Decode.string
            ProxyIconUrl = get |> Get.optional Property.ProxyIconUrl Decode.string
        })

    let encoder (v: EmbedAuthor) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optional Property.Url Encode.string v.Url
            |> Encode.optional Property.IconUrl Encode.string v.IconUrl
            |> Encode.optional Property.ProxyIconUrl Encode.string v.ProxyIconUrl
        )

module EmbedFooter =
    module Property =
        let [<Literal>] Text = "text"
        let [<Literal>] IconUrl = "icon_url"
        let [<Literal>] ProxyIconUrl = "proxy_icon_url"

    let decoder: Decoder<EmbedFooter> =
        Decode.object (fun get -> {
            Text = get |> Get.required Property.Text Decode.string
            IconUrl = get |> Get.optional Property.IconUrl Decode.string
            ProxyIconUrl = get |> Get.optional Property.ProxyIconUrl Decode.string
        })

    let encoder (v: EmbedFooter) =
        Encode.object ([]
            |> Encode.required Property.Text Encode.string v.Text
            |> Encode.optional Property.IconUrl Encode.string v.IconUrl
            |> Encode.optional Property.ProxyIconUrl Encode.string v.ProxyIconUrl
        )

module EmbedField =
    module Property =
        let [<Literal>] Name = "name"
        let [<Literal>] Value = "value"
        let [<Literal>] Inline = "inline"

    let decoder: Decoder<EmbedField> =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            Value = get |> Get.required Property.Value Decode.string
            Inline = get |> Get.optional Property.Inline Decode.bool
        })

    let encoder (v: EmbedField) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Value Encode.string v.Value
            |> Encode.optional Property.Inline Encode.bool v.Inline
        )

module Attachment =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Filename = "filename"
        let [<Literal>] Title = "title"
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

    let decoder: Decoder<Attachment> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Filename = get |> Get.required Property.Filename Decode.string
            Title = get |> Get.optional Property.Title Decode.string
            Description = get |> Get.optional Property.Description Decode.string
            ContentType = get |> Get.optional Property.ContentType Decode.string
            Size = get |> Get.required Property.Size Decode.int
            Url = get |> Get.required Property.Url Decode.string
            ProxyUrl = get |> Get.required Property.ProxyUrl Decode.string
            Height = get |> Get.optinull Property.Height Decode.int
            Width = get |> Get.optinull Property.Width Decode.int
            Ephemeral = get |> Get.optional Property.Ephemeral Decode.bool
            DurationSecs = get |> Get.optional Property.DurationSecs Decode.float
            Waveform = get |> Get.optional Property.Waveform Decode.string
            Flags = get |> Get.optional Property.Flags Decode.bitfield
        })

    let encoder (v: Attachment) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Filename Encode.string v.Filename
            |> Encode.optional Property.Title Encode.string v.Title
            |> Encode.optional Property.Description Encode.string v.Description
            |> Encode.optional Property.ContentType Encode.string v.ContentType
            |> Encode.required Property.Size Encode.int v.Size
            |> Encode.required Property.Url Encode.string v.Url
            |> Encode.required Property.ProxyUrl Encode.string v.ProxyUrl
            |> Encode.optinull Property.Height Encode.int v.Height
            |> Encode.optinull Property.Width Encode.int v.Width
            |> Encode.optional Property.Ephemeral Encode.bool v.Ephemeral
            |> Encode.optional Property.DurationSecs Encode.float v.DurationSecs
            |> Encode.optional Property.Waveform Encode.string v.Waveform
            |> Encode.optional Property.Flags Encode.bitfield v.Flags
        )

    module Partial =
        let decoder: Decoder<PartialAttachment> =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Filename = get |> Get.optional Property.Filename Decode.string
                Title = get |> Get.optional Property.Title Decode.string
                Description = get |> Get.optional Property.Description Decode.string
                ContentType = get |> Get.optional Property.ContentType Decode.string
                Size = get |> Get.optional Property.Size Decode.int
                Url = get |> Get.optional Property.Url Decode.string
                ProxyUrl = get |> Get.optional Property.ProxyUrl Decode.string
                Height = get |> Get.optinull Property.Height Decode.int
                Width = get |> Get.optinull Property.Width Decode.int
                Ephemeral = get |> Get.optional Property.Ephemeral Decode.bool
                DurationSecs = get |> Get.optional Property.DurationSecs Decode.float
                Waveform = get |> Get.optional Property.Waveform Decode.string
                Flags = get |> Get.optional Property.Flags Decode.bitfield
            })

        let encoder (v: PartialAttachment) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.Filename Encode.string v.Filename
                |> Encode.optional Property.Title Encode.string v.Title
                |> Encode.optional Property.Description Encode.string v.Description
                |> Encode.optional Property.ContentType Encode.string v.ContentType
                |> Encode.optional Property.Size Encode.int v.Size
                |> Encode.optional Property.Url Encode.string v.Url
                |> Encode.optional Property.ProxyUrl Encode.string v.ProxyUrl
                |> Encode.optinull Property.Height Encode.int v.Height
                |> Encode.optinull Property.Width Encode.int v.Width
                |> Encode.optional Property.Ephemeral Encode.bool v.Ephemeral
                |> Encode.optional Property.DurationSecs Encode.float v.DurationSecs
                |> Encode.optional Property.Waveform Encode.string v.Waveform
                |> Encode.optional Property.Flags Encode.bitfield v.Flags
            )

module ChannelMention =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Name = "name"

    let decoder: Decoder<ChannelMention> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<ChannelType>
            Name = get |> Get.required Property.Name Decode.string
        })

    let encoder (v: ChannelMention) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.Name Encode.string v.Name
        )

module AllowedMentions =
    module Property =
        let [<Literal>] Parse = "parse"
        let [<Literal>] Roles = "roles"
        let [<Literal>] Users = "users"
        let [<Literal>] RepliedUser = "replied_user"

    let decoder: Decoder<AllowedMentions> =
        Decode.object (fun get -> {
            Parse = get |> Get.optional Property.Parse (Decode.list AllowedMentionsParseType.decoder)
            Roles = get |> Get.optional Property.Roles (Decode.list Decode.string)
            Users = get |> Get.optional Property.Users (Decode.list Decode.string)
            RepliedUser = get |> Get.optional Property.RepliedUser Decode.bool
        })

    let encoder (v: AllowedMentions) =
        Encode.object ([]
            |> Encode.optional Property.Parse (List.map AllowedMentionsParseType.encoder >> Encode.list) v.Parse
            |> Encode.optional Property.Roles (List.map Encode.string >> Encode.list) v.Roles
            |> Encode.optional Property.Users (List.map Encode.string >> Encode.list) v.Users
            |> Encode.optional Property.RepliedUser Encode.bool v.RepliedUser
        )

module RoleSubscriptionData =
    module Property =
        let [<Literal>] RoleSubscriptionListingId = "role_subscription_listing_id"
        let [<Literal>] TierName = "tier_name"
        let [<Literal>] TotalMonthsSubscribed = "total_months_subscribed"
        let [<Literal>] IsRenewal = "is_renewal"

    let decoder: Decoder<RoleSubscriptionData> =
        Decode.object (fun get -> {
            RoleSubscriptionListingId = get |> Get.required Property.RoleSubscriptionListingId Decode.string
            TierName = get |> Get.required Property.TierName Decode.string
            TotalMonthsSubscribed = get |> Get.required Property.TotalMonthsSubscribed Decode.int
            IsRenewal = get |> Get.required Property.IsRenewal Decode.bool
        })

    let encoder (v: RoleSubscriptionData) =
        Encode.object ([]
            |> Encode.required Property.RoleSubscriptionListingId Encode.string v.RoleSubscriptionListingId
            |> Encode.required Property.TierName Encode.string v.TierName
            |> Encode.required Property.TotalMonthsSubscribed Encode.int v.TotalMonthsSubscribed
            |> Encode.required Property.IsRenewal Encode.bool v.IsRenewal
        )

module Poll =
    module Property =
        let [<Literal>] Question = "question"
        let [<Literal>] Answers = "answers"
        let [<Literal>] Expiry = "expiry"
        let [<Literal>] AllowMultiselect = "allow_multiselect"
        let [<Literal>] LayoutType = "layout_type"
        let [<Literal>] Results = "results"

    let decoder: Decoder<Poll> =
        Decode.object (fun get -> {
            Question = get |> Get.required Property.Question PollMedia.decoder
            Answers = get |> Get.required Property.Answers (Decode.list PollAnswer.decoder)
            Expiry = get |> Get.nullable Property.Expiry Decode.datetimeUtc
            AllowMultiselect = get |> Get.required Property.AllowMultiselect Decode.bool
            LayoutType = get |> Get.required Property.LayoutType Decode.Enum.int<PollLayout>
            Results = get |> Get.optional Property.Results PollResults.decoder
        })

    let encoder (v: Poll) =
        Encode.object ([]
            |> Encode.required Property.Question PollMedia.encoder v.Question
            |> Encode.required Property.Answers (List.map PollAnswer.encoder >> Encode.list) v.Answers
            |> Encode.optional Property.Expiry Encode.datetime v.Expiry
            |> Encode.required Property.AllowMultiselect Encode.bool v.AllowMultiselect
            |> Encode.required Property.LayoutType Encode.Enum.int v.LayoutType
            |> Encode.optional Property.Results PollResults.encoder v.Results
        )

module PollCreateRequest =
    module Property =
        let [<Literal>] Question = "question"
        let [<Literal>] Answers = "answers"
        let [<Literal>] Duration = "duration"
        let [<Literal>] AllowMultiselect = "allow_multiselect"
        let [<Literal>] LayoutType = "layout_type"

    let decoder: Decoder<PollCreateRequest> =
        Decode.object (fun get -> {
            Question = get |> Get.required Property.Question PollMedia.decoder
            Answers = get |> Get.required Property.Answers (Decode.list PollAnswer.decoder)
            Duration = get |> Get.optional Property.Duration Decode.int |> Option.defaultValue 24
            AllowMultiselect = get |> Get.optional Property.AllowMultiselect Decode.bool |> Option.defaultValue false
            LayoutType = get |> Get.optional Property.LayoutType Decode.Enum.int<PollLayout> |> Option.defaultValue PollLayout.DEFAULT
        })

    let encoder (v: PollCreateRequest) =
        Encode.object ([]
            |> Encode.required Property.Question PollMedia.encoder v.Question
            |> Encode.required Property.Answers (List.map PollAnswer.encoder >> Encode.list) v.Answers
            |> Encode.optional Property.Duration Encode.int (Some v.Duration)
            |> Encode.optional Property.AllowMultiselect Encode.bool (Some v.AllowMultiselect)
            |> Encode.optional Property.LayoutType Encode.Enum.int (Some v.LayoutType)
        )

module PollMedia =
    module Property =
        let [<Literal>] Text = "text"
        let [<Literal>] Emoji = "emoji"

    let decoder: Decoder<PollMedia> =
        Decode.object (fun get -> {
            Text = get |> Get.optional Property.Text Decode.string
            Emoji = get |> Get.optional Property.Emoji Emoji.Partial.decoder
        })

    let encoder (v: PollMedia) =
        Encode.object ([]
            |> Encode.optional Property.Text Encode.string v.Text
            |> Encode.optional Property.Emoji Emoji.Partial.encoder v.Emoji
        )

module PollAnswer =
    module Property =
        let [<Literal>] AnswerId = "answer_id"
        let [<Literal>] PollMedia = "poll_media"

    let decoder: Decoder<PollAnswer> =
        Decode.object (fun get -> {
            AnswerId = get |> Get.required Property.AnswerId Decode.int
            PollMedia = get |> Get.required Property.PollMedia PollMedia.decoder
        })

    let encoder (v: PollAnswer) =
        Encode.object ([]
            |> Encode.required Property.AnswerId Encode.int v.AnswerId
            |> Encode.required Property.PollMedia PollMedia.encoder v.PollMedia
        )

module PollResults =
    module Property =
        let [<Literal>] IsFinalized = "is_finalized"
        let [<Literal>] AnswerCounts = "answer_counts"

    let decoder: Decoder<PollResults> =
        Decode.object (fun get -> {
            IsFinalized = get |> Get.required Property.IsFinalized Decode.bool
            AnswerCounts = get |> Get.required Property.AnswerCounts (Decode.list PollAnswerCount.decoder)
        })

    let encoder (v: PollResults) =
        Encode.object ([]
            |> Encode.required Property.IsFinalized Encode.bool v.IsFinalized
            |> Encode.required Property.AnswerCounts (List.map PollAnswerCount.encoder >> Encode.list) v.AnswerCounts
        )

module PollAnswerCount =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Count = "count"
        let [<Literal>] MeVoted = "me_voted"

    let decoder: Decoder<PollAnswerCount> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.int
            Count = get |> Get.required Property.Count Decode.int
            MeVoted = get |> Get.required Property.MeVoted Decode.bool
        })

    let encoder (v: PollAnswerCount) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.int v.Id
            |> Encode.required Property.Count Encode.int v.Count
            |> Encode.required Property.MeVoted Encode.bool v.MeVoted
        )

module Sku =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Name = "name"
        let [<Literal>] Slug = "slug"
        let [<Literal>] Flags = "flags"

    let decoder: Decoder<Sku> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<SkuType>
            ApplicationId = get |> Get.required Property.ApplicationId Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Slug = get |> Get.required Property.Slug Decode.string
            Flags = get |> Get.required Property.Flags Decode.bitfield
        })

    let encoder (v: Sku) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Slug Encode.string v.Slug
            |> Encode.required Property.Flags Encode.bitfield v.Flags
        )

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

    let decoder: Decoder<SoundboardSound> =
        Decode.object (fun get -> {
            Name = get |> Get.required Property.Name Decode.string
            SoundId = get |> Get.required Property.SoundId Decode.string
            Volume = get |> Get.required Property.Volume Decode.float
            EmojiId = get |> Get.nullable Property.EmojiId Decode.string
            EmojiName = get |> Get.nullable Property.EmojiName Decode.string
            GuildId = get |> Get.optional Property.GuildId Decode.string
            Available = get |> Get.required Property.Available Decode.bool
            User = get |> Get.optional Property.User User.decoder
        })

    let encoder (v: SoundboardSound) =
        Encode.object ([]
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.SoundId Encode.string v.SoundId
            |> Encode.required Property.Volume Encode.float v.Volume
            |> Encode.optional Property.EmojiId Encode.string v.EmojiId
            |> Encode.optional Property.EmojiName Encode.string v.EmojiName
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.Available Encode.bool v.Available
            |> Encode.optional Property.User User.encoder v.User
        )

module StageInstance =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Topic = "topic"
        let [<Literal>] PrivacyLevel = "privacy_level"
        let [<Literal>] DiscoverableEnabled = "discoverable_enabled"
        let [<Literal>] GuildScheduledEventId = "guild_scheduled_event_id"

    let decoder: Decoder<StageInstance> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            GuildId = get |> Get.required Property.GuildId Decode.string
            ChannelId = get |> Get.required Property.ChannelId Decode.string
            Topic = get |> Get.required Property.Topic Decode.string
            PrivacyLevel = get |> Get.required Property.PrivacyLevel Decode.Enum.int<PrivacyLevel>
            DiscoverableEnabled = get |> Get.required Property.DiscoverableEnabled Decode.bool
            GuildScheduledEventId = get |> Get.optional Property.GuildScheduledEventId Decode.string
        })

    let encoder (v: StageInstance) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.GuildId Encode.string v.GuildId
            |> Encode.required Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.Topic Encode.string v.Topic
            |> Encode.required Property.PrivacyLevel Encode.Enum.int v.PrivacyLevel
            |> Encode.required Property.DiscoverableEnabled Encode.bool v.DiscoverableEnabled
            |> Encode.optional Property.GuildScheduledEventId Encode.string v.GuildScheduledEventId
        )

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

    let decoder: Decoder<Sticker> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            PackId = get |> Get.optional Property.PackId Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Description = get |> Get.nullable Property.Description Decode.string
            Tags = get |> Get.required Property.Tags Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<StickerType>
            FormatType = get |> Get.required Property.FormatType Decode.Enum.int<StickerFormat>
            Available = get |> Get.optional Property.Available Decode.bool
            GuildId = get |> Get.optional Property.GuildId Decode.string
            User = get |> Get.optional Property.User User.decoder
            SortValue = get |> Get.optional Property.SortValue Decode.int
        })

    let encoder (v: Sticker) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.optional Property.PackId Encode.string v.PackId
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.optional Property.Description Encode.string v.Description
            |> Encode.required Property.Tags Encode.string v.Tags
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.required Property.FormatType Encode.Enum.int v.FormatType
            |> Encode.optional Property.Available Encode.bool v.Available
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.optional Property.User User.encoder v.User
            |> Encode.optional Property.SortValue Encode.int v.SortValue
        )

module StickerItem =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] FormatType = "format_type"

    let decoder: Decoder<StickerItem> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            FormatType = get |> Get.required Property.FormatType Decode.Enum.int<StickerFormat>
        })

    let encoder (v: StickerItem) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.FormatType Encode.Enum.int v.FormatType
        )

module StickerPack =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Stickers = "stickers"
        let [<Literal>] Name = "name"
        let [<Literal>] SkuId = "sku_id"
        let [<Literal>] CoverStickerId = "cover_sticker_id"
        let [<Literal>] Description = "description"
        let [<Literal>] BannerAssetId = "banner_asset_id"

    let decoder: Decoder<StickerPack> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Stickers = get |> Get.required Property.Stickers (Decode.list Sticker.decoder)
            Name = get |> Get.required Property.Name Decode.string
            SkuId = get |> Get.required Property.SkuId Decode.string
            CoverStickerId = get |> Get.optional Property.CoverStickerId Decode.string
            Description = get |> Get.required Property.Description Decode.string
            BannerAssetId = get |> Get.optional Property.BannerAssetId Decode.string
        })

    let encoder (v: StickerPack) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Stickers (List.map Sticker.encoder >> Encode.list) v.Stickers
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.SkuId Encode.string v.SkuId
            |> Encode.optional Property.CoverStickerId Encode.string v.CoverStickerId
            |> Encode.required Property.Description Encode.string v.Description
            |> Encode.optional Property.BannerAssetId Encode.string v.BannerAssetId
        )

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
        let [<Literal>] CanceledAt = "canceled_at"
        let [<Literal>] Country = "country"

    let decoder: Decoder<Subscription> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            UserId = get |> Get.required Property.UserId Decode.string
            SkuIds = get |> Get.required Property.SkuIds (Decode.list Decode.string)
            EntitlementIds = get |> Get.required Property.EntitlementIds (Decode.list Decode.string)
            RenewalSkuIds = get |> Get.nullable Property.RenewalSkuIds (Decode.list Decode.string)
            CurrentPeriodStart = get |> Get.required Property.CurrentPeriodStart Decode.datetimeUtc
            CurrentPeriodEnd = get |> Get.required Property.CurrentPeriodEnd Decode.datetimeUtc
            Status = get |> Get.required Property.Status Decode.Enum.int<SubscriptionStatus>
            CanceledAt = get |> Get.nullable Property.CanceledAt Decode.datetimeUtc
            Country = get |> Get.optional Property.Country Decode.string
        })

    let encoder (v: Subscription) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.required Property.SkuIds (List.map Encode.string >> Encode.list) v.SkuIds
            |> Encode.required Property.EntitlementIds (List.map Encode.string >> Encode.list) v.EntitlementIds
            |> Encode.nullable Property.RenewalSkuIds (List.map Encode.string >> Encode.list) v.RenewalSkuIds
            |> Encode.required Property.CurrentPeriodStart Encode.datetime v.CurrentPeriodStart
            |> Encode.required Property.CurrentPeriodEnd Encode.datetime v.CurrentPeriodEnd
            |> Encode.required Property.Status Encode.Enum.int v.Status
            |> Encode.nullable Property.CanceledAt Encode.datetime v.CanceledAt
            |> Encode.optional Property.Country Encode.string v.Country
        )

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

    let decoder: Decoder<User> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Username = get |> Get.required Property.Username Decode.string
            Discriminator = get |> Get.required Property.Discriminator Decode.string
            GlobalName = get |> Get.nullable Property.GlobalName Decode.string
            Avatar = get |> Get.nullable Property.Avatar Decode.string
            Bot = get |> Get.optional Property.Bot Decode.bool
            System = get |> Get.optional Property.System Decode.bool
            MfaEnabled = get |> Get.optional Property.MfaEnabled Decode.bool
            Banner = get |> Get.optinull Property.Banner Decode.string
            AccentColor = get |> Get.optinull Property.AccentColor Decode.int
            Locale = get |> Get.optional Property.Locale Decode.string
            Verified = get |> Get.optional Property.Verified Decode.bool
            Email = get |> Get.optinull Property.Email Decode.string
            Flags = get |> Get.optional Property.Flags Decode.bitfield
            PremiumType = get |> Get.optional Property.PremiumType Decode.Enum.int<UserPremiumTier>
            PublicFlags = get |> Get.optional Property.PublicFlags Decode.bitfield
            AvatarDecorationData = get |> Get.optinull Property.AvatarDecorationData AvatarDecorationData.decoder
        })

    let internal encodeProperties (v: User) =
        []
        |> Encode.required Property.Id Encode.string v.Id
        |> Encode.required Property.Username Encode.string v.Username
        |> Encode.required Property.Discriminator Encode.string v.Discriminator
        |> Encode.nullable Property.GlobalName Encode.string v.GlobalName
        |> Encode.nullable Property.Avatar Encode.string v.Avatar
        |> Encode.optional Property.Bot Encode.bool v.Bot
        |> Encode.optional Property.System Encode.bool v.System
        |> Encode.optional Property.MfaEnabled Encode.bool v.MfaEnabled
        |> Encode.optinull Property.Banner Encode.string v.Banner
        |> Encode.optinull Property.AccentColor Encode.int v.AccentColor
        |> Encode.optional Property.Locale Encode.string v.Locale
        |> Encode.optional Property.Verified Encode.bool v.Verified
        |> Encode.optinull Property.Email Encode.string v.Email
        |> Encode.optional Property.Flags Encode.bitfield v.Flags
        |> Encode.optional Property.PremiumType Encode.Enum.int v.PremiumType
        |> Encode.optional Property.PublicFlags Encode.bitfield v.PublicFlags
        |> Encode.optinull Property.AvatarDecorationData AvatarDecorationData.encoder v.AvatarDecorationData

    let encoder (v: User) =
        Encode.object (encodeProperties v)

    module Partial =
        let decoder: Decoder<PartialUser> =
            Decode.object (fun get -> {
                Id = get |> Get.required Property.Id Decode.string
                Username = get |> Get.optional Property.Username Decode.string
                Discriminator = get |> Get.optional Property.Discriminator Decode.string
                GlobalName = get |> Get.optinull Property.GlobalName Decode.string
                Avatar = get |> Get.optinull Property.Avatar Decode.string
                Bot = get |> Get.optional Property.Bot Decode.bool
                System = get |> Get.optional Property.System Decode.bool
                MfaEnabled = get |> Get.optional Property.MfaEnabled Decode.bool
                Banner = get |> Get.optinull Property.Banner Decode.string
                AccentColor = get |> Get.optinull Property.AccentColor Decode.int
                Locale = get |> Get.optional Property.Locale Decode.string
                Verified = get |> Get.optional Property.Verified Decode.bool
                Email = get |> Get.optinull Property.Email Decode.string
                Flags = get |> Get.optional Property.Flags Decode.bitfield
                PremiumType = get |> Get.optional Property.PremiumType Decode.Enum.int<UserPremiumTier>
                PublicFlags = get |> Get.optional Property.PublicFlags Decode.bitfield
                AvatarDecorationData = get |> Get.optinull Property.AvatarDecorationData AvatarDecorationData.decoder
            })

        let encoder (v: PartialUser) =
            Encode.object ([]
                |> Encode.required Property.Id Encode.string v.Id
                |> Encode.optional Property.Username Encode.string v.Username
                |> Encode.optional Property.Discriminator Encode.string v.Discriminator
                |> Encode.optinull Property.GlobalName Encode.string v.GlobalName
                |> Encode.optinull Property.Avatar Encode.string v.Avatar
                |> Encode.optional Property.Bot Encode.bool v.Bot
                |> Encode.optional Property.System Encode.bool v.System
                |> Encode.optional Property.MfaEnabled Encode.bool v.MfaEnabled
                |> Encode.optinull Property.Banner Encode.string v.Banner
                |> Encode.optinull Property.AccentColor Encode.int v.AccentColor
                |> Encode.optional Property.Locale Encode.string v.Locale
                |> Encode.optional Property.Verified Encode.bool v.Verified
                |> Encode.optinull Property.Email Encode.string v.Email
                |> Encode.optional Property.Flags Encode.bitfield v.Flags
                |> Encode.optional Property.PremiumType Encode.Enum.int v.PremiumType
                |> Encode.optional Property.PublicFlags Encode.bitfield v.PublicFlags
                |> Encode.optinull Property.AvatarDecorationData AvatarDecorationData.encoder v.AvatarDecorationData
            )

module AvatarDecorationData =
    module Property =
        let [<Literal>] Asset = "asset"
        let [<Literal>] SkuId = "sku_id"

    let decoder: Decoder<AvatarDecorationData> =
        Decode.object (fun get -> {
            Asset = get |> Get.required Property.Asset Decode.string
            SkuId = get |> Get.required Property.SkuId Decode.string
        })

    let encoder (v: AvatarDecorationData) =
        Encode.object ([]
            |> Encode.required Property.Asset Encode.string v.Asset
            |> Encode.required Property.SkuId Encode.string v.SkuId
        )

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

    let decoder: Decoder<Connection> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Type = get |> Get.required Property.Type ConnectionServiceType.decoder
            Revoked = get |> Get.optional Property.Revoked Decode.bool
            Integrations = get |> Get.optional Property.Integrations (Decode.list Integration.Partial.decoder)
            Verified = get |> Get.required Property.Verified Decode.bool
            FriendSync = get |> Get.required Property.FriendSync Decode.bool
            ShowActivity = get |> Get.required Property.ShowActivity Decode.bool
            TwoWayLink = get |> Get.required Property.TwoWayLink Decode.bool
            Visibility = get |> Get.required Property.Visibility Decode.Enum.int<ConnectionVisibility>
        })

    let encoder (v: Connection) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Type ConnectionServiceType.encoder v.Type
            |> Encode.optional Property.Revoked Encode.bool v.Revoked
            |> Encode.optional Property.Integrations (List.map Integration.Partial.encoder >> Encode.list) v.Integrations
            |> Encode.required Property.Verified Encode.bool v.Verified
            |> Encode.required Property.FriendSync Encode.bool v.FriendSync
            |> Encode.required Property.ShowActivity Encode.bool v.ShowActivity
            |> Encode.required Property.TwoWayLink Encode.bool v.TwoWayLink
            |> Encode.required Property.Visibility Encode.Enum.int v.Visibility
        )

module ApplicationRoleConnection =
    module Property =
        let [<Literal>] PlatformName = "platform_name"
        let [<Literal>] PlatformUsername = "platform_username"
        let [<Literal>] Metadata = "metadata"

    let decoder: Decoder<ApplicationRoleConnection> =
        Decode.object (fun get -> {
            PlatformName = get |> Get.nullable Property.PlatformName Decode.string
            PlatformUsername = get |> Get.nullable Property.PlatformUsername Decode.string
            Metadata = get |> Get.required Property.Metadata (Decode.dict Decode.string)
        })

    let encoder (v: ApplicationRoleConnection) =
        Encode.object ([]
            |> Encode.nullable Property.PlatformName Encode.string v.PlatformName
            |> Encode.nullable Property.PlatformUsername Encode.string v.PlatformUsername
            |> Encode.required Property.Metadata (Encode.mapv Encode.string) v.Metadata
        )

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

    let decoder: Decoder<VoiceState> =
        Decode.object (fun get -> {
            GuildId = get |> Get.optional Property.GuildId Decode.string
            ChannelId = get |> Get.nullable Property.ChannelId Decode.string
            UserId = get |> Get.required Property.UserId Decode.string
            Member = get |> Get.optional Property.Member GuildMember.decoder
            SessionId = get |> Get.required Property.SessionId Decode.string
            Deaf = get |> Get.required Property.Deaf Decode.bool
            Mute = get |> Get.required Property.Mute Decode.bool
            SelfDeaf = get |> Get.required Property.SelfDeaf Decode.bool
            SelfMute = get |> Get.required Property.SelfMute Decode.bool
            SelfStream = get |> Get.optional Property.SelfStream Decode.bool
            SelfVideo = get |> Get.required Property.SelfVideo Decode.bool
            Suppress = get |> Get.required Property.Suppress Decode.bool
            RequestToSpeakTimestamp = get |> Get.nullable Property.RequestToSpeakTimestamp Decode.datetimeUtc
        })

    let encoder (v: VoiceState) =
        Encode.object ([]
            |> Encode.optional Property.GuildId Encode.string v.GuildId
            |> Encode.nullable Property.ChannelId Encode.string v.ChannelId
            |> Encode.required Property.UserId Encode.string v.UserId
            |> Encode.optional Property.Member GuildMember.encoder v.Member
            |> Encode.required Property.SessionId Encode.string v.SessionId
            |> Encode.required Property.Deaf Encode.bool v.Deaf
            |> Encode.required Property.Mute Encode.bool v.Mute
            |> Encode.required Property.SelfDeaf Encode.bool v.SelfDeaf
            |> Encode.required Property.SelfMute Encode.bool v.SelfMute
            |> Encode.optional Property.SelfStream Encode.bool v.SelfStream
            |> Encode.required Property.SelfVideo Encode.bool v.SelfVideo
            |> Encode.required Property.Suppress Encode.bool v.Suppress
            |> Encode.nullable Property.RequestToSpeakTimestamp Encode.datetime v.RequestToSpeakTimestamp
        )

    module Partial =
        let decoder: Decoder<PartialVoiceState> =
            Decode.object (fun get -> {
                GuildId = get |> Get.optional Property.GuildId Decode.string
                ChannelId = get |> Get.optinull Property.ChannelId Decode.string
                UserId = get |> Get.optional Property.UserId Decode.string
                Member = get |> Get.optional Property.Member GuildMember.decoder
                SessionId = get |> Get.optional Property.SessionId Decode.string
                Deaf = get |> Get.optional Property.Deaf Decode.bool
                Mute = get |> Get.optional Property.Mute Decode.bool
                SelfDeaf = get |> Get.optional Property.SelfDeaf Decode.bool
                SelfMute = get |> Get.optional Property.SelfMute Decode.bool
                SelfStream = get |> Get.optional Property.SelfStream Decode.bool
                SelfVideo = get |> Get.optional Property.SelfVideo Decode.bool
                Suppress = get |> Get.optional Property.Suppress Decode.bool
                RequestToSpeakTimestamp = get |> Get.optinull Property.RequestToSpeakTimestamp Decode.datetimeUtc
            })

        let encoder (v: PartialVoiceState) =
            Encode.object ([]
                |> Encode.optional Property.GuildId Encode.string v.GuildId
                |> Encode.optinull Property.ChannelId Encode.string v.ChannelId
                |> Encode.optional Property.UserId Encode.string v.UserId
                |> Encode.optional Property.Member GuildMember.encoder v.Member
                |> Encode.optional Property.SessionId Encode.string v.SessionId
                |> Encode.optional Property.Deaf Encode.bool v.Deaf
                |> Encode.optional Property.Mute Encode.bool v.Mute
                |> Encode.optional Property.SelfDeaf Encode.bool v.SelfDeaf
                |> Encode.optional Property.SelfMute Encode.bool v.SelfMute
                |> Encode.optional Property.SelfStream Encode.bool v.SelfStream
                |> Encode.optional Property.SelfVideo Encode.bool v.SelfVideo
                |> Encode.optional Property.Suppress Encode.bool v.Suppress
                |> Encode.optinull Property.RequestToSpeakTimestamp Encode.datetime v.RequestToSpeakTimestamp
            )

module VoiceRegion =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Optimal = "optimal"
        let [<Literal>] Deprecated = "deprecated"
        let [<Literal>] Custom = "custom"

    let decoder: Decoder<VoiceRegion> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Optimal = get |> Get.required Property.Optimal Decode.bool
            Deprecated = get |> Get.required Property.Deprecated Decode.bool
            Custom = get |> Get.required Property.Custom Decode.bool
        })

    let encoder (v: VoiceRegion) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Optimal Encode.bool v.Optimal
            |> Encode.required Property.Deprecated Encode.bool v.Deprecated
            |> Encode.required Property.Custom Encode.bool v.Custom
        )

module Webhook =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Type = "type"
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

    let decoder: Decoder<Webhook> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Type = get |> Get.required Property.Type Decode.Enum.int<WebhookType>
            GuildId = get |> Get.optinull Property.GuildId Decode.string
            ChannelId = get |> Get.nullable Property.ChannelId Decode.string
            User = get |> Get.optional Property.User User.decoder
            Name = get |> Get.nullable Property.Name Decode.string
            Avatar = get |> Get.nullable Property.Avatar Decode.string
            Token = get |> Get.optional Property.Token Decode.string
            ApplicationId = get |> Get.nullable Property.ApplicationId Decode.string
            SourceGuild = get |> Get.optional Property.SourceGuild Guild.Partial.decoder
            SourceChannel = get |> Get.optional Property.SourceChannel Channel.Partial.decoder
            Url = get |> Get.optional Property.Url Decode.string
        })

    let encoder (v: Webhook) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Type Encode.Enum.int v.Type
            |> Encode.optinull Property.GuildId Encode.string v.GuildId
            |> Encode.nullable Property.ChannelId Encode.string v.ChannelId
            |> Encode.optional Property.User User.encoder v.User
            |> Encode.nullable Property.Name Encode.string v.Name
            |> Encode.nullable Property.Avatar Encode.string v.Avatar
            |> Encode.optional Property.Token Encode.string v.Token
            |> Encode.nullable Property.ApplicationId Encode.string v.ApplicationId
            |> Encode.optional Property.SourceGuild Guild.Partial.encoder v.SourceGuild
            |> Encode.optional Property.SourceChannel Channel.Partial.encoder v.SourceChannel
            |> Encode.optional Property.Url Encode.string v.Url
        )

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

    let decoder: Decoder<Role> =
        Decode.object (fun get -> {
            Id = get |> Get.required Property.Id Decode.string
            Name = get |> Get.required Property.Name Decode.string
            Color = get |> Get.required Property.Color Decode.int
            Hoist = get |> Get.required Property.Hoist Decode.bool
            Icon = get |> Get.optinull Property.Icon Decode.string
            UnicodeEmoji = get |> Get.optinull Property.UnicodeEmoji Decode.string
            Position = get |> Get.required Property.Position Decode.int
            Permissions = get |> Get.required Property.Permissions Decode.bitfieldL<Permission>
            Managed = get |> Get.required Property.Managed Decode.bool
            Mentionable = get |> Get.required Property.Mentionable Decode.bool
            Tags = get |> Get.optional Property.Tags RoleTags.decoder
            Flags = get |> Get.required Property.Flags Decode.bitfield
        })

    let encoder (v: Role) =
        Encode.object ([]
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.Color Encode.int v.Color
            |> Encode.required Property.Hoist Encode.bool v.Hoist
            |> Encode.optinull Property.Icon Encode.string v.Icon
            |> Encode.optinull Property.UnicodeEmoji Encode.string v.UnicodeEmoji
            |> Encode.required Property.Position Encode.int v.Position
            |> Encode.required Property.Permissions Encode.bitfieldL<Permission> v.Permissions
            |> Encode.required Property.Managed Encode.bool v.Managed
            |> Encode.required Property.Mentionable Encode.bool v.Mentionable
            |> Encode.optional Property.Tags RoleTags.encoder v.Tags
            |> Encode.required Property.Flags Encode.bitfield v.Flags
        )

module RoleTags =
    module Property =
        let [<Literal>] BotId = "bot_id"
        let [<Literal>] IntegrationId = "integration_id"
        let [<Literal>] PremiumSubscriber = "premium_subscriber"
        let [<Literal>] SubscriptionListingId = "subscription_listing_id"
        let [<Literal>] AvailableForPurchase = "available_for_purchase"
        let [<Literal>] GuildConnections = "guild_connections"

    let decoder: Decoder<RoleTags> =
        Decode.object (fun get -> {
            BotId = get |> Get.optional Property.BotId Decode.string
            IntegrationId = get |> Get.optional Property.IntegrationId Decode.string
            PremiumSubscriber = get |> Get.exists Property.PremiumSubscriber
            SubscriptionListingId = get |> Get.optional Property.SubscriptionListingId Decode.string
            AvailableForPurchase = get |> Get.exists Property.AvailableForPurchase
            GuildConnections = get |> Get.exists Property.GuildConnections
        })

    let encoder (v: RoleTags) =
        Encode.object ([]
            |> Encode.optional Property.BotId Encode.string v.BotId
            |> Encode.optional Property.IntegrationId Encode.string v.IntegrationId
            |> Encode.exists Property.PremiumSubscriber Encode.nil v.PremiumSubscriber
            |> Encode.optional Property.SubscriptionListingId Encode.string v.SubscriptionListingId
            |> Encode.exists Property.AvailableForPurchase Encode.nil v.AvailableForPurchase
            |> Encode.exists Property.GuildConnections Encode.nil v.GuildConnections
        )

module Team =
    module Property =
        let [<Literal>] Icon = "icon"
        let [<Literal>] Id = "id"
        let [<Literal>] Members = "members"
        let [<Literal>] Name = "name"
        let [<Literal>] OwnerUserId = "owner_user_id"

    let decoder: Decoder<Team> =
        Decode.object (fun get -> {
            Icon = get |> Get.nullable Property.Icon Decode.string
            Id = get |> Get.required Property.Id Decode.string
            Members = get |> Get.required Property.Members (Decode.list TeamMember.decoder)
            Name = get |> Get.required Property.Name Decode.string
            OwnerUserId = get |> Get.required Property.OwnerUserId Decode.string
        })

    let encoder (v: Team) =
        Encode.object ([]
            |> Encode.nullable Property.Icon Encode.string v.Icon
            |> Encode.required Property.Id Encode.string v.Id
            |> Encode.required Property.Members (List.map TeamMember.encoder >> Encode.list) v.Members
            |> Encode.required Property.Name Encode.string v.Name
            |> Encode.required Property.OwnerUserId Encode.string v.OwnerUserId
        )

module TeamMember =
    module Property =
        let [<Literal>] MembershipState = "membership_state"
        let [<Literal>] TeamId = "team_id"
        let [<Literal>] User = "user"
        let [<Literal>] Role = "role"

    let decoder: Decoder<TeamMember> =
        Decode.object (fun get -> {
            MembershipState = get |> Get.required Property.MembershipState Decode.Enum.int<MembershipState>
            TeamId = get |> Get.required Property.TeamId Decode.string
            User = get |> Get.required Property.User User.Partial.decoder
            Role = get |> Get.required Property.Role TeamMemberRoleType.decoder
        })

    let encoder (v: TeamMember) =
        Encode.object ([]
            |> Encode.required Property.MembershipState Encode.Enum.int v.MembershipState
            |> Encode.required Property.TeamId Encode.string v.TeamId
            |> Encode.required Property.User User.Partial.encoder v.User
            |> Encode.required Property.Role TeamMemberRoleType.encoder v.Role
        )

// TODO: Count how many instances of encode/decode for optional and nullable and look for mismatches
// TODO: Make `Encode.list'` helper function to remove `List.map` boilerplate
// TODO: Write updated tests for encoding and decoding helper functions
// TODO: Rewrite `Decode.oneOf` usages to property check type (probably fair bit more efficient)
