namespace FSharp.Discord.Webhook.Serialization

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open FSharp.Discord.Webhook
open Thoth.Json.Net

module WebhookReceiveEvent =
    let decoder path v =
        let webhookType = Decode.field WebhookEventPayload.Property.Type Decode.Enum.int<WebhookPayloadType> path v

        match webhookType with
        | Error err ->
            Error err

        | Ok WebhookPayloadType.PING ->
            WebhookEventPayload.decoder Decode.unit path v
            |> Result.map WebhookReceiveEvent.PING

        | Ok WebhookPayloadType.EVENT ->
            let eventType =
                Decode.field WebhookEventBody.Property.Type WebhookEventType.decoder
                |> Decode.field WebhookEventPayload.Property.Event
                |> Decode.option
                |> fun f -> f path v
                |> Result.bind (function | Some v -> Ok v | None -> Error (path, BadPrimitive("a webhook event type", v)))

            match eventType with
            | Error err ->
                Error err

            | Ok WebhookEventType.ENTITLEMENT_CREATE ->
                WebhookEventPayload.decoder EntitlementCreateEvent.decoder path v
                |> Result.map WebhookReceiveEvent.ENTITLEMENT_CREATE

            | Ok WebhookEventType.APPLICATION_AUTHORIZED ->
                WebhookEventPayload.decoder ApplicationAuthorizedEvent.decoder path v
                |> Result.map WebhookReceiveEvent.APPLICATION_AUTHORIZED

        | _ ->
            Error (path, BadPrimitive("a webhook payload type", v))

    let encoder (v: WebhookReceiveEvent) =
        match v with
        | WebhookReceiveEvent.PING p -> WebhookEventPayload.encoder Encode.unit p
        | WebhookReceiveEvent.ENTITLEMENT_CREATE p -> WebhookEventPayload.encoder EntitlementCreateEvent.encoder p
        | WebhookReceiveEvent.APPLICATION_AUTHORIZED p -> WebhookEventPayload.encoder ApplicationAuthorizedEvent.encoder p

    // TODO: The underlying type doesn't guarantee correct format as it involves options where this validates when they
    //       should and shouldn't be optional. Probably worth redesigning this WebhookReceiveEvent to have its own
    //       entirely different structure that is domain specific, but this should work for now.
