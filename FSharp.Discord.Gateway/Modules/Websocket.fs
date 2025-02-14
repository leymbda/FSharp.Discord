namespace FSharp.Discord.Gateway

open System
open System.IO
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading.Tasks

type WebsocketResponse =
    | Message of string
    | Close of int option

type WebsocketError =
    | NotConnected
    | Closed

type IWebsocket =
    inherit IDisposable

    abstract ConnectAsync:
        uri: Uri * ct: CancellationToken ->
        Task

    abstract ReceiveAsync:
        buffer: ArraySegment<byte> * ct: CancellationToken ->
        Task<WebSocketReceiveResult>

    abstract SendAsync:
        buffer: ArraySegment<byte> * messageType: WebSocketMessageType * endOfMessage: bool * ct: CancellationToken ->
        Task

module Websocket =
    let readNext (ct: CancellationToken) (ws: IWebsocket) = task {
        try
            use ms = new MemoryStream()

            let mutable messageType = WebSocketMessageType.Text
            let mutable closeStatus: int option = None
            let mutable isEndOfMessage = false

            while not isEndOfMessage do
                let buffer = Array.zeroCreate<byte> 4096 |> ArraySegment
                let! res = ws.ReceiveAsync(buffer, ct)

                ms.Write(buffer.Array, buffer.Offset, res.Count)
                messageType <- res.MessageType
                closeStatus <- res.CloseStatus |> Option.ofNullable |> Option.map int
                isEndOfMessage <- res.EndOfMessage

            ms.Seek(0, SeekOrigin.Begin) |> ignore
            use sr = new StreamReader(ms)
            let! message = sr.ReadToEndAsync()

            match messageType with
            | WebSocketMessageType.Close -> return  Ok (WebsocketResponse.Close closeStatus)
            | _ -> return Ok (WebsocketResponse.Message message)
        with
        | :? InvalidOperationException -> return Error WebsocketError.NotConnected
        | :? WebSocketException -> return Error WebsocketError.Closed
    }

    let write (message: string) (ct: CancellationToken) (ws: IWebsocket) = task {
        try
            let bytes = Encoding.UTF8.GetBytes message
            let size = 4096

            let mutable offset = 0
            let mutable isEndOfMessage = false

            while not isEndOfMessage do
                let count = Math.Min(size, bytes.Length - offset)
                let buffer = ArraySegment(bytes, offset, count)
                offset <- offset + size
                isEndOfMessage <- offset >= bytes.Length

                do! ws.SendAsync(buffer, WebSocketMessageType.Text, isEndOfMessage, ct)

            return Ok ()
        with
        | :? InvalidOperationException -> return Error WebsocketError.NotConnected
        | :? WebSocketException -> return Error WebsocketError.Closed
    }
