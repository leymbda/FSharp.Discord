namespace FSharp.Discord.Webhook

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open Thoth.Json.Net

type WebhookReceiveEventMetadata = {
    Version: int
    ApplicationId: string
    Timestamp: DateTime option
}

type WebhookReceiveEvent =
    | PING                   of                              WebhookReceiveEventMetadata
    | APPLICATION_AUTHORIZED of ApplicationAuthorizedEvent * WebhookReceiveEventMetadata
    | ENTITLEMENT_CREATE     of EntitlementCreateEvent     * WebhookReceiveEventMetadata

module WebhookReceiveEvent =
    let decoder: Decoder<WebhookReceiveEvent> =
        WebhookEventPayload.decoder
        |> Decode.andThen (fun payload ->
            let metadata = {
                Version = payload.Version
                ApplicationId = payload.ApplicationId
                Timestamp = payload.Event |> Option.map _.Timestamp
            }

            match payload.Type, payload.Event with
            | WebhookPayloadType.PING, _ ->
                Decode.succeed (WebhookReceiveEvent.PING metadata)

            | WebhookPayloadType.EVENT, Some body ->
                match body.Type, body.Data with
                | WebhookEventType.APPLICATION_AUTHORIZED, Some (WebhookEventData.ApplicationAuthorized e) ->
                    Decode.succeed (WebhookReceiveEvent.APPLICATION_AUTHORIZED (e, metadata))

                | WebhookEventType.ENTITLEMENT_CREATE, Some (WebhookEventData.EntitlementCreate e) ->
                    Decode.succeed (WebhookReceiveEvent.ENTITLEMENT_CREATE (e, metadata))

                | _ ->
                    Decode.fail "Unexpected webhook event data received"

            | _ ->
                Decode.fail "Unexpected webhook payload data received"
        )

    let encoder (v: WebhookReceiveEvent) =
        match v with
        | WebhookReceiveEvent.PING metadata ->
            WebhookEventPayload.encoder {
                Type = WebhookPayloadType.PING
                Version = metadata.Version
                ApplicationId = metadata.ApplicationId
                Event = None
            }

        | WebhookReceiveEvent.APPLICATION_AUTHORIZED (event, metadata) ->
            WebhookEventPayload.encoder {
                Type = WebhookPayloadType.EVENT
                Version = metadata.Version
                ApplicationId = metadata.ApplicationId
                Event = Some {
                    Type = WebhookEventType.APPLICATION_AUTHORIZED
                    Timestamp = metadata.Timestamp |> Option.get
                    Data = Some (WebhookEventData.ApplicationAuthorized event)
                }
            }

        | WebhookReceiveEvent.ENTITLEMENT_CREATE (event, metadata) ->
            WebhookEventPayload.encoder {
                Type = WebhookPayloadType.EVENT
                Version = metadata.Version
                ApplicationId = metadata.ApplicationId
                Event = Some {
                    Type = WebhookEventType.ENTITLEMENT_CREATE
                    Timestamp = metadata.Timestamp |> Option.get
                    Data = Some (WebhookEventData.EntitlementCreate event)
                }
            }
            
// TODO: Make all values pascal case
