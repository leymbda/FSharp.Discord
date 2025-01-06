namespace FSharp.Discord.Types

open System
open System.Text.Json
open System.Text.Json.Serialization

type TextInputStyle =
    | SHORT     = 1
    | PARAGRAPH = 2

type ButtonStyle =
    | PRIMARY   = 1
    | SECONDARY = 2
    | SUCCESS   = 3
    | DANGER    = 4
    | LINK      = 5

type ComponentType =
    | ACTION_ROW         = 1
    | BUTTON             = 2
    | STRING_SELECT      = 3
    | TEXT_INPUT         = 4
    | USER_SELECT        = 5
    | ROLE_SELECT        = 6
    | MENTIONABLE_SELECT = 7
    | CHANNEL_SELECT     = 8

type PermissionOverwriteType =
    | ROLE   = 0
    | MEMBER = 1

type ChannelForumLayout =
    | NOT_SET      = 0
    | LIST_VIEW    = 1
    | GALLERY_VIEW = 2

type ChannelSortOrder =
    | LATEST_ACTIVITY = 0
    | CREATION_DATE   = 1

type VideoQualityMode =
    | AUTO = 1
    | FULL = 2

type PollLayoutType =
    | DEFAULT = 1

type TeamMembershipState =
    | INVITED  = 1
    | ACCEPTED = 2

type MessageActivityType =
    | JOIN         = 1
    | SPECTATE     = 2
    | LISTEN       = 3
    | JOIN_REQUEST = 5

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

// https://discord.com/developers/docs/resources/channel#channel-object-channel-types
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

type EntitlementType =
    | PURCHASE                 = 1
    | PREMIUM_SUBSCRIPTION     = 2
    | DEVELOPER_GIFT           = 3
    | TEST_MODE_PURCHASE       = 4
    | FREE_PURCHASE            = 5
    | USER_GIFT                = 6
    | PREMIUM_PURCHASE         = 7
    | APPLICATION_SUBSCRIPTION = 8

type UserPremiumType =
    | NONE          = 0
    | NITRO_CLASSIC = 1
    | NITRO         = 2
    | NITRO_BASIC   = 3

type StickerFormatType = 
    | PNG    = 1
    | APNG   = 2
    | LOTTIE = 3
    | GIF    = 4

type StickerType = 
    | STANDARD = 1
    | GUILD    = 2

// https://discord.com/developers/docs/resources/guild#guild-object-guild-nsfw-level
type GuildNsfwLevel =
    | DEFAULT        = 0
    | EXPLICIT       = 1
    | SAFE           = 2
    | AGE_RESTRICTED = 3

// https://discord.com/developers/docs/resources/guild#guild-object-premium-tier
type GuildPremiumTier =
    | NONE    = 0
    | LEVEL_1 = 1
    | LEVEL_2 = 2
    | LEVEL_3 = 3

// https://discord.com/developers/docs/resources/guild#guild-object-mfa-level
type GuildMfaLevel =
    | NONE     = 0
    | ELEVATED = 1

// https://discord.com/developers/docs/resources/guild#guild-object-explicit-content-filter-level
type GuildExplicitContentFilterLevel =
    | DISABLED              = 0
    | MEMBERS_WITHOUT_ROLES = 1
    | ALL_MEMBERS           = 2

// https://discord.com/developers/docs/resources/guild#guild-object-default-message-notification-level
type GuildMessageNotificationLevel =
    | ALL_MESSAGES  = 0
    | ONLY_MENTIONS = 1

// https://discord.com/developers/docs/resources/guild#guild-object-verification-level
type GuildVerificationLevel =
    | NONE      = 0
    | LOW       = 1
    | MEDIUM    = 2
    | HIGH      = 3
    | VERY_HIGH = 4

// https://discord.com/developers/docs/resources/guild#guild-object-guild-features
[<JsonConverter(typeof<GuildFeatureConverter>)>]
type GuildFeature =
    | ANIMATED_BANNER
    | ANIMATED_ICON
    | APPLICATION_COMMAND_PERMISSIONS_V2
    | AUTO_MODERATION
    | BANNER
    | COMMUNITY // mutable
    | CREATOR_MONETIZABLE_PROVISIONAL
    | CREATOR_STORE_PAGE
    | DEVELOPER_SUPPORT_SERVER
    | DISCOVERABLE // mutable
    | FEATURABLE
    | INVITES_DISABLED // mutable
    | INVITE_SPLASH
    | MEMBER_VERIFICATION_GATE_ENABLED
    | MORE_STICKERS
    | NEWS
    | PARTNERED
    | PREVIEW_ENABLED
    | RAID_ALERTS_DISABLED // mutable
    | ROLE_ICONS
    | ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE
    | ROLE_SUBSCRIPTIONS_ENABLED
    | TICKETED_EVENTS_ENABLED
    | VANITY_URL
    | VERIFIED
    | VIP_REGIONS
    | WELCOME_SCREEN_ENABLED
with
    override this.ToString () =
        match this with
        | GuildFeature.ANIMATED_BANNER -> "ANIMATED_BANNER"
        | GuildFeature.ANIMATED_ICON -> "ANIMATED_ICON"
        | GuildFeature.APPLICATION_COMMAND_PERMISSIONS_V2 -> "APPLICATION_COMMAND_PERMISSIONS_V2"
        | GuildFeature.AUTO_MODERATION -> "AUTO_MODERATION"
        | GuildFeature.BANNER -> "BANNER"
        | GuildFeature.COMMUNITY -> "COMMUNITY"
        | GuildFeature.CREATOR_MONETIZABLE_PROVISIONAL -> "CREATOR_MONETIZABLE_PROVISIONAL"
        | GuildFeature.CREATOR_STORE_PAGE -> "CREATOR_STORE_PAGE"
        | GuildFeature.DEVELOPER_SUPPORT_SERVER -> "DEVELOPER_SUPPORT_SERVER"
        | GuildFeature.DISCOVERABLE -> "DISCOVERABLE"
        | GuildFeature.FEATURABLE -> "FEATURABLE"
        | GuildFeature.INVITES_DISABLED -> "INVITES_DISABLED"
        | GuildFeature.INVITE_SPLASH -> "INVITE_SPLASH"
        | GuildFeature.MEMBER_VERIFICATION_GATE_ENABLED -> "MEMBER_VERIFICATION_GATE_ENABLED"
        | GuildFeature.MORE_STICKERS -> "MORE_STICKERS"
        | GuildFeature.NEWS -> "NEWS"
        | GuildFeature.PARTNERED -> "PARTNERED"
        | GuildFeature.PREVIEW_ENABLED -> "PREVIEW_ENABLED"
        | GuildFeature.RAID_ALERTS_DISABLED -> "RAID_ALERTS_DISABLED"
        | GuildFeature.ROLE_ICONS -> "ROLE_ICONS"
        | GuildFeature.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE -> "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE"
        | GuildFeature.ROLE_SUBSCRIPTIONS_ENABLED -> "ROLE_SUBSCRIPTIONS_ENABLED"
        | GuildFeature.TICKETED_EVENTS_ENABLED -> "TICKETED_EVENTS_ENABLED"
        | GuildFeature.VANITY_URL -> "VANITY_URL"
        | GuildFeature.VERIFIED -> "VERIFIED"
        | GuildFeature.VIP_REGIONS -> "VIP_REGIONS"
        | GuildFeature.WELCOME_SCREEN_ENABLED -> "WELCOME_SCREEN_ENABLED"

    static member FromString (str: string) =
        match str with
        | "ANIMATED_BANNER" -> Some GuildFeature.ANIMATED_BANNER
        | "ANIMATED_ICON" -> Some GuildFeature.ANIMATED_ICON
        | "APPLICATION_COMMAND_PERMISSIONS_V2" -> Some GuildFeature.APPLICATION_COMMAND_PERMISSIONS_V2
        | "AUTO_MODERATION" -> Some GuildFeature.AUTO_MODERATION
        | "BANNER" -> Some GuildFeature.BANNER
        | "COMMUNITY" -> Some GuildFeature.COMMUNITY
        | "CREATOR_MONETIZABLE_PROVISIONAL" -> Some GuildFeature.CREATOR_MONETIZABLE_PROVISIONAL
        | "CREATOR_STORE_PAGE" -> Some GuildFeature.CREATOR_STORE_PAGE
        | "DEVELOPER_SUPPORT_SERVER" -> Some GuildFeature.DEVELOPER_SUPPORT_SERVER
        | "DISCOVERABLE" -> Some GuildFeature.DISCOVERABLE
        | "FEATURABLE" -> Some GuildFeature.FEATURABLE
        | "INVITES_DISABLED" -> Some GuildFeature.INVITES_DISABLED
        | "INVITE_SPLASH" -> Some GuildFeature.INVITE_SPLASH
        | "MEMBER_VERIFICATION_GATE_ENABLED" -> Some GuildFeature.MEMBER_VERIFICATION_GATE_ENABLED
        | "MORE_STICKERS" -> Some GuildFeature.MORE_STICKERS
        | "NEWS" -> Some GuildFeature.NEWS
        | "PARTNERED" -> Some GuildFeature.PARTNERED
        | "PREVIEW_ENABLED" -> Some GuildFeature.PREVIEW_ENABLED
        | "RAID_ALERTS_DISABLED" -> Some GuildFeature.RAID_ALERTS_DISABLED
        | "ROLE_ICONS" -> Some GuildFeature.ROLE_ICONS
        | "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE" -> Some GuildFeature.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE
        | "ROLE_SUBSCRIPTIONS_ENABLED" -> Some GuildFeature.ROLE_SUBSCRIPTIONS_ENABLED
        | "TICKETED_EVENTS_ENABLED" -> Some GuildFeature.TICKETED_EVENTS_ENABLED
        | "VANITY_URL" -> Some GuildFeature.VANITY_URL
        | "VERIFIED" -> Some GuildFeature.VERIFIED
        | "VIP_REGIONS" -> Some GuildFeature.VIP_REGIONS
        | "WELCOME_SCREEN_ENABLED" -> Some GuildFeature.WELCOME_SCREEN_ENABLED
        | _ -> None

and GuildFeatureConverter () =
    inherit JsonConverter<GuildFeature> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> GuildFeature.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected GuildFeature type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-onboarding-mode
type OnboardingMode =
    | ONBOARDING_DEFAULT  = 0
    | ONBOARDING_ADVANCED = 1

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-prompt-types
type OnboardingPromptType =
    | MULTIPLE_CHOICE = 0
    | DROPDOWN        = 1

[<JsonConverter(typeof<CommandInteractionDataOptionValueConverter>)>]
type CommandInteractionDataOptionValue =
    | String of string
    | Int    of int
    | Double of double
    | Bool   of bool

and CommandInteractionDataOptionValueConverter () =
    inherit JsonConverter<CommandInteractionDataOptionValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.String -> CommandInteractionDataOptionValue.String (reader.GetString())
            | JsonTokenType.Number -> CommandInteractionDataOptionValue.Int (reader.GetInt32())
            | JsonTokenType.True -> CommandInteractionDataOptionValue.Bool true
            | JsonTokenType.False -> CommandInteractionDataOptionValue.Bool false
            | _ -> raise (JsonException "Unexpected CommandInteractionDataOptionValue value")

        override _.Write (writer: Utf8JsonWriter, value: CommandInteractionDataOptionValue, options: JsonSerializerOptions) =
            match value with
            | CommandInteractionDataOptionValue.String v -> writer.WriteStringValue v
            | CommandInteractionDataOptionValue.Int v -> writer.WriteNumberValue v
            | CommandInteractionDataOptionValue.Bool v -> writer.WriteBooleanValue v
            | CommandInteractionDataOptionValue.Double v -> writer.WriteNumberValue v

type ApplicationCommandType = 
    | CHAT_INPUT          = 1
    | USER                = 2
    | MESSAGE             = 3
    | PRIMARY_ENTRY_POINT = 4

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

type ApplicationCommandPermissionType =
    | ROLE    = 1
    | USER    = 2
    | CHANNEL = 3

type InteractionContextType =
    | GUILD           = 0
    | BOT_DM          = 1
    | PRIVATE_CHANNEL = 2

type ApplicationIntegrationType =
    | GUILD_INSTALL = 0
    | USER_INSTALL  = 1

type InteractionType = 
    | PING                             = 1
    | APPLICATION_COMMAND              = 2
    | MESSAGE_COMPONENT                = 3
    | APPLICATION_COMMAND_AUTOCOMPLETE = 4
    | MODAL_SUBMIT                     = 5

type InteractionCallbackType = 
    | PONG                                    = 1
    | CHANNEL_MESSAGE_WITH_SOURCE             = 4
    | DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE    = 5
    | DEFERRED_UPDATE_MESSAGE                 = 6
    | UPDATE_MESSAGE                          = 7
    | APPLICATION_COMMAND_AUTOCOMPLETE_RESULT = 8
    | MODAL                                   = 9
    | LAUNCH_ACTIVITY                         = 12

type InviteType =
    | GUILD    = 0
    | GROUP_DM = 1
    | FRIEND   = 2

type InviteTargetType =
    | STREAM               = 1
    | EMBEDDED_APPLICATION = 2

[<JsonConverter(typeof<MessageNonceConverter>)>]
type MessageNonce =
    | Number of int
    | String of string

and MessageNonceConverter () =
    inherit JsonConverter<MessageNonce> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.Number -> MessageNonce.Number (reader.GetInt32())
            | JsonTokenType.String -> MessageNonce.String (reader.GetString())
            | _ -> raise (JsonException "Unexpected MessageNonce value")

        override _.Write (writer: Utf8JsonWriter, value: MessageNonce, options: JsonSerializerOptions) =
            match value with
            | MessageNonce.Number v -> writer.WriteNumberValue v
            | MessageNonce.String v -> writer.WriteStringValue v

[<JsonConverter(typeof<ApplicationCommandOptionChoiceValueConverter>)>]
type ApplicationCommandOptionChoiceValue =
    | String of string
    | Int    of int
    | Double of double

and ApplicationCommandOptionChoiceValueConverter () =
    inherit JsonConverter<ApplicationCommandOptionChoiceValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.String -> ApplicationCommandOptionChoiceValue.String (reader.GetString())
            | JsonTokenType.Number ->
                let double: double = 0
                let int: int = 0
                if reader.TryGetInt32(ref int) then
                    ApplicationCommandOptionChoiceValue.Int int
                else if reader.TryGetDouble(ref double) then
                    ApplicationCommandOptionChoiceValue.Double double
                else
                    raise (JsonException "Unexpected ApplicationCommandOptionChoiceValue value")
                // TODO: Test if this correctly handles int and double
            | _ -> raise (JsonException "Unexpected ApplicationCommandOptionChoiceValue value")

        override _.Write (writer: Utf8JsonWriter, value: ApplicationCommandOptionChoiceValue, options: JsonSerializerOptions) =
            match value with
            | ApplicationCommandOptionChoiceValue.String v -> writer.WriteStringValue v
            | ApplicationCommandOptionChoiceValue.Int v -> writer.WriteNumberValue v
            | ApplicationCommandOptionChoiceValue.Double v -> writer.WriteNumberValue v
    
[<JsonConverter(typeof<ApplicationCommandMinValueConverter>)>]
type ApplicationCommandMinValue =
    | Int    of int
    | Double of double

and ApplicationCommandMinValueConverter () =
    inherit JsonConverter<ApplicationCommandMinValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.Number ->
                let double: double = 0
                let int: int = 0
                if reader.TryGetInt32(ref int) then
                    ApplicationCommandMinValue.Int int
                else if reader.TryGetDouble(ref double) then
                    ApplicationCommandMinValue.Double double
                else
                    raise (JsonException "Unexpected ApplicationCommandMinValue value")
                // TODO: Test if this correctly handles int and double
            | _ -> raise (JsonException "Unexpected ApplicationCommandMinValue value")

        override _.Write (writer: Utf8JsonWriter, value: ApplicationCommandMinValue, options: JsonSerializerOptions) = 
            match value with
            | ApplicationCommandMinValue.Int v -> writer.WriteNumberValue v
            | ApplicationCommandMinValue.Double v -> writer.WriteNumberValue v
    
[<JsonConverter(typeof<ApplicationCommandMaxValueConverter>)>]
type ApplicationCommandMaxValue =
    | Int    of int
    | Double of double

and ApplicationCommandMaxValueConverter () =
    inherit JsonConverter<ApplicationCommandMaxValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.Number ->
                let double: double = 0
                let int: int = 0
                if reader.TryGetInt32(ref int) then
                    ApplicationCommandMaxValue.Int int
                else if reader.TryGetDouble(ref double) then
                    ApplicationCommandMaxValue.Double double
                else
                    raise (JsonException "Unexpected ApplicationCommandMaxValue value")
                // TODO: Test if this correctly handles int and double
            | _ -> raise (JsonException "Unexpected ApplicationCommandMaxValue value")

        override _.Write (writer: Utf8JsonWriter, value: ApplicationCommandMaxValue, options: JsonSerializerOptions) = 
            match value with
            | ApplicationCommandMaxValue.Int v -> writer.WriteNumberValue v
            | ApplicationCommandMaxValue.Double v -> writer.WriteNumberValue v

[<JsonConverter(typeof<AllowedMentionsParseTypeConverter>)>]
type AllowedMentionsParseType =
    | ROLES
    | USERS
    | EVERYONE
with
    override this.ToString () =
        match this with
        | AllowedMentionsParseType.ROLES -> "roles"
        | AllowedMentionsParseType.USERS -> "users"
        | AllowedMentionsParseType.EVERYONE -> "everyone"

    static member FromString (str: string) =
        match str with
        | "roles" -> Some AllowedMentionsParseType.ROLES
        | "users" -> Some AllowedMentionsParseType.USERS
        | "everyone" -> Some AllowedMentionsParseType.EVERYONE
        | _ -> None

and AllowedMentionsParseTypeConverter () =
    inherit JsonConverter<AllowedMentionsParseType> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> AllowedMentionsParseType.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected AllowedMentionsParseType type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

type ApplicationCommandHandlerType =
    | APP_HANDER              = 1
    | DISCORD_LAUNCH_ACTIVITY = 2

type AnimationType =
    | PREMIUM = 0
    | BAISC   = 1

[<JsonConverter(typeof<SoundboardSoundIdConverter>)>]
type SoundboardSoundId =
    | String of string
    | Int    of int

and SoundboardSoundIdConverter () =
    inherit JsonConverter<SoundboardSoundId> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with // TODO: Test this, sounds wrong
            | JsonTokenType.String -> SoundboardSoundId.String (reader.GetString())
            | JsonTokenType.Number -> SoundboardSoundId.Int (reader.GetInt32())
            | _ -> raise (JsonException "Unexpected SoundboardSoundId value")

        override _.Write (writer: Utf8JsonWriter, value: SoundboardSoundId, options: JsonSerializerOptions) =
            match value with
            | SoundboardSoundId.String v -> writer.WriteStringValue v
            | SoundboardSoundId.Int v -> writer.WriteNumberValue v

type ApplicationRoleConnectionMetadataType =
    | INTEGER_LESS_THAN_OR_EQUAL     = 1
    | INTEGER_GREATER_THAN_OR_EQUAL  = 2
    | INTEGER_EQUAL                  = 3
    | INTEGER_NOT_EQUAL              = 4
    | DATETIME_LESS_THAN_OR_EQUAL    = 5
    | DATETIME_GREATER_THAN_OR_EQUAL = 6
    | BOOLEAN_EQUAL                  = 7
    | BOOLEAN_NOT_EQUAL              = 8

type EditChannelPermissionsType =
    | ROLE   = 0
    | MEMBER = 1

type AutoArchiveDurationType =
    | HOUR       = 60
    | DAY        = 1440
    | THREE_DAYS = 4320
    | WEEK       = 10080

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-event-types
type AutoModerationEventType =
    | MESSAGE_SEND  = 1
    | MEMBER_UDPATE = 2

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-trigger-types
type AutoModerationTriggerType =
    | KEYWORD        = 1
    | SPAM           = 3
    | KEYWORD_PRESET = 4
    | MENTION_SPAM   = 5
    | MEMBER_PROFILE = 6

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-keyword-preset-types
type AutoModerationKeywordPresetType =
    | PROFANITY      = 1
    | SEXUAL_CONTENT = 2
    | SLURS          = 3

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object-action-types
type AutoModerationActionType =
    | BLOCK_MESSAGE            = 1
    | SEND_ALERT_MESSAGE       = 2
    | TIMEOUT                  = 3
    | BLOCK_MEMBER_INTERACTION = 4

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-location-kind-enum
[<JsonConverter(typeof<ActivityLocationKindConverter>)>]
type ActivityLocationKind =
    | GUILD_CHANNEL
    | PRIVATE_CHANNEL
with
    override this.ToString () =
        match this with
        | ActivityLocationKind.GUILD_CHANNEL -> "gc"
        | ActivityLocationKind.PRIVATE_CHANNEL -> "pc"

    static member FromString (str: string) =
        match str with
        | "gc" -> Some ActivityLocationKind.GUILD_CHANNEL
        | "pc" -> Some ActivityLocationKind.PRIVATE_CHANNEL
        | _ -> None
        

and ActivityLocationKindConverter () =
    inherit JsonConverter<ActivityLocationKind> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> ActivityLocationKind.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected ActivityLocationKind type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-audit-log-events
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

// https://discord.com/developers/docs/resources/guild#integration-object-integration-structure
type GuildIntegrationType =
    | TWITCH
    | YOUTUBE
    | DISCORD
    | GUILD_SUBSCRIPTION
with
    override this.ToString () =
        match this with
        | GuildIntegrationType.TWITCH -> "twitch"
        | GuildIntegrationType.YOUTUBE -> "youtube"
        | GuildIntegrationType.DISCORD -> "discord"
        | GuildIntegrationType.GUILD_SUBSCRIPTION -> "guild_subscription"

    static member FromString (str: string) =
        match str with
        | "twitch" -> Some GuildIntegrationType.TWITCH
        | "youtube" -> Some GuildIntegrationType.YOUTUBE
        | "discord" -> Some GuildIntegrationType.DISCORD
        | "guild_subscription" -> Some GuildIntegrationType.GUILD_SUBSCRIPTION
        | _ -> None

type GuildIntegrationTypeConverter () =
    inherit JsonConverter<GuildIntegrationType> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> GuildIntegrationType.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected GuildIntegrationType type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

type IntegrationExpireBehaviorType =
    | REMOVE_ROLE = 0
    | KICK        = 1

[<JsonConverter(typeof<OAuth2ScopeConverter>)>]
type OAuth2Scope =
    | ACTIVITIES_READ
    | ACTIVITIES_WRITE
    | APPLICATIONS_BUILDS_READ
    | APPLICATIONS_BUILDS_UPLOAD
    | APPLICATIONS_COMMANDS
    | APPLICATIONS_COMMANDS_UPDATE
    | APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE
    | APPLICATIONS_ENTITLEMENTS
    | APPLICATIONS_STORE_UPDATE
    | BOT
    | CONNECTIONS
    | DM_CHANNELS_READ
    | EMAIL
    | GDM_JOIN
    | GUILDS
    | GUILDS_JOIN
    | GUILDS_MEMBERS_READ
    | IDENTIFY
    | MESSAGES_READ
    | RELATIONSHIPS_READ
    | ROLE_CONNECTIONS_WRITE
    | RPC
    | RPC_ACTIVITIES_WRITE
    | RPC_NOTIFICATIONS_READ
    | RPC_VOICE_READ
    | RPC_VOICE_WRITE
    | VOICE
    | WEBHOOK_INCOMING
with
    override this.ToString () =
        match this with
        | OAuth2Scope.ACTIVITIES_READ -> "activities.read"
        | OAuth2Scope.ACTIVITIES_WRITE -> "activities.write"
        | OAuth2Scope.APPLICATIONS_BUILDS_READ -> "applications.builds.read"
        | OAuth2Scope.APPLICATIONS_BUILDS_UPLOAD -> "applications.builds.upload"
        | OAuth2Scope.APPLICATIONS_COMMANDS -> "applications.commands"
        | OAuth2Scope.APPLICATIONS_COMMANDS_UPDATE -> "applications.commands.update"
        | OAuth2Scope.APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE -> "applications.commands.permissions.update"
        | OAuth2Scope.APPLICATIONS_ENTITLEMENTS -> "applications.entitlements"
        | OAuth2Scope.APPLICATIONS_STORE_UPDATE -> "applications.store.update"
        | OAuth2Scope.BOT -> "bot"
        | OAuth2Scope.CONNECTIONS -> "connections"
        | OAuth2Scope.DM_CHANNELS_READ -> "dm_channels.read"
        | OAuth2Scope.EMAIL -> "email"
        | OAuth2Scope.GDM_JOIN -> "gdm.join"
        | OAuth2Scope.GUILDS -> "guilds"
        | OAuth2Scope.GUILDS_JOIN -> "guilds.join"
        | OAuth2Scope.GUILDS_MEMBERS_READ -> "guilds.members.read"
        | OAuth2Scope.IDENTIFY -> "identify"
        | OAuth2Scope.MESSAGES_READ -> "messages.read"
        | OAuth2Scope.RELATIONSHIPS_READ -> "relationships.read"
        | OAuth2Scope.ROLE_CONNECTIONS_WRITE -> "role_connections.write"
        | OAuth2Scope.RPC -> "rpc"
        | OAuth2Scope.RPC_ACTIVITIES_WRITE -> "rpc.activities.write"
        | OAuth2Scope.RPC_NOTIFICATIONS_READ -> "rpc.notifications.read"
        | OAuth2Scope.RPC_VOICE_READ -> "rpc.voice.read"
        | OAuth2Scope.RPC_VOICE_WRITE -> "rpc.voice.write"
        | OAuth2Scope.VOICE -> "voice"
        | OAuth2Scope.WEBHOOK_INCOMING -> "webhook.incoming"

    static member FromString (str: string) =
        match str with
        | "activities.read" -> Some OAuth2Scope.ACTIVITIES_READ
        | "activities.write" -> Some OAuth2Scope.ACTIVITIES_WRITE
        | "applications.builds.read" -> Some OAuth2Scope.APPLICATIONS_BUILDS_READ
        | "applications.builds.upload" -> Some OAuth2Scope.APPLICATIONS_BUILDS_UPLOAD
        | "applications.commands" -> Some OAuth2Scope.APPLICATIONS_COMMANDS
        | "applications.commands.update" -> Some OAuth2Scope.APPLICATIONS_COMMANDS_UPDATE
        | "applications.commands.permissions.update" -> Some OAuth2Scope.APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE
        | "applications.entitlements" -> Some OAuth2Scope.APPLICATIONS_ENTITLEMENTS
        | "applications.store.update" -> Some OAuth2Scope.APPLICATIONS_STORE_UPDATE
        | "bot" -> Some OAuth2Scope.BOT
        | "connections" -> Some OAuth2Scope.CONNECTIONS
        | "dm_channels.read" -> Some OAuth2Scope.DM_CHANNELS_READ
        | "email" -> Some OAuth2Scope.EMAIL
        | "gdm.join" -> Some OAuth2Scope.GDM_JOIN
        | "guilds" -> Some OAuth2Scope.GUILDS
        | "guilds.join" -> Some OAuth2Scope.GUILDS_JOIN
        | "guilds.members.read" -> Some OAuth2Scope.GUILDS_MEMBERS_READ
        | "identify" -> Some OAuth2Scope.IDENTIFY
        | "messages.read" -> Some OAuth2Scope.MESSAGES_READ
        | "relationships.read" -> Some OAuth2Scope.RELATIONSHIPS_READ
        | "role_connections.write" -> Some OAuth2Scope.ROLE_CONNECTIONS_WRITE
        | "rpc" -> Some OAuth2Scope.RPC
        | "rpc.activities.write" -> Some OAuth2Scope.RPC_ACTIVITIES_WRITE
        | "rpc.notifications.read" -> Some OAuth2Scope.RPC_NOTIFICATIONS_READ
        | "rpc.voice.read" -> Some OAuth2Scope.RPC_VOICE_READ
        | "rpc.voice.write" -> Some OAuth2Scope.RPC_VOICE_WRITE
        | "voice" -> Some OAuth2Scope.VOICE
        | "webhook.incoming" -> Some OAuth2Scope.WEBHOOK_INCOMING
        | _ -> None

and OAuth2ScopeConverter () =
    inherit JsonConverter<OAuth2Scope> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> OAuth2Scope.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected OAuth2Scope type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

and OAuth2ScopeListConverter () =
    inherit JsonConverter<OAuth2Scope list> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> _.Split(' ')
            |> Array.map OAuth2Scope.FromString
            |> Array.map (Option.defaultWith (JsonException.raiseThunk "Unexpected OAuth2Scope type"))
            |> Array.toList

        override _.Write (writer, value, _) =
            value
            |> List.map (_.ToString())
            |> (fun v -> String.Join(' ', v))
            |> writer.WriteStringValue

[<JsonConverter(typeof<TokenTypeHintConverter>)>]
type TokenTypeHint =
    | ACCESS_TOKEN
    | REFRESH_TOKEN
with
    override this.ToString () =
        match this with
        | TokenTypeHint.ACCESS_TOKEN -> "access_token"
        | TokenTypeHint.REFRESH_TOKEN -> "refresh_token"

    static member FromString (str: string) =
        match str with
        | "access_token" -> Some TokenTypeHint.ACCESS_TOKEN
        | "refresh_token" -> Some TokenTypeHint.REFRESH_TOKEN
        | _ -> None

and TokenTypeHintConverter () =
    inherit JsonConverter<TokenTypeHint> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> TokenTypeHint.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected TokenTypeHint type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-types
type WebhookType =
    | INCOMING         = 1
    | CHANNEL_FOLLOWER = 2
    | APPLICATION      = 3

// https://discord.com/developers/docs/resources/entitlement#create-test-entitlement-json-params
type EntitlementOwnerType =
    | GUILD_SUBSCRIPTION = 1
    | USER_SUBSCRIPTION  = 2

// https://discord.com/developers/docs/resources/guild#get-guild-widget-image-widget-style-options
type GuildWidgetStyle =
    | SHIELD
    | BANNER_1
    | BANNER_2
    | BANNER_3
    | BANNER_4
with
    override this.ToString () =
        match this with
        | GuildWidgetStyle.SHIELD -> "shield"
        | GuildWidgetStyle.BANNER_1 -> "banner_1"
        | GuildWidgetStyle.BANNER_2 -> "banner_2"
        | GuildWidgetStyle.BANNER_3 -> "banner_3"
        | GuildWidgetStyle.BANNER_4 -> "banner_4"

    static member FromString (str: string) =
        match str with
        | "shield" -> Some GuildWidgetStyle.SHIELD
        | "banner_1" -> Some GuildWidgetStyle.BANNER_1
        | "banner_2" -> Some GuildWidgetStyle.BANNER_2
        | "banner_3" -> Some GuildWidgetStyle.BANNER_3
        | "banner_4" -> Some GuildWidgetStyle.BANNER_4
        | _ -> None


type GuildWidgetStyleConverter () =
    inherit JsonConverter<GuildWidgetStyle> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> GuildWidgetStyle.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected GuildWidgetStyle type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue
       
// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-privacy-level
type PrivacyLevelType =
    | GUILD_ONLY = 2

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-status
type EventStatusType =
    | SCHEDULED = 1
    | ACTIVE    = 2
    | COMPLETED = 3
    | CANCELED  = 4

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-entity-types
type ScheduledEntityType =
    | STANCE_INSTANCE = 1
    | VOICE           = 2
    | EXTERNAL        = 3

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-frequency
type RecurrenceRuleFrequencyType =
    | YEARLY  = 0
    | MONTHLY = 1
    | WEEKLY  = 2
    | DAILY   = 3

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-weekday
type RecurrenceRuleWeekdayType =
    | MONDAY    = 1
    | TUESDAY   = 2
    | WEDNESDAY = 3
    | THURSDAY  = 4
    | FRIDAY    = 5
    | SATURDAY  = 6
    | SUNDAY    = 7

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-month
type RecurrenceRuleMonthType =
    | JANUARY   = 1
    | FEBRUARY  = 2
    | MARCH     = 3
    | APRIL     = 4
    | MAY       = 5
    | JUNE      = 6
    | JULY      = 7
    | AUGUST    = 8
    | SEPTEMBER = 9
    | OCTOBER   = 10
    | NOVEMBER  = 11
    | DECEMBER  = 12

// https://discord.com/developers/docs/resources/sku#sku-object-sku-types
type SkuType =
    | DURABLE            = 2
    | CONSUMABLE         = 3
    | SUBSCRIPTION       = 5
    | SUBSCRIPTION_GROUP = 6

// https://discord.com/developers/docs/resources/subscription#subscription-statuses
type SubscriptionStatusType =
    | ACTIVE   = 0
    | ENDING   = 1
    | INACTIVE = 2

// https://discord.com/developers/docs/resources/message#get-reactions-reaction-types
type ReactionType =
    | NORMAL = 0
    | BURST  = 1

// https://discord.com/developers/docs/resources/user#connection-object-services
[<JsonConverter(typeof<ConnectionServiceTypeConverter>)>]
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
with
    override this.ToString () =
        match this with
        | ConnectionServiceType.AMAZON_MUSIC -> "amazon-music"
        | ConnectionServiceType.BATTLE_NET -> "battlenet"
        | ConnectionServiceType.BLUESKY -> "bluesky"
        | ConnectionServiceType.BUNGIE -> "bungie"
        | ConnectionServiceType.CRUNCHYROLL -> "crunchyroll"
        | ConnectionServiceType.DOMAIN -> "domain"
        | ConnectionServiceType.EBAY -> "ebay"
        | ConnectionServiceType.EPIC_GAMES -> "epicgames"
        | ConnectionServiceType.FACEBOOK -> "facebook"
        | ConnectionServiceType.GITHUB -> "github"
        | ConnectionServiceType.INSTAGRAM -> "instagram"
        | ConnectionServiceType.LEAGUE_OF_LEGENDS -> "leagueoflegends"
        | ConnectionServiceType.MASTODON -> "mastodon"
        | ConnectionServiceType.PAYPAL -> "paypal"
        | ConnectionServiceType.PLAYSTATION -> "playstation"
        | ConnectionServiceType.REDDIT -> "reddit"
        | ConnectionServiceType.RIOT_GAMES -> "riotgames"
        | ConnectionServiceType.ROBLOX -> "roblox"
        | ConnectionServiceType.SPOTIFY -> "spotify"
        | ConnectionServiceType.SKYPE -> "skype"
        | ConnectionServiceType.STEAM -> "steam"
        | ConnectionServiceType.TIKTOK -> "tiktok"
        | ConnectionServiceType.TWITCH -> "twitch"
        | ConnectionServiceType.TWITTER -> "twitter"
        | ConnectionServiceType.XBOX -> "xbox"
        | ConnectionServiceType.YOUTUBE -> "youtube"

    static member FromString (str: string) =
        match str with
        | "amazon-music" -> Some ConnectionServiceType.AMAZON_MUSIC
        | "battlenet" -> Some ConnectionServiceType.BATTLE_NET
        | "bluesky" -> Some ConnectionServiceType.BLUESKY
        | "bungie" -> Some ConnectionServiceType.BUNGIE
        | "cruncyroll" -> Some ConnectionServiceType.CRUNCHYROLL
        | "domain" -> Some ConnectionServiceType.DOMAIN
        | "ebay" -> Some ConnectionServiceType.EBAY
        | "epicgames" -> Some ConnectionServiceType.EPIC_GAMES
        | "facebook" -> Some ConnectionServiceType.FACEBOOK
        | "github" -> Some ConnectionServiceType.GITHUB
        | "instagram" -> Some ConnectionServiceType.INSTAGRAM
        | "leagueoflegends" -> Some ConnectionServiceType.LEAGUE_OF_LEGENDS
        | "mastodon" -> Some ConnectionServiceType.MASTODON
        | "paypal" -> Some ConnectionServiceType.PAYPAL
        | "playstation" -> Some ConnectionServiceType.PLAYSTATION
        | "reddit" -> Some ConnectionServiceType.REDDIT
        | "riotgames" -> Some ConnectionServiceType.RIOT_GAMES
        | "roblox" -> Some ConnectionServiceType.ROBLOX
        | "spotify" -> Some ConnectionServiceType.SPOTIFY
        | "skype" -> Some ConnectionServiceType.SKYPE
        | "steam" -> Some ConnectionServiceType.STEAM
        | "tiktok" -> Some ConnectionServiceType.TIKTOK
        | "twitch" -> Some ConnectionServiceType.TWITCH
        | "twitter" -> Some ConnectionServiceType.TWITTER
        | "xbox" -> Some ConnectionServiceType.XBOX
        | "youtube" -> Some ConnectionServiceType.YOUTUBE
        | _ -> None

and ConnectionServiceTypeConverter () =
    inherit JsonConverter<ConnectionServiceType> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> ConnectionServiceType.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected ConnectionServiceType type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/resources/user#connection-object-visibility-types
type ConnectionVisibilityType =
    | NONE     = 0
    | EVERYONE = 1

// https://discord.com/developers/docs/events/webhook-events#webhook-types
type WebhookPayloadType =
    | PING  = 0
    | EVENT = 1

// https://discord.com/developers/docs/resources/application#application-object-application-event-webhook-status
type WebhookEventStatus =
    | DISABLED            = 1
    | ENABLED             = 2
    | DISABLED_BY_DISCORD = 3

// https://discord.com/developers/docs/events/webhook-events#event-types
[<JsonConverter(typeof<WebhookEventTypeConverter>)>]
type WebhookEventType =
    | APPLICATION_AUTHORIZED
    | ENTITLEMENT_CREATE
with
    override this.ToString () =
        match this with
        | WebhookEventType.APPLICATION_AUTHORIZED -> "APPLICATION_AUTHORIZED"
        | WebhookEventType.ENTITLEMENT_CREATE -> "ENTITLEMENT_CREATE"

    static member FromString (str: string) =
        match str with
        | "APPLICATION_AUTHORIZED" -> Some WebhookEventType.APPLICATION_AUTHORIZED
        | "ENTITLEMENT_CREATE" -> Some WebhookEventType.ENTITLEMENT_CREATE
        | _ -> None

and WebhookEventTypeConverter () =
    inherit JsonConverter<WebhookEventType> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> WebhookEventType.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected WebhookEventType type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/topics/permissions#permissions-bitwise-permission-flags
type Permission =
    /// Allows creation of instant invites
    | CREATE_INSTANT_INVITE               = (1L <<< 0)
    /// Allows kicking members
    | KICK_MEMBERS                        = (1L <<< 1)
    /// Allows banning members
    | BAN_MEMBERS                         = (1L <<< 2)
    /// Allows all permissions and bypasses channel permission overwrites
    | ADMINISTRATOR                       = (1L <<< 3)
    /// Allows management and editing of channels
    | MANAGE_CHANNELS                     = (1L <<< 4)
    /// Allows management and editing of the guild
    | MANAGE_GUILD                        = (1L <<< 5)
    /// Allows for the addition of reactions to messages
    | ADD_REACTIONS                       = (1L <<< 6)
    /// Allows for viewing of audit logs
    | VIEW_AUDIT_LOG                      = (1L <<< 7)
    /// Allows for using priority speaker in a voice channel
    | PRIORITY_SPEAKER                    = (1L <<< 8)
    /// Allows the user to go live
    | STREAM                              = (1L <<< 9)
    /// Allows guild members to view a channel, which includes reading messages in text channels and joining voice channels
    | VIEW_CHANNEL                        = (1L <<< 10)
    /// Allows for sending messages in a channel and creating threads in a forum (does not allow for sending messages in threads)
    | SEND_MESSAGES                       = (1L <<< 11)
    /// Allows for sending of `tts` messages
    | SEND_TTS_MESSAGES                   = (1L <<< 12)
    /// Allows for deletion of other users messages
    | MANAGE_MESSAGES                     = (1L <<< 13)
    /// Links sent by users with this permission will be auto-embedded
    | EMBED_LINKS                         = (1L <<< 14)
    /// Allows for uploading images and files
    | ATTACH_FILES                        = (1L <<< 15)
    /// Allows for reading of message history
    | READ_MESSAGE_HISTORY                = (1L <<< 16)
    /// Allows for using the `@everyone` tag to notify all users in a channel, and the `@here` tag to notify all online users in a channel
    | MENTION_EVERYONE                    = (1L <<< 17)
    /// Allows the usage of custom emojis from other servers
    | USE_EXTERNAL_EMOJIS                 = (1L <<< 18)
    /// Allows for viewing guild insights
    | VIEW_GUILD_INSIGHTS                 = (1L <<< 19)
    /// Allows for joining of a voice channel
    | CONNECT                             = (1L <<< 20)
    /// Allows for speaking in a voice channel
    | SPEAK                               = (1L <<< 21)
    /// Allows for muting members in a voice channel
    | MUTE_MEMBERS                        = (1L <<< 22)
    /// Allows for deafening of members in a voice channel
    | DEAFEN_MEMBERS                      = (1L <<< 23)
    /// Allows for moving of members between voice channels
    | MOVE_MEMBERS                        = (1L <<< 24)
    /// Allows for using voice-activity-detection in a voice channel
    | USE_VAD                             = (1L <<< 25)
    /// Allows for modification of own nickname (labelled `Use Voice Activity` in Discord client)
    | CHANGE_NICKNAME                     = (1L <<< 26)
    /// Allows for modification of other users nicknames
    | MANAGE_NICKNAMES                    = (1L <<< 27)
    /// Allows management and editing of roles (labelled `Manage Permissions` in Discord client)
    | MANAGE_ROLES                        = (1L <<< 28)
    /// Allows management and editing of webhooks
    | MANAGE_WEBHOOKS                     = (1L <<< 29)
    /// Allows for editing and deleting emojis, stickers, and soundboard sounds created by all users
    | MANAGE_GUILD_EXPRESSIONS            = (1L <<< 30)
    /// Allows members to use application commands, including slash commands and context menu commands
    | USE_APPLICATION_COMMANDS            = (1L <<< 31)
    /// Allows for requesting to speak in stage channels
    | REQUEST_TO_SPEAK                    = (1L <<< 32)
    /// Allows for editing and deleting scheduled events created by all users
    | MANAGE_EVENTS                       = (1L <<< 33)
    /// Allows for deleting and archiving threads, and viewing all private threads
    | MANAGE_THREADS                      = (1L <<< 34)
    /// Allows for creating public and announcement threads
    | CREATE_PUBLIC_THREADS               = (1L <<< 35)
    /// Allows for creating private threads
    | CREATE_PRIVATE_THREADS              = (1L <<< 36)
    /// Allows the usage of custom stickers from other servers
    | USE_EXTERNAL_STICKERS               = (1L <<< 37)
    /// Allows for sending messages in threads
    | SEND_MESSAGES_IN_THREADS            = (1L <<< 38)
    /// Allows for using Activities (applications with the `EMBEDDED` flag) in a voice channel
    | USE_EMBEDDED_ACTIVITIES             = (1L <<< 39)
    /// Allows for timing out users to prevent them from sending or reacting to messages in chat and threads, and from speaking in voice and stage channels (labelled `Timeout Members` in Discord client)
    | MODERATE_MEMBERS                    = (1L <<< 40)
    /// Allows for viewing role subscription insights
    | VIEW_CREATOR_MONETIZATION_ANALYTICS = (1L <<< 41)
    /// Allows for using soundboard in a voice channel
    | USE_SOUNDBOARD                      = (1L <<< 42)
    ///// Allows for creating emojis, stickers, and soundboard sounds, and editing and deleting those created by the current user
    //| CREATE_GUILD_EXPRESSIONS            = (1L <<< 43) // (Not yet available to developers)
    ///// Allows for creating scheduled events, and editing and deleting those created by the current user
    //| CREATE_EVENTS                       = (1L <<< 44) // (Not yet available to developers)
    /// Allows the usage of custom soundboard sounds from other servers
    | USE_EXTERNAL_SOUNDS                 = (1L <<< 45)
    /// Allows sending voice messages
    | SEND_VOICE_MESSAGES                 = (1L <<< 46)
    /// Allows sending polls
    | SEND_POLLS                          = (1L <<< 49)
    /// Allows user-installed apps to send public responses. When disabled, users will still be allowed to use their apps but the responses will be ephemeral. This only applies to apps not also installed to the server
    | USE_EXTERNAL_APPS                   = (1L <<< 50)

module Permission =
    /// Returns whether the permission applies to the given channel type
    let appliesToChannelType channelType permission =
        let (|Text|Voice|Stage|Invalid|) (channelType: ChannelType) =
            match channelType with
            | ChannelType.GUILD_TEXT -> Text
            | ChannelType.GUILD_VOICE -> Voice
            | ChannelType.GUILD_ANNOUNCEMENT -> Text
            | ChannelType.GUILD_STAGE_VOICE -> Stage
            | ChannelType.GUILD_FORUM -> Text
            | ChannelType.GUILD_MEDIA -> Text
            | _ -> Invalid

        match permission, channelType with
        | Permission.CREATE_INSTANT_INVITE, (Text | Voice | Stage) -> true
        | Permission.MANAGE_CHANNELS, (Text | Voice | Stage) -> true
        | Permission.ADD_REACTIONS, (Text | Voice | Stage) -> true
        | Permission.PRIORITY_SPEAKER, (Voice) -> true
        | Permission.STREAM, (Voice | Stage) -> true
        | Permission.VIEW_CHANNEL, (Text | Voice | Stage) -> true
        | Permission.SEND_MESSAGES, (Text | Voice | Stage) -> true
        | Permission.SEND_TTS_MESSAGES, (Text | Voice | Stage) -> true
        | Permission.MANAGE_MESSAGES, (Text | Voice | Stage) -> true
        | Permission.EMBED_LINKS, (Text | Voice | Stage) -> true
        | Permission.ATTACH_FILES, (Text | Voice | Stage) -> true
        | Permission.READ_MESSAGE_HISTORY, (Text | Voice | Stage) -> true
        | Permission.MENTION_EVERYONE, (Text | Voice | Stage) -> true
        | Permission.USE_EXTERNAL_EMOJIS, (Text | Voice | Stage) -> true
        | Permission.CONNECT, (Voice | Stage) -> true
        | Permission.SPEAK, (Voice) -> true
        | Permission.MUTE_MEMBERS, (Voice | Stage) -> true
        | Permission.DEAFEN_MEMBERS, (Voice) -> true
        | Permission.MOVE_MEMBERS, (Voice | Stage) -> true
        | Permission.USE_VAD, (Voice) -> true
        | Permission.MANAGE_ROLES, (Text | Voice | Stage) -> true
        | Permission.MANAGE_WEBHOOKS, (Text | Voice | Stage) -> true
        | Permission.USE_APPLICATION_COMMANDS, (Text | Voice | Stage) -> true
        | Permission.REQUEST_TO_SPEAK, (Stage) -> true
        | Permission.MANAGE_EVENTS, (Voice | Stage) -> true
        | Permission.MANAGE_THREADS, (Text) -> true
        | Permission.CREATE_PUBLIC_THREADS, (Text) -> true
        | Permission.CREATE_PRIVATE_THREADS, (Text) -> true
        | Permission.USE_EXTERNAL_STICKERS, (Text | Voice | Stage) -> true
        | Permission.SEND_MESSAGES_IN_THREADS, (Text) -> true
        | Permission.USE_EMBEDDED_ACTIVITIES, (Voice) -> true
        | Permission.USE_SOUNDBOARD, (Voice) -> true
        //| Permission.CREATE_EVENTS, (Voice | Stage) -> true
        | Permission.USE_EXTERNAL_SOUNDS, (Voice) -> true
        | Permission.SEND_VOICE_MESSAGES, (Text | Voice | Stage) -> true
        | Permission.SEND_POLLS, (Text | Voice | Stage) -> true
        | Permission.USE_EXTERNAL_APPS, (Text | Voice | Stage) -> true
        | _ -> false

    // Returns whether the permission requires 2FA when used on a guild that has server-wide 2FA enabled
    let requiresTwoFactorAuthentication permission =
        match permission with
        | Permission.KICK_MEMBERS -> true
        | Permission.BAN_MEMBERS -> true
        | Permission.ADMINISTRATOR -> true
        | Permission.MANAGE_CHANNELS -> true
        | Permission.MANAGE_GUILD -> true
        | Permission.MANAGE_MESSAGES -> true
        | Permission.MANAGE_ROLES -> true
        | Permission.MANAGE_WEBHOOKS -> true
        | Permission.MANAGE_GUILD_EXPRESSIONS -> true
        | Permission.MANAGE_THREADS -> true
        | Permission.VIEW_CREATOR_MONETIZATION_ANALYTICS -> true
        | _ -> false

// TODO: Sort alphabetically and extract more into separate files in enums folder
