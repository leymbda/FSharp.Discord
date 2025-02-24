namespace FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Thoth.Json.Net

module ApplicationIntegrationType =
    let toString (value: ApplicationIntegrationType) =
        match value with
        | ApplicationIntegrationType.GUILD_INSTALL -> "GUILD_INSTALL"
        | ApplicationIntegrationType.USER_INSTALL -> "USER_INSTALL"
        | _ -> "" // TODO: How can this be cleanly handled? (caused by int backed enum supporting string representation)

    let fromString (str: string) =
        match str with
        | "GUILD_INSTALL" -> Some ApplicationIntegrationType.GUILD_INSTALL
        | "USER_INSTALL" -> Some ApplicationIntegrationType.USER_INSTALL
        | _ -> None

module ConnectionServiceType =
    let toString (connectionServiceType: ConnectionServiceType) =
        match connectionServiceType with
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

    let fromString (str: string) =
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

module EmbedType =
    let toString (embedType: EmbedType) =
        match embedType with
        | EmbedType.RICH -> "rich"
        | EmbedType.IMAGE -> "image"
        | EmbedType.VIDEO -> "video"
        | EmbedType.GIFV -> "gifv"
        | EmbedType.ARTICLE -> "article"
        | EmbedType.LINK -> "link"
        | EmbedType.POLL_RESULT -> "poll_result"

    let fromString (str: string) =
        match str with
        | "rich" -> Some EmbedType.RICH
        | "image" -> Some EmbedType.IMAGE
        | "video" -> Some EmbedType.VIDEO
        | "gifv" -> Some EmbedType.GIFV
        | "article" -> Some EmbedType.ARTICLE
        | "link" -> Some EmbedType.LINK
        | "poll_result" -> Some EmbedType.POLL_RESULT
        | _ -> None

module GuildIntegrationType =
    let toString (guildIntegrationType: GuildIntegrationType) =
        match guildIntegrationType with
        | GuildIntegrationType.TWITCH -> "twitch"
        | GuildIntegrationType.YOUTUBE -> "youtube"
        | GuildIntegrationType.DISCORD -> "discord"
        | GuildIntegrationType.GUILD_SUBSCRIPTION -> "guild_subscription"

    let fromString (str: string) =
        match str with
        | "twitch" -> Some GuildIntegrationType.TWITCH
        | "youtube" -> Some GuildIntegrationType.YOUTUBE
        | "discord" -> Some GuildIntegrationType.DISCORD
        | "guild_subscription" -> Some GuildIntegrationType.GUILD_SUBSCRIPTION
        | _ -> None
        
module SelectMenuDefaultValueType =
    let toString (selectMenuDefaultValue: SelectMenuDefaultValueType) =
        match selectMenuDefaultValue with
        | SelectMenuDefaultValueType.USER -> "user"
        | SelectMenuDefaultValueType.ROLE -> "role"
        | SelectMenuDefaultValueType.CHANNEL -> "channel"

    let fromString (str: string) =
        match str with
        | "user" -> Some SelectMenuDefaultValueType.USER
        | "role" -> Some SelectMenuDefaultValueType.ROLE
        | "channel" -> Some SelectMenuDefaultValueType.CHANNEL
        | _ -> None

    let decoder path v =
        let res =
            if Decode.Helpers.isString v then
                match fromString (unbox<string> v) with
                | Some value -> Some value
                | None -> None
            else
                None

        match res with
        | None -> Error (path, BadPrimitive("a select menu default value type", v))
        | Some res -> Ok res

    let encoder v =
        toString v |> Encode.string

module WebhookEventType =
    let toString (webhookEventType: WebhookEventType) =
        match webhookEventType with
        | WebhookEventType.APPLICATION_AUTHORIZED -> "APPLICATION_AUTHORIZED"
        | WebhookEventType.ENTITLEMENT_CREATE -> "ENTITLEMENT_CREATE"

    let fromString (str: string) =
        match str with
        | "APPLICATION_AUTHORIZED" -> Some WebhookEventType.APPLICATION_AUTHORIZED
        | "ENTITLEMENT_CREATE" -> Some WebhookEventType.ENTITLEMENT_CREATE
        | _ -> None

    let decoder path v =
        let res =
            if Decode.Helpers.isString v then
                match fromString (unbox<string> v) with
                | Some value -> Some value
                | None -> None
            else
                None

        match res with
        | None -> Error (path, BadPrimitive("an application event webhook type", v))
        | Some res -> Ok res

    let encoder v =
        toString v |> Encode.string
