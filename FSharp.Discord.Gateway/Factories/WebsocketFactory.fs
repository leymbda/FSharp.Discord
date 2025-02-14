namespace FSharp.Discord.Gateway

open System.Net.WebSockets

type IWebsocketFactory =
    abstract CreateClient: unit -> IWebsocket

type WebsocketFactory () =
    interface IWebsocketFactory with
        member _.CreateClient () =
            let ws = new ClientWebSocket()

            { new IWebsocket with
                member _.ConnectAsync (uri, ct) = ws.ConnectAsync(uri, ct)
                member _.ReceiveAsync (buffer, ct) = ws.ReceiveAsync(buffer, ct)
                member _.SendAsync (buffer, messageType, endOfMessage, ct) = ws.SendAsync(buffer, messageType, endOfMessage, ct)
                member _.Dispose () = ws.Dispose() }
