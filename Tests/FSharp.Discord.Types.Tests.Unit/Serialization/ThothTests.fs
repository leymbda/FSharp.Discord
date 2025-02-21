namespace FSharp.Discord.Types.Serialization

open FSharp.Discord.Types
open Microsoft.VisualStudio.TestTools.UnitTesting
open Thoth.Json.Net

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
        