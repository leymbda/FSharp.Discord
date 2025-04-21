namespace FSharp.Discord.Rest

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open System.Net.Http
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
    member val DefaultMemberPermissions: Permission list option option = defaultMemberPermissions
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
            |> Encode.optinull "default_member_permissions" Encode.bitfieldL v.DefaultMemberPermissions
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
    member val DefaultMemberPermissions: Permission list option option = defaultMemberPermissions
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
            |> Encode.optinull "default_member_permissions" Encode.bitfieldL v.DefaultMemberPermissions
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
    member val DefaultMemberPermissions: Permission list option option = defaultMemberPermissions
    member val Type: ApplicationCommandType option = type'
    member val Nsfw: bool option = nsfw
    
    static member Encoder(v: CreateGuildApplicationCommandPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optinull "name_localizations" (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optinull "description_localizations" (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional "options" (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.optinull "default_member_permissions" Encode.bitfieldL v.DefaultMemberPermissions
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
    member val DefaultMemberPermissions: Permission list option option = defaultMemberPermissions
    member val Nsfw: bool option = nsfw
    
    static member Encoder(v: EditGuildApplicationCommandPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "name_localizations" (Encode.mapv Encode.string) v.NameLocalizations
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optinull "description_localizations" (Encode.mapv Encode.string) v.DescriptionLocalizations
            |> Encode.optional "options" (List.map ApplicationCommandOption.encoder >> Encode.list) v.Options
            |> Encode.optinull "default_member_permissions" Encode.bitfieldL v.DefaultMemberPermissions
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
            
// ----- Resources: Emoji -----

type CreateGuildEmojiPayload(name, image, ?roles) =
    member val Name: string = name
    member val Image: string = image
    member val Roles: string list = defaultArg roles []

    static member Encoder(v: CreateGuildEmojiPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.required "image" Encode.string v.Image
            |> Encode.required "roles" (List.map Encode.string >> Encode.list) v.Roles
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildEmojiPayload.Encoder this

type ModifyGuildEmojiPayload(?name, ?roles) =
    member val Name: string option = name
    member val Roles: string list option option = roles

    static member Encoder(v: ModifyGuildEmojiPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "roles" (List.map Encode.string >> Encode.list) v.Roles
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildEmojiPayload.Encoder this
            
type CreateApplicationEmojiPayload(name, image) =
    member val Name: string = name
    member val Image: string = image

    static member Encoder(v: CreateApplicationEmojiPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.required "image" Encode.string v.Image
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateApplicationEmojiPayload.Encoder this
            
type ModifyApplicationEmojiPayload(?name) =
    member val Name: string option = name

    static member Encoder(v: ModifyApplicationEmojiPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyApplicationEmojiPayload.Encoder this
            
// ----- Resources: Entitlement -----

type EntitlementOwnerType =
    | GUILD_SUBSCRIPTION = 1
    | USER_SUBSCRIPTION  = 2

type CreateTestEntitlementPayload(skuId, ownerId, ownerType) =
    member val SkuId: string = skuId
    member val OwnerId: string = ownerId
    member val OwnerType: EntitlementOwnerType = ownerType

    static member Encoder(v: CreateTestEntitlementPayload) =
        Encode.object ([]
            |> Encode.required "sku_id" Encode.string v.SkuId
            |> Encode.required "owner_id" Encode.string v.OwnerId
            |> Encode.required "owner_type" Encode.Enum.int v.OwnerType
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateTestEntitlementPayload.Encoder this
            
// ----- Resources: Guild Scheduled Event -----

type CreateGuildScheduledEventPayload(
    name, privacyLevel, scheduledStartTime, entityType, ?channelId, ?entityMetadata, ?scheduledEndTime, ?description,
    ?image, ?recurrenceRule
) =
    member val ChannelId: string option = channelId
    member val EntityMetadata: EntityMetadata option = entityMetadata
    member val Name: string = name
    member val PrivacyLevel: PrivacyLevel = privacyLevel
    member val ScheduledStartTime: DateTime = scheduledStartTime
    member val ScheduledEndTime: DateTime option = scheduledEndTime
    member val Description: string option = description
    member val EntityType: ScheduledEntityType = entityType
    member val Image: string option = image
    member val RecurrenceRule: string option = recurrenceRule

    static member Encoder(v: CreateGuildScheduledEventPayload) =
        Encode.object ([]
            |> Encode.optional "channel_id" Encode.string v.ChannelId
            |> Encode.optional "entity_metadata" EntityMetadata.encoder v.EntityMetadata
            |> Encode.required "name" Encode.string v.Name
            |> Encode.required "privacy_level" Encode.Enum.int v.PrivacyLevel
            |> Encode.required "scheduled_start_time" Encode.datetime v.ScheduledStartTime
            |> Encode.optional "scheduled_end_time" Encode.datetime v.ScheduledEndTime
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.required "entity_type" Encode.Enum.int v.EntityType
            |> Encode.optional "image" Encode.string v.Image
            |> Encode.optional "recurrence_rule" Encode.string v.RecurrenceRule
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildScheduledEventPayload.Encoder this

type ModifyGuildScheduledEventPayload(
    ?channelId, ?entityMetadata, ?name, ?privacyLevel, ?scheduledStartTime, ?scheduledEndTime, ?description, ?entityType,
    ?status, ?image, ?recurrenceRule
) =
    member val ChannelId: string option = channelId
    member val EntityMetadata: EntityMetadata option option = entityMetadata
    member val Name: string option = name
    member val PrivacyLevel: PrivacyLevel option = privacyLevel
    member val ScheduledStartTime: DateTime option = scheduledStartTime
    member val ScheduledEndTime: DateTime option = scheduledEndTime
    member val Description: string option option = description
    member val EntityType: ScheduledEntityType option = entityType
    member val Status: EventStatus option = status
    member val Image: string option = image
    member val RecurrenceRule: string option option = recurrenceRule
            
    static member Encoder(v: ModifyGuildScheduledEventPayload) =
        Encode.object ([]
            |> Encode.optional "channel_id" Encode.string v.ChannelId
            |> Encode.optinull "entity_metadata" EntityMetadata.encoder v.EntityMetadata
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optional "privacy_level" Encode.Enum.int v.PrivacyLevel
            |> Encode.optional "scheduled_start_time" Encode.datetime v.ScheduledStartTime
            |> Encode.optional "scheduled_end_time" Encode.datetime v.ScheduledEndTime
            |> Encode.optinull "description" Encode.string v.Description
            |> Encode.optional "entity_type" Encode.Enum.int v.EntityType
            |> Encode.optional "status" Encode.Enum.int v.Status
            |> Encode.optional "image" Encode.string v.Image
            |> Encode.optinull "recurrence_rule" Encode.string v.RecurrenceRule
        )

    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildScheduledEventPayload.Encoder this

// ----- Resources: Guild Template -----

type CreateGuildFromGuildTemplatePayload(name, ?icon) =
    member val Name: string = name
    member val Icon: string option = icon

    static member Encoder(v: CreateGuildFromGuildTemplatePayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optional "icon" Encode.string v.Icon
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildFromGuildTemplatePayload.Encoder this

type CreateGuildTemplatePayload(name, ?description) =
    member val Name: string = name
    member val Description: string option option = description

    static member Encoder(v: CreateGuildTemplatePayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optinull "description" Encode.string v.Description
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildTemplatePayload.Encoder this

type ModifyGuildTemplatePayload(name, description) =
    member val Name: string option option = name
    member val Description: string option option = description

    static member Encoder(v: ModifyGuildTemplatePayload) =
        Encode.object ([]
            |> Encode.optinull "name" Encode.string v.Name
            |> Encode.optinull "description" Encode.string v.Description
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildTemplatePayload.Encoder this

// ----- Resources: Guild -----

type ModifyGuildPayload(
    ?name, ?verificationLevel, ?defaultMessageNotifications, ?explicitContentFilter, ?afkChannelId, ?afkTimeout, ?icon,
    ?ownerId, ?splash, ?discoverySplash, ?banner, ?systemChannelId, ?systemChannelFlags, ?rulesChannelId,
    ?publicUpdatesChannelId, ?preferredLocale, ?features, ?description, ?premiumProgressBarEnabled,
    ?safetyAlertsChannelId
) =
    member val Name: string option = name
    member val VerificationLevel: VerificationLevel option option = verificationLevel
    member val DefaultMessageNotifications: MessageNotificationLevel option option = defaultMessageNotifications
    member val ExplicitContentFilter: ExplicitContentFilterLevel option option = explicitContentFilter
    member val AfkChannelId: string option option = afkChannelId
    member val AfkTimeout: int option = afkTimeout
    member val Icon: string option option = icon
    member val OwnerId: string option = ownerId
    member val Splash: string option option = splash
    member val DiscoverySplash: string option option = discoverySplash
    member val Banner: string option option = banner
    member val SystemChannelId: string option option = systemChannelId
    member val SystemChannelFlags: SystemChannelFlag list option = systemChannelFlags
    member val RulesChannelId: string option option = rulesChannelId
    member val PublicUpdatesChannelId: string option option = publicUpdatesChannelId
    member val PreferredLocale: string option option = preferredLocale
    member val Features: GuildFeature list option = features
    member val Description: string option option = description
    member val PremiumProgressBarEnabled: bool option = premiumProgressBarEnabled
    member val SafetyAlertsChannelId: string option option = safetyAlertsChannelId

    static member Encoder(v: ModifyGuildPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "verification_level" Encode.Enum.int v.VerificationLevel
            |> Encode.optinull "default_message_notifications" Encode.Enum.int v.DefaultMessageNotifications
            |> Encode.optinull "explicit_content_filter" Encode.Enum.int v.ExplicitContentFilter
            |> Encode.optinull "afk_channel_id" Encode.string v.AfkChannelId
            |> Encode.optional "afk_timeout" Encode.int v.AfkTimeout
            |> Encode.optinull "icon" Encode.string v.Icon
            |> Encode.optional "owner_id" Encode.string v.OwnerId
            |> Encode.optinull "splash" Encode.string v.Splash
            |> Encode.optinull "discovery_splash" Encode.string v.DiscoverySplash
            |> Encode.optinull "banner" Encode.string v.Banner
            |> Encode.optinull "system_channel_id" Encode.string v.SystemChannelId
            |> Encode.optional "system_channel_flags" Encode.bitfield v.SystemChannelFlags
            |> Encode.optinull "rules_channel_id" Encode.string v.RulesChannelId
            |> Encode.optinull "public_updates_channel_id" Encode.string v.PublicUpdatesChannelId
            |> Encode.optinull "preferred_locale" Encode.string v.PreferredLocale
            |> Encode.optional "features" (List.map GuildFeature.encoder >> Encode.list) v.Features
            |> Encode.optinull "description" Encode.string v.Description
            |> Encode.optional "premium_progress_bar_enabled" Encode.bool v.PremiumProgressBarEnabled
            |> Encode.optinull "safety_alerts_channel_id" Encode.string v.SafetyAlertsChannelId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildPayload.Encoder this

type CreateGuildChannelPayload(
    name, ?type', ?topic, ?bitrate, ?userLimit, ?rateLimitPerUser, ?position, ?permissionOverwrites, ?parentId, ?nsfw,
    ?rtcRegion, ?videoQualityMode, ?defaultAutoArchiveDuration, ?defaultReactionEmoji, ?availableTags,
    ?defaultSortOrder, ?defaultForumLayout, ?defaultThreadRateLimitPerUser
) =
    member val Name: string = name
    member val Type: ChannelType option option = type'
    member val Topic: string option option = topic
    member val Bitrate: int option option = bitrate
    member val UserLimit: int option option = userLimit
    member val RateLimitPerUser: int option option = rateLimitPerUser
    member val Position: int option option = position
    member val PermissionOverwrites: PartialPermissionOverwrite list option option = permissionOverwrites
    member val ParentId: string option option = parentId
    member val Nsfw: bool option option = nsfw
    member val RtcRegion: string option option = rtcRegion
    member val VideoQualityMode: VideoQualityMode option option = videoQualityMode
    member val DefaultAutoArchiveDuration: AutoArchiveDuration option option = defaultAutoArchiveDuration
    member val DefaultReactionEmoji: DefaultReaction option option = defaultReactionEmoji
    member val AvailableTags: ForumTag list option option = availableTags
    member val DefaultSortOrder: ChannelSortOrder option option = defaultSortOrder
    member val DefaultForumLayout: ForumLayout option option = defaultForumLayout
    member val DefaultThreadRateLimitPerUser: int option option = defaultThreadRateLimitPerUser

    static member Encoder(v: CreateGuildChannelPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.optinull "type" Encode.Enum.int v.Type
            |> Encode.optinull "topic" Encode.string v.Topic
            |> Encode.optinull "bitrate" Encode.int v.Bitrate
            |> Encode.optinull "user_limit" Encode.int v.UserLimit
            |> Encode.optinull "rate_limit_per_user" Encode.int v.RateLimitPerUser
            |> Encode.optinull "position" Encode.int v.Position
            |> Encode.optinull "permission_overwrites" (List.map PermissionOverwrite.Partial.encoder >> Encode.list) v.PermissionOverwrites
            |> Encode.optinull "parent_id" Encode.string v.ParentId
            |> Encode.optinull "nsfw" Encode.bool v.Nsfw
            |> Encode.optinull "rtc_region" Encode.string v.RtcRegion
            |> Encode.optinull "video_quality_mode" Encode.Enum.int<VideoQualityMode> v.VideoQualityMode
            |> Encode.optinull "default_auto_archive_duration" Encode.Enum.int v.DefaultAutoArchiveDuration
            |> Encode.optinull "default_reaction_emoji" DefaultReaction.encoder v.DefaultReactionEmoji
            |> Encode.optinull "available_tags" (List.map ForumTag.encoder >> Encode.list) v.AvailableTags
            |> Encode.optinull "default_sort_order" Encode.Enum.int<ChannelSortOrder> v.DefaultSortOrder
            |> Encode.optinull "default_forum_layout" Encode.Enum.int<ForumLayout> v.DefaultForumLayout
            |> Encode.optinull "default_thread_rate_limit_per_user" Encode.int v.DefaultThreadRateLimitPerUser
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildChannelPayload.Encoder this

type ModifyGuildChannelPosition = {
    Id: string
    Position: int option
    LockPermissions: bool option
    ParentId: string option option
}

module ModifyGuildChannelPosition =
    let encoder (v: ModifyGuildChannelPosition) =
        Encode.object ([]
            |> Encode.required "id" Encode.string v.Id
            |> Encode.optional "position" Encode.int v.Position
            |> Encode.optional "lock_permissions" Encode.bool v.LockPermissions
            |> Encode.optinull "parent_id" Encode.string v.ParentId
        )

type ModifyGuildChannelPositionsPayload(positions) =
    member val positions: ModifyGuildChannelPosition list = positions

    static member Encoder(v: ModifyGuildChannelPositionsPayload) =
        Encode.list (List.map ModifyGuildChannelPosition.encoder v.positions)
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildChannelPositionsPayload.Encoder this

type ListActiveGuildThreadsResponse = {
    Threads: Channel list
    Members: ThreadMember list
}

module ListActiveGuildThreadsResponse =
    let decoder: Decoder<ListActiveGuildThreadsResponse> =
        Decode.object (fun get -> {
            Threads = get |> Get.required "threads" (Decode.list Channel.decoder)
            Members = get |> Get.required "members" (Decode.list ThreadMember.decoder)
        })

type AddGuildMemberPayload(accessToken, ?nick, ?roles, ?mute, ?deaf) =
    member val AccessToken: string = accessToken
    member val Nick: string option = nick
    member val Roles: string list option = roles
    member val Mute: bool option = mute
    member val Deaf: bool option = deaf

    static member Encoder(v: AddGuildMemberPayload) =
        Encode.object ([]
            |> Encode.required "access_token" Encode.string v.AccessToken
            |> Encode.optional "nick" Encode.string v.Nick
            |> Encode.optional "roles" (List.map Encode.string >> Encode.list) v.Roles
            |> Encode.optional "mute" Encode.bool v.Mute
            |> Encode.optional "deaf" Encode.bool v.Deaf
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson AddGuildMemberPayload.Encoder this

type ModifyGuildMemberPayload(?nick, ?roles, ?mute, ?deaf, ?channelId, ?communicationDisabledUntil, ?flags) =
    member val Nick: string option option = nick
    member val Roles: string list option option = roles
    member val Mute: bool option option = mute
    member val Deaf: bool option option = deaf
    member val ChannelId: string option option = channelId
    member val CommunicationDisabledUntil: DateTime option option = communicationDisabledUntil
    member val Flags: GuildMemberFlag list option option = flags

    static member Encoder(v: ModifyGuildMemberPayload) =
        Encode.object ([]
            |> Encode.optinull "nick" Encode.string v.Nick
            |> Encode.optinull "roles" (List.map Encode.string >> Encode.list) v.Roles
            |> Encode.optinull "mute" Encode.bool v.Mute
            |> Encode.optinull "deaf" Encode.bool v.Deaf
            |> Encode.optinull "channel_id" Encode.string v.ChannelId
            |> Encode.optinull "communication_disabled_until" Encode.datetime v.CommunicationDisabledUntil
            |> Encode.optinull "flags" Encode.bitfield v.Flags
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildMemberPayload.Encoder this

type ModifyCurrentMemberPayload(?nick) =
    member val Nick: string option = nick

    static member Encoder(v: ModifyCurrentMemberPayload) =
        Encode.object ([]
            |> Encode.optional "nick" Encode.string v.Nick
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyCurrentMemberPayload.Encoder this

type CreateGuildBanPayload(?deleteMessageSeconds) =
    member val DeleteMessageSeconds: int option = deleteMessageSeconds

    static member Encoder(v: CreateGuildBanPayload) =
        Encode.object ([]
            |> Encode.optional "delete_message_seconds" Encode.int v.DeleteMessageSeconds
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildBanPayload.Encoder this

type BulkGuildBanPayload(userIds, ?deleteMessageSeconds) =
    member val UserIds: string list = userIds
    member val DeleteMessageSeconds: int option = deleteMessageSeconds

    static member Encoder(v: BulkGuildBanPayload) =
        Encode.object ([]
            |> Encode.required "user_ids" (List.map Encode.string >> Encode.list) v.UserIds
            |> Encode.optional "delete_message_seconds" Encode.int v.DeleteMessageSeconds
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson BulkGuildBanPayload.Encoder this

type BulkBanResponse = {
    BannedUsers: string list
    FailedUsers: string list
}

module BulkBanResponse =
    let decoder: Decoder<BulkBanResponse> =
        Decode.object (fun get -> {
            BannedUsers = get |> Get.required "banned_users" (Decode.list Decode.string)
            FailedUsers = get |> Get.required "failed_users" (Decode.list Decode.string)
        })

type CreateGuildRolePayload(?name, ?permissions, ?color, ?hoist, ?icon, ?unicodeEmoji, ?mentionable) =
    member val Name: string option = name
    member val Permissions: string option = permissions
    member val Color: int option = color
    member val Hoist: bool option = hoist
    member val Icon: string option option = icon
    member val UnicodeEmoji: string option option = unicodeEmoji
    member val Mentionable: bool option = mentionable

    static member Encoder(v: CreateGuildRolePayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optional "permissions" Encode.string v.Permissions
            |> Encode.optional "color" Encode.int v.Color
            |> Encode.optional "hoist" Encode.bool v.Hoist
            |> Encode.optinull "icon" Encode.string v.Icon
            |> Encode.optinull "unicode_emoji" Encode.string v.UnicodeEmoji
            |> Encode.optional "mentionable" Encode.bool v.Mentionable
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildRolePayload.Encoder this

type ModifyGuildRolePosition = {
    Id: string
    Position: int option option
}

module ModifyGuildRolePosition =
    let encoder (v: ModifyGuildRolePosition) =
        Encode.object ([]
            |> Encode.required "id" Encode.string v.Id
            |> Encode.optinull "position" Encode.int v.Position
        )

type ModifyGuildRolePositionsPayload(positions) =
    member val Positions: ModifyGuildRolePosition list = positions

    static member Encoder(v: ModifyGuildRolePositionsPayload) =
        Encode.list (List.map ModifyGuildRolePosition.encoder v.Positions)
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildRolePositionsPayload.Encoder this

type ModifyGuildRolePayload(?name, ?permissions, ?color, ?hoist, ?icon, ?unicodeEmoji, ?mentionable) =
    member val Name: string option option = name
    member val Permissions: string option option = permissions
    member val Color: int option option = color
    member val Hoist: bool option option = hoist
    member val Icon: string option option = icon
    member val UnicodeEmoji: string option option = unicodeEmoji
    member val Mentionable: bool option option = mentionable

    static member Encoder(v: ModifyGuildRolePayload) =
        Encode.object ([]
            |> Encode.optinull "name" Encode.string v.Name
            |> Encode.optinull "permissions" Encode.string v.Permissions
            |> Encode.optinull "color" Encode.int v.Color
            |> Encode.optinull "hoist" Encode.bool v.Hoist
            |> Encode.optinull "icon" Encode.string v.Icon
            |> Encode.optinull "unicode_emoji" Encode.string v.UnicodeEmoji
            |> Encode.optinull "mentionable" Encode.bool v.Mentionable
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildRolePayload.Encoder this

type ModifyGuildMfaLevelPayload(level) =
    member val Level: MfaLevel = level

    static member Encoder(v: ModifyGuildMfaLevelPayload) =
        Encode.object ([]
            |> Encode.required "level" Encode.Enum.int v.Level
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildMfaLevelPayload.Encoder this

type GetGuildPruneCountResponse = {
    Pruned: int
}

module GetGuildPruneCountResponse =
    let decoder: Decoder<GetGuildPruneCountResponse> =
        Decode.object (fun get -> {
            Pruned = get |> Get.required "pruned" Decode.int
        })

type BeginGuildPrunePayload(days, computePruneCount, includeRoles) =
    member val Days: int option = days
    member val ComputePruneCount: bool option = computePruneCount
    member val IncludeRoles: string list option = includeRoles

    static member Encoder(v: BeginGuildPrunePayload) =
        Encode.object ([]
            |> Encode.optional "days" Encode.int v.Days
            |> Encode.optional "compute_prune_count" Encode.bool v.ComputePruneCount
            |> Encode.optional "include_roles" (List.map Encode.string >> Encode.list) v.IncludeRoles
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson BeginGuildPrunePayload.Encoder this
            
type BeginGuildPruneResponse = {
    Pruned: int option
}

module BeginGuildPruneResponse =
    let decoder: Decoder<BeginGuildPruneResponse> =
        Decode.object (fun get -> {
            Pruned = get |> Get.optional "pruned" Decode.int
        })

type ModifyGuildWidgetPayload(?enabled, ?channelId) =
    member val Enabled: bool option = enabled
    member val ChannelId: string option option = channelId

    static member Encoder(v: ModifyGuildWidgetPayload) =
        Encode.object ([]
            |> Encode.optional "enabled" Encode.bool v.Enabled
            |> Encode.optinull "channel_id" Encode.string v.ChannelId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildWidgetPayload.Encoder this

type ModifyGuildWelcomeScreenPayload(?enabled, ?welcomeChannels, ?description) =
    member val Enabled: bool option option = enabled
    member val WelcomeChannels: WelcomeScreenChannel list option option = welcomeChannels
    member val Description: string option option = description

    static member Encoder(v: ModifyGuildWelcomeScreenPayload) =
        Encode.object ([]
            |> Encode.optinull "enabled" Encode.bool v.Enabled
            |> Encode.optinull "welcome_channels" (List.map WelcomeScreenChannel.encoder >> Encode.list) v.WelcomeChannels
            |> Encode.optinull "description" Encode.string v.Description
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildWelcomeScreenPayload.Encoder this

type ModifyGuildOnboardingPayload(prompts, defaultChannelIds, enabled, mode) =
    member val Prompts: GuildOnboardingPrompt list = prompts
    member val DefaultChannelIds: string list = defaultChannelIds
    member val Enabled: bool = enabled
    member val Mode: OnboardingMode = mode

    static member Encoder(v: ModifyGuildOnboardingPayload) =
        Encode.object ([]
            |> Encode.required "prompts" (List.map GuildOnboardingPrompt.encoder >> Encode.list) v.Prompts
            |> Encode.required "default_channel_ids" (List.map Encode.string >> Encode.list) v.DefaultChannelIds
            |> Encode.required "enabled" Encode.bool v.Enabled
            |> Encode.required "mode" Encode.Enum.int v.Mode
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildOnboardingPayload.Encoder this

type ModifyGuildIncidentActionsPayload(?invitesDisabledUntil, ?dmsDisabledUntil) =
    member val invitesDisabledUntil: DateTime option option = invitesDisabledUntil
    member val dmsDisabledUntil: DateTime option option = dmsDisabledUntil

    static member Encoder(v: ModifyGuildIncidentActionsPayload) =
        Encode.object ([]
            |> Encode.optinull "invites_disabled_until" Encode.datetime v.invitesDisabledUntil
            |> Encode.optinull "dms_disabled_until" Encode.datetime v.dmsDisabledUntil
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildIncidentActionsPayload.Encoder this

// ----- Resources: Invite -----

// ----- Resources: Lobby -----

type CreateLobbyPayload(?metadata, ?members, ?idleTimeoutSeconds) =
    member val Metadata: Map<string, string> option option = metadata
    member val Members: LobbyMember list option = members
    member val IdleTimeoutSeconds: int option = idleTimeoutSeconds

    static member Encoder(v: CreateLobbyPayload) =
        Encode.object ([]
            |> Encode.optinull "metadata" (Encode.mapv Encode.string) v.Metadata
            |> Encode.optional "members" (List.map LobbyMember.encoder >> Encode.list) v.Members
            |> Encode.optional "idle_timeout_seconds" Encode.int v.IdleTimeoutSeconds
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateLobbyPayload.Encoder this

type ModifyLobbyPayload(?metadata, ?members, ?idleTimeoutSeconds) =
    member val Metadata: Map<string, string> option option = metadata
    member val Members: LobbyMember list option = members
    member val IdleTimeoutSeconds: int option = idleTimeoutSeconds

    static member Encoder(v: ModifyLobbyPayload) =
        Encode.object ([]
            |> Encode.optinull "metadata" (Encode.mapv Encode.string) v.Metadata
            |> Encode.optional "members" (List.map LobbyMember.encoder >> Encode.list) v.Members
            |> Encode.optional "idle_timeout_seconds" Encode.int v.IdleTimeoutSeconds
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyLobbyPayload.Encoder this

type AddMemberToLobbyPayload(?metadata, ?flags) =
    member val Metadata: Map<string, string> option option = metadata
    member val Flags: LobbyMemberFlag list option = flags

    static member Encoder(v: AddMemberToLobbyPayload) =
        Encode.object ([]
            |> Encode.optinull "metadata" (Encode.mapv Encode.string) v.Metadata
            |> Encode.optional "flags" Encode.bitfield v.Flags
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson AddMemberToLobbyPayload.Encoder this

type LinkChannelToLobbyPayload(?channelId) =
    member val ChannelId: string option = channelId

    static member Encoder(v: LinkChannelToLobbyPayload) =
        Encode.object ([]
            |> Encode.optional "channel_id" Encode.string v.ChannelId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson LinkChannelToLobbyPayload.Encoder this

// ----- Resources: Message -----

type CreateMessagePayload(
    ?content, ?nonce, ?tts, ?embeds, ?allowedMentions, ?messageReference, ?components, ?stickerIds, ?attachments,
    ?flags, ?enforceNonce, ?poll, ?files
) =
    member val Content: string option = content
    member val Nonce: MessageNonce option = nonce
    member val Tts: bool option = tts
    member val Embeds: Embed list option = embeds
    member val AllowedMentions: AllowedMentions option = allowedMentions
    member val MessageReference: MessageReference option = messageReference
    member val Components: Component list option = components
    member val StickerIds: string list option = stickerIds
    member val Attachments: PartialAttachment list option = attachments
    member val Flags: MessageFlag list option = flags
    member val EnforceNonce: bool option = enforceNonce
    member val Poll: Poll option = poll
    member val Files: Media list = defaultArg files []

    static member Encoder(v: CreateMessagePayload) =
        Encode.object ([]
            |> Encode.optional "content" Encode.string v.Content
            |> Encode.optional "nonce" MessageNonce.encoder v.Nonce
            |> Encode.optional "tts" Encode.bool v.Tts
            |> Encode.optional "embeds" (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optional "allowed_mentions" AllowedMentions.encoder v.AllowedMentions
            |> Encode.optional "message_reference" MessageReference.encoder v.MessageReference
            |> Encode.optional "components" (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optional "sticker_ids" (List.map Encode.string >> Encode.list) v.StickerIds
            |> Encode.optional "attachments" (List.map Attachment.Partial.encoder >> Encode.list) v.Attachments
            |> Encode.optional "flags" Encode.bitfield v.Flags
            |> Encode.optional "enforce_nonce" Encode.bool v.EnforceNonce
            |> Encode.optional "poll" Poll.encoder v.Poll
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles CreateMessagePayload.Encoder this this.Files
            
type EditMessagePayload(?content, ?embeds, ?flags, ?allowedMentions, ?components, ?attachments, ?files) =
    member val Content: string option option = content
    member val Embeds: Embed list option option = embeds
    member val Flags: MessageFlag list option option = flags
    member val AllowedMentions: AllowedMentions option option = allowedMentions
    member val Components: Component list option option = components
    member val Attachments: PartialAttachment list option option = attachments
    member val Files: Media list = defaultArg files []

    static member Encoder(v: EditMessagePayload) =
        Encode.object ([]
            |> Encode.optinull "content" Encode.string v.Content
            |> Encode.optinull "embeds" (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optinull "flags" Encode.bitfield v.Flags
            |> Encode.optinull "allowed_mentions" AllowedMentions.encoder v.AllowedMentions
            |> Encode.optinull "components" (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optinull "attachments" (List.map Attachment.Partial.encoder >> Encode.list) v.Attachments
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles EditMessagePayload.Encoder this this.Files

type BulkDeleteMessagesPayload(messages) =
    member val Messages: string list = messages

    static member Encoder(v: BulkDeleteMessagesPayload) =
        Encode.object ([]
            |> Encode.required "messages" (List.map Encode.string >> Encode.list) v.Messages
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson BulkDeleteMessagesPayload.Encoder this

// ----- Resources: Poll -----

type GetAnswerVotersResponse = {
    Users: User list
}

module GetAnswerVotersResponse =
    let decoder: Decoder<GetAnswerVotersResponse> =
        Decode.object (fun get -> {
            Users = get |> Get.required "users" (Decode.list User.decoder)
        })

// ----- Resources: SKU -----

// ----- Resources: Soundboard -----

type SendSoundboardSoundPayload(soundId, ?sourceGuildId) =
    member val SoundId: string = soundId
    member val SourceGuildId: string option = sourceGuildId

    static member Encoder(v: SendSoundboardSoundPayload) =
        Encode.object ([]
            |> Encode.required "sound_id" Encode.string v.SoundId
            |> Encode.optional "source_guild_id" Encode.string v.SourceGuildId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson SendSoundboardSoundPayload.Encoder this

type ListGuildSoundboardSoundsResponse = {
    Items: SoundboardSound list
}

module ListGuildSoundboardSoundsResponse =
    let decoder: Decoder<ListGuildSoundboardSoundsResponse> =
        Decode.object (fun get -> {
            Items = get |> Get.required "items" (Decode.list SoundboardSound.decoder)
        })

type CreateGuildSoundboardSoundPayload(name, sound, ?volume, ?emojiId, ?emojiName) =
    member val Name: string = name
    member val Sound: string = sound
    member val Volume: double option option = volume
    member val EmojiId: string option option = emojiId
    member val EmojiName: string option option = emojiName

    static member Encoder(v: CreateGuildSoundboardSoundPayload) =
        Encode.object ([]
            |> Encode.required "name" Encode.string v.Name
            |> Encode.required "sound" Encode.string v.Sound
            |> Encode.optinull "volume" Encode.float v.Volume
            |> Encode.optinull "emoji_id" Encode.string v.EmojiId
            |> Encode.optinull "emoji_name" Encode.string v.EmojiName
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGuildSoundboardSoundPayload.Encoder this

type ModifyGuildSoundboardSoundPayload(?name, ?volume, ?emojiId, ?emojiName) =
    member val Name: string option = name
    member val Volume: double option option = volume
    member val EmojiId: string option option = emojiId
    member val EmojiName: string option option = emojiName
    
    static member Encoder(v: ModifyGuildSoundboardSoundPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optinull "volume" Encode.float v.Volume
            |> Encode.optinull "emoji_id" Encode.string v.EmojiId
            |> Encode.optinull "emoji_name" Encode.string v.EmojiName
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildSoundboardSoundPayload.Encoder this

// ----- Resources: Stage Instance -----

type CreateStageInstancePayload(channelId, topic, ?privacyLevel, ?sendStartNotification, ?guildScheduledEventId) =
    member val ChannelId: string = channelId
    member val Topic: string = topic
    member val PrivacyLevel: PrivacyLevel option = privacyLevel
    member val SendStartNotification: bool option = sendStartNotification
    member val GuildScheduledEventId: string option = guildScheduledEventId

    static member Encoder(v: CreateStageInstancePayload) =
        Encode.object ([]
            |> Encode.required "channel_id" Encode.string v.ChannelId
            |> Encode.required "topic" Encode.string v.Topic
            |> Encode.optional "privacy_level" Encode.Enum.int v.PrivacyLevel
            |> Encode.optional "send_start_notification" Encode.bool v.SendStartNotification
            |> Encode.optional "guild_scheduled_event_id" Encode.string v.GuildScheduledEventId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateStageInstancePayload.Encoder this

type ModifyStageInstancePayload(?topic, ?privacyLevel) =
    member val Topic: string option = topic
    member val PrivacyLevel: PrivacyLevel option = privacyLevel
    
    static member Encoder(v: ModifyStageInstancePayload) =
        Encode.object ([]
            |> Encode.optional "topic" Encode.string v.Topic
            |> Encode.optional "privacy_level" Encode.Enum.int v.PrivacyLevel
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyStageInstancePayload.Encoder this

// ----- Resources: Sticker -----

type ListStickerPacksResponse = {
    StickerPacks: StickerPack list
}

module ListStickerPacksResponse =
    let decoder: Decoder<ListStickerPacksResponse> =
        Decode.object (fun get -> {
            StickerPacks = get |> Get.required "sticker_packs" (Decode.list StickerPack.decoder)
        })

type CreateGuildStickerPayload(name, description, tags, file) =
    member val Name: string = name
    member val Description: string = description
    member val Tags: string = tags // TODO: Should this be a list? Comma delimited? Not documented
    member val File: Media = file
    
    interface IPayload with
        member this.ToHttpContent() =
            MultipartFormDataContent.create()
            |> MultipartFormDataContent.withText "name" this.Name
            |> MultipartFormDataContent.withText "description" this.Description
            |> MultipartFormDataContent.withText "tags" this.Tags
            |> MultipartFormDataContent.withMedia "file" this.File
            :> HttpContent

type ModifyGuildStickerPayload(?name, ?description, ?tags) =
    member val Name: string option = name
    member val Description: string option = description
    member val Tags: string option = tags // TODO: Should this be a list? Comma delimited? Not documented
    
    static member Encoder(v: ModifyGuildStickerPayload) =
        Encode.object ([]
            |> Encode.optional "name" Encode.string v.Name
            |> Encode.optional "description" Encode.string v.Description
            |> Encode.optional "tags" Encode.string v.Tags
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyGuildStickerPayload.Encoder this

// ----- Resources: Subscription -----

// ----- Resources: User -----

type ModifyCurrentUserPayload(?username, ?avatar, ?banner) =
    member val Username: string option = username
    member val Avatar: string option option = avatar
    member val Banner: string option option = banner

    static member Encoder(v: ModifyCurrentUserPayload) =
        Encode.object ([]
            |> Encode.optional "username" Encode.string v.Username
            |> Encode.optinull "avatar" Encode.string v.Avatar
            |> Encode.optinull "banner" Encode.string v.Banner
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson ModifyCurrentUserPayload.Encoder this

type CreateDmPayload(recipientId) =
    member val RecipientId: string = recipientId

    static member Encoder(v: CreateDmPayload) =
        Encode.object ([]
            |> Encode.required "recipient_id" Encode.string v.RecipientId
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateDmPayload.Encoder this

type CreateGroupDmPayload(accessTokens, nicks) =
    member val AccessTokens: string list = accessTokens
    member val Nicks: Map<string, string> = nicks // TODO: Docs do not specify this as optional but seems like it should be

    static member Encoder(v: CreateGroupDmPayload) =
        Encode.object ([]
            |> Encode.required "access_tokens" (List.map Encode.string >> Encode.list) v.AccessTokens
            |> Encode.required "nicks" (Encode.mapv Encode.string) v.Nicks
        )
        
    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson CreateGroupDmPayload.Encoder this

type UpdateCurrentUserApplicationRoleConnectionPayload(?platformName, ?platformUsername, ?metadata) =
    member val PlatformName: string option = platformName
    member val PlatformUsername: string option = platformUsername
    member val Metadata: Map<string, string> option = metadata
    
    static member Encoder(v: UpdateCurrentUserApplicationRoleConnectionPayload) =
        Encode.object ([]
            |> Encode.optional "platform_name" Encode.string v.PlatformName
            |> Encode.optional "platform_username" Encode.string v.PlatformUsername
            |> Encode.optional "metadata" (Encode.mapv Encode.string) v.Metadata
        )

    interface IPayload with
        member this.ToHttpContent() =
            StringContent.createJson UpdateCurrentUserApplicationRoleConnectionPayload.Encoder this

// ----- Resources: Voice -----

// ----- Resources: Webhook -----

// ----- Topics: OAuth2 -----
