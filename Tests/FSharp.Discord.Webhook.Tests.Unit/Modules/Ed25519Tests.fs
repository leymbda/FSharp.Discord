namespace FSharp.Discord.Webhook

open Microsoft.VisualStudio.TestTools.UnitTesting
open System

[<TestClass>]
type Ed25519Tests () =
    [<TestMethod>]
    member _.``TESTUTIL - Generate valid ed25519 datarows`` () =
        let body = Guid.NewGuid().ToString()
        let timestamp = DateTime.UtcNow.ToString()

        let keypair = Ed25519.generate()
        let msg = Ed25519.sign timestamp body keypair.PrivateKey

        Console.WriteLine ("Body:        " + msg.Body)
        Console.WriteLine ("Timestamp:   " + msg.Timestamp)
        Console.WriteLine ("Signature:   " + msg.Signature)
        Console.WriteLine ("Public key:  " + keypair.PublicKey)
        Console.WriteLine ("Private key: " + keypair.PrivateKey)

    [<TestMethod>]
    member _.``generate - Generates valid ed25519 key pair`` () =
        // Arrange
        let timestamp = "timestamp"
        let body = "body"
        
        // Act
        let keypair = Ed25519.generate()

        // Assert
        let msg = Ed25519.sign timestamp body keypair.PrivateKey
        let matches = Ed25519.verify timestamp body msg.Signature keypair.PublicKey
        Assert.IsTrue matches

    [<TestMethod>]
    [<DataRow(
        "6/02/2025 1:44:32 AM",
        "66c1d789-969a-46e3-9951-8acbc906bba1",
        "5822A3FF7F64659DAE638040768BD001424EF5A984431BA945B7283A160E29919DA88238805A5673C69910592963D67E54C90B71460090235F6CB12898937A09",
        "1A5BC93C24A801F5E24BC4B97EF8C0EA608444E171AC5E6B0DE881CF98A81320AF32C8CC277AA735B56C81F4C87562A77F47E6A5CD94EE9F06B369DFF357AC09"
    )>]
    member _.``sign - Correctly signs messages`` (
        timestamp: string,
        body: string,
        expectedSignature: string,
        privateKey: string
    ) =
        // Arrange
        
        // Act
        let msg = Ed25519.sign timestamp body privateKey

        // Assert
        Assert.AreEqual<string>(expectedSignature, msg.Signature)

    [<TestMethod>]
    [<DataRow(
        "14/06/2024 3:33:11 PM",
        "e4e3e46e-99b1-4cc5-8fcb-a43ea68afaa6",
        "8228317DD172FCB211E6532D2CC34B7158D0877D768EAEA6A1205CEBEFF32CAE114455830E2885E1CAB7D54BF2ED387FD4F4D73E8AFAF186D2141F8778582901",
        "1DA73B9FFB48F89FB07D0226FB147A95C7DD412D985548822D3127416D505C06"
    )>]
    member _.``verify - Invalid parameters fails`` (
        timestamp: string,
        body: string,
        signature: string,
        publicKey: string
    ) =
        // Arrange

        // Act
        let res = Ed25519.verify timestamp body signature publicKey

        // Assert
        Assert.IsFalse(res)

    [<TestMethod>]
    [<DataRow(
        "14/06/2024 3:33:11 PM",
        "e4e3e46e-99b1-4cc5-8fcb-a43ea68afaa6",
        "9228317DD172FCB211E6532D2CC34B7158D0877D768EAEA6A1205CEBEFF32CAE114455830E2885E1CAB7D54BF2ED387FD4F4D73E8AFAF186D2141F8778582901",
        "1DA73B9FFB48F89FB07D0226FB147A95C7DD412D985548822D3127416D505C06"
    )>]
    member _.``verify - Valid parameters succeeds`` (
        timestamp: string,
        body: string,
        signature: string,
        publicKey: string
    ) =
        // Arrange
        
        // Act
        let res = Ed25519.verify timestamp body signature publicKey

        // Assert
        Assert.IsTrue(res)
