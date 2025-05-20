namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type GatewayReceiveEvent =
    | HEARTBEAT
    | HEARTBEAT_ACK
    | HELLO                                  of HelloReceiveEvent
    | READY                                  of ReadyReceiveEvent                         * sequence: int
    | RESUMED
    | RECONNECT
    | INVALID_SESSION                        of bool
    | APPLICATION_COMMAND_PERMISSIONS_UPDATE of ApplicationCommandPermission              * sequence: int
    | AUTO_MODERATION_RULE_CREATE            of AutoModerationRule                        * sequence: int
    | AUTO_MODERATION_RULE_UPDATE            of AutoModerationRule                        * sequence: int
    | AUTO_MODERATION_RULE_DELETE            of AutoModerationRule                        * sequence: int
    | AUTO_MODERATION_ACTION_EXECUTION       of AutoModerationActionExecutionReceiveEvent * sequence: int
    | CHANNEL_CREATE                         of Channel                                   * sequence: int
    | CHANNEL_UPDATE                         of Channel                                   * sequence: int
    | CHANNEL_DELETE                         of Channel                                   * sequence: int
    | CHANNEL_PINS_UPDATE                    of ChannelPinsUpdateReceiveEvent             * sequence: int
    | THREAD_CREATE                          of ThreadCreateReceiveEvent                  * sequence: int
    | THREAD_UPDATE                          of Channel                                   * sequence: int
    | THREAD_DELETE                          of ThreadDeleteReceiveEvent                  * sequence: int
    | THREAD_LIST_SYNC                       of ThreadListSyncReceiveEvent                * sequence: int
    | THREAD_MEMBER_UPDATE                   of ThreadMemberUpdateReceiveEvent            * sequence: int
    | THREAD_MEMBERS_UPDATE                  of ThreadMembersUpdateReceiveEvent           * sequence: int
    | ENTITLEMENT_CREATE                     of Entitlement                               * sequence: int
    | ENTITLEMENT_UPDATE                     of Entitlement                               * sequence: int
    | ENTITLEMENT_DELETE                     of Entitlement                               * sequence: int
    | GUILD_CREATE                           of GuildCreateReceiveEvent                   * sequence: int
    | GUILD_UPDATE                           of Guild                                     * sequence: int
    | GUILD_DELETE                           of GuildDeleteReceiveEvent                   * sequence: int
    | GUILD_AUDIT_LOG_ENTRY_CREATE           of GuildAuditLogEntryCreateReceiveEvent      * sequence: int
    | GUILD_BAN_ADD                          of GuildUserReceiveEvent                     * sequence: int
    | GUILD_BAN_REMOVE                       of GuildUserReceiveEvent                     * sequence: int
    | GUILD_EMOJIS_UPDATE                    of GuildEmojisUpdateReceiveEvent             * sequence: int
    | GUILD_STICKERS_UPDATE                  of GuildStickersUpdateReceiveEvent           * sequence: int
    | GUILD_INTEGRATIONS_UPDATE              of GuildIntegrationsUpdateReceiveEvent       * sequence: int
    | GUILD_MEMBER_ADD                       of GuildMemberAddReceiveEvent                * sequence: int
    | GUILD_MEMBER_REMOVE                    of GuildUserReceiveEvent                     * sequence: int
    | GUILD_MEMBER_UPDATE                    of GuildMemberUpdateReceiveEvent             * sequence: int
    | GUILD_MEMBERS_CHUNK                    of GuildMembersChunkReceiveEvent             * sequence: int
    | GUILD_ROLE_CREATE                      of GuildRoleReceiveEvent                     * sequence: int
    | GUILD_ROLE_UPDATE                      of GuildRoleReceiveEvent                     * sequence: int
    | GUILD_ROLE_DELETE                      of GuildRoleDeleteReceiveEvent               * sequence: int
    | GUILD_SCHEDULED_EVENT_CREATE           of GuildScheduledEvent                       * sequence: int
    | GUILD_SCHEDULED_EVENT_UPDATE           of GuildScheduledEvent                       * sequence: int
    | GUILD_SCHEDULED_EVENT_DELETE           of GuildScheduledEvent                       * sequence: int
    | GUILD_SCHEDULED_EVENT_USER_ADD         of GuildScheduledEventUserReceiveEvent       * sequence: int
    | GUILD_SCHEDULED_EVENT_USER_REMOVE      of GuildScheduledEventUserReceiveEvent       * sequence: int
    | GUILD_SOUNDBOARD_SOUND_CREATE          of SoundboardSound                           * sequence: int
    | GUILD_SOUNDBOARD_SOUND_UPDATE          of SoundboardSound                           * sequence: int
    | GUILD_SOUNDBOARD_SOUND_DELETE          of GuildSoundboardSoundDeleteReceiveEvent    * sequence: int
    | GUILD_SOUNDBOARD_SOUNDS_UPDATE         of GuildSoundboardSoundsReceiveEvent         * sequence: int
    | GUILD_SOUNDBOARD_SOUNDS                of GuildSoundboardSoundsReceiveEvent         * sequence: int
    | INTEGRATION_CREATE                     of IntegrationReceiveEvent                   * sequence: int
    | INTEGRATION_UPDATE                     of IntegrationReceiveEvent                   * sequence: int
    | INTEGRATION_DELETE                     of IntegrationDeleteReceiveEvent             * sequence: int
    | INVITE_CREATE                          of InviteCreateReceiveEvent                  * sequence: int
    | INVITE_DELETE                          of InviteDeleteReceiveEvent                  * sequence: int
    | MESSAGE_CREATE                         of MessageReceiveEvent                       * sequence: int
    | MESSAGE_UPDATE                         of MessageReceiveEvent                       * sequence: int
    | MESSAGE_DELETE                         of MessageDeleteReceiveEvent                 * sequence: int
    | MESSAGE_DELETE_BULK                    of MessageDeleteBulkReceiveEvent             * sequence: int
    | MESSAGE_REACTION_ADD                   of MessageReactionAddReceiveEvent            * sequence: int
    | MESSAGE_REACTION_REMOVE                of MessageReactionRemoveReceiveEvent         * sequence: int
    | MESSAGE_REACTION_REMOVE_ALL            of MessageReactionRemoveAllReceiveEvent      * sequence: int
    | MESSAGE_REACTION_REMOVE_EMOJI          of MessageReactionRemoveEmojiReceiveEvent    * sequence: int
    | PRESENCE_UPDATE                        of PresenceUpdateReceiveEvent                * sequence: int
    | TYPING_START                           of TypingStartReceiveEvent                   * sequence: int
    | USER_UPDATE                            of User                                      * sequence: int
    | VOICE_CHANNEL_EFFECT_SEND              of VoiceChannelEffectSendReceiveEvent        * sequence: int
    | VOICE_STATE_UPDATE                     of VoiceState                                * sequence: int
    | VOICE_SERVER_UPDATE                    of VoiceServerUpdateReceiveEvent             * sequence: int
    | WEBHOOKS_UPDATE                        of WebhooksUpdateReceiveEvent                * sequence: int
    | INTERACTION_CREATE                     of Interaction                               * sequence: int
    | STAGE_INSTANCE_CREATE                  of StageInstance                             * sequence: int
    | STAGE_INSTANCE_UPDATE                  of StageInstance                             * sequence: int
    | STAGE_INSTANCE_DELETE                  of StageInstance                             * sequence: int
    | SUBSCRIPTION_CREATE                    of Subscription                              * sequence: int
    | SUBSCRIPTION_UPDATE                    of Subscription                              * sequence: int
    | SUBSCRIPTION_DELETE                    of Subscription                              * sequence: int
    | MESSAGE_POLL_VOTE_ADD                  of MessagePollVoteReceiveEvent               * sequence: int
    | MESSAGE_POLL_VOTE_REMOVE               of MessagePollVoteReceiveEvent               * sequence: int

module GatewayReceiveEvent =
    let decoder: Decoder<GatewayReceiveEvent> =
        GatewayReceiveEventPayload.decoder
        |> Decode.andThen (fun payload ->
            match payload.Opcode, payload.Data, payload.EventName, payload.Sequence with
            | GatewayOpcode.HEARTBEAT, None, None, None -> Decode.succeed GatewayReceiveEvent.HEARTBEAT
            | GatewayOpcode.HEARTBEAT_ACK, None, None, None -> Decode.succeed GatewayReceiveEvent.HEARTBEAT_ACK
            | GatewayOpcode.HELLO, Some (GatewayReceiveEventData.Hello d), None, None -> Decode.succeed (GatewayReceiveEvent.HELLO d)
            | GatewayOpcode.RESUME, None, None, None -> Decode.succeed GatewayReceiveEvent.RESUMED
            | GatewayOpcode.RECONNECT, None, None, None -> Decode.succeed GatewayReceiveEvent.RECONNECT
            | GatewayOpcode.INVALID_SESSION, Some (GatewayReceiveEventData.Boolean d), None, None -> Decode.succeed (GatewayReceiveEvent.INVALID_SESSION d)
            | GatewayOpcode.DISPATCH, Some data, Some eventName, Some sequence ->
                match eventName, data with
                | nameof READY, GatewayReceiveEventData.Ready d -> Decode.succeed (GatewayReceiveEvent.READY (d, sequence))
                | nameof APPLICATION_COMMAND_PERMISSIONS_UPDATE, GatewayReceiveEventData.ApplicationCommandPermission d -> Decode.succeed (GatewayReceiveEvent.APPLICATION_COMMAND_PERMISSIONS_UPDATE (d, sequence))
                | nameof AUTO_MODERATION_RULE_CREATE, GatewayReceiveEventData.AutoModerationRule d -> Decode.succeed (GatewayReceiveEvent.AUTO_MODERATION_RULE_CREATE (d, sequence))
                | nameof AUTO_MODERATION_RULE_UPDATE, GatewayReceiveEventData.AutoModerationRule d -> Decode.succeed (GatewayReceiveEvent.AUTO_MODERATION_RULE_UPDATE (d, sequence))
                | nameof AUTO_MODERATION_RULE_DELETE, GatewayReceiveEventData.AutoModerationRule d -> Decode.succeed (GatewayReceiveEvent.AUTO_MODERATION_RULE_DELETE (d, sequence))
                | nameof AUTO_MODERATION_ACTION_EXECUTION, GatewayReceiveEventData.AutoModerationActionExecution d -> Decode.succeed (GatewayReceiveEvent.AUTO_MODERATION_ACTION_EXECUTION (d, sequence))
                | nameof CHANNEL_CREATE, GatewayReceiveEventData.Channel d -> Decode.succeed (GatewayReceiveEvent.CHANNEL_CREATE (d, sequence))
                | nameof CHANNEL_UPDATE, GatewayReceiveEventData.Channel d -> Decode.succeed (GatewayReceiveEvent.CHANNEL_UPDATE (d, sequence))
                | nameof CHANNEL_DELETE, GatewayReceiveEventData.Channel d -> Decode.succeed (GatewayReceiveEvent.CHANNEL_DELETE (d, sequence))
                | nameof CHANNEL_PINS_UPDATE, GatewayReceiveEventData.ChannelPinsUpdate d -> Decode.succeed (GatewayReceiveEvent.CHANNEL_PINS_UPDATE (d, sequence))
                | nameof THREAD_CREATE, GatewayReceiveEventData.ThreadCreate d -> Decode.succeed (GatewayReceiveEvent.THREAD_CREATE (d, sequence))
                | nameof THREAD_UPDATE, GatewayReceiveEventData.Channel d -> Decode.succeed (GatewayReceiveEvent.THREAD_UPDATE (d, sequence))
                | nameof THREAD_DELETE, GatewayReceiveEventData.ThreadDelete d -> Decode.succeed (GatewayReceiveEvent.THREAD_DELETE (d, sequence))
                | nameof THREAD_LIST_SYNC, GatewayReceiveEventData.ThreadListSync d -> Decode.succeed (GatewayReceiveEvent.THREAD_LIST_SYNC (d, sequence))
                | nameof THREAD_MEMBER_UPDATE, GatewayReceiveEventData.ThreadMemberUpdate d -> Decode.succeed (GatewayReceiveEvent.THREAD_MEMBER_UPDATE (d, sequence))
                | nameof THREAD_MEMBERS_UPDATE, GatewayReceiveEventData.ThreadMembersUpdate d -> Decode.succeed (GatewayReceiveEvent.THREAD_MEMBERS_UPDATE (d, sequence))
                | nameof ENTITLEMENT_CREATE, GatewayReceiveEventData.Entitlement d -> Decode.succeed (GatewayReceiveEvent.ENTITLEMENT_CREATE (d, sequence))
                | nameof ENTITLEMENT_UPDATE, GatewayReceiveEventData.Entitlement d -> Decode.succeed (GatewayReceiveEvent.ENTITLEMENT_UPDATE (d, sequence))
                | nameof ENTITLEMENT_DELETE, GatewayReceiveEventData.Entitlement d -> Decode.succeed (GatewayReceiveEvent.ENTITLEMENT_DELETE (d, sequence))
                | nameof GUILD_CREATE, GatewayReceiveEventData.GuildCreate d -> Decode.succeed (GatewayReceiveEvent.GUILD_CREATE (d, sequence))
                | nameof GUILD_UPDATE, GatewayReceiveEventData.Guild d -> Decode.succeed (GatewayReceiveEvent.GUILD_UPDATE (d, sequence))
                | nameof GUILD_DELETE, GatewayReceiveEventData.GuildDelete d -> Decode.succeed (GatewayReceiveEvent.GUILD_DELETE (d, sequence))
                | nameof GUILD_AUDIT_LOG_ENTRY_CREATE, GatewayReceiveEventData.GuildAuditLogEntryCreate d -> Decode.succeed (GatewayReceiveEvent.GUILD_AUDIT_LOG_ENTRY_CREATE (d, sequence))
                | nameof GUILD_BAN_ADD, GatewayReceiveEventData.GuildUser d -> Decode.succeed (GatewayReceiveEvent.GUILD_BAN_ADD (d, sequence))
                | nameof GUILD_BAN_REMOVE, GatewayReceiveEventData.GuildUser d -> Decode.succeed (GatewayReceiveEvent.GUILD_BAN_REMOVE (d, sequence))
                | nameof GUILD_EMOJIS_UPDATE, GatewayReceiveEventData.GuildEmojisUpdate d -> Decode.succeed (GatewayReceiveEvent.GUILD_EMOJIS_UPDATE (d, sequence))
                | nameof GUILD_STICKERS_UPDATE, GatewayReceiveEventData.GuildStickersUpdate d -> Decode.succeed (GatewayReceiveEvent.GUILD_STICKERS_UPDATE (d, sequence))
                | nameof GUILD_INTEGRATIONS_UPDATE, GatewayReceiveEventData.GuildIntegrationsUpdate d -> Decode.succeed (GatewayReceiveEvent.GUILD_INTEGRATIONS_UPDATE (d, sequence))
                | nameof GUILD_MEMBER_ADD, GatewayReceiveEventData.GuildMemberAdd d -> Decode.succeed (GatewayReceiveEvent.GUILD_MEMBER_ADD (d, sequence))
                | nameof GUILD_MEMBER_REMOVE, GatewayReceiveEventData.GuildUser d -> Decode.succeed (GatewayReceiveEvent.GUILD_MEMBER_REMOVE (d, sequence))
                | nameof GUILD_MEMBER_UPDATE, GatewayReceiveEventData.GuildMemberUpdate d -> Decode.succeed (GatewayReceiveEvent.GUILD_MEMBER_UPDATE (d, sequence))
                | nameof GUILD_MEMBERS_CHUNK, GatewayReceiveEventData.GuildMembersChunk d -> Decode.succeed (GatewayReceiveEvent.GUILD_MEMBERS_CHUNK (d, sequence))
                | nameof GUILD_ROLE_CREATE, GatewayReceiveEventData.GuildRole d -> Decode.succeed (GatewayReceiveEvent.GUILD_ROLE_CREATE (d, sequence))
                | nameof GUILD_ROLE_UPDATE, GatewayReceiveEventData.GuildRole d -> Decode.succeed (GatewayReceiveEvent.GUILD_ROLE_UPDATE (d, sequence))
                | nameof GUILD_ROLE_DELETE, GatewayReceiveEventData.GuildRoleDelete d -> Decode.succeed (GatewayReceiveEvent.GUILD_ROLE_DELETE (d, sequence))
                | nameof GUILD_SCHEDULED_EVENT_CREATE, GatewayReceiveEventData.GuildScheduledEvent d -> Decode.succeed (GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_CREATE (d, sequence))
                | nameof GUILD_SCHEDULED_EVENT_UPDATE, GatewayReceiveEventData.GuildScheduledEvent d -> Decode.succeed (GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_UPDATE (d, sequence))
                | nameof GUILD_SCHEDULED_EVENT_DELETE, GatewayReceiveEventData.GuildScheduledEvent d -> Decode.succeed (GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_DELETE (d, sequence))
                | nameof GUILD_SCHEDULED_EVENT_USER_ADD, GatewayReceiveEventData.GuildScheduledEventUser d -> Decode.succeed (GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_USER_ADD (d, sequence))
                | nameof GUILD_SCHEDULED_EVENT_USER_REMOVE, GatewayReceiveEventData.GuildScheduledEventUser d -> Decode.succeed (GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_USER_REMOVE (d, sequence))
                | nameof GUILD_SOUNDBOARD_SOUND_CREATE, GatewayReceiveEventData.SoundboardSound d -> Decode.succeed (GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUND_CREATE (d, sequence))
                | nameof GUILD_SOUNDBOARD_SOUND_UPDATE, GatewayReceiveEventData.SoundboardSound d -> Decode.succeed (GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUND_UPDATE (d, sequence))
                | nameof GUILD_SOUNDBOARD_SOUND_DELETE, GatewayReceiveEventData.GuildSoundboardSoundDelete d -> Decode.succeed (GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUND_DELETE (d, sequence))
                | nameof GUILD_SOUNDBOARD_SOUNDS_UPDATE, GatewayReceiveEventData.GuildSoundboardSounds d -> Decode.succeed (GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUNDS_UPDATE (d, sequence))
                | nameof GUILD_SOUNDBOARD_SOUNDS, GatewayReceiveEventData.GuildSoundboardSounds d -> Decode.succeed (GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUNDS (d, sequence))
                | nameof INTEGRATION_CREATE, GatewayReceiveEventData.Integration d -> Decode.succeed (GatewayReceiveEvent.INTEGRATION_CREATE (d, sequence))
                | nameof INTEGRATION_UPDATE, GatewayReceiveEventData.Integration d -> Decode.succeed (GatewayReceiveEvent.INTEGRATION_UPDATE (d, sequence))
                | nameof INTEGRATION_DELETE, GatewayReceiveEventData.IntegrationDelete d -> Decode.succeed (GatewayReceiveEvent.INTEGRATION_DELETE (d, sequence))
                | nameof INVITE_CREATE, GatewayReceiveEventData.InviteCreate d -> Decode.succeed (GatewayReceiveEvent.INVITE_CREATE (d, sequence))
                | nameof INVITE_DELETE, GatewayReceiveEventData.InviteDelete d -> Decode.succeed (GatewayReceiveEvent.INVITE_DELETE (d, sequence))
                | nameof MESSAGE_CREATE, GatewayReceiveEventData.Message d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_CREATE (d, sequence))
                | nameof MESSAGE_UPDATE, GatewayReceiveEventData.Message d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_UPDATE (d, sequence))
                | nameof MESSAGE_DELETE, GatewayReceiveEventData.MessageDelete d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_DELETE (d, sequence))
                | nameof MESSAGE_DELETE_BULK, GatewayReceiveEventData.MessageDeleteBulk d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_DELETE_BULK (d, sequence))
                | nameof MESSAGE_REACTION_ADD, GatewayReceiveEventData.MessageReactionAdd d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_REACTION_ADD (d, sequence))
                | nameof MESSAGE_REACTION_REMOVE, GatewayReceiveEventData.MessageReactionRemove d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_REACTION_REMOVE (d, sequence))
                | nameof MESSAGE_REACTION_REMOVE_ALL, GatewayReceiveEventData.MessageReactionRemoveAll d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_REACTION_REMOVE_ALL (d, sequence))
                | nameof MESSAGE_REACTION_REMOVE_EMOJI, GatewayReceiveEventData.MessageReactionRemoveEmoji d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_REACTION_REMOVE_EMOJI (d, sequence))
                | nameof PRESENCE_UPDATE, GatewayReceiveEventData.PresenceUpdate d -> Decode.succeed (GatewayReceiveEvent.PRESENCE_UPDATE (d, sequence))
                | nameof TYPING_START, GatewayReceiveEventData.TypingStart d -> Decode.succeed (GatewayReceiveEvent.TYPING_START (d, sequence))
                | nameof USER_UPDATE, GatewayReceiveEventData.User d -> Decode.succeed (GatewayReceiveEvent.USER_UPDATE (d, sequence))
                | nameof VOICE_CHANNEL_EFFECT_SEND, GatewayReceiveEventData.VoiceChannelEffectSend d -> Decode.succeed (GatewayReceiveEvent.VOICE_CHANNEL_EFFECT_SEND (d, sequence))
                | nameof VOICE_STATE_UPDATE, GatewayReceiveEventData.VoiceState d -> Decode.succeed (GatewayReceiveEvent.VOICE_STATE_UPDATE (d, sequence))
                | nameof VOICE_SERVER_UPDATE, GatewayReceiveEventData.VoiceServerUpdate d -> Decode.succeed (GatewayReceiveEvent.VOICE_SERVER_UPDATE (d, sequence))
                | nameof WEBHOOKS_UPDATE, GatewayReceiveEventData.WebhooksUpdate d -> Decode.succeed (GatewayReceiveEvent.WEBHOOKS_UPDATE (d, sequence))
                | nameof INTERACTION_CREATE, GatewayReceiveEventData.Interaction d -> Decode.succeed (GatewayReceiveEvent.INTERACTION_CREATE (d, sequence))
                | nameof STAGE_INSTANCE_CREATE, GatewayReceiveEventData.StageInstance d -> Decode.succeed (GatewayReceiveEvent.STAGE_INSTANCE_CREATE (d, sequence))
                | nameof STAGE_INSTANCE_UPDATE, GatewayReceiveEventData.StageInstance d -> Decode.succeed (GatewayReceiveEvent.STAGE_INSTANCE_UPDATE (d, sequence))
                | nameof STAGE_INSTANCE_DELETE, GatewayReceiveEventData.StageInstance d -> Decode.succeed (GatewayReceiveEvent.STAGE_INSTANCE_DELETE (d, sequence))
                | nameof SUBSCRIPTION_CREATE, GatewayReceiveEventData.Subscription d -> Decode.succeed (GatewayReceiveEvent.SUBSCRIPTION_CREATE (d, sequence))
                | nameof SUBSCRIPTION_UPDATE, GatewayReceiveEventData.Subscription d -> Decode.succeed (GatewayReceiveEvent.SUBSCRIPTION_UPDATE (d, sequence))
                | nameof SUBSCRIPTION_DELETE, GatewayReceiveEventData.Subscription d -> Decode.succeed (GatewayReceiveEvent.SUBSCRIPTION_DELETE (d, sequence))
                | nameof MESSAGE_POLL_VOTE_ADD, GatewayReceiveEventData.MessagePollVote d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_POLL_VOTE_ADD (d, sequence))
                | nameof MESSAGE_POLL_VOTE_REMOVE, GatewayReceiveEventData.MessagePollVote d -> Decode.succeed (GatewayReceiveEvent.MESSAGE_POLL_VOTE_REMOVE (d, sequence))
                | _ -> Decode.fail "Unexpected gateway dispatch event received"
            | _ -> Decode.fail "Unexpected gateway payload received"
        )

    let encoder (v: GatewayReceiveEvent) =
        match v with
        | GatewayReceiveEvent.HEARTBEAT -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.HEARTBEAT; EventName = None; Sequence = None; Data = None }
        | GatewayReceiveEvent.HEARTBEAT_ACK -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.HEARTBEAT_ACK; EventName = None; Sequence = None; Data = None }
        | GatewayReceiveEvent.HELLO ev -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.HELLO; EventName = None; Sequence = None; Data = Some (GatewayReceiveEventData.Hello ev) }
        | GatewayReceiveEvent.RESUMED -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.RESUME; EventName = None; Sequence = None; Data = None }
        | GatewayReceiveEvent.RECONNECT -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.RECONNECT; EventName = None; Sequence = None; Data = None }
        | GatewayReceiveEvent.INVALID_SESSION ev -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.INVALID_SESSION; EventName = None; Sequence = None; Data = Some (GatewayReceiveEventData.Boolean ev) }
        | GatewayReceiveEvent.READY (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof READY); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Ready ev) }
        | GatewayReceiveEvent.APPLICATION_COMMAND_PERMISSIONS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof APPLICATION_COMMAND_PERMISSIONS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.ApplicationCommandPermission ev) }
        | GatewayReceiveEvent.AUTO_MODERATION_RULE_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof AUTO_MODERATION_RULE_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.AutoModerationRule ev) }
        | GatewayReceiveEvent.AUTO_MODERATION_RULE_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof AUTO_MODERATION_RULE_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.AutoModerationRule ev) }
        | GatewayReceiveEvent.AUTO_MODERATION_RULE_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof AUTO_MODERATION_RULE_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.AutoModerationRule ev) }
        | GatewayReceiveEvent.AUTO_MODERATION_ACTION_EXECUTION (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof AUTO_MODERATION_ACTION_EXECUTION); Sequence = Some seq; Data = Some (GatewayReceiveEventData.AutoModerationActionExecution ev) }
        | GatewayReceiveEvent.CHANNEL_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof CHANNEL_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Channel ev) }
        | GatewayReceiveEvent.CHANNEL_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof CHANNEL_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Channel ev) }
        | GatewayReceiveEvent.CHANNEL_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof CHANNEL_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Channel ev) }
        | GatewayReceiveEvent.CHANNEL_PINS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof CHANNEL_PINS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.ChannelPinsUpdate ev) }
        | GatewayReceiveEvent.THREAD_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof THREAD_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.ThreadCreate ev) }
        | GatewayReceiveEvent.THREAD_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof THREAD_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Channel ev) }
        | GatewayReceiveEvent.THREAD_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof THREAD_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.ThreadDelete ev) }
        | GatewayReceiveEvent.THREAD_LIST_SYNC (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof THREAD_LIST_SYNC); Sequence = Some seq; Data = Some (GatewayReceiveEventData.ThreadListSync ev) }
        | GatewayReceiveEvent.THREAD_MEMBER_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof THREAD_MEMBER_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.ThreadMemberUpdate ev) }
        | GatewayReceiveEvent.THREAD_MEMBERS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof THREAD_MEMBERS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.ThreadMembersUpdate ev) }
        | GatewayReceiveEvent.ENTITLEMENT_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof ENTITLEMENT_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Entitlement ev) }
        | GatewayReceiveEvent.ENTITLEMENT_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof ENTITLEMENT_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Entitlement ev) }
        | GatewayReceiveEvent.ENTITLEMENT_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof ENTITLEMENT_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Entitlement ev) }
        | GatewayReceiveEvent.GUILD_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildCreate ev) }
        | GatewayReceiveEvent.GUILD_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Guild ev) }
        | GatewayReceiveEvent.GUILD_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildDelete ev) }
        | GatewayReceiveEvent.GUILD_AUDIT_LOG_ENTRY_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_AUDIT_LOG_ENTRY_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildAuditLogEntryCreate ev) }
        | GatewayReceiveEvent.GUILD_BAN_ADD (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_BAN_ADD); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildUser ev) }
        | GatewayReceiveEvent.GUILD_BAN_REMOVE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_BAN_REMOVE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildUser ev) }
        | GatewayReceiveEvent.GUILD_EMOJIS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_EMOJIS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildEmojisUpdate ev) }
        | GatewayReceiveEvent.GUILD_STICKERS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_STICKERS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildStickersUpdate ev) }
        | GatewayReceiveEvent.GUILD_INTEGRATIONS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_INTEGRATIONS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildIntegrationsUpdate ev) }
        | GatewayReceiveEvent.GUILD_MEMBER_ADD (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_MEMBER_ADD); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildMemberAdd ev) }
        | GatewayReceiveEvent.GUILD_MEMBER_REMOVE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_MEMBER_REMOVE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildUser ev) }
        | GatewayReceiveEvent.GUILD_MEMBER_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_MEMBER_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildMemberUpdate ev) }
        | GatewayReceiveEvent.GUILD_MEMBERS_CHUNK (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_MEMBERS_CHUNK); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildMembersChunk ev) }
        | GatewayReceiveEvent.GUILD_ROLE_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_ROLE_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildRole ev) }
        | GatewayReceiveEvent.GUILD_ROLE_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_ROLE_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildRole ev) }
        | GatewayReceiveEvent.GUILD_ROLE_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_ROLE_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildRoleDelete ev) }
        | GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SCHEDULED_EVENT_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildScheduledEvent ev) }
        | GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SCHEDULED_EVENT_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildScheduledEvent ev) }
        | GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SCHEDULED_EVENT_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildScheduledEvent ev) }
        | GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_USER_ADD (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SCHEDULED_EVENT_USER_ADD); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildScheduledEventUser ev) }
        | GatewayReceiveEvent.GUILD_SCHEDULED_EVENT_USER_REMOVE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SCHEDULED_EVENT_USER_REMOVE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildScheduledEventUser ev) }
        | GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUND_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SOUNDBOARD_SOUND_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.SoundboardSound ev) }
        | GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUND_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SOUNDBOARD_SOUND_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.SoundboardSound ev) }
        | GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUND_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SOUNDBOARD_SOUND_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildSoundboardSoundDelete ev) }
        | GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUNDS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SOUNDBOARD_SOUNDS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildSoundboardSounds ev) }
        | GatewayReceiveEvent.GUILD_SOUNDBOARD_SOUNDS (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof GUILD_SOUNDBOARD_SOUNDS); Sequence = Some seq; Data = Some (GatewayReceiveEventData.GuildSoundboardSounds ev) }
        | GatewayReceiveEvent.INTEGRATION_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof INTEGRATION_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Integration ev) }
        | GatewayReceiveEvent.INTEGRATION_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof INTEGRATION_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Integration ev) }
        | GatewayReceiveEvent.INTEGRATION_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof INTEGRATION_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.IntegrationDelete ev) }
        | GatewayReceiveEvent.INVITE_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof INVITE_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.InviteCreate ev) }
        | GatewayReceiveEvent.INVITE_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof INVITE_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.InviteDelete ev) }
        | GatewayReceiveEvent.MESSAGE_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Message ev) }
        | GatewayReceiveEvent.MESSAGE_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Message ev) }
        | GatewayReceiveEvent.MESSAGE_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessageDelete ev) }
        | GatewayReceiveEvent.MESSAGE_DELETE_BULK (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_DELETE_BULK); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessageDeleteBulk ev) }
        | GatewayReceiveEvent.MESSAGE_REACTION_ADD (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_REACTION_ADD); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessageReactionAdd ev) }
        | GatewayReceiveEvent.MESSAGE_REACTION_REMOVE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_REACTION_REMOVE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessageReactionRemove ev) }
        | GatewayReceiveEvent.MESSAGE_REACTION_REMOVE_ALL (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_REACTION_REMOVE_ALL); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessageReactionRemoveAll ev) }
        | GatewayReceiveEvent.MESSAGE_REACTION_REMOVE_EMOJI (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_REACTION_REMOVE_EMOJI); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessageReactionRemoveEmoji ev) }
        | GatewayReceiveEvent.PRESENCE_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof PRESENCE_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.PresenceUpdate ev) }
        | GatewayReceiveEvent.TYPING_START (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof TYPING_START); Sequence = Some seq; Data = Some (GatewayReceiveEventData.TypingStart ev) }
        | GatewayReceiveEvent.USER_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof USER_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.User ev) }
        | GatewayReceiveEvent.VOICE_CHANNEL_EFFECT_SEND (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof VOICE_CHANNEL_EFFECT_SEND); Sequence = Some seq; Data = Some (GatewayReceiveEventData.VoiceChannelEffectSend ev) }
        | GatewayReceiveEvent.VOICE_STATE_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof VOICE_STATE_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.VoiceState ev) }
        | GatewayReceiveEvent.VOICE_SERVER_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof VOICE_SERVER_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.VoiceServerUpdate ev) }
        | GatewayReceiveEvent.WEBHOOKS_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof WEBHOOKS_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.WebhooksUpdate ev) }
        | GatewayReceiveEvent.INTERACTION_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof INTERACTION_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Interaction ev) }
        | GatewayReceiveEvent.STAGE_INSTANCE_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof STAGE_INSTANCE_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.StageInstance ev) }
        | GatewayReceiveEvent.STAGE_INSTANCE_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof STAGE_INSTANCE_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.StageInstance ev) }
        | GatewayReceiveEvent.STAGE_INSTANCE_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof STAGE_INSTANCE_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.StageInstance ev) }
        | GatewayReceiveEvent.SUBSCRIPTION_CREATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof SUBSCRIPTION_CREATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Subscription ev) }
        | GatewayReceiveEvent.SUBSCRIPTION_UPDATE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof SUBSCRIPTION_UPDATE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Subscription ev) }
        | GatewayReceiveEvent.SUBSCRIPTION_DELETE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof SUBSCRIPTION_DELETE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.Subscription ev) }
        | GatewayReceiveEvent.MESSAGE_POLL_VOTE_ADD (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_POLL_VOTE_ADD); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessagePollVote ev) }
        | GatewayReceiveEvent.MESSAGE_POLL_VOTE_REMOVE (ev, seq) -> GatewayReceiveEventPayload.encoder { Opcode = GatewayOpcode.DISPATCH; EventName = Some (nameof MESSAGE_POLL_VOTE_REMOVE); Sequence = Some seq; Data = Some (GatewayReceiveEventData.MessagePollVote ev) }
        
// TODO: Make all values pascal case
