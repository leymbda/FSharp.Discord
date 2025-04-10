namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type SubCommand = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Options: SubCommandOption list
}

module SubCommand =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Options = []
        }
        
    let addLocale locale name description (v: SubCommand) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: SubCommand) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let addOption option (v: SubCommand) =
        { v with Options = v.Options @ [option] }

    let removeOption name (v: SubCommand) =
        { v with Options = v.Options |> List.filter (fun o -> SubCommandOption.getName o <> name )}

    let toCommandOption (v: SubCommand) =
        {
            Type = ApplicationCommandOptionType.SUB_COMMAND
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
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
