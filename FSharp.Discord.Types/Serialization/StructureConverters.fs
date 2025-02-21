namespace FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Thoth.Json.Net

module Encode =
    /// Encode a map to an object with a value encoder.
    let mapv (encoder: Encoder<'a>) value =
        Map.map (fun _ v -> encoder v) value
        |> Encode.dict

    /// Encode a map to an object with a key mapper and value encoder.
    let mapkv (mapper: 'a -> string) (encoder: Encoder<'b>) value =
        value
        |> Map.toSeq
        |> Seq.map (fun (k, v) -> mapper k, encoder v)
        |> Map.ofSeq
        |> Encode.dict

module ErrorResponse =
    let decoder: Decoder<_> = Decode.object (fun get -> {
        Code = get.Required.Field "code" Decode.Enum.int<JsonErrorCode>
        Message = get.Required.Field "message" Decode.string
        Errors = get.Required.Field "errors" (Decode.dict Decode.string)
    })

    let encoder (errorResponse: ErrorResponse) = Encode.object [
        "code", Encode.Enum.int errorResponse.Code
        "message", Encode.string errorResponse.Message
        "errors", Encode.mapv Encode.string errorResponse.Errors
    ]

//module Interaction =
//    let decoder = Decode.object (fun get -> {
//        Id = get.Required.Field "id" Decode.string
//        ApplicationId = get.Required.Field "application_id" Decode.string
//        Type = get.Required.Field "type" Decode.Enum.int<InteractionType>
//        Data = get.Optional.Field "data" InteractionData.decoder
//        Guild = get.Optional.Field "guild" PartialGuild.decoder
//        GuildId = get.Optional.Field "guild_id" Decode.string
//        Channel = get.Optional.Field "channel" PartialChannel.decoder
//        ChannelId = get.Optional.Field "channel_id" Decode.string
//        Member = get.Optional.Field "member" GuildMember.decoder
//        User = get.Optional.Field "user" User.decoder
//        Token = get.Required.Field "token" Decode.string
//        Version = get.Required.Field "version" Decode.int
//        Message = get.Optional.Field "message" Message.decoder
//        AppPermissions = get.Required.Field "app_permissions" Decode.string
//        Locale = get.Optional.Field "locale" Decode.string
//        GuildLocale = get.Optional.Field "guild_locale" Decode.string
//        Entitlements = get.Required.Field "entitlements" (Decode.list Entitlement.decoder)
//        AuthorizingIntegrationOwners = get.Required.Field "authorizing_integration_owners" (Decode.dict ApplicationIntegrationTypeConfiguration.decoder) // TODO: Ensure key is a valid option
//        Context = get.Optional.Field "context" Decode.Enum.int<InteractionContextType>
//    })

//    let encoder (interaction: Interaction) = Encode.object [
//        "id", Encode.string interaction.Id
//        "application_id", Encode.string interaction.ApplicationId
//        "type", Encode.Enum.int interaction.Type
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
//        "authorizing_integration_owners", Encode.mapv ApplicationIntegrationTypeConfiguration.encoder interaction.AuthorizingIntegrationOwners
//        "context", Encode.option Encode.Enum.int interaction.Context
//    ]
