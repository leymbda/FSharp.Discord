namespace FSharp.Discord.Webhook.Types

open FSharp.Discord.Types
open System.Text.Json

type PongResponseEvent = Empty

type ChannelMessageWithSourceResponseEvent = MessageInteractionResponse

type DeferredChannelMessageWithSourceResponseEvent = Empty

type DeferredUpdateMessageResponseEvent = Empty

type UpdateMessageResponseEvent = MessageInteractionResponse

type ApplicationCommandAutocompleteResponseEvent = AutocompleteInteractionResponse

type ModalResponseEvent = ModalInteractionResponse

type LaunchActivityResponseEvent = Empty
