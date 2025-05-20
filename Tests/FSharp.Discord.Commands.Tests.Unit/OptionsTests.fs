﻿namespace FSharp.Discord.Commands

open FSharp.Discord.Types
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type OptionsTests () =
    [<TestMethod>]
    member _.SubCommand_FindsCorrectSubCommand () =
        // Arrange
        let name = "command"

        let expectedSubcommandOptions = [
            {
                Name = "option"
                Type = ApplicationCommandOptionType.STRING
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "option")
                Options = None
                Focused = None
            }
        ]

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.SUB_COMMAND
                Value = None
                Options = Some expectedSubcommandOptions
                Focused = None
            }
        ]

        // Act
        let subcommandOptions =
            match options with
            | SubCommand name options -> options
            | _ -> failwith "Could not find subcommand"

        // Assert
        Assert.AreSame(expectedSubcommandOptions, subcommandOptions)

    [<TestMethod>]
    member _.SubCommand_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "subcommand"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | SubCommand name _ -> failwith "Unexpectedly found non-existent subcommand"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.SubCommand_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "subcommand"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | SubCommand name _ -> failwith "Unexpectedly found non-existent subcommand"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.SubCommandGroup_FindsCorrectSubCommandGroup () =
        // Arrange
        let name = "command"

        let expectedSubcommandOptions = [
            {
                Name = "option"
                Type = ApplicationCommandOptionType.STRING
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "option")
                Options = None
                Focused = None
            }
        ]

        let expectedSubcommandGroupOptions = [
            {
                Name = "subcommand"
                Type = ApplicationCommandOptionType.SUB_COMMAND
                Value = None
                Options = Some expectedSubcommandOptions
                Focused = None
            }
        ]

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.SUB_COMMAND_GROUP
                Value = None
                Options = Some expectedSubcommandGroupOptions
                Focused = None
            }
        ]

        // Act
        let subcommandGroupOptions =
            match options with
            | SubCommandGroup name options -> options
            | _ -> failwith "Could not find subcommand group"

        // Assert
        Assert.AreSame(expectedSubcommandGroupOptions, subcommandGroupOptions)

    [<TestMethod>]
    member _.SubCommandGroup_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "subcommandGroup"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | SubCommandGroup name _ -> failwith "Unexpectedly found non-existent subcommand group"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.SubCommandGroup_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "subcommandGroup"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | SubCommand name _ -> failwith "Unexpectedly found non-existent subcommand group"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.String_FindsCorrectString () =
        // Arrange
        let name = "string"
        let value = "string"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.STRING
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let string =
            match options with
            | String.Required name string -> string
            | _ -> failwith "Could not find string"

        // Assert
        Assert.AreEqual<string>(value, string)

    [<TestMethod>]
    member _.String_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "string"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | String.Required name _ -> failwith "Unexpectedly found non-existent string"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.String_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "string"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | String.Required name _ -> failwith "Unexpectedly found non-existent string"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Integer_FindsCorrectInteger () =
        // Arrange
        let name = "integer"
        let value = 1

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.INTEGER
                Value = Some (ApplicationCommandInteractionDataOptionValue.Int value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let integer =
            match options with
            | Integer.Required name integer -> integer
            | _ -> failwith "Could not find integer"

        // Assert
        Assert.AreEqual<int>(value, integer)

    [<TestMethod>]
    member _.Integer_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "integer"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | Integer.Required name _ -> failwith "Unexpectedly found non-existent integer"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Integer_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "integer"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Integer.Required name _ -> failwith "Unexpectedly found non-existent integer"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Boolean_FindsCorrectBoolean () =
        // Arrange
        let name = "boolean"
        let value = true

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.BOOLEAN
                Value = Some (ApplicationCommandInteractionDataOptionValue.Bool value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let boolean =
            match options with
            | Boolean.Required name boolean -> boolean
            | _ -> failwith "Could not find boolean"

        // Assert
        Assert.AreEqual<bool>(value, boolean)

    [<TestMethod>]
    member _.Boolean_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "boolean"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | Boolean.Required name _ -> failwith "Unexpectedly found non-existent boolean"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Boolean_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "boolean"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Boolean.Required name _ -> failwith "Unexpectedly found non-existent boolean"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.UserId_FindsCorrectUser () =
        // Arrange
        let name = "user"
        let value = "userId"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.USER
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let userId =
            match options with
            | UserId.Required name userId -> userId
            | _ -> failwith "Could not find user"

        // Assert
        Assert.AreEqual<string>(value, userId)

    [<TestMethod>]
    member _.UserId_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "user"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | UserId.Required name _ -> failwith "Unexpectedly found non-existent user"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.UserId_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "user"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | UserId.Required name _ -> failwith "Unexpectedly found non-existent user"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.ChannelId_FindsCorrectChannel () =
        // Arrange
        let name = "channel"
        let value = "channelId"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.CHANNEL
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let channelId =
            match options with
            | ChannelId.Required name channelId -> channelId
            | _ -> failwith "Could not find channel"

        // Assert
        Assert.AreEqual<string>(value, channelId)

    [<TestMethod>]
    member _.ChannelId_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "channel"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | ChannelId.Required "channel" _ -> failwith "Unexpectedly found non-existent channel"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.ChannelId_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "channel"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | ChannelId.Required name _ -> failwith "Unexpectedly found non-existent channel"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.RoleId_FindsCorrectRole () =
        // Arrange
        let name = "role"
        let value = "roleId"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let roleId =
            match options with
            | RoleId.Required name roleId -> roleId
            | _ -> failwith "Could not find role"

        // Assert
        Assert.AreEqual<string>(value, roleId)

    [<TestMethod>]
    member _.RoleId_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "role"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.CHANNEL
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "channelId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | RoleId.Required name _ -> failwith "Unexpectedly found non-existent role"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.RoleId_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "role"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | RoleId.Required name _ -> failwith "Unexpectedly found non-existent role"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.MentionableId_FindsCorrectMentionable () =
        // Arrange
        let name = "mentionable"
        let value = "mentionableId"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.MENTIONABLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let mentionableId =
            match options with
            | MentionableId.Required name mentionableId -> mentionableId
            | _ -> failwith "Could not find mentionable"

        // Assert
        Assert.AreEqual<string>(value, mentionableId)

    [<TestMethod>]
    member _.MentionableId_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "mentionable"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | MentionableId.Required name _ -> failwith "Unexpectedly found non-existent mentionable"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.MentionableId_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "mentionable"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | MentionableId.Required name _ -> failwith "Unexpectedly found non-existent mentionable"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Number_FindsCorrectNumber () =
        // Arrange
        let name = "number"
        let value = 12.34

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.NUMBER
                Value = Some (ApplicationCommandInteractionDataOptionValue.Double value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let double =
            match options with
            | Number.Required name double -> double
            | _ -> failwith "Could not find number"

        // Assert
        Assert.AreEqual<double>(value, double)

    [<TestMethod>]
    member _.Number_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "number"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | Number.Required name _ -> failwith "Unexpectedly found non-existent number"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Number_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "number"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Number.Required name _ -> failwith "Unexpectedly found non-existent number"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.AttachmentId_FindsCorrectAttachment () =
        // Arrange
        let name = "attachment"
        let value = "attachmentId"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ATTACHMENT
                Value = Some (ApplicationCommandInteractionDataOptionValue.String value)
                Options = None
                Focused = None
            }
        ]

        // Act
        let attachmentId =
            match options with
            | AttachmentId.Required name attachmentId -> attachmentId
            | _ -> failwith "Could not find attachment"

        // Assert
        Assert.AreEqual<string>(value, attachmentId)

    [<TestMethod>]
    member _.AttachmentId_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "attachment"

        let options = [
            {
                Name = name
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | AttachmentId.Required name _ -> failwith "Unexpectedly found non-existent attachment"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.AttachmentId_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "attachment"

        let options: ApplicationCommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | AttachmentId.Required name _ -> failwith "Unexpectedly found non-existent attachment"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Options_FetchesMultipleOptions () =
        // Arrange
        let options = [
            {
                Name = "role1"
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId1")
                Options = None
                Focused = None
            }
            {
                Name = "role2"
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId2")
                Options = None
                Focused = None
            }
            {
                Name = "channel"
                Type = ApplicationCommandOptionType.CHANNEL
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "channelId")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res =
            match options with
            | RoleId.Required "role1" roleId1 & RoleId.Required "role2" roleId2 & ChannelId.Required "channel" channelId ->
                {|
                    roleId1 = roleId1;
                    roleId2 = roleId2;
                    channelId = channelId;
                |}
            | _ -> failwith "Could not find all options"

        // Assert
        Assert.AreEqual<string>("roleId1", res.roleId1)
        Assert.AreEqual<string>("roleId2", res.roleId2)
        Assert.AreEqual<string>("channelId", res.channelId)

    [<TestMethod>]
    member _.Options_FailsWhenAtLeastOneOptionNotAvailable () =
        // Arrange
        let options = [
            {
                Name = "role1"
                Type = ApplicationCommandOptionType.ROLE
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "roleId1")
                Options = None
                Focused = None
            }
            {
                Name = "channel"
                Type = ApplicationCommandOptionType.CHANNEL
                Value = Some (ApplicationCommandInteractionDataOptionValue.String "channel")
                Options = None
                Focused = None
            }
        ]

        // Act
        let res () =
            match options with
            | RoleId.Required "role1" _ & RoleId.Required "role2" _ & ChannelId.Required "channel" _ -> ()
            | _ -> failwith "Could not find all options"

        // Assert
        Assert.ThrowsException(res) |> ignore
        ()
