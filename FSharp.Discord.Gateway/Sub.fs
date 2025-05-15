module FSharp.Discord.Gateway.Sub

open Elmish
open System
open System.Threading
open System.Threading.Tasks

let delay (timespan: TimeSpan) (callback: Dispatch<'msg> -> unit) (dispatch: Dispatch<'msg>) =
    use cts = new CancellationTokenSource()

    Task.Delay(timespan, cts.Token)
    |> _.ContinueWith(fun _ -> callback dispatch)
    |> ignore

    { new IDisposable with
        member _.Dispose () = cts.Cancel() }
