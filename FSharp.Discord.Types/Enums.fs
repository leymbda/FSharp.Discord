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

type ForumLayout =
    | NOT_SET      = 0
    | LIST_VIEW    = 1
    | GALLERY_VIEW = 2

type ChannelSortOrder =
    | LATEST_ACTIVITY = 0
    | CREATION_DATE   = 1

type VideoQualityMode =
    | AUTO = 1
    | FULL = 2

type PollLayout =
    | DEFAULT = 1

type MembershipState =
    | INVITED  = 1
    | ACCEPTED = 2

type UserPremiumTier =
    | NONE          = 0
    | NITRO_CLASSIC = 1
    | NITRO         = 2
    | NITRO_BASIC   = 3

type StickerFormat = 
    | PNG    = 1
    | APNG   = 2
    | LOTTIE = 3
    | GIF    = 4

// https://discord.com/developers/docs/resources/guild#guild-object-guild-nsfw-level
type NsfwLevel =
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
type MfaLevel =
    | NONE     = 0
    | ELEVATED = 1

// https://discord.com/developers/docs/resources/guild#guild-object-explicit-content-filter-level
type ExplicitContentFilterLevel =
    | DISABLED              = 0
    | MEMBERS_WITHOUT_ROLES = 1
    | ALL_MEMBERS           = 2

// https://discord.com/developers/docs/resources/guild#guild-object-default-message-notification-level
type MessageNotificationLevel =
    | ALL_MESSAGES  = 0
    | ONLY_MENTIONS = 1

// https://discord.com/developers/docs/resources/guild#guild-object-verification-level
type VerificationLevel =
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
    inherit JsonConverter<GuildFeature> ()

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

[<JsonConverter(typeof<CommandInteractionDataOptionValueConverter>)>]
type CommandInteractionDataOptionValue =
    | String of string
    | Int    of int
    | Double of double
    | Bool   of bool

and CommandInteractionDataOptionValueConverter () =
    inherit JsonConverter<CommandInteractionDataOptionValue> ()

    override _.Read (reader, _, _) =
        match reader.TokenType with
        | JsonTokenType.String -> CommandInteractionDataOptionValue.String (reader.GetString())
        | JsonTokenType.Number -> CommandInteractionDataOptionValue.Int (reader.GetInt32())
        | JsonTokenType.True -> CommandInteractionDataOptionValue.Bool true
        | JsonTokenType.False -> CommandInteractionDataOptionValue.Bool false
        | _ -> raise (JsonException "Unexpected CommandInteractionDataOptionValue value")

    override _.Write (writer, value, _) =
        match value with
        | CommandInteractionDataOptionValue.String v -> writer.WriteStringValue v
        | CommandInteractionDataOptionValue.Int v -> writer.WriteNumberValue v
        | CommandInteractionDataOptionValue.Bool v -> writer.WriteBooleanValue v
        | CommandInteractionDataOptionValue.Double v -> writer.WriteNumberValue v

[<JsonConverter(typeof<MessageNonceConverter>)>]
type MessageNonce =
    | Number of int
    | String of string

and MessageNonceConverter () =
    inherit JsonConverter<MessageNonce> ()

    override _.Read (reader, _, _) =
        match reader.TokenType with
        | JsonTokenType.Number -> MessageNonce.Number (reader.GetInt32())
        | JsonTokenType.String -> MessageNonce.String (reader.GetString())
        | _ -> raise (JsonException "Unexpected MessageNonce value")

    override _.Write (writer, value, _) =
        match value with
        | MessageNonce.Number v -> writer.WriteNumberValue v
        | MessageNonce.String v -> writer.WriteStringValue v

[<JsonConverter(typeof<ApplicationCommandOptionChoiceValueConverter>)>]
type ApplicationCommandOptionChoiceValue =
    | String of string
    | Int    of int
    | Double of double

and ApplicationCommandOptionChoiceValueConverter () =
    inherit JsonConverter<ApplicationCommandOptionChoiceValue> ()

    override _.Read (reader, _, _) =
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

    override _.Write (writer, value, _) =
        match value with
        | ApplicationCommandOptionChoiceValue.String v -> writer.WriteStringValue v
        | ApplicationCommandOptionChoiceValue.Int v -> writer.WriteNumberValue v
        | ApplicationCommandOptionChoiceValue.Double v -> writer.WriteNumberValue v
    
[<JsonConverter(typeof<ApplicationCommandMinValueConverter>)>]
type ApplicationCommandMinValue =
    | Int    of int
    | Double of double

and ApplicationCommandMinValueConverter () =
    inherit JsonConverter<ApplicationCommandMinValue> ()

    override _.Read (reader, _, _) =
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

    override _.Write (writer, value, _) = 
        match value with
        | ApplicationCommandMinValue.Int v -> writer.WriteNumberValue v
        | ApplicationCommandMinValue.Double v -> writer.WriteNumberValue v
    
[<JsonConverter(typeof<ApplicationCommandMaxValueConverter>)>]
type ApplicationCommandMaxValue =
    | Int    of int
    | Double of double

and ApplicationCommandMaxValueConverter () =
    inherit JsonConverter<ApplicationCommandMaxValue> ()

    override _.Read (reader, _, _) =
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

    override _.Write (writer, value, _) = 
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
    inherit JsonConverter<AllowedMentionsParseType> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> AllowedMentionsParseType.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected AllowedMentionsParseType type")

    override _.Write (writer, value, _) = 
        value.ToString() |> writer.WriteStringValue

[<JsonConverter(typeof<SoundboardSoundIdConverter>)>]
type SoundboardSoundId =
    | String of string
    | Int    of int

and SoundboardSoundIdConverter () =
    inherit JsonConverter<SoundboardSoundId> ()

    override _.Read (reader, _, _) =
        match reader.TokenType with // TODO: Test this, sounds wrong
        | JsonTokenType.String -> SoundboardSoundId.String (reader.GetString())
        | JsonTokenType.Number -> SoundboardSoundId.Int (reader.GetInt32())
        | _ -> raise (JsonException "Unexpected SoundboardSoundId value")

    override _.Write (writer, value, _) =
        match value with
        | SoundboardSoundId.String v -> writer.WriteStringValue v
        | SoundboardSoundId.Int v -> writer.WriteNumberValue v

type AutoArchiveDuration =
    | HOUR       = 60
    | DAY        = 1440
    | THREE_DAYS = 4320
    | WEEK       = 10080

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-keyword-preset-types
type AutoModerationKeywordPreset =
    | PROFANITY      = 1
    | SEXUAL_CONTENT = 2
    | SLURS          = 3

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
    inherit JsonConverter<ActivityLocationKind> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> ActivityLocationKind.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected ActivityLocationKind type")

    override _.Write (writer, value, _) = 
        value.ToString() |> writer.WriteStringValue

type IntegrationExpireBehavior =
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
    inherit JsonConverter<OAuth2Scope> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> OAuth2Scope.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected OAuth2Scope type")

    override _.Write (writer, value, _) = 
        value.ToString() |> writer.WriteStringValue

and OAuth2ScopeListConverter () =
    inherit JsonConverter<OAuth2Scope list> ()

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
    inherit JsonConverter<TokenTypeHint> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> TokenTypeHint.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected TokenTypeHint type")

    override _.Write (writer, value, _) = 
        value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/resources/guild#get-guild-widget-image-widget-style-options
[<JsonConverter(typeof<GuildWidgetStyleConverter>)>]
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

and GuildWidgetStyleConverter () =
    inherit JsonConverter<GuildWidgetStyle> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> GuildWidgetStyle.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected GuildWidgetStyle type")

    override _.Write (writer, value, _) = 
        value.ToString() |> writer.WriteStringValue
       
// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-privacy-level
type PrivacyLevel =
    | GUILD_ONLY = 2

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-status
type EventStatus =
    | SCHEDULED = 1
    | ACTIVE    = 2
    | COMPLETED = 3
    | CANCELED  = 4

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-frequency
type RecurrenceRuleFrequency =
    | YEARLY  = 0
    | MONTHLY = 1
    | WEEKLY  = 2
    | DAILY   = 3

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-weekday
type RecurrenceRuleWeekday =
    | MONDAY    = 1
    | TUESDAY   = 2
    | WEDNESDAY = 3
    | THURSDAY  = 4
    | FRIDAY    = 5
    | SATURDAY  = 6
    | SUNDAY    = 7

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-month
type RecurrenceRuleMonth =
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

// https://discord.com/developers/docs/resources/subscription#subscription-statuses
type SubscriptionStatus =
    | ACTIVE   = 0
    | ENDING   = 1
    | INACTIVE = 2

// https://discord.com/developers/docs/resources/user#connection-object-visibility-types
type ConnectionVisibility =
    | NONE     = 0
    | EVERYONE = 1

// https://discord.com/developers/docs/resources/application#application-object-application-event-webhook-status
type WebhookEventStatus =
    | DISABLED            = 1
    | ENABLED             = 2
    | DISABLED_BY_DISCORD = 3

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

// https://discord.com/developers/docs/events/gateway#transport-compression
[<JsonConverter(typeof<GatewayCompressionConverter>)>]
type GatewayCompression =
    | ZLIBSTREAM
    | ZSTDSTREAM
with
    override this.ToString () =
        match this with
        | GatewayCompression.ZLIBSTREAM -> "zlib-stream"
        | GatewayCompression.ZSTDSTREAM -> "zstd-stream"

    static member FromString (str: string) =
        match str with
        | "zlib-stream" -> Some GatewayCompression.ZLIBSTREAM
        | "zstd-stream" -> Some GatewayCompression.ZSTDSTREAM
        | _ -> None

and GatewayCompressionConverter () =
    inherit JsonConverter<GatewayCompression> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> GatewayCompression.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected GatewayCompression type")

    override _.Write (writer, value, _) = 
        value.ToString()
        |> writer.WriteStringValue

// https://discord.com/developers/docs/events/gateway#encoding-and-compression
[<JsonConverter(typeof<GatewayEncodingConverter>)>]
type GatewayEncoding =
    | JSON
    | ETF
with
    override this.ToString () =
        match this with
        | GatewayEncoding.JSON -> "json"
        | GatewayEncoding.ETF -> "etf"

    static member FromString (str: string) =
        match str with
        | "json" -> Some GatewayEncoding.JSON
        | "etf" -> Some GatewayEncoding.ETF
        | _ -> None

and GatewayEncodingConverter () =
    inherit JsonConverter<GatewayEncoding> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> GatewayEncoding.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected GatewayEncoding type")

    override _.Write (writer, value, _) = 
        value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/events/gateway#list-of-intents
type GatewayIntent =
    | GUILDS =                        (1 <<< 0)
    | GUILD_MEMBERS =                 (1 <<< 1)
    | GUILD_MODERATION =              (1 <<< 2)
    | GUILD_EMOJIS_AND_STICKERS =     (1 <<< 3)
    | GUILD_INTEGRATIONS =            (1 <<< 4)
    | GUILD_WEBHOOKS =                (1 <<< 5)
    | GUILD_INVITES =                 (1 <<< 6)
    | GUILD_VOICE_STATES =            (1 <<< 7)
    | GUILD_PRESENCES =               (1 <<< 8)
    | GUILD_MESSAGES =                (1 <<< 9)
    | GUILD_MESSAGE_REACTIONS =       (1 <<< 10)
    | GUILD_MESSAGE_TYPING =          (1 <<< 11)
    | DIRECT_MESSAGES =               (1 <<< 12)
    | DIRECT_MESSAGE_REACTIONS =      (1 <<< 13)
    | DIRECT_MESSAGE_TYPING =         (1 <<< 14)
    | MESSAGE_CONTENT =               (1 <<< 15)
    | GUILD_SCHEDULED_EVENTS =        (1 <<< 16)
    | AUTO_MODERATION_CONFIGURATION = (1 <<< 20)
    | AUTO_MODERATION_EXECUTION =     (1 <<< 21)
    | GUILD_MESSAGE_POLLS =           (1 <<< 24)
    | DIRECT_MESSAGE_POLLS =          (1 <<< 25)

module GatewayIntent =
    let ALL =
        int <| (
                GatewayIntent.GUILDS
            ||| GatewayIntent.GUILD_MEMBERS
            ||| GatewayIntent.GUILD_MODERATION
            ||| GatewayIntent.GUILD_EMOJIS_AND_STICKERS
            ||| GatewayIntent.GUILD_INTEGRATIONS
            ||| GatewayIntent.GUILD_WEBHOOKS
            ||| GatewayIntent.GUILD_INVITES
            ||| GatewayIntent.GUILD_VOICE_STATES
            ||| GatewayIntent.GUILD_PRESENCES
            ||| GatewayIntent.GUILD_MESSAGES
            ||| GatewayIntent.GUILD_MESSAGE_REACTIONS
            ||| GatewayIntent.GUILD_MESSAGE_TYPING
            ||| GatewayIntent.DIRECT_MESSAGES
            ||| GatewayIntent.DIRECT_MESSAGE_REACTIONS
            ||| GatewayIntent.DIRECT_MESSAGE_TYPING
            ||| GatewayIntent.MESSAGE_CONTENT
            ||| GatewayIntent.GUILD_SCHEDULED_EVENTS
            ||| GatewayIntent.AUTO_MODERATION_CONFIGURATION
            ||| GatewayIntent.AUTO_MODERATION_EXECUTION
            ||| GatewayIntent.GUILD_MESSAGE_POLLS
            ||| GatewayIntent.DIRECT_MESSAGE_POLLS
        )

// https://discord.com/developers/docs/events/gateway-events#update-presence-status-types
[<JsonConverter(typeof<StatusConverter>)>]
type Status =
    | ONLINE
    | DND
    | IDLE
    | INVISIBLE
    | OFFLINE
with
    override this.ToString () =
        match this with
        | Status.ONLINE -> "online"
        | Status.DND -> "dnd"
        | Status.IDLE -> "idle"
        | Status.INVISIBLE -> "invisible"
        | Status.OFFLINE -> "offline"

    static member FromString (str: string) =
        match str with
        | "online" -> Some Status.ONLINE
        | "dnd" -> Some Status.DND
        | "idle" -> Some Status.IDLE
        | "invisible" -> Some Status.INVISIBLE
        | "offline" -> Some Status.OFFLINE
        | _ -> None

and StatusConverter () =
    inherit JsonConverter<Status> ()

    override _.Read (reader, _, _) =
        reader.GetString()
        |> Status.FromString
        |> Option.defaultWith (JsonException.raiseThunk "Unexpected Status type")

    override _.Write (writer, value, _) = 
        value.ToString() |> writer.WriteStringValue
        
type RateLimitScope =
    | USER
    | GLOBAL
    | SHARED
with
    override this.ToString () =
        match this with
        | USER -> "user"
        | GLOBAL -> "global"
        | SHARED -> "shared"

    static member FromString (str: string) =
        match str with
        | "user" -> Some RateLimitScope.USER
        | "global" -> Some RateLimitScope.GLOBAL
        | "shared" -> Some RateLimitScope.SHARED
        | _ -> None

// TODO: Sort alphabetically and extract more into separate files in enums folder
// TODO: Add missing documentation links
