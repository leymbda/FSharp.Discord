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

    // TODO: Figure out how to implement below signals. Could probably be done with an observable passed into the init,
    //       but may be a more standard approach too.

    /// Request the client to disconnect gracefully, resolving once terminated.
    member this.StopAsync() = async {
        raise (new NotImplementedException()) // TODO: Call Msg.Disconnect to stop gracefully
    }

    /// Send event to request guild members.
    member this.RequestGuildMembers(event) =
        GatewaySendEvent.REQUEST_GUILD_MEMBERS event
        |> ignore // TODO: Send to program (somehow)

        raise (new NotImplementedException()) // TODO: Call Msg.Send to send event

    /// Send event to request soundboard sounds.
    member this.RequestSoundboardSounds(event) =
        GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS event
        |> ignore // TODO: Send to program (somehow)

        raise (new NotImplementedException()) // TODO: Call Msg.Send to send event

    /// Send event to update voice state.
    member this.UpdateVoiceState(event) =
        GatewaySendEvent.UPDATE_VOICE_STATE event
        |> ignore // TODO: Send to program (somehow)

        raise (new NotImplementedException()) // TODO: Call Msg.Send to send event

    /// Send event to update presence.
    member this.UpdatePresence(event) =
        GatewaySendEvent.UPDATE_PRESENCE event
        |> ignore // TODO: Send to program (somehow)

        raise (new NotImplementedException()) // TODO: Call Msg.Send to send event

    /// Wait until the client disconnects synchronously.
    member this.Wait() =
        this.TaskCompletionSource.Task.Wait()

    /// Wait until the client disconnects asynchronously.
    member this.WaitAsync() =
        this.TaskCompletionSource.Task |> Async.AwaitTask

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
