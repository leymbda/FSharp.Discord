namespace FSharp.Discord.Types

open Microsoft.VisualStudio.TestTools.UnitTesting
open System
open System.Text.Json

[<TestClass>]
type StructuresTests () =
    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesActionRow () =
        // Arrange
        let json = """{"type":1,"components":[]}"""

        let expected = Component.ACTION_ROW {
            Type = ComponentType.ACTION_ROW;
            Components = [];
        }

        // Act
        let actual = Json.deserializeF<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)

    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesButton () =
        // Arrange
        let json = """{"type":2,"style":1,"label":"Label","emoji":null,"custom_id":"custom-id","url":null,"disabled":null}"""

        let expected = Component.BUTTON {
            Type = ComponentType.BUTTON;
            Style = ButtonStyle.PRIMARY;
            Label = "Label";
            Emoji = None;
            CustomId = Some "custom-id";
            Url = None;
            Disabled = false;
        }

        // Act
        let actual = Json.deserializeF<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesSelectMenu () =
        // Arrange
        let json = """{"type":3,"custom_id":"custom-id","options":[],"channel_types":null,"placeholder":null,"default_values":null,"min_values":null,"max_values":null,"disabled":null}"""
        
        let expected = Component.SELECT_MENU {
            Type = ComponentType.STRING_SELECT;
            CustomId = "custom-id";
            Options = Some [];
            ChannelTypes = None;
            Placeholder = None;
            DefaultValues = None;
            MinValues = 1;
            MaxValues = 1;
            Disabled = false;
        }

        // Act
        let actual = Json.deserializeF<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Read_CorrectlyDeserializesTextInput () =
        // Arrange
        let json = """{"type":4,"custom_id":"custom-id","style":1,"label":"Label","min_length":null,"max_length":null,"required":null,"value":null,"placeholder":null}"""
        
        let expected = Component.TEXT_INPUT {
            Type = ComponentType.TEXT_INPUT;
            CustomId = "custom-id";
            Style = TextInputStyle.SHORT;
            Label = "Label";
            MinLength = None;
            MaxLength = None;
            Required = false;
            Value = None;
            Placeholder = None;
        }

        // Act
        let actual = Json.deserializeF<Component> json

        // Assert
        Assert.AreEqual<Component>(expected, actual)

    [<TestMethod>]
    member _.ComponentConverter_Read_FailsOnInvalidComponentType () =
        // Arrange
        let json = """{"type":0}"""

        // Act
        let res () = Json.deserializeF<Component> json |> ignore

        // Assert
        Assert.ThrowsException<JsonException> res |> ignore

    [<TestMethod>]
    member _.ComponentConverter_Read_FailsOnInvalidJsonString () =
        // Arrange
        let json = """{this is in valid json}"""

        // Act
        let res () = Json.deserializeF<Component> json |> ignore

        // Assert
        Assert.ThrowsException<JsonException> res |> ignore

    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesActionRow () =
        // Arrange
        let actionRow = Component.ACTION_ROW {
            Type = ComponentType.ACTION_ROW;
            Components = [];
        }

        let expected = """{"type":1,"components":[]}"""

        // Act
        let actual = Json.serializeF actionRow

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesButton () =
        // Arrange
        let button = Component.BUTTON {
            Type = ComponentType.BUTTON;
            Style = ButtonStyle.PRIMARY;
            Label = "Label";
            Emoji = None;
            CustomId = Some "custom-id";
            Url = None;
            Disabled = false;
        }

        let expected = """{"type":2,"style":1,"label":"Label","emoji":null,"custom_id":"custom-id","url":null,"disabled":null}"""

        // Act
        let actual = Json.serializeF button

        // Assert
        Assert.AreEqual<string>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesSelectMenu () =
        // Arrange
        let selectMenu = Component.SELECT_MENU {
            Type = ComponentType.STRING_SELECT;
            CustomId = "custom-id";
            Options = Some [];
            ChannelTypes = None;
            Placeholder = None;
            DefaultValues = None;
            MinValues = 1;
            MaxValues = 1;
            Disabled = false;
        }

        let expected = """{"type":3,"custom_id":"custom-id","options":[],"channel_types":null,"placeholder":null,"default_values":null,"min_values":null,"max_values":null,"disabled":null}"""
        
        // Act
        let actual = Json.serializeF selectMenu

        // Assert
        Assert.AreEqual<string>(expected, actual)
        
    [<TestMethod>]
    member _.ComponentConverter_Write_CorrectlySerializesTextInput () =
        // Arrange
        let textInput = Component.TEXT_INPUT {
            Type = ComponentType.TEXT_INPUT;
            CustomId = "custom-id";
            Style = TextInputStyle.SHORT;
            Label = "Label";
            MinLength = None;
            MaxLength = None;
            Required = false;
            Value = None;
            Placeholder = None;
        }

        let expected = """{"type":4,"custom_id":"custom-id","style":1,"label":"Label","min_length":null,"max_length":null,"required":null,"value":null,"placeholder":null}"""

        // Act
        let actual = Json.serializeF textInput

        // Assert
        Assert.AreEqual<string>(expected, actual)
