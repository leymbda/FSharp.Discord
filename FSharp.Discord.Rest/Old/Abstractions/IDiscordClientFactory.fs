namespace FSharp.Discord.Rest.Old

open System.Net.Http
open System.Threading.Tasks

type IDiscordClient =
    abstract SendAsync: HttpRequestMessage -> Task<HttpResponseMessage>

type IBotClient =
    inherit IDiscordClient

type IOAuthClient =
    inherit IDiscordClient

type IBasicClient =
    inherit IDiscordClient

type IDiscordClientFactory =
    abstract CreateBotClient: token: string -> IBotClient
    abstract CreateOAuthClient: token: string -> IOAuthClient
    abstract CreateBasicClient: clientId: string -> clientSecret: string -> IBasicClient
