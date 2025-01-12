namespace rec FSharp.Discord.Types

open System
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<WebhookReceiveEventConverter>)>]
type WebhookReceiveEvent =
    | PING                   of WebhookReceiveEventPayload<unit>
    | ENTITLEMENT_CREATE     of WebhookReceiveEventPayload<WebhookReceiveEventBody<EntitlementCreateEvent>>
    | APPLICATION_AUTHORIZED of WebhookReceiveEventPayload<WebhookReceiveEventBody<ApplicationAuthorizedEvent>>

and WebhookReceiveEventConverter () =
    inherit JsonConverter<WebhookReceiveEvent> ()

    override __.Read (reader, _, _) =
        let success, document = JsonDocument.TryParseValue &reader
        JsonException.raiseIf (not success)

        let webhookType =
            document.RootElement.GetProperty "type"
            |> _.GetInt32()
            |> enum<WebhookPayloadType>

        let webhookEventType =
            try
                document.RootElement.GetProperty "data"
                |> _.GetProperty("type")
                |> _.GetString()
                |> WebhookEventType.fromString
            with | _ ->
                None

        let json = document.RootElement.GetRawText()

        match webhookType, webhookEventType with
        | WebhookPayloadType.PING, _ -> PING <| Json.deserializeF json
        | WebhookPayloadType.EVENT, Some WebhookEventType.ENTITLEMENT_CREATE -> ENTITLEMENT_CREATE <| Json.deserializeF json
        | WebhookPayloadType.EVENT, Some WebhookEventType.APPLICATION_AUTHORIZED -> APPLICATION_AUTHORIZED <| Json.deserializeF json
        | _ -> JsonException.raise "Unexpected WebhookPayloadType and/or WebhookEventType provided" // TODO: Handle gracefully for unfamiliar events
                
    override __.Write (writer, value, _) =
        match value with
        | PING p -> Json.serializeF p |> writer.WriteRawValue
        | ENTITLEMENT_CREATE e -> Json.serializeF e |> writer.WriteRawValue
        | APPLICATION_AUTHORIZED a -> Json.serializeF a |> writer.WriteRawValue
        

// https://discord.com/developers/docs/events/webhook-events#event-body-object
type WebhookReceiveEventBody<'a> = {
    [<JsonPropertyName "type">] Type: WebhookEventType
    [<JsonPropertyName "timestamp">] [<JsonConverter(typeof<JsonConverter.UnixEpoch>)>] Timestamp: DateTime
    [<JsonPropertyName "data">] Data: 'a
}

// https://discord.com/developers/docs/events/webhook-events#payload-structure
type WebhookReceiveEventPayload<'a> = {
    [<JsonPropertyName "version">] Version: int
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "type">] Type: WebhookType
    [<JsonPropertyName "event">] Event: 'a
}

// https://discord.com/developers/docs/events/webhook-events#application-authorized-application-authorized-structure
type ApplicationAuthorizedEvent = {
    [<JsonPropertyName "integration_type">] IntegrationType: ApplicationIntegrationType option
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list
    [<JsonPropertyName "guild">] Guild: Guild option
}

// https://discord.com/developers/docs/events/webhook-events#entitlement-create-entitlement-create-structure
type EntitlementCreateEvent = Entitlement
