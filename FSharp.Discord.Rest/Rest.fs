module FSharp.Discord.Rest.Rest

open FSharp.Discord.Types.Serialization
open System.Net.Http
open Thoth.Json.Net

let [<Literal>] API_BASE_URL = "https://discord.com/api/v10"
let [<Literal>] OAUTH_BASE_URL = "https://discord.com/api/oauth2"

// ----- Interactions: Receiving and Responding -----

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

// https://discord.com/developers/docs/interactions/receiving-and-responding#create-followup-message
let createFollowupMessage (req: CreateFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#get-followup-message
let getFollowupMessage (req: GetFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken; "messages"; req.MessageId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/interactions/receiving-and-responding#edit-followup-message
let editFollowupMessage (req: EditFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken; "messages"; req.MessageId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode Message.decoder)

// https://discord.com/developers/docs/resources/webhook#delete-webhook-message
let deleteFollowupMessage (req: DeleteFollowupMessageRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "webhooks"; req.ApplicationId; req.InteractionToken; "messages"; req.MessageId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// ----- Interactions: Application Commands -----

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-commands
let getGlobalApplicationCommands (req: GetGlobalApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"]
    |> Uri.withOptionalQuery "with_localizations" (Option.map string req.withLocalizations)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#create-global-application-command
let createGlobalApplicationCommand (req: CreateGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-command
let getGlobalApplicationCommand (req: GetGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#edit-global-application-command
let editGlobalApplicationCommand (req: EditGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#delete-global-application-command
let deleteGlobalApplicationCommand (req: DeleteGlobalApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
    
// https://discord.com/developers/docs/interactions/application-commands#bulk-overwrite-global-application-commands
let bulkOverwriteGlobalApplicationCommands (req: BulkOverwriteGlobalApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "commands"]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-commands
let getGuildApplicationCommands (req: GetGuildApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.withOptionalQuery "with_localizations" (Option.map string req.withLocalizations)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#create-guild-application-command
let createGuildApplicationCommand (req: CreateGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.toRequest HttpMethod.Post
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#get-global-application-command
let getGuildApplicationCommand (req: GetGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#edit-guild-application-command
let editGuildApplicationCommand (req: EditGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Patch
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode ApplicationCommand.decoder)

// https://discord.com/developers/docs/interactions/application-commands#create-guild-application-command
let deleteGuildApplicationCommand (req: DeleteGuildApplicationCommandRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId]
    |> Uri.toRequest HttpMethod.Delete
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit

// https://discord.com/developers/docs/interactions/application-commands#bulk-overwrite-guild-application-commands
let bulkOverwriteGuildApplicationCommands (req: BulkOverwriteGuildApplicationCommandsRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list ApplicationCommand.decoder))

// https://discord.com/developers/docs/interactions/application-commands#get-guild-application-command-permissions
let getGuildApplicationCommandPermissions (req: GetGuildApplicationCommandPermissionsRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; "permissions"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode (Decode.list GuildApplicationCommandPermissions.decoder))

// https://discord.com/developers/docs/interactions/application-commands#get-application-command-permissions
let getApplicationCommandPermissions (req: GetApplicationCommandPermissionsRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId; "permissions"]
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode GuildApplicationCommandPermissions.decoder)

// https://discord.com/developers/docs/interactions/application-commands#edit-application-command-permissions
let editApplicationCommandPermissions (req: EditApplicationCommandPermissionsRequest) (client: IOAuthClient) =
    Uri.create [API_BASE_URL; "applications"; req.ApplicationId; "guilds"; req.GuildId; "commands"; req.CommandId; "permissions"]
    |> Uri.toRequest HttpMethod.Put
    |> HttpRequestMessage.withPayload req.Payload
    |> client.SendAsync
    |> Task.bind DiscordResponse.unit
    
// ----- Events: Using Gateway -----

// https://discord.com/developers/docs/events/gateway#get-gateway
let getGateway (req: GetGatewayRequest) (client: HttpClient) = // TODO: This should probably use an interface rather than the concrete HttpClient
    Uri.create [API_BASE_URL; "gateway"]
    |> Uri.withRequiredQuery "v" req.Version
    |> Uri.withRequiredQuery "encoding" (GatewayEncoding.toString req.Encoding)
    |> Uri.withOptionalQuery "compress" (Option.map GatewayCompression.toString req.Compression)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode GetGatewayResponse.decoder)

// https://discord.com/developers/docs/events/gateway#get-gateway-bot
let getGatewayBot (req: GetGatewayBotRequest) (client: IBotClient) =
    Uri.create [API_BASE_URL; "gateway"; "bot"]
    |> Uri.withRequiredQuery "v" req.Version
    |> Uri.withRequiredQuery "encoding" (GatewayEncoding.toString req.Encoding)
    |> Uri.withOptionalQuery "compress" (Option.map GatewayCompression.toString req.Compression)
    |> Uri.toRequest HttpMethod.Get
    |> client.SendAsync
    |> Task.bind (DiscordResponse.decode GetGatewayBotResponse.decoder)

// ----- Resources: Application -----

// ----- Resources: Application Role Connection Metadata -----

// ----- Resources: Audit Log -----

// ----- Resources: Auto Moderation -----

// ----- Resources: Channel -----

// ----- Resources: Emoji -----

// ----- Resources: Entitlement -----

// ----- Resources: Guild -----

// ----- Resources: Guild Scheduled Event -----

// ----- Resources: Guild Template -----

// ----- Resources: Invite -----

// ----- Resources: Lobby -----

// ----- Resources: Message -----

// ----- Resources: Poll -----

// ----- Resources: SKU -----

// ----- Resources: Soundboard -----

// ----- Resources: Stage Instance -----

// ----- Resources: Sticker -----

// ----- Resources: Subscription -----

// ----- Resources: User -----

// ----- Resources: Voice -----

// ----- Resources: Webhook -----

// ----- Topics: OAuth2 -----
