namespace FSharp.Discord.Webhook

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type WebhookReceiveEvent =
    | PING
    | APPLICATION_AUTHORIZED of ApplicationAuthorizedEvent
    | ENTITLEMENT_CREATE     of EntitlementCreateEvent

module WebhookReceiveEvent =
    let decoder: Decoder<WebhookReceiveEvent> =
        WebhookEventPayload.decoder
        |> Decode.andThen (fun payload ->
            match payload.Type, payload.Event with
            | WebhookPayloadType.PING, _ ->
                Decode.succeed WebhookReceiveEvent.PING

            | WebhookPayloadType.EVENT, Some body ->
                match body.Type, body.Data with
                | WebhookEventType.APPLICATION_AUTHORIZED, Some (WebhookEventData.APPLICATION_AUTHORIZED e) ->
                    Decode.succeed (WebhookReceiveEvent.APPLICATION_AUTHORIZED e)

                | WebhookEventType.ENTITLEMENT_CREATE, Some (WebhookEventData.ENTITLEMENT_CREATE e) ->
                    Decode.succeed (WebhookReceiveEvent.ENTITLEMENT_CREATE e)

                | _ ->
                    Decode.fail "Unexpected webhook event data received"

            | _ ->
                Decode.fail "Unexpected webhook payload data received"
        )

    let encoder: Encoder<WebhookReceiveEvent> = raise (System.NotImplementedException()) // TODO: Include outer data in decoded result to preserve and support encoding then implement encoder
