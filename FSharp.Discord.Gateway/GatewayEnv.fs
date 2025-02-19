namespace FSharp.Discord.Gateway

open System

type IGetCurrentTime =
    abstract GetCurrentTime: unit -> DateTime

type GetCurrentTime () =
    member _.GetCurrentTime () =
        DateTime.UtcNow
