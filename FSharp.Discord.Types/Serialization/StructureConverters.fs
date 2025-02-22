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
    module Property =
        let [<Literal>] Code = "code"
        let [<Literal>] Message = "message"
        let [<Literal>] Errors = "errors"

    let decoder path v =
        Decode.object (fun get -> {
            Code = get.Required.Field Property.Code Decode.Enum.int<JsonErrorCode>
            Message = get.Required.Field Property.Message Decode.string
            Errors = get.Required.Field Property.Errors (Decode.dict Decode.string)
        }) path v

    let encoder (v: ErrorResponse) =
        Encode.object [
            Property.Code, Encode.Enum.int v.Code
            Property.Message, Encode.string v.Message
            Property.Errors, Encode.mapv Encode.string v.Errors
        ]

module Interaction =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] ApplicationId = "application_id"
        let [<Literal>] Type = "type"
        let [<Literal>] Data = "data"
        let [<Literal>] Guild = "guild"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] Channel = "channel"
        let [<Literal>] ChannelId = "channel_id"
        let [<Literal>] Member = "member"
        let [<Literal>] User = "user"
        let [<Literal>] Token = "token"
        let [<Literal>] Version = "version"
        let [<Literal>] Message = "message"
        let [<Literal>] AppPermissions = "app_permissions"
        let [<Literal>] Locale = "locale"
        let [<Literal>] GuildLocale = "guild_locale"
        let [<Literal>] Entitlements = "entitlements"
        let [<Literal>] AuthorizingIntegrationOwners = "authorizing_integration_owners"
        let [<Literal>] Context = "context"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            ApplicationId = get.Required.Field Property.ApplicationId Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<InteractionType>
            Data = get.Optional.Field Property.Data InteractionData.decoder
            Guild = get.Optional.Field Property.Guild PartialGuild.decoder
            GuildId = get.Optional.Field Property.GuildId Decode.string
            Channel = get.Optional.Field Property.Channel PartialChannel.decoder
            ChannelId = get.Optional.Field Property.ChannelId Decode.string
            Member = get.Optional.Field Property.Member GuildMember.decoder
            User = get.Optional.Field Property.User User.decoder
            Token = get.Required.Field Property.Token Decode.string
            Version = get.Required.Field Property.Version Decode.int
            Message = get.Optional.Field Property.Message Message.decoder
            AppPermissions = get.Required.Field Property.AppPermissions Decode.string
            Locale = get.Optional.Field Property.Locale Decode.string
            GuildLocale = get.Optional.Field Property.GuildLocale Decode.string
            Entitlements = get.Required.Field Property.Entitlements (Decode.list Entitlement.decoder)
            AuthorizingIntegrationOwners = get.Required.Field Property.AuthorizingIntegrationOwners (Decode.dict ApplicationIntegrationTypeConfiguration.decoder) // TODO: Ensure key is a valid option
            Context = get.Optional.Field Property.Context Decode.Enum.int<InteractionContextType>
        }) path v

    let encoder (v: Interaction) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.ApplicationId, Encode.string v.ApplicationId
            Property.Type, Encode.Enum.int v.Type
            Property.Data, Encode.option InteractionData.encoder v.Data
            Property.Guild, Encode.option Guild.encoder v.Guild
            Property.GuildId, Encode.option Encode.string v.GuildId
            Property.Channel, Encode.option PartialChannel.encoder v.Channel
            Property.ChannelId, Encode.option Encode.string v.ChannelId
            Property.Member, Encode.option GuildMember.encoder v.Member
            Property.User, Encode.option User.encoder v.User
            Property.Token, Encode.string v.Token
            Property.Version, Encode.int v.Version
            Property.Message, Encode.option Message.encoder v.Message
            Property.AppPermissions, Encode.string v.AppPermissions
            Property.Locale, Encode.option Encode.string v.Locale
            Property.GuildLocale, Encode.option Encode.string v.GuildLocale
            Property.Entitlements, Encode.list <| List.map Entitlement.encoder v.Entitlements
            Property.AuthorizingIntegrationOwners, Encode.mapv ApplicationIntegrationTypeConfiguration.encoder v.AuthorizingIntegrationOwners
            Property.Context, Encode.option Encode.Enum.int v.Context
        ]

module ApplicationCommandData =
    module Property =
        let [<Literal>] Id = "id"
        let [<Literal>] Name = "name"
        let [<Literal>] Type = "type"
        let [<Literal>] Resolved = "resolved"
        let [<Literal>] Options = "options"
        let [<Literal>] GuildId = "guild_id"
        let [<Literal>] TargetId = "target_id"

    let decoder path v =
        Decode.object (fun get -> {
            Id = get.Required.Field Property.Id Decode.string
            Name = get.Required.Field Property.Name Decode.string
            Type = get.Required.Field Property.Type Decode.Enum.int<ApplicationCommandType>
            Resolved = get.Required.Field Property.Resolved ResolvedData.decoder
            Options = get.Optional.Field Property.Options (Decode.list ApplicationCommandOption.decoder)
            GuildId = get.Optional.Field Property.GuildId Decode.string
            TargetId = get.Optional.Field Property.TargetId Decode.string
        }) path v

    let encoder (v: ApplicationCommandData) =
        Encode.object [
            Property.Id, Encode.string v.Id
            Property.Name, Encode.string v.Name
            Property.Type, Encode.Enum.int v.Type
            Property.Resolved, ResolvedData.encoder v.Resolved
            Property.Options, Encode.option (Encode.list ApplicationCommandOption.encoder) v.Options
            Property.GuildId, Encode.option Encode.string v.GuildId
            Property.TargetId, Encode.option Encode.string v.TargetId
        ]

module MessageComponentData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] ComponentType = "component_type"
        let [<Literal>] Values = "values"
        let [<Literal>] Resolved = "resolved"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get.Required.Field Property.CustomId Decode.string
            ComponentType = get.Required.Field Property.ComponentType Decode.Enum.int<ComponentType>
            Values = get.Optional.Field Property.Values (Decode.list SelectMenuOption.decoder)
            Resolved = get.Optional.Field Property.Resolved ResolvedData.decoder
        }) path v

    let encoder (v: MessageComponentData) =
        Encode.object [
            Property.CustomId, Encode.string v.CustomId
            Property.ComponentType, Encode.Enum.int v.ComponentType
            Property.Values, Encode.option (Encode.list SelectMenuOption.encoder) v.Values
            Property.Resolved, Encode.option ResolvedData.encoder v.Resolved
        ]

module ModalSubmitData =
    module Property =
        let [<Literal>] CustomId = "custom_id"
        let [<Literal>] Components = "components"

    let decoder path v =
        Decode.object (fun get -> {
            CustomId = get.Required.Field Property.CustomId Decode.string
            Components = get.Required.Field Property.Components (Decode.list Component.decoder)
        }) path v

    let encoder (v: ModalSubmitData) =
        Encode.object [
            Property.CustomId, Encode.string v.CustomId
            Property.Components, Encode.list Component.encoder v.Components
        ]

module InteractionData =
    let decoder path v =
        Decode.oneOf [
            Decode.map InteractionData.APPLICATION_COMMAND ApplicationCommandData.decoder
            Decode.map InteractionData.MESSAGE_COMPONENT MessageComponentData.decoder
            Decode.map InteractionData.MODAL_SUBMIT ModalSubmitData.decoder
        ] path v

    let encoder (v: InteractionData) =
        match v with
        | InteractionData.APPLICATION_COMMAND data -> ApplicationCommandData.encoder data
        | InteractionData.MESSAGE_COMPONENT data -> MessageComponentData.encoder data
        | InteractionData.MODAL_SUBMIT data -> ModalSubmitData.encoder data
