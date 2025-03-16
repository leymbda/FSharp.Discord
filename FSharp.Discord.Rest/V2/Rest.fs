module FSharp.Discord.Rest.V2.Rest

open FSharp.Discord.Types.Serialization
open System.Net.Http

let [<Literal>] API_BASE_URL = "https://discord.com/api/v10"
let [<Literal>] OAUTH_BASE_URL = "https://discord.com/api/oauth2"

let createInteractionResponse (req: CreateInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "interactions"; req.InteractionId; req.InteractionToken; "callback"]
    |> Uri.withRequiredQuery "with_response" (string req.WithResponse)
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode InteractionCallbackResponse.decoder)

let getOriginalInteractionResponse (req: GetOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)
        
let editOriginalInteractionResponse (req: EditOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

    // TODO: Does this expect the query params from the equivalent webhook endpoint?

let deleteOriginalInteractionResponse (req: DeleteOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
