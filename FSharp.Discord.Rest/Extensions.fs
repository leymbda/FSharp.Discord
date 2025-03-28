[<AutoOpen>]
module FSharp.Discord.Rest.Extensions

open System
open System.Net.Http
open System.Net.Http.Headers
open System.Text
open System.Web
open Thoth.Json.Net

module Task =
    let bind f v = task {
        let! v' = v
        return! f v'
    }

    let map f v = task {
        let! v' = v
        return f v'
    }

module List =
    let foldBacki folder list state =
        List.foldBack (fun cur (i, acc) -> i + 1, folder i cur acc) list (0, state) |> snd

module Uri =
    let create (parts: string list) =
        parts |> String.concat "/" |> Uri

    let withOptionalQuery (key: string) (value: string option) (uri: Uri) =
        match value with
        | None -> uri
        | Some v ->
            let uriBuilder = UriBuilder uri
            let query = HttpUtility.ParseQueryString uriBuilder.Query
            query.Add(key, v)
            uriBuilder.Query <- query.ToString()
            uriBuilder.Uri

    let withRequiredQuery (key: string) (value: string) (uri: Uri) =
        withOptionalQuery key (Some value) uri

    let toRequest (method: HttpMethod) (uri: Uri) =
        new HttpRequestMessage(method, uri)

module StringContent =
    let create (content: string) (contentType: string) =
        new StringContent(content, Encoding.UTF8, contentType)

    let createText (content: string) =
        create content "text/plain"

    let createJson (encoder: Encoder<'a>) (content: 'a) =
        create (Encode.toString 0 (encoder content)) "application/json"

    let createMedia (media: Media) =
        new StringContent(media.Content, MediaTypeHeaderValue(media.Type))

module MultipartFormDataContent =
    let create () =
        new MultipartFormDataContent()

    let withText (name: string) (content: string) (m: MultipartFormDataContent) =
        m.Add(StringContent.createText content, name)
        m

    let withJson (name: string) (encoder: Encoder<'a>) (content: 'a) (m: MultipartFormDataContent) =
        m.Add(StringContent.createJson encoder content, name)
        m

    let withMedia (name: string) (media: Media) (m: MultipartFormDataContent) =
        m.Add(StringContent.createMedia media, name, media.Name)
        m

    let withFile (index: int) (file: Media) (m: MultipartFormDataContent) =
        withMedia $"files[{index}]" file m

    let withFiles (files: Media list) (m: MultipartFormDataContent) =
        List.foldBacki withFile files m

module HttpContent =
    let createJsonWithFiles (encoder: Encoder<'a>) (payload: 'a) (files: Media list) =
        match files with
        | [] -> StringContent.createJson encoder payload :> HttpContent
        | files ->
            MultipartFormDataContent.create ()
            |> MultipartFormDataContent.withJson "payload_json" encoder payload
            |> MultipartFormDataContent.withFiles files
            :> HttpContent

module HttpRequestMessage =
    let withAuditLogReason (auditLogReason: string option) (req: HttpRequestMessage) =
        let folder (req: HttpRequestMessage) (auditLogReason: string) =
            req.Headers.Add(DiscordRequest.AUDIT_LOG_REASON_HEADER_KEY, HttpUtility.UrlEncode auditLogReason)
            req

        Option.fold folder req auditLogReason

    let withContent (content: HttpContent) (req: HttpRequestMessage) =
        req.Content <- content
        req

    let withPayload (payload: IPayload) (req: HttpRequestMessage) =
        withContent (payload.ToHttpContent()) req

type IHttpClientFactory with
    member this.CreateBotClient (token: string) =
        let client = this.CreateClient()

        client.DefaultRequestHeaders.Add("Authorization", "Bot " + token)
        client.DefaultRequestHeaders.Add("User-Agent", DiscordRequest.DEFAULT_USER_AGENT)

        { new IBotClient with
            member _.SendAsync req = client.SendAsync req }

    member this.CreateOAuthClient (token: string) =
        let client = this.CreateClient()

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token)
        client.DefaultRequestHeaders.Add("User-Agent", DiscordRequest.DEFAULT_USER_AGENT)

        { new IOAuthClient with
            member _.SendAsync req = client.SendAsync req  }

    member this.CreateBasicClient (clientId: string) (clientSecret: string) =
        let client = this.CreateClient()

        let token =
            $"{clientId}:{clientSecret}"
            |> Encoding.UTF8.GetBytes
            |> Convert.ToBase64String
                
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + token)
        client.DefaultRequestHeaders.Add("User-Agent", DiscordRequest.DEFAULT_USER_AGENT)

        { new IBasicClient with
            member _.SendAsync req = client.SendAsync req  }
