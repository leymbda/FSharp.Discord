namespace FSharp.Discord.Commands

open FSharp.Discord.Rest
open FSharp.Discord.Types

type GlobalCommandContext = {
    IntegrationTypes: ApplicationIntegrationType list option
    Contexts: InteractionContextType list option    
}

type CommandContext =
    | Global of GlobalCommandContext
    | Guild of guildId: string

type CommandPayload =
    | Global of payload: CreateGlobalApplicationCommandPayload
    | Guild of guildId: string * payload: CreateGuildApplicationCommandPayload

type NestedChatInputCommandOption =
    | SubCommandGroup of SubCommandGroup
    | SubCommand of SubCommand

type ChatInputCommandOptions =
    | Nesting of NestedChatInputCommandOption list
    | SubCommand of SubCommandOption list

type ChatInputCommand = {
    Name: string
    Description: string
    Options: ChatInputCommandOptions
    DefaultMemberPermissions: string option
    Nsfw: bool
    Context: CommandContext
}

module ChatInputCommand =
    let create name description =
        {
            Name = name
            Description = description
            Options = ChatInputCommandOptions.SubCommand []
            DefaultMemberPermissions = None
            Nsfw = false
            Context = CommandContext.Global {
                IntegrationTypes = None
                Contexts = None
            }
        }

    // TODO: Options functions

    let setDefaultMemberPermissions permissions (v: ChatInputCommand) =
        { v with DefaultMemberPermissions = Some permissions }

    let removeDefaultMemberPermissions (v: ChatInputCommand) =
        { v with DefaultMemberPermissions = None }

    let setNsfw nsfw (v: ChatInputCommand) =
        { v with Nsfw = nsfw }

    // TODO: Context modifier functions

    let toPayload (command: ChatInputCommand) =
        match command.Context with
        | CommandContext.Global ctx ->
            CommandPayload.Global (new CreateGlobalApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                // TODO: options,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                ?integrationTypes = ctx.IntegrationTypes,
                ?contexts = ctx.Contexts,
                type' = ApplicationCommandType.CHAT_INPUT,
                nsfw = command.Nsfw));

        | CommandContext.Guild guildId ->
            CommandPayload.Guild(guildId, new CreateGuildApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                // TODO: options,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                type' = ApplicationCommandType.CHAT_INPUT,
                nsfw = command.Nsfw));

type UserCommand = {
    Name: string
    Description: string
    DefaultMemberPermissions: string option
    Nsfw: bool
    Context: CommandContext
}

module UserCommand =
    // TODO: User command functions

    let toPayload (command: UserCommand) =
        match command.Context with
        | CommandContext.Global ctx ->
            CommandPayload.Global (new CreateGlobalApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                ?integrationTypes = ctx.IntegrationTypes,
                ?contexts = ctx.Contexts,
                type' = ApplicationCommandType.USER,
                nsfw = command.Nsfw));

        | CommandContext.Guild guildId ->
            CommandPayload.Guild(guildId, new CreateGuildApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                type' = ApplicationCommandType.USER,
                nsfw = command.Nsfw));

type MessageCommand = {
    Name: string
    Description: string
    DefaultMemberPermissions: string option
    Nsfw: bool
    Context: CommandContext
}

module MessageCommand =
    // TODO: Message command functions

    let toPayload (command: MessageCommand) =
        match command.Context with
        | CommandContext.Global ctx ->
            CommandPayload.Global (new CreateGlobalApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                ?integrationTypes = ctx.IntegrationTypes,
                ?contexts = ctx.Contexts,
                type' = ApplicationCommandType.MESSAGE,
                nsfw = command.Nsfw));

        | CommandContext.Guild guildId ->
            CommandPayload.Guild(guildId, new CreateGuildApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                type' = ApplicationCommandType.MESSAGE,
                nsfw = command.Nsfw));

type EntryPointCommand = {
    Name: string
    Description: string
    DefaultMemberPermissions: string option
    Nsfw: bool
    Handler: ApplicationCommandHandlerType
    Context: CommandContext
}

module EntryPointCommand =
    // TODO: Entry point command functions

    let toPayload (command: EntryPointCommand) =
        match command.Context with
        | CommandContext.Global ctx ->
            CommandPayload.Global (new CreateGlobalApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                ?integrationTypes = ctx.IntegrationTypes,
                ?contexts = ctx.Contexts,
                type' = ApplicationCommandType.PRIMARY_ENTRY_POINT,
                nsfw = command.Nsfw));

        | CommandContext.Guild guildId ->
            CommandPayload.Guild(guildId, new CreateGuildApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                defaultMemberPermissions = command.DefaultMemberPermissions,
                type' = ApplicationCommandType.PRIMARY_ENTRY_POINT,
                nsfw = command.Nsfw));

type Command =
    | ChatInput of ChatInputCommand
    | User of UserCommand
    | Message of MessageCommand
    | EntryPoint of EntryPointCommand

module Command =
    let toPayload (command: Command) =
        match command with
        | Command.ChatInput c -> ChatInputCommand.toPayload c
        | Command.User c -> UserCommand.toPayload c
        | Command.Message c -> MessageCommand.toPayload c
        | Command.EntryPoint c -> EntryPointCommand.toPayload c

// TODO: Add localizations
// TODO: Validate value conditions e.g. string length, regex, etc (?)
