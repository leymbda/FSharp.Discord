[<AutoOpen>]
module FSharp.Discord.Commands.Expressions

type [<Struct; NoComparison; NoEquality>] SubCommandBuilder(name: string, description: string) =
    member _.Yield (_: unit) = SubCommand.create name description
    member _.Yield (o: SubCommand) = o
    
    member inline _.Delay(f: unit -> 'a) = f()
    member inline _.For(o: SubCommand, _: unit -> unit) = o

    [<CustomOperation>] member _.test o = o

let inline subcommand name description = SubCommandBuilder(name, description)

type [<Struct; NoComparison; NoEquality>] SubCommandGroupBuilder(name: string, description: string) =
    member _.Yield (_: unit) = SubCommandGroup.create name description
    member _.Yield (o: SubCommandGroup) = o

    member inline _.Delay(f: unit -> 'a) = f()
    member inline _.For(o: SubCommandGroup, _: unit -> unit) = o

    member inline _.Yield (o: SubCommand) = o
    member inline _.Combine(o: SubCommand, _: unit) = o
    member inline _.For(c: SubCommandGroup, [<InlineIfLambda>] f: unit -> SubCommand) =
        SubCommandGroup.addSubCommand (f()) c

let inline group name description = SubCommandGroupBuilder(name, description)

type [<Struct; NoComparison; NoEquality>] CommandBuilder(name: string, description: string) =
    // Yields
    member _.Yield (_: unit) = ()
    member _.Yield (c: ChatInputCommand) = c
    member _.Yield (c: UserCommand) = c
    member _.Yield (c: MessageCommand) = c
    member _.Yield (c: EntryPointCommand) = c

    // Initialisers
    [<CustomOperation>] member _.chatInput (_: unit) = ChatInputCommand.create name description
    [<CustomOperation>] member _.userContext (_: unit) = UserCommand.create name description
    [<CustomOperation>] member _.messageContext (_: unit) = MessageCommand.create name description
    [<CustomOperation>] member _.entryPoint (_: unit) = EntryPointCommand.create name description

    // Common properties
    [<CustomOperation>] member _.permissions (c, permissions) = ChatInputCommand.setDefaultMemberPermissions permissions c
    [<CustomOperation>] member _.permissions (c, permissions) = UserCommand.setDefaultMemberPermissions permissions c
    [<CustomOperation>] member _.permissions (c, permissions) = MessageCommand.setDefaultMemberPermissions permissions c
    [<CustomOperation>] member _.permissions (c, permissions) = EntryPointCommand.setDefaultMemberPermissions permissions c

    [<CustomOperation>] member _.nsfw c = ChatInputCommand.setNsfw true c
    [<CustomOperation>] member _.nsfw c = UserCommand.setNsfw true c
    [<CustomOperation>] member _.nsfw c = MessageCommand.setNsfw true c
    [<CustomOperation>] member _.nsfw c = EntryPointCommand.setNsfw true c

    [<CustomOperation>] member _.guild (c, guildId) = ChatInputCommand.setGuild guildId c
    [<CustomOperation>] member _.guild (c, guildId) = UserCommand.setGuild guildId c
    [<CustomOperation>] member _.guild (c, guildId) = MessageCommand.setGuild guildId c
    [<CustomOperation>] member _.guild (c, guildId) = EntryPointCommand.setGuild guildId c

    [<CustomOperation>] member _.integrations (c, types) = ChatInputCommand.setIntegrations types c
    [<CustomOperation>] member _.integrations (c, types) = UserCommand.setIntegrations types c
    [<CustomOperation>] member _.integrations (c, types) = MessageCommand.setIntegrations types c
    [<CustomOperation>] member _.integrations (c, types) = EntryPointCommand.setIntegrations types c

    [<CustomOperation>] member _.contexts (c, contexts) = ChatInputCommand.setContexts contexts c
    [<CustomOperation>] member _.contexts (c, contexts) = UserCommand.setContexts contexts c
    [<CustomOperation>] member _.contexts (c, contexts) = MessageCommand.setContexts contexts c
    [<CustomOperation>] member _.contexts (c, contexts) = EntryPointCommand.setContexts contexts c

    // Chat input command option functions
    member inline _.Delay(f: unit -> SubCommandGroup) = f()
    member inline _.Yield (o: SubCommandGroup) = o
    member inline _.Combine(o: SubCommandGroup, _: unit) = o
    member inline _.For(c: ChatInputCommand, [<InlineIfLambda>] f: unit -> SubCommandGroup) =
        ChatInputCommand.addSubCommandGroup (f()) c
        
    member inline _.Delay(f: unit -> SubCommand) = f()
    member inline _.Yield (o: SubCommand) = o
    member inline _.Combine(o: SubCommand, _: unit) = o
    member inline _.For(c: ChatInputCommand, [<InlineIfLambda>] f: unit -> SubCommand) =
        ChatInputCommand.addSubCommand (f()) c

    //[<CustomOperation>] member _.string (c, option) = ChatInputCommand.addOption (SubCommandOption.String option) c
    //[<CustomOperation>] member _.integer (c, option) = ChatInputCommand.addOption (SubCommandOption.Integer option) c
    //[<CustomOperation>] member _.boolean (c, option) = ChatInputCommand.addOption (SubCommandOption.Boolean option) c
    //[<CustomOperation>] member _.user (c, option) = ChatInputCommand.addOption (SubCommandOption.User option) c
    //[<CustomOperation>] member _.channel (c, option) = ChatInputCommand.addOption (SubCommandOption.Channel option) c
    //[<CustomOperation>] member _.role (c, option) = ChatInputCommand.addOption (SubCommandOption.Role option) c
    //[<CustomOperation>] member _.mentionable (c, option) = ChatInputCommand.addOption (SubCommandOption.Mentionable option) c
    //[<CustomOperation>] member _.number (c, option) = ChatInputCommand.addOption (SubCommandOption.Number option) c
    //[<CustomOperation>] member _.attachment (c, option) = ChatInputCommand.addOption (SubCommandOption.Attachment option) c


let command name description = CommandBuilder(name, description)

//let test () =
//    command "name" "description" {
//        chatInput
//        guild "guildId"
//        guild "guildId"
//        group "name" "description" {
//            subcommand "name" "description" {
//                test
//            }
//        }
//        group "name" "description" {
//            subcommand "name" "description" {
//                test
//            }
//        }
//        nsfw
//    }

// GOAL:
let piped () =
    ChatInputCommand.create "name" "description"
    |> ChatInputCommand.setGuild "guildId"
    |> ChatInputCommand.addSubCommandGroup (
        SubCommandGroup.create "name" "description"
        |> SubCommandGroup.addSubCommand (
            SubCommand.create "name" "description"
            |> SubCommand.addOption (
                IntegerSubCommandOption.create "name" "description"
                |> IntegerSubCommandOption.setRange 1 10
                |> SubCommandOption.Integer
            )
        )
    )
    |> ChatInputCommand.addSubCommand (
        SubCommand.create "name" "description"
        |> SubCommand.addOption (
            IntegerSubCommandOption.create "name" "description"
            |> IntegerSubCommandOption.setRange 1 10
            |> SubCommandOption.Integer
        )
    )

// TODO: Is there some way something like the fable router could be created for parsing interactions? Could that apply to this? Needs to be experimented with
