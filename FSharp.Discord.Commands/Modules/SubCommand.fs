namespace FSharp.Discord.Commands

type SubCommand = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100> option
    Options: SubCommandOption list
}
