namespace FSharp.Discord.Rest

open System.Collections.Generic
open System.Net.Http
open System.Net.Http.Headers
open System.Text.Json

type Payload = (string * obj) seq

module StringContent =
    let fromObjectAsJson (o: obj) =
        new StringContent(Json.serializeF o, MediaTypeHeaderValue("application/json"))

module HttpContent =
    let fromObjectAsJson (o: obj) =
        StringContent.fromObjectAsJson o :> HttpContent

module Payload =
    let toJsonContent (p: Payload) =
        new StringContent(Json.serializeF p, MediaTypeHeaderValue("application/json"))

    let toFormContent (p: Payload) =
        new FormUrlEncodedContent(p |> Seq.map (fun (k, v) -> KeyValuePair(k, v.ToString())))

type File = {
    Content: string
    Type: string
    Name: string
}

module File =
    let toContent (file: File) =
        new StringContent(file.Content, MediaTypeHeaderValue(file.Type))

[<AutoOpen>]
module PayloadBuilder =
    type PayloadBuilder () =
        member _.Yield (_: unit) = Seq.empty<string * obj>
        member _.Yield (p: Payload) = p

        [<CustomOperation>]
        member _.required (p: Payload, key: string, value: 'a) =
            Seq.append [key, (value :> obj)] p

        [<CustomOperation>]
        member _.optional (p: Payload, key: string, value: 'a option) =
            match value with
            | Some v -> Seq.append [key, (v :> obj)] p
            | None -> p

    let payload = PayloadBuilder ()
    
[<AutoOpen>]
module MultipartBuilder =
    type MultipartBuilder () =
        let mutable fileCount = 0

        member _.Yield (_: unit) = new MultipartFormDataContent()
        member _.Yield (m: MultipartFormDataContent) = m

        [<CustomOperation>]
        member _.json (m: MultipartFormDataContent, name: string, value: Payload) =
            m.Add(Payload.toJsonContent value, name)
            m

        [<CustomOperation>]
        member _.json (m: MultipartFormDataContent, name: string, value: obj) =
            m.Add(StringContent.fromObjectAsJson value, name)
            m

        [<CustomOperation>]
        member _.file (m: MultipartFormDataContent, name: string, file: File) =
            m.Add(File.toContent file, name, file.Name)
            fileCount <- fileCount + 1
            m

        [<CustomOperation>]
        member this.file (m: MultipartFormDataContent, file: File) =
            this.file (m, $"files[{fileCount}]", file)

        [<CustomOperation>]
        member this.files (m: MultipartFormDataContent, files: File list) =
            List.fold (fun acc file -> this.file (acc, file)) m files

        [<CustomOperation>]
        member this.text (m: MultipartFormDataContent, name: string, content: string) =
            m.Add(new StringContent(content, MediaTypeHeaderValue("plain/text")), name)
            m

    let multipart = MultipartBuilder ()

type IPayload =
    abstract Content: HttpContent
