namespace FSharp.Discord.Commands

type Int0to6000 = | Int0to6000 of int

module Int0to6000 =
    let tryCreate v =
        if v < 6000 && v >= 0 then Some (Int0to6000 v)
        else None

type Int1to6000 = | Int1to6000 of int

module Int1to6000 =
    let tryCreate v =
        if v < 6000 && v >= 1 then Some (Int1to6000 v)
        else None

type String1to100 = String1to100 of string

module String1to100 =
    let tryCreate v =
        let length = String.length v
        if length <= 100 && length >= 1 then Some (String1to100 v)
        else None
    
type StringCommandName = StringCommandName of string

module StringCommandName =
    let tryCreate v =
        Some (StringCommandName v) // TODO: Implement regex for valid command name
