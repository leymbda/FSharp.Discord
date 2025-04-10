namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type StringSubCommandOptionChoice = {
    Name: string
    Localizations: Map<string, string>
    Value: string
}

module StringSubCommandOptionChoice =
    let create name value =
        {
            Name = name
            Localizations = Map.empty
            Value = value
        }
        
    let addLocale locale name (v: StringSubCommandOptionChoice) =
        { v with Localizations = v.Localizations |> Map.add locale name }

    let removeLocale locale (v: StringSubCommandOptionChoice) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let toChoice (v: StringSubCommandOptionChoice) =
        {
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.toOption |> Some
            Value = ApplicationCommandOptionChoiceValue.STRING v.Value
        }

type StringSubCommandOptionInput =
    | Choices of StringSubCommandOptionChoice list
    | Autocomplete
    | Any

module StringSubCommandOptionInput =
    let toValues (v: StringSubCommandOptionInput) =
        match v with
        | StringSubCommandOptionInput.Choices c -> Some (List.map StringSubCommandOptionChoice.toChoice c), false
        | StringSubCommandOptionInput.Autocomplete -> None, true
        | StringSubCommandOptionInput.Any -> None, false

type StringSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
    Length: (int * int) option
    Input: StringSubCommandOptionInput
}

module StringSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
            Length = None
            Input = StringSubCommandOptionInput.Any
        }
        
    let addLocale locale name description (v: StringSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: StringSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: StringSubCommandOption) =
        { v with Required = true }

    let setOptional (v: StringSubCommandOption) =
        { v with Required = false }

    let setLength min max (v: StringSubCommandOption) =
        { v with Length = Some (min, max) }

    let removeLengthRequirements (v: StringSubCommandOption) =
        { v with Length = None }

    let setChoices choices (v: StringSubCommandOption) =
        { v with Input = StringSubCommandOptionInput.Choices choices }

    let setAutocomplete (v: StringSubCommandOption) =
        { v with Input = StringSubCommandOptionInput.Autocomplete }

    let removeInputRequirements (v: StringSubCommandOption) =
        { v with Input = StringSubCommandOptionInput.Any }

    let toCommandOption (v: StringSubCommandOption) =
        let choices, autocomplete = StringSubCommandOptionInput.toValues v.Input

        {
            Type = ApplicationCommandOptionType.STRING
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = Option.map fst v.Length
            MaxLength = Option.map snd v.Length
            Choices = choices
            Autocomplete = Some autocomplete
        }

type IntegerSubCommandOptionChoice = {
    Name: string
    Localizations: Map<string, string>
    Value: int
}

module IntegerSubCommandOptionChoice =
    let create name value =
        {
            Name = name
            Localizations = Map.empty
            Value = value
        }
        
    let addLocale locale name (v: IntegerSubCommandOptionChoice) =
        { v with Localizations = v.Localizations |> Map.add locale name }

    let removeLocale locale (v: IntegerSubCommandOptionChoice) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let toChoice (v: IntegerSubCommandOptionChoice) =
        {
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.toOption |> Some
            Value = ApplicationCommandOptionChoiceValue.INT v.Value
        }

type IntegerSubCommandOptionInput =
    | Choices of IntegerSubCommandOptionChoice list
    | Autocomplete
    | Any

module IntegerSubCommandOptionInput =
    let toValues (v: IntegerSubCommandOptionInput) =
        match v with
        | IntegerSubCommandOptionInput.Choices c -> Some (List.map IntegerSubCommandOptionChoice.toChoice c), false
        | IntegerSubCommandOptionInput.Autocomplete -> None, true
        | IntegerSubCommandOptionInput.Any -> None, false

type IntegerSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
    Range: (int * int) option
    Input: IntegerSubCommandOptionInput
}

module IntegerSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
            Range = None
            Input = IntegerSubCommandOptionInput.Any
        }
        
    let addLocale locale name description (v: IntegerSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: IntegerSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: IntegerSubCommandOption) =
        { v with Required = true }

    let setOptional (v: IntegerSubCommandOption) =
        { v with Required = false }

    let setRange min max (v: IntegerSubCommandOption) =
        { v with Range = Some (min, max) }

    let removeRangeRequirements (v: IntegerSubCommandOption) =
        { v with Range = None }

    let setChoices choices (v: IntegerSubCommandOption) =
        { v with Input = IntegerSubCommandOptionInput.Choices choices }

    let setAutocomplete (v: IntegerSubCommandOption) =
        { v with Input = IntegerSubCommandOptionInput.Autocomplete }

    let removeInputRequirements (v: IntegerSubCommandOption) =
        { v with Input = IntegerSubCommandOptionInput.Any }

    let toCommandOption (v: IntegerSubCommandOption) =
        let choices, autocomplete = IntegerSubCommandOptionInput.toValues v.Input

        {
            Type = ApplicationCommandOptionType.INTEGER
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = Option.map (fst >> ApplicationCommandOptionMinValue.INT) v.Range
            MaxValue = Option.map (snd >> ApplicationCommandOptionMaxValue.INT) v.Range
            MinLength = None
            MaxLength = None
            Choices = choices
            Autocomplete = Some autocomplete
        }

type BooleanSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
}

module BooleanSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
        }
        
    let addLocale locale name description (v: BooleanSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: BooleanSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: BooleanSubCommandOption) =
        { v with Required = true }

    let setOptional (v: BooleanSubCommandOption) =
        { v with Required = false }

    let toCommandOption (v: BooleanSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.BOOLEAN
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }

type UserSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
}

module UserSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
        }
        
    let addLocale locale name description (v: UserSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: UserSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: UserSubCommandOption) =
        { v with Required = true }

    let setOptional (v: UserSubCommandOption) =
        { v with Required = false }

    let toCommandOption (v: UserSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.USER
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }

type ChannelSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
    ChannelTypes: ChannelType list option
}

module ChannelSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
            ChannelTypes = None
        }
        
    let addLocale locale name description (v: ChannelSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: ChannelSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: ChannelSubCommandOption) =
        { v with Required = true }

    let setOptional (v: ChannelSubCommandOption) =
        { v with Required = false }

    let setChannelTypes types (v: ChannelSubCommandOption) =
        { v with ChannelTypes = Some types }

    let removeChannelTypesRequirement (v: ChannelSubCommandOption) =
        { v with ChannelTypes = None }

    let toCommandOption (v: ChannelSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.CHANNEL
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = v.ChannelTypes
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }

type RoleSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
}

module RoleSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
        }
        
    let addLocale locale name description (v: RoleSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: RoleSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: RoleSubCommandOption) =
        { v with Required = true }

    let setOptional (v: RoleSubCommandOption) =
        { v with Required = false }

    let toCommandOption (v: RoleSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.ROLE
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }

type MentionableSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
}

module MentionableSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
        }
        
    let addLocale locale name description (v: MentionableSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: MentionableSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: MentionableSubCommandOption) =
        { v with Required = true }

    let setOptional (v: MentionableSubCommandOption) =
        { v with Required = false }

    let toCommandOption (v: MentionableSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.MENTIONABLE
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }

type NumberSubCommandOptionChoice = {
    Name: string
    Localizations: Map<string, string>
    Value: double
}

module NumberSubCommandOptionChoice =
    let create name value =
        {
            Name = name
            Localizations = Map.empty
            Value = value
        }
        
    let addLocale locale name (v: NumberSubCommandOptionChoice) =
        { v with Localizations = v.Localizations |> Map.add locale name }

    let removeLocale locale (v: NumberSubCommandOptionChoice) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let toChoice (v: NumberSubCommandOptionChoice) =
        {
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.toOption |> Some
            Value = ApplicationCommandOptionChoiceValue.DOUBLE v.Value
        }

type NumberSubCommandOptionInput =
    | Choices of NumberSubCommandOptionChoice list
    | Autocomplete
    | Any

module NumberSubCommandOptionInput =
    let toValues (v: NumberSubCommandOptionInput) =
        match v with
        | NumberSubCommandOptionInput.Choices c -> Some (List.map NumberSubCommandOptionChoice.toChoice c), false
        | NumberSubCommandOptionInput.Autocomplete -> None, true
        | NumberSubCommandOptionInput.Any -> None, false

type NumberSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
    Range: (double * double) option
    Input: NumberSubCommandOptionInput
}

module NumberSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
            Range = None
            Input = NumberSubCommandOptionInput.Any
        }
        
    let addLocale locale name description (v: NumberSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: NumberSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: NumberSubCommandOption) =
        { v with Required = true }

    let setOptional (v: NumberSubCommandOption) =
        { v with Required = false }

    let setRange min max (v: NumberSubCommandOption) =
        { v with Range = Some (min, max) }

    let removeRangeRequirements (v: NumberSubCommandOption) =
        { v with Range = None }

    let setChoices choices (v: NumberSubCommandOption) =
        { v with Input = NumberSubCommandOptionInput.Choices choices }

    let setAutocomplete (v: NumberSubCommandOption) =
        { v with Input = NumberSubCommandOptionInput.Autocomplete }

    let removeInputRequirements (v: NumberSubCommandOption) =
        { v with Input = NumberSubCommandOptionInput.Any }

    let toCommandOption (v: NumberSubCommandOption) =
        let choices, autocomplete = NumberSubCommandOptionInput.toValues v.Input

        {
            Type = ApplicationCommandOptionType.NUMBER
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = Option.map (fst >> ApplicationCommandOptionMinValue.DOUBLE) v.Range
            MaxValue = Option.map (snd >> ApplicationCommandOptionMaxValue.DOUBLE) v.Range
            MinLength = None
            MaxLength = None
            Choices = choices
            Autocomplete = Some autocomplete
        }

type AttachmentSubCommandOption = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    Required: bool
}

module AttachmentSubCommandOption =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Required = false
        }
        
    let addLocale locale name description (v: AttachmentSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: AttachmentSubCommandOption) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setRequired (v: AttachmentSubCommandOption) =
        { v with Required = true }

    let setOptional (v: AttachmentSubCommandOption) =
        { v with Required = false }

    let toCommandOption (v: AttachmentSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.ATTACHMENT
            Name = v.Name
            NameLocalizations = v.Localizations |> Map.map (fun _ v -> fst v) |> Map.toOption |> Some
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = v.Localizations |> Map.map (fun _ v -> snd v) |> Map.toOption |> Some
            LocalizedDescription = None
            Required = Some v.Required
            Options = None
            ChannelTypes = None
            MinValue = None
            MaxValue = None
            MinLength = None
            MaxLength = None
            Choices = None
            Autocomplete = None
        }

type SubCommandOption =
    | String of StringSubCommandOption
    | Integer of IntegerSubCommandOption
    | Boolean of BooleanSubCommandOption
    | User of UserSubCommandOption
    | Channel of ChannelSubCommandOption
    | Role of RoleSubCommandOption
    | Mentionable of MentionableSubCommandOption
    | Number of NumberSubCommandOption
    | Attachment of AttachmentSubCommandOption

module SubCommandOption =
    let internal getName (v: SubCommandOption) =
        match v with
        | SubCommandOption.String o -> o.Name
        | SubCommandOption.Integer o -> o.Name
        | SubCommandOption.Boolean o -> o.Name
        | SubCommandOption.User o -> o.Name
        | SubCommandOption.Channel o -> o.Name
        | SubCommandOption.Role o -> o.Name
        | SubCommandOption.Mentionable o -> o.Name
        | SubCommandOption.Number o -> o.Name
        | SubCommandOption.Attachment o -> o.Name

    let toCommandOption (v: SubCommandOption) =
        match v with
        | SubCommandOption.String o -> StringSubCommandOption.toCommandOption o
        | SubCommandOption.Integer o -> IntegerSubCommandOption.toCommandOption o
        | SubCommandOption.Boolean o -> BooleanSubCommandOption.toCommandOption o
        | SubCommandOption.User o -> UserSubCommandOption.toCommandOption o
        | SubCommandOption.Channel o -> ChannelSubCommandOption.toCommandOption o
        | SubCommandOption.Role o -> RoleSubCommandOption.toCommandOption o
        | SubCommandOption.Mentionable o -> MentionableSubCommandOption.toCommandOption o
        | SubCommandOption.Number o -> NumberSubCommandOption.toCommandOption o
        | SubCommandOption.Attachment o -> AttachmentSubCommandOption.toCommandOption o
