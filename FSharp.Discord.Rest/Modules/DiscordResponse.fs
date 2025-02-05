namespace FSharp.Discord.Rest

open FSharp.Discord.Types
open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Text.Json
open System.Threading.Tasks

type RateLimitHeaders = {
    Limit: int option
    Remaining: int option
    Reset: DateTime option
    ResetAfter: double option
    Bucket: string option
    Global: bool option
    Scope: RateLimitScope option
}

module RateLimitHeaders =
    let [<Literal>] LIMIT_KEY = "X-RateLimit-Limit"
    let [<Literal>] REMAINING_KEY = "X-RateLimit-Remaining"
    let [<Literal>] RESET_KEY = "X-RateLimit-Reset"
    let [<Literal>] RESET_AFTER_KEY = "X-RateLimit-ResetAfter"
    let [<Literal>] BUCKET_KEY = "X-RateLimit-Bucket"
    let [<Literal>] GLOBAL_KEY = "X-RateLimit-Global"
    let [<Literal>] SCOPE_KEY = "X-RateLimit-Scope"

    let fromResponseHeaders (headers: HttpResponseHeaders) =
        let getOptionalHeader (key: string) (headers: HttpResponseHeaders) =
            match headers.TryGetValues key with
            | true, v -> v |> Seq.tryHead
            | false, _ -> None

        {
            Limit = headers |> getOptionalHeader LIMIT_KEY |> Option.map Int32.Parse
            Remaining = headers |> getOptionalHeader REMAINING_KEY |> Option.map int
            Reset = headers |> getOptionalHeader RESET_KEY |> Option.map DateTime.Parse
            ResetAfter = headers |> getOptionalHeader RESET_AFTER_KEY |> Option.map Double.Parse
            Bucket = headers |> getOptionalHeader BUCKET_KEY
            Global = headers |> getOptionalHeader GLOBAL_KEY |> Option.map bool.Parse
            Scope = headers |> getOptionalHeader SCOPE_KEY |> Option.bind RateLimitScope.fromString
        }

type ResponseWithMetadata<'a> = {
    Data: 'a
    RateLimitHeaders: RateLimitHeaders
    Status: HttpStatusCode
}

type DiscordResponse<'a> = Result<ResponseWithMetadata<'a>, ResponseWithMetadata<DiscordError>>

module DiscordResponse =
    let inline private (?>) v f = Task.map f v
    let inline private (<?) f v = v ?> f

    let private withMetadata<'a> (res: HttpResponseMessage) (obj: 'a) = {
        Data = obj
        RateLimitHeaders = RateLimitHeaders.fromResponseHeaders res.Headers
        Status = res.StatusCode
    }

    let private toJson<'a> (res: HttpResponseMessage) =
        res.Content.ReadAsStringAsync() ?> Json.deserializeF<'a>

    // Used in requests that return content in a success result
    let asJson<'a> (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | _ when res.IsSuccessStatusCode -> return! (toJson<'a> res) ?> withMetadata res ?> Ok
        | v when v = 429 -> return! RateLimit <? (toJson res) ?> withMetadata res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (toJson res) ?> withMetadata res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withMetadata res |> Error
    }

    // Used in requests that do not return content in a success result
    let asEmpty (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | _ when res.IsSuccessStatusCode -> return () |> withMetadata res |> Ok
        | v when v = 429 -> return! RateLimit <? (toJson res) ?> withMetadata res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (toJson res) ?> withMetadata res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withMetadata res |> Error
    }

    /// Used in requests that may return a content or no content success result
    let asOptionalJson<'a> (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | _ when res.IsSuccessStatusCode ->
            let length = res.Content.Headers.ContentLength |> Option.ofNullable

            match length with
            | Some l when l = 0L -> return Option<'a>.None |> withMetadata res |> Ok
            | _ -> return! (toJson res) ?> withMetadata res ?> Ok

        | v when v = 429 ->
            return! RateLimit <? (toJson res) ?> withMetadata res ?> Error

        | v when v >= 400 && v < 500 ->
            return! ClientError <? (toJson res) ?> withMetadata res ?> Error
        | _ ->
            return Unexpected (res.StatusCode) |> withMetadata res |> Error
    }

    let asRaw (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | _ when res.IsSuccessStatusCode -> return! res.Content.ReadAsStringAsync() ?> withMetadata res ?> Ok
        | v when v = 429 -> return! RateLimit <? (toJson res) ?> withMetadata res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (toJson res) ?> withMetadata res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withMetadata res |> Error 
    }

    let unwrap<'a> (res: DiscordResponse<'a>) =
        match res with
        | Ok { Data = v } -> v
        | Error _ -> failwith "Unsuccessful discord response was unwrapped"

    let toOption<'a> (res: DiscordResponse<'a>) =
        match res with
        | Error _ -> None
        | Ok { Data = v } -> Some v
