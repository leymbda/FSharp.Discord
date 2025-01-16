namespace FSharp.Discord.Utils

open System

type Snowflake = {
    Increment: int
    WorkerId: int
    ProcessId: int
    Timestamp: DateTime
}

module Snowflake =
    let [<Literal>] INCREMENT_MASK  = 0b00000000_00000000_00000000_00000000_00000000_00000000_00001111_11111111L
    let [<Literal>] WORKER_ID_MASK  = 0b00000000_00000000_00000000_00000000_00000000_00000001_11110000_00000000L
    let [<Literal>] PROCESS_ID_MASK = 0b00000000_00000000_00000000_00000000_00000000_00111110_00000000_00000000L
    let [<Literal>] TIMESTAMP_MASK  = 0b11111111_11111111_11111111_11111111_11111111_11000000_00000000_00000000L

    /// Parse a snowflake to read its inner data.
    let tryParse (snowflake: string) =
        match Int64.TryParse snowflake with
        | false, _ -> None
        | true, long -> Some {
            Increment = int (long &&& INCREMENT_MASK >>> 0)
            WorkerId = int (long &&& WORKER_ID_MASK >>> 12)
            ProcessId = int (long &&& PROCESS_ID_MASK >>> 17)
            Timestamp = DateTime.UnixEpoch.AddMilliseconds (float (DISCORD_EPOCH + (long &&& TIMESTAMP_MASK >>> 22)))
        }

    /// Create a snowflake based on the given tiemstamp for use in pagination requests.
    let tryCreate (timestamp: DateTime) =
        match DateTimeOffset timestamp |> _.ToUnixTimeMilliseconds() with
        | unix when unix < DISCORD_EPOCH -> None
        | unix -> Some ((unix - DISCORD_EPOCH) <<< 22)
