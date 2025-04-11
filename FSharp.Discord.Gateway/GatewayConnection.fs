namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open System
open Thoth.Json.Net
open WebSocketSharp

type GatewayConnectionState = {
    Sequence: int option
    // TODO: Add other relevant lifecycle state here
}

module GatewayConnectionState =
    let create () = {
        Sequence = None
    }

type GatewayConnection(socket, state) =
    member val Socket: WebSocket = socket
    member val State: GatewayConnectionState = state

    interface IDisposable with
        member this.Dispose() =
            this.Socket :> IDisposable |> _.Dispose()

module GatewayConnection =
    let onOpen (state: GatewayConnectionState) = fun () ->
        ()

    let onMessage (state: GatewayConnectionState) = fun (event: GatewayReceiveEvent) ->
        ()

    let onClose (state: GatewayConnectionState) = fun (code: GatewayCloseEventCode) ->
        ()

    // TODO: Implement lifecycle through above functions
    // TODO: How to notify Gateway when GatewayConnection closes? TBD

    let create gatewayUrl initialState =
        let ws = new WebSocket(gatewayUrl)
        let state = initialState |> Option.defaultValue (GatewayConnectionState.create ())

        ws.OnOpen.Add(fun _ -> onOpen state ())

        ws.OnMessage.Add(fun (e: MessageEventArgs) ->
            e.Data
            |> Decode.fromString GatewayReceiveEvent.decoder
            |> Result.iter (onMessage state)
        )

        ws.OnClose.Add(fun e ->
            e.Code
            |> int
            |> enum<GatewayCloseEventCode>
            |> onClose state
        )

        new GatewayConnection(ws, state)

    let connect (connection: GatewayConnection) =
        connection.Socket.Connect()

    let close (connection: GatewayConnection) =
        connection.Socket.Close()

    let send event (connection: GatewayConnection) =
        match event with
        | GatewaySendEvent.REQUEST_GUILD_MEMBERS d ->
            Some {
                Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS
                Data = GatewaySendEventData.REQUEST_GUILD_MEMBERS d
                Sequence = connection.State.Sequence
                EventName = None
            }

        | _ ->
            None

        |> Option.iter (GatewaySendEventPayload.encoder >> Encode.toString 0 >> connection.Socket.Send)
