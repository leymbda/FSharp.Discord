namespace FSharp.Discord.Rest.Old

open FSharp.Discord.Types

type ErrorResponse = {
    Code: JsonErrorCode
    Message: string
    Errors: Map<string, string>
}

type RateLimitResponse = {
    Message: string
    RetryAfter: float
    Global: bool
    Code: JsonErrorCode option
}

// This file is temporary just so the old rest doesnt break
