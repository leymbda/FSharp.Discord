namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type GatewayReceiveEvent =
    | HEARTBEAT
    | HEARTBEAT_ACK
    | HELLO                                  of HelloReceiveEvent
    | READY                                  of ReadyReceiveEvent * sequence: int
    | RESUMED
    | RECONNECT
    | INVALID_SESSION                        of bool
    | APPLICATION_COMMAND_PERMISSIONS_UPDATE of ApplicationCommandPermission * sequence: int
    | AUTO_MODERATION_RULE_CREATE            of AutoModerationRule * sequence: int
    | AUTO_MODERATION_RULE_UPDATE            of AutoModerationRule * sequence: int
    | AUTO_MODERATION_RULE_DELETE            of AutoModerationRule * sequence: int
    | AUTO_MODERATION_ACTION_EXECUTION       of AutoModerationActionExecutionReceiveEvent * sequence: int
    | CHANNEL_CREATE                         of Channel * sequence: int
    | CHANNEL_UPDATE                         of Channel * sequence: int
    | CHANNEL_DELETE                         of Channel * sequence: int
    | CHANNEL_PINS_UPDATE                    of ChannelPinsUpdateReceiveEvent * sequence: int
    | THREAD_CREATE                          of ThreadCreateReceiveEvent * sequence: int
    | THREAD_UPDATE                          of Channel * sequence: int
    | THREAD_DELETE                          of ThreadDeleteReceiveEvent * sequence: int
    | THREAD_LIST_SYNC                       of ThreadListSyncReceiveEvent * sequence: int
    | THREAD_MEMBER_UPDATE                   of ThreadMemberUpdateReceiveEvent * sequence: int
    | THREAD_MEMBERS_UPDATE                  of ThreadMembersUpdateReceiveEvent * sequence: int
    | ENTITLEMENT_CREATE                     of Entitlement * sequence: int
    | ENTITLEMENT_UPDATE                     of Entitlement * sequence: int
    | ENTITLEMENT_DELETE                     of Entitlement * sequence: int
    | GUILD_CREATE                           of GuildCreateReceiveEvent * sequence: int
    | GUILD_UPDATE                           of Guild * sequence: int
    | GUILD_DELETE                           of GuildDeleteReceiveEvent * sequence: int
    | GUILD_AUDIT_LOG_ENTRY_CREATE           of GuildAuditLogEntryCreateReceiveEvent * sequence: int
    | GUILD_BAN_ADD                          of GuildUserReceiveEvent * sequence: int
    | GUILD_BAN_REMOVE                       of GuildUserReceiveEvent * sequence: int
    | GUILD_EMOJIS_UPDATE                    of GuildEmojisUpdateReceiveEvent * sequence: int
    | GUILD_STICKERS_UPDATE                  of GuildStickersUpdateReceiveEvent * sequence: int
    | GUILD_INTEGRATIONS_UPDATE              of GuildIntegrationsUpdateReceiveEvent * sequence: int
    | GUILD_MEMBER_ADD                       of GuildMemberAddReceiveEvent * sequence: int
    | GUILD_MEMBER_REMOVE                    of GuildUserReceiveEvent * sequence: int
    | GUILD_MEMBER_UPDATE                    of GuildMemberUpdateReceiveEvent * sequence: int
    | GUILD_MEMBERS_CHUNK                    of GuildMembersChunkReceiveEvent * sequence: int
    | GUILD_ROLE_CREATE                      of GuildRoleReceiveEvent * sequence: int
    | GUILD_ROLE_UPDATE                      of GuildRoleReceiveEvent * sequence: int
    | GUILD_ROLE_DELETE                      of GuildRoleDeleteReceiveEvent * sequence: int
    | GUILD_SCHEDULED_EVENT_CREATE           of GuildScheduledEvent * sequence: int
    | GUILD_SCHEDULED_EVENT_UPDATE           of GuildScheduledEvent * sequence: int
    | GUILD_SCHEDULED_EVENT_DELETE           of GuildScheduledEvent * sequence: int
    | GUILD_SCHEDULED_EVENT_USER_ADD         of GuildScheduledEventUserReceiveEvent * sequence: int
    | GUILD_SCHEDULED_EVENT_USER_REMOVE      of GuildScheduledEventUserReceiveEvent * sequence: int
    | GUILD_SOUNDBOARD_SOUND_CREATE          of SoundboardSound * sequence: int
    | GUILD_SOUNDBOARD_SOUND_UPDATE          of SoundboardSound * sequence: int
    | GUILD_SOUNDBOARD_SOUND_DELETE          of GuildSoundboardSoundDeleteReceiveEvent * sequence: int
    | GUILD_SOUNDBOARD_SOUNDS_UPDATE         of GuildSoundboardSoundsReceiveEvent * sequence: int
    | GUILD_SOUNDBOARD_SOUNDS                of GuildSoundboardSoundsReceiveEvent * sequence: int
    | INTEGRATION_CREATE                     of IntegrationReceiveEvent * sequence: int
    | INTEGRATION_UPDATE                     of IntegrationReceiveEvent * sequence: int
    | INTEGRATION_DELETE                     of IntegrationDeleteReceiveEvent * sequence: int
    | INVITE_CREATE                          of InviteCreateReceiveEvent * sequence: int
    | INVITE_DELETE                          of InviteDeleteReceiveEvent * sequence: int
    | MESSAGE_CREATE                         of MessageReceiveEvent * sequence: int
    | MESSAGE_UPDATE                         of MessageReceiveEvent * sequence: int
    | MESSAGE_DELETE                         of MessageDeleteReceiveEvent * sequence: int
    | MESSAGE_DELETE_BULK                    of MessageDeleteBulkReceiveEvent * sequence: int
    | MESSAGE_REACTION_ADD                   of MessageReactionAddReceiveEvent * sequence: int
    | MESSAGE_REACTION_REMOVE                of MessageReactionRemoveReceiveEvent * sequence: int
    | MESSAGE_REACTION_REMOVE_ALL            of MessageReactionRemoveAllReceiveEvent * sequence: int
    | MESSAGE_REACTION_REMOVE_EMOJI          of MessageReactionRemoveEmojiReceiveEvent * sequence: int
    | PRESENCE_UPDATE                        of PresenceUpdateReceiveEvent * sequence: int
    | TYPING_START                           of TypingStartReceiveEvent * sequence: int
    | USER_UPDATE                            of User * sequence: int
    | VOICE_CHANNEL_EFFECT_SEND              of VoiceChannelEffectSendReceiveEvent * sequence: int
    | VOICE_STATE_UPDATE                     of VoiceState * sequence: int
    | VOICE_SERVER_UPDATE                    of VoiceServerUpdateReceiveEvent * sequence: int
    | WEBHOOKS_UPDATE                        of WebhooksUpdateReceiveEvent * sequence: int
    | INTERACTION_CREATE                     of Interaction * sequence: int
    | STAGE_INSTANCE_CREATE                  of StageInstance * sequence: int
    | STAGE_INSTANCE_UPDATE                  of StageInstance * sequence: int
    | STAGE_INSTANCE_DELETE                  of StageInstance * sequence: int
    | SUBSCRIPTION_CREATE                    of Subscription * sequence: int
    | SUBSCRIPTION_UPDATE                    of Subscription * sequence: int
    | SUBSCRIPTION_DELETE                    of Subscription * sequence: int
    | MESSAGE_POLL_VOTE_ADD                  of MessagePollVoteReceiveEvent * sequence: int
    | MESSAGE_POLL_VOTE_REMOVE               of MessagePollVoteReceiveEvent * sequence: int

module GatewayReceiveEvent =
    let decoder: Decoder<GatewayReceiveEvent> =
        GatewayReceiveEventPayload.decoder
        |> Decode.andThen (fun payload ->
            match payload.Opcode, payload.Data, payload.EventName, payload.Sequence with
            | GatewayOpcode.HEARTBEAT, None, None, None ->
                Decode.succeed GatewayReceiveEvent.HEARTBEAT

            | GatewayOpcode.HEARTBEAT_ACK, None, None, None ->
                Decode.succeed GatewayReceiveEvent.HEARTBEAT_ACK

            | GatewayOpcode.HELLO, Some (GatewayReceiveEventData.HELLO d), None, None ->
                Decode.succeed (GatewayReceiveEvent.HELLO d)

            | GatewayOpcode.RECONNECT, None, None, None ->
                Decode.succeed GatewayReceiveEvent.RECONNECT

            | GatewayOpcode.INVALID_SESSION, Some (GatewayReceiveEventData.BOOLEAN d), None, None ->
                Decode.succeed (GatewayReceiveEvent.INVALID_SESSION d)

            | GatewayOpcode.DISPATCH, Some data, Some eventName, Some sequence ->
                match eventName, data with
                | nameof READY, GatewayReceiveEventData.READY d ->
                    Decode.succeed (GatewayReceiveEvent.READY (d, sequence))

                // TODO: Add remaining dispatch events

                | _ ->
                    Decode.fail "Unexpected gateway dispatch event received"

            | _ ->
                Decode.fail "Unexpected gateway payload received"
        )
