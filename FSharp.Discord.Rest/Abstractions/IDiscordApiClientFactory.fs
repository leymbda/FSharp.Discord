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
    abstract BotClient: token: string -> IBotClient
    abstract OAuthClient: token: string -> IOAuthClient
    abstract BasicClient: clientId: string -> clientSecret: string -> IBasicClient
