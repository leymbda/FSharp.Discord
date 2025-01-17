namespace FSharp.Discord.Rest

open System.Net.Http
open System.Threading.Tasks

[<Interface>]
type IBotClient =
    abstract SendAsync: HttpRequestMessage -> Task<HttpResponseMessage>

[<Interface>]
type IOAuthClient =
    abstract SendAsync: HttpRequestMessage -> Task<HttpResponseMessage>

[<Interface>]
type IBasicClient =
    abstract SendAsync: HttpRequestMessage -> Task<HttpResponseMessage>

[<Interface>]
type IDiscordApiClientFactory =
    abstract CreateBotClient: token: string -> IBotClient
    abstract CreateOAuthClient: token: string -> IOAuthClient
    abstract CreateBasicClient: clientId: string -> clientSecret: string -> IBasicClient

[<Interface>]
type IDiscordApiClientFactoryEnv =
    abstract DiscordApiClientFactory: IDiscordApiClientFactory
