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
            OptionalAndNullable = get |> Get.optinull "optionalAndNullable" Decode.string
        }) path v

    let encoder (v: SerializationVariantsModel) =
        Encode.object ([]
            |> Encode.required "required" Encode.string v.Required
            |> Encode.optional "optional" Encode.string v.Optional
            |> Encode.nullable "nullable" Encode.string v.Nullable
            |> Encode.optinull "optionalAndNullable" Encode.string v.OptionalAndNullable
        )

type ChildA = {
    A: bool
}

module ChildA =
    let decoder path v =
        Decode.object (fun get -> {
            A = get |> Get.required "A" Decode.bool
        }) path v
        
    let internal encodeProperties (v: ChildA) =
        Encode.required "A" Encode.bool v.A []

    let encoder (v: ChildA) =
        Encode.object (encodeProperties v)

type ChildB = {
    B: bool
}

module ChildB =
    let decoder path v =
        Decode.object (fun get -> {
            B = get |> Get.required "B" Decode.bool
        }) path v

    let internal encodeProperties (v: ChildB) =
        Encode.required "B" Encode.bool v.B []

    let encoder (v: ChildB) =
        Encode.object (encodeProperties v)

type Parent = {
    A: ChildA
    B: ChildB
}

module Parent =
    let decoder path v =
        Decode.object (fun get -> {
            A = get |> Get.extract ChildA.decoder
            B = get |> Get.extract ChildB.decoder
        }) path v

    let encoder (v: Parent) =
        Encode.object (ChildA.encodeProperties v.A @ ChildB.encodeProperties v.B)
        
type ParentOpt = {
    A: ChildA
    B: ChildB option
}

module ParentOpt =
    let decoder path v =
        Decode.object (fun get -> {
            A = get |> Get.extract ChildA.decoder
            B = get |> Get.extractOpt ChildB.decoder
        }) path v

    let encoder (v: ParentOpt) =
        Encode.object (ChildA.encodeProperties v.A @ (v.B |> Option.map ChildB.encodeProperties |> Option.defaultValue []))

type Exists = {
    Value: bool
}

module Exists =
    let decoder path v =
        Decode.object (fun get -> {
            Value = get |> Get.exists "value"
        }) path v

    let encoder (v: Exists) =
        Encode.object ([]
            |> Encode.exists "value" Encode.nil v.Value
        )

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
    member _.``Decode.mapkv - Correctly deserializes to map`` () =
        // Arrange
        let id = "1234567890"
        let data = $"""{{"0": "{id}"}}"""

        // Act
        let res = Decode.fromString (Decode.mapkv (int >> enum<ApplicationIntegrationType> >> Some) Decode.string) data

        // Assert
        match res with
        | Error err -> Assert.Fail err
        | Ok actual ->
            Assert.IsTrue(actual.ContainsKey ApplicationIntegrationType.GUILD_INSTALL)
            Assert.AreEqual<string>(id, actual |> Map.find ApplicationIntegrationType.GUILD_INSTALL)

    [<TestMethod>]
    member _.``Get.extract - Extracts child records from single parent json payload`` () =
        // Arrange
        let data = """{ "A": true, "B": true }"""

        // Act
        let res = Decode.fromString Parent.decoder data

        // Assert
        match res with
        | Error err -> Assert.Fail err
        | Ok actual ->
            Assert.AreEqual<bool>(true, actual.A.A)
            Assert.AreEqual<bool>(true, actual.B.B)

    [<TestMethod>]
    member _.``Get.extractOpt - Doesn't fail when optional child record is missing`` () =
        // Arrange
        let data = """{ "A": true }"""

        // Act
        let res = Decode.fromString ParentOpt.decoder data

        // Assert
        match res with
        | Error err -> Assert.Fail err
        | Ok actual ->
            Assert.AreEqual<bool>(true, actual.A.A)
            Assert.AreEqual(None, actual.B)
    
    [<TestMethod>]
    member _.``Get.exists - Returns true if property is present`` () =
        // Arrange
        let data = """{ "value": null }"""

        // Act
        let res = Decode.fromString Exists.decoder data

        // Assert
        match res with
        | Ok actual -> Assert.AreEqual<bool>(true, actual.Value)
        | Error err -> Assert.Fail err
    
    [<TestMethod>]
    member _.``Get.exists - Returns false if property is not present`` () =
        // Arrange
        let data = "{}"

        // Act
        let res = Decode.fromString Exists.decoder data

        // Assert
        match res with
        | Ok actual -> Assert.AreEqual<bool>(false, actual.Value)
        | Error err -> Assert.Fail err
    
    [<TestMethod>]
    member _.``Encode.exists - Encodes given value if value is true`` () =
        // Arrange
        let exists = { Value = true }
        let expected = """{"value":null}"""

        // Act
        let actual = Encode.toString 0 (Exists.encoder exists)

        // Assert
        Assert.AreEqual<string>(expected, actual)
    
    [<TestMethod>]
    member _.``Encode.exists - Does not encode property if value is false`` () =
        // Arrange
        let exists = { Value = false }
        let expected = """{}"""

        // Act
        let actual = Encode.toString 0 (Exists.encoder exists)

        // Assert
        Assert.AreEqual<string>(expected, actual)
