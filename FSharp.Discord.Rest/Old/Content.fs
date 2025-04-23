namespace FSharp.Discord.Rest.Old

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization

// ----- Interaction -----

type CreateInteractionResponsePayload<'a> (
    payload: InteractionResponse, // TODO: Create DU for interaction responses to ensure valid type/data
    ?files:  FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            match files with
            | None -> HttpContent.fromObjectAsJson payload
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

type EditOriginalInteractionResponsePayload (
    ?content:          string,
    ?embeds:           Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components:       Component list,
    ?attachments:      PartialAttachment list,
    ?poll:             Poll,
    ?files:            FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                optional "content" content
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "poll" poll
            }

            match files with
            | None -> HttpContent.fromObjectAsJson payload
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

type CreateFollowUpMessagePayload (
    ?content:          string,
    ?tts:              bool,
    ?embeds:           Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components:       Component list,
    ?attachments:      PartialAttachment list,
    ?flags:            MessageFlag list, // Only supports EPHEMERAL
    ?poll:             Poll,
    ?files:            FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                optional "content" content
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "flags" (flags |> Option.map (List.map int >> List.distinct >> List.sum))
                optional "poll" poll                
            }

            match files with
            | None -> HttpContent.fromObjectAsJson payload
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

type EditFollowUpMessagePayload (
    ?content:          string option,
    ?embeds:           Embed list option,
    ?allowed_mentions: AllowedMentions option,
    ?components:       Component list option,
    ?attachments:      PartialAttachment list option,
    ?poll:             Poll option,
    ?files:            FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                optional "content" content
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "poll" poll                
            }
            
            match files with
            | None -> HttpContent.fromObjectAsJson payload
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }
            
// ----- Application Command -----

type LocalizedApplicationCommandExtraFields = {
    NameLocalized: string option
    DescriptionLocalized: string option
}

type LocalizedApplicationCommand = {
    Command: ApplicationCommand
    ExtraFields: LocalizedApplicationCommandExtraFields
}

// TODO: Implement serialization for localized application command

type CreateGlobalApplicationCommandPayload (
    name:                        string,
    ?name_localizations:         (string * string) seq option,
    ?description:                string,
    ?description_localizations:  (string * string) seq option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?integration_types:          ApplicationIntegrationType list,
    ?contexts:                   InteractionContextType list,
    ?``type``:                   ApplicationCommandType,
    ?nsfw:                       bool
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "name_localizations" (name_localizations |> Option.map (Option.map dict))
                optional "description" description
                optional "description_localizations" (description_localizations |> Option.map (Option.map dict))
                optional "options" options
                optional "default_member_permissions" default_member_permissions
                optional "integration_types" integration_types
                optional "contexts" contexts
                optional "type" ``type``
                optional "nsfw" nsfw
            }
            |> Payload.toJsonContent
            :> HttpContent

type EditGlobalApplicationCommandPayload (
    ?name:                       string,
    ?name_localizations:         (string * string) seq option,
    ?description:                string,
    ?description_localizations:  (string * string) seq option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?integration_types:          ApplicationIntegrationType list,
    ?contexts:                   InteractionContextType list,
    ?nsfw:                       bool
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "name_localizations" (name_localizations |> Option.map (Option.map dict))
                optional "description" description
                optional "description_localizations" (description_localizations |> Option.map (Option.map dict))
                optional "options" options
                optional "default_member_permissions" default_member_permissions
                optional "integration_types" integration_types
                optional "contexts" contexts
                optional "nsfw" nsfw
            }
            |> Payload.toJsonContent
            :> HttpContent

type BulkOverwriteApplicationCommand = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: (string * string) seq option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: (string * string) seq option
    [<JsonPropertyName "options">] Options: ApplicationCommandOption list option
    [<JsonPropertyName "default_member_permissions">] DefaultMemberPermissions: string option
    [<JsonPropertyName "integration_types">] IntegrationTypes: ApplicationIntegrationType list option
    [<JsonPropertyName "contexts">] Contexts: InteractionContextType list option
    [<JsonPropertyName "type">] Type: ApplicationCommandType option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
}

type BulkOverwriteGlobalApplicationCommandsPayload (
    commands: BulkOverwriteApplicationCommand list
) =
    interface IPayload with
        member _.Content = HttpContent.fromObjectAsJson commands

type CreateGuildApplicationCommandPayload (
    name:                        string,
    ?name_localizations:         (string * string) seq option,
    ?description:                string,
    ?description_localizations:  (string * string) seq option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?``type``:                   ApplicationCommandType,
    ?nsfw:                       bool
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "name_localizations" (name_localizations |> Option.map (Option.map dict))
                optional "description" description
                optional "description_localizations" (description_localizations |> Option.map (Option.map dict))
                optional "options" options
                optional "default_member_permissions" default_member_permissions
                optional "type" ``type``
                optional "nsfw" nsfw
            }
            |> Payload.toJsonContent
            :> HttpContent

type EditGuildApplicationCommandPayload (
    ?name:                       string,
    ?name_localizations:         (string * string) seq option,
    ?description:                string,
    ?description_localizations:  (string * string) seq option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?nsfw:                       bool
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "name_localizations" (name_localizations |> Option.map (Option.map dict))
                optional "description" description
                optional "description_localizations" (description_localizations |> Option.map (Option.map dict))
                optional "options" options
                optional "default_member_permissions" default_member_permissions
                optional "nsfw" nsfw
            }
            |> Payload.toJsonContent
            :> HttpContent

type BulkOverwriteGuildApplicationCommands (
    commands: BulkOverwriteApplicationCommand list
) =
    interface IPayload with
        member _.Content = HttpContent.fromObjectAsJson commands

type EditApplicationCommandPermissions (
    permissions: ApplicationCommandPermission list
) =
    interface IPayload with
        member _.Content =
            payload {
                required "permissions" permissions
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Application -----

type EditCurrentApplicationPayload (
    ?custom_install_url:               string,
    ?description:                      string,
    ?role_connection_verification_url: string,
    ?install_params:                   InstallParams,
    ?integration_types_config:         (ApplicationIntegrationType * ApplicationIntegrationTypeConfiguration) seq,
    ?flags:                            ApplicationFlag list,
    ?icon:                             string option,
    ?cover_image:                      string option,
    ?interactions_endpoint_url:        string,
    ?tags:                             string list,
    ?event_webhooks_url:               string,
    ?event_webhooks_status:            WebhookEventStatus,
    ?event_webhooks_types:             WebhookEventType list
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "custom_install_url" custom_install_url
                optional "description" description
                optional "role_connection_verification_url" role_connection_verification_url
                optional "install_params" install_params
                optional "integration_types_config" (integration_types_config |> Option.map dict)
                optional "flags" (flags |> Option.map (List.map int >> List.distinct >> List.sum))
                optional "icon" icon
                optional "cover_image" cover_image
                optional "interactions_endpoint_url" interactions_endpoint_url
                optional "tags" tags
                optional "event_webhooks_url" event_webhooks_url
                optional "event_webhooks_status" event_webhooks_status
                optional "event_webhooks_types" event_webhooks_types
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Auto Moderation -----

type CreateAutoModerationRulePayload (
    name:              string,
    event_type:        AutoModerationEventType,
    trigger_type:      AutoModerationTriggerType,
    actions:           AutoModerationAction list,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?enabled:          bool,
    ?exempt_roles:     string list,
    ?exempt_channels:  string list
) =
    interface IPayload with
        override _.Content =
            payload {
                required "name" name
                required "event_type" event_type
                required "trigger_type" trigger_type
                optional "trigger_metadata" trigger_metadata
                required "actions" actions
                optional "enabled" enabled
                optional "exempt_roles" exempt_roles
                optional "exempt_channels" exempt_channels
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyAutoModerationRulePayload (
    ?name:             string,
    ?event_type:       AutoModerationEventType,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?actions:          AutoModerationAction list,
    ?enabled:          bool,
    ?exempt_roles:     string list,
    ?exempt_channels:  string list
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "event_type" event_type
                optional "trigger_metadata" trigger_metadata
                optional "actions" actions
                optional "enabled" enabled
                optional "exempt_roles" exempt_roles
                optional "exempt_channels" exempt_channels
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Channel -----

type ModifyGroupDmChannelPayload(
    ?name: string,
    ?icon: string
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "icon" icon
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildChannelPayload(
    ?name:                               string,
    ?``type``:                           ChannelType,
    ?position:                           int option,
    ?topic:                              string option,
    ?nsfw:                               bool option,
    ?rate_limit_per_user:                int option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?permission_overwrites:              PartialPermissionOverwrite list option,
    ?parent_id:                          string option,
    ?rtc_region:                         string option,
    ?video_quality_mode:                 VideoQualityMode option,
    ?default_auto_archive_duration:      int option,
    ?flags:                              ChannelFlag list,
    ?available_tags:                     ForumTag list,
    ?default_reaction_emoji:             DefaultReaction option,
    ?default_thread_rate_limit_per_user: int,
    ?default_sort_order:                 ChannelSortOrder option,
    ?default_forum_layout:               ForumLayout
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "type" ``type``
                optional "position" position
                optional "topic" topic
                optional "nsfw" nsfw
                optional "rate_limit_per_user" rate_limit_per_user
                optional "bitrate" bitrate
                optional "user_limit" user_limit
                optional "permission_overwrites" permission_overwrites
                optional "parent_id" parent_id
                optional "rtc_region" rtc_region
                optional "video_quality_mode" video_quality_mode
                optional "default_auto_archive_duration" default_auto_archive_duration
                optional "flags" (flags |> Option.map (List.map int >> List.distinct >> List.sum))
                optional "available_tags" available_tags
                optional "default_reaction_emoji" default_reaction_emoji
                optional "default_thread_rate_limit_per_user" default_thread_rate_limit_per_user
                optional "default_sort_order" default_sort_order
                optional "default_forum_layout" default_forum_layout
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyThreadChannelPayload (
    ?name:                  string,
    ?archived:              bool,
    ?auto_archive_duration: int,
    ?locked:                bool,
    ?invitable:             bool,
    ?rate_limit_per_user:   int option,
    ?flags:                 ChannelFlag list,
    ?applied_tags:          string list
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "archived" archived
                optional "auto_archive_duration" auto_archive_duration
                optional "locked" locked
                optional "invitable" invitable
                optional "rate_limit_per_user" rate_limit_per_user
                optional "flags" (flags |> Option.map (List.map int >> List.distinct >> List.sum))
                optional "applied_tags" applied_tags
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyChannelPayload =
    | GroupDm of ModifyGroupDmChannelPayload
    | Guild   of ModifyGuildChannelPayload
    | Thread  of ModifyThreadChannelPayload

type EditChannelPermissionsType =
    | ROLE   = 0
    | MEMBER = 1

type EditChannelPermissionsPayload (
    ``type``: EditChannelPermissionsType,
    ?allow:   string option,
    ?deny:    string option
) =
    interface IPayload with
        member _.Content =
            payload {
                required "type" ``type``
                optional "allow" allow
                optional "deny" deny
            }
            |> Payload.toJsonContent
            :> HttpContent

type CreateChannelInvitePayload (
    target_type:            InviteTargetType,
    ?max_age:               int,
    ?max_uses:              int,
    ?temporary:             bool,
    ?unique:                bool,
    ?target_user_id:        string,
    ?target_application_id: string
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "max_age" max_age
                optional "max_uses" max_uses
                optional "temporary" temporary
                optional "unique" unique
                required "target_type" target_type
                optional "target_user_id" target_user_id
                optional "target_application_id" target_application_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type FollowAnnouncementChannelPayload (
    webhook_channel_id: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "webhook_channel_id" webhook_channel_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type GroupDmAddRecipientPayload (
    access_token: string,
    ?nick: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "access_token" access_token
                optional "nick" nick
            }
            |> Payload.toJsonContent
            :> HttpContent

type StartThreadFromMessagePayload (
    name:                   string,
    ?auto_archive_duration: int,
    ?rate_limit_per_user:   int option
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "auto_archive_duration" auto_archive_duration
                optional "rate_limit_per_user" rate_limit_per_user
            }
            |> Payload.toJsonContent
            :> HttpContent

type ThreadType =
    | ANNOUNCEMENT_THREAD = 10
    | PUBLIC_THREAD = 11
    | PRIVATE_THREAD = 12

type StartThreadWithoutMessagePayload (
    name:                   string,
    ?auto_archive_duration: int,
    ?``type``:              ThreadType,
    ?invitable:             bool,
    ?rate_limit_per_user:   int option
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "auto_archive_duration" auto_archive_duration
                optional "type" ``type``
                optional "invitable" invitable
                optional "rate_limit_per_user" rate_limit_per_user
            }
            |> Payload.toJsonContent
            :> HttpContent

type ForumAndMediaThreadMessageParams = {
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "sticker_ids">] StickerIds: string list option
    [<JsonPropertyName "attachments">] Attachments: PartialAttachment list option
    [<JsonPropertyName "flags">] Flags: int option
}

type StartThreadInForumOrMediaChannelPayload (
    name:                   string,
    message:                ForumAndMediaThreadMessageParams,
    ?auto_archive_duration: int,
    ?applied_tags:          string list,
    ?files:                 FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                required "name" name
                optional "auto_archive_duration" auto_archive_duration
                required "message" message
                optional "applied_tags" applied_tags
            }

            match files with
            | None -> payload |> Payload.toJsonContent :> HttpContent
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

type StartThreadInForumOrMediaChannelOkResponseExtraFields = {
    [<JsonPropertyName "message">] Message: Message option
}

[<JsonConverter(typeof<StartThreadInForumOrMediaChannelOkResponseConverter>)>]
type StartThreadInForumOrMediaChannelOkResponse = {
    Channel: Channel
    ExtraFields: StartThreadInForumOrMediaChannelOkResponseExtraFields
}

and StartThreadInForumOrMediaChannelOkResponseConverter () =
    inherit JsonConverter<StartThreadInForumOrMediaChannelOkResponse> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Channel = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let channel = Json.serializeF value.Channel
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge channel extraFields)

type ListPublicArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

type ListPrivateArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

type ListJoinedPrivateArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

// ----- Emoji -----

type CreateGuildEmojiPayload (
    name:  string,
    image: string,
    roles: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                required "image" image
                required "roles" roles
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildEmojiPayload (
    ?name:  string,
    ?roles: string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "roles" roles
            }
            |> Payload.toJsonContent
            :> HttpContent

type ListApplicationEmojisOkResponse = {
    [<JsonPropertyName "items">] Items: Emoji list
}

type CreateApplicationEmojiPayload (
    name:  string,
    image: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                required "image" image
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyApplicationEmojiPayload (
    name: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Entitlement -----

// https://discord.com/developers/docs/resources/entitlement#create-test-entitlement-json-params
type EntitlementOwnerType =
    | GUILD_SUBSCRIPTION = 1
    | USER_SUBSCRIPTION  = 2

type CreateTestEntitlementPayload (
    sku_id:     string,
    owner_id:   string,
    owner_type: EntitlementOwnerType
) =
    interface IPayload with
        member _.Content =
            payload {
                required "sku_id" sku_id
                required "owner_id" owner_id
                required "owner_type" owner_type
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Guild -----

type CreateGuildPayload (
    name:                           string,
    ?icon:                          string,
    ?verification_level:            VerificationLevel,
    ?default_message_notifications: MessageNotificationLevel,
    ?explicit_content_filter:       ExplicitContentFilterLevel,
    ?roles:                         Role list,
    ?channels:                      PartialChannel list,
    ?afk_channel_id:                string,
    ?afk_timeout:                   int,
    ?system_channel_id:             string,
    ?system_channel_flags:          SystemChannelFlag list
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "icon" icon
                optional "verification_level" verification_level
                optional "default_message_notifications" default_message_notifications
                optional "explicit_content_filter" explicit_content_filter
                optional "roles" roles
                optional "channels" channels
                optional "afk_channel_id" afk_channel_id
                optional "afk_timeout" afk_timeout
                optional "system_channel_id" system_channel_id
                optional "system_channel_flags" (system_channel_flags |> Option.map (List.map int >> List.distinct >> List.sum))
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildPayload (
    ?name:                          string,
    ?verification_level:            VerificationLevel option,
    ?default_message_notifications: MessageNotificationLevel option,
    ?explicit_content_filter:       ExplicitContentFilterLevel option,
    ?afk_channel_id:                string option,
    ?afk_timeout:                   int,
    ?icon:                          string option,
    ?owner_id:                      string,
    ?splash:                        string option,
    ?discovery_splash:              string option,
    ?banner:                        string option,
    ?system_channel_id:             string option,
    ?system_channel_flags:          SystemChannelFlag list,
    ?rules_channel_id:              string option,
    ?public_updates_channel_id:     string option,
    ?preferred_locale:              string option,
    ?features:                      GuildFeature list,
    ?description:                   string option,
    ?premium_progress_bar_enabled:  bool,
    ?safety_alerts_channel_id:      string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "verification_level" verification_level
                optional "default_message_notifications" default_message_notifications
                optional "explicit_content_filter" explicit_content_filter
                optional "afk_channel_id" afk_channel_id
                optional "afk_timeout" afk_timeout
                optional "icon" icon
                optional "owner_id" owner_id
                optional "splash" splash
                optional "discovery_splash" discovery_splash
                optional "banner" banner
                optional "system_channel_id" system_channel_id
                optional "system_channel_flags" (system_channel_flags |> Option.map (List.map int >> List.distinct >> List.sum))
                optional "rules_channel_id" rules_channel_id
                optional "public_updates_channel_id" public_updates_channel_id
                optional "preferred_locale" preferred_locale
                optional "features" (Option.map (List.map GuildFeature.toString) features)
                optional "description" description
                optional "premium_progress_bar_enabled" premium_progress_bar_enabled
                optional "safety_alerts_channel_id" safety_alerts_channel_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type CreateGuildChannelPayload (
    name:                                string,
    ?``type``:                           ChannelType option,
    ?topic:                              string option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?rate_limit_per_user:                int option,
    ?position:                           int option,
    ?permission_overwrites:              PartialPermissionOverwrite list option,
    ?parent_id:                          string option,
    ?nsfw:                               bool option,
    ?rtc_region:                         string option,
    ?video_quality_mode:                 VideoQualityMode option,
    ?default_auto_archive_duration:      int option,
    ?default_reaction_emoji:             DefaultReaction option,
    ?available_tags:                     ForumTag list option,
    ?default_sort_order:                 ChannelSortOrder option,
    ?default_forum_layout:               ForumLayout option,
    ?default_thread_rate_limit_per_user: int option
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "type" ``type``
                optional "topic" topic
                optional "bitrate" bitrate
                optional "user_limit" user_limit
                optional "rate_limit_per_user" rate_limit_per_user
                optional "position" position
                optional "permission_overwrites" permission_overwrites
                optional "parent_id" parent_id
                optional "nsfw" nsfw
                optional "rtc_region" rtc_region
                optional "video_quality_mode" video_quality_mode
                optional "default_auto_archive_duration" default_auto_archive_duration
                optional "default_reaction_emoji" default_reaction_emoji
                optional "available_tags" available_tags
                optional "default_sort_order" default_sort_order
                optional "default_forum_layout" default_forum_layout
                optional "default_thread_rate_limit_per_user" default_thread_rate_limit_per_user
            }
            |> Payload.toJsonContent
            :> HttpContent


type ModifyGuildChannelPosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "lock_permissions">] LockPermissions: bool option
    [<JsonPropertyName "parent_id">] ParentId: string option
}

type ModifyGuildChannelPositionsPayload (
    positions: ModifyGuildChannelPosition list
) =
    interface IPayload with
        member _.Content = HttpContent.fromObjectAsJson positions

type ListActiveGuildThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: GuildMember list
}

type AddGuildMemberPayload (
    access_token: string,
    ?nick:        string,
    ?roles:       string list,
    ?mute:        bool,
    ?deaf:        bool
) =
    interface IPayload with
        member _.Content =
            payload {
                required "access_token" access_token
                optional "nick" nick
                optional "roles" roles
                optional "mute" mute
                optional "deaf" deaf
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildMemberPayload (
    ?nick:                         string option,
    ?roles:                        string list option,
    ?mute:                         bool option,
    ?deaf:                         bool option,
    ?channel_id:                   string option,
    ?communication_disabled_until: DateTime option,
    ?flags:                        GuildMemberFlag list option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "nick" nick
                optional "roles" roles
                optional "mute" mute
                optional "deaf" deaf
                optional "channel_id" channel_id
                optional "communication_disabled_until" communication_disabled_until
                optional "flags" (flags |> Option.map (Option.map (List.map int >> List.distinct >> List.sum)))
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyCurrentMemberPayload (
    ?nick: string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "nick" nick
            }
            |> Payload.toJsonContent
            :> HttpContent

type CreateGuildBanPayload (
    ?delete_message_days:    int,
    ?delete_message_seconds: int
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "delete_message_days" delete_message_days
                optional "delete_message_seconds" delete_message_seconds
            }
            |> Payload.toJsonContent
            :> HttpContent

type BulkGuildBanPayload (
    user_ids:                string list,
    ?delete_message_seconds: int
) =
    interface IPayload with
        member _.Content =
            payload {
                required "user_ids" user_ids
                optional "delete_message_seconds" delete_message_seconds
            }
            |> Payload.toJsonContent
            :> HttpContent

type BulkGuildBanOkResponse = {
    [<JsonPropertyName "banned_users">] BannedUsers: string list
    [<JsonPropertyName "failed_users">] FailedUsers: string list
}
    
type CreateGuildRolePayload (
    ?name:          string,
    ?permissions:   string,
    ?color:         int,
    ?hoist:         bool,
    ?icon:          string option,
    ?unicode_emoji: string option,
    ?mentionable:   bool
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "permissions" permissions
                optional "color" color
                optional "hoist" hoist
                optional "icon" icon
                optional "unicode_emoji" unicode_emoji
                optional "mentionable" mentionable
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildRolePosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
}

type ModifyGuildRolePositionsPayload(
    positions: ModifyGuildRolePosition list
) =
    interface IPayload with
        member _.Content = HttpContent.fromObjectAsJson positions

type ModifyGuildRolePayload (
    ?name:          string option,
    ?permissions:   string option,
    ?color:         int option,
    ?hoist:         bool option,
    ?icon:          string option,
    ?unicode_emoji: string option,
    ?mentionable:   bool option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "permissions" permissions
                optional "color" color
                optional "hoist" hoist
                optional "icon" icon
                optional "unicode_emoji" unicode_emoji
                optional "mentionable" mentionable
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildMfaLevelPayload (
    level: MfaLevel
) =
    interface IPayload with
        member _.Content =
            payload {
                required "level" level
            }
            |> Payload.toJsonContent
            :> HttpContent

type GetGuildPruneCountOkResponse = {
    [<JsonPropertyName "pruned">] Pruned: int
}

type BeginGuildPrunePayload (
    ?days: int,
    ?compute_prune_count: bool,
    ?include_roles: string list,
    ?reason: string
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "days" days
                optional "compute_prune_count" compute_prune_count
                optional "include_roles" include_roles
                optional "reason" reason
            }
            |> Payload.toJsonContent
            :> HttpContent

type BeginGuildPruneOkResponse = {
    [<JsonPropertyName "pruned">] Pruned: int option
}

type ModifyGuildWidgetPayload (
    ?enabled:    bool,
    ?channel_id: string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "enabled" enabled
                optional "channel_id" channel_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type GetGuildVanityUrlOkResponse = {
    [<JsonPropertyName "code">] Code: string option
    [<JsonPropertyName "uses">] Uses: int
}

type ModifyGuildWelcomeScreenPayload (
    ?enabled:          bool option,
    ?welcome_channels: WelcomeScreenChannel list option,
    ?description:      string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "enabled" enabled
                optional "welcome_channels" welcome_channels
                optional "description" description
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildOnboardingPayload (
    prompts:             GuildOnboardingPrompt list,
    default_channel_ids: string list,
    enabled:             bool,
    mode:                OnboardingMode
) =
    interface IPayload with
        member _.Content =
            payload {
                required "prompts" prompts
                required "default_channel_ids" default_channel_ids
                required "enabled" enabled
                required "mode" mode
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildIncidentActionsPayload (
    invites_disabled_until: DateTime option, // max 24 hours
    dms_disabled_until:     DateTime option  // max 24 hours
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "invites_disabled_until" invites_disabled_until
                optional "dms_disabled_until" dms_disabled_until
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Guild Scheduled Event -----

type CreateGuildScheduledEventPayload (
    name:                 string,
    privacy_level:        PrivacyLevel,
    scheduled_start_time: DateTime,
    entity_type:          ScheduledEntityType,
    ?channel_id:          string,
    ?entity_metadata:     EntityMetadata,
    ?scheduled_end_time:  DateTime,
    ?description:         string,
    ?image:               string,
    ?recurrence_rule:     RecurrenceRule
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "channel_id" channel_id
                optional "entity_metadata" entity_metadata
                required "name" name
                required "privacy_level" privacy_level
                required "scheduled_start_time" scheduled_start_time
                optional "scheduled_end_time" scheduled_end_time
                optional "description" description
                required "entity_type" entity_type
                optional "image" image
                optional "recurrence_rule" recurrence_rule
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildScheduledEventPayload (
    ?channel_id:           string option,
    ?entity_metadata:      EntityMetadata option,
    ?name:                 string,
    ?privacy_level:        PrivacyLevel,
    ?scheduled_start_time: DateTime,
    ?scheduled_end_time:   DateTime,
    ?description:          string option,
    ?entity_type:          ScheduledEntityType,
    ?image:                string,
    ?recurrence_rule:      RecurrenceRule option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "channel_id" channel_id
                optional "entity_metadata" entity_metadata
                optional "name" name
                optional "privacy_level" privacy_level
                optional "scheduled_start_time" scheduled_start_time
                optional "scheduled_end_time" scheduled_end_time
                optional "description" description
                optional "entity_type" entity_type
                optional "image" image
                optional "recurrence_rule" recurrence_rule
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Guild Template -----

type CreateGuildFromTemplatePayload (
    name:  string,
    ?icon: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "icon" icon
            }
            |> Payload.toJsonContent
            :> HttpContent

type CreateGuildTemplatePayload (
    name:         string,
    ?description: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "description" description
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildTemplatePayload (
    ?name:        string,
    ?description: string
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "description" description
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Message -----

type CreateMessagePayload (
    ?content:           string,
    ?nonce:             MessageNonce,
    ?tts:               bool,
    ?embeds:            Embed list,
    ?allow_mentions:    AllowedMentions,
    ?message_reference: MessageReference,
    ?components:        Component list,
    ?sticker_ids:       string list,
    ?attachments:       PartialAttachment list,
    ?flags:             MessageFlag list,
    ?enforce_nonce:     bool,
    ?poll:              Poll,
    ?files:             FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                optional "content" content
                optional "nonce" (match nonce with | Some (MessageNonce.INT n) -> Some n | _ -> None)
                optional "nonce" (match nonce with | Some (MessageNonce.STRING s) -> Some s | _ -> None)
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allow_mentions
                optional "message_reference" message_reference
                optional "components" components
                optional "sticker_ids" sticker_ids
                optional "attachments" attachments
                optional "flags" (flags |> Option.map (List.map int >> List.distinct >> List.sum))
                optional "enforce_nonce" enforce_nonce
                optional "poll" poll
            }

            match files with
            | None -> payload |> Payload.toJsonContent :> HttpContent
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

type EditMessagePayload (
    ?content:        string option,
    ?embeds:         Embed list option,
    ?flags:          MessageFlag list option,
    ?allow_mentions: AllowedMentions option,
    ?components:     Component list option,
    ?attachments:    PartialAttachment list option,
    ?files:          FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                optional "content" content
                optional "embeds" embeds
                optional "flags" (flags |> Option.map (Option.map (List.map int >> List.distinct >> List.sum)))
                optional "allowed_mentions" allow_mentions
                optional "components" components
                optional "attachments" attachments
            }

            match files with
            | None -> payload |> Payload.toJsonContent :> HttpContent
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

type BulkDeleteMessagesPayload (
    messages: string list
) =
    interface IPayload with
        member _.Content =
            payload {
                required "messages" messages
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Poll -----

type GetAnswerVotersOkResponse = {
    [<JsonPropertyName "users">] Users: User list
}

// ----- Role Connection -----

type UpdateApplicationRoleConnectionMetadataRecordsPayload (
    metadata: ApplicationRoleConnectionMetadata list
) =
    interface IPayload with
        member _.Content = HttpContent.fromObjectAsJson metadata

// ----- Soundboard -----

type SendSoundboardSoundPayload (
    sound_id:         string,
    ?source_guild_id: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "sound_id" sound_id
                optional "source_guild_id" source_guild_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type CreateGuildSoundboardSoundPayload (
    name: string,
    sound: string,
    ?volume: double option,
    ?emoji_id: string option,
    ?emoji_name: string option
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                required "sound" sound
                optional "volume" volume
                optional "emoji_id" emoji_id
                optional "emoji_name" emoji_name
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyGuildSoundboardSoundPayload (
    ?name: string,
    ?volume: double option,
    ?emoji_id: string option,
    ?emoji_name: string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "volume" volume
                optional "emoji_id" emoji_id
                optional "emoji_name" emoji_name
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Stage Instance -----

type CreateStageInstancePayload (
    channel_id:                string,
    topic:                     string,
    ?privacy_level:            PrivacyLevel,
    ?send_start_notification:  bool,
    ?guild_scheduled_event_id: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "channel_id" channel_id
                required "topic" topic
                optional "privacy_level" privacy_level
                optional "send_start_notification" send_start_notification
                optional "guild_scheduled_event_id" guild_scheduled_event_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyStageInstancePayload (
    ?topic:         string,
    ?privacy_level: PrivacyLevel
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "topic" topic
                optional "privacy_level" privacy_level
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Sticker -----

type ListStickerPacksOkResponse = {
    [<JsonPropertyName "sticker_packs">] StickerPacks: StickerPack list
}

type CreateGuildStickerPayload (
    name: string,
    description: string,
    tags: string,
    stickerFile: FSharp.Discord.Rest.Old.File
) =
    interface IPayload with
        member _.Content =
            multipart {
                text "name" name
                text "description" description
                text "tags" tags
                file "file" stickerFile
            }

type ModifyGuildStickerPayload (
    name:        string,
    description: string option,
    tags:        string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                required "description" description
                required "tags" tags
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- User -----

type ModifyCurrentUserPayload (
    ?username: string,
    ?avatar:   string option,
    ?banner:   string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "username" username
                optional "avatar" avatar
                optional "banner" banner
            }
            |> Payload.toJsonContent
            :> HttpContent

type CreateDmPayload (
    recipient_id: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "recipient_id" recipient_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type CreateGroupDmPayload (
    access_tokens: string list,
    nicks:         (string * string) seq
) =
    interface IPayload with
        member _.Content =
            payload {
                required "access_tokens" access_tokens
                required "nicks" (nicks |> dict)
            }
            |> Payload.toJsonContent
            :> HttpContent

    // TODO: Test if these are optional (likely just nicks is, but cant use openapi spec to check because same endpoint
    //       used for createDM which uses a different kind of payload and it doesnt discriminate them)

type UpdateCurrentUserApplicationRoleConnectionPayload (
    ?platform_name:     string,
    ?platform_username: string,
    ?metadata:          (string * string) seq // value is the "stringified value"
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "platform_name" platform_name
                optional "platform_username" platform_username
                optional "metadata" (metadata |> Option.map dict)
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Voice -----

type ModifyCurrentUserVoiceStatePayload (
    ?channel_id:                 string,
    ?suppress:                   bool,
    ?request_to_speak_timestamp: DateTime option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "channel_id" channel_id
                optional "suppress" suppress
                optional "request_to_speak_timestamp" request_to_speak_timestamp
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyUserVoiceStatePayload (
    channel_id: string,
    ?suppress:  bool
) =
    interface IPayload with
        member _.Content =
            payload {
                required "channel_id" channel_id
                optional "suppress" suppress
            }
            |> Payload.toJsonContent
            :> HttpContent

// ----- Webhook -----

type CreateWebhookPayload (
    name:    string,
    ?avatar: string option
) =
    interface IPayload with
        member _.Content =
            payload {
                required "name" name
                optional "avatar" avatar
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyWebhookPayload (
    ?name:       string,
    ?avatar:     string option,
    ?channel_id: string
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "avatar" avatar
                optional "channel_id" channel_id
            }
            |> Payload.toJsonContent
            :> HttpContent

type ModifyWebhookWithTokenPayload (
    ?name:   string,
    ?avatar: string option
) =
    interface IPayload with
        member _.Content =
            payload {
                optional "name" name
                optional "avatar" avatar
            }
            |> Payload.toJsonContent
            :> HttpContent

type ExecuteWebhookPayload (
    ?content:          string,
    ?username:         string,
    ?avatar_url:       string,
    ?tts:              bool,
    ?embeds:           Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components:       Component list,
    ?attachments:      PartialAttachment list,
    ?flags:            MessageFlag list,
    ?thread_name:      string,
    ?applied_tags:     string list,
    ?poll:             Poll,
    ?files:            FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                optional "content" content
                optional "username" username
                optional "avatar_url" avatar_url
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "flags" (flags |> Option.map (List.map int >> List.distinct >> List.sum))
                optional "thread_name" thread_name
                optional "applied_tags" applied_tags
                optional "poll" poll                
            }

            match files with
            | None -> payload |> Payload.toJsonContent :> HttpContent
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

type EditWebhookMessagePayload (
    ?content:          string option,
    ?embeds:           Embed list option,
    ?allowed_mentions: AllowedMentions option,
    ?components:       Component list option,
    ?attachments:      PartialAttachment list option,
    ?poll:             Poll option,
    ?files:            FSharp.Discord.Rest.Old.File list
) =
    interface IPayload with
        member _.Content =
            let payload = payload {
                optional "content" content
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "poll" poll                
            }

            match files with
            | None -> payload |> Payload.toJsonContent :> HttpContent
            | Some f -> multipart {
                json "payload_json" payload
                files f
            }

// ----- Gateway -----

type GetGatewayOkResponse = {
    [<JsonPropertyName "url">] Url: string
}

type GetGatewayBotOkResponse = {
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "shards">] Shards: int
    [<JsonPropertyName "session_start_limit">] SessionStartLimit: SessionStartLimit
}

// ----- OAuth2 -----

// TODO: Add content regarding external auth (social sdk) here and wherever else relevant

type GetCurrentAuthorizationInformationOkResponse = {
    [<JsonPropertyName "application">] Application: PartialApplication
    [<JsonPropertyName "scopes">] Scopes: OAuthScope list
    [<JsonPropertyName "expires">] Expires: DateTime
    [<JsonPropertyName "user">] User: User option
}

type AuthorizationCodeGrantPayload (
    code:          string,
    redirect_uri:  string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "grant_type" "authorization_code"
                required "code" code
                required "redirect_uri" redirect_uri
            }
            |> Payload.toFormContent
            :> HttpContent
        
type AuthorizationCodeGrantResponse = {
    [<JsonPropertyName "access_token">] AccessToken: string
    [<JsonPropertyName "token_type">] TokenType: string
    [<JsonPropertyName "expires_in">] ExpiresIn: int
    [<JsonPropertyName "refresh_token">] RefreshToken: string
    [<JsonPropertyName "scope">] Scope: OAuthScope list
}

type RefreshTokenGrantPayload (
    refresh_token: string
) =
    interface IPayload with
        member _.Content =
            payload {
                required "grant_type" "refresh_token"
                required "refresh_token" refresh_token
            }
            |> Payload.toFormContent
            :> HttpContent

type RefreshTokenGrantResponse = {
    [<JsonPropertyName "access_token">] AccessToken: string
    [<JsonPropertyName "token_type">] TokenType: string
    [<JsonPropertyName "expires_in">] ExpiresIn: int
    [<JsonPropertyName "refresh_token">] RefreshToken: string
    [<JsonPropertyName "scope">] Scope: OAuthScope list
}

type RevokeTokenPayload (
    token:            string,
    ?token_type_hint: TokenTypeHint
) =
    interface IPayload with
        member _.Content =
            payload {
                required "token" token
                optional "token_type_hint" token_type_hint
            }
            |> Payload.toFormContent
            :> HttpContent
        
type ClientCredentialsGrantPayload (
    scope: OAuthScope list
) =
    interface IPayload with
        member _.Content =
            payload {
                required "grant_type" "client_credentials"
                required "scope" scope
            }
            |> Payload.toFormContent
            :> HttpContent

type ClientCredentialsGrantResponse = {
    [<JsonPropertyName "access_token">] AccessToken: string
    [<JsonPropertyName "token_type">] TokenType: string
    [<JsonPropertyName "expires_in">] ExpiresIn: int
    [<JsonPropertyName "scope">] Scope: OAuthScope list
}
