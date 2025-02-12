module FSharp.Discord.Webhook.Modules.Ed25519

open System
open System.Text
open TweetNaclSharp

type KeyPair = {
    PublicKey: string
    PrivateKey: string
}

type SignedMessage = {
    Body: string
    Timestamp: string
    Signature: string
}

/// Generate a new ed25519 key pair.
let generate () =
    let keypair = Nacl.SignKeyPair()

    {
        PublicKey = keypair.PublicKey |> Convert.ToHexString
        PrivateKey = keypair.SecretKey |> Convert.ToHexString
    }

/// Sign a message using the ed25519 private key.
let sign (timestamp: string) (body: string) (privateKey: string) =
    let msg = timestamp + body |> Encoding.UTF8.GetBytes
    let secretKey = privateKey |> Convert.FromHexString

    let signature = Nacl.SignDetached(msg, secretKey) |> Convert.ToHexString

    {
        Body = body
        Timestamp = timestamp
        Signature = signature
    }

/// Verify a message is authentic by comparing with the ed25519 public key.
let verify (timestamp: string) (body: string) (signature: string) (publicKey: string) =
    let msg = timestamp + body |> Encoding.UTF8.GetBytes
    let ``sig`` = signature |> Convert.FromHexString
    let key = publicKey |> Convert.FromHexString

    Nacl.SignDetachedVerify(msg, ``sig``, key)
