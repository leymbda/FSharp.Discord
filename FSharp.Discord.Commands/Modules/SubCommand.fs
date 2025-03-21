namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type SubCommand = {
    Name: string
    Description: string
    Options: SubCommandOption list
}

module SubCommand =
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
            Options = Some <| List.map SubCommandOption.toCommandOption v.Options
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }
