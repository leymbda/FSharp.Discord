namespace rec FSharp.Discord.Types

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-types
[<RequireQualifiedAccess>]
type ActivityType =
    | PLAYING   = 0
    | STREAMING = 1
    | LISTENING = 2
    | WATCHING  = 3
    | CUSTOM    = 4
    | COMPETING = 5
    
[<RequireQualifiedAccess>]
type AnimationType =
    | PREMIUM = 0
    | BASIC   = 1
    
[<RequireQualifiedAccess>]
type ApplicationCommandHandlerType =
    | APP_HANDER              = 1
    | DISCORD_LAUNCH_ACTIVITY = 2
    
[<RequireQualifiedAccess>]
type ApplicationCommandOptionType =
    | SUB_COMMAND       = 1
    | SUB_COMMAND_GROUP = 2
    | STRING            = 3
    | INTEGER           = 4
    | BOOLEAN           = 5
    | USER              = 6
    | CHANNEL           = 7
    | ROLE              = 8
    | MENTIONABLE       = 9
    | NUMBER            = 10
    | ATTACHMENT        = 11
    
[<RequireQualifiedAccess>]
type ApplicationCommandPermissionType =
    | ROLE    = 1
    | USER    = 2
    | CHANNEL = 3
    
[<RequireQualifiedAccess>]
type ApplicationCommandType = 
    | CHAT_INPUT          = 1
    | USER                = 2
    | MESSAGE             = 3
    | PRIMARY_ENTRY_POINT = 4
    
[<RequireQualifiedAccess>]
type ApplicationIntegrationType =
    | GUILD_INSTALL = 0
    | USER_INSTALL  = 1
    
[<RequireQualifiedAccess>]
type ApplicationRoleConnectionMetadataType =
    | INTEGER_LESS_THAN_OR_EQUAL     = 1
    | INTEGER_GREATER_THAN_OR_EQUAL  = 2
    | INTEGER_EQUAL                  = 3
    | INTEGER_NOT_EQUAL              = 4
    | DATETIME_LESS_THAN_OR_EQUAL    = 5
    | DATETIME_GREATER_THAN_OR_EQUAL = 6
    | BOOLEAN_EQUAL                  = 7
    | BOOLEAN_NOT_EQUAL              = 8

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-audit-log-events
[<RequireQualifiedAccess>]
type AuditLogEventType =
    | GUILD_UPDATE                                = 1
    | CHANNEL_CREATE                              = 10
    | CHANNEL_UDPATE                              = 11
    | CHANNEL_DELETE                              = 12
    | CHANNEL_OVERWRITE_CREATE                    = 13
    | CHANNEL_OVERWRITE_UPDATE                    = 14
    | CHANNEL_OVERWRITE_DELETE                    = 15
    | MEMBER_KICK                                 = 20
    | MEMBER_PRUNE                                = 21
    | MEMBER_BAN_ADD                              = 22
    | MEMBER_BAN_REMOVE                           = 23
    | MEMBER_UPDATE                               = 24
    | MEMBER_ROLE_UPDATE                          = 25
    | MEMBER_MOVE                                 = 26
    | MEMBER_DISCONNECT                           = 27
    | BOT_ADD                                     = 28
    | ROLE_CREATE                                 = 30
    | ROLE_UPDATE                                 = 31
    | ROLE_DELETE                                 = 32
    | INVITE_CREATE                               = 40
    | INVITE_UPDATE                               = 41
    | INVITE_DELETE                               = 42
    | WEBHOOK_CREATE                              = 50
    | WEBHOOK_UPDATE                              = 51
    | WEBHOOK_DELETE                              = 52
    | EMOJI_CREATE                                = 60
    | EMOJI_UPDATE                                = 61
    | EMOJI_DELETE                                = 62
    | MESSAGE_DELETE                              = 72
    | MESSAGE_BULK_DELETE                         = 73
    | MESSAGE_PIN                                 = 74
    | MESSAGE_UNPIN                               = 75
    | INTEGRATION_CREATE                          = 80
    | INTEGRATION_UPDATE                          = 81
    | INTEGRATION_DELETE                          = 82
    | STAGE_INSTANCE_CREATE                       = 83
    | STAGE_INSTANCE_UPDATE                       = 84
    | STAGE_INSTANCE_DELETE                       = 85
    | STICKER_CREATE                              = 90
    | STICKER_UPDATE                              = 91
    | STICKER_DELETE                              = 92
    | GUILD_SCHEDULED_EVENT_CREATE                = 100
    | GUILD_SCHEDULED_EVENT_UPDATE                = 101
    | GUILD_SCHEDULED_EVENT_DELETE                = 102
    | THREAD_CREATE                               = 110
    | THREAD_UPDATE                               = 111
    | THREAD_DELETE                               = 112
    | APPLICATION_COMMAND_PERMISSION_UPDATE       = 121
    | SOUNDBOARD_SOUND_CREATE                     = 130
    | SOUNDBOARD_SOUND_UPDATE                     = 131
    | SOUNDBOARD_SOUND_DELETE                     = 132
    | AUTO_MODERATION_RULE_CREATE                 = 140
    | AUTO_MODERATION_RULE_UPDATE                 = 141
    | AUTO_MODERATION_RULE_DELETE                 = 142
    | AUTO_MODERATION_BLOCK_MESSAGE               = 143
    | AUTO_MODERATION_FLAG_TO_CHANNEL             = 144
    | AUTO_MODERATION_USER_COMMUNICATION_DISABLED = 145
    | CREATOR_MONETIZATION_REQUEST_CREATED        = 150
    | CREATOR_MONETIZATION_TERMS_ACCEPTED         = 151
    | ONBOARDING_PROMPT_CREATE                    = 163
    | ONBOARDING_PROMPT_UPDATE                    = 164
    | ONBOARDING_PROMPT_DELETE                    = 165
    | ONBOARDING_CREATE                           = 166
    | ONBOARDING_UPDATE                           = 167
    | HOME_SETTINGS_CREATE                        = 190
    | HOME_SETTINGS_UPDATE                        = 191

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object-action-types
[<RequireQualifiedAccess>]
type AutoModerationActionType =
    | BLOCK_MESSAGE            = 1
    | SEND_ALERT_MESSAGE       = 2
    | TIMEOUT                  = 3
    | BLOCK_MEMBER_INTERACTION = 4

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-event-types
[<RequireQualifiedAccess>]
type AutoModerationEventType =
    | MESSAGE_SEND  = 1
    | MEMBER_UDPATE = 2

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-trigger-types
[<RequireQualifiedAccess>]
type AutoModerationTriggerType =
    | KEYWORD        = 1
    | SPAM           = 3
    | KEYWORD_PRESET = 4
    | MENTION_SPAM   = 5
    | MEMBER_PROFILE = 6

// https://discord.com/developers/docs/resources/channel#channel-object-channel-types
[<RequireQualifiedAccess>]
type ChannelType =
    | GUILD_TEXT          = 0
    | DM                  = 1
    | GUILD_VOICE         = 2
    | GROUP_DM            = 3
    | GUILD_CATEGORY      = 4
    | GUILD_ANNOUNCEMENT  = 5
    | ANNOUNCEMENT_THREAD = 10
    | PUBLIC_THREAD       = 11
    | PRIVATE_THREAD      = 12
    | GUILD_STAGE_VOICE   = 13
    | GUILD_DIRECTORY     = 14
    | GUILD_FORUM         = 15
    | GUILD_MEDIA         = 16
    
[<RequireQualifiedAccess>]
type ComponentType =
    | ACTION_ROW         = 1
    | BUTTON             = 2
    | STRING_SELECT      = 3
    | TEXT_INPUT         = 4
    | USER_SELECT        = 5
    | ROLE_SELECT        = 6
    | MENTIONABLE_SELECT = 7
    | CHANNEL_SELECT     = 8

// https://discord.com/developers/docs/resources/user#connection-object-services
[<RequireQualifiedAccess>]
type ConnectionServiceType =
    | AMAZON_MUSIC
    | BATTLE_NET
    | BLUESKY
    | BUNGIE
    | CRUNCHYROLL
    | DOMAIN
    | EBAY
    | EPIC_GAMES
    | FACEBOOK
    | GITHUB
    | INSTAGRAM
    | LEAGUE_OF_LEGENDS
    | MASTODON
    | PAYPAL
    | PLAYSTATION
    | REDDIT
    | RIOT_GAMES
    | ROBLOX
    | SPOTIFY
    | SKYPE
    | STEAM
    | TIKTOK
    | TWITCH
    | TWITTER
    | XBOX
    | YOUTUBE
    
[<RequireQualifiedAccess>]
type EmbedType =
    | RICH
    | IMAGE
    | VIDEO
    | GIFV
    | ARTICLE
    | LINK
    | POLL_RESULT
    
[<RequireQualifiedAccess>]
type EntitlementType =
    | PURCHASE                 = 1
    | PREMIUM_SUBSCRIPTION     = 2
    | DEVELOPER_GIFT           = 3
    | TEST_MODE_PURCHASE       = 4
    | FREE_PURCHASE            = 5
    | USER_GIFT                = 6
    | PREMIUM_PURCHASE         = 7
    | APPLICATION_SUBSCRIPTION = 8

// https://discord.com/developers/docs/resources/guild#integration-object-integration-structure
[<RequireQualifiedAccess>]
type GuildIntegrationType =
    | TWITCH
    | YOUTUBE
    | DISCORD
    | GUILD_SUBSCRIPTION
    
[<RequireQualifiedAccess>]
type InteractionCallbackType = 
    | PONG                                    = 1
    | CHANNEL_MESSAGE_WITH_SOURCE             = 4
    | DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE    = 5
    | DEFERRED_UPDATE_MESSAGE                 = 6
    | UPDATE_MESSAGE                          = 7
    | APPLICATION_COMMAND_AUTOCOMPLETE_RESULT = 8
    | MODAL                                   = 9
    | LAUNCH_ACTIVITY                         = 12
    
[<RequireQualifiedAccess>]
type InteractionContextType =
    | GUILD           = 0
    | BOT_DM          = 1
    | PRIVATE_CHANNEL = 2

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-interaction-data
[<RequireQualifiedAccess>]
type InteractionDataType =
    | APPLICATION_COMMAND
    | MESSAGE_COMPONENT
    | MODAL_SUBMIT
    
[<RequireQualifiedAccess>]
type InteractionType = 
    | PING                             = 1
    | APPLICATION_COMMAND              = 2
    | MESSAGE_COMPONENT                = 3
    | APPLICATION_COMMAND_AUTOCOMPLETE = 4
    | MODAL_SUBMIT                     = 5
    
[<RequireQualifiedAccess>]
type InviteTargetType =
    | STREAM               = 1
    | EMBEDDED_APPLICATION = 2
    
[<RequireQualifiedAccess>]
type InviteType =
    | GUILD    = 0
    | GROUP_DM = 1
    | FRIEND   = 2
    
[<RequireQualifiedAccess>]
type MessageActivityType =
    | JOIN         = 1
    | SPECTATE     = 2
    | LISTEN       = 3
    | JOIN_REQUEST = 5

// https://discord.com/developers/docs/resources/message#message-reference-types
[<RequireQualifiedAccess>]
type MessageReferenceType =
    | DEFAULT = 0
    | FORWARD = 1
    
[<RequireQualifiedAccess>]
type MessageType =
    | DEFAULT                                      = 0
    | RECIPIENT_ADD                                = 1
    | RECIPIENT_REMOVE                             = 2
    | CALL                                         = 3
    | CHANNEL_NAME_CHANGE                          = 4
    | CHANNEL_ICON_CHANGE                          = 5
    | CHANNEL_PINNED_MESSAGE                       = 6
    | USER_JOIN                                    = 7
    | GUILD_BOOST                                  = 8
    | GUILD_BOOST_TIER_1                           = 9
    | GUILD_BOOST_TIER_2                           = 10
    | GUILD_BOOST_TIER_3                           = 11
    | CHANNEL_FOLLOW_ADD                           = 12
    | GUILD_DISCOVERY_DISQUALIFIED                 = 14
    | GUILD_DISCOVERY_REQUALIFIED                  = 15
    | GUILD_DISCOVERY_GRACE_PERIOD_INITIAL_WARNING = 16
    | GUILD_DISCOVERY_GRACE_PERIOD_FINAL_WARNING   = 17
    | THREAD_CREATED                               = 18
    | REPLY                                        = 19
    | CHAT_INPUT_COMMAND                           = 20
    | THREAD_STARTER_MESSAGE                       = 21
    | GUILD_INVITE_REMINDER                        = 22
    | CONTEXT_MENU_COMMAND                         = 23
    | AUTO_MODERATION_ACTION                       = 24
    | ROLE_SUBSCRIPTION_PURCHASE                   = 25
    | INTERACTION_PREMIUM_UPSELL                   = 26
    | STAGE_START                                  = 27
    | STAGE_END                                    = 28
    | STAGE_SPEAKER                                = 29
    | STAGE_TOPIC                                  = 31
    | GUILD_APPLICATION_PREMIUM_SUBSCRIPTION       = 32
    | GUILD_INCIDENT_ALERT_MODE_ENABLED            = 36
    | GUILD_INCIDENT_ALERT_MODE_DISABLED           = 37
    | GUILD_INCIDENT_REPORT_RAID                   = 38
    | GUILD_INCIDENT_REPORT_FALSE_ALARM            = 39
    | PURCHASE_NOTIFICATION                        = 44
    | POLL_RESULT                                  = 46

module MessageType =
    let isDeletable (messageType: MessageType) =
        match messageType with
        | MessageType.DEFAULT                                      -> true
        | MessageType.RECIPIENT_ADD                                -> false
        | MessageType.RECIPIENT_REMOVE                             -> false
        | MessageType.CALL                                         -> false
        | MessageType.CHANNEL_NAME_CHANGE                          -> false
        | MessageType.CHANNEL_ICON_CHANGE                          -> false
        | MessageType.CHANNEL_PINNED_MESSAGE                       -> true
        | MessageType.USER_JOIN                                    -> true
        | MessageType.GUILD_BOOST                                  -> true
        | MessageType.GUILD_BOOST_TIER_1                           -> true
        | MessageType.GUILD_BOOST_TIER_2                           -> true
        | MessageType.GUILD_BOOST_TIER_3                           -> true
        | MessageType.CHANNEL_FOLLOW_ADD                           -> true
        | MessageType.GUILD_DISCOVERY_DISQUALIFIED                 -> true
        | MessageType.GUILD_DISCOVERY_REQUALIFIED                  -> true
        | MessageType.GUILD_DISCOVERY_GRACE_PERIOD_INITIAL_WARNING -> true
        | MessageType.GUILD_DISCOVERY_GRACE_PERIOD_FINAL_WARNING   -> true
        | MessageType.THREAD_CREATED                               -> true
        | MessageType.REPLY                                        -> true
        | MessageType.CHAT_INPUT_COMMAND                           -> true
        | MessageType.THREAD_STARTER_MESSAGE                       -> false
        | MessageType.GUILD_INVITE_REMINDER                        -> true
        | MessageType.CONTEXT_MENU_COMMAND                         -> true
        | MessageType.AUTO_MODERATION_ACTION                       -> true // only with MANAGE_MESSAGES permission
        | MessageType.ROLE_SUBSCRIPTION_PURCHASE                   -> true
        | MessageType.INTERACTION_PREMIUM_UPSELL                   -> true
        | MessageType.STAGE_START                                  -> true
        | MessageType.STAGE_END                                    -> true
        | MessageType.STAGE_SPEAKER                                -> true
        | MessageType.STAGE_TOPIC                                  -> true
        | MessageType.GUILD_APPLICATION_PREMIUM_SUBSCRIPTION       -> true
        | MessageType.GUILD_INCIDENT_ALERT_MODE_ENABLED            -> true
        | MessageType.GUILD_INCIDENT_ALERT_MODE_DISABLED           -> true
        | MessageType.GUILD_INCIDENT_REPORT_RAID                   -> true
        | MessageType.GUILD_INCIDENT_REPORT_FALSE_ALARM            -> true
        | MessageType.PURCHASE_NOTIFICATION                        -> true
        | MessageType.POLL_RESULT                                  -> true
        | _                                                        -> false

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-prompt-types
[<RequireQualifiedAccess>]
type OnboardingPromptType =
    | MULTIPLE_CHOICE = 0
    | DROPDOWN        = 1
    
[<RequireQualifiedAccess>]
type PermissionOverwriteType =
    | ROLE   = 0
    | MEMBER = 1

// https://discord.com/developers/docs/resources/message#get-reactions-reaction-types
[<RequireQualifiedAccess>]
type ReactionType =
    | NORMAL = 0
    | BURST  = 1

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-entity-types
[<RequireQualifiedAccess>]
type ScheduledEntityType =
    | STANCE_INSTANCE = 1
    | VOICE           = 2
    | EXTERNAL        = 3

// https://discord.com/developers/docs/interactions/message-components#select-menu-object-select-default-value-structure
[<RequireQualifiedAccess>]
type SelectMenuDefaultValueType =
    | USER
    | ROLE
    | CHANNEL

// https://discord.com/developers/docs/resources/sku#sku-object-sku-types
[<RequireQualifiedAccess>]
type SkuType =
    | DURABLE            = 2
    | CONSUMABLE         = 3
    | SUBSCRIPTION       = 5
    | SUBSCRIPTION_GROUP = 6
    
[<RequireQualifiedAccess>]
type StickerType = 
    | STANDARD = 1
    | GUILD    = 2

// https://discord.com/developers/docs/events/webhook-events#event-types
[<RequireQualifiedAccess>]
type WebhookEventType =
    | APPLICATION_AUTHORIZED
    | ENTITLEMENT_CREATE

// https://discord.com/developers/docs/events/webhook-events#webhook-types
[<RequireQualifiedAccess>]
type WebhookPayloadType =
    | PING  = 0
    | EVENT = 1

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-types
[<RequireQualifiedAccess>]
type WebhookType =
    | INCOMING         = 1
    | CHANNEL_FOLLOWER = 2
    | APPLICATION      = 3

// TODO: Add missing documentation links
