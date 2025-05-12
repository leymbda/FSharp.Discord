open FSharp.Discord.Gateway
open FSharp.Discord.Rest
open FSharp.Discord.Types
open FsToolkit.ErrorHandling
open System
open System.Net.Http
open System.Threading.Tasks
open Thoth.Json.Net

let token = "" // TODO: Turn into secret config value

asyncResult {
    // Get gateway url from Discord
    let! res, _ = Rest.getGateway (GetGatewayRequest("10", GatewayEncoding.JSON, None)) (new HttpClient())
    let! gateway = res
    let gatewayUrl = gateway.Url

    // Create identify event
    let identify = {
        Token = token
        Intents = GatewayIntent.ALL
        Shard = None
        Properties = {
            OperatingSystem = "OperatingSystem"
            Browser = "Browser"
            Device = "Device"
        }
        Compress = None
        LargeThreshold = None
        Presence = None
    }

    // Create dispatcher
    let dispatcher (event: GatewayReceiveEvent) =
        let json = Encode.toString 0 (GatewayReceiveEvent.encoder event)
        Console.WriteLine json

    // Create gateway connection
    Gateway.create gatewayUrl identify dispatcher
    |> Gateway.connect
    |> ignore

    do! Task.Delay (1000 * 60 * 60) // arbitrary
}
|> AsyncResult.mapError (fun e ->
    Console.WriteLine $"Error: {e}"
    e
)
|> Async.Ignore
|> Async.RunSynchronously

// TODO: Rewrite gateway connection to use FSharp.Control.Websockets since WebSocketSharp only supports .NET Framework...
// At least it will be more functional and won't have the whole ugly event emitter and stuff above
