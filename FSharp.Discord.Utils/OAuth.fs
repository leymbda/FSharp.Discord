module FSharp.Discord.Utils.OAuth

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open System.Web

let authorizationUrl (clientId: string) (redirectUri: string) (scopes: OAuthScope list) (state: string option) (prompt: OAuthConsent) (integrationType: ApplicationIntegrationType option) =
    let builder = UriBuilder "https://discord.com/oauth2/authorize"

    let query = HttpUtility.ParseQueryString String.Empty
    query.Add("client_id", clientId)
    query.Add("redirect_uri", redirectUri)
    query.Add("response_type", "code")
    query.Add("scope", scopes |> List.map OAuthScope.toString |> String.concat " ")
    match state with | Some s -> query.Add("state", s) | None -> ()
    query.Add("prompt", OAuthConsent.toString prompt)
    match integrationType with | Some i -> query.Add("integration_type", i |> int |> string) | None -> ()

    builder.Query <- query.ToString()

    builder.Uri.AbsoluteUri

// TODO: Add implicit flow builder https://discord.com/developers/docs/topics/oauth2#implicit-grant
// TODO: Add bot authorization flow builder https://discord.com/developers/docs/topics/oauth2#bot-authorization-flow-bot-auth-parameters
// TODO: Add Webhook flow builder https://discord.com/developers/docs/topics/oauth2#webhooks
