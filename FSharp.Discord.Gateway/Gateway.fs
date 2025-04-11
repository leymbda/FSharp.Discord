namespace FSharp.Discord.Gateway

open System

type Gateway(gatewayUrl) =
    member val GatewayUrl: string = gatewayUrl
    member val Connection: GatewayConnection option = None with get, set
    
    interface IDisposable with
        member this.Dispose() =
            this.Connection |> Option.iter (fun c -> c :> IDisposable |> _.Dispose())

module Gateway =
    let create gatewayUrl =
        new Gateway(gatewayUrl)

    let connect (gateway: Gateway) =
        match gateway.Connection with
        | Some _ ->
            Error "Connection already exists"

        | None ->
            gateway.Connection <- Some (GatewayConnection.create gateway.GatewayUrl None)
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
