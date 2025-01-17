namespace FSharp.Discord.Types

open Microsoft.VisualStudio.TestTools.UnitTesting
open System
open System.Text.Json
open System.Text.Json.Serialization

[<TestClass>]
type JsonExceptionTests () =
    [<TestMethod>]
    [<DataRow "Error 1">]
    [<DataRow "Error 2">]
    [<DataRow "">]
    member _.raise_RaisesException (message: string) =
        // Arrange
        
        // Act
        let res = fun _ -> JsonException.raise message

        // Assert
        Assert.ThrowsException<JsonException> res |> ignore
        try res() with | :? JsonException as ex -> Assert.AreEqual<string>(message, ex.Message)

    [<TestMethod>]
    [<DataRow "Error 1">]
    [<DataRow "Error 2">]
    [<DataRow "">]
    member _.raiseThunk_CreatesCallbackToRaiseMessage (message: string) =
        // Arrange
        
        // Act
        let res = JsonException.raiseThunk message

        // Assert
        Assert.ThrowsException<JsonException> res |> ignore
        try res() with | :? JsonException as ex -> Assert.AreEqual<string>(message, ex.Message)

    [<TestMethod>]
    member _.raiseIf_RaisesGenericExceptionIfConditionIsTrue () =
        // Arrange
        let condition = true

        // Act
        let res = fun _ -> JsonException.raiseIf condition
        
        // Assert
        Assert.ThrowsException<JsonException> res |> ignore

        try res() with
        | :? JsonException -> ()
        | _ -> Assert.Fail()

    [<TestMethod>]
    member _.raiseIf_DoesNotRaiseGenericExceptionIfConditionIsFalse () =
        // Arrange
        let condition = false

        // Act
        let res = fun _ -> JsonException.raiseIf condition
        
        // Assert
        try res() with | :? JsonException -> Assert.Fail()

type UnixEpochRecord = {
    [<JsonConverter(typeof<JsonConverter.UnixEpoch>)>] Timestamp: DateTime
}

type NullUndefinedAsBoolRecord = {
    [<JsonConverter(typeof<JsonConverter.NullUndefinedAsBool>)>] State: bool
}

[<TestClass>]
type JsonConverterTests () =
    [<TestMethod>]
    member _.UnixEpoch_CorrectlySerializesTimestamp () =
        // Arrange
        let time = DateTime(2024, 9, 27, 0, 0, 0, DateTimeKind.Utc)
        let expected = 1727395200000L
        let object = { Timestamp = time; }

        // Act
        let json = Json.serializeF object

        // Assert
        Assert.IsTrue(json.Contains(expected.ToString()))
        
    [<TestMethod>]
    member _.UnixEpoch_CorrectlyDeserializesDateTime () =
        // Arrange
        let expected = DateTime(2024, 9, 27, 0, 0, 0, DateTimeKind.Utc)
        let timestamp = 1727395200000L
        let json = $"""{{"Timestamp":{timestamp}}}"""

        // Act
        let actual = Json.deserializeF<UnixEpochRecord> json

        // Assert
        Assert.AreEqual<DateTime>(expected, actual.Timestamp)

    [<TestMethod>]
    member _.NullUndefinedAsBool_ThrowsErrorIfSerializesUndefined () =
        // Arrange
        let original = { State = false }

        // Act
        let res () = Json.serializeF original |> ignore

        // Assert
        Assert.ThrowsException<NotImplementedException> res |> ignore

    [<TestMethod>]
    member _.NullUndefinedAsBool_ThrowsErrorIfSSerializesNull () =
        // Arrange
        let original = { State = true }

        // Act
        let res () = Json.serializeF original |> ignore

        // Assert
        Assert.ThrowsException<NotImplementedException> res |> ignore

    [<TestMethod>]
    member _.NullUndefinedAsBool_CorrectlyDeserializesFalse () =
        // Arrange
        let original = "{}"

        // Act
        let actual = Json.deserializeF<NullUndefinedAsBoolRecord> original

        // Assert
        Assert.IsFalse(actual.State)

    [<TestMethod>]
    member _.NullUndefinedAsBool_CorrectlyDeserializesTrue () =
        // Arrange
        let original = """{"State":null}"""

        // Act
        let actual = Json.deserializeF<NullUndefinedAsBoolRecord> original

        // Assert
        Assert.IsTrue(actual.State)
