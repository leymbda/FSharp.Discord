namespace FSharp.Discord.Gateway

open Elmish
open FSharp.Discord.Types
open System
open System.Threading.Tasks

type GatewayClient(gatewayUri: Uri, identify: IdentifySendEvent, handler: Gateway.Handler) =
    member val private TaskCompletionSource = TaskCompletionSource()

    /// Start the client.
    member this.Start() =
        Program.mkProgram Gateway.init Gateway.update (fun _ _ -> ())
        |> Program.withSubscription Gateway.subscribe
        |> Program.withTermination Gateway.terminate (fun _ -> this.TaskCompletionSource.SetResult())
        |> Program.runWith (gatewayUri, identify, handler)

    /// Start the client, resolving once the gateway connection is ready.
    member this.StartAsync() = async {
        raise (new NotImplementedException()) // TODO: Wait until connection is ready before resolving
    }

    /// Stop the client immediately.
    member this.Stop() =
        this.TaskCompletionSource.SetResult()

    /// Request the client to disconnect gracefully, resolving once terminated.
    member this.StopAsync() = async {
        raise (new NotImplementedException()) // TODO: Call Msg.Disconnect to stop gracefully
    }

    /// Wait until the client disconnects synchronously.
    member this.Wait() =
        this.TaskCompletionSource.Task.Wait()

    /// Wait until the client disconnects asynchronously.
    member this.WaitAsync() =
        this.TaskCompletionSource.Task |> Async.AwaitTask
    