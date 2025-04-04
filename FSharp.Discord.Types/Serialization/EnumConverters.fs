namespace FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Thoth.Json.Net

module GuildFeature =
    module String =
        let [<Literal>] ANIMATED_BANNER = "ANIMATED_BANNER"
        let [<Literal>] ANIMATED_ICON = "ANIMATED_ICON"
        let [<Literal>] APPLICATION_COMMAND_PERMISSIONS_V2 = "APPLICATION_COMMAND_PERMISSIONS_V2"
        let [<Literal>] AUTO_MODERATION = "AUTO_MODERATION"
        let [<Literal>] BANNER = "BANNER"
        let [<Literal>] COMMUNITY = "COMMUNITY"
        let [<Literal>] CREATOR_MONETIZABLE_PROVISIONAL = "CREATOR_MONETIZABLE_PROVISIONAL"
        let [<Literal>] CREATOR_STORE_PAGE = "CREATOR_STORE_PAGE"
        let [<Literal>] DEVELOPER_SUPPORT_SERVER = "DEVELOPER_SUPPORT_SERVER"
        let [<Literal>] DISCOVERABLE = "DISCOVERABLE"
        let [<Literal>] FEATURABLE = "FEATURABLE"
        let [<Literal>] INVITES_DISABLED = "INVITES_DISABLED"
        let [<Literal>] INVITE_SPLASH = "INVITE_SPLASH"
        let [<Literal>] MEMBER_VERIFICATION_GATE_ENABLED = "MEMBER_VERIFICATION_GATE_ENABLED"
        let [<Literal>] MORE_STICKERS = "MORE_STICKERS"
        let [<Literal>] NEWS = "NEWS"
        let [<Literal>] PARTNERED = "PARTNERED"
        let [<Literal>] PREVIEW_ENABLED = "PREVIEW_ENABLED"
        let [<Literal>] RAID_ALERTS_DISABLED = "RAID_ALERTS_DISABLED"
        let [<Literal>] ROLE_ICONS = "ROLE_ICONS"
        let [<Literal>] ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE = "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE"
        let [<Literal>] ROLE_SUBSCRIPTIONS_ENABLED = "ROLE_SUBSCRIPTIONS_ENABLED"
        let [<Literal>] TICKETED_EVENTS_ENABLED = "TICKETED_EVENTS_ENABLED"
        let [<Literal>] VANITY_URL = "VANITY_URL"
        let [<Literal>] VERIFIED = "VERIFIED"
        let [<Literal>] VIP_REGIONS = "VIP_REGIONS"
        let [<Literal>] WELCOME_SCREEN_ENABLED = "WELCOME_SCREEN_ENABLED"

    let toString (feature: GuildFeature) =
        match feature with
        | GuildFeature.ANIMATED_BANNER -> String.ANIMATED_BANNER
        | GuildFeature.ANIMATED_ICON -> String.ANIMATED_ICON
        | GuildFeature.APPLICATION_COMMAND_PERMISSIONS_V2 -> String.APPLICATION_COMMAND_PERMISSIONS_V2
        | GuildFeature.AUTO_MODERATION -> String.AUTO_MODERATION
        | GuildFeature.BANNER -> String.BANNER
        | GuildFeature.COMMUNITY -> String.COMMUNITY
        | GuildFeature.CREATOR_MONETIZABLE_PROVISIONAL -> String.CREATOR_MONETIZABLE_PROVISIONAL
        | GuildFeature.CREATOR_STORE_PAGE -> String.CREATOR_STORE_PAGE
        | GuildFeature.DEVELOPER_SUPPORT_SERVER -> String.DEVELOPER_SUPPORT_SERVER
        | GuildFeature.DISCOVERABLE -> String.DISCOVERABLE
        | GuildFeature.FEATURABLE -> String.FEATURABLE
        | GuildFeature.INVITES_DISABLED -> String.INVITES_DISABLED
        | GuildFeature.INVITE_SPLASH -> String.INVITE_SPLASH
        | GuildFeature.MEMBER_VERIFICATION_GATE_ENABLED -> String.MEMBER_VERIFICATION_GATE_ENABLED
        | GuildFeature.MORE_STICKERS -> String.MORE_STICKERS
        | GuildFeature.NEWS -> String.NEWS
        | GuildFeature.PARTNERED -> String.PARTNERED
        | GuildFeature.PREVIEW_ENABLED -> String.PREVIEW_ENABLED
        | GuildFeature.RAID_ALERTS_DISABLED -> String.RAID_ALERTS_DISABLED
        | GuildFeature.ROLE_ICONS -> String.ROLE_ICONS
        | GuildFeature.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE -> String.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE
        | GuildFeature.ROLE_SUBSCRIPTIONS_ENABLED -> String.ROLE_SUBSCRIPTIONS_ENABLED
        | GuildFeature.TICKETED_EVENTS_ENABLED -> String.TICKETED_EVENTS_ENABLED
        | GuildFeature.VANITY_URL -> String.VANITY_URL
        | GuildFeature.VERIFIED -> String.VERIFIED
        | GuildFeature.VIP_REGIONS -> String.VIP_REGIONS
        | GuildFeature.WELCOME_SCREEN_ENABLED -> String.WELCOME_SCREEN_ENABLED
        | GuildFeature.UNDOCUMENTED str -> str

    let fromString (str: string) =
        match str with
        | String.ANIMATED_BANNER -> Some GuildFeature.ANIMATED_BANNER
        | String.ANIMATED_ICON -> Some GuildFeature.ANIMATED_ICON
        | String.APPLICATION_COMMAND_PERMISSIONS_V2 -> Some GuildFeature.APPLICATION_COMMAND_PERMISSIONS_V2
        | String.AUTO_MODERATION -> Some GuildFeature.AUTO_MODERATION
        | String.BANNER -> Some GuildFeature.BANNER
        | String.COMMUNITY -> Some GuildFeature.COMMUNITY
        | String.CREATOR_MONETIZABLE_PROVISIONAL -> Some GuildFeature.CREATOR_MONETIZABLE_PROVISIONAL
        | String.CREATOR_STORE_PAGE -> Some GuildFeature.CREATOR_STORE_PAGE
        | String.DEVELOPER_SUPPORT_SERVER -> Some GuildFeature.DEVELOPER_SUPPORT_SERVER
        | String.DISCOVERABLE -> Some GuildFeature.DISCOVERABLE
        | String.FEATURABLE -> Some GuildFeature.FEATURABLE
        | String.INVITES_DISABLED -> Some GuildFeature.INVITES_DISABLED
        | String.INVITE_SPLASH -> Some GuildFeature.INVITE_SPLASH
        | String.MEMBER_VERIFICATION_GATE_ENABLED -> Some GuildFeature.MEMBER_VERIFICATION_GATE_ENABLED
        | String.MORE_STICKERS -> Some GuildFeature.MORE_STICKERS
        | String.NEWS -> Some GuildFeature.NEWS
        | String.PARTNERED -> Some GuildFeature.PARTNERED
        | String.PREVIEW_ENABLED -> Some GuildFeature.PREVIEW_ENABLED
        | String.RAID_ALERTS_DISABLED -> Some GuildFeature.RAID_ALERTS_DISABLED
        | String.ROLE_ICONS -> Some GuildFeature.ROLE_ICONS
        | String.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE -> Some GuildFeature.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE
        | String.ROLE_SUBSCRIPTIONS_ENABLED -> Some GuildFeature.ROLE_SUBSCRIPTIONS_ENABLED
        | String.TICKETED_EVENTS_ENABLED -> Some GuildFeature.TICKETED_EVENTS_ENABLED
        | String.VANITY_URL -> Some GuildFeature.VANITY_URL
        | String.VERIFIED -> Some GuildFeature.VERIFIED
        | String.VIP_REGIONS -> Some GuildFeature.VIP_REGIONS
        | String.WELCOME_SCREEN_ENABLED -> Some GuildFeature.WELCOME_SCREEN_ENABLED
        | str -> Some (GuildFeature.UNDOCUMENTED str)

    let decoder: Decoder<GuildFeature> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid GuildFeature"
        )
        
    let encoder v =
        toString v |> Encode.string

    // TODO: Should undocumented be a separate DU? Otherwise fromString/decoder can be simplified since never returns None

module OAuthConsent =
    let toString (consent: OAuthConsent) =
        match consent with
        | OAuthConsent.Consent -> "consent"
        | OAuthConsent.None -> "none"

    let fromString (str: string) =
        match str with
        | "consent" -> Some OAuthConsent.Consent
        | "none" -> Some OAuthConsent.None
        | _ -> None

    let decoder: Decoder<OAuthConsent> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid OAuthConsent"
        )
        
    let encoder v =
        toString v |> Encode.string

module AllowedMentionsParseType =
    let toString (mentions: AllowedMentionsParseType) =
        match mentions with
        | AllowedMentionsParseType.ROLES -> "roles"
        | AllowedMentionsParseType.USERS -> "users"
        | AllowedMentionsParseType.EVERYONE -> "everyone"

    let fromString (str: string) =
        match str with
        | "roles" -> Some AllowedMentionsParseType.ROLES
        | "users" -> Some AllowedMentionsParseType.USERS
        | "everyone" -> Some AllowedMentionsParseType.EVERYONE
        | _ -> None

    let decoder: Decoder<AllowedMentionsParseType> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid AllowedMentionsParseType"
        )
        
    let encoder v =
        toString v |> Encode.string

module ActivityLocationKind =
    let toString (kind: ActivityLocationKind) =
        match kind with
        | ActivityLocationKind.GUILD_CHANNEL -> "gc"
        | ActivityLocationKind.PRIVATE_CHANNEL -> "pc"

    let fromString (str: string) =
        match str with
        | "gc" -> Some ActivityLocationKind.GUILD_CHANNEL
        | "pc" -> Some ActivityLocationKind.PRIVATE_CHANNEL
        | _ -> None

    let decoder: Decoder<ActivityLocationKind> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid ActivityLocationKind"
        )

    let encoder v =
        toString v |> Encode.string

module OAuthScope =
    let toString (scope: OAuthScope) =
        match scope with
        | OAuthScope.ACTIVITIES_READ -> "activities.read"
        | OAuthScope.ACTIVITIES_WRITE -> "activities.write"
        | OAuthScope.APPLICATIONS_BUILDS_READ -> "applications.builds.read"
        | OAuthScope.APPLICATIONS_BUILDS_UPLOAD -> "applications.builds.upload"
        | OAuthScope.APPLICATIONS_COMMANDS -> "applications.commands"
        | OAuthScope.APPLICATIONS_COMMANDS_UPDATE -> "applications.commands.update"
        | OAuthScope.APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE -> "applications.commands.permissions.update"
        | OAuthScope.APPLICATIONS_ENTITLEMENTS -> "applications.entitlements"
        | OAuthScope.APPLICATIONS_STORE_UPDATE -> "applications.store.update"
        | OAuthScope.BOT -> "bot"
        | OAuthScope.CONNECTIONS -> "connections"
        | OAuthScope.DM_CHANNELS_READ -> "dm_channels.read"
        | OAuthScope.EMAIL -> "email"
        | OAuthScope.GDM_JOIN -> "gdm.join"
        | OAuthScope.GUILDS -> "guilds"
        | OAuthScope.GUILDS_JOIN -> "guilds.join"
        | OAuthScope.GUILDS_MEMBERS_READ -> "guilds.members.read"
        | OAuthScope.IDENTIFY -> "identify"
        | OAuthScope.MESSAGES_READ -> "messages.read"
        | OAuthScope.RELATIONSHIPS_READ -> "relationships.read"
        | OAuthScope.ROLE_CONNECTIONS_WRITE -> "role_connections.write"
        | OAuthScope.RPC -> "rpc"
        | OAuthScope.RPC_ACTIVITIES_WRITE -> "rpc.activities.write"
        | OAuthScope.RPC_NOTIFICATIONS_READ -> "rpc.notifications.read"
        | OAuthScope.RPC_VOICE_READ -> "rpc.voice.read"
        | OAuthScope.RPC_VOICE_WRITE -> "rpc.voice.write"
        | OAuthScope.VOICE -> "voice"
        | OAuthScope.WEBHOOK_INCOMING -> "webhook.incoming"

    let fromString (str: string) =
        match str with
        | "activities.read" -> Some OAuthScope.ACTIVITIES_READ
        | "activities.write" -> Some OAuthScope.ACTIVITIES_WRITE
        | "applications.builds.read" -> Some OAuthScope.APPLICATIONS_BUILDS_READ
        | "applications.builds.upload" -> Some OAuthScope.APPLICATIONS_BUILDS_UPLOAD
        | "applications.commands" -> Some OAuthScope.APPLICATIONS_COMMANDS
        | "applications.commands.update" -> Some OAuthScope.APPLICATIONS_COMMANDS_UPDATE
        | "applications.commands.permissions.update" -> Some OAuthScope.APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE
        | "applications.entitlements" -> Some OAuthScope.APPLICATIONS_ENTITLEMENTS
        | "applications.store.update" -> Some OAuthScope.APPLICATIONS_STORE_UPDATE
        | "bot" -> Some OAuthScope.BOT
        | "connections" -> Some OAuthScope.CONNECTIONS
        | "dm_channels.read" -> Some OAuthScope.DM_CHANNELS_READ
        | "email" -> Some OAuthScope.EMAIL
        | "gdm.join" -> Some OAuthScope.GDM_JOIN
        | "guilds" -> Some OAuthScope.GUILDS
        | "guilds.join" -> Some OAuthScope.GUILDS_JOIN
        | "guilds.members.read" -> Some OAuthScope.GUILDS_MEMBERS_READ
        | "identify" -> Some OAuthScope.IDENTIFY
        | "messages.read" -> Some OAuthScope.MESSAGES_READ
        | "relationships.read" -> Some OAuthScope.RELATIONSHIPS_READ
        | "role_connections.write" -> Some OAuthScope.ROLE_CONNECTIONS_WRITE
        | "rpc" -> Some OAuthScope.RPC
        | "rpc.activities.write" -> Some OAuthScope.RPC_ACTIVITIES_WRITE
        | "rpc.notifications.read" -> Some OAuthScope.RPC_NOTIFICATIONS_READ
        | "rpc.voice.read" -> Some OAuthScope.RPC_VOICE_READ
        | "rpc.voice.write" -> Some OAuthScope.RPC_VOICE_WRITE
        | "voice" -> Some OAuthScope.VOICE
        | "webhook.incoming" -> Some OAuthScope.WEBHOOK_INCOMING
        | _ -> None

    let decoder: Decoder<OAuthScope> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid OAuthScope"
        )

    let encoder v =
        toString v |> Encode.string

module TokenTypeHint =
    let toString (tokenTypeHint: TokenTypeHint) =
        match tokenTypeHint with
        | TokenTypeHint.ACCESS_TOKEN -> "access_token"
        | TokenTypeHint.REFRESH_TOKEN -> "refresh_token"

    let fromString (str: string) =
        match str with
        | "access_token" -> Some TokenTypeHint.ACCESS_TOKEN
        | "refresh_token" -> Some TokenTypeHint.REFRESH_TOKEN
        | _ -> None

    let decoder: Decoder<TokenTypeHint> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid TokenTypeHint"
        )

    let encoder v =
        toString v |> Encode.string
        
module GuildWidgetStyle =
    let toString (style: GuildWidgetStyle) =
        match style with
        | GuildWidgetStyle.SHIELD -> "shield"
        | GuildWidgetStyle.BANNER_1 -> "banner_1"
        | GuildWidgetStyle.BANNER_2 -> "banner_2"
        | GuildWidgetStyle.BANNER_3 -> "banner_3"
        | GuildWidgetStyle.BANNER_4 -> "banner_4"

    let fromString (str: string) =
        match str with
        | "shield" -> Some GuildWidgetStyle.SHIELD
        | "banner_1" -> Some GuildWidgetStyle.BANNER_1
        | "banner_2" -> Some GuildWidgetStyle.BANNER_2
        | "banner_3" -> Some GuildWidgetStyle.BANNER_3
        | "banner_4" -> Some GuildWidgetStyle.BANNER_4
        | _ -> None

    let decoder: Decoder<GuildWidgetStyle> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid GuildWidgetStyle"
        )

    let encoder v =
        toString v |> Encode.string
        
module GatewayCompression =
    let toString (compression: GatewayCompression) =
        match compression with
        | GatewayCompression.ZLIBSTREAM -> "zlib-stream"
        | GatewayCompression.ZSTDSTREAM -> "zstd-stream"

    let fromString (str: string) =
        match str with
        | "zlib-stream" -> Some GatewayCompression.ZLIBSTREAM
        | "zstd-stream" -> Some GatewayCompression.ZSTDSTREAM
        | _ -> None

    let decoder: Decoder<GatewayCompression> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid GatewayCompression"
        )

    let encoder v =
        toString v |> Encode.string
        
module GatewayEncoding =
    let toString (encoding: GatewayEncoding) =
        match encoding with
        | GatewayEncoding.JSON -> "json"
        | GatewayEncoding.ETF -> "etf"

    let fromString (str: string) =
        match str with
        | "json" -> Some GatewayEncoding.JSON
        | "etf" -> Some GatewayEncoding.ETF
        | _ -> None

    let decoder: Decoder<GatewayEncoding> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid GatewayEncoding"
        )

    let encoder v =
        toString v |> Encode.string
        
module Status =
    let toString (status: Status) =
        match status with
        | Status.ONLINE -> "online"
        | Status.DND -> "dnd"
        | Status.IDLE -> "idle"
        | Status.INVISIBLE -> "invisible"
        | Status.OFFLINE -> "offline"

    let fromString (str: string) =
        match str with
        | "online" -> Some Status.ONLINE
        | "dnd" -> Some Status.DND
        | "idle" -> Some Status.IDLE
        | "invisible" -> Some Status.INVISIBLE
        | "offline" -> Some Status.OFFLINE
        | _ -> None

    let decoder: Decoder<Status> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid Status"
        )

    let encoder v =
        toString v |> Encode.string

module ClientDeviceStatus =
    let toString (status: ClientDeviceStatus) =
        match status with
        | ClientDeviceStatus.ONLINE -> "online"
        | ClientDeviceStatus.IDLE -> "idle"
        | ClientDeviceStatus.DND -> "dnd"

    let fromString (str: string) =
        match str with
        | "online" -> Some ClientDeviceStatus.ONLINE
        | "idle" -> Some ClientDeviceStatus.IDLE
        | "dnd" -> Some ClientDeviceStatus.DND
        | _ -> None

    let decoder: Decoder<ClientDeviceStatus> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid ClientDeviceStatus"
        )

    let encoder v =
        toString v |> Encode.string
        
module RateLimitScope =
    let toString (scope: RateLimitScope) =
        match scope with
        | RateLimitScope.USER -> "user"
        | RateLimitScope.GLOBAL -> "global"
        | RateLimitScope.SHARED -> "shared"

    let fromString (str: string) =
        match str with
        | "user" -> Some RateLimitScope.USER
        | "global" -> Some RateLimitScope.GLOBAL
        | "shared" -> Some RateLimitScope.SHARED
        | _ -> None

    let decoder: Decoder<RateLimitScope> =
        Decode.string
        |> Decode.andThen (fun str ->
            match fromString str with
            | Some value -> Decode.succeed value
            | None -> Decode.fail $"{str} is not a valid RateLimitScope"
        )

    let encoder v =
        toString v |> Encode.string
    
// TODO: Create modules within each to define strings once
