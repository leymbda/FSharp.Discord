module FSharp.Discord.Rest.Rest

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open System.Net.Http
open System.Threading.Tasks

// ----- Interaction -----

let createInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (withResponse: bool option)
    (content: CreateInteractionResponsePayload<'a>)
    (client: IBotClient) =
        req {
            post $"interactions/{interactionId}/{interactionToken}/callback"
            query "with_response" (Option.map string withResponse)
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asOptionalJson<InteractionCallbackResponse>
            
let getOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (client: IBotClient) =
        req {
            get $"webhooks/{interactionId}/{interactionToken}/messages/@original"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>
            
let editOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (content: EditOriginalInteractionResponsePayload)
    (client: IBotClient) =
        req {
            patch $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>
            
let deleteOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (client: IBotClient) =
        req {
            delete $"webhooks/{interactionId}/{interactionToken}/messages/@original"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty
            
let createFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (content: CreateFollowUpMessagePayload)
    (client: IBotClient) =
        req {
            post $"webhooks/{applicationId}/{interactionToken}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty
            
let getFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (client: IBotClient) =
        req {
            get $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>
            
let editFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (content: EditFollowUpMessagePayload)
    (client: IBotClient) =
        req {
            patch $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>
            
let deleteFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (client: IBotClient) =
        req {
            delete $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Application Command -----

let getGlobalApplicationCommandsWithAllLocalizations
    (applicationId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/commands"
            query "with_localizations" (string true)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand list>

let getGlobalApplicationCommands
    (applicationId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/commands"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<LocalizedApplicationCommand list>
            
let createGlobalApplicationCommand
    (applicationId: string)
    (content: CreateGlobalApplicationCommandPayload)
    (client: IBotClient) =
        req {
            post $"applications/{applicationId}/commands"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand>
            
let getGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/commands/{commandId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand>
            
let editGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (content: EditGlobalApplicationCommandPayload)
    (client: IBotClient) =
        req {
            patch $"applications/{applicationId}/commands/{commandId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand>
            
let deleteGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (client: IBotClient) =
        req {
            delete $"applications/{applicationId}/commands/{commandId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty
            
let bulkOverwriteGlobalApplicationCommands
    (applicationId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    (client: IBotClient) =
        req {
            put $"applications/{applicationId}/commands"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommandsWithAllLocalizations
    (applicationId: string)
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands"
            query "with_localizations" (string true)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<LocalizedApplicationCommand list>
            
let createGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (content: CreateGuildApplicationCommandPayload)
    (client: IBotClient) =
        req {
            post $"applications/{applicationId}/guilds/{guildId}/commands"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand>
            
let getGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand>
            
let editGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditGuildApplicationCommandPayload)
    (client: IBotClient) =
        req {
            patch $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand>
            
let deleteGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (client: IBotClient) =
        req {
            delete $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty
            
let bulkOverwriteGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    (client: IBotClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommandsPermissions
    (applicationId: string)
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/permissions"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildApplicationCommandPermissions list>
            
let getGuildApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildApplicationCommandPermissions>
            
let editApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditApplicationCommandPermissions)
    (client: IOAuthClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildApplicationCommandPermissions>

// ----- Application -----

let getCurrentApplication
    (client: IBotClient) =
        req {
            get "applications/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Application>

let editCurrentApplication
    (content: EditCurrentApplicationPayload)
    (client: IBotClient) =
        req {
            patch "applications/@me"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Application>

let getApplicationActivityInstance
    (applicationId: string)
    (instanceId: string)
    (client: IBotClient) =
        req {
            patch $"applications/{applicationId}/activity-instances/{instanceId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ActivityInstance>

// ----- Audit Log -----

let getGuildAuditLog
    (guildId: string)
    (userId: string option)
    (actionType: AuditLogEventType option)
    (before: string option)
    (after: string option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/audit-logs"
            query "user_id" userId
            query "action_type" (Option.map (int >> string) actionType)
            query "before" before
            query "after" after
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<AuditLog>

// ----- Auto Moderation -----

let listAutoModerationRulesForGuild
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/auto-moderation/rules"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<AutoModerationRule list>

let getAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<AutoModerationRule>

let createAutoModerationRule
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateAutoModerationRulePayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/auto-moderation/rules"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<AutoModerationRule>

let modifyAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (auditLogReason: string option)
    (content: ModifyAutoModerationRulePayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<AutoModerationRule>

let deleteAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Channel -----

let getChannel
    (channelId: string)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let modifyChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: ModifyChannelPayload)
    (client: IBotClient) =
        req {
            patch $"channels/{channelId}"
            payload (
                match content with
                | ModifyChannelPayload.GroupDm g -> g :> IPayload
                | ModifyChannelPayload.Guild g -> g :> IPayload
                | ModifyChannelPayload.Thread t -> t :> IPayload
            )
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let deleteChannel
    (channelId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let editChannelPermissions
    (channelId: string)
    (overwriteId: string)
    (auditLogReason: string option)
    (content: EditChannelPermissionsPayload)
    (client: IBotClient) =
        req {
            put $"channels/{channelId}/permissions/{overwriteId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getChannelInvites
    (channelId: string)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/invites"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<InviteWithMetadata list>

let createChannelInvite
    (channelId: string)
    (auditLogReason: string option)
    (content: CreateChannelInvitePayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/invites"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asOptionalJson<InviteWithMetadata>

let deleteChannelPermission
    (channelId: string)
    (overwriteId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/permissions/{overwriteId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let followAnnouncementChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: FollowAnnouncementChannelPayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/followers"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<FollowedChannel>

let triggerTypingIndicator
    (channelId: string)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/typing"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getPinnedMessages
    (channelId: string)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/pins"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message list>

let pinMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            put $"channels/{channelId}/pins/{messageId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let unpinMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/pins/{messageId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let groupDmAddRecipient
    (channelId: string)
    (userId: string)
    (content: GroupDmAddRecipientPayload)
    (client: IBotClient) =
        req {
            put $"channels/{channelId}/recipients/{userId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asOptionalJson<Channel>

let groupDmRemoveRecipient
    (channelId: string)
    (userId: string)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/recipients/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let startThreadFromMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (content: StartThreadFromMessagePayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/threads"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let startThreadWithoutMessage
    (channelId: string)
    (auditLogReason: string option)
    (content: StartThreadWithoutMessagePayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/threads"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let startThreadInForumOrMediaChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: StartThreadWithoutMessagePayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/threads"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<StartThreadInForumOrMediaChannelOkResponse>

let joinThread
    (channelId: string)
    (client: IBotClient) =
        req {
            put $"channels/{channelId}/thread-members/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let addThreadMember
    (channelId: string)
    (userId: string)
    (client: IBotClient) =
        req {
            put $"channels/{channelId}/thread-members/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let leaveThread
    (channelId: string)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/thread-members/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let removeThreadMember
    (channelId: string)
    (userId: string)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/thread-members/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getThreadMember
    (channelId: string)
    (userId: string)
    (withMember: bool option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/thread-members/{userId}"
            query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ThreadMember>

let listThreadMembers
    (channelId: string)
    (withMember: bool option)
    (after: string option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/thread-members"
            query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
            query "after" after
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ThreadMember list>

    // TODO: Test paginated response and implement for list thread members

let listPublicArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/threads/archived/public"
            query "before" (Option.map string before)
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ListPublicArchivedThreadsOkResponse>

let listPrivateArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/threads/archived/private"
            query "before" (Option.map string before)
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ListPrivateArchivedThreadsOkResponse>

let listJoinedPrivateArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/users/@me/threads/archived/private"
            query "before" (Option.map string before)
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ListJoinedPrivateArchivedThreadsOkResponse>

// ----- Emoji -----

let listGuildEmojis
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/emojis"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Emoji list>

let getGuildEmoji
    (guildId: string)
    (emojiId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/emojis/{emojiId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Emoji>

let createGuildEmoji
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildEmojiPayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/emojis"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Emoji>
        
let modifyGuildEmoji
    (guildId: string)
    (emojiId: string)
    (auditLogReason: string option)
    (content: ModifyGuildEmojiPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/emojis/{emojiId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Emoji>

let deleteGuildEmoji
    (guildId: string)
    (emojiId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/emojis/{emojiId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty
        
let listApplicationEmojis
    (applicationId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/emojis"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ListApplicationEmojisOkResponse>

let getApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/emojis/{emojiId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Emoji>

let createApplicationEmoji
    (applicationId: string)
    (content: CreateApplicationEmojiPayload)
    (client: IBotClient) =
        req {
            post $"applications/{applicationId}/emojis"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Emoji>
        
let modifyApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    (content: ModifyApplicationEmojiPayload)
    (client: IBotClient) =
        req {
            patch $"applications/{applicationId}/emojis/{emojiId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Emoji>

let deleteApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    (client: IBotClient) =
        req {
            delete $"applications/{applicationId}/emojis/{emojiId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Entitlement -----

let listEntitlements
    (applicationId: string)
    (userId: string option)
    (skuIds: string list option)
    (before: string option)
    (after: string option)
    (limit: int option)
    (guildId: string option)
    (excludeEnded: bool option)
    (excludeDeleted: bool option)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/entitlements"
            query "user_id" userId
            query "sku_ids" (Option.map (String.concat ",") skuIds)
            query "before" before
            query "after" after
            query "limit" (Option.map string limit)
            query "guild_id" guildId
            query "exclude_ended" (Option.map string excludeEnded)
            query "exclude_deleted" (Option.map string excludeDeleted)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Entitlement list>

let getEntitlement
    (applicationId: string)
    (entitlementId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/entitlements/{entitlementId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Entitlement>

let consumeEntitlement
    (applicationId: string)
    (entitlementId: string)
    (client: IBotClient) =
        req {
            post $"applications/{applicationId}/entitlements/{entitlementId}/consume"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let createTestEntitlement
    (applicationId: string)
    (content: CreateTestEntitlementPayload)
    (client: IBotClient) =
        req {
            post $"applications/{applicationId}/entitlements"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Entitlement>

let deleteTestEntitlement
    (applicationId: string)
    (entitlementId: string)
    (client: IBotClient) =
        req {
            delete $"applications/{applicationId}/entitlements/{entitlementId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Guild -----

let createGuild
    (content: CreateGuildPayload)
    (client: IBotClient) =
        req {
            post "guilds"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Guild>

let getGuild
    (guildId: string)
    (withCounts: bool option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}"
            query "with_counts" (Option.map string withCounts)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Guild>

let getGuildPreview
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/preview"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildPreview>

let modifyGuild
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Guild>

let deleteGuild
    (guildId: string)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getGuildChannels
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/channels"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel list>

let createGuildChannel
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildChannelPayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/channels"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let modifyGuildChannelPositions
    (guildId: string)
    (content: ModifyGuildChannelPositionsPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/channels"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let listActiveGuildThreads
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/threads/active"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ListActiveGuildThreadsOkResponse>

let getGuildMember
    (guildId: string)
    (userId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/members/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildMember>

let listGuildMembers
    (guildId: string)
    (limit: int option)
    (after: string option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/members"
            query "limit" (Option.map string limit)
            query "after" (Option.map string after)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildMember list>

let searchGuildMembers
    (guildId: string)
    (q: string) // query (cannot name same due to req ce)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/members/search"
            query "query" q
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildMember list>

let addGuildMember
    (guildId: string)
    (userId: string)
    (content: AddGuildMemberPayload)
    (client: IBotClient) =
        req {
            put $"guilds/{guildId}/members/{userId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204

let modifyGuildMember
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (content: ModifyGuildMemberPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/members/{userId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204

let modifyCurrentMember
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyCurrentMemberPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/members/@me"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204
        
let addGuildMemberRole
    (guildId: string)
    (userId: string)
    (roleId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            put $"guilds/{guildId}/members/{userId}/roles/{roleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let removeGuildMemberRole
    (guildId: string)
    (userId: string)
    (roleId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/members/{userId}/roles/{roleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let removeGuildMember
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/members/{userId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getGuildBans
    (guildId: string)
    (limit: int option)
    (before: string option)
    (after: string option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/bans"
            query "limit" (Option.map string limit)
            query "before" before
            query "after" after
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Ban list>

let getGuildBan
    (guildId: string)
    (userId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/bans/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Ban>

let createGuildBan
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (content: CreateGuildBanPayload)
    (client: IBotClient) =
        req {
            put $"guilds/{guildId}/bans/{userId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let removeGuildBan
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/bans/{userId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let bulkGuildBan
    (guildId: string)
    (auditLogReason: string option)
    (content: BulkGuildBanPayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/bulk-ban"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<BulkGuildBanOkResponse>

let getGuildRoles
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/roles"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Role list>

let getGuildRole
    (guildId: string)
    (roleId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/roles/{roleId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Role>

let createGuildRole
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildRolePayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/roles"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Role>

let modifyGuildRolePositions
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildRolePositionsPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/channels"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Role list>

let modifyGuildRole
    (guildId: string)
    (roleId: string)
    (auditLogReason: string option)
    (content: ModifyGuildRolePayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/roles/{roleId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Role>

let modifyGuildMfaLevel
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildMfaLevelPayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/mfa"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<MfaLevel>

let deleteGuildRole
    (guildId: string)
    (roleId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/roles/{roleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getGuildPruneCount
    (guildId: string)
    (days: int option)
    (includeRoles: string list option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/prune"
            query "days" (Option.map string days)
            query "include_roles" (Option.map (String.concat ",") includeRoles)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GetGuildPruneCountOkResponse>

let beginGuildPrune
    (guildId: string)
    (auditLogReason: string option)
    (content: BeginGuildPrunePayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/prune"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<BeginGuildPruneOkResponse>

let getGuildVoiceRegions
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/regions"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<VoiceRegion list>

let getGuildInvites
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/invites"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<InviteWithMetadata list>

let getGuildIntegrations
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/integrations"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Integration list>

let deleteGuildIntegration
    (guildId: string)
    (integrationId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/integrations/{integrationId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getGuildWidgetSettings
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/widget"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildWidgetSettings>

let modifyGuildWidget
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildWidgetPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/widget"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildWidgetSettings>

let getGuildWidget
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/widget.json"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildWidget>

let getGuildVanityUrl
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/vanity-url"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GetGuildVanityUrlOkResponse>

let getGuildWidgetImage
    (guildId: string)
    (style: GuildWidgetStyle option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/widget.png"
            query "style" (Option.map (GuildWidgetStyle.toString) style)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asRaw // TODO: Convert to png image format

let getGuildWelcomeScreen
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/welcome-screen"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<WelcomeScreen>

let modifyGuildWelcomeScreen
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildWelcomeScreenPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/welcome-screen"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<WelcomeScreen>

let getGuildOnboarding
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/onboarding"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildOnboarding>

let modifyGuildOnboarding
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildOnboardingPayload)
    (client: IBotClient) =
        req {
            put $"guilds/{guildId}/onboarding"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildOnboarding>

let modifyGuildIncidentActions
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildIncidentActionsPayload)
    (client: IBotClient) =
        req {
            put $"guilds/{guildId}/incident-actions"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<IncidentsData>

// ----- Guild Scheduled Event -----

let listGuildScheduledEvents
    (guildId: string)
    (withUserCount: bool option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/scheduled-events"
            query "with_user_count" (Option.map string withUserCount)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildScheduledEvent list>

let createGuildScheduledEvent
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildScheduledEventPayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/scheduled-events"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildScheduledEvent>

let getGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    (withUserCount: bool option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
            query "with_user_count" (Option.map string withUserCount)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildScheduledEvent>

let modifyGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    (auditLogReason: string option)
    (content: ModifyGuildScheduledEventPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildScheduledEvent>

let deleteGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    //(auditLogReason: string option) // TODO: Check if audit log is supposed to be available for this
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getGuildScheduledEventUsers
    (guildId: string)
    (guildScheduledEventId: string)
    (limit: int option)
    (withMember: bool option)
    (before: string option)
    (after: string option)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}/users"
            query "limit" (Option.map string limit)
            query "with_member" (Option.map string withMember)
            query "before" before
            query "after" after
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildScheduledEventUser list>

// ----- Guild Template -----

let getGuildTemplate
    (templateCode: string)
    (client: IBotClient) =
        req {
            get $"guilds/templates/{templateCode}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildTemplate>

let createGuildFromTemplate
    (templateCode: string)
    (content: CreateGuildFromTemplatePayload)
    (client: IBotClient) =
        req {
            post $"guilds/templates/{templateCode}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Guild>

let getGuildTemplates
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/templates"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildTemplate list>

let createGuildTemplate
    (guildId: string)
    (content: CreateGuildTemplatePayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/templates"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildTemplate>

let syncGuildTemplate
    (guildId: string)
    (templateCode: string)
    (client: IBotClient) =
        req {
            put $"guilds/{guildId}/templates/{templateCode}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildTemplate>

let modifyGuildTemplate
    (guildId: string)
    (templateCode: string)
    (content: ModifyGuildTemplatePayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/templates/{templateCode}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildTemplate>

let deleteGuildTemplate
    (guildId: string)
    (templateCode: string)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/templates/{templateCode}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildTemplate>

// ----- Invite -----

let getInvite
    (inviteCode: string)
    (withCounts: bool option)
    (withExpiration: bool option)
    (guildScheduledEventId: string option)
    (client: IBotClient) =
        req {
            get $"invites/{inviteCode}"
            query "with_counts" (Option.map string withCounts)
            query "with_expiration" (Option.map string withExpiration)
            query "guild_scheduled_event_id" guildScheduledEventId
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Invite>

let deleteInvite
    (inviteCode: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"invites/{inviteCode}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Invite>

// ----- Message -----

let getChannelMessages
    (channelId: string)
    (around: string option)
    (before: string option)
    (after: string option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/messages"
            query "around" around
            query "before" before
            query "after" after
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message list>

let getChannelMessage
    (channelId: string)
    (messageId: string)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/messages/{messageId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

let createMessage
    (channelId: string)
    (content: CreateMessagePayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/messages"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

let crosspostMessage
    (channelId: string)
    (messageId: string)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/crosspost"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

let createReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let deleteOwnReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let deleteUserReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (userId: string)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let getReactions
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (``type``: ReactionType option)
    (after: string option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
            query "type" (Option.map (int >> string) ``type``)
            query "after" after
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<User list>

let deleteAllReactions
    (channelId: string)
    (messageId: string)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let deleteAllReactionsForEmoji
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let editMessage
    (channelId: string)
    (messageId: string)
    (content: EditMessagePayload)
    (client: IBotClient) =
        req {
            patch $"channels/{channelId}/messages/{messageId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

let deleteMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let bulkDeleteMessages
    (channelId: string)
    (auditLogReason: string option)
    (content: BulkDeleteMessagesPayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/messages/bulk-delete"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Poll -----

let getAnswerVoters
    (channelId: string)
    (messageId: string)
    (answerId: string)
    (after: string option)
    (limit: int option)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/polls/{messageId}/answers/{answerId}"
            query "after" after
            query "limit" (Option.map string limit)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GetAnswerVotersOkResponse>

let endPoll
    (channelId: string)
    (messageId: string)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/polls/{messageId}/expire"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

// ----- Role Connection -----

let getApplicationRoleConnectionMetadataRecords
    (applicationId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/role-connections/metadata"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationRoleConnectionMetadata list>

let updateApplicationRoleConnectionMetadataRecords
    (applicationId: string)
    (content: UpdateApplicationRoleConnectionMetadataRecordsPayload)
    (client: IBotClient) =
        req {
            put $"applications/{applicationId}/role-connections/metadata"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationRoleConnectionMetadata list>

// ----- Sku -----

let listSkus
    (applicationId: string)
    (client: IBotClient) =
        req {
            get $"applications/{applicationId}/skus"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Sku list>

// ----- Soundboard -----

let sendSoundboardSound
    (channelId: string)
    (content: SendSoundboardSoundPayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/send-soundboard-sound"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let listDefaultSoundboardSounds
    (client: IBotClient) =
        req {
            get "soundboard-default-sounds"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<SoundboardSound list>

let listGuildSoundboardSounds
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/soundboard-sounds"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<SoundboardSound list>

let getGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/soundboard-sounds/{soundId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<SoundboardSound>

let createGuildSoundboardSound
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildSoundboardSoundPayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/soundboard-sounds"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<SoundboardSound>

let modifyGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (auditLogReason: string option)
    (content: ModifyGuildSoundboardSoundPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/soundboard-sounds/{soundId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<SoundboardSound>

let deleteGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/soundboard-sounds/{soundId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Stage Instance -----

let createStageInstance
    (auditLogReason: string option)
    (content: CreateStageInstancePayload)
    (client: IBotClient) =
        req {
            post "stage_instances"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<StageInstance>

let getStanceInstance
    (channelId: string)
    (client: IBotClient) =
        req {
            get $"stage-instances/{channelId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<StageInstance>

let modifyStageInstance
    (channelId: string)
    (auditLogReason: string option)
    (content: ModifyStageInstancePayload)
    (client: IBotClient) =
        req {
            patch $"stage-instances/{channelId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<StageInstance>

let deleteStageInstance
    (channelId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"stage-instances/{channelId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Sticker -----

let listStickerPacks
    (client: IBotClient) =
        req {
            get "sticker-packs"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ListStickerPacksOkResponse>

let getStickerPack
    (packId: string)
    (client: IBotClient) =
        req {
            get $"sticker-packs/{packId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<StickerPack>

let listGuildStickers
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/stickers"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Sticker list>

let getGuildSticker
    (guildId: string)
    (stickerId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/stickers/{stickerId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Sticker>

let createGuildSticker
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildStickerPayload)
    (client: IBotClient) =
        req {
            post $"guilds/{guildId}/stickers"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Sticker>

let modifyGuildSticker
    (guildId: string)
    (stickerId: string)
    (auditLogReason: string option)
    (content: CreateGuildStickerPayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/stickers/{stickerId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Sticker>

let deleteGuildSticker
    (guildId: string)
    (stickerId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"guilds/{guildId}/stickers/{stickerId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Subscription -----

let listSkuSubscriptions
    (skuId: string)
    (before: string option)
    (after: string option)
    (limit: int option)
    (userId: string option)
    (client: IBotClient) =
        req {
            get $"skus/{skuId}/subscriptions"
            query "before" before
            query "after" after
            query "limit" (Option.map string limit)
            query "userId" userId
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Subscription list>

let getSkuSubscription
    (skuId: string)
    (subscriptionId: string)
    (client: IBotClient) =
        req {
            get $"skus/{skuId}/subscriptions/{subscriptionId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Subscription>

// ----- User -----

let getCurrentUser
    (client: IDiscordClient) =
        req {
            get "users/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<User>
let getUser
    (userId: string)
    (client: IBotClient) =
        req {
            get $"users/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<User>

let modifyCurrentUser
    (content: ModifyCurrentUserPayload)
    (client: IBotClient) =
        req {
            patch "users/@me"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<User>

let getCurrentUserGuilds
    (before: string option)
    (after: string option)
    (limit: int option)
    (withCounts: bool option)
    (client: IDiscordClient) =
        req {
            get "users/@me/guilds"
            query "before" before
            query "after" after
            query "limit" (Option.map string limit)
            query "with_counts" (Option.map string withCounts)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<PartialGuild list>

let getCurrentUserGuildMember
    (guildId: string)
    (client: IDiscordClient) =
        req {
            get $"users/@me/guilds/{guildId}/member"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GuildMember>

let leaveGuild
    (guildId: string)
    (client: IBotClient) =
        req {
            delete $"users/@me/guilds/{guildId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let createDm
    (content: CreateDmPayload)
    (client: IBotClient) =
        req {
            post "users/@me/channels"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let createGroupDm
    (content: CreateGroupDmPayload)
    (client: IBotClient) =
        req {
            post "users/@me/channels"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Channel>

let getCurrentUserConnections
    (client: IOAuthClient) =
        req {
            get "users/@me/connections"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Connection list>

let getCurrentUserApplicationRoleConnection
    (applicationId: string)
    (client: IOAuthClient) =
        req {
            get $"users/@me/applications/{applicationId}/role-connection"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationRoleConnection>

let updateCurrentUserApplicationRoleConnection
    (applicationId: string)
    (content: UpdateCurrentUserApplicationRoleConnectionPayload)
    (client: IOAuthClient) =
        req {
            put $"users/@me/applications/{applicationId}/role-connection"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ApplicationRoleConnection>

// ----- Voice -----

let listVoiceRegions
    (client: IBotClient) =
        req {
            get "voice/regions"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<VoiceRegion list>

let getCurrentUserVoiceState
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/voice-states/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<VoiceState>

let getUserVoiceState
    (guildId: string)
    (userId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/voice-states/{userId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<VoiceState>

let modifyCurrentUserVoiceState
    (guildId: string)
    (content: ModifyCurrentUserVoiceStatePayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/voice-states/@me"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let modifyUserVoiceState
    (guildId: string)
    (userId: string)
    (content: ModifyUserVoiceStatePayload)
    (client: IBotClient) =
        req {
            patch $"guilds/{guildId}/voice-states/{userId}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Webhook -----

let createWebhook
    (channelId: string)
    (auditLogReason: string option)
    (content: CreateWebhookPayload)
    (client: IBotClient) =
        req {
            post $"channels/{channelId}/webhooks"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Webhook>

let getChannelWebhooks
    (channelId: string)
    (client: IBotClient) =
        req {
            get $"channels/{channelId}/webhooks"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Webhook list>

let getGuildWebhooks
    (guildId: string)
    (client: IBotClient) =
        req {
            get $"guilds/{guildId}/webhooks"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Webhook list>

let getWebhook
    (webhookId: string)
    (client: IBotClient) =
        req {
            get $"webhooks/{webhookId}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Webhook>

let getWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (client: IBotClient) =
        req {
            get $"webhooks/{webhookId}/{webhookToken}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Webhook>

let modifyWebhook
    (webhookId: string)
    (auditLogReason: string option)
    (content: ModifyWebhookPayload)
    (client: IBotClient) =
        req {
            patch $"webhooks/{webhookId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Webhook>

let modifyWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (content: ModifyWebhookWithTokenPayload)
    (client: IBotClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Webhook>

let deleteWebhook
    (webhookId: string)
    (auditLogReason: string option)
    (client: IBotClient) =
        req {
            delete $"webhooks/{webhookId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let deleteWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (client: IBotClient) =
        req {
            delete $"webhooks/{webhookId}/{webhookToken}"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let executeWebhook
    (webhookId: string)
    (webhookToken: string)
    (wait: bool option)
    (threadId: string option)
    (content: ExecuteWebhookPayload)
    (client: IBotClient) =
        req {
            post $"webhooks/{webhookId}/{webhookToken}"
            query "wait" (Option.map string wait)
            query "thread_id" threadId
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

let getWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (client: IBotClient) =
        req {
            get $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

let editWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (content: EditWebhookMessagePayload)
    (client: IBotClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Message>

let deleteWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (client: IBotClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

// ----- Gateway -----

let getGateway
    (version: string)
    (encoding: GatewayEncoding)
    (compression: GatewayCompression option)
    (client: HttpClient) = // TODO: Should this use the concrete HttpClient type?
        req {
            get "gateway"
            query "v" version
            query "encoding" (GatewayEncoding.toString encoding)
            query "compress" (Option.map (GatewayCompression.toString) compression)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GetGatewayOkResponse>
        
let getGatewayBot
    (version: string)
    (encoding: GatewayEncoding)
    (compression: GatewayCompression option)
    (client: IBotClient) =
        req {
            get "gateway/bot"
            query "v" version
            query "encoding" (GatewayEncoding.toString encoding)
            query "compress" (Option.map (GatewayCompression.toString) compression)
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GetGatewayBotOkResponse>

// ----- OAuth2 -----

let getCurrentBotApplicationInformation
    (client: IOAuthClient) =
        req {
            host Constants.DISCORD_OAUTH_URL
            get "applications/@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<Application>

let getCurrentAuthorizationInformation
    (client: IOAuthClient) =
        req {
            host Constants.DISCORD_OAUTH_URL
            get "@me"
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<GetCurrentAuthorizationInformationOkResponse>

let authorizationCodeGrant
    (content: AuthorizationCodeGrantPayload)
    (client: IBasicClient) =
        req {
            host Constants.DISCORD_OAUTH_URL
            post "token"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<AuthorizationCodeGrantResponse>

let refreshTokenGrant
    (content: RefreshTokenGrantPayload)
    (client: IBasicClient) =
        req {
            host Constants.DISCORD_OAUTH_URL
            post "token"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<RefreshTokenGrantResponse>

let revokeToken
    (content: RevokeTokenPayload)
    (client: IBasicClient) =
        req {
            host Constants.DISCORD_OAUTH_URL
            post "token/revoke"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asEmpty

let clientCredentialsGrant
    (content: ClientCredentialsGrantPayload)
    (client: IBasicClient) =
        req {
            host Constants.DISCORD_OAUTH_URL
            post "token"
            payload content
        }
        |> client.SendAsync
        |> Task.mapT DiscordResponse.asJson<ClientCredentialsGrantResponse>
