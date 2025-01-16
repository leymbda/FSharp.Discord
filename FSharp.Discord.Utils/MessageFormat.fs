namespace rec FSharp.Discord.Utils

open System

type TimestampStyle =
    | SHORT_TIME
    | LONG_TIME
    | SHORT_DATE
    | LONG_DATE
    | SHORT_DATE_TIME
    | LONG_DATE_TIME
    | RELATIVE_TIME

module TimestampStyle =
    /// Default timestamp style, to use instead of format without style <t:TIMESTAMP>
    let zero () = TimestampStyle.SHORT_DATE_TIME

    let toString (style: TimestampStyle) =
        match style with
        | SHORT_TIME -> "t"
        | LONG_TIME -> "T"
        | SHORT_DATE -> "d"
        | LONG_DATE -> "D"
        | SHORT_DATE_TIME -> "f"
        | LONG_DATE_TIME -> "F"
        | RELATIVE_TIME -> "R"

type GuildNavigationType =
    | CUSTOMIZE
    | BROWSE
    | GUIDE
    | LINKED_ROLES
    | LINKED_ROLE of roleId: string

module GuildNavigationStyle =
    let toString (``type``: GuildNavigationType) =
        match ``type`` with
        | CUSTOMIZE -> "customize"
        | BROWSE -> "browse"
        | GUIDE -> "guide"
        | LINKED_ROLES -> "linked-roles"
        | LINKED_ROLE roleId -> $"linked-roles:{roleId}"

module MessageFormat =
    let user (userId: string) = $"<@{userId}>"

    let channel (channelId: string) = $"<#{channelId}>"

    let role (roleId: string) = $"<@&{roleId}>"

    /// Commands with subcommands and subcommand groups are separated by spaces in name. E.g.: "command subcommandgroup subcommand"
    let slashCommand (name: string) (commandId: string) = $"</{name}:{commandId}>"

    let customEmoji (name: string) (emojiId: string) = $"<:{name}:{emojiId}>"

    let customAnimatedEmoji (name: string) (emojiId: string) = $"<a:{name}:{emojiId}>"

    /// Default style can be used through `TimestampStyle.zero()`
    let timestamp (timestamp: DateTime) (style: TimestampStyle) = $"<t:{Math.Floor(timestamp.Subtract(DateTime.UnixEpoch).TotalMilliseconds)}:{style}>"

    let guildNavigation (guildId: string) (``type``: GuildNavigationType) = $"<{guildId}:{``type``}>"
