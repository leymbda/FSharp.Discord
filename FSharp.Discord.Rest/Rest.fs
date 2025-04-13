module FSharp.Discord.Rest.Rest

open FSharp.Discord.Types.Serialization
open System.Net.Http
open Thoth.Json.Net

let [<Literal>] API_BASE_URL = "https://discord.com/api/v10"
let [<Literal>] OAUTH_BASE_URL = "https://discord.com/api/oauth2"

// ----- Interactions: Receiving and Responding -----

// https://discord.com/developers/docs/interactions/receiving-and-responding#create-interaction-response
let createInteractionResponse (req: CreateInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "interactions"; req.InteractionId; req.InteractionToken; "callback"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
    
// https://discord.com/developers/docs/interactions/receiving-and-responding#create-interaction-response
let createInteractionResponseWithCallback (req: CreateInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "interactions"; req.InteractionId; req.InteractionToken; "callback"]
    |> Uri.withRequiredQuery "with_response" "true"
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode InteractionCallbackResponse.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#get-original-interaction-response
let getOriginalInteractionResponse (req: GetOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)
    
// https://discord.com/developers/docs/interactions/receiving-and-responding#edit-original-interaction-response
let editOriginalInteractionResponse (req: EditOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#delete-original-interaction-response
let deleteOriginalInteractionResponse (req: DeleteOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/interactions/receiving-and-responding#create-followup-message
let createFollowupMessage (req: CreateFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#get-followup-message
let getFollowupMessage (req: GetFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken; "messages"; req.MessageId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#edit-followup-message
let editFollowupMessage (req: EditFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken; "messages"; req.MessageId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/resources/webhook#delete-webhook-message
let deleteFollowupMessage (req: DeleteFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken; "messages"; req.MessageId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// ----- Interactions: Application Commands -----

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-commands
let getGlobalApplicationCommands (req: GetGlobalApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"]
    |> Uri.withOptionalQuery "with_localizations" (Option.map string req.withLocalizations)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#create-global-application-command
let createGlobalApplicationCommand (req: CreateGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-command
let getGlobalApplicationCommand (req: GetGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#edit-global-application-command
let editGlobalApplicationCommand (req: EditGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#delete-global-application-command
let deleteGlobalApplicationCommand (req: DeleteGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
    
// https://discord.com/developers/docs/interactions/application-commands#bulk-overwrite-global-application-commands
let bulkOverwriteGlobalApplicationCommands (req: BulkOverwriteGlobalApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-commands
let getGuildApplicationCommands (req: GetGuildApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.withOptionalQuery "with_localizations" (Option.map string req.withLocalizations)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#create-guild-application-command
let createGuildApplicationCommand (req: CreateGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-command
let getGuildApplicationCommand (req: GetGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#edit-guild-application-command
let editGuildApplicationCommand (req: EditGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#create-guild-application-command
let deleteGuildApplicationCommand (req: DeleteGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/interactions/application-commands#bulk-overwrite-guild-application-commands
let bulkOverwriteGuildApplicationCommands (req: BulkOverwriteGuildApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#get-guild-application-command-permissions
let getGuildApplicationCommandPermissions (req: GetGuildApplicationCommandPermissionsRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; "permissions"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list GuildApplicationCommandPermissions.decoder))

// https://discord.com/developers/docs/interactions/application-commands#get-application-command-permissions
let getApplicationCommandPermissions (req: GetApplicationCommandPermissionsRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId; "permissions"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode GuildApplicationCommandPermissions.decoder)

// https://discord.com/developers/docs/interactions/application-commands#edit-application-command-permissions
let editApplicationCommandPermissions (req: EditApplicationCommandPermissionsRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId; "permissions"]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
    
// ----- Events: Using Gateway -----

// https://discord.com/developers/docs/events/gateway#get-gateway
let getGateway (req: GetGatewayRequest) (client: HttpClient) = // TODO: This should probably use an interface rather than the concrete HttpClient
    Uri.create [API_BASE_URL; "gateway"]
    |> Uri.withRequiredQuery "v" req.Version
    |> Uri.withRequiredQuery "encoding" (GatewayEncoding.toString req.Encoding)
    |> Uri.withOptionalQuery "compress" (Option.map GatewayCompression.toString req.Compression)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode GetGatewayResponse.decoder)

// https://discord.com/developers/docs/events/gateway#get-gateway-bot
let getGatewayBot (req: GetGatewayBotRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "gateway"; "bot"]
    |> Uri.withRequiredQuery "v" req.Version
    |> Uri.withRequiredQuery "encoding" (GatewayEncoding.toString req.Encoding)
    |> Uri.withOptionalQuery "compress" (Option.map GatewayCompression.toString req.Compression)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode GetGatewayBotResponse.decoder)

// ----- Resources: Application -----

// https://discord.com/developers/docs/resources/application#get-current-application
let getCurrentApplication (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; "@me"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Application.decoder)

// https://discord.com/developers/docs/resources/application#edit-current-application
let editCurrentApplication (req: EditCurrentApplicationRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; "@me"]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Application.decoder)

// https://discord.com/developers/docs/resources/application#get-application-activity-instance
let getApplicationActivityInstance (req: GetApplicationActivityInstanceRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "activity-instances"; req.InstanceId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ActivityInstance.decoder)

// ----- Resources: Application Role Connection Metadata -----

// https://discord.com/developers/docs/resources/application-role-connection-metadata#get-application-role-connection-metadata-records
let getApplicationRoleConnectionMetadataRecords (req: GetApplicationRoleConnectionMetadataRecordsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "role-connections"; "metadata"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationRoleConnectionMetadata.decoder))

// https://discord.com/developers/docs/resources/application-role-connection-metadata#update-application-role-connection-metadata-records
let updateApplicationRoleConnectionMetadataRecords (req: UpdateApplicationRoleConnectionMetadataRecordsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "role-connections"; "metadata"]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationRoleConnectionMetadata.decoder))

// ----- Resources: Audit Log -----

// https://discord.com/developers/docs/resources/audit-log#get-guild-audit-log
let getGuildAuditLog (req: GetGuildAuditLogRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "audit-logs"]
    |> Uri.withOptionalQuery "user_id" req.UserId
    |> Uri.withOptionalQuery "action_type" (Option.map string req.ActionType)
    |> Uri.withOptionalQuery "before" req.Before
    |> Uri.withOptionalQuery "after" req.After
    |> Uri.withOptionalQuery "limit" (Option.map string req.Limit)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode AuditLog.decoder)

// ----- Resources: Auto Moderation -----

// https://discord.com/developers/docs/resources/auto-moderation#list-auto-moderation-rules-for-guild
let listAutoModerationRulesForGuild (req: ListAutoModerationRulesForGuildRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "auto-moderation"; "rules"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list AutoModerationRule.decoder))

// https://discord.com/developers/docs/resources/auto-moderation#get-auto-moderation-rule
let getAutoModerationRule (req: GetAutoModerationRuleRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "auto-moderation"; "rules"; req.RuleId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode AutoModerationRule.decoder)

// https://discord.com/developers/docs/resources/auto-moderation#create-auto-moderation-rule
let createAutoModerationRule (req: CreateAutoModerationRuleRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "auto-moderation"; "rules"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode AutoModerationRule.decoder)

// https://discord.com/developers/docs/resources/auto-moderation#modify-auto-moderation-rule
let modifyAutoModerationRule (req: ModifyAutoModerationRuleRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "auto-moderation"; "rules"; req.RuleId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode AutoModerationRule.decoder)

// https://discord.com/developers/docs/resources/auto-moderation#delete-auto-moderation-rule
let deleteAutoModerationRule (req: DeleteAutoModerationRuleRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "auto-moderation"; "rules"; req.RuleId]
    |> Uri.toRequest HttpMethod.Delete
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// ----- Resources: Channel -----

// https://discord.com/developers/docs/resources/channel#get-channel
let getChannel (req: GetChannelRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Channel.decoder)

// https://discord.com/developers/docs/resources/channel#modify-channel
let modifyChannel (req: ModifyChannelRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Channel.decoder)

// https://discord.com/developers/docs/resources/channel#deleteclose-channel
let deleteChannel (req: DeleteChannelRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId]
    |> Uri.toRequest HttpMethod.Delete
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Channel.decoder)

// https://discord.com/developers/docs/resources/channel#edit-channel-permissions
let editChannelPermissions (req: EditChannelPermissionsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "permissions"; req.OverwriteId]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#get-channel-invites
let getChannelInvites (req: GetChannelInvitesRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "invites"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list InviteWithMetadata.decoder))

// https://discord.com/developers/docs/resources/channel#create-channel-invite
let createChannelInvite (req: CreateChannelInviteRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "invites"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode InviteWithMetadata.decoder)

// https://discord.com/developers/docs/resources/channel#delete-channel-permission
let deleteChannelPermission (req: DeleteChannelPermissionRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "permissions"; req.OverwriteId]
    |> Uri.toRequest HttpMethod.Delete
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#follow-announcement-channel
let followAnnouncementChannel (req: FollowAnnouncementChannelRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "followers"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode FollowedChannel.decoder)

// https://discord.com/developers/docs/resources/channel#trigger-typing-indicator
let triggerTypingIndicator (req: TriggerTypingIndicatorRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "typing"]
    |> Uri.toRequest HttpMethod.Post
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#get-pinned-messages
let getPinnedMessages (req: GetPinnedMessagesRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "pins"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list Message.decoder))

// https://discord.com/developers/docs/resources/channel#pin-message
let pinMessage (req: PinMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "pins"; req.MessageId]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#unpin-message
let unpinMessage (req: UnpinMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "pins"; req.MessageId]
    |> Uri.toRequest HttpMethod.Delete
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#group-dm-add-recipient
let groupDmAddRecipient (req: GroupDmAddRecipientRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "recipients"; req.UserId]
    |> Uri.toRequest HttpMethod.Put
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Channel.decoder)
    
    // TODO: Test this response type to confirm (not documented)

// https://discord.com/developers/docs/resources/channel#group-dm-remove-recipient
let groupDmRemoveRecipient (req: GroupDmRemoveRecipientRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "recipients"; req.UserId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
    
    // TODO: Test this response type to confirm (not documented)

// https://discord.com/developers/docs/resources/channel#start-thread-from-message
let startThreadFromMessage (req: StartThreadFromMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "messages"; req.MessageId; "threads"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Channel.decoder)

// https://discord.com/developers/docs/resources/channel#start-thread-without-message
let startThreadWithoutMessage (req: StartThreadWithoutMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "threads"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Channel.decoder)

// https://discord.com/developers/docs/resources/channel#start-thread-in-forum-or-media-channel
let startThreadInForumOrMediaChannel (req: StartThreadInForumOrMediaChannelRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "threads"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode StartThreadInForumOrMediaChannelResponse.decoder)

// https://discord.com/developers/docs/resources/channel#join-thread
let joinThread (req: JoinThreadRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "thread-members"; "@me"]
    |> Uri.toRequest HttpMethod.Put
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#add-thread-member
let addThreadMember (req: AddThreadMemberRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "thread-members"; req.UserId]
    |> Uri.toRequest HttpMethod.Put
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#leave-thread
let leaveThread (req: LeaveThreadRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "thread-members"; "@me"]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#remove-thread-member
let removeThreadMember (req: RemoveThreadMemberRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "thread-members"; req.UserId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/channel#get-thread-member
let getThreadMember (req: GetThreadMemberRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "thread-members"; req.UserId]
    |> Uri.withOptionalQuery "with_member" (Option.map string req.WithMember)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ThreadMember.decoder)

// https://discord.com/developers/docs/resources/channel#list-thread-members
let listThreadMembers (req: ListThreadMembersRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "thread-members"]
    |> Uri.withOptionalQuery "with_member" (Option.map string req.WithMember)
    |> Uri.withOptionalQuery "after" req.After
    |> Uri.withOptionalQuery "limit" (Option.map string req.Limit)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ThreadMember.decoder))

// https://discord.com/developers/docs/resources/channel#list-public-archived-threads
let listPublicArchivedThreads (req: ListPublicArchivedThreadsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "threads"; "archived"; "public"]
    |> Uri.withOptionalQuery "before" (Option.map _.ToString() req.Before)
    |> Uri.withOptionalQuery "limit" (Option.map string req.Limit)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ArchivedThreadsResponse.decoder)

// https://discord.com/developers/docs/resources/channel#list-private-archived-threads
let listPrivateArchivedThreads (req: ListPrivateArchivedThreadsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "threads"; "archived"; "private"]
    |> Uri.withOptionalQuery "before" (Option.map _.ToString() req.Before)
    |> Uri.withOptionalQuery "limit" (Option.map string req.Limit)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ArchivedThreadsResponse.decoder)

// https://discord.com/developers/docs/resources/channel#list-joined-private-archived-threads
let listJoinedPrivateArchivedThreads (req: ListJoinedPrivateArchivedThreadsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "users"; "@me"; "threads"; "archived"; "private"]
    |> Uri.withOptionalQuery "before" req.Before
    |> Uri.withOptionalQuery "limit" (Option.map string req.Limit)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ArchivedThreadsResponse.decoder)

// ----- Resources: Emoji -----

// https://discord.com/developers/docs/resources/emoji#list-guild-emojis
let listGuildEmojis (req: ListGuildEmojisRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "emojis"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list Emoji.decoder))

// https://discord.com/developers/docs/resources/emoji#get-guild-emoji
let getGuildEmoji (req: GetGuildEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "emojis"; req.EmojiId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Emoji.decoder)

// https://discord.com/developers/docs/resources/emoji#create-guild-emoji
let createGuildEmoji (req: CreateGuildEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "emojis"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Emoji.decoder)

// https://discord.com/developers/docs/resources/emoji#modify-guild-emoji
let modifyGuildEmoji (req: ModifyGuildEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "emojis"; req.EmojiId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Emoji.decoder)

// https://discord.com/developers/docs/resources/emoji#delete-guild-emoji
let deleteGuildEmoji (req: DeleteGuildEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "emojis"; req.EmojiId]
    |> Uri.toRequest HttpMethod.Delete
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/emoji#list-application-emojis
let listApplicationEmojis (req: ListApplicationEmojisRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "emojis"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ListApplicationEmojisResponse.decoder)
    |> Task.map (DiscordResponse.map _.Items)

// https://discord.com/developers/docs/resources/emoji#get-application-emoji
let getApplicationEmoji (req: GetApplicationEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "emojis"; req.EmojiId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Emoji.decoder)

// https://discord.com/developers/docs/resources/emoji#create-application-emoji
let createApplicationEmoji (req: CreateApplicationEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "emojis"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Emoji.decoder)

// https://discord.com/developers/docs/resources/emoji#modify-application-emoji
let modifyApplicationEmoji (req: ModifyApplicationEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "emojis"; req.EmojiId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Emoji.decoder)

// https://discord.com/developers/docs/resources/emoji#delete-application-emoji
let deleteApplicationEmoji (req: DeleteApplicationEmojiRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "emojis"; req.EmojiId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// ----- Resources: Entitlement -----

// https://discord.com/developers/docs/resources/entitlement#list-entitlements
let listEntitlements (req: ListEntitlementsRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "entitlements"]
    |> Uri.withOptionalQuery "user_id" req.UserId
    |> Uri.withOptionalQuery "sku_ids" (Option.map (String.concat ",") req.SkuIds)
    |> Uri.withOptionalQuery "before" req.Before
    |> Uri.withOptionalQuery "after" req.After
    |> Uri.withOptionalQuery "limit" (Option.map string req.Limit)
    |> Uri.withOptionalQuery "guild_id" req.GuildId
    |> Uri.withOptionalQuery "exclude_ended" (Option.map string req.ExcludeEnded)
    |> Uri.withOptionalQuery "exclude_deleted" (Option.map string req.ExcludeDeleted)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list Entitlement.decoder))

// https://discord.com/developers/docs/resources/entitlement#get-entitlement
let getEntitlement (req: GetEntitlementRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "entitlements"; req.EntitlementId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Entitlement.decoder)

// https://discord.com/developers/docs/resources/entitlement#consume-an-entitlement
let consumeEntitlement (req: ConsumeEntitlementRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "entitlements"; req.EntitlementId; "consume"]
    |> Uri.toRequest HttpMethod.Post
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/entitlement#create-test-entitlement
let createTestEntitlement (req: CreateTestEntitlementRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "entitlements"; "test"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Entitlement.decoder)

// https://discord.com/developers/docs/resources/entitlement#delete-test-entitlement
let deleteTestEntitlement (req: DeleteTestEntitlementRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "entitlements"; "test"]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// ----- Resources: Guild -----

// https://discord.com/developers/docs/resources/guild#create-guild
let createGuild (req: CreateGuildRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Guild.decoder)

// https://discord.com/developers/docs/resources/guild#get-guild
let getGuild (req: GetGuildRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId]
    |> Uri.withOptionalQuery "with_counts" (Option.map string req.WithCounts)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Guild.decoder)

// https://discord.com/developers/docs/resources/guild#get-guild-preview
let getGuildPreview (req: GetGuildPreviewRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "preview"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode GuildPreview.decoder)

// https://discord.com/developers/docs/resources/guild#modify-guild
let modifyGuild (req: ModifyGuildRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Guild.decoder)

// https://discord.com/developers/docs/resources/guild#delete-guild
let deleteGuild (req: DeleteGuildRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/guild#get-guild-channels
let getGuildChannels (req: GetGuildChannelsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "channels"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list Channel.decoder))

// https://discord.com/developers/docs/resources/guild#create-guild-channel
let createGuildChannel (req: CreateGuildChannelRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "channels"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Channel.decoder)

// https://discord.com/developers/docs/resources/guild#modify-guild-channel-positions
let modifyGuildChannelPositions (req: ModifyGuildChannelPositionsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "channels"]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withAuditLogReason req.AuditLogReason
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/resources/guild#list-active-guild-threads
let listActiveGuildThreads (req: ListActiveGuildThreadsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "guilds"; req.GuildId; "threads"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ListActiveGuildThreadsResponse.decoder)

// ----- Resources: Guild Scheduled Event -----

// ----- Resources: Guild Template -----

// ----- Resources: Invite -----

// ----- Resources: Lobby -----

// ----- Resources: Message -----

// https://discord.com/developers/docs/resources/message#create-message
let createMessage (req: CreateMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "channels"; req.ChannelId; "messages"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// TODO: All other message endpoints (only implemented this one because it is particularly important)

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
