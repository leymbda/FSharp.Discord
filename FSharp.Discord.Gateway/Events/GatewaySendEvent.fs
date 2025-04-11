namespace FSharp.Discord.Gateway

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type GatewaySendEvent =
    | IDENTIFY                  of IdentifySendEvent
    | RESUME                    of ResumeSendEvent
    | HEARTBEAT                 of int option
    | REQUEST_GUILD_MEMBERS     of RequestGuildMembersSendEvent
    | REQUEST_SOUNDBOARD_SOUNDS of RequestSoundboardSoundsSendEvent
    | UPDATE_VOICE_STATE        of UpdateVoiceStateSendEvent
    | UPDATE_PRESENCE           of UpdatePresenceSendEvent

module GatewaySendEvent =
    let decoder: Decoder<GatewaySendEvent> =
        GatewaySendEventPayload.decoder
        |> Decode.andThen (fun payload ->
            match payload.Opcode, payload.Data with
            | GatewayOpcode.IDENTIFY, GatewaySendEventData.IDENTIFY d ->
                Decode.succeed (GatewaySendEvent.IDENTIFY d)

            | GatewayOpcode.RESUME, GatewaySendEventData.RESUME d ->
                Decode.succeed (GatewaySendEvent.RESUME d)

            | GatewayOpcode.HEARTBEAT, GatewaySendEventData.OPTIONAL_INT d ->
                Decode.succeed (GatewaySendEvent.HEARTBEAT d)

            | GatewayOpcode.REQUEST_GUILD_MEMBERS, GatewaySendEventData.REQUEST_GUILD_MEMBERS d ->
                Decode.succeed (GatewaySendEvent.REQUEST_GUILD_MEMBERS d)

            | GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS, GatewaySendEventData.REQUEST_SOUNDBOARD_SOUNDS d ->
                Decode.succeed (GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS d)

            | GatewayOpcode.VOICE_STATE_UPDATE, GatewaySendEventData.UPDATE_VOICE_STATE d ->
                Decode.succeed (GatewaySendEvent.UPDATE_VOICE_STATE d)

            | GatewayOpcode.PRESENCE_UPDATE, GatewaySendEventData.UPDATE_PRESENCE d ->
                Decode.succeed (GatewaySendEvent.UPDATE_PRESENCE d)

            | _ ->
                Decode.fail "Unexpected gateway send event data received"
        )
