namespace FSharp.Discord.Gateway

open Elmish
open FSharp.Discord.Types
open System
open System.Threading.Tasks

type GatewayClient(gatewayUri: Uri, identify: IdentifySendEvent, handler: Gateway.Handler) =
    member val private TerminationTaskSource = TaskCompletionSource()

    member val private Observable = MessageObservable<Gateway.Msg>(["clientevent"])

    /// Start the client.
    member this.Start() =
        let update msg =
            this.Observable.Interceptor.Send msg
            Gateway.update msg

        let subscribe model =
            Sub.batch [
                this.Observable.Forwarder.Subscription
                Gateway.subscribe model
            ]

        Program.mkProgram Gateway.init update (fun _ _ -> ())
        |> Program.withSubscription subscribe
        |> Program.withTermination Gateway.terminate (fun _ -> this.TerminationTaskSource.SetResult())
        |> Program.runWith (gatewayUri, identify, handler)

    /// Start the client, resolving once the gateway connection is ready.
    member this.StartAsync() = async {
        this.Start()

        do! this.Observable.Interceptor.AwaitMessage (function
            | Gateway.Msg.Lifecycle (Lifecycle.Msg.Ready _) -> true
            | Gateway.Msg.Lifecycle (Lifecycle.Msg.Resumed) -> true
            | _ -> false
        )
    }
    
    /// Wait until the client disconnects synchronously.
    member this.Wait() =
        this.TerminationTaskSource.Task.Wait()

    /// Wait until the client disconnects asynchronously.
    member this.WaitAsync() =
        this.TerminationTaskSource.Task |> Async.AwaitTask

    /// Stop the client immediately.
    member this.Stop() =
        this.TerminationTaskSource.SetResult() // TODO: How to handle disconnecting the ws from inside the program state?

    /// Request the client to disconnect gracefully, resolving once terminated.
    member this.StopAsync() = async {
        AsyncPerformMsg.Perform ()
        |> Gateway.Msg.Disconnect
        |> this.Observable.Forwarder.Send

        do! this.WaitAsync()
    }

    /// Send event to request guild members.
    member this.RequestGuildMembers(event) =
        GatewaySendEvent.REQUEST_GUILD_MEMBERS event
        |> AsyncAttemptMsg.Attempt
        |> Gateway.Msg.Send
        |> this.Observable.Forwarder.Send

    /// Send event to request soundboard sounds.
    member this.RequestSoundboardSounds(event) =
        GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS event
        |> AsyncAttemptMsg.Attempt
        |> Gateway.Msg.Send
        |> this.Observable.Forwarder.Send

    /// Send event to update voice state.
    member this.UpdateVoiceState(event) =
        GatewaySendEvent.UPDATE_VOICE_STATE event
        |> AsyncAttemptMsg.Attempt
        |> Gateway.Msg.Send
        |> this.Observable.Forwarder.Send

    /// Send event to update presence.
    member this.UpdatePresence(event) =
        GatewaySendEvent.UPDATE_PRESENCE event
        |> AsyncAttemptMsg.Attempt
        |> Gateway.Msg.Send
        |> this.Observable.Forwarder.Send

    interface IDisposable with
        member this.Dispose() =
            this.Stop()
    
    interface IAsyncDisposable with
        member this.DisposeAsync() =
            async {
                do! this.StopAsync()
            }
            |> Async.StartAsTask
            |> ValueTask
