namespace FSharp.Discord.Rest

open System.Net.Http

type DiscordClient =
    | Bot of IBotClient
    | OAuth of IOAuthClient
    | Basic of IBasicClient

module DiscordClient =
    type BotClient () =
        inherit HttpClient()
        interface IBotClient with
            member this.SendAsync req = this.SendAsync req

    type OAuthClient () =
        inherit HttpClient()
        interface IOAuthClient with
            member this.SendAsync req = this.SendAsync req

    type BasicClient () =
        inherit HttpClient()
        interface IBasicClient with
            member this.SendAsync req = this.SendAsync req

    let sendAsync req (client: DiscordClient) =
        match client with
        | Bot c -> c.SendAsync req
        | OAuth c -> c.SendAsync req
        | Basic c -> c.SendAsync req
