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
            |> Result.map (fun v -> WebhookReceiveEvent.PING {
                Version = v.Version
                ApplicationId = v.ApplicationId
            })

        | Ok WebhookPayloadType.EVENT ->
            let eventType =
                Decode.field WebhookEventBody.Property.Type WebhookEventType.decoder
                |> Decode.field WebhookEventPayload.Property.Event
                |> Decode.option
                |> fun f -> f path v
                |> Result.bind (function | Some v -> Ok v | None -> Error (path, BadPrimitive("a webhook event type", v)))

            let event (payload: WebhookEventPayload<'a>) =
                match payload.Event.Value.Data with
                | None -> Error (path, BadType("missing webhook event data", v))
                | Some data ->
                    Ok {
                        Version = payload.Version
                        ApplicationId = payload.ApplicationId
                        Timestamp = payload.Event.Value.Timestamp
                        Data = data
                    }

                // TODO: Clean this up so it doesnt use `.Value` (it is true based on how `eventType` works but not obvious)

            match eventType with
            | Error err ->
                Error err

            | Ok WebhookEventType.ENTITLEMENT_CREATE ->
                WebhookEventPayload.decoder EntitlementCreateEvent.decoder path v
                |> Result.bind event
                |> Result.map WebhookReceiveEvent.ENTITLEMENT_CREATE

            | Ok WebhookEventType.APPLICATION_AUTHORIZED ->
                WebhookEventPayload.decoder ApplicationAuthorizedEvent.decoder path v
                |> Result.bind event
                |> Result.map WebhookReceiveEvent.APPLICATION_AUTHORIZED

        | _ ->
            Error (path, BadPrimitive("a webhook payload type", v))

    let encoder (v: WebhookReceiveEvent) =
        let event t p = {
            Version = p.Version
            ApplicationId = p.ApplicationId
            Type = WebhookPayloadType.EVENT
            Event = Some {
                Type = t
                Timestamp = p.Timestamp
                Data = Some p.Data
            }
        }

        match v with
        | WebhookReceiveEvent.PING p ->
            WebhookEventPayload.encoder Encode.unit {
                Version = p.Version
                ApplicationId = p.ApplicationId
                Type = WebhookPayloadType.PING
                Event = None
            }

        | WebhookReceiveEvent.ENTITLEMENT_CREATE p ->
            WebhookEventPayload.encoder
                EntitlementCreateEvent.encoder
                (event WebhookEventType.ENTITLEMENT_CREATE p)

        | WebhookReceiveEvent.APPLICATION_AUTHORIZED p ->
            WebhookEventPayload.encoder
                ApplicationAuthorizedEvent.encoder
                (event WebhookEventType.APPLICATION_AUTHORIZED p)
