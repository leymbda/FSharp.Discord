namespace FSharp.Discord.Rest.V2

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open System.Net
open System.Net.Http
open Thoth.Json.Net

type DiscordApiError =
    | ClientError of ErrorResponse
    | RateLimit of RateLimitResponse
    | Serialization of string
    | Unexcepted of HttpStatusCode

type RateLimitHeaders = {
    Limit: int option
    Remaining: int option
    Reset: DateTime option
    ResetAfter: double option
    Bucket: string option
    Global: bool option
    Scope: RateLimitScope option
}

module DiscordResponse =
    let private rateLimits (res: HttpResponseMessage) =
        let getOptionalHeader (key: string) (res: HttpResponseMessage) =
            match res.Headers.TryGetValues key with
            | true, v -> v |> Seq.tryHead
            | false, _ -> None

        {
            Limit = res |> getOptionalHeader Constants.Headers.LIMIT_KEY |> Option.map Int32.Parse
            Remaining = res |> getOptionalHeader Constants.Headers.REMAINING_KEY |> Option.map int
            Reset = res |> getOptionalHeader Constants.Headers.RESET_KEY |> Option.map DateTime.Parse
            ResetAfter = res |> getOptionalHeader Constants.Headers.RESET_AFTER_KEY |> Option.map Double.Parse
            Bucket = res |> getOptionalHeader Constants.Headers.BUCKET_KEY
            Global = res |> getOptionalHeader Constants.Headers.GLOBAL_KEY |> Option.map bool.Parse
            Scope = res |> getOptionalHeader Constants.Headers.SCOPE_KEY |> Option.bind RateLimitScope.fromString
        }

    let private map<'a> (mapper: string -> Result<'a, string>) (res: HttpResponseMessage) = task {
        let! json = res.Content.ReadAsStringAsync()

        let content =
            match int res.StatusCode with
            | v when v >= 200 && v < 300 ->
                mapper json
                |> Result.mapError Serialization

            | v when v = 429 ->
                Decode.fromString RateLimitResponse.decoder json
                |> function | Error s -> Serialization s | Ok e -> RateLimit e
                |> Error

            | v when v >= 400 && v < 500 ->
                Decode.fromString ErrorResponse.decoder json
                |> function | Error s -> Serialization s | Ok e -> ClientError e
                |> Error

            | _ ->
                Unexcepted res.StatusCode
                |> Error

        return content, rateLimits res
    }

    /// Resolve the response to the given type from the decoder.
    let decode<'a> (decoder: Decoder<'a>) (res: HttpResponseMessage) =
        map (Decode.fromString decoder) res

    /// Resolve the response to unit if the response is successful.
    let unit (res: HttpResponseMessage) =
        map (fun _ -> Ok ()) res

    /// Resolve the response body to the type if a body is present, otherwise return None.
    let tryDecode<'a> (decoder: Decoder<'a>) (res: HttpResponseMessage) =
        match res.Content.Headers.ContentLength |> Option.ofNullable with
        | Some l when l > 0 -> Decode.fromString decoder >> Result.map Some
        | _ -> fun _ -> Ok None
        |> fun mapper -> map mapper res
