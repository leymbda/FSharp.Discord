namespace FSharp.Discord.Types

open System.Collections.Generic
open System.Text.Json.Serialization

type RateLimitResponse = {
    [<JsonPropertyName "message">] Message: string
    [<JsonPropertyName "retry_after">] RetryAfter: float
    [<JsonPropertyName "global">] Global: bool
    [<JsonPropertyName "code">] Code: JsonErrorCode option
}

type ErrorResponse = {
    [<JsonPropertyName "code">] Code: JsonErrorCode
    [<JsonPropertyName "message">] Message: string
    [<JsonPropertyName "errors">] Errors: IDictionary<string, string>
}
