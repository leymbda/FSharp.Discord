namespace FSharp.Discord.Commands

type SubCommandGroup = {
    Name: string
    Description: string option
    Options: SubCommand list
}
