namespace FSharp.Discord.Rest.V2

open FSharp.Discord.Types
open FSharp.Discord.Types.Serialization
open Thoth.Json.Net

type CreateInteractionResponsePayload(response, ?files) =
    member val Response: InteractionResponse = response
    member val Files: Media list = defaultArg files []

    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles InteractionResponse.encoder this.Response this.Files

type CreateInteractionResponseRequest(interactionId, interactionToken, payload) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken

    member val Payload: CreateInteractionResponsePayload = payload

type GetOriginalInteractionResponseRequest(interactionId, interactionToken) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken
    
    // TODO: Does this expect the query params from the equivalent webhook endpoint?

type EditOriginalInteractionResponsePayload(?content, ?embeds, ?allowedMentions, ?components, ?attachments, ?poll, ?files) =
    member val Content: string option option = content
    member val Embeds: Embed list option option = embeds
    member val AllowedMentions: AllowedMentions option option = allowedMentions
    member val Components: Component list option option = components
    member val Attachments: Attachment list option option = attachments
    member val Poll: Poll option option = poll
    member val Files: Media list = defaultArg files []

    static member Encoder(v: EditOriginalInteractionResponsePayload) =
        Encode.object ([]
            |> Encode.optinull "content" Encode.string v.Content
            |> Encode.optinull "embeds" (List.map Embed.encoder >> Encode.list) v.Embeds
            |> Encode.optinull "allowed_mentions" AllowedMentions.encoder v.AllowedMentions
            |> Encode.optinull "components" (List.map Component.encoder >> Encode.list) v.Components
            |> Encode.optinull "attachments" (List.map Attachment.encoder >> Encode.list) v.Attachments
            |> Encode.optinull "poll" Poll.encoder v.Poll
        )
    
    interface IPayload with
        member this.ToHttpContent() =
            HttpContent.createJsonWithFiles EditOriginalInteractionResponsePayload.Encoder this this.Files
            
    // TODO: Does this expect the query params from the equivalent webhook endpoint?
    // TODO: Functions the same as edit webhook message - should it just use the same payload type too?

type EditOriginalInteractionResponseRequest(interactionId, interactionToken, payload) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken

    member val Payload: EditOriginalInteractionResponsePayload = payload

type DeleteOriginalInteractionResponseRequest(interactionId, interactionToken) =
    member val InteractionId: string = interactionId
    member val InteractionToken: string = interactionToken
