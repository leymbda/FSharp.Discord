namespace FSharp.Discord.Rest

open System
open System.Net.Http
open System.Text

type BotClient () =
    inherit HttpClient ()

type OAuthClient () =
    inherit HttpClient ()

type BasicClient () =
    inherit HttpClient ()

type DiscordClient =
    | Bot of BotClient
    | OAuth of OAuthClient
    | Basic of BasicClient
with
    member this.SendAsync req =
        match this with
        | Bot c -> c.SendAsync req
        | OAuth c -> c.SendAsync req
        | Basic c -> c.SendAsync req

module HttpClient =
    let private setHeader key (value: string) (client: HttpClient) =
        match client.DefaultRequestHeaders.Contains key with
        | false -> client.DefaultRequestHeaders.Add(key, value); client
        | true -> client

    let toBotClient (token: string) (client: HttpClient) =
        client
        |> setHeader "Authorization" ("Bot " + token)
        |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT
        :?> BotClient

    let toOAuthClient (token: string) (client: HttpClient) =
        client
        |> setHeader "Authorization" ("Bearer " + token)
        |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT
        :?> OAuthClient

    let toBasicClient (clientId: string) (clientSecret: string) (client: HttpClient) =
        let token =
            $"{clientId}:{clientSecret}"
            |> Encoding.UTF8.GetBytes
            |> Convert.ToBase64String

        client
        |> setHeader "Authorization" ("Basic " + token)
        |> setHeader "User-Agent" Constants.DEFAULT_USER_AGENT
        :?> BasicClient

// TODO: Potentially refactor this so it isn't using HttpClient-inherited classes for distinguishing authentication types for clients
