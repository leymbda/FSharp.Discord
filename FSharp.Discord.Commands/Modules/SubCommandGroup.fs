namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type SubCommandGroup = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Options: SubCommand list
}

module SubCommandGroup =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Options = []
        }
        
    let addLocale locale name description (v: SubCommandGroup) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: SubCommandGroup) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let addSubCommand subcommand (v: SubCommandGroup) =
        { v with Options = v.Options @ [subcommand] }

    let removeSubCommand name (v: SubCommandGroup) =
        { v with Options = v.Options |> List.filter (fun c -> c.Name <> name )}

    let toCommandOption (v: SubCommandGroup) =
        {
            Type = ApplicationCommandOptionType.SUB_COMMAND_GROUP
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
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
