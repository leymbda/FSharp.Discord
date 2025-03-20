namespace FSharp.Discord.Commands

open FSharp.Discord.Types

type ChatInputCommandOption =
    | SubCommandGroup of SubCommandGroup
    | SubCommand of SubCommand

type ChatInputCommand = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100> option
    Options: ChatInputCommandOption list option
    DefaultMemberPermissions: string option
    IntegrationTypes: ApplicationIntegrationType list option
    Contexts: InteractionContextType list option
    Nsfw: bool
    GuildId: string option
}

type UserCommand = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100> option
    DefaultMemberPermissions: string option
    IntegrationTypes: ApplicationIntegrationType list option
    Contexts: InteractionContextType list option
    Nsfw: bool
    GuildId: string option
}

type MessageCommand = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100> option
    DefaultMemberPermissions: string option
    IntegrationTypes: ApplicationIntegrationType list option
    Contexts: InteractionContextType list option
    Nsfw: bool
    GuildId: string option
}

type EntryPointCommand = {
    Name: Localized<StringCommandName>
    Description: Localized<String1to100> option
    DefaultMemberPermissions: string option
    IntegrationTypes: ApplicationIntegrationType list option
    Contexts: InteractionContextType list option
    Nsfw: bool
    Handler: ApplicationCommandHandlerType
    GuildId: string option
}

type Command =
    | ChatInput of ChatInputCommand
    | User of UserCommand
    | Message of MessageCommand
    | EntryPoint of EntryPointCommand

//module Command =
//    let zero name = {
//        Name = name
//        NameLocalizations = None
//        Description = None
//        DescriptionLocalizations = None
//        Options = None
//        DefaultMemberPermissions = None
//        IntegrationTypes = None
//        Contexts = None
//        Type = ApplicationCommandType.CHAT_INPUT
//        Nsfw = false
//    }

//    let setName name (command: Command) =
//        { command with Name = name }

//    let setDescription description (command: Command) =
//        { command with Description = Some description }
        
//    let addLocalization locale name description command =
//        let nameLocalizations =
//            command.NameLocalizations
//            |> Option.defaultValue Map.empty
//            |> Map.add locale name

//        let descriptionLocalizations =
//            command.DescriptionLocalizations
//            |> Option.defaultValue Map.empty
//            |> Map.add locale description

//        { command with
//            NameLocalizations = Some nameLocalizations
//            DescriptionLocalizations = Some descriptionLocalizations }

//    let setDefaultMemberPermissions defaultMemberPermissions (command: Command) =
//        { command with DefaultMemberPermissions = Some defaultMemberPermissions }

//        // TODO: Change to permission list from bitfield, then write new functions for this

//    let addIntegrationType integrationType command =
//        let updatedIntegrationTypes =
//            command.IntegrationTypes
//            |> Option.defaultValue []
//            |> List.append [integrationType]
//            |> List.distinct

//        { command with IntegrationTypes = Some updatedIntegrationTypes }

//    let addContext context command =
//        let updatedContexts =
//            command.Contexts
//            |> Option.defaultValue []
//            |> List.append [context]
//            |> List.distinct

//        { command with Contexts = Some updatedContexts }

//    let setType type' (command: Command) =
//        { command with Type = type' }

//    let setNsfw nsfw (command: Command) =
//        { command with Nsfw = nsfw }
