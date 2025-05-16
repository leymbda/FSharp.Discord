namespace FSharp.Discord.Gateway

open Elmish

[<RequireQualifiedAccess>]
type AsyncAttemptMsg<'Input> =
    | Attempt of 'Input
    | Failure of exn

module AsyncAttemptMsg =
    let inline toCmd (task: 'Args -> Async<'Success>) (args: 'Args) (msg: AsyncAttemptMsg<'Args> -> 'Msg) =
        Cmd.OfAsync.attempt task args (AsyncAttemptMsg.Failure >> msg)

[<RequireQualifiedAccess>]
type AsyncEitherMsg<'Input, 'Success> =
    | Either of 'Input
    | Success of 'Success
    | Failure of exn

module AsyncEitherMsg =
    let inline toCmd (task: 'Args -> Async<'Success>) (args: 'Args) (msg: AsyncEitherMsg<'Args, 'Success> -> 'Msg) =
        Cmd.OfAsync.either task args (AsyncEitherMsg.Success >> msg) (AsyncEitherMsg.Failure >> msg)
           
[<RequireQualifiedAccess>]
type AsyncPerformMsg<'Input, 'Success> =
    | Perform of 'Input
    | Success of 'Success

module AsyncPerformMsg =
    let inline toCmd (task: 'Args -> Async<'Success>) (args: 'Args) (msg: AsyncPerformMsg<'Args, 'Success> -> 'Msg) =
        Cmd.OfAsync.perform task args (AsyncPerformMsg.Success >> msg)
 