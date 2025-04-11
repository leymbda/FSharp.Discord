namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type GatewayReceiveEvent =
    | HEARTBEAT
    | HEARTBEAT_ACK
    | HELLO of event: HelloReceiveEvent * sequence: int
    | READY of event: ReadyReceiveEvent * sequence: int
    | RESUMED
    | RECONNECT
    | INVALID_SESSION of bool

module GatewayReceiveEvent =
    let decoder: Decoder<GatewayReceiveEvent> =
        GatewayReceiveEventPayload.decoder
        |> Decode.andThen (fun payload ->
            match payload.Opcode, payload.Data, payload.EventName, payload.Sequence with
            | GatewayOpcode.HEARTBEAT, None, None, None ->
                Decode.succeed GatewayReceiveEvent.HEARTBEAT

            | GatewayOpcode.HEARTBEAT_ACK, None, None, None ->
                Decode.succeed GatewayReceiveEvent.HEARTBEAT_ACK

            | GatewayOpcode.RECONNECT, None, None, None ->
                Decode.succeed GatewayReceiveEvent.RECONNECT

            | GatewayOpcode.INVALID_SESSION, Some (GatewayReceiveEventData.BOOLEAN d), None, None ->
                Decode.succeed (GatewayReceiveEvent.INVALID_SESSION d)

            | GatewayOpcode.DISPATCH, Some data, Some eventName, Some sequence ->
                match eventName, data with
                | nameof HELLO, GatewayReceiveEventData.HELLO d ->
                    Decode.succeed (GatewayReceiveEvent.HELLO (d, sequence))

                | nameof READY, GatewayReceiveEventData.READY d ->
                    Decode.succeed (GatewayReceiveEvent.READY (d, sequence))

                // TODO: Add remaining dispatch events

                | _ ->
                    Decode.fail "Unexpected gateway dispatch event received"

            | _ ->
                Decode.fail "Unexpected gateway payload received"
        )

// TODO: Delete below once all dispatch events copied over to new

type GatewayReceiveEventOld =
    | HEARTBEAT                              of GatewayEventPayload<HeartbeatReceiveEvent>
    | HEARTBEAT_ACK                          of GatewayEventPayload<HeartbeatAckReceiveEvent>
    | HELLO                                  of GatewayEventPayload<HelloReceiveEvent>
    | READY                                  of GatewayEventPayload<ReadyReceiveEvent>
    | RESUMED                                of GatewayEventPayload<ResumedReceiveEvent>
    | RECONNECT                              of GatewayEventPayload<ReconnectReceiveEvent>
    | INVALID_SESSION                        of GatewayEventPayload<InvalidSessionReceiveEvent>
    | APPLICATION_COMMAND_PERMISSIONS_UPDATE of GatewayEventPayload<ApplicationCommandPermissionsUpdateReceiveEvent>
    | AUTO_MODERATION_RULE_CREATE            of GatewayEventPayload<AutoModerationRuleCreateReceiveEvent>
    | AUTO_MODERATION_RULE_UPDATE            of GatewayEventPayload<AutoModerationRuleUpdateReceiveEvent>
    | AUTO_MODERATION_RULE_DELETE            of GatewayEventPayload<AutoModerationRuleDeleteReceiveEvent>
    | AUTO_MODERATION_ACTION_EXECUTION       of GatewayEventPayload<AutoModerationActionExecutionReceiveEvent>
    | CHANNEL_CREATE                         of GatewayEventPayload<ChannelCreateReceiveEvent>
    | CHANNEL_UPDATE                         of GatewayEventPayload<ChannelUpdateReceiveEvent>
    | CHANNEL_DELETE                         of GatewayEventPayload<ChannelDeleteReceiveEvent>
    | THREAD_CREATE                          of GatewayEventPayload<ChannelCreateReceiveEvent>
    | THREAD_UPDATE                          of GatewayEventPayload<ChannelUpdateReceiveEvent>
    | THREAD_DELETE                          of GatewayEventPayload<ThreadDeleteReceiveEvent>
    | THREAD_LIST_SYNC                       of GatewayEventPayload<ThreadListSyncReceiveEvent>
    | ENTITLEMENT_CREATE                     of GatewayEventPayload<EntitlementCreateReceiveEvent>
    | ENTITLEMENT_UPDATE                     of GatewayEventPayload<EntitlementUpdateReceiveEvent>
    | ENTITLEMENT_DELETE                     of GatewayEventPayload<EntitlementDeleteReceiveEvent>
    | GUILD_CREATE                           of GatewayEventPayload<GuildCreateReceiveEvent>
    | GUILD_UPDATE                           of GatewayEventPayload<GuildUpdateReceiveEvent>
    | GUILD_DELETE                           of GatewayEventPayload<GuildDeleteReceiveEvent>
    | GUILD_BAN_ADD                          of GatewayEventPayload<GuildBanAddReceiveEvent>
    | GUILD_BAN_REMOVE                       of GatewayEventPayload<GuildBanRemoveReceiveEvent>
    | GUILD_EMOJIS_UPDATE                    of GatewayEventPayload<GuildEmojisUpdateReceiveEvent>
    | GUILD_STICKERS_UPDATE                  of GatewayEventPayload<GuildStickersUpdateReceiveEvent>
    | GUILD_INTEGRATIONS_UPDATE              of GatewayEventPayload<GuildIntegrationsUpdateReceiveEvent>
    | GUILD_MEMBER_ADD                       of GatewayEventPayload<GuildMemberAddReceiveEvent>
    | GUILD_MEMBER_REMOVE                    of GatewayEventPayload<GuildMemberRemoveReceiveEvent>
    | GUILD_MEMBER_UPDATE                    of GatewayEventPayload<GuildMemberUpdateReceiveEvent>
    | GUILD_MEMBERS_CHUNK                    of GatewayEventPayload<GuildMembersChunkReceiveEvent>
    | GUILD_ROLE_CREATE                      of GatewayEventPayload<GuildRoleCreateReceiveEvent>
    | GUILD_ROLE_UPDATE                      of GatewayEventPayload<GuildRoleUpdateReceiveEvent>
    | GUILD_ROLE_DELETE                      of GatewayEventPayload<GuildRoleDeleteReceiveEvent>
    | GUILD_SCHEDULED_EVENT_CREATE           of GatewayEventPayload<GuildScheduledEventCreateReceiveEvent>
    | GUILD_SCHEDULED_EVENT_UPDATE           of GatewayEventPayload<GuildScheduledEventUpdateReceiveEvent>
    | GUILD_SCHEDULED_EVENT_DELETE           of GatewayEventPayload<GuildScheduledEventDeleteReceiveEvent>
    | GUILD_SCHEDULED_EVENT_USER_ADD         of GatewayEventPayload<GuildScheduledEventUserAddReceiveEvent>
    | GUILD_SCHEDULED_EVENT_USER_REMOVE      of GatewayEventPayload<GuildScheduledEventUserRemoveReceiveEvent>
    | GUILD_SOUNDBOARD_SOUND_CREATE          of GatewayEventPayload<GuildSoundboardSoundCreateReceiveEvent>
    | GUILD_SOUNDBOARD_SOUND_UPDATE          of GatewayEventPayload<GuildSoundboardSoundUpdateReceiveEvent>
    | GUILD_SOUNDBOARD_SOUND_DELETE          of GatewayEventPayload<GuildSoundboardSoundDeleteReceiveEvent>
    | GUILD_SOUNDBOARD_SOUNDS_UPDATE         of GatewayEventPayload<GuildSoundboardSoundsUpdateReceiveEvent>
    | GUILD_SOUNDBOARD_SOUNDS                of GatewayEventPayload<GuildSoundboardSoundsReceiveEvent>
    | INTEGRATION_CREATE                     of GatewayEventPayload<IntegrationCreateReceiveEvent>
    | INTEGRATION_UPDATE                     of GatewayEventPayload<IntegrationUpdateReceiveEvent>
    | INTEGRATION_DELETE                     of GatewayEventPayload<IntegrationDeleteReceiveEvent>
    | INVITE_CREATE                          of GatewayEventPayload<InviteCreateReceiveEvent>
    | INVITE_DELETE                          of GatewayEventPayload<InviteDeleteReceiveEvent>
    | MESSAGE_CREATE                         of GatewayEventPayload<MessageCreateReceiveEvent>
    | MESSAGE_UPDATE                         of GatewayEventPayload<MessageUpdateReceiveEvent>
    | MESSAGE_DELETE                         of GatewayEventPayload<MessageDeleteReceiveEvent>
    | MESSAGE_DELETE_BULK                    of GatewayEventPayload<MessageDeleteBulkReceiveEvent>
    | MESSAGE_REACTION_ADD                   of GatewayEventPayload<MessageReactionAddReceiveEvent>
    | MESSAGE_REACTION_REMOVE                of GatewayEventPayload<MessageReactionRemoveReceiveEvent>
    | MESSAGE_REACTION_REMOVE_ALL            of GatewayEventPayload<MessageReactionRemoveAllReceiveEvent>
    | MESSAGE_REACTION_REMOVE_EMOJI          of GatewayEventPayload<MessageReactionRemoveEmojiReceiveEvent>
    | PRESENCE_UPDATE                        of GatewayEventPayload<PresenceUpdateReceiveEvent>
    | TYPING_START                           of GatewayEventPayload<TypingStartReceiveEvent>
    | USER_UPDATE                            of GatewayEventPayload<UserUpdateReceiveEvent>
    | VOICE_CHANNEL_EFFECT_SEND              of GatewayEventPayload<VoiceChannelEffectSendReceiveEvent>
    | VOICE_STATE_UPDATE                     of GatewayEventPayload<VoiceStateUpdateReceiveEvent>
    | VOICE_SERVER_UPDATE                    of GatewayEventPayload<VoiceServerUpdateReceiveEvent>
    | WEBHOOKS_UPDATE                        of GatewayEventPayload<WebhooksUpdateReceiveEvent>
    | INTERACTION_CREATE                     of GatewayEventPayload<InteractionCreateReceiveEvent>
    | STAGE_INSTANCE_CREATE                  of GatewayEventPayload<StageInstanceCreateReceiveEvent>
    | STAGE_INSTANCE_UPDATE                  of GatewayEventPayload<StageInstanceUpdateReceiveEvent>
    | STAGE_INSTANCE_DELETE                  of GatewayEventPayload<StageInstanceDeleteReceiveEvent>
    | SUBSCRIPTION_CREATE                    of GatewayEventPayload<SubscriptionCreateReceiveEvent>
    | SUBSCRIPTION_UPDATE                    of GatewayEventPayload<SubscriptionUpdateReceiveEvent>
    | SUBSCRIPTION_DELETE                    of GatewayEventPayload<SubscriptionDeleteReceiveEvent>
    | MESSAGE_POLL_VOTE_ADD                  of GatewayEventPayload<MessagePollVoteAddReceiveEvent>
    | MESSAGE_POLL_VOTE_REMOVE               of GatewayEventPayload<MessagePollVoteRemoveReceiveEvent>
    | UNKNOWN                                of GatewayEventPayload<obj>
