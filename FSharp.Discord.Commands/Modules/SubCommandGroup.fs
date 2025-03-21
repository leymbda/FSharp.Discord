namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type SubCommandGroup = {
    Name: string
    Description: string
    Options: SubCommand list
}

module SubCommandGroup =
    let toCommandOption (v: SubCommandGroup) =
        {
            Type = ApplicationCommandOptionType.SUB_COMMAND
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
