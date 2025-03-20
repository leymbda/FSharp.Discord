namespace FSharp.Discord.Commands

type SubCommand = {
    Name: string
    Description: string option
    Options: SubCommandOption list
}
