[<AutoOpen>]
module FSharp.Discord.Commands.Options

open FSharp.Discord.Types

let (|SubCommand|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.SUB_COMMAND
            Options = Some opts
        }) -> Some opts
        | _ -> None
    )

let (|SubCommandGroup|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.SUB_COMMAND_GROUP
            Options = Some opts
        }) -> Some opts
        | _ -> None
    )

module String =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.STRING
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
            }) -> Some value
            | _ -> None
        )

    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None

    let (|Autocomplete|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.STRING
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
                Focused = Some true
            }) -> Some value
            | _ -> None
        )

module Integer =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.INTEGER
                Value = Some (ApplicationCommandInteractionDataOptionValue.Int value)
            }) -> Some value
            | _ -> None
        )
        
    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None

    let (|Autocomplete|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.INTEGER
                Value = Some (ApplicationCommandInteractionDataOptionValue.Int value)
                Focused = Some true
            }) -> Some value
            | _ -> None
        )

module Boolean =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.BOOLEAN
                Value = Some (ApplicationCommandInteractionDataOptionValue.Bool value)
            }) -> Some value
            | _ -> None
        )
        
    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None
        
module UserId =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.USER
                Value = Some (ApplicationCommandInteractionDataOptionValue.String userId)
            }) -> Some userId
            | _ -> None
        )

    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None

module User =
    let (|Required|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | UserId.Required name userId -> Option.bind (Map.tryFind userId) resolved.Users
        | _ -> None
        
    let (|Optional|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name resolved v -> Some (Some v)
        | _ -> Some None

module ChannelId =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.CHANNEL
                Value = Some (ApplicationCommandInteractionDataOptionValue.String channelId)
            }) -> Some channelId
            | _ -> None
        )
        
    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None

module Channel =
    let (|Required|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | ChannelId.Required name channelId -> Option.bind (Map.tryFind channelId) resolved.Channels
        | _ -> None

    let (|Optional|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name resolved v -> Some (Some v)
        | _ -> Some None

module RoleId =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String roleId)
            }) -> Some roleId
            | _ -> None
        )
        
    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None

module Role =
    let (|Required|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | RoleId.Required name roleId -> Option.bind (Map.tryFind roleId) resolved.Roles
        | _ -> None

    let (|Optional|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name resolved v -> Some (Some v)
        | _ -> Some None

module MentionableId =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.MENTIONABLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String mentionableId)
            }) -> Some mentionableId
            | _ -> None
        )
        
    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None
        
[<RequireQualifiedAccess>]
type Mentionable =
    | User of User
    | Role of Role

module Mentionable =
    let (|Required|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | MentionableId.Required name mentionableId ->
            let user = Option.bind (Map.tryFind mentionableId) resolved.Users
            let role = Option.bind (Map.tryFind mentionableId) resolved.Roles

            match user, role with
            | Some user, _ -> Some (Mentionable.User user)
            | _, Some role -> Some (Mentionable.Role role)
            | None, None -> None
        | _ -> None

    let (|Optional|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name resolved v -> Some (Some v)
        | _ -> Some None

module Number =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.NUMBER
                Value = Some (ApplicationCommandInteractionDataOptionValue.Double value)
            }) -> Some value
            | _ -> None
        )
        
    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None

    let (|Autocomplete|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.NUMBER
                Value = Some (ApplicationCommandInteractionDataOptionValue.Double value)
                Focused = Some true
            }) -> Some value
            | _ -> None
        )

module AttachmentId =
    let (|Required|_|) name (options: ApplicationCommandInteractionDataOption list) =
        options
        |> List.tryFind (_.Name >> (=) name)
        |> Option.bind (function
            | ({
                Type = ApplicationCommandOptionType.ATTACHMENT
                Value = Some (ApplicationCommandInteractionDataOptionValue.String attachmentId)
            }) -> Some attachmentId
            | _ -> None
        )
        
    let (|Optional|_|) name (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name v -> Some (Some v)
        | _ -> Some None

module Attachment =
    let (|Required|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | AttachmentId.Required name attachmentId -> Option.bind (Map.tryFind attachmentId) resolved.Attachments
        | _ -> None

    let (|Optional|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
        match options with
        | Required name resolved v -> Some (Some v)
        | _ -> Some None
