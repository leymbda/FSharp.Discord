namespace FSharp.Discord.Rest.V2

open System
open System.Net.Http
open System.Text
open System.Threading.Tasks

type IDiscordClient =
    abstract SendAsync: HttpRequestMessage -> Task<HttpResponseMessage>

type IBotClient =
    inherit IDiscordClient

type IOAuthClient =
    inherit IDiscordClient

type IBasicClient =
    inherit IDiscordClient

[<AutoOpen>]
module Extensions =
    type IHttpClientFactory with
        member this.CreateBotClient (token: string) =
            let client = this.CreateClient()

            client.DefaultRequestHeaders.Add("Authorization", "Bot " + token)
            client.DefaultRequestHeaders.Add("User-Agent", Constants.DEFAULT_USER_AGENT)

            { new IBotClient with
                member _.SendAsync req = client.SendAsync req }

        member this.CreateOAuthClient (token: string) =
            let client = this.CreateClient()

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token)
            client.DefaultRequestHeaders.Add("User-Agent", Constants.DEFAULT_USER_AGENT)

            { new IOAuthClient with
                member _.SendAsync req = client.SendAsync req  }

        member this.CreateBasicClient (clientId: string) (clientSecret: string) =
            let client = this.CreateClient()

            let token =
                $"{clientId}:{clientSecret}"
                |> Encoding.UTF8.GetBytes
                |> Convert.ToBase64String
                
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + token)
            client.DefaultRequestHeaders.Add("User-Agent", Constants.DEFAULT_USER_AGENT)

            { new IBasicClient with
                member _.SendAsync req = client.SendAsync req  }
