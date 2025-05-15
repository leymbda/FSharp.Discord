namespace FSharp.Discord.Gateway

open Elmish
open System
open System.Collections.Generic
open System.Threading.Tasks

type Observable<'msg>() =
    member val Observers = new Dictionary<Guid, IObserver<'msg>>()

    interface IObservable<'msg> with
        member this.Subscribe(observer) =
            let guid = Guid.NewGuid()
            this.Observers.Add(guid, observer)
            { new IDisposable with member _.Dispose() = this.Observers.Remove(guid) |> ignore }

    member this.Send(msg) =
        this.Observers.Values
        |> List.ofSeq
        |> List.iter (_.OnNext(msg))

type MessageInterceptor<'msg>() =
    inherit Observable<'msg>()

    member this.AwaitMessage(comparer: 'msg -> bool) = async {
        let tcs = TaskCompletionSource()
        use _ = this.Subscribe(fun msg -> if comparer msg then tcs.SetResult())
        do! tcs.Task |> Async.AwaitTask
    }

type MessageForwarder<'msg>(id: string list) =
    inherit Observable<'msg>()

    member this.Subscription: Sub<'msg> =
        let handler (dispatch: Dispatch<'msg>) =
            this.Subscribe dispatch

        [id, handler]

type MessageObservable<'msg>(id: string list) =
    member val Interceptor = new MessageInterceptor<'msg>()
    member val Forwarder = new MessageForwarder<'msg>(id)
