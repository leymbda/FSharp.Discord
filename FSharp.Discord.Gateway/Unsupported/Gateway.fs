namespace FSharp.Discord.Gateway.Unsupported

open FSharp.Discord.Gateway
open FSharp.Discord.Types
open System

type Gateway(gatewayUrl, identify, dispatcher) =
    member val GatewayUrl: string = gatewayUrl
    member val IdentifyEvent: IdentifySendEvent = identify
    member val Dispatcher: Dispatcher = dispatcher
    member val Connection: GatewayConnection option = None with get, set
    
    interface IDisposable with
        member this.Dispose() =
            this.Connection |> Option.iter (fun c -> c :> IDisposable |> _.Dispose())

module Gateway =
    let create gatewayUrl identify dispatcher =
        new Gateway(gatewayUrl, identify, dispatcher)

    let connect (gateway: Gateway) =
        match gateway.Connection with
        | Some _ ->
            Error "Connection already exists"

        | None ->
            let connection = GatewayConnection.create gateway.GatewayUrl gateway.IdentifyEvent None gateway.Dispatcher
            gateway.Connection <- Some connection

            GatewayConnection.connect connection

            Ok ()

    let close (gateway: Gateway) =
        match gateway.Connection with
        | None ->
            Error "No connection exists"

        | Some connection ->
            GatewayConnection.close connection
            gateway.Connection <- None
            Ok ()
        
    let requestGuildMembers event (gateway: Gateway) =
        gateway.Connection |> Option.iter (GatewayConnection.send (GatewaySendEvent.REQUEST_GUILD_MEMBERS event))

    let requestSoundboardSounds event (gateway: Gateway) =
        gateway.Connection |> Option.iter (GatewayConnection.send (GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS event))

    let updateVoiceState event (gateway: Gateway) =
        gateway.Connection |> Option.iter (GatewayConnection.send (GatewaySendEvent.UPDATE_VOICE_STATE event))

    let updatePresence event (gateway: Gateway) =
        gateway.Connection |> Option.iter (GatewayConnection.send (GatewaySendEvent.UPDATE_PRESENCE event))
