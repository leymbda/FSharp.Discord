namespace FSharp.Discord.Rest

open System
open System.Net.Http
open System.Text

module HttpClient =
    let private setHeader key (value: string) (client: HttpClient) =
        match client.DefaultRequestHeaders.Contains key with
        | false -> client.DefaultRequestHeaders.Add(key, value); client
        | true -> client

    let toBotClient (token: string) (client: HttpClient) =
        client
        |> setHeader "Authorization" ("Bot " + token)
        |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT
        :?> DiscordClient.BotClient
        :> IBotClient

    let toOAuthClient (token: string) (client: HttpClient) =
        client
        |> setHeader "Authorization" ("Bearer " + token)
        |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT
        :?> DiscordClient.OAuthClient
        :> IOAuthClient

    let toBasicClient (clientId: string) (clientSecret: string) (client: HttpClient) =
        let token =
            $"{clientId}:{clientSecret}"
            |> Encoding.UTF8.GetBytes
            |> Convert.ToBase64String

        client
        |> setHeader "Authorization" ("Basic " + token)
        |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT
        :?> DiscordClient.BasicClient
        :> IBasicClient

// TODO: Potentially rework this... Maybe add Microsoft.Extensions.Http so the IDiscordApiFactory can be properly implemented here instead/also?
