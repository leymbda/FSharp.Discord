namespace FSharp.Discord.Types.Serialization

open FSharp.Discord.Types

module ApplicationIntegrationType =
    let toString (value: ApplicationIntegrationType) =
        match value with
        | ApplicationIntegrationType.GUILD_INSTALL -> Some "GUILD_INSTALL"
        | ApplicationIntegrationType.USER_INSTALL -> Some "USER_INSTALL"
        | _ -> None

    let fromString (str: string) =
        match str with
        | "GUILD_INSTALL" -> Some ApplicationIntegrationType.GUILD_INSTALL
        | "USER_INSTALL" -> Some ApplicationIntegrationType.USER_INSTALL
        | _ -> None
