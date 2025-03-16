namespace rec FSharp.Discord.Rest

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open System.Net.Http
open System.Reflection
open System.Threading.Tasks

type IDiscordClient =
    abstract SendAsync: HttpRequestMessage -> Task<HttpResponseMessage>

type IBotClient =
    inherit IDiscordClient

type IOAuthClient =
    inherit IDiscordClient

type IBasicClient =
    inherit IDiscordClient

type Media = {
    Content: string
    Type: string
    Name: string
}

type IPayload =
    abstract ToHttpContent: unit -> HttpContent

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
    let [<Literal>] LIMIT_HEADER_KEY = "X-RateLimit-Limit"
    let [<Literal>] REMAINING_HEADER_KEY = "X-RateLimit-Remaining"
    let [<Literal>] RESET_HEADER_KEY = "X-RateLimit-Reset"
    let [<Literal>] RESET_AFTER_HEADER_KEY = "X-RateLimit-ResetAfter"
    let [<Literal>] BUCKET_HEADER_KEY = "X-RateLimit-Bucket"
    let [<Literal>] GLOBAL_HEADER_KEY = "X-RateLimit-Global"
    let [<Literal>] SCOPE_HEADER_KEY = "X-RateLimit-Scope"

    let fromResponse (res: HttpResponseMessage) =
        let getOptionalHeader (key: string) (res: HttpResponseMessage) =
            match res.Headers.TryGetValues key with
            | true, v -> v |> Seq.tryHead
            | false, _ -> None

        {
            Limit = res |> getOptionalHeader LIMIT_HEADER_KEY |> Option.map Int32.Parse
            Remaining = res |> getOptionalHeader REMAINING_HEADER_KEY |> Option.map int
            Reset = res |> getOptionalHeader RESET_HEADER_KEY |> Option.map DateTime.Parse
            ResetAfter = res |> getOptionalHeader RESET_AFTER_HEADER_KEY |> Option.map Double.Parse
            Bucket = res |> getOptionalHeader BUCKET_HEADER_KEY
            Global = res |> getOptionalHeader GLOBAL_HEADER_KEY |> Option.map bool.Parse
            Scope = res |> getOptionalHeader SCOPE_HEADER_KEY |> Option.bind RateLimitScope.fromString
        }

module DiscordRequest =
    let [<Literal>] AUDIT_LOG_REASON_HEADER_KEY = "X-Audit-Log-Reason"

    let DEFAULT_USER_AGENT =
        let version =
            try Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
            with | _ -> "0.0.0+dev"

        // TODO: Test if the version is correctly read here (both when testing as-is and when published as package)
        // TODO: Will the assembly version ever not be present? Can the try..catch be removed?
        // TODO: If not, decide on an appropriate name for the testing version

        $"DiscordBot (https://github.com/leydel/FSharp.Discord, {version})"
