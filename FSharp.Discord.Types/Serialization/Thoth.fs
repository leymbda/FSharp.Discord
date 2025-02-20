namespace FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Thoth.Json.Net

// TODO: Figure out why Decoder<_> seems to be necessary but isn't in docs
// TODO: See if auto encoding/decoding works (and if so, prioritize using it over doing EVERYTHING manually...)

module ErrorResponse =
    let decoder: Decoder<_> = Decode.object (fun get -> {
        Code = get.Required.Field "code" Decode.Enum.int<JsonErrorCode>
        Message = get.Required.Field "message" Decode.string
        Errors = get.Required.Field "errors" (Decode.map' Decode.string Decode.string)
    })

    let encoder (errorResponse: ErrorResponse) = Encode.object [
        "code", Encode.Enum.int errorResponse.Code
        "message", Encode.string errorResponse.Message
        "errors", Encode.map Encode.string Encode.string errorResponse.Errors
    ]

// TODO: Refactor below to use neater encoding/decoding (like above) and implement child types (but also test auto)

//module Interaction =
//    // TODO: Does authorizing_integration_owners use the stringified int type or the string name (e.g. "0" vs "GUILD_INSTALL")? Need to check by hitting API manually to check 
//    // TODO: Should authorizing_integration_owners be separated into its own type?

//    let decoder = Decode.object (fun get -> {
//        Id = get.Required.Field "id" Decode.string
//        ApplicationId = get.Required.Field "application_id" Decode.string
//        Type = get.Required.Field "type" Decode.int |> enum<InteractionType>
//        Data = get.Optional.Field "data" (Decode.object InteractionData.decoder)
//        Guild = get.Optional.Field "guild" (Decode.object PartialGuild.decoder)
//        GuildId = get.Optional.Field "guild_id" Decode.string
//        Channel = get.Optional.Field "channel" (Decode.object PartialChannel.decoder)
//        ChannelId = get.Optional.Field "channel_id" Decode.string
//        Member = get.Optional.Field "member" (Decode.object GuildMember.decoder)
//        User = get.Optional.Field "user" (Decode.object User.decoder)
//        Token = get.Required.Field "token" Decode.string
//        Version = get.Required.Field "version" Decode.int
//        Message = get.Optional.Field "message" (Decode.object Message.decoder)
//        AppPermissions = get.Required.Field "app_permissions" Decode.string
//        Locale = get.Optional.Field "locale" Decode.string
//        GuildLocale = get.Optional.Field "guild_locale" Decode.string
//        Entitlements = get.Required.Field "entitlements" (Decode.list (Decode.object Entitlement.decoder))
//        AuthorizingIntegrationOwners = get.Required.Field "authorizing_integration_owners" (Decode.map' (Decode.int |> Decode.map enum<ApplicationIntegrationType>) (Decode.object ApplicationIntegrationTypeConfiguration.decoder))
//        Context = get.Optional.Field "context" Decode.int |> Option.map enum<InteractionContextType>
//    })

//    let encoder (interaction: Interaction) = Encode.object [
//        "id", Encode.string interaction.Id
//        "application_id", Encode.string interaction.ApplicationId
//        "type", Encode.int (interaction.Type |> int)
//        "data", Encode.option InteractionData.encoder interaction.Data
//        "guild", Encode.option Guild.encoder interaction.Guild
//        "guild_id", Encode.option Encode.string interaction.GuildId
//        "channel", Encode.option PartialChannel.encoder interaction.Channel
//        "channel_id", Encode.option Encode.string interaction.ChannelId
//        "member", Encode.option GuildMember.encoder interaction.Member
//        "user", Encode.option User.encoder interaction.User
//        "token", Encode.string interaction.Token
//        "version", Encode.int interaction.Version
//        "message", Encode.option Message.encoder interaction.Message
//        "app_permissions", Encode.string interaction.AppPermissions
//        "locale", Encode.option Encode.string interaction.Locale
//        "guild_locale", Encode.option Encode.string interaction.GuildLocale
//        "entitlements", Encode.list <| List.map Entitlement.encoder interaction.Entitlements
//        "authorizing_integration_owners", Encode.object <| (interaction.AuthorizingIntegrationOwners |> Map.toList |> List.map (fun (k, v) -> (string k, ApplicationIntegrationTypeConfiguration.encoder v)))
//        "context", Encode.option Encode.int (interaction.Context |> Option.map int)
//    ]
