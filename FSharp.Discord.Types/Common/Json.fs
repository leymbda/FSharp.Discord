namespace System.Text.Json

open System
open System.Text.Json.Nodes
open System.Text.Json.Serialization

module Json =
    let serializeF (value: 'a) =
        JsonSerializer.Serialize(value)

    let serialize (value: 'a) =
        try serializeF value |> Some
        with | _ -> None

    let deserializeF<'a> (json: string) =
        JsonSerializer.Deserialize<'a>(json)

    let deserialize<'a> (json: string) =
        try deserializeF json |> Some
        with | _ -> None

    let merge (json1: string) (json2: string) =
        let obj1 = JsonNode.Parse(json1).AsObject()
        let obj2 = JsonNode.Parse(json2).AsObject()

        let res = obj1.DeepClone()

        for prop in obj2 do
            res[prop.Key] <- prop.Value.DeepClone()

        res.ToJsonString()

    // TODO: Remove this module if STJ works as expected

module JsonException =
    let raise message =
        raise (JsonException message)

    let raiseThunk message =
        fun () -> raise message

    let raiseIf (condition: bool) =
        if condition then raise "An error occurred while processing JSON data"

module JsonConverter =
    type UnixEpoch () =
        inherit JsonConverter<DateTime> () with
            override _.Read (reader, _, _) =
                DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime

            override _.Write (writer, value, _) =
                DateTimeOffset(value).ToUnixTimeMilliseconds() |> writer.WriteNumberValue

    type NullUndefinedAsBool() =
        inherit JsonConverter<bool> () with
            override _.Read (reader, _, _) =
                match reader.TokenType with
                | JsonTokenType.Null -> true
                | JsonTokenType.None -> false
                | _ -> JsonException.raise "Unexpected token received in NullUndefinedAsBoolConverter"

            override _.Write (writer, value, options) =
                raise (NotImplementedException "Serializing property using null/undefined as bool convert is not possible")
