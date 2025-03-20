namespace FSharp.Discord.Commands

type Localized<'T> = {
    Invariant: 'T
    Localizations: Map<string, 'T>
}
