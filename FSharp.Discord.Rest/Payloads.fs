namespace FSharp.Discord.Rest

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

// ----- Interactions: Receiving and Responding -----

type CreateInteractionResponsePayload(response, ?files) =
    member val Response: InteractionResponse = response
    member val Files: Media list = defaultArg files []

    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles InteractionResponse.encoder this.Response this.Files
            
type EditOriginalInteractionResponsePayload(
    ?content, ?embeds, ?allowedMentions, ?components, ?attachments, ?poll, ?files
) =
    member val Content: string option option = content
    member val Embeds: Embed list option option = embeds
    member val AllowedMentions: AllowedMentions option option = allowedMentions
    member val Components: Component list option option = components
    member val Attachments: Attachment list option option = attachments
    member val Poll: Poll option option = poll
    member val Files: Media list = defaultArg files []

    static member Encoder(v: EditOriginalInteractionResponsePayload) =
        Encode.object ([]
            |> Encode.optinull "content" Encode.string v.Content
            |> Encode.optinull "embeds" (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optinull "allowed_mentions" AllowedMentions.encoder v.AllowedMentions
            |> Encode.optinull "components" (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optinull "attachments" (List.map Attachment.encoder >> Encode.list) v.Attachments
            |> Encode.optinull "poll" Poll.encoder v.Poll
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles EditOriginalInteractionResponsePayload.Encoder this this.Files
           
type CreateFollowUpMessagePayload(
    ?content, ?tts, ?embeds, ?allowedMentions, ?components, ?attachments, ?flags, ?poll, ?files
) =
    member val Content: string option = content
    member val Tts: bool option = tts
    member val Embeds: Embed list option = embeds
    member val AllowedMentions: AllowedMentions option = allowedMentions
    member val Components: Component list option = components
    member val Attachments: Attachment list option = attachments
    member val Flags: MessageFlag list option = flags
    member val Poll: Poll option = poll
    member val Files: Media list = defaultArg files []

    static member Encoder(v: CreateFollowUpMessagePayload) =
        Encode.object ([]
            |> Encode.optional "content" Encode.string v.Content
            |> Encode.optional "tts" Encode.bool v.Tts
            |> Encode.optional "embeds" (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optional "allowed_mentions" AllowedMentions.encoder v.AllowedMentions
            |> Encode.optional "components" (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optional "attachments" (List.map Attachment.encoder >> Encode.list) v.Attachments
            |> Encode.optional "flags" Encode.bitfield v.Flags
            |> Encode.optional "poll" Poll.encoder v.Poll
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles CreateFollowUpMessagePayload.Encoder this this.Files

    // TODO: Should this support thread_name and applied_tags? Not explicitly refused in docs but wouldn't make sense
    
type EditFollowupMessagePayload(?content, ?embeds, ?allowedMentions, ?components, ?attachments, ?poll, ?files) =
    member val Content: string option option = content
    member val Embeds: Embed list option option = embeds
    member val AllowedMentions: AllowedMentions option option = allowedMentions
    member val Components: Component list option option = components
    member val Attachments: Attachment list option option = attachments
    member val Poll: Poll option option = poll
    member val Files: Media list = defaultArg files []

    static member Encoder(v: EditFollowupMessagePayload) =
        Encode.object ([]
            |> Encode.optinull "content" Encode.string v.Content
            |> Encode.optinull "embeds" (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optinull "allowed_mentions" AllowedMentions.encoder v.AllowedMentions
            |> Encode.optinull "components" (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optinull "attachments" (List.map Attachment.encoder >> Encode.list) v.Attachments
            |> Encode.optinull "poll" Poll.encoder v.Poll
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles EditFollowupMessagePayload.Encoder this this.Files
            
// ----- Interactions: Application Commands -----

type CreateGlobalApplicationCommandPayload(
    name, ?nameLocalizations, ?description, ?descriptionLocalizations, ?options, ?defaultMemberPermissions,
    ?integrationTypes, ?contexts, ?type', ?nsfw
) =
    member val Name: string = name
    member val NameLocalizations: Map<string, string> option option = nameLocalizations
    member val Description: string option = description
    member val DescriptionLocalizations: Map<string, string> option option = descriptionLocalizations
    member val Options: ApplicationCommandOption list option = options
    member val DefaultMemberPermissions: string option option = defaultMemberPermissions
    member val IntegrationTypes: ApplicationIntegrationType list option = integrationTypes
    member val Contexts: InteractionContextType list option = contexts
    member val Type: ApplicationCommandType option = type'
    member val Nsfw: bool option = nsfw
    
    static member Encoder(v: CreateGlobalApplicationCommandPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optinull "name_localizations" (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optinull "description_localizations" (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional "options" (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.optinull "default_member_permissions" Encode.string v.DefaultMemberPermissions
            |> Encode.optional "integration_types" (List.map Encode.Enum.int<ApplicationIntegrationType> >> Encode.list) v.IntegrationTypes
            |> Encode.optional "contexts" (List.map Encode.Enum.int<InteractionContextType> >> Encode.list) v.Contexts
            |> Encode.optional "type" Encode.Enum.int<ApplicationCommandType> v.Type
            |> Encode.optional "nsfw" Encode.bool v.Nsfw
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGlobalApplicationCommandPayload.Encoder this
            
type EditGlobalApplicationCommandPayload(
    ?name, ?nameLocalizations, ?description, ?descriptionLocalizations, ?options, ?defaultMemberPermissions,
    ?integrationTypes, ?contexts, ?type', ?nsfw
) =
    member val Name: string option = name
    member val NameLocalizations: Map<string, string> option option = nameLocalizations
    member val Description: string option = description
    member val DescriptionLocalizations: Map<string, string> option option = descriptionLocalizations
    member val Options: ApplicationCommandOption list option = options
    member val DefaultMemberPermissions: bool option option = defaultMemberPermissions
    member val IntegrationTypes: ApplicationIntegrationType list option = integrationTypes
    member val Contexts: InteractionContextType list option = contexts
    member val Nsfw: bool option = nsfw
    
    static member Encoder(v: EditGlobalApplicationCommandPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "name_localizations" (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optinull "description_localizations" (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional "options" (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.optinull "default_member_permissions" Encode.bool v.DefaultMemberPermissions
            |> Encode.optional "integration_types" (List.map Encode.Enum.int<ApplicationIntegrationType> >> Encode.list) v.IntegrationTypes
            |> Encode.optional "contexts" (List.map Encode.Enum.int<InteractionContextType> >> Encode.list) v.Contexts
            |> Encode.optional "nsfw" Encode.bool v.Nsfw
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson EditGlobalApplicationCommandPayload.Encoder this
            
type BulkOverwriteGlobalApplicationCommandsPayload(commands) =
    member val Commands: CreateGlobalApplicationCommandPayload list = commands
    
    static member Encoder(v: BulkOverwriteGlobalApplicationCommandsPayload) =
        (List.map CreateGlobalApplicationCommandPayload.Encoder >> Encode.list) v.Commands
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson BulkOverwriteGlobalApplicationCommandsPayload.Encoder this

    // TODO: Docs appear incorrect currently, likely also contains `id` like bulk guild commands, which also looks partially incorrectly documented
    
type CreateGuildApplicationCommandPayload(
    name, ?nameLocalizations, ?description, ?descriptionLocalizations, ?options, ?defaultMemberPermissions,
    ?type', ?nsfw
) =
    member val Name: string = name
    member val NameLocalizations: Map<string, string> option option = nameLocalizations
    member val Description: string option = description
    member val DescriptionLocalizations: Map<string, string> option option = descriptionLocalizations
    member val Options: ApplicationCommandOption list option = options
    member val DefaultMemberPermissions: string option option = defaultMemberPermissions
    member val Type: ApplicationCommandType option = type'
    member val Nsfw: bool option = nsfw
    
    static member Encoder(v: CreateGuildApplicationCommandPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optinull "name_localizations" (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optinull "description_localizations" (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional "options" (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.optinull "default_member_permissions" Encode.string v.DefaultMemberPermissions
            |> Encode.optional "type" Encode.Enum.int<ApplicationCommandType> v.Type
            |> Encode.optional "nsfw" Encode.bool v.Nsfw
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildApplicationCommandPayload.Encoder this
            
type EditGuildApplicationCommandPayload(
    ?name, ?nameLocalizations, ?description, ?descriptionLocalizations, ?options, ?defaultMemberPermissions, ?nsfw
) =
    member val Name: string option = name
    member val NameLocalizations: Map<string, string> option option = nameLocalizations
    member val Description: string option = description
    member val DescriptionLocalizations: Map<string, string> option option = descriptionLocalizations
    member val Options: ApplicationCommandOption list option = options
    member val DefaultMemberPermissions: bool option option = defaultMemberPermissions
    member val Nsfw: bool option = nsfw
    
    static member Encoder(v: EditGuildApplicationCommandPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "name_localizations" (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optinull "description_localizations" (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional "options" (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.optinull "default_member_permissions" Encode.bool v.DefaultMemberPermissions
            |> Encode.optional "nsfw" Encode.bool v.Nsfw
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson EditGuildApplicationCommandPayload.Encoder this
            
type BulkOverwriteGuildApplicationCommandsPayload(commands) =
    member val Commands: CreateGuildApplicationCommandPayload list = commands
    
    static member Encoder(v: BulkOverwriteGuildApplicationCommandsPayload) =
        (List.map CreateGuildApplicationCommandPayload.Encoder >> Encode.list) v.Commands
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson BulkOverwriteGuildApplicationCommandsPayload.Encoder this

    // TODO: Also contains optional `id` property "ID of the command, if known"
    // TODO: Docs include `integration_types` and `contexts` but these should be global only afaik
    
type EditApplicationCommandPermissionsPayload(permissions) =
    member val Permissions: ApplicationCommandPermission list = permissions
    
    static member Encoder(v: EditApplicationCommandPermissionsPayload) =
        Encode.object ([]
            |> Encode.required "permissions" (List.map ApplicationCommandPermission.encoder >> Encode.list) v.Permissions
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson EditApplicationCommandPermissionsPayload.Encoder this
            
// ----- Events: Using Gateway -----

// ----- Resources: Application -----

type EditCurrentApplicationPayload(
    customInstallUrl, description, roleConnectionsVerificationUrl, installParams, integrationTypesConfig, flags, icon,
    coverImage, interactionsEndpointUrl, tags, eventWebhooksUrl, eventWebhooksStatus, eventWebhooksTypes
) =
    member val CustomInstallUrl: string option = customInstallUrl
    member val Description: string option = description
    member val RoleConnectionsVerificationUrl: string option = roleConnectionsVerificationUrl
    member val InstallParams: InstallParams option = installParams
    member val IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option = integrationTypesConfig
    member val Flags: ApplicationFlag list option = flags
    member val Icon: string option option = icon
    member val CoverImage: string option option = coverImage
    member val InteractionsEndpointUrl: string option = interactionsEndpointUrl
    member val Tags: string list option = tags
    member val EventWebhooksUrl: string option = eventWebhooksUrl
    member val EventWebhooksStatus: WebhookEventStatus option = eventWebhooksStatus
    member val EventWebhooksTypes: WebhookEventType list option = eventWebhooksTypes

    static member Encoder (v: EditCurrentApplicationPayload) =
        Encode.object ([]
            |> Encode.optional "custom_install_url" Encode.string v.CustomInstallUrl
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optional "role_connections_verification_url" Encode.string v.RoleConnectionsVerificationUrl
            |> Encode.optional "install_params" InstallParams.encoder v.InstallParams
            |> Encode.optional "integration_types_config" (Encode.mapkv ApplicationIntegrationType.toString ApplicationIntegrationTypeConfiguration.encoder) v.IntegrationTypesConfig
            |> Encode.optional "flags" Encode.bitfield v.Flags
            |> Encode.optinull "icon" Encode.string v.Icon
            |> Encode.optinull "cover_image" Encode.string v.CoverImage
            |> Encode.optional "interactions_endpoint_url" Encode.string v.InteractionsEndpointUrl
            |> Encode.optional "tags" (List.map Encode.string >> Encode.list) v.Tags
            |> Encode.optional "event_webhooks_url" Encode.string v.EventWebhooksUrl
            |> Encode.optional "event_webhooks_status" Encode.Enum.int v.EventWebhooksStatus
            |> Encode.optional "event_webhooks_types" (List.map WebhookEventType.encoder >> Encode.list) v.EventWebhooksTypes
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson EditCurrentApplicationPayload.Encoder this
            
// ----- Resources: Application Role Connection Metadata -----

type UpdateApplicationRoleConnectionMetadataRecordsPayload(records) =
    member val Records: ApplicationRoleConnectionMetadata list = records
    
    static member Encoder(v: UpdateApplicationRoleConnectionMetadataRecordsPayload) =
        Encode.object ([]
            |> Encode.required "records" (List.map ApplicationRoleConnectionMetadata.encoder >> Encode.list) v.Records
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson UpdateApplicationRoleConnectionMetadataRecordsPayload.Encoder this

// ----- Resources: Audit Log -----

// ----- Resources: Auto Moderation -----

type CreateAutoModerationRulePayload(
    name, eventType, triggerType, actions, ?triggerMetadata, ?enabled, ?exemptRoles, ?exemptChannels
) =
    member val Name: string = name
    member val EventType: AutoModerationEventType = eventType
    member val TriggerType: AutoModerationTriggerType = triggerType
    member val TriggerMetadata: AutoModerationTriggerMetadata option = triggerMetadata
    member val Actions: AutoModerationAction list = actions
    member val Enabled: bool option = enabled
    member val ExemptRoles: string list option = exemptRoles
    member val ExemptChannels: string list option = exemptChannels

    static member Encoder(v: CreateAutoModerationRulePayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.required "event_type" Encode.Enum.int v.EventType
            |> Encode.required "trigger_type" Encode.Enum.int v.TriggerType
            |> Encode.optional "trigger_metadata" AutoModerationTriggerMetadata.encoder v.TriggerMetadata
            |> Encode.required "actions" (List.map AutoModerationAction.encoder >> Encode.list) v.Actions
            |> Encode.optional "enabled" Encode.bool v.Enabled
            |> Encode.optional "exempt_roles" (List.map Encode.string >> Encode.list) v.ExemptRoles
            |> Encode.optional "exempt_channels" (List.map Encode.string >> Encode.list) v.ExemptChannels
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateAutoModerationRulePayload.Encoder this
            
type ModifyAutoModerationRulePayload(
    ?name, ?eventType, ?triggerType, ?actions, ?triggerMetadata, ?enabled, ?exemptRoles, ?exemptChannels
) =
    member val Name: string option = name
    member val EventType: AutoModerationEventType option = eventType
    member val TriggerType: AutoModerationTriggerType option = triggerType
    member val TriggerMetadata: AutoModerationTriggerMetadata option = triggerMetadata
    member val Actions: AutoModerationAction list option = actions
    member val Enabled: bool option = enabled
    member val ExemptRoles: string list option = exemptRoles
    member val ExemptChannels: string list option = exemptChannels

    static member Encoder(v: ModifyAutoModerationRulePayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optional "event_type" Encode.Enum.int v.EventType
            |> Encode.optional "trigger_type" Encode.Enum.int v.TriggerType
            |> Encode.optional "trigger_metadata" AutoModerationTriggerMetadata.encoder v.TriggerMetadata
            |> Encode.optional "actions" (List.map AutoModerationAction.encoder >> Encode.list) v.Actions
            |> Encode.optional "enabled" Encode.bool v.Enabled
            |> Encode.optional "exempt_roles" (List.map Encode.string >> Encode.list) v.ExemptRoles
            |> Encode.optional "exempt_channels" (List.map Encode.string >> Encode.list) v.ExemptChannels
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyAutoModerationRulePayload.Encoder this

// ----- Resources: Channel -----

type ModifyGroupDmChannelPayload(?name, ?icon) =
    member val Name: string option = name
    member val Icon: string option = icon

    static member Encoder(v: ModifyGroupDmChannelPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optional "icon" Encode.string v.Icon
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGroupDmChannelPayload.Encoder this
            
type ModifyGuildTextChannelPayload(
    ?name, ?type', ?position, ?topic, ?nsfw, ?rateLimitPerUser, ?permissionOverwrites, ?parentId,
    ?defaultAutoArchiveDuration, ?defaultThreadRateLimitPerUser
) =
    member val Name: string option = name
    member val Type: ChannelType option = type'
    member val Position: int option option = position
    member val Topic: string option option = topic
    member val Nsfw: bool option option = nsfw
    member val RateLimitPerUser: int option option = rateLimitPerUser
    member val PermissionOverwrites: PermissionOverwrite list option option = permissionOverwrites
    member val ParentId: string option option = parentId
    member val DefaultAutoArchiveDuration: AutoArchiveDuration option = defaultAutoArchiveDuration
    member val DefaultThreadRateLimitPerUser: int option = defaultThreadRateLimitPerUser

    static member Encoder(v: ModifyGuildTextChannelPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optional "type" Encode.Enum.int v.Type
            |> Encode.optinull "position" Encode.int v.Position
            |> Encode.optinull "topic" Encode.string v.Topic
            |> Encode.optinull "nsfw" Encode.bool v.Nsfw
            |> Encode.optinull "rate_limit_per_user" Encode.int v.RateLimitPerUser
            |> Encode.optinull "permission_overwrites" (List.map PermissionOverwrite.encoder >> Encode.list) v.PermissionOverwrites
            |> Encode.optinull "parent_id" Encode.string v.ParentId
            |> Encode.optional "default_auto_archive_duration" Encode.Enum.int v.DefaultAutoArchiveDuration
            |> Encode.optional "default_thread_rate_limit_per_user" Encode.int v.DefaultThreadRateLimitPerUser
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildTextChannelPayload.Encoder this

type ModifyGuildForumChannelPayload(
    ?name, ?position,?topic, ?nsfw, ?rateLimitPerUser, ?permissionOverwrites, ?parentId, ?defaultAutoArchiveDuration,
    ?flags, ?availableTags, ?defaultReaction, ?defaultThreadRateLimitPerUser, ?defaultSortOrder, ?defaultForumLayout
) =
    member val Name: string option = name
    member val Position: int option option = position
    member val Topic: string option option = topic
    member val Nsfw: bool option option = nsfw
    member val RateLimitPerUser: int option option = rateLimitPerUser
    member val PermissionOverwrites: PermissionOverwrite list option option = permissionOverwrites
    member val ParentId: string option option = parentId
    member val DefaultAutoArchiveDuration: AutoArchiveDuration option = defaultAutoArchiveDuration
    member val Flags: ChannelFlag list option = flags
    member val AvailableTags: ForumTag list option = availableTags
    member val DefaultReactionEmoji: DefaultReaction option option = defaultReaction
    member val DefaultThreadRateLimitPerUser: int option = defaultThreadRateLimitPerUser
    member val DefaultSortOrder: ChannelSortOrder option = defaultSortOrder
    member val DefaultForumLayout: ForumLayout option = defaultForumLayout

    static member Encoder(v: ModifyGuildForumChannelPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "position" Encode.int v.Position
            |> Encode.optinull "topic" Encode.string v.Topic
            |> Encode.optinull "nsfw" Encode.bool v.Nsfw
            |> Encode.optinull "rate_limit_per_user" Encode.int v.RateLimitPerUser
            |> Encode.optinull "permission_overwrites" (List.map PermissionOverwrite.encoder >> Encode.list) v.PermissionOverwrites
            |> Encode.optinull "parent_id" Encode.string v.ParentId
            |> Encode.optional "default_auto_archive_duration" Encode.Enum.int v.DefaultAutoArchiveDuration
            |> Encode.optional "flags" Encode.bitfield v.Flags
            |> Encode.optional "available_tags" (List.map ForumTag.encoder >> Encode.list) v.AvailableTags
            |> Encode.optinull "default_reaction_emoji" DefaultReaction.encoder v.DefaultReactionEmoji
            |> Encode.optional "default_thread_rate_limit_per_user" Encode.int v.DefaultThreadRateLimitPerUser
            |> Encode.optional "default_sort_order" Encode.Enum.int<ChannelSortOrder> v.DefaultSortOrder
            |> Encode.optional "default_forum_layout" Encode.Enum.int<ForumLayout> v.DefaultForumLayout
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildForumChannelPayload.Encoder this

type ModifyGuildMediaChannelPayload(
    ?name, ?position,?topic, ?nsfw, ?rateLimitPerUser, ?permissionOverwrites, ?parentId, ?defaultAutoArchiveDuration,
    ?flags, ?availableTags, ?defaultReaction, ?defaultThreadRateLimitPerUser, ?defaultSortOrder
) =
    member val Name: string option = name
    member val Position: int option option = position
    member val Topic: string option option = topic
    member val Nsfw: bool option option = nsfw
    member val RateLimitPerUser: int option option = rateLimitPerUser
    member val PermissionOverwrites: PermissionOverwrite list option option = permissionOverwrites
    member val ParentId: string option option = parentId
    member val DefaultAutoArchiveDuration: AutoArchiveDuration option = defaultAutoArchiveDuration
    member val Flags: ChannelFlag list option = flags
    member val AvailableTags: ForumTag list option = availableTags
    member val DefaultReactionEmoji: DefaultReaction option option = defaultReaction
    member val DefaultThreadRateLimitPerUser: int option = defaultThreadRateLimitPerUser
    member val DefaultSortOrder: ChannelSortOrder option = defaultSortOrder

    static member Encoder(v: ModifyGuildMediaChannelPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "position" Encode.int v.Position
            |> Encode.optinull "topic" Encode.string v.Topic
            |> Encode.optinull "nsfw" Encode.bool v.Nsfw
            |> Encode.optinull "rate_limit_per_user" Encode.int v.RateLimitPerUser
            |> Encode.optinull "permission_overwrites" (List.map PermissionOverwrite.encoder >> Encode.list) v.PermissionOverwrites
            |> Encode.optinull "parent_id" Encode.string v.ParentId
            |> Encode.optional "default_auto_archive_duration" Encode.Enum.int v.DefaultAutoArchiveDuration
            |> Encode.optional "flags" Encode.bitfield v.Flags
            |> Encode.optional "available_tags" (List.map ForumTag.encoder >> Encode.list) v.AvailableTags
            |> Encode.optinull "default_reaction_emoji" DefaultReaction.encoder v.DefaultReactionEmoji
            |> Encode.optional "default_thread_rate_limit_per_user" Encode.int v.DefaultThreadRateLimitPerUser
            |> Encode.optional "default_sort_order" Encode.Enum.int<ChannelSortOrder> v.DefaultSortOrder
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildMediaChannelPayload.Encoder this

type ModifyGuildVoiceChannelPayload(
    ?name, ?position, ?bitrate, ?userLimit, ?permissionOverwrites, ?parentId, ?rtcRegion, ?videoQualityMode
) =
    member val Name: string option = name
    member val Position: int option option = position
    member val Bitrate: int option option = bitrate
    member val UserLimit: int option option = userLimit
    member val PermissionOverwrites: PermissionOverwrite list option option = permissionOverwrites
    member val ParentId: string option option = parentId
    member val RtcRegion: string option option = rtcRegion
    member val VideoQualityMode: VideoQualityMode option option = videoQualityMode

    static member Encoder(v: ModifyGuildVoiceChannelPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "position" Encode.int v.Position
            |> Encode.optinull "bitrate" Encode.int v.Bitrate
            |> Encode.optinull "user_limit" Encode.int v.UserLimit
            |> Encode.optinull "permission_overwrites" (List.map PermissionOverwrite.encoder >> Encode.list) v.PermissionOverwrites
            |> Encode.optinull "parent_id" Encode.string v.ParentId
            |> Encode.optinull "rtc_region" Encode.string v.RtcRegion
            |> Encode.optinull "video_quality_mode" Encode.Enum.int<VideoQualityMode> v.VideoQualityMode
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildVoiceChannelPayload.Encoder this

type ModifyThreadChannelPayload(
    ?name, ?archived, ?autoArchiveDuration, ?locked, ?invitable, ?rateLimitPerUser, ?flags, ?appliedTags
) =
    member val Name: string option = name
    member val Archived: bool option = archived
    member val AutoArchiveDuration: AutoArchiveDuration option = autoArchiveDuration
    member val Locked: bool option = locked
    member val Invitable: bool option = invitable
    member val RateLimitPerUser: int option option = rateLimitPerUser
    member val Flags: ChannelFlag list option = flags
    member val AppliedTags: string list option = appliedTags

    static member Encoder(v: ModifyThreadChannelPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optional "archived" Encode.bool v.Archived
            |> Encode.optional "auto_archive_duration" Encode.Enum.int v.AutoArchiveDuration
            |> Encode.optional "locked" Encode.bool v.Locked
            |> Encode.optional "invitable" Encode.bool v.Invitable
            |> Encode.optinull "rate_limit_per_user" Encode.int v.RateLimitPerUser
            |> Encode.optional "flags" Encode.bitfield v.Flags
            |> Encode.optional "applied_tags" (List.map Encode.string >> Encode.list) v.AppliedTags
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyThreadChannelPayload.Encoder this

type ModifyChannelPayload =
    | GroupDm of ModifyGroupDmChannelPayload
    | Text of ModifyGuildTextChannelPayload
    | Announcement of ModifyGuildTextChannelPayload
    | Forum of ModifyGuildForumChannelPayload
    | Media of ModifyGuildMediaChannelPayload
    | Voice of ModifyGuildVoiceChannelPayload
    | Stage of ModifyGuildVoiceChannelPayload
    | Thread of ModifyThreadChannelPayload

module ModifyChannelPayload =
    let encoder (v: ModifyChannelPayload) =
        match v with
        | ModifyChannelPayload.GroupDm v -> ModifyGroupDmChannelPayload.Encoder v
        | ModifyChannelPayload.Text v -> ModifyGuildTextChannelPayload.Encoder v
        | ModifyChannelPayload.Announcement v -> ModifyGuildTextChannelPayload.Encoder v
        | ModifyChannelPayload.Forum v -> ModifyGuildForumChannelPayload.Encoder v
        | ModifyChannelPayload.Media v -> ModifyGuildMediaChannelPayload.Encoder v
        | ModifyChannelPayload.Voice v -> ModifyGuildVoiceChannelPayload.Encoder v
        | ModifyChannelPayload.Stage v -> ModifyGuildVoiceChannelPayload.Encoder v
        | ModifyChannelPayload.Thread v -> ModifyThreadChannelPayload.Encoder v

    let toPayload (v: ModifyChannelPayload) =
        match v with
        | ModifyChannelPayload.GroupDm v -> v :> IPayload
        | ModifyChannelPayload.Text v -> v :> IPayload
        | ModifyChannelPayload.Announcement v -> v :> IPayload
        | ModifyChannelPayload.Forum v -> v :> IPayload
        | ModifyChannelPayload.Media v -> v :> IPayload
        | ModifyChannelPayload.Voice v -> v :> IPayload
        | ModifyChannelPayload.Stage v -> v :> IPayload
        | ModifyChannelPayload.Thread v -> v :> IPayload

type EditChannelPermissionsType =
    | ROLE   = 0
    | MEMBER = 1

type EditChannelPermissionsPayload(type', ?allow, ?deny) =
    member val Allow: string option = allow
    member val Deny: string option = deny
    member val Type: EditChannelPermissionsType = type'

    static member Encoder(v: EditChannelPermissionsPayload) =
        Encode.object ([]
            |> Encode.optional "allow" Encode.string v.Allow
            |> Encode.optional "deny" Encode.string v.Deny
            |> Encode.required "type" Encode.Enum.int v.Type
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson EditChannelPermissionsPayload.Encoder this

type CreateChannelInvitePayload(?maxAge, ?maxUses, ?temporary, ?unique, ?targetType, ?targetUserId, ?targetApplicationId) =
    member val MaxAge: int option = maxAge
    member val MaxUses: int option = maxUses
    member val Temporary: bool option = temporary
    member val Unique: bool option = unique
    member val TargetType: InviteTargetType option = targetType
    member val TargetUserId: string option = targetUserId
    member val TargetApplicationId: string option = targetApplicationId

    static member Encoder(v: CreateChannelInvitePayload) =
        Encode.object ([]
            |> Encode.optional "max_age" Encode.int v.MaxAge
            |> Encode.optional "max_uses" Encode.int v.MaxUses
            |> Encode.optional "temporary" Encode.bool v.Temporary
            |> Encode.optional "unique" Encode.bool v.Unique
            |> Encode.optional "target_type" Encode.Enum.int v.TargetType
            |> Encode.optional "target_user_id" Encode.string v.TargetUserId
            |> Encode.optional "target_application_id" Encode.string v.TargetApplicationId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateChannelInvitePayload.Encoder this

type FollowAnnouncementChannelPayload(webhookChannelId) =
    member val webhookChannelId: string = webhookChannelId

    static member Encoder(v: FollowAnnouncementChannelPayload) =
        Encode.object ([]
            |> Encode.required "webhook_channel_id" Encode.string v.webhookChannelId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson FollowAnnouncementChannelPayload.Encoder this

type GroupDmAddRecipientPayload(accessToken, nick) =
    member val AccessToken: string = accessToken
    member val Nick: string option = nick // TODO: Confirm this can be option (isn't in docs)

    static member Encoder(v: GroupDmAddRecipientPayload) =
        Encode.object ([]
            |> Encode.required "access_token" Encode.string v.AccessToken
            |> Encode.optional "nick" Encode.string v.Nick
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson GroupDmAddRecipientPayload.Encoder this

type StartThreadFromMessagePayload(name, ?autoArchiveDuration, ?rateLimitPerUser) =
    member val Name: string = name
    member val AutoArchiveDuration: AutoArchiveDuration option option = autoArchiveDuration
    member val RateLimitPerUser: int option = rateLimitPerUser

    static member Encoder(v: StartThreadFromMessagePayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optinull "auto_archive_duration" Encode.Enum.int v.AutoArchiveDuration
            |> Encode.optional "rate_limit_per_user" Encode.int v.RateLimitPerUser
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson StartThreadFromMessagePayload.Encoder this

type StartThreadWithoutMessagePayload(name, type', ?autoArchiveDuration, ?invitable, ?rateLimitPerUser) =
    member val Name: string = name
    member val AutoArchiveDuration: AutoArchiveDuration option = autoArchiveDuration
    member val Type: ChannelType = type'
    member val Invitable: bool option = invitable
    member val RateLimitPerUser: int option option = rateLimitPerUser

    static member Encoder(v: StartThreadWithoutMessagePayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optional "auto_archive_duration" Encode.Enum.int v.AutoArchiveDuration
            |> Encode.required "type" Encode.Enum.int v.Type
            |> Encode.optional "invitable" Encode.bool v.Invitable
            |> Encode.optinull "rate_limit_per_user" Encode.int v.RateLimitPerUser
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson StartThreadWithoutMessagePayload.Encoder this

type ForumAndMediaThreadMessageParams = {
    Content: string option
    Embeds: Embed list option
    AllowedMentions: AllowedMentions option
    Components: Component list option
    StickerIds: string list option
    Attachments: PartialAttachment list option
    Flags: MessageFlag list option
}

module ForumAndMediaThreadMessageParams =
    let encoder (v: ForumAndMediaThreadMessageParams) =
        Encode.object ([]
            |> Encode.optional "content" Encode.string v.Content
            |> Encode.optional "embeds" (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optional "allowed_mentions" AllowedMentions.encoder v.AllowedMentions
            |> Encode.optional "components" (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optional "sticker_ids" (List.map Encode.string >> Encode.list) v.StickerIds
            |> Encode.optional "attachments" (List.map Attachment.Partial.encoder >> Encode.list) v.Attachments
            |> Encode.optional "flags" Encode.bitfield v.Flags
        )

type StartThreadInForumOrMediaChannelPayload(name, message, ?autoArchiveDuration, ?rateLimitPerUser, ?appliedTags, ?files) =
    member val Name: string = name
    member val AutoArchiveDuration: AutoArchiveDuration option = autoArchiveDuration
    member val RateLimitPerUser: int option option = rateLimitPerUser
    member val Message: ForumAndMediaThreadMessageParams = message
    member val AppliedTags: string list option = appliedTags
    member val Files: Media list = defaultArg files []

    static member Encoder(v: StartThreadInForumOrMediaChannelPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optional "auto_archive_duration" Encode.Enum.int v.AutoArchiveDuration
            |> Encode.optinull "rate_limit_per_user" Encode.int v.RateLimitPerUser
            |> Encode.required "message" ForumAndMediaThreadMessageParams.encoder v.Message
            |> Encode.optional "applied_tags" (List.map Encode.string >> Encode.list) v.AppliedTags
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles StartThreadInForumOrMediaChannelPayload.Encoder this this.Files

type StartThreadInForumOrMediaChannelResponse = {
    Channel: Channel
    Message: Message
}

module StartThreadInForumOrMediaChannelResponse =
    let decoder: Decoder<StartThreadInForumOrMediaChannelResponse> =
        Decode.object (fun get -> {
            Channel = get |> Get.extract Channel.decoder
            Message = get |> Get.required "message" Message.decoder
        })

// TODO: Double check this response structure, old rest has message as option but doesn't appear to be in docs
            
// ----- Resources: Emoji -----

// ----- Resources: Entitlement -----

// ----- Resources: Guild -----

// ----- Resources: Guild Scheduled Event -----

// ----- Resources: Guild Template -----

// ----- Resources: Invite -----

// ----- Resources: Lobby -----

// ----- Resources: Message -----

// ----- Resources: Poll -----

// ----- Resources: SKU -----

// ----- Resources: Soundboard -----

// ----- Resources: Stage Instance -----

// ----- Resources: Sticker -----

// ----- Resources: Subscription -----

// ----- Resources: User -----

// ----- Resources: Voice -----

// ----- Resources: Webhook -----

// ----- Topics: OAuth2 -----
