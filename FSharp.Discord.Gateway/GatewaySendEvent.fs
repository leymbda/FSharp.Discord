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
            | GatewayOpcode.IDENTIFY, GatewaySendEventData.Identify d ->
                Decode.succeed (GatewaySendEvent.IDENTIFY d)

            | GatewayOpcode.RESUME, GatewaySendEventData.Resume d ->
                Decode.succeed (GatewaySendEvent.RESUME d)

            | GatewayOpcode.HEARTBEAT, GatewaySendEventData.OptionalInt d ->
                Decode.succeed (GatewaySendEvent.HEARTBEAT d)

            | GatewayOpcode.REQUEST_GUILD_MEMBERS, GatewaySendEventData.RequestGuildMembers d ->
                Decode.succeed (GatewaySendEvent.REQUEST_GUILD_MEMBERS d)

            | GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS, GatewaySendEventData.RequestSoundboardSounds d ->
                Decode.succeed (GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS d)

            | GatewayOpcode.VOICE_STATE_UPDATE, GatewaySendEventData.UpdateVoiceState d ->
                Decode.succeed (GatewaySendEvent.UPDATE_VOICE_STATE d)

            | GatewayOpcode.PRESENCE_UPDATE, GatewaySendEventData.UpdatePresence d ->
                Decode.succeed (GatewaySendEvent.UPDATE_PRESENCE d)

            | _ ->
                Decode.fail "Unexpected gateway send event data received"
        )

    let encoder (v: GatewaySendEvent) =
        match v with
        | GatewaySendEvent.IDENTIFY ev ->
            GatewaySendEventPayload.encoder {
                Opcode = GatewayOpcode.IDENTIFY
                EventName = None
                Sequence = None
                Data = GatewaySendEventData.Identify ev
            }

        | GatewaySendEvent.RESUME ev ->
            GatewaySendEventPayload.encoder {
                Opcode = GatewayOpcode.RESUME
                EventName = None
                Sequence = None
                Data = GatewaySendEventData.Resume ev
            }

        | GatewaySendEvent.HEARTBEAT ev ->
            GatewaySendEventPayload.encoder {
                Opcode = GatewayOpcode.HEARTBEAT
                EventName = None
                Sequence = None
                Data = GatewaySendEventData.OptionalInt ev
            }

        | GatewaySendEvent.REQUEST_GUILD_MEMBERS ev ->
            GatewaySendEventPayload.encoder {
                Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS
                EventName = None
                Sequence = None
                Data = GatewaySendEventData.RequestGuildMembers ev
            }

        | GatewaySendEvent.REQUEST_SOUNDBOARD_SOUNDS ev ->
            GatewaySendEventPayload.encoder {
                Opcode = GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS
                EventName = None
                Sequence = None
                Data = GatewaySendEventData.RequestSoundboardSounds ev
            }

        | GatewaySendEvent.UPDATE_VOICE_STATE ev ->
            GatewaySendEventPayload.encoder {
                Opcode = GatewayOpcode.VOICE_STATE_UPDATE
                EventName = None
                Sequence = None
                Data = GatewaySendEventData.UpdateVoiceState ev
            }

        | GatewaySendEvent.UPDATE_PRESENCE ev ->
            GatewaySendEventPayload.encoder {
                Opcode = GatewayOpcode.PRESENCE_UPDATE
                EventName = None
                Sequence = None
                Data = GatewaySendEventData.UpdatePresence ev
            }
            
// TODO: Make all values pascal case
