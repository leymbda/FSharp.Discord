namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type StringSubCommandOptionChoice = {
    Name: string
    Value: string
}

module StringSubCommandOptionChoice =
    let toChoice (v: StringSubCommandOptionChoice) =
        {
            Name = v.Name
            NameLocalizations = None
            Value = ApplicationCommandOptionChoiceValue.STRING v.Value
        }

type StringSubCommandOptionAccept =
    | Choices of StringSubCommandOptionChoice list
    | Autocomplete
    | Any

module StringSubCommandOptionAccept =
    let toValues (v: StringSubCommandOptionAccept) =
        match v with
        | StringSubCommandOptionAccept.Choices c -> Some (List.map StringSubCommandOptionChoice.toChoice c), false
        | StringSubCommandOptionAccept.Autocomplete -> None, true
        | StringSubCommandOptionAccept.Any -> None, false

type StringSubCommandOption = {
    Name: string
    Description: string
    Required: bool
    Length: (int * int) option
    Accept: StringSubCommandOptionAccept
}

module StringSubCommandOption =
    let toCommandOption (v: StringSubCommandOption) =
        let choices, autocomplete = StringSubCommandOptionAccept.toValues v.Accept

        {
            Type = ApplicationCommandOptionType.STRING
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Value: int
}

module IntegerSubCommandOptionChoice =
    let toChoice (v: IntegerSubCommandOptionChoice) =
        {
            Name = v.Name
            NameLocalizations = None
            Value = ApplicationCommandOptionChoiceValue.INT v.Value
        }

type IntegerSubCommandOptionAccept =
    | Choices of IntegerSubCommandOptionChoice list
    | Autocomplete
    | Any

module IntegerSubCommandOptionAccept =
    let toValues (v: IntegerSubCommandOptionAccept) =
        match v with
        | IntegerSubCommandOptionAccept.Choices c -> Some (List.map IntegerSubCommandOptionChoice.toChoice c), false
        | IntegerSubCommandOptionAccept.Autocomplete -> None, true
        | IntegerSubCommandOptionAccept.Any -> None, false

type IntegerSubCommandOption = {
    Name: string
    Description: string
    Required: bool
    Range: (int * int) option
    Accept: IntegerSubCommandOptionAccept
}

module IntegerSubCommandOption =
    let toCommandOption (v: IntegerSubCommandOption) =
        let choices, autocomplete = IntegerSubCommandOptionAccept.toValues v.Accept

        {
            Type = ApplicationCommandOptionType.INTEGER
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Required: bool
}

module BooleanSubCommandOption =
    let toCommandOption (v: BooleanSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.BOOLEAN
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Required: bool
}

module UserSubCommandOption =
    let toCommandOption (v: UserSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.USER
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Required: bool
    ChannelTypes: ChannelType list option
}

module ChannelSubCommandOption =
    let toCommandOption (v: ChannelSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.CHANNEL
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Required: bool
}

module RoleSubCommandOption =
    let toCommandOption (v: RoleSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.ROLE
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Required: bool
}

module MentionableSubCommandOption =
    let toCommandOption (v: MentionableSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.MENTIONABLE
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Value: double
}

module NumberSubCommandOptionChoice =
    let toChoice (v: NumberSubCommandOptionChoice) =
        {
            Name = v.Name
            NameLocalizations = None
            Value = ApplicationCommandOptionChoiceValue.DOUBLE v.Value
        }

type NumberSubCommandOptionAccept =
    | Choices of NumberSubCommandOptionChoice list
    | Autocomplete
    | Any

module NumberSubCommandOptionAccept =
    let toValues (v: NumberSubCommandOptionAccept) =
        match v with
        | NumberSubCommandOptionAccept.Choices c -> Some (List.map NumberSubCommandOptionChoice.toChoice c), false
        | NumberSubCommandOptionAccept.Autocomplete -> None, true
        | NumberSubCommandOptionAccept.Any -> None, false

type NumberSubCommandOption = {
    Name: string
    Description: string
    Required: bool
    Range: (double * double) option
    Accept: NumberSubCommandOptionAccept
}

module NumberSubCommandOption =
    let toCommandOption (v: NumberSubCommandOption) =
        let choices, autocomplete = NumberSubCommandOptionAccept.toValues v.Accept

        {
            Type = ApplicationCommandOptionType.NUMBER
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
    Required: bool
}

module AttachmentSubCommandOption =
    let toCommandOption (v: AttachmentSubCommandOption) =
        {
            Type = ApplicationCommandOptionType.ATTACHMENT
            Name = v.Name
            NameLocalizations = None
            LocalizedName = None
            Description = v.Description
            DescriptionLocalizations = None
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
