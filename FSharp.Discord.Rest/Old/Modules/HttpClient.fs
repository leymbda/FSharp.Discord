namespace FSharp.Discord.Rest.Old

open System
open System.Net.Http
open System.Text

module HttpClient =
    let private setHeader key (value: string) (client: HttpClient) =
        if client.DefaultRequestHeaders.Contains key then
            client.DefaultRequestHeaders.Remove key |> ignore

        client.DefaultRequestHeaders.Add(key, value)

    let toBotClient (token: string) (client: HttpClient) =
        client |> setHeader "Authorization" ("Bot " + token)
        client |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT

        { new IBotClient with
            member _.SendAsync req = client.SendAsync req  }

    let toOAuthClient (token: string) (client: HttpClient) =
        client |> setHeader "Authorization" ("Bearer " + token)
        client |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT

        { new IOAuthClient with
            member _.SendAsync req = client.SendAsync req  }

    let toBasicClient (clientId: string) (clientSecret: string) (client: HttpClient) =
        let token =
            $"{clientId}:{clientSecret}"
            |> Encoding.UTF8.GetBytes
            |> Convert.ToBase64String

        client |> setHeader "Authorization" ("Basic " + token)
        client |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT

        { new IBasicClient with
            member _.SendAsync req = client.SendAsync req  }
