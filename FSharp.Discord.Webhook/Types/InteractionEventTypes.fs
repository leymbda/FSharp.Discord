namespace FSharp.Discord.Webhook.Types

open FSharp.Discord.Types
open System.Text.Json

type PongResponseEvent = unit

type ChannelMessageWithSourceResponseEvent = MessageInteractionResponse

type DeferredChannelMessageWithSourceResponseEvent = unit

type DeferredUpdateMessageResponseEvent = unit

type UpdateMessageResponseEvent = MessageInteractionResponse

type ApplicationCommandAutocompleteResponseEvent = AutocompleteInteractionResponse

type ModalResponseEvent = ModalInteractionResponse

type LaunchActivityResponseEvent = unit
