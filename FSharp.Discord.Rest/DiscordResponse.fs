namespace FSharp.Discord.Rest

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System.Net
open System.Net.Http
open System.Threading.Tasks
open Thoth.Json.Net

// https://discord.com/developers/docs/reference#error-messages
type ErrorResponse = {
    Code: JsonErrorCode
    Message: string
    Errors: Map<string, string>
}

module ErrorResponse =
    module Property =
        let [<Literal>] Code = "code"
        let [<Literal>] Message = "message"
        let [<Literal>] Errors = "errors"

    let decoder: Decoder<ErrorResponse> =
        Decode.object (fun get -> {
            Code = get |> Get.required Property.Code Decode.Enum.int<JsonErrorCode>
            Message = get |> Get.required Property.Message Decode.string
            Errors = get |> Get.required Property.Errors (Decode.dict Decode.string)
        })

    let encoder (v: ErrorResponse) =
        Encode.object ([]
            |> Encode.required Property.Code Encode.Enum.int v.Code
            |> Encode.required Property.Message Encode.string v.Message
            |> Encode.required Property.Errors (Encode.mapv Encode.string) v.Errors
        )
        
// https://discord.com/developers/docs/topics/rate-limits#exceeding-a-rate-limit-rate-limit-response-structure
type RateLimitResponse = {
    Message: string
    RetryAfter: float
    Global: bool
    Code: JsonErrorCode option
}

module RateLimitResponse =
    module Property =
        let [<Literal>] Message = "message"
        let [<Literal>] RetryAfter = "retry_after"
        let [<Literal>] Global = "global"
        let [<Literal>] Code = "code"

    let decoder: Decoder<RateLimitResponse> =
        Decode.object (fun get -> {
            Message = get |> Get.required Property.Message Decode.string
            RetryAfter = get |> Get.required Property.RetryAfter Decode.float
            Global = get |> Get.required Property.Global Decode.bool
            Code = get |> Get.optional Property.Code Decode.Enum.int<JsonErrorCode>
        })

    let encoder (v: RateLimitResponse) =
        Encode.object ([]
            |> Encode.required Property.Message Encode.string v.Message
            |> Encode.required Property.RetryAfter Encode.float v.RetryAfter
            |> Encode.required Property.Global Encode.bool v.Global
            |> Encode.optional Property.Code Encode.Enum.int v.Code
        )

type DiscordApiError =
    | ClientError of ErrorResponse
    | RateLimit of RateLimitResponse
    | Serialization of string
    | Unexcepted of HttpStatusCode

type DiscordResponse<'a> = Result<'a, DiscordApiError> * RateLimitHeaders

module DiscordResponse =
    let private map'<'a> (mapper: string -> Result<'a, string>) (res: HttpResponseMessage) = task {
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
        map' (Decode.fromString decoder) res

    /// Resolve the response to unit if the response is successful.
    let unit (res: HttpResponseMessage): Task<DiscordResponse<unit>> =
        map' (fun _ -> Ok ()) res

    /// Resolve the response to the raw string if successful.
    let string (res: HttpResponseMessage): Task<DiscordResponse<string>> =
        map' (fun s -> Ok s) res

    /// Resolve the response body to the type if a body is present, otherwise return None.
    let tryDecode<'a> (decoder: Decoder<'a>) (res: HttpResponseMessage): Task<DiscordResponse<'a option>> =
        match res.Content.Headers.ContentLength |> Option.ofNullable with
        | Some l when l > 0 -> Decode.fromString decoder >> Result.map Some
        | _ -> fun _ -> Ok None
        |> fun mapper -> map' mapper res

    let map (f: 'a -> 'b) (res: DiscordResponse<'a>): DiscordResponse<'b> =
        match res with
        | Ok a, r -> Ok (f a), r
        | Error e, r -> Error e, r
