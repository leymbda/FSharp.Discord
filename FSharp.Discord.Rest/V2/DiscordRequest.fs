module FSharp.Discord.Rest.V2.DiscordRequest

open System.Net.Http
open System.Web

/// Append the audit log reason header to the request.
let withAuditLogReason (auditLogReason: string option) (req: HttpRequestMessage) =
    let folder (req: HttpRequestMessage) (auditLogReason: string) =
        req.Headers.Add("X-Audit-Log-Reason", HttpUtility.UrlEncode auditLogReason)
        req

    Option.fold folder req auditLogReason
