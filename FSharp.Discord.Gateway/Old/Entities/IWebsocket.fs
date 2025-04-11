namespace FSharp.Discord.Gateway

//open System
//open System.Net.WebSockets
//open System.Threading
//open System.Threading.Tasks

///// A client to connect to a websocket server.
//type IWebsocket =
//    inherit IAsyncDisposable

//    /// Connect to websocket server.
//    abstract ConnectAsync:
//        uri: Uri * ct: CancellationToken ->
//        Task

//    /// Close the websocket connection.
//    abstract CloseAsync:
//        status: WebSocketCloseStatus * description: string * ct: CancellationToken ->
//        Task

//    /// Receive data from the websocket.
//    abstract ReceiveAsync:
//        buffer: ArraySegment<byte> * ct: CancellationToken ->
//        Task<WebSocketReceiveResult>

//    /// Send data to the websocket.
//    abstract SendAsync:
//        buffer: ArraySegment<byte> * messageType: WebSocketMessageType * endOfMessage: bool * ct: CancellationToken ->
//        Task

///// A factory to create websocket clients.
//type IWebsocketFactory =
//    /// Create a new websocket client.
//    abstract CreateClient: unit -> IWebsocket

//type WebsocketFactory () =
//    interface IWebsocketFactory with
//        member _.CreateClient () =
//            let ws = new ClientWebSocket()

//            { new IWebsocket with
//                member _.ConnectAsync (uri, ct) = ws.ConnectAsync(uri, ct)
//                member _.CloseAsync (status, description, ct) = ws.CloseAsync(status, description, ct)
//                member _.ReceiveAsync (buffer, ct) = ws.ReceiveAsync(buffer, ct)
//                member _.SendAsync (buffer, messageType, endOfMessage, ct) = ws.SendAsync(buffer, messageType, endOfMessage, ct)
//                member _.DisposeAsync () = ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None) |> ValueTask }
