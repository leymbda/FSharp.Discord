namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type SubCommand = {
    Name: string
    Description: string
    Options: SubCommandOption list
}

module SubCommand =
    let create name description =
        {
            Name = name
            Description = description
            Options = []
        }

    let addOption option (v: SubCommand) =
        { v with Options = v.Options @ [option] }

    let removeOption name (v: SubCommand) =
        { v with Options = v.Options |> List.filter (fun o -> SubCommandOption.getName o <> name )}

    let toCommandOption (v: SubCommand) =
        {
            Type = ApplicationCommandOptionType.SUB_COMMAND
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
            LocalizedDescription = None
            Required = None
            Options =
                List.map SubCommandOption.toCommandOption v.Options
                |> List.sortBy (_.Required >> Option.defaultValue false >> not)
                |> Some
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }
