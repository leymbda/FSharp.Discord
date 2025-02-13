namespace FSharp.Discord.Gateway

open System
open System.IO
open System.Net.WebSockets
open System.Text
open System.Threading

type WebsocketReadResponse =
    | Message of string
    | Close of int option

type WebsocketWriteError =
    | NotConnected
    | Closed

module Websocket =
    let readNext (ws: ClientWebSocket) (ct: CancellationToken) = task {
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
        | WebSocketMessageType.Close -> return WebsocketReadResponse.Close closeStatus
        | _ -> return WebsocketReadResponse.Message message
    }

    let write (message: string) (ws: ClientWebSocket) (ct: CancellationToken) = task {
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
        | :? InvalidOperationException -> return Error WebsocketWriteError.NotConnected
        | :? WebSocketException -> return Error WebsocketWriteError.Closed
    }
