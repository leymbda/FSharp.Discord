namespace FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Microsoft.VisualStudio.TestTools.UnitTesting
open Thoth.Json.Net

type SerializationVariantsModel = {
    Required: string
    Optional: string option
    Nullable: string option
    OptionalAndNullable: string option option
}

module SerializationVariantsModel =
    let decoder path v =
        Decode.object (fun get -> {
            Required = get |> Get.required "required" Decode.string
            Optional = get |> Get.optional "optional" Decode.string
            Nullable = get |> Get.nullable "nullable" Decode.string
            OptionalAndNullable = get |> Get.optnull "optionalAndNullable" Decode.string
        }) path v

    let encoder (v: SerializationVariantsModel) =
        Encode.object [
            "required", Encode.string v.Required
            match v.Optional with | Some p -> "optional", Encode.string p | None -> ()
            "nullable", Encode.option Encode.string v.Nullable
            match v.OptionalAndNullable with | Some p -> "optionalAndNullable", Encode.option Encode.string p | None -> ()
        ]

type Pair<'a> = {
    Decoded: 'a
    Encoded: string
}

module Pair =
    let create<'a> decoded encoded: Pair<'a> = {
        Decoded = decoded
        Encoded = encoded
    }

    let ErrorResponse =
        create<ErrorResponse>
            {
                Code = JsonErrorCode.ACCOUNT_VERIFICATION_REQUIRED
                Message = "message"
                Errors = Map.empty |> Map.add "name" "value"
            }
            """{"code":40002,"message":"message","errors":{"name":"value"}}"""

[<TestClass>]
type ThothTests () =
    [<TestMethod>]
        // Arrange
    member _.``Optional/nullable values decode correctly - Values present`` () =
        let value = """{"required":"value","optional":"value","nullable":"value","optionalAndNullable":"value"}"""

        let expected = {
            Required = "value"
            Optional = Some "value"
            Nullable = Some "value"
            OptionalAndNullable = Some (Some "value")
        }

        // Act
        let res = Decode.fromString SerializationVariantsModel.decoder value

        // Assert
        match res with
        | Error err -> Assert.Fail err
        | Ok actual -> Assert.AreEqual<SerializationVariantsModel>(expected, actual)
        
    [<TestMethod>]
    member _.``Optional/nullable values encode correctly - Values present`` () =
        // Arrange
        let value = {
            Required = "value"
            Optional = Some "value"
            Nullable = Some "value"
            OptionalAndNullable = Some (Some "value")
        }

        let expected = """{"required":"value","optional":"value","nullable":"value","optionalAndNullable":"value"}"""

        // Act
        let actual = Encode.toString 0 (SerializationVariantsModel.encoder value)

        // Assert
        Assert.AreEqual<string>(expected, actual)
        
    [<TestMethod>]
        // Arrange
    member _.``Optional/nullable values decode correctly - Values not present and o/n null`` () =
        let value = """{"required":"value","nullable":null,"optionalAndNullable":null}"""

        let expected = {
            Required = "value"
            Optional = None
            Nullable = None
            OptionalAndNullable = Some None
        }

        // Act
        let res = Decode.fromString SerializationVariantsModel.decoder value

        // Assert
        match res with
        | Error err -> Assert.Fail err
        | Ok actual -> Assert.AreEqual<SerializationVariantsModel>(expected, actual)
        
    [<TestMethod>]
    member _.``Optional/nullable values encode correctly - Values not present and o/n null`` () =
        // Arrange
        let value = {
            Required = "value"
            Optional = None
            Nullable = None
            OptionalAndNullable = Some None
        }
        
        let expected = """{"required":"value","nullable":null,"optionalAndNullable":null}"""

        // Act
        let actual = Encode.toString 0 (SerializationVariantsModel.encoder value)

        // Assert
        Assert.AreEqual<string>(expected, actual)
        
    [<TestMethod>]
        // Arrange
    member _.``Optional/nullable values decode correctly - Values not present and o/n undefined`` () =
        let value = """{"required":"value","nullable":null}"""

        let expected = {
            Required = "value"
            Optional = None
            Nullable = None
            OptionalAndNullable = None
        }

        // Act
        let res = Decode.fromString SerializationVariantsModel.decoder value

        // Assert
        match res with
        | Error err -> Assert.Fail err
        | Ok actual -> Assert.AreEqual<SerializationVariantsModel>(expected, actual)
        
    [<TestMethod>]
    member _.``Optional/nullable values encode correctly - Values not present and o/n undefined`` () =
        // Arrange
        let value = {
            Required = "value"
            Optional = None
            Nullable = None
            OptionalAndNullable = None
        }
        
        let expected = """{"required":"value","nullable":null}"""

        // Act
        let actual = Encode.toString 0 (SerializationVariantsModel.encoder value)

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    member _.``ErrorResponse.decoder`` () =
        // Arrange
        let value = Pair.ErrorResponse.Encoded
        let expected = Pair.ErrorResponse.Decoded

        // Act
        let res = Decode.fromString ErrorResponse.decoder value

        // Assert
        match res with
        | Error err -> Assert.Fail err
        | Ok actual -> Assert.AreEqual<ErrorResponse>(expected, actual)
        
    [<TestMethod>]
    member _.``ErrorResponse.encoder`` () =
        // Arrange
        let value = Pair.ErrorResponse.Decoded
        let expected = Pair.ErrorResponse.Encoded

        // Act
        let actual = Encode.toString 0 (ErrorResponse.encoder value)

        // Assert
        Assert.AreEqual<string>(expected, actual)
        