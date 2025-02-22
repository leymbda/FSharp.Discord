namespace FSharp.Discord.Commands

open FSharp.Discord.Types

module Options =
    let (|SubCommand|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Options = Some options;
                Type = ApplicationCommandOptionType.SUB_COMMAND;
            }) when option.Name = name -> Some options
            | _ -> None
        )

    let (|SubCommandGroup|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Options = Some options;
                Type = ApplicationCommandOptionType.SUB_COMMAND_GROUP;
            }) when option.Name = name -> Some options
            | _ -> None
        )

    let (|String|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.STRING value);
                Type = ApplicationCommandOptionType.STRING;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|Integer|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.INT value);
                Type = ApplicationCommandOptionType.INTEGER;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|Boolean|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.BOOL value);
                Type = ApplicationCommandOptionType.BOOLEAN;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|User|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.STRING userId);
                Type = ApplicationCommandOptionType.USER;
            }) when option.Name = name -> Some userId
            | _ -> None
        )

    let (|Channel|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.STRING channelId);
                Type = ApplicationCommandOptionType.CHANNEL;
            }) when option.Name = name -> Some channelId
            | _ -> None
        )

    let (|Role|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.STRING roleId);
                Type = ApplicationCommandOptionType.ROLE;
            }) when option.Name = name -> Some roleId
            | _ -> None
        )

    let (|Mentionable|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.STRING mentionableId);
                Type = ApplicationCommandOptionType.MENTIONABLE;
            }) when option.Name = name -> Some mentionableId
            | _ -> None
        )

    let (|Number|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.DOUBLE value);
                Type = ApplicationCommandOptionType.NUMBER;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|Attachment|_|) (name: string) (options: ApplicationCommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (ApplicationCommandInteractionDataOptionValue.STRING attachmentId);
                Type = ApplicationCommandOptionType.ATTACHMENT;
            }) when option.Name = name -> Some attachmentId
            | _ -> None
        )
