module FSharp.Discord.Rest.V2.Rest

open FSharp.Discord.Types.Serialization
open System.Net.Http

let [<Literal>] API_BASE_URL = "https://discord.com/api/v10"
let [<Literal>] OAUTH_BASE_URL = "https://discord.com/api/oauth2"

// https://discord.com/developers/docs/interactions/receiving-and-responding#create-interaction-response
let createInteractionResponse (req: CreateInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "interactions"; req.InteractionId; req.InteractionToken; "callback"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
    
// https://discord.com/developers/docs/interactions/receiving-and-responding#create-interaction-response
let createInteractionResponseWithCallback (req: CreateInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "interactions"; req.InteractionId; req.InteractionToken; "callback"]
    |> Uri.withRequiredQuery "with_response" "true"
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode InteractionCallbackResponse.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#get-original-interaction-response
let getOriginalInteractionResponse (req: GetOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)
    
// https://discord.com/developers/docs/interactions/receiving-and-responding#edit-original-interaction-response
let editOriginalInteractionResponse (req: EditOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#delete-original-interaction-response
let deleteOriginalInteractionResponse (req: DeleteOriginalInteractionResponseRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.InteractionId; req.InteractionToken; "messages"; "@original"]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
