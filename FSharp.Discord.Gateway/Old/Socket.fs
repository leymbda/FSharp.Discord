namespace FSharp.Discord.Gateway

//open FSharp.Discord.Types
//open System
//open System.Net.WebSockets
//open System.Text.Json
//open System.Threading
//open System.Threading.Tasks

//type ISocket =
//    inherit IObservable<(GatewayReceiveEvent * string)>

//    abstract Id: Guid

//    abstract Connect: Uri -> CancellationToken -> Task<unit>

//type Socket (ws: IWebsocket, id) =
//    member val private _observers: IObserver<(GatewayReceiveEvent * string)> list = [] with get, set

//    member val Id: Guid = id with get

//    interface ISocket with
//        member this.Subscribe observer =
//            this._observers <- this._observers @ [observer]
//            { new IDisposable with member _.Dispose () = this._observers <- this._observers |> List.except [observer] }

//        member this.Id = this.Id

//        member this.Connect uri ct = task {
//            do! ws.ConnectAsync(uri, ct)

//            let mapper = Result.mapError (fun _ -> None)
//            let binder = Result.bind (function
//                | WebsocketResponse.Close code -> Error (Option.map enum<GatewayCloseEventCode> code)
//                | WebsocketResponse.Message message -> Ok (Json.deserializeF<GatewayReceiveEvent> message, message)
//            )

//            let! res = Websocket.readNext ct ws |> Task.map (mapper >> binder)

//            match res with
//            | Error _ -> this._observers |> List.iter (_.OnCompleted()) // TODO: Pass actual error code to handle resume/reconnect
//            | Ok (ev, raw) -> this._observers |> List.iter (_.OnNext(ev, raw))
//        }

//type ISocketFactory =
//    abstract CreateSocket: unit -> ISocket

//type SocketFactory () =
//    interface ISocketFactory with
//        member _.CreateSocket () =
//            //use ws = new ClientWebSocket()
//            let wsfac = WebsocketFactory() :> IWebsocketFactory
//            let ws = wsfac.CreateClient()
//            Socket(ws, Guid.NewGuid())

//// TODO: Replace IWebsocket with this (?)
