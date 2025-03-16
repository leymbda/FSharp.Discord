namespace FSharp.Discord.Rest.V2

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System.Net
open System.Net.Http
open System.Threading.Tasks
open Thoth.Json.Net

type DiscordApiError =
    | ClientError of ErrorResponse
    | RateLimit of RateLimitResponse
    | Serialization of string
    | Unexcepted of HttpStatusCode

type DiscordResponse<'a> = Result<'a, DiscordApiError> * RateLimitHeaders

module DiscordResponse =
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

        return content, RateLimitHeaders.fromResponse res
    }

    /// Resolve the response to the given type from the decoder.
    let decode<'a> (decoder: Decoder<'a>) (res: HttpResponseMessage): Task<DiscordResponse<'a>> =
        map (Decode.fromString decoder) res

    /// Resolve the response to unit if the response is successful.
    let unit (res: HttpResponseMessage): Task<DiscordResponse<unit>> =
        map (fun _ -> Ok ()) res

    /// Resolve the response body to the type if a body is present, otherwise return None.
    let tryDecode<'a> (decoder: Decoder<'a>) (res: HttpResponseMessage): Task<DiscordResponse<'a option>> =
        match res.Content.Headers.ContentLength |> Option.ofNullable with
        | Some l when l > 0 -> Decode.fromString decoder >> Result.map Some
        | _ -> fun _ -> Ok None
        |> fun mapper -> map mapper res
