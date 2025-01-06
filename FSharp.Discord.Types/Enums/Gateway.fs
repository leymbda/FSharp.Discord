namespace FSharp.Discord.Types

open System.Text.Json
open System.Text.Json.Serialization

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-types
type ActivityType =
    | PLAYING   = 0
    | STREAMING = 1
    | LISTENING = 2
    | WATCHING  = 3
    | CUSTOM    = 4
    | COMPETING = 5

// https://discord.com/developers/docs/events/gateway#transport-compression
[<JsonConverter(typeof<GatewayCompressionConverter>)>]
type GatewayCompression =
    | ZLIBSTREAM
    | ZSTDSTREAM
with
    override this.ToString () =
        match this with
        | GatewayCompression.ZLIBSTREAM -> "zlib-stream"
        | GatewayCompression.ZSTDSTREAM -> "zstd-stream"

    static member FromString (str: string) =
        match str with
        | "zlib-stream" -> Some GatewayCompression.ZLIBSTREAM
        | "zstd-stream" -> Some GatewayCompression.ZSTDSTREAM
        | _ -> None

and GatewayCompressionConverter () =
    inherit JsonConverter<GatewayCompression> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> GatewayCompression.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected GatewayCompression type")

        override _.Write (writer, value, _) = 
            value.ToString()
            |> writer.WriteStringValue

// https://discord.com/developers/docs/events/gateway#encoding-and-compression
[<JsonConverter(typeof<GatewayEncodingConverter>)>]
type GatewayEncoding =
    | JSON
    | ETF
with
    override this.ToString () =
        match this with
        | GatewayEncoding.JSON -> "json"
        | GatewayEncoding.ETF -> "etf"

    static member FromString (str: string) =
        match str with
        | "json" -> Some GatewayEncoding.JSON
        | "etf" -> Some GatewayEncoding.ETF
        | _ -> None

and GatewayEncodingConverter () =
    inherit JsonConverter<GatewayEncoding> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> GatewayEncoding.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected GatewayEncoding type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue

// https://discord.com/developers/docs/events/gateway#list-of-intents
type GatewayIntent =
    | GUILDS =                        (1 <<< 0)
    | GUILD_MEMBERS =                 (1 <<< 1)
    | GUILD_MODERATION =              (1 <<< 2)
    | GUILD_EMOJIS_AND_STICKERS =     (1 <<< 3)
    | GUILD_INTEGRATIONS =            (1 <<< 4)
    | GUILD_WEBHOOKS =                (1 <<< 5)
    | GUILD_INVITES =                 (1 <<< 6)
    | GUILD_VOICE_STATES =            (1 <<< 7)
    | GUILD_PRESENCES =               (1 <<< 8)
    | GUILD_MESSAGES =                (1 <<< 9)
    | GUILD_MESSAGE_REACTIONS =       (1 <<< 10)
    | GUILD_MESSAGE_TYPING =          (1 <<< 11)
    | DIRECT_MESSAGES =               (1 <<< 12)
    | DIRECT_MESSAGE_REACTIONS =      (1 <<< 13)
    | DIRECT_MESSAGE_TYPING =         (1 <<< 14)
    | MESSAGE_CONTENT =               (1 <<< 15)
    | GUILD_SCHEDULED_EVENTS =        (1 <<< 16)
    | AUTO_MODERATION_CONFIGURATION = (1 <<< 20)
    | AUTO_MODERATION_EXECUTION =     (1 <<< 21)
    | GUILD_MESSAGE_POLLS =           (1 <<< 24)
    | DIRECT_MESSAGE_POLLS =          (1 <<< 25)

module GatewayIntent =
    let ALL =
        int <| (
                GatewayIntent.GUILDS
            ||| GatewayIntent.GUILD_MEMBERS
            ||| GatewayIntent.GUILD_MODERATION
            ||| GatewayIntent.GUILD_EMOJIS_AND_STICKERS
            ||| GatewayIntent.GUILD_INTEGRATIONS
            ||| GatewayIntent.GUILD_WEBHOOKS
            ||| GatewayIntent.GUILD_INVITES
            ||| GatewayIntent.GUILD_VOICE_STATES
            ||| GatewayIntent.GUILD_PRESENCES
            ||| GatewayIntent.GUILD_MESSAGES
            ||| GatewayIntent.GUILD_MESSAGE_REACTIONS
            ||| GatewayIntent.GUILD_MESSAGE_TYPING
            ||| GatewayIntent.DIRECT_MESSAGES
            ||| GatewayIntent.DIRECT_MESSAGE_REACTIONS
            ||| GatewayIntent.DIRECT_MESSAGE_TYPING
            ||| GatewayIntent.MESSAGE_CONTENT
            ||| GatewayIntent.GUILD_SCHEDULED_EVENTS
            ||| GatewayIntent.AUTO_MODERATION_CONFIGURATION
            ||| GatewayIntent.AUTO_MODERATION_EXECUTION
            ||| GatewayIntent.GUILD_MESSAGE_POLLS
            ||| GatewayIntent.DIRECT_MESSAGE_POLLS
        )

// https://discord.com/developers/docs/topics/opcodes-and-status-codes#gateway-gateway-opcodes
type GatewayOpcode =
    | DISPATCH                  = 0
    | HEARTBEAT                 = 1
    | IDENTIFY                  = 2
    | PRESENCE_UPDATE           = 3
    | VOICE_STATE_UPDATE        = 4
    | RESUME                    = 6
    | RECONNECT                 = 7
    | REQUEST_GUILD_MEMBERS     = 8
    | INVALID_SESSION           = 9
    | HELLO                     = 10
    | HEARTBEAT_ACK             = 11
    | REQUEST_SOUNDBOARD_SOUNDS = 31

// https://discord.com/developers/docs/events/gateway-events#update-presence-status-types
[<JsonConverter(typeof<StatusTypeConverter>)>]
type StatusType =
    | ONLINE
    | DND
    | IDLE
    | INVISIBLE
    | OFFLINE
with
    override this.ToString () =
        match this with
        | StatusType.ONLINE -> "online"
        | StatusType.DND -> "dnd"
        | StatusType.IDLE -> "idle"
        | StatusType.INVISIBLE -> "invisible"
        | StatusType.OFFLINE -> "offline"

    static member FromString (str: string) =
        match str with
        | "online" -> Some StatusType.ONLINE
        | "dnd" -> Some StatusType.DND
        | "idle" -> Some StatusType.IDLE
        | "invisible" -> Some StatusType.INVISIBLE
        | "offline" -> Some StatusType.OFFLINE
        | _ -> None

and StatusTypeConverter () =
    inherit JsonConverter<StatusType> () with
        override _.Read (reader, _, _) =
            reader.GetString()
            |> StatusType.FromString
            |> Option.defaultWith (JsonException.raiseThunk "Unexpected StatusType type")

        override _.Write (writer, value, _) = 
            value.ToString() |> writer.WriteStringValue
