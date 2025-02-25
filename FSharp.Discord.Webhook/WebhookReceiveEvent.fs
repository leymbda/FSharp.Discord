namespace FSharp.Discord.Webhook

open FSharp.Discord.Types

type WebhookReceiveEvent =
    | PING                   of WebhookEventPayload<unit>
    | ENTITLEMENT_CREATE     of WebhookEventPayload<EntitlementCreateEvent>
    | APPLICATION_AUTHORIZED of WebhookEventPayload<ApplicationAuthorizedEvent>
