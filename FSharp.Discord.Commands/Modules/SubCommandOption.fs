namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type StringSubCommandOptionChoice = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Value: string
}

type StringSubCommandOptionAccept =
    | Choices of StringSubCommandOptionChoice list
    | Autocomplete
    | Any

type StringSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
    Length: (Int0to6000 * Int1to6000) option
    Accept: StringSubCommandOptionAccept
}

type IntegerSubCommandOptionChoice = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Value: int
}

type IntegerSubCommandOptionAccept =
    | Choices of IntegerSubCommandOptionChoice list
    | Autocomplete
    | Any

type IntegerSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
    Range: (int * int) option
    Accept: IntegerSubCommandOptionAccept
}

type BooleanSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
}

type UserSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
}

type ChannelSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
    ChannelTypes: ChannelType list option
}

type RoleSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
}

type MentionableSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
}

type NumberSubCommandOptionChoice = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Value: double
}

type NumberSubCommandOptionAccept =
    | Choices of NumberSubCommandOptionChoice list
    | Autocomplete
    | Any

type NumberSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
    Required: bool
    Range: (double * double) option
    Accept: NumberSubCommandOptionAccept
}

type AttachmentSubCommandOption = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100>
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
