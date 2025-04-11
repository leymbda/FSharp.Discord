namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net
open WebSocketSharp

type GatewayState = {
    Sequence: int option
    // TODO: Add other relevant lifecycle state here
}

type Gateway = {
    Socket: WebSocket
    State: GatewayState option
}

module Gateway =
    let create gatewayUrl =
        let ws = new WebSocket(gatewayUrl)

        ws.OnOpen.Add(fun _ -> ()) // TODO: Handle initiating lifecycle
        ws.OnMessage.Add(fun (e: MessageEventArgs) -> ()) // TODO: Handle lifecycle events then event handler
        ws.OnError.Add(fun (e: ErrorEventArgs) -> ()) // TODO: Figure out when this occurs and what it should handle
        ws.OnClose.Add(fun (e: CloseEventArgs) -> ()) // TODO: Handle resume/reconnect if appropriate (involves creating new ws ??)

        ws

    let private send event gateway =
        gateway.State |> Option.iter (fun state ->
            match event with
            | GatewaySendEvent.REQUEST_GUILD_MEMBERS d ->
                Some {
                    Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS
                    Data = GatewaySendEventData.REQUEST_GUILD_MEMBERS d
                    Sequence = state.Sequence
                    EventName = None
                }

            | _ ->
                None

            |> Option.iter (GatewaySendEventPayload.encoder >> Encode.toString 0 >> gateway.Socket.Send)
        )

    let requestGuildMembers event gateway =
        send (GatewaySendEvent.REQUEST_GUILD_MEMBERS event) gateway

    let requestSoundboardSounds event gateway =
        send (GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS event) gateway

    let updateVoiceState event gateway =
        send (GatewaySendEvent.UPDATE_VOICE_STATE event) gateway

    let updatePresence event gateway =
        send (GatewaySendEvent.UPDATE_PRESENCE event) gateway

    let close (gateway: Gateway) =
        gateway.Socket.Close()
