namespace FSharp.Discord.Commands

type SubCommandGroup = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100> option
    Options: SubCommand list
}
