namespace FSharp.Discord.Types

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-flags
[<RequireQualifiedAccess>]
type ActivityFlag =
    | INSTANCE =                    (1 <<< 0)
    | JOIN =                        (1 <<< 1)
    | SPECTATE =                    (1 <<< 2)
    | JOIN_REQUEST =                (1 <<< 3)
    | SYNC =                        (1 <<< 4)
    | PLAY =                        (1 <<< 5)
    | PARTY_PRIVACY_FRIENDS =       (1 <<< 6)
    | PARTY_PRIVACY_VOICE_CHANNEL = (1 <<< 7)
    | EMBEDDED =                    (1 <<< 8)

// https://discord.com/developers/docs/resources/application#application-object-application-flags
[<RequireQualifiedAccess>]
type ApplicationFlag =
    /// Indicates if the app uses the Auto Moderation API
    | APPLICATION_AUTO_MODERATION_RULE_CREATE_BADGE = (1 <<< 6)
    /// Intent required for bots in 100 or more servers to receive PRESENCE_UPDATE events
    | GATEWAY_PRESENCE                              = (1 <<< 12)
    /// Intent required for bots in under 100 servers to receive PRESENCE_UPDATE events, found on the Bot page in your app's settings
    | GATEWAY_PRESENCE_LIMITED                      = (1 <<< 13)
    /// Intent required for bots in 100 or more servers to receive member-related events like GUILD_MEMBER_ADD. See the list of member-related events under GUILD_MEMBERS
    | GATEWAY_GUILD_MEMBERS                         = (1 <<< 14)
    /// Intent required for bots in under 100 servers to receive member-related events like GUILD_MEMBER_ADD, found on the Bot page in your app's settings. See the list of member-related events under GUILD_MEMBERS
    | GATEWAY_GUILD_MEMBERS_LIMITED                 = (1 <<< 15)
    /// Indicates unusual growth of an app that prevents verification
    | VERIFICATION_PENDING_GUILD_LIMIT              = (1 <<< 16)
    /// Indicates if an app is embedded within the Discord client (currently unavailable publicly)
    | EMBEDDED                                      = (1 <<< 17)
    /// Intent required for bots in 100 or more servers to receive message content
    | GATEWAY_MESSAGE_CONTENT                       = (1 <<< 18)
    /// Intent required for bots in under 100 servers to receive message content, found on the Bot page in your app's settings
    | GATEWAY_MESSAGE_CONTENT_LIMITED               = (1 <<< 19)
    /// Indicates if an app has registered global application commands
    | APPLICATION_COMMAND_BADGE                     = (1 <<< 23)

// https://discord.com/developers/docs/resources/message#attachment-object-attachment-flags
[<RequireQualifiedAccess>]
type AttachmentFlag =
    /// This attachment has been edited using the remix feature on mobile
    | IS_REMIX = (1 <<< 2)

// https://discord.com/developers/docs/resources/channel#channel-object-channel-flags
[<RequireQualifiedAccess>]
type ChannelFlag =
    /// This thread is pinned to the top of its parent `GUILD_FORUM` or `GUILD_MEDIA` channel
    | PINNED =                      (1 <<< 1)
    /// Whether a tag is required to be specified when creating a thread in a `GUILD_FORUM` or a `GUILD_MEDIA`channel. Tags are specified in the `applied_tags` field
    | REQUIRE_TAG =                 (1 <<< 4)
    /// When set hides the embedded media download options. Available only for media channels
    | HIDE_MEDIA_DOWNLOAD_OPTIONS = (1 <<< 15)

// https://discord.com/developers/docs/resources/guild#guild-member-object-guild-member-flags
[<RequireQualifiedAccess>]
type GuildMemberFlag =
    /// Member has left and rejoined the guild
    | DID_REJOIN                      = (1 <<< 0)
    /// Member has completed onboarding
    | COMPLETED_ONBOARDING            = (1 <<< 1)
    /// Member is exempt from guild verification requirements
    | BYPASSES_VERIFICATION           = (1 <<< 2)
    /// Member has started onboarding
    | STARTED_ONBOARDING              = (1 <<< 3)
    /// Member is a guest and can only access the voice channel they were invited to
    | IS_GUEST                        = (1 <<< 4)
    /// Member has started Server Guide new member actions
    | STARTED_HOME_ACTIONS            = (1 <<< 5)
    /// Member has completed Server Guide new member actions
    | COMPLETED_HOME_ACTIONS          = (1 <<< 6)
    /// Member's username, display name, or nickname is blocked by AutoMod
    | AUTOMOD_QUARANTINED_USERNAME    = (1 <<< 7)
    /// Member has dismissed the DM settings upsell
    | DM_SETTINGS_UPSELL_ACKNOWLEDGED = (1 <<< 9)

module GuildMemberFlag =
    let isEditable (flag: GuildMemberFlag) =
        match flag with
        | GuildMemberFlag.DID_REJOIN                      -> false
        | GuildMemberFlag.COMPLETED_ONBOARDING            -> false
        | GuildMemberFlag.BYPASSES_VERIFICATION           -> true
        | GuildMemberFlag.STARTED_ONBOARDING              -> false
        | GuildMemberFlag.IS_GUEST                        -> false
        | GuildMemberFlag.STARTED_HOME_ACTIONS            -> false
        | GuildMemberFlag.COMPLETED_HOME_ACTIONS          -> false
        | GuildMemberFlag.AUTOMOD_QUARANTINED_USERNAME    -> false
        | GuildMemberFlag.DM_SETTINGS_UPSELL_ACKNOWLEDGED -> false
        | _ -> false

// https://discord.com/developers/docs/resources/lobby#lobby-member-object-lobby-member-flags
[<RequireQualifiedAccess>]
type LobbyMemberFlag =
    /// User can link a text channel to a lobby
    | CAN_LINK_LOBBY = (1 <<< 0)

// https://discord.com/developers/docs/resources/message#message-object-message-flags
[<RequireQualifiedAccess>]
type MessageFlag =
    /// This message has been published to subscribed channels (via Channel Following)
    | CROSSPOSTED =                            (1 <<< 0)
    /// This message originated from a message in another channel (via Channel Following)
    | IS_CROSSPOST =                           (1 <<< 1)
    /// Do not include any embeds when serializing this message
    | SUPPRESS_EMBEDS =                        (1 <<< 2)
    /// The source message for this crosspost has been deleted (via Channel Following)
    | SOURCE_MESSAGE_DELETED =                 (1 <<< 3)
    /// This message came from the urgent message system
    | URGENT =                                 (1 <<< 4)
    /// This message has an associated thread, with the same id as the message
    | HAS_THREAD =                             (1 <<< 5)
    /// This message is only visible to the user who invoked the Interaction
    | EPHEMERAL =                              (1 <<< 6)
    /// This message is an Interaction Response and the bot is "thinking"
    | LOADING =                                (1 <<< 7)
    /// This message failed to mention some roles and add their members to the thread
    | FAILED_TO_MENTION_SOME_ROLES_IN_THREAD = (1 <<< 8)
    /// This message will not trigger push and desktop notifications
    | SUPPRESS_NOTIFICATIONS =                 (1 <<< 12)
    /// This message is a voice message
    | IS_VOICE_MESSAGE =                       (1 <<< 13)
    /// This message has a snapshot (via Message Forwarding)
    | HAS_SNAPSHOT =                           (1 <<< 14)
    /// This message allows you to create fully component driven messages
    | IS_COMPONENTS_V2 =                       (1 <<< 15)

// https://discord.com/developers/docs/topics/permissions#role-object-role-flags
[<RequireQualifiedAccess>]
type RoleFlag =
    /// Role can be selected by members in an onboarding prompt
    | IN_PROMPT = (1 <<< 0)

// https://discord.com/developers/docs/resources/sku#sku-object-sku-flags
[<RequireQualifiedAccess>]
type SkuFlag =
    /// SKU is available for purchase
    | AVAILABLE          = (1 <<< 2)
    /// Recurring SKU that can be purchased by a user and applied to a single server. Grants access to every user in that server
    | GUILD_SUBSCRIPTION = (1 <<< 7)
    /// Recurring SKU purchased by a user for themselves. Grants access to the purchasing user in every server
    | USER_SUBSCRIPTION  = (1 <<< 8)

// https://discord.com/developers/docs/resources/guild#guild-object-system-channel-flags
[<RequireQualifiedAccess>]
type SystemChannelFlag =
    /// Suppress member join notifications
    | SUPPRESS_JOIN_NOTIFICATIONS                               = (1 <<< 0)
    /// Suppress server boost notifications
    | SUPPRESS_PREMIUM_SUBSCRIPTIONS                            = (1 <<< 1)
    /// Suppress server setup tips
    | SUPPRESS_GUILD_REMINDER_NOTIFICATIONS                     = (1 <<< 2)
    /// Hide member join sticker reply buttons
    | SUPPRESS_JOIN_NOTIFICATION_REPLIES                        = (1 <<< 3)
    /// Suppress role subscription purchase and renewal notifications
    | SUPPRESS_ROLE_SUBSCRIPTION_PURCHASE_NOTIFICATIONS         = (1 <<< 4)
    /// Hide role subscription sticker reply buttons
    | SUPPRESS_ROLE_SUBSCRIPTION_PURCHASE_NOTIFICATION_REPLIES  = (1 <<< 5)

// https://discord.com/developers/docs/resources/user#user-object-user-flags
[<RequireQualifiedAccess>]
type UserFlag =
    /// Discord employee
    | STAFF                    = (1 <<< 0)
    /// Partnered server owner
    | PARTNER                  = (1 <<< 1)
    /// HypeSquad events member
    | HYPERSQUAD               = (1 <<< 2)
    /// Bug hunter level 1
    | BUG_HUNTER_LEVEL_1       = (1 <<< 3)
    /// House bravery member
    | HYPESQUAD_ONLINE_HOUSE_1 = (1 <<< 6)
    /// House brilliance member
    | HYPESQUAD_ONLINE_HOUSE_2 = (1 <<< 7)
    /// House balance member
    | HYPESQUAD_ONLINE_HOUSE_3 = (1 <<< 8)
    /// Early nitro supporter
    | PREMIUM_EARLY_SUPPORTER  = (1 <<< 9)
    /// User is a team
    | TEAM_PSEUDO_USER         = (1 <<< 10)
    /// Bug hunter level 2
    | BUG_HUNTER_LEVEL_2       = (1 <<< 14)
    /// Verified bot
    | VERIFIED_BOT             = (1 <<< 16)
    /// Early verified bot developer
    | VERIFIED_DEVELOPER       = (1 <<< 17)
    /// Moderator programs alumni
    | CERTIFIED_MODERATOR      = (1 <<< 18)
    /// Bot uses only HTTP interactions and is shown in the online member list
    | BOT_HTTP_INTERACTIONS    = (1 <<< 19)
    /// User is an active developer
    | ACTIVE_DEVELOPER         = (1 <<< 22)
