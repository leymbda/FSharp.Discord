namespace FSharp.Discord.Types

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CodesTests () =
    [<TestMethod>]
    [<DataRow 1005>]
    [<DataRow 4000>]
    [<DataRow 4001>]
    [<DataRow 4002>]
    [<DataRow 4003>]
    [<DataRow 4005>]
    [<DataRow 4007>]
    [<DataRow 4008>]
    [<DataRow 4009>]
    member _.GatewayCloseEventCode_shouldReconnect_ReturnsTrueForValidCodes (intCode: int) =
        // Arrange
        let code = intCode |> enum<GatewayCloseEventCode>

        // Act
        let shouldReconnect = GatewayCloseEventCode.shouldReconnect code

        // Assert
        Assert.IsTrue shouldReconnect

    [<TestMethod>]
    [<DataRow 1000>]
    [<DataRow 1001>]
    [<DataRow 4004>]
    [<DataRow 4010>]
    [<DataRow 4011>]
    [<DataRow 4012>]
    [<DataRow 4013>]
    [<DataRow 4014>]
    member _.GatewayCloseEventCode_shouldReconnect_ReturnsFalseForInvalidCodes (intCode: int) =
        // Arrange
        let code = intCode |> enum<GatewayCloseEventCode>

        // Act
        let shouldReconnect = GatewayCloseEventCode.shouldReconnect code

        // Assert
        Assert.IsFalse shouldReconnect
