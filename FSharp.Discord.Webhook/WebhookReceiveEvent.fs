namespace FSharp.Discord.Webhook

open FSharp.Discord.Types
open System

type WebhookPingEvent = {
    Version: int
    ApplicationId: string
}

type WebhookEvent<'a> = {
    Version: int
    ApplicationId: string
    Timestamp: DateTime
    Data: 'a
}

type WebhookReceiveEvent =
    | PING                   of WebhookPingEvent
    | ENTITLEMENT_CREATE     of WebhookEvent<EntitlementCreateEvent>
    | APPLICATION_AUTHORIZED of WebhookEvent<ApplicationAuthorizedEvent>
