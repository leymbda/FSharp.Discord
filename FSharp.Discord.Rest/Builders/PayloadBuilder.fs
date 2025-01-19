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
        member _.file (m: MultipartFormDataContent, file: File) =
            m.Add(File.toContent file, $"files[{fileCount}", file.Name)
            fileCount <- fileCount + 1
            m

        [<CustomOperation>]
        member this.files (m: MultipartFormDataContent, files: File list) =
            List.fold (fun acc file -> this.file (acc, file)) m files

    let multipart = MultipartBuilder ()

type IPayload =
    abstract Content: HttpContent

// TODO: Replace usage of old code below then delete it

[<AutoOpen>]
module PayloadBuilderOld =
    type IPayloadBuilder =
        abstract member ToContent: unit -> HttpContent
    
    type JsonPayloadBuilder() =
        member val Properties: IDictionary<string, obj> = Dictionary()

        member this.Yield(_) =
            this

        [<CustomOperation>]
        member this.required (_, name: string, value: 'a) =
            this.Properties.Add(name, value)
            this

        [<CustomOperation>]
        member this.optional (_, name: string, value: 'a option) =
            if value.IsSome then
                this.Properties.Add(name, value)
            this

        interface IPayloadBuilder with
            member this.ToContent () =
                new StringContent(Json.serializeF this.Properties, MediaTypeHeaderValue("application/json"))

    let json = JsonPayloadBuilder()

    type UrlEncodedPayloadBuilder() =
        member val Properties: IDictionary<string, obj> = Dictionary()

        member this.Yield(_) =
            this

        [<CustomOperation>]
        member this.required (_, name: string, value: 'a) =
            this.Properties.Add(name, value)
            this

        [<CustomOperation>]
        member this.optional (_, name: string, value: 'a option) =
            if value.IsSome then
                this.Properties.Add(name, value)
            this

        interface IPayloadBuilder with
            member this.ToContent () =
                new FormUrlEncodedContent(
                    this.Properties
                    |> Seq.map (|KeyValue|)
                    |> Seq.map (fun (k, v) -> KeyValuePair(k, v.ToString()))
                )

    let urlencoded = UrlEncodedPayloadBuilder()

    type JsonPayload<'a>(payload: 'a) =
        interface IPayloadBuilder with
            member _.ToContent () =
                new StringContent(Json.serializeF payload, MediaTypeHeaderValue("application/json"))

    type JsonListPayload<'a>(list: 'a list) =
        interface IPayloadBuilder with
            member _.ToContent () =
                new StringContent(Json.serializeF list, MediaTypeHeaderValue("application/json"))

    type StringPayload(str: string) =
        interface IPayloadBuilder with
            member _.ToContent () =
                new StringContent(str, MediaTypeHeaderValue("plain/text"))

    type FilePayload(fileContent: string, mimeType: string) =
            member _.ToContent () =
                new StringContent(fileContent, MediaTypeHeaderValue(mimeType))

    type MultipartPayloadBuilder() =
        let mutable fileCount = 0

        member val Form = new MultipartFormDataContent()

        member this.Yield(_) =
            this

        [<CustomOperation>]
        member this.part (_, name: string, content: IPayloadBuilder) =
            this.Form.Add(content.ToContent(), name)
            this
            
        [<CustomOperation>]
        member this.file (_, fileName: string, fileContent: IPayloadBuilder) =
            this.Form.Add(fileContent.ToContent(), $"files[{fileCount}]", fileName)
            fileCount <- fileCount + 1
            this
            
        [<CustomOperation>]
        member this.files (_, files: IDictionary<string, IPayloadBuilder>) =
            for fileName, fileContent in Seq.map (|KeyValue|) files do
                this.Form.Add(fileContent.ToContent(), $"files[{fileCount}]", fileName)
                fileCount <- fileCount + 1
            this

        interface IPayloadBuilder with
            member this.ToContent () =
                this.Form

    let multipartOld = MultipartPayloadBuilder()

    [<AbstractClass>]
    [<System.Obsolete>]
    type Payload() =
        abstract member Content: IPayloadBuilder

    type JsonPayloadImpl<'a> (content: 'a) =
        inherit Payload()

        override _.Content = JsonPayload content

    let fromObj<'a> (value: 'a) =
        JsonPayloadImpl value :> Payload
