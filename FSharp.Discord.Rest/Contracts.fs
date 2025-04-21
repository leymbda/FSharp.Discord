namespace FSharp.Discord.Rest

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open Thoth.Json.Net

// ----- Interactions: Receiving and Responding -----

type CreateInteractionResponseRequest(interactionId, interactionToken, payload) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken

    member val Payload: CreateInteractionResponsePayload = payload

type GetOriginalInteractionResponseRequest(interactionId, interactionToken) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken
     
type EditOriginalInteractionResponseRequest(interactionId, interactionToken, payload) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken

    member val Payload: EditOriginalInteractionResponsePayload = payload

type DeleteOriginalInteractionResponseRequest(interactionId, interactionToken) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken

type CreateFollowupMessageRequest(applicationId, interactionToken, payload) =
    member val ApplicationId: string = applicationId
    member val InteractionToken: string = interactionToken

    member val Payload: CreateFollowUpMessagePayload = payload

type GetFollowupMessageRequest(applicationId, interactionToken, messageId) =
    member val ApplicationId: string = applicationId
    member val InteractionToken: string = interactionToken
    member val MessageId: string = messageId
    
type EditFollowupMessageRequest(applicationId, interactionToken, messageId, payload) =
    member val ApplicationId: string = applicationId
    member val InteractionToken: string = interactionToken
    member val MessageId: string = messageId

    member val Payload: EditFollowupMessagePayload = payload

type DeleteFollowupMessageRequest(applicationId, interactionToken, messageId) =
    member val ApplicationId: string = applicationId
    member val InteractionToken: string = interactionToken
    member val MessageId: string = messageId

// ----- Interactions: Application Commands -----

type GetGlobalApplicationCommandsRequest(applicationId, ?withLocalizations) =
    member val ApplicationId: string = applicationId

    member val withLocalizations: bool option = withLocalizations
    
type CreateGlobalApplicationCommandRequest(applicationId, payload) =
    member val ApplicationId: string = applicationId

    member val Payload: CreateGlobalApplicationCommandPayload = payload

type GetGlobalApplicationCommandRequest(applicationId, commandId) =
    member val ApplicationId: string = applicationId
    member val CommandId: string = commandId

type EditGlobalApplicationCommandRequest(applicationId, commandId, payload) =
    member val ApplicationId: string = applicationId
    member val CommandId: string = commandId

    member val Payload: EditGlobalApplicationCommandPayload = payload

type DeleteGlobalApplicationCommandRequest(applicationId, commandId) =
    member val ApplicationId: string = applicationId
    member val CommandId: string = commandId

type BulkOverwriteGlobalApplicationCommandsRequest(applicationId, payload) =
    member val ApplicationId: string = applicationId

    member val Payload: BulkOverwriteGlobalApplicationCommandsPayload = payload

type GetGuildApplicationCommandsRequest(applicationId, guildId, ?withLocalizations) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId

    member val withLocalizations: bool option = withLocalizations

type CreateGuildApplicationCommandRequest(applicationId, guildId, payload) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId

    member val Payload: CreateGuildApplicationCommandPayload = payload
    
type GetGuildApplicationCommandRequest(applicationId, guildId, commandId) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId
    member val CommandId: string = commandId
    
type EditGuildApplicationCommandRequest(applicationId, guildId, commandId, payload) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId
    member val CommandId: string = commandId

    member val Payload: EditGuildApplicationCommandPayload = payload
    
type DeleteGuildApplicationCommandRequest(applicationId, guildId, commandId) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId
    member val CommandId: string = commandId

type BulkOverwriteGuildApplicationCommandsRequest(applicationId, guildId, payload) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId

    member val Payload: BulkOverwriteGuildApplicationCommandsPayload = payload

type GetGuildApplicationCommandPermissionsRequest(applicationId, guildId) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId

type GetApplicationCommandPermissionsRequest(applicationId, guildId, commandId) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId
    member val CommandId: string = commandId

type EditApplicationCommandPermissionsRequest(applicationId, guildId, commandId, payload) =
    member val ApplicationId: string = applicationId
    member val GuildId: string = guildId
    member val CommandId: string = commandId

    member val Payload: EditApplicationCommandPermissionsPayload = payload

// ----- Events: Using Gateway -----

type GetGatewayRequest(version, encoding, compression) =
    member val Version: string = version
    member val Encoding: GatewayEncoding = encoding
    member val Compression: GatewayCompression option = compression

type GetGatewayResponse = {
    Url: string
}

module GetGatewayResponse =
    let decoder: Decoder<GetGatewayResponse> =
        Decode.object (fun get -> {
            Url = get |> Get.required "url" Decode.string
        })
        
type GetGatewayBotRequest(version, encoding, compression) =
    member val Version: string = version
    member val Encoding: GatewayEncoding = encoding
    member val Compression: GatewayCompression option = compression

type GetGatewayBotResponse = {
    Url: string
    Shards: int
    SessionStartLimit: SessionStartLimit
}

module GetGatewayBotResponse =
    let decoder: Decoder<GetGatewayBotResponse> =
        Decode.object (fun get -> {
            Url = get |> Get.required "url" Decode.string
            Shards = get |> Get.required "shards" Decode.int
            SessionStartLimit = get |> Get.required "session_start_limit" SessionStartLimit.decoder
        })
        
// ----- Resources: Application -----

type EditCurrentApplicationRequest(payload) =
    member val Payload: EditCurrentApplicationPayload = payload

type GetApplicationActivityInstanceRequest(applicationId, instanceId) =
    member val ApplicationId: string = applicationId
    member val InstanceId: string = instanceId

// ----- Resources: Application Role Connection Metadata -----

type GetApplicationRoleConnectionMetadataRecordsRequest(applicationId) =
    member val ApplicationId: string = applicationId

type UpdateApplicationRoleConnectionMetadataRecordsRequest(applicationId, payload) =
    member val ApplicationId: string = applicationId

    member val Payload: UpdateApplicationRoleConnectionMetadataRecordsPayload = payload

// ----- Resources: Audit Log -----

type GetGuildAuditLogRequest(guildId, ?userId, ?actionType, ?before, ?after, ?limit) =
    member val GuildId: string = guildId

    member val UserId: string option = userId
    member val ActionType: AuditLogEventType option = actionType
    member val Before: string option = before
    member val After: string option = after
    member val Limit: int option = limit

// ----- Resources: Auto Moderation -----

type ListAutoModerationRulesForGuildRequest(guildId) =
    member val GuildId: string = guildId
    
type GetAutoModerationRuleRequest(guildId, ruleId) =
    member val GuildId: string = guildId
    member val RuleId: string = ruleId

type CreateAutoModerationRuleRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateAutoModerationRulePayload = payload

type ModifyAutoModerationRuleRequest(guildId, ruleId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val RuleId: string = ruleId

    member val AuditLogReason: string option = auditLogReason
    
    member val Payload: ModifyAutoModerationRulePayload = payload

type DeleteAutoModerationRuleRequest(guildId, ruleId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val RuleId: string = ruleId

    member val AuditLogReason: string option = auditLogReason

// ----- Resources: Channel -----

type GetChannelRequest(channelId) =
    member val ChannelId: string = channelId

type ModifyChannelRequest(channelId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

    member val Payload = payload |> ModifyChannelPayload.toPayload

type DeleteChannelRequest(channelId, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

type EditChannelPermissionsRequest(channelId, overwriteId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId
    member val OverwriteId: string = overwriteId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: EditChannelPermissionsPayload = payload

type GetChannelInvitesRequest(channelId) =
    member val ChannelId: string = channelId

type CreateChannelInviteRequest(channelId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateChannelInvitePayload = payload

type DeleteChannelPermissionRequest(channelId, overwriteId, ?auditLogReason) =
    member val ChannelId: string = channelId
    member val OverwriteId: string = overwriteId

    member val AuditLogReason: string option = auditLogReason

type FollowAnnouncementChannelRequest(channelId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: FollowAnnouncementChannelPayload = payload

type TriggerTypingIndicatorRequest(channelId) =
    member val ChannelId: string = channelId

type GetPinnedMessagesRequest(channelId) =
    member val ChannelId: string = channelId

type PinMessageRequest(channelId, messageId, ?auditLogReason) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

    member val AuditLogReason: string option = auditLogReason

type UnpinMessageRequest(channelId, messageId, ?auditLogReason) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

    member val AuditLogReason: string option = auditLogReason

type GroupDmAddRecipientRequest(channelId, userId, payload) =
    member val ChannelId: string = channelId
    member val UserId: string = userId
    
    member val Payload: GroupDmAddRecipientPayload = payload

type GroupDmRemoveRecipientRequest(channelId, userId) =
    member val ChannelId: string = channelId
    member val UserId: string = userId

type StartThreadFromMessageRequest(channelId, messageId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId
    
    member val AuditLogReason: string option = auditLogReason

    member val Payload: StartThreadFromMessagePayload = payload

type StartThreadWithoutMessageRequest(channelId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: StartThreadWithoutMessagePayload = payload

type StartThreadInForumOrMediaChannelRequest(channelId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: StartThreadInForumOrMediaChannelPayload = payload
    
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

type JoinThreadRequest(channelId) =
    member val ChannelId: string = channelId

type AddThreadMemberRequest(channelId, userId) =
    member val ChannelId: string = channelId
    member val UserId: string = userId

type LeaveThreadRequest(channelId) =
    member val ChannelId: string = channelId

type RemoveThreadMemberRequest(channelId, userId) =
    member val ChannelId: string = channelId
    member val UserId: string = userId

type GetThreadMemberRequest(channelId, userId, ?withMember) =
    member val ChannelId: string = channelId
    member val UserId: string = userId

    member val WithMember: bool option = withMember

type ListThreadMembersRequest(channelId, ?withMember, ?after, ?limit) =
    member val ChannelId: string = channelId

    member val WithMember: bool option = withMember
    member val After: string option = after
    member val Limit: int option = limit

type ListPublicArchivedThreadsRequest(channelId, ?before, ?limit) =
    member val ChannelId: string = channelId

    member val Before: DateTime option = before
    member val Limit: int option = limit
    
type ListPrivateArchivedThreadsRequest(channelId, ?before, ?limit) =
    member val ChannelId: string = channelId

    member val Before: DateTime option = before
    member val Limit: int option = limit

type ListJoinedPrivateArchivedThreadsRequest(channelId, ?before, ?limit) =
    member val ChannelId: string = channelId

    member val Before: string option = before
    member val Limit: int option = limit

type ArchivedThreadsResponse = {
    Threads: Channel list
    Members: ThreadMember list
    HasMore: bool
}

module ArchivedThreadsResponse =
    let decoder: Decoder<ArchivedThreadsResponse> =
        Decode.object (fun get -> {
            Threads = get |> Get.required "threads" (Decode.list Channel.decoder)
            Members = get |> Get.required "members" (Decode.list ThreadMember.decoder)
            HasMore = get |> Get.required "has_more" Decode.bool
        })

// ----- Resources: Emoji -----

type ListGuildEmojisRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildEmojiRequest(guildId, emojiId) =
    member val GuildId: string = guildId
    member val EmojiId: string = emojiId

type CreateGuildEmojiRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    
    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateGuildEmojiPayload = payload

type ModifyGuildEmojiRequest(guildId, emojiId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val EmojiId: string = emojiId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildEmojiPayload = payload

type DeleteGuildEmojiRequest(guildId, emojiId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val EmojiId: string = emojiId

    member val AuditLogReason: string option = auditLogReason

type ListApplicationEmojisRequest(applicationId) =
    member val ApplicationId: string = applicationId

type ListApplicationEmojisResponse = {
    Items: Emoji list
}

module ListApplicationEmojisResponse =
    let decoder: Decoder<ListApplicationEmojisResponse> =
        Decode.object (fun get -> {
            Items = get |> Get.required "emojis" (Decode.list Emoji.decoder)
        })

type GetApplicationEmojiRequest(applicationId, emojiId) =
    member val ApplicationId: string = applicationId
    member val EmojiId: string = emojiId

type CreateApplicationEmojiRequest(applicationId, payload) =
    member val ApplicationId: string = applicationId

    member val Payload: CreateApplicationEmojiPayload = payload

type ModifyApplicationEmojiRequest(applicationId, emojiId, payload) =
    member val ApplicationId: string = applicationId
    member val EmojiId: string = emojiId

    member val Payload: ModifyApplicationEmojiPayload = payload

type DeleteApplicationEmojiRequest(applicationId, emojiId) =
    member val ApplicationId: string = applicationId
    member val EmojiId: string = emojiId

// ----- Resources: Entitlement -----

type ListEntitlementsRequest(
    applicationId, ?userId, ?skuIds, ?before, ?after, ?limit, ?guildId, ?excludeEndedm, ?excludeDeleted
) =
    member val ApplicationId: string = applicationId

    member val UserId: string option = userId
    member val SkuIds: string list option = skuIds
    member val Before: string option = before
    member val After: string option = after
    member val Limit: int option = limit
    member val GuildId: string option = guildId
    member val ExcludeEnded: bool option = excludeEndedm
    member val ExcludeDeleted: bool option = excludeDeleted

type GetEntitlementRequest(applicationId, entitlementId) =
    member val ApplicationId: string = applicationId
    member val EntitlementId: string = entitlementId

type ConsumeEntitlementRequest(applicationId, entitlementId) =
    member val ApplicationId: string = applicationId
    member val EntitlementId: string = entitlementId

type CreateTestEntitlementRequest(applicationId, payload) =
    member val ApplicationId: string = applicationId

    member val Payload: CreateTestEntitlementPayload = payload

type DeleteTestEntitlementRequest(applicationId, entitlementId) =
    member val ApplicationId: string = applicationId
    member val EntitlementId: string = entitlementId
    
// ----- Resources: Guild Scheduled Event -----

type ListScheduledEventsForGuildRequest(guildId, ?withUserCount) =
    member val GuildId: string = guildId

    member val WithUserCount: bool option = withUserCount

type CreateGuildScheduledEventRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateGuildScheduledEventPayload = payload

type GetGuildScheduledEventRequest(guildId, scheduledEventId, ?withUserCount) =
    member val GuildId: string = guildId
    member val ScheduledEventId: string = scheduledEventId
    
    member val WithUserCount: bool option = withUserCount

type ModifyGuildScheduledEventRequest(guildId, scheduledEventId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val ScheduledEventId: string = scheduledEventId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildScheduledEventPayload = payload

type DeleteGuildScheduledEventRequest(guildId, scheduledEventId) =
    member val GuildId: string = guildId
    member val ScheduledEventId: string = scheduledEventId

type GetGuildScheduledEventUsersRequest(guildId, scheduledEventId, ?limit, ?withMember, ?before, ?after) =
    member val GuildId: string = guildId
    member val ScheduledEventId: string = scheduledEventId

    member val Limit: int option = limit
    member val WithMember: bool option = withMember
    member val Before: string option = before
    member val After: string option = after

// ----- Resources: Guild Template -----

type GetGuildTemplateRequest(code) =
    member val Code: string = code

type CreateGuildFromGuildTemplateRequest(code, payload) =
    member val Code: string = code

    member val Payload: CreateGuildFromGuildTemplatePayload = payload

type GetGuildTemplatesRequest(guildId) =
    member val GuildId: string = guildId

type CreateGuildTemplateRequest(guildId, payload) =
    member val GuildId: string = guildId

    member val Payload: CreateGuildTemplatePayload = payload

type SyncGuildTemplateRequest(guildId, code) =
    member val GuildId: string = guildId
    member val Code: string = code

type ModifyGuildTemplateRequest(guildId, code, payload) =
    member val GuildId: string = guildId
    member val Code: string = code

    member val Payload: ModifyGuildTemplatePayload = payload

type DeleteGuildTemplateRequest(guildId, code) =
    member val GuildId: string = guildId
    member val Code: string = code

// ----- Resources: Guild -----

type GetGuildRequest(guildId, ?withCounts) =
    member val GuildId: string = guildId

    member val WithCounts: bool option = withCounts

type GetGuildPreviewRequest(guildId) =
    member val GuildId: string = guildId

type ModifyGuildRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    
    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildPayload = payload

type DeleteGuildRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildChannelsRequest(guildId) =
    member val GuildId: string = guildId

type CreateGuildChannelRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateGuildChannelPayload = payload

type ModifyGuildChannelPositionsRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildChannelPositionsPayload = payload

type ListActiveGuildThreadsRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildMemberRequest(guildId, userId) =
    member val GuildId: string = guildId
    member val UserId: string = userId

type ListGuildMembersRequest(guildId, ?after, ?limit) =
    member val GuildId: string = guildId

    member val After: string option = after
    member val Limit: int option = limit

type SearchGuildMembersRequest(guildId, query, ?limit) =
    member val GuildId: string = guildId

    member val Query: string = query
    member val Limit: int option = limit

type AddGuildMemberRequest(guildId, userId, payload) =
    member val GuildId: string = guildId
    member val UserId: string = userId

    member val Payload: AddGuildMemberPayload = payload

type ModifyGuildMemberRequest(guildId, userId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val UserId: string = userId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildMemberPayload = payload

type ModifyCurrentMemberRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    
    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyCurrentMemberPayload = payload

type AddGuildMemberRoleRequest(guildId, userId, roleId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val UserId: string = userId
    member val RoleId: string = roleId

    member val AuditLogReason: string option = auditLogReason

type RemoveGuildMemberRoleRequest(guildId, userId, roleId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val UserId: string = userId
    member val RoleId: string = roleId

    member val AuditLogReason: string option = auditLogReason

type RemoveGuildMemberRequest(guildId, userId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val UserId: string = userId

    member val AuditLogReason: string option = auditLogReason

type GetGuildBansRequest(guildId, ?limit, ?before, ?after) =
    member val GuildId: string = guildId
    
    member val Limit: int option = limit
    member val Before: string option = before
    member val After: string option = after

type GetGuildBanRequest(guildId, userId) =
    member val GuildId: string = guildId
    member val UserId: string = userId

type CreateGuildBanRequest(guildId, userId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val UserId: string = userId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateGuildBanPayload = payload

type RemoveGuildBanRequest(guildId, userId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val UserId: string = userId
    member val AuditLogReason: string option = auditLogReason

type BulkGuildBanRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: BulkGuildBanPayload = payload

type GetGuildRolesRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildRoleRequest(guildId, roleId) =
    member val GuildId: string = guildId
    member val RoleId: string = roleId

type CreateGuildRoleRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateGuildRolePayload = payload

type ModifyGuildRolePositionsRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildRolePositionsPayload = payload

type ModifyGuildRoleRequest(guildId, roleId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val RoleId: string = roleId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildRolePayload = payload

type ModifyGuildMfaLevelRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildMfaLevelPayload = payload

type DeleteGuildRoleRequest(guildId, roleId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val RoleId: string = roleId

    member val AuditLogReason: string option = auditLogReason

type GetGuildPruneCountRequest(guildId, ?days, ?includeRoles) =
    member val GuildId: string = guildId

    member val Days: int option = days
    member val IncludeRoles: string list option = includeRoles

type BeginGuildPruneRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: BeginGuildPrunePayload = payload

type GetGuildVoiceRegionsRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildInvitesRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildIntegrationsRequest(guildId) =
    member val GuildId: string = guildId

type DeleteGuildIntegrationRequest(guildId, integrationId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val IntegrationId: string = integrationId

    member val AuditLogReason: string option = auditLogReason

type GetGuildWidgetSettingsRequest(guildId) =
    member val GuildId: string = guildId

type ModifyGuildWidgetRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildWidgetPayload = payload

type GetGuildWidgetRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildVanityUrlRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildWidgetImageRequest(guildId, ?style) =
    member val GuildId: string = guildId
    member val Style: GuildWidgetStyle option = style

type GetGuildWelcomeScreenRequest(guildId) =
    member val GuildId: string = guildId

type ModifyGuildWelcomeScreenRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildWelcomeScreenPayload = payload

type GetGuildOnboardingRequest(guildId) =
    member val GuildId: string = guildId

type ModifyGuildOnboardingRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildOnboardingPayload = payload

type ModifyGuildIncidentActionsRequest(guildId, payload) =
    member val GuildId: string = guildId
    member val Payload: ModifyGuildIncidentActionsPayload = payload

// ----- Resources: Invite -----

type GetInviteRequest(code, ?withCounts, ?withExpiration, ?guildScheduledEventId) =
    member val Code: string = code

    member val WithCounts: bool option = withCounts
    member val WithExpiration: bool option = withExpiration
    member val GuildScheduledEventId: string option = guildScheduledEventId

type DeleteInviteRequest(code, ?auditLogReason) =
    member val Code: string = code
    member val AuditLogReason: string option = auditLogReason

// ----- Resources: Lobby -----

type CreateLobbyRequest(payload) =
    member val Payload: CreateLobbyPayload = payload

type GetLobbyRequest(lobbyId) =
    member val LobbyId: string = lobbyId

type ModifyLobbyRequest(lobbyId, payload) =
    member val LobbyId: string = lobbyId

    member val Payload: ModifyLobbyPayload = payload

type DeleteLobbyRequest(lobbyId) =
    member val LobbyId: string = lobbyId

type AddMemberToLobbyRequest(lobbyId, userId, payload) =
    member val LobbyId: string = lobbyId
    member val UserId: string = userId

    member val Payload: AddMemberToLobbyPayload = payload

type RemoveMemberFromLobbyRequest(lobbyId, userId) =
    member val LobbyId: string = lobbyId
    member val UserId: string = userId

type LeaveLobbyRequest(lobbyId) =
    member val LobbyId: string = lobbyId

type LinkChannelToLobbyRequest(lobbyId, payload) =
    member val LobbyId: string = lobbyId

    member val Payload: LinkChannelToLobbyPayload = payload

type UnlinkChannelFromLobbyRequest(lobbyId) =
    member val LobbyId: string = lobbyId

// ----- Resources: Message -----

type GetChannelMessagesRequest(channelId, ?around, ?before, ?after, ?limit) =
    member val ChannelId: string = channelId

    member val Around: string option = around
    member val Before: string option = before
    member val After: string option = after
    member val Limit: int option = limit

type GetChannelMessageRequest(channelId, messageId) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

type CreateMessageRequest(channelId, payload) =
    member val ChannelId: string = channelId

    member val Payload: CreateMessagePayload = payload

type CrosspostMessageRequest(channelId, messageId) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

type CreateReactionRequest(channelId, messageId, emoji) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId
    member val Emoji: string = emoji

type DeleteOwnReactionRequest(channelId, messageId, emoji) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId
    member val Emoji: string = emoji

type DeleteUserReactionRequest(channelId, messageId, emoji, userId) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId
    member val Emoji: string = emoji
    member val UserId: string = userId

type GetReactionsRequest(channelId, messageId, emoji, ?type', ?limit, ?after) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId
    member val Emoji: string = emoji

    member val Type: ReactionType option = type'
    member val Limit: int option = limit
    member val After: string option = after

type DeleteAllReactionsRequest(channelId, messageId) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

type DeleteAllReactionsForEmojiRequest(channelId, messageId, emoji) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId
    member val Emoji: string = emoji

type EditMessageRequest(channelId, messageId, payload) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

    member val Payload: EditMessagePayload = payload

type DeleteMessageRequest(channelId, messageId, ?auditLogReason) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

    member val AuditLogReason: string option = auditLogReason

type BulkDeleteMessagesRequest(channelId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: BulkDeleteMessagesPayload = payload

// ----- Resources: Poll -----

type GetAnswerVotersRequest(channelId, messageId, answerId, ?after, ?limit) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId
    member val AnswerId: string = answerId

    member val After: string option = after
    member val Limit: int option = limit

type EndPollRequest(channelId, messageId) =
    member val ChannelId: string = channelId
    member val MessageId: string = messageId

// ----- Resources: SKU -----

type ListSkusRequest(applicationId) =
    member val ApplicationId: string = applicationId
 
// ----- Resources: Soundboard -----

type SendSoundboardSoundRequest(channelId, payload) =
    member val ChannelId: string = channelId

    member val Payload: SendSoundboardSoundPayload = payload

type ListGuildSoundboardSoundsRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildSoundboardSoundRequest(guildId, soundId) =
    member val GuildId: string = guildId
    member val SoundId: string = soundId

type CreateGuildSoundboardSoundRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateGuildSoundboardSoundPayload = payload

type ModifyGuildSoundboardSoundRequest(guildId, soundId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val SoundId: string = soundId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildSoundboardSoundPayload = payload

type DeleteGuildSoundboardSoundRequest(guildId, soundId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val SoundId: string = soundId

    member val AuditLogReason: string option = auditLogReason    

// ----- Resources: Stage Instance -----

type CreateStageInstanceRequest(payload, ?auditLogReason) =
    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateStageInstancePayload = payload

type GetStageInstanceRequest(channelId) =
    member val ChannelId: string = channelId

type ModifyStageInstanceRequest(channelId, payload, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyStageInstancePayload = payload

type DeleteStageInstanceRequest(channelId, ?auditLogReason) =
    member val ChannelId: string = channelId

    member val AuditLogReason: string option = auditLogReason

// ----- Resources: Sticker -----

type GetStickerRequest(stickerId) =
    member val StickerId: string = stickerId

type GetStickerPackRequest(packId) =
    member val PackId: string = packId

type ListGuildStickersRequest(guildId) =
    member val GuildId: string = guildId

type GetGuildStickerRequest(guildId, stickerId) =
    member val GuildId: string = guildId
    member val StickerId: string = stickerId

type CreateGuildStickerRequest(guildId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    
    member val AuditLogReason: string option = auditLogReason

    member val Payload: CreateGuildStickerPayload = payload

type ModifyGuildStickerRequest(guildId, stickerId, payload, ?auditLogReason) =
    member val GuildId: string = guildId
    member val StickerId: string = stickerId
    
    member val AuditLogReason: string option = auditLogReason

    member val Payload: ModifyGuildStickerPayload = payload

type DeleteGuildStickerRequest(guildId, stickerId, ?auditLogReason) =
    member val GuildId: string = guildId
    member val StickerId: string = stickerId
    
    member val AuditLogReason: string option = auditLogReason

// ----- Resources: Subscription -----

type ListSkuSubscriptionsRequest(skuId, ?before, ?after, ?limit, ?userId) =
    member val SkuId: string = skuId

    member val Before: string option = before
    member val After: string option = after
    member val Limit: int option = limit
    member val UserId: string option = userId

type GetSkuSubscriptionRequest(skuId, subscriptionId) =
    member val SkuId: string = skuId
    member val SubscriptionId: string = subscriptionId

// ----- Resources: User -----

type GetUserRequest(userId) =
    member val UserId: string = userId

type ModifyCurrentUserRequest(payload) =
    member val Payload: ModifyCurrentUserPayload = payload

type GetCurrentUserGuildsRequest(?before, ?after, ?limit, ?withCounts) =
    member val Before: string option = before
    member val After: string option = after
    member val Limit: int option = limit
    member val WithCounts: bool option = withCounts

type GetCurrentUserGuildMemberRequest(guildId) =
    member val GuildId: string = guildId

type LeaveGuildRequest(guildId) =
    member val GuildId: string = guildId

type CreateDmRequest(payload) =
    member val Payload: CreateDmPayload = payload

type CreateGroupDmRequest(payload) =
    member val Payload: CreateGroupDmPayload = payload

type GetCurrentUserApplicationRoleConnectionRequest(applicationId) =
    member val ApplicationId: string = applicationId

type UpdateCurrentUserApplicationRoleConnectionRequest(applicationId, payload) =
    member val ApplicationId: string = applicationId

    member val Payload: UpdateCurrentUserApplicationRoleConnectionPayload = payload

// ----- Resources: Voice -----

// ----- Resources: Webhook -----

// ----- Topics: OAuth2 -----
