namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type SubCommandGroup = {
    Name: string
    Description: string
    Options: SubCommand list
}

module SubCommandGroup =
    let create name description =
        {
            Name = name
            Description = description
            Options = []
        }

    let addSubCommand subcommand (v: SubCommandGroup) =
        { v with Options = v.Options @ [subcommand] }

    let removeSubCommand name (v: SubCommandGroup) =
        { v with Options = v.Options |> List.filter (fun c -> c.Name <> name )}

    let toCommandOption (v: SubCommandGroup) =
        {
            Type = ApplicationCommandOptionType.SUB_COMMAND_GROUP
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
            LocalizedDescription = None
            Required = None
            Options = Some <| List.map SubCommand.toCommandOption v.Options
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }
