module FSharp.Discord.Rest.V2.Constants

open System.Reflection

let [<Literal>] DISCORD_API_URL = "https://discord.com/api/v10"
let [<Literal>] DISCORD_OAUTH_URL = "https://discord.com/api/oauth2"

let DEFAULT_USER_AGENT =
    let version =
        try Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
        with | _ -> "0.0.0+dev"

    // TODO: Test if the version is correctly read here (both when testing as-is and when published as package)
    // TODO: Will the assembly version ever not be present? Can the try..catch be removed?
    // TODO: If not, decide on an appropriate name for the testing version

    $"DiscordBot (https://github.com/leydel/FSharp.Discord, {version})"

module Headers =
    let [<Literal>] LIMIT_KEY = "X-RateLimit-Limit"
    let [<Literal>] REMAINING_KEY = "X-RateLimit-Remaining"
    let [<Literal>] RESET_KEY = "X-RateLimit-Reset"
    let [<Literal>] RESET_AFTER_KEY = "X-RateLimit-ResetAfter"
    let [<Literal>] BUCKET_KEY = "X-RateLimit-Bucket"
    let [<Literal>] GLOBAL_KEY = "X-RateLimit-Global"
    let [<Literal>] SCOPE_KEY = "X-RateLimit-Scope"

    let [<Literal>] AUDIT_LOG_REASON = "X-Audit-Log-Reason"
    