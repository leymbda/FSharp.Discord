namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type StringSubCommandOptionChoice = {
    Name: string
    Description: string option
    Value: string
}

type StringSubCommandOptionAccept =
    | Choices of StringSubCommandOptionChoice list
    | Autocomplete
    | Any

type StringSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
    Length: (Int0to6000 * Int1to6000) option
    Accept: StringSubCommandOptionAccept
}

type IntegerSubCommandOptionChoice = {
    Name: string
    Description: string option
    Value: int
}

type IntegerSubCommandOptionAccept =
    | Choices of IntegerSubCommandOptionChoice list
    | Autocomplete
    | Any

type IntegerSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
    Range: (int * int) option
    Accept: IntegerSubCommandOptionAccept
}

type BooleanSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
}

type UserSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
}

type ChannelSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
    ChannelTypes: ChannelType list option
}

type RoleSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
}

type MentionableSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
}

type NumberSubCommandOptionChoice = {
    Name: string
    Description: string option
    Value: double
}

type NumberSubCommandOptionAccept =
    | Choices of NumberSubCommandOptionChoice list
    | Autocomplete
    | Any

type NumberSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
    Range: (double * double) option
    Accept: NumberSubCommandOptionAccept
}

type AttachmentSubCommandOption = {
    Name: string
    Description: string option
    Required: bool
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
