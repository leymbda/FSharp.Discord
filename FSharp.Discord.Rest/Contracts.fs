namespace FSharp.Discord.Rest

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
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
