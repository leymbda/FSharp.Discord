namespace FSharp.Discord.Commands

open FSharp.Core.LanguagePrimitives
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
    Localizations: Map<string, string * string>
    Options: ChatInputCommandOptions
    DefaultMemberPermissions: Permission list option
    Nsfw: bool
    Context: CommandContext
}

module ChatInputCommand =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            Options = ChatInputCommandOptions.SubCommand []
            DefaultMemberPermissions = None
            Nsfw = false
            Context = CommandContext.Global {
                IntegrationTypes = None
                Contexts = None
            }
        }

    let addLocale locale name description (v: ChatInputCommand) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: ChatInputCommand) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let addSubCommandGroup subCommandGroup (v: ChatInputCommand) =
        let option = NestedChatInputCommandOption.SubCommandGroup subCommandGroup

        let options =
            match v.Options with
            | ChatInputCommandOptions.Nesting options ->  options @ [option]
            | ChatInputCommandOptions.SubCommand _ -> [option]

        { v with Options = ChatInputCommandOptions.Nesting options }

    let addSubCommand subCommand (v: ChatInputCommand) =
        let option = NestedChatInputCommandOption.SubCommand subCommand

        let options =
            match v.Options with
            | ChatInputCommandOptions.Nesting options ->  options @ [option]
            | ChatInputCommandOptions.SubCommand _ -> [option]

        { v with Options = ChatInputCommandOptions.Nesting options }

    let addOption option (v: ChatInputCommand) =
        let options =
            match v.Options with
            | ChatInputCommandOptions.Nesting _ -> [option]
            | ChatInputCommandOptions.SubCommand options -> options @ [option]

        { v with Options = ChatInputCommandOptions.SubCommand options }

    let setDefaultMemberPermissions permissions (v: ChatInputCommand) =
        { v with DefaultMemberPermissions = Some permissions }

    let setAdministratorOnly (v: ChatInputCommand) =
        { v with DefaultMemberPermissions = Some [EnumOfValue<int64, Permission> 0L] }

    let setNsfw nsfw (v: ChatInputCommand) =
        { v with Nsfw = nsfw }

    let setGuild guildId (v: ChatInputCommand) =
        { v with Context = CommandContext.Guild guildId }

    let setIntegrations types (v: ChatInputCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = Some types; Contexts = None }
            | CommandContext.Global ctx -> { ctx with IntegrationTypes = Some types }

        { v with Context = CommandContext.Global ctx }

    let setContexts contexts (v: ChatInputCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = None; Contexts = Some contexts }
            | CommandContext.Global ctx -> { ctx with Contexts = Some contexts }

        { v with Context = CommandContext.Global ctx }

    let toPayload (command: ChatInputCommand) =
        match command.Context with
        | CommandContext.Global ctx ->
            CommandPayload.Global (new CreateGlobalApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                options = (
                    match command.Options with
                    | ChatInputCommandOptions.Nesting options ->
                        options |> List.map (function
                            | NestedChatInputCommandOption.SubCommand o -> SubCommand.toCommandOption o
                            | NestedChatInputCommandOption.SubCommandGroup o -> SubCommandGroup.toCommandOption o
                        )

                    | ChatInputCommandOptions.SubCommand options ->
                        options |> List.map SubCommandOption.toCommandOption
                ),        
                defaultMemberPermissions = command.DefaultMemberPermissions,
                ?integrationTypes = ctx.IntegrationTypes,
                ?contexts = ctx.Contexts,
                type' = ApplicationCommandType.CHAT_INPUT,
                nsfw = command.Nsfw));

        | CommandContext.Guild guildId ->
            CommandPayload.Guild(guildId, new CreateGuildApplicationCommandPayload(
                name = command.Name,
                description = command.Description,
                options = (
                    match command.Options with
                    | ChatInputCommandOptions.Nesting options ->
                        options |> List.map (function
                            | NestedChatInputCommandOption.SubCommand o -> SubCommand.toCommandOption o
                            | NestedChatInputCommandOption.SubCommandGroup o -> SubCommandGroup.toCommandOption o
                        )

                    | ChatInputCommandOptions.SubCommand options ->
                        options |> List.map SubCommandOption.toCommandOption
                ),   
                defaultMemberPermissions = command.DefaultMemberPermissions,
                type' = ApplicationCommandType.CHAT_INPUT,
                nsfw = command.Nsfw));

type UserCommand = {
    Name: string
    Description: string
    Localizations: Map<string, string * string>
    DefaultMemberPermissions: Permission list option
    Nsfw: bool
    Context: CommandContext
}

module UserCommand =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            DefaultMemberPermissions = None
            Nsfw = false
            Context = CommandContext.Global {
                IntegrationTypes = None
                Contexts = None
            }
        }
        
    let addLocale locale name description (v: UserCommand) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: UserCommand) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setDefaultMemberPermissions permissions (v: UserCommand) =
        { v with DefaultMemberPermissions = Some permissions }
        
    let setAdministratorOnly (v: UserCommand) =
        { v with DefaultMemberPermissions = Some [EnumOfValue<int64, Permission> 0L] }

    let setNsfw nsfw (v: UserCommand) =
        { v with Nsfw = nsfw }

    let setGuild guildId (v: UserCommand) =
        { v with Context = CommandContext.Guild guildId }

    let setIntegrations types (v: UserCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = Some types; Contexts = None }
            | CommandContext.Global ctx -> { ctx with IntegrationTypes = Some types }

        { v with Context = CommandContext.Global ctx }

    let setContexts contexts (v: UserCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = None; Contexts = Some contexts }
            | CommandContext.Global ctx -> { ctx with Contexts = Some contexts }

        { v with Context = CommandContext.Global ctx }

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
    Localizations: Map<string, string * string>
    DefaultMemberPermissions: Permission list option
    Nsfw: bool
    Context: CommandContext
}

module MessageCommand =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            DefaultMemberPermissions = None
            Nsfw = false
            Context = CommandContext.Global {
                IntegrationTypes = None
                Contexts = None
            }
        }
        
    let addLocale locale name description (v: MessageCommand) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: MessageCommand) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setDefaultMemberPermissions permissions (v: MessageCommand) =
        { v with DefaultMemberPermissions = Some permissions }
        
    let setAdministratorOnly (v: MessageCommand) =
        { v with DefaultMemberPermissions = Some [EnumOfValue<int64, Permission> 0L] }

    let setNsfw nsfw (v: MessageCommand) =
        { v with Nsfw = nsfw }

    let setGuild guildId (v: MessageCommand) =
        { v with Context = CommandContext.Guild guildId }

    let setIntegrations types (v: MessageCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = Some types; Contexts = None }
            | CommandContext.Global ctx -> { ctx with IntegrationTypes = Some types }

        { v with Context = CommandContext.Global ctx }

    let setContexts contexts (v: MessageCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = None; Contexts = Some contexts }
            | CommandContext.Global ctx -> { ctx with Contexts = Some contexts }

        { v with Context = CommandContext.Global ctx }

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
    Localizations: Map<string, string * string>
    DefaultMemberPermissions: Permission list option
    Nsfw: bool
    Handler: ApplicationCommandHandlerType
    Context: CommandContext
}

module EntryPointCommand =
    let create name description =
        {
            Name = name
            Description = description
            Localizations = Map.empty
            DefaultMemberPermissions = None
            Nsfw = false
            Handler = ApplicationCommandHandlerType.APP_HANDER
            Context = CommandContext.Global {
                IntegrationTypes = None
                Contexts = None
            }
        }
        
    let addLocale locale name description (v: EntryPointCommand) =
        { v with Localizations = v.Localizations |> Map.add locale (name, description) }

    let removeLocale locale (v: EntryPointCommand) =
        { v with Localizations = v.Localizations |> Map.remove locale }

    let setDefaultMemberPermissions permissions (v: EntryPointCommand) =
        { v with DefaultMemberPermissions = Some permissions }
        
    let setAdministratorOnly (v: EntryPointCommand) =
        { v with DefaultMemberPermissions = Some [EnumOfValue<int64, Permission> 0L] }

    let setNsfw nsfw (v: EntryPointCommand) =
        { v with Nsfw = nsfw }

    let setGuild guildId (v: EntryPointCommand) =
        { v with Context = CommandContext.Guild guildId }

    let setIntegrations types (v: EntryPointCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = Some types; Contexts = None }
            | CommandContext.Global ctx -> { ctx with IntegrationTypes = Some types }

        { v with Context = CommandContext.Global ctx }

    let setContexts contexts (v: EntryPointCommand) =
        let ctx =
            match v.Context with
            | CommandContext.Guild _ -> { IntegrationTypes = None; Contexts = Some contexts }
            | CommandContext.Global ctx -> { ctx with Contexts = Some contexts }

        { v with Context = CommandContext.Global ctx }

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

// TODO: Add localizations to payloads
// TODO: Validate value conditions e.g. string length, regex, etc (?)
// TODO: A single `Command` type may be able to contain a DU with type-specific content to allow for a single definition of shared properties and functions
