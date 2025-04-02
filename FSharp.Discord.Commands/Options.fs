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

let (|String|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.STRING
            Value = Some (ApplicationCommandInteractionDataOptionValue.STRING value)
        }) -> Some value
        | _ -> None
    )

let (|StringOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | String name v -> Some (Some v)
    | _ -> Some None

let (|AutocompletingString|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.STRING
            Value = Some (ApplicationCommandInteractionDataOptionValue.STRING value)
            Focused = Some true
        }) -> Some value
        | _ -> None
    )

let (|Integer|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.INTEGER
            Value = Some (ApplicationCommandInteractionDataOptionValue.INT value)
        }) -> Some value
        | _ -> None
    )
        
let (|IntegerOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | Integer name v -> Some (Some v)
    | _ -> Some None

let (|AutocompletingInteger|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.INTEGER
            Value = Some (ApplicationCommandInteractionDataOptionValue.INT value)
            Focused = Some true
        }) -> Some value
        | _ -> None
    )

let (|Boolean|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.BOOLEAN
            Value = Some (ApplicationCommandInteractionDataOptionValue.BOOL value)
        }) -> Some value
        | _ -> None
    )
        
let (|BooleanOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | Boolean name v -> Some (Some v)
    | _ -> Some None
        
let (|UserId|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.USER
            Value = Some (ApplicationCommandInteractionDataOptionValue.STRING userId)
        }) -> Some userId
        | _ -> None
    )

let (|UserIdOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | UserId name v -> Some (Some v)
    | _ -> Some None

let (|User|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | UserId name userId -> Option.bind (Map.tryFind userId) resolved.Users
    | _ -> None
        
let (|UserOpt|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | User name resolved v -> Some (Some v)
    | _ -> Some None

let (|ChannelId|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.CHANNEL
            Value = Some (ApplicationCommandInteractionDataOptionValue.STRING channelId)
        }) -> Some channelId
        | _ -> None
    )
        
let (|ChannelIdOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | ChannelId name v -> Some (Some v)
    | _ -> Some None

let (|Channel|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | ChannelId name channelId -> Option.bind (Map.tryFind channelId) resolved.Channels
    | _ -> None

let (|ChannelOpt|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | Channel name resolved v -> Some (Some v)
    | _ -> Some None

let (|RoleId|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.ROLE
            Value = Some (ApplicationCommandInteractionDataOptionValue.STRING roleId)
        }) -> Some roleId
        | _ -> None
    )
        
let (|RoleIdOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | RoleId name v -> Some (Some v)
    | _ -> Some None

let (|Role|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | RoleId name roleId -> Option.bind (Map.tryFind roleId) resolved.Roles
    | _ -> None

let (|RoleOpt|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | Role name resolved v -> Some (Some v)
    | _ -> Some None

let (|MentionableId|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.MENTIONABLE
            Value = Some (ApplicationCommandInteractionDataOptionValue.STRING mentionableId)
        }) -> Some mentionableId
        | _ -> None
    )
        
let (|MentionableIdOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | MentionableId name v -> Some (Some v)
    | _ -> Some None

type Mentionable =
    | User of User
    | Role of Role

let (|Mentionable|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | MentionableId name mentionableId ->
        let user = Option.bind (Map.tryFind mentionableId) resolved.Users
        let role = Option.bind (Map.tryFind mentionableId) resolved.Roles

        match user, role with
        | Some user, _ -> Some (Mentionable.User user)
        | _, Some role -> Some (Mentionable.Role role)
        | None, None -> None
    | _ -> None

let (|MentionableOpt|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | Mentionable name resolved v -> Some (Some v)
    | _ -> Some None

let (|Number|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.NUMBER
            Value = Some (ApplicationCommandInteractionDataOptionValue.DOUBLE value)
        }) -> Some value
        | _ -> None
    )
        
let (|NumberOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | Number name v -> Some (Some v)
    | _ -> Some None

let (|AutocompletingNumber|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.NUMBER
            Value = Some (ApplicationCommandInteractionDataOptionValue.DOUBLE value)
            Focused = Some true
        }) -> Some value
        | _ -> None
    )

let (|AttachmentId|_|) name (options: ApplicationCommandInteractionDataOption list) =
    options
    |> List.tryFind (_.Name >> (=) name)
    |> Option.bind (function
        | ({
            Type = ApplicationCommandOptionType.ATTACHMENT
            Value = Some (ApplicationCommandInteractionDataOptionValue.STRING attachmentId)
        }) -> Some attachmentId
        | _ -> None
    )
        
let (|AttachmentIdOpt|_|) name (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | AttachmentId name v -> Some (Some v)
    | _ -> Some None

let (|Attachment|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | AttachmentId name attachmentId -> Option.bind (Map.tryFind attachmentId) resolved.Attachments
    | _ -> None

let (|AttachmentOpt|_|) name (resolved: ResolvedData) (options: ApplicationCommandInteractionDataOption list) =
    match options with
    | Attachment name resolved v -> Some (Some v)
    | _ -> Some None
