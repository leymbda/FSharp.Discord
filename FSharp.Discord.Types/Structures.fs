namespace rec FSharp.Discord.Types

open System

// ----- Interactions: Receiving and Responding -----

type InteractionAuthor =
    | User of User
    | GuildMember of GuildMember

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-interaction-structure
type Interaction = {
    Id: string
    ApplicationId: string
    Type: InteractionType
    Data: InteractionData option
    Guild: PartialGuild option
    GuildId: string option
    Channel: PartialChannel option
    ChannelId: string option
    Author: InteractionAuthor
    Token: string
    Version: int
    Message: Message option
    AppPermissions: string
    Locale: string option
    GuildLocale: string option
    Entitlements: Entitlement list
    AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, string>
    Context: InteractionContextType option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-application-command-data-structure
type ApplicationCommandData = {
    Id: string
    Name: string
    Type: ApplicationCommandType
    Resolved: ResolvedData option
    Options: ApplicationCommandInteractionDataOption list option
    GuildId: string option
    TargetId: string option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-message-component-data-structure
type MessageComponentData = {
    CustomId: string
    ComponentType: ComponentType
    Values: SelectMenuOption list option
    Resolved: ResolvedData option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-modal-submit-data-structure
type ModalSubmitData = {
    CustomId: string
    Components: Component list
}

type InteractionData =
    | APPLICATION_COMMAND of ApplicationCommandData
    | MESSAGE_COMPONENT   of MessageComponentData
    | MODAL_SUBMIT        of ModalSubmitData

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-resolved-data-structure
type ResolvedData = {
    Users: Map<string, User> option
    Members: Map<string, PartialGuildMember> option // Missing user, deaf, mute
    Roles: Map<string, Role> option
    Channels: Map<string, PartialChannel> option // Only have id, name, type, permissions (and threads have thread_metadata, parent_id)
    Messages: Map<string, PartialMessage> option
    Attachments: Map<string, Attachment> option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-application-command-interaction-data-option-structure
type ApplicationCommandInteractionDataOption = {
    Name: string
    Type: ApplicationCommandOptionType
    Value: ApplicationCommandInteractionDataOptionValue option
    Options: ApplicationCommandInteractionDataOption list option
    Focused: bool option
}

[<RequireQualifiedAccess>]
type ApplicationCommandInteractionDataOptionValue =
    | STRING of string
    | INT    of int
    | DOUBLE of double
    | BOOL   of bool

// https://discord.com/developers/docs/interactions/receiving-and-responding#message-interaction-object-message-interaction-structure
type MessageInteraction = {
    Id: string
    Type: InteractionType
    Name: string
    User: User
    Member: PartialGuildMember option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-interaction-response-structure
type InteractionResponse = {
    Type: InteractionCallbackType
    Data: InteractionCallbackData option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-messages
type MessageInteractionCallbackData = {
    Tts: bool option
    Content: string option
    Embeds: Embed list option
    AllowedMentions: AllowedMentions option
    Flags: MessageFlag list option
    Components: Component list option
    Attachments: PartialAttachment list option
    Poll: Poll option // TODO: Poll REQUEST object, not poll itself
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-autocomplete
type AutocompleteInteractionCallbackData = {
    Choices: ApplicationCommandOptionChoice list
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-modal
type ModalInteractionCallbackData = {
    CustomId: string
    Title: string
    Components: Component list
}

type InteractionCallbackData =
    | MESSAGE      of MessageInteractionCallbackData
    | AUTOCOMPLETE of AutocompleteInteractionCallbackData
    | MODAL        of ModalInteractionCallbackData

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-callback-interaction-callback-response-object
type InteractionCallbackResponse = {
    Interaction: InteractionCallback
    Resource: InteractionCallbackResource option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-callback-interaction-callback-object
type InteractionCallback = {
    Id: string
    Type: InteractionType
    ActivityInstanceId: string option
    ResponseMessageId: string option
    ResponseMessageLoading: bool option
    ResponseMessageEphemeral: bool option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-callback-interaction-callback-resource-object
type InteractionCallbackResource = {
    Type: InteractionCallbackType
    ActivityInstance: InteractionCallbackActivityInstance option
    Message: Message option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-callback-interaction-callback-activity-instance-resource
type InteractionCallbackActivityInstance = {
    Id: string
}

// TODO: Refactor how Structures/* and Events types are implemented and put the structures here (basing off above)

// ----- Interactions: Application Commands -----

// https://discord.com/developers/docs/interactions/application-commands#application-command-object-application-command-structure
type ApplicationCommand = {
    Id: string
    Type: ApplicationCommandType
    ApplicationId: string
    GuildId: string option
    Name: string // TODO: Enforce 1-32 character name with valid chars
    NameLocalizations: Map<string, string> option option
    LocalizedName: string option
    Description: string
    DescriptionLocalizations: Map<string, string> option option
    LocalizedDescription: string option
    Options: ApplicationCommandOption list option
    DefaultMemberPermissions: string option // TODO: Serialize bitfield into permission list
    Nsfw: bool
    IntegrationTypes: ApplicationIntegrationType list
    Contexts: InteractionContextType list option
    Version: string
    Handler: ApplicationCommandHandlerType option
}

// TODO: Should localized name and description be handled separately?
// TODO: Create DU for different application command types? A few values only present on one

// https://discord.com/developers/docs/interactions/application-commands#application-command-object-application-command-option-structure
type ApplicationCommandOption = {
    Type: ApplicationCommandOptionType
    Name: string // TODO: Enforce 1-32 character name with valid chars
    NameLocalizations: Map<string, string> option option
    LocalizedName: string option
    Description: string
    DescriptionLocalizations: Map<string, string> option option
    LocalizedDescription: string option
    Required: bool option
    Choices: ApplicationCommandOptionChoice list option
    Options: ApplicationCommandOption list option
    ChannelTypes: ChannelType list option
    MinValue: ApplicationCommandOptionMinValue option
    MaxValue: ApplicationCommandOptionMaxValue option
    MinLength: int option
    MaxLength: int option
    Autocomplete: bool option
}

// TODO: Should localized name and description be handled separately?
// TODO: Create DU for different types of application command option

[<RequireQualifiedAccess>]
type ApplicationCommandOptionMinValue =
    | INT    of int
    | DOUBLE of double

// TODO: Ensure min 1, max 6000 (create single DU with this requirement)

[<RequireQualifiedAccess>]
type ApplicationCommandOptionMaxValue =
    | INT    of int
    | DOUBLE of double

// TODO: Ensure min 1, max 6000 (create single DU with this requirement)

// https://discord.com/developers/docs/interactions/application-commands#application-command-object-application-command-option-choice-structure
type ApplicationCommandOptionChoice = {
    Name: string
    NameLocalizations: Map<string, string> option option
    Value: ApplicationCommandOptionChoiceValue
}

[<RequireQualifiedAccess>]
type ApplicationCommandOptionChoiceValue =
    | STRING of string
    | INT    of int
    | DOUBLE of double

// TODO: Handle value type based on parent application command option type

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-guild-application-command-permissions-structure
type GuildApplicationCommandPermissions = {
    Id: string
    ApplicationId: string
    GuildId: string
    Permissions: ApplicationCommandPermission list
}

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-application-command-permissions-structure
type ApplicationCommandPermission = {
    Id: string
    Type: ApplicationCommandPermissionType
    Permission: bool
}

module ApplicationCommandPermissionConstants =
    /// Sets an application command permission on all members in a guild
    let everyone (guildId: string) =
        guildId

    /// Sets an application command permission on all channels
    let allChannels (guildId: string) =
        (int64 guildId) - 1L |> string // TODO: Return option instead of throwing exception

    // TODO: Should this module be moved into Utils or somewhere else?

// TODO: Handle maximum subcommand depth (is this even possible?)
// TODO: Implement locales and locale fallbacks

// ----- Interactions: Message Components -----

// https://discord.com/developers/docs/interactions/message-components#action-rows
type ActionRow = {
    Type: ComponentType
    Components: Component list
}

// https://discord.com/developers/docs/interactions/message-components#button-object-button-structure
type Button = {
    Type: ComponentType
    Style: ButtonStyle
    Label: string option
    Emoji: PartialEmoji option
    CustomId: string option
    SkuId: string option
    Url: string option
    Disabled: bool
}

// TODO: Ensure values meet requirements set in docs for buttons

// https://discord.com/developers/docs/interactions/message-components#select-menu-object-select-menu-structure
type SelectMenu = {
    Type: ComponentType
    CustomId: string
    Options: SelectMenuOption list option
    ChannelTypes: ChannelType list option
    Placeholder: string option
    DefaultValues: SelectMenuDefaultValue list option
    MinValues: int
    MaxValues: int
    Disabled: bool
}

// TODO: DU for different types of select menus
// TODO: Ensure values meet requirements set in docs for select menus

// https://discord.com/developers/docs/interactions/message-components#select-menu-object-select-option-structure
type SelectMenuOption = {
    Label: string
    Value: string
    Description: string option
    Emoji: PartialEmoji option
    Default: bool option
}

// TODO: Ensure values meet requirements set in docs for select menu options

// https://discord.com/developers/docs/interactions/message-components#select-menu-object-select-default-value-structure
type SelectMenuDefaultValue = {
    Id: string
    Type: SelectMenuDefaultValueType
}

// https://discord.com/developers/docs/interactions/message-components#text-input-object-text-input-structure
type TextInput = {
    Type: ComponentType
    CustomId: string
    Style: TextInputStyle
    Label: string
    MinLength: int option
    MaxLength: int option
    Required: bool
    Value: string option
    Placeholder: string option
}

// TODO: Ensure values meet requirements set in docs for text input

type Component =
    | ACTION_ROW of ActionRow
    | BUTTON of Button
    | SELECT_MENU of SelectMenu
    | TEXT_INPUT of TextInput

// ----- Events: Using Gateway -----

// https://discord.com/developers/docs/events/gateway#session-start-limit-object-session-start-limit-structure
type SessionStartLimit = {
    Total: int
    Remaining: int
    ResetAfter: int
    MaxConcurrency: int
}

// ----- Events: Gateway Events -----

// https://discord.com/developers/docs/topics/gateway-events#payload-structure
type GatewayEventPayload<'a> = {
    Opcode: GatewayOpcode
    Data: 'a
    Sequence: int option
    EventName: string option
}

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-structure
type IdentifySendEvent = {
    Token: string
    Properties: IdentifyConnectionProperties
    Compress: bool option
    LargeThreshold: int option
    Shard: (int * int) option
    Presence: UpdatePresenceSendEvent option
    Intents: int
}

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-connection-properties
type IdentifyConnectionProperties = {
    OperatingSystem: string
    Browser: string
    Device: string
}

// TODO: Make single DU for ensuring the device matches expected format (?)

// https://discord.com/developers/docs/topics/gateway-events#resume-resume-structure
type ResumeSendEvent = {
    Token: string
    SessionId: string
    Sequence: int
}

// https://discord.com/developers/docs/topics/gateway-events#heartbeat-example-heartbeat
type HeartbeatSendEvent = int option

// https://discord.com/developers/docs/topics/gateway-events#request-guild-members-request-guild-members-structure
type RequestGuildMembersSendEvent = {
    GuildId: string
    Query: string option
    Limit: int
    Presences: bool option
    UserIds: string list option
    Nonce: string option
}

// https://discord.com/developers/docs/events/gateway-events#request-soundboard-sounds
type RequestSoundboardSoundsSendEvent = {
    GuildIds: string list 
}

// https://discord.com/developers/docs/topics/gateway-events#update-voice-state-gateway-voice-state-update-structure
type UpdateVoiceStateSendEvent = {
    GuildId: string
    ChannelId: string option
    SelfMute: bool
    SelfDeaf: bool
}

// https://discord.com/developers/docs/topics/gateway-events#update-presence-gateway-presence-update-structure
type UpdatePresenceSendEvent = {
    Since: DateTime option
    Activities: Activity list
    Status: Status
    Afk: bool
}

type PartialUpdatePresenceSendEvent = {
    Since: DateTime option option
    Activities: Activity list option
    Status: Status option
    Afk: bool option
}

// https://discord.com/developers/docs/events/gateway#connection-lifecycle
type HeartbeatReceiveEvent = unit

// https://discord.com/developers/docs/events/gateway#connection-lifecycle
type HeartbeatAckReceiveEvent = unit

// https://discord.com/developers/docs/topics/gateway#hello-event-example-hello-event
type HelloReceiveEvent = {
    HeartbeatInterval: int
}

// https://discord.com/developers/docs/topics/gateway-events#ready-ready-event-fields
type ReadyReceiveEvent = {
    Version: int
    User: User
    Guilds: UnavailableGuild list
    SessionId: string
    ResumeGatewayUrl: string
    Shard: (int * int) option
    Application: PartialApplication
}

// https://discord.com/developers/docs/events/gateway-events#resumed
type ResumedReceiveEvent = unit

// https://discord.com/developers/docs/events/gateway-events#reconnect
type ReconnectReceiveEvent = unit

// https://discord.com/developers/docs/topics/gateway-events#invalid-session
type InvalidSessionReceiveEvent = bool

// https://discord.com/developers/docs/events/gateway-events#application-command-permissions-update
type ApplicationCommandPermissionsUpdateReceiveEvent = ApplicationCommandPermission

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-rule-create
type AutoModerationRuleCreateReceiveEvent = AutoModerationRule

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-rule-update
type AutoModerationRuleUpdateReceiveEvent = AutoModerationRule

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-rule-delete
type AutoModerationRuleDeleteReceiveEvent = AutoModerationRule

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-action-execution-auto-moderation-action-execution-event-fields
type AutoModerationActionExecutionReceiveEvent = {
    GuildId: string
    Action: AutoModerationAction
    RuleId: string
    RuleTriggerType: AutoModerationTriggerType
    UserId: string
    ChannelId: string option
    MessageId: string option
    AlertSystemMessageId: string option
    Content: string option
    MatchedKeyword: string option
    MatchedContent: string option option
}

// https://discord.com/developers/docs/events/gateway-events#channel-create
type ChannelCreateReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#channel-update
type ChannelUpdateReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#channel-delete
type ChannelDeleteReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#thread-create
type ThreadCreateReceiveEvent = {
    Channel: Channel
    NewlyCreated: bool option
    ThreadMember: ThreadMember option
}

// https://discord.com/developers/docs/events/gateway-events#thread-update
type ThreadUpdateReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#thread-delete
type ThreadDeleteReceiveEvent = {
    Id: string
    GuildId: string option
    ParentId: string option option
    Type: ChannelType
}

// https://discord.com/developers/docs/events/gateway-events#thread-list-sync-thread-list-sync-event-fields
type ThreadListSyncReceiveEvent = {
    GuildId: string
    ChannelIds: string list option
    Threads: Channel list
    Members: ThreadMember list
}

// https://discord.com/developers/docs/events/gateway-events#thread-member-update
type ThreadMemberUpdateReceiveEvent = {
    ThreadMember: ThreadMember
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#thread-members-update
type ThreadMembersUpdateReceiveEvent = {
    Id: string
    GuildId: string
    MemberCount: int
    AddedMembers: ThreadMember list option
    RemovedMemberIds: string list option
}

// https://discord.com/developers/docs/events/gateway-events#channel-pins-update-channel-pins-update-event-fields
type ChannelPinsUpdateReceiveEvent = {
    GuildId: string option
    ChannelId: string
    LastPinTimestamp: DateTime option option
}

// https://discord.com/developers/docs/events/gateway-events#entitlement-create
type EntitlementCreateReceiveEvent = Entitlement

// https://discord.com/developers/docs/events/gateway-events#entitlement-update
type EntitlementUpdateReceiveEvent = Entitlement

// https://discord.com/developers/docs/events/gateway-events#entitlement-delete
type EntitlementDeleteReceiveEvent = Entitlement

// https://discord.com/developers/docs/events/gateway-events#guild-create
type GuildCreateReceiveEvent =
    | AvailableGuild of GuildCreateReceiveEventAvailableGuild
    | UnavailableGuild of UnavailableGuild

type GuildCreateReceiveEventAvailableGuild = {
    Guild: Guild
    JoinedAt: DateTime
    Large: bool
    Unavailable: bool
    MemberCount: int
    VoiceStates: PartialVoiceState list
    Members: GuildMember list
    Channels: Channel list
    Threads: Channel list
    Presences: PartialUpdatePresenceSendEvent list
    StageInstances: StageInstance list
    GuildScheduledEvents: GuildScheduledEvent list
    SoundboardSounds: SoundboardSound list
}

// https://discord.com/developers/docs/events/gateway-events#guild-update
type GuildUpdateReceiveEvent = Guild

// https://discord.com/developers/docs/events/gateway-events#guild-delete
type GuildDeleteReceiveEvent = {
    GuildId: string
    Unavailable: bool option
}

// https://discord.com/developers/docs/events/gateway-events#guild-audit-log-entry-create
type GuildAuditLogEntryCreateReceiveEvent = {
    AuditLogEntry: AuditLogEntry
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-ban-add
type GuildBanAddReceiveEvent = {
    User: User
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-ban-remove
type GuildBanRemoveReceiveEvent = {
    User: User
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-emojis-update
type GuildEmojisUpdateReceiveEvent = {
    GuildId: string
    Emojis: Emoji list
}

// https://discord.com/developers/docs/events/gateway-events#guild-stickers-update
type GuildStickersUpdateReceiveEvent = {
    GuildId: string
    Stickers: Sticker list
}

// https://discord.com/developers/docs/events/gateway-events#guild-integrations-update
type GuildIntegrationsUpdateReceiveEvent = {
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-member-add
type GuildMemberAddReceiveEvent = {
    GuildMember: GuildMember
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-member-remove
type GuildMemberRemoveReceiveEvent = {
    User: User
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-member-update
type GuildMemberUpdateReceiveEvent = {
    GuildId: string
    Roles: string list
    User: User
    Nick: string option option
    Avatar: string option
    Banner: string option
    JoinedAt: DateTime option
    PremiumSince: DateTime option option
    Deaf: bool option
    Mute: bool option
    Pending: bool option
    CommunicationDisabledUntil: DateTime option option
    Flags: GuildMemberFlag list option
    AvatarDecorationData: AvatarDecorationData option option
}

// https://discord.com/developers/docs/events/gateway-events#guild-members-chunk
type GuildMembersChunkReceiveEvent = {
    GuildId: string
    Members: GuildMember list
    ChunkIndex: int
    ChunkCount: int
    NotFound: string list option
    Presences: UpdatePresenceSendEvent list option
    Nonce: string option
}

// https://discord.com/developers/docs/events/gateway-events#guild-role-create
type GuildRoleCreateReceiveEvent = {
    GuildId: string
    Role: Role
}

// https://discord.com/developers/docs/events/gateway-events#guild-role-update
type GuildRoleUpdateReceiveEvent = {
    GuildId: string
    Role: Role
}

// https://discord.com/developers/docs/events/gateway-events#guild-role-delete
type GuildRoleDeleteReceiveEvent = {
    GuildId: string
    RoleId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-create
type GuildScheduledEventCreateReceiveEvent = GuildScheduledEvent

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-update
type GuildScheduledEventUpdateReceiveEvent = GuildScheduledEvent

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-delete
type GuildScheduledEventDeleteReceiveEvent = GuildScheduledEvent

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-user-add
type GuildScheduledEventUserAddReceiveEvent = {
    GuildScheduledEventId: string
    UserId: string
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-user-remove
type GuildScheduledEventUserRemoveReceiveEvent = {
    GuildScheduledEventId: string
    UserId: string
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sound-create
type GuildSoundboardSoundCreateReceiveEvent = SoundboardSound

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sound-update
type GuildSoundboardSoundUpdateReceiveEvent = SoundboardSound

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sound-delete
type GuildSoundboardSoundDeleteReceiveEvent = {
    SoundId: string
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sounds-update
type GuildSoundboardSoundsUpdateReceiveEvent = {
    SoundboardSounds: SoundboardSound list
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#soundboard-sounds
type GuildSoundboardSoundsReceiveEvent = {
    SoundboardSounds: SoundboardSound list
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#integration-create
type IntegrationCreateReceiveEvent = {
    Integration: Integration
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#integration-update
type IntegrationUpdateReceiveEvent = {
    Integration: Integration
    GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#integration-delete
type IntegrationDeleteReceiveEvent = {
    Id: string
    GuildId: string
    ApplicationId: string
}

// https://discord.com/developers/docs/events/gateway-events#invite-create
type InviteCreateReceiveEvent = {
    ChannelId: string
    Code: string
    CreatedAt: DateTime
    GuildId: string option
    Inviter: User option
    MaxAge: int
    MaxUses: int
    TargetType: InviteTargetType option
    TargetUser: User option
    TargetApplication: PartialApplication option
    Temporary: bool
    Uses: int
}

// https://discord.com/developers/docs/events/gateway-events#invite-delete
type InviteDeleteReceiveEvent = {
    ChannelId: string
    GuildId: string option
    Code: string
}

// https://discord.com/developers/docs/events/gateway-events#message-create
type MessageCreateReceiveEvent = {
    Message: Message
    GuildId: string option
    Member: PartialGuildMember option
    Mentions: MessageCreateReceiveEventMention list
}

type MessageCreateReceiveEventMention = {
    User: User
    Member: PartialGuildMember option
}

// https://discord.com/developers/docs/events/gateway-events#message-update
type MessageUpdateReceiveEvent = {
    Message: Message
    GuildId: string option
    Member: PartialGuildMember option
    Mentions: MessageUpdateReceiveEventMention list
}

type MessageUpdateReceiveEventMention = {
    User: User
    Member: PartialGuildMember option
}

// https://discord.com/developers/docs/events/gateway-events#message-delete
type MessageDeleteReceiveEvent = {
    Id: string
    ChannelId: string
    GuildId: string option
}

// https://discord.com/developers/docs/events/gateway-events#message-delete-bulk
type MessageDeleteBulkReceiveEvent = {
    Ids: string list
    ChannelId: string
    GuildId: string option
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-add
type MessageReactionAddReceiveEvent = {
    UserId: string
    ChannelId: string
    MessageId: string
    GuildId: string option
    Member: GuildMember option
    Emoji: PartialEmoji
    MessageAuthorId: string option
    Burst: bool
    BurstColors: string list option // TODO: Convert into some type representing color (hex string send e.g. `#123456`)
    Type: ReactionType
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-remove
type MessageReactionRemoveReceiveEvent = {
    UserId: string
    ChannelId: string
    MessageId: string
    GuildId: string option
    Emoji: PartialEmoji
    Burst: bool
    Type: ReactionType
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-remove-all
type MessageReactionRemoveAllReceiveEvent = {
    ChannelId: string
    MessageId: string
    GuildId: string option
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-remove-emoji
type MessageReactionRemoveEmojiReceiveEvent = {
    ChannelId: string
    GuildId: string option
    MessageId: string
    Emoji: PartialEmoji
}

// https://discord.com/developers/docs/events/gateway-events#presence-update
type PresenceUpdateReceiveEvent = {
    User: PartialUser option
    GuildId: string option
    Status: Status option // TODO: Cannot be invisible
    Activities: Activity list option
    ClientStatus: ClientStatus option
}

// TODO: Unclear requirements from docs for above, need to check in on these. Especially around "types of fields are NOT validated"

// https://discord.com/developers/docs/events/gateway-events#client-status-object
type ClientStatus = {
    Desktop: ClientDeviceStatus option
    Mobile: ClientDeviceStatus option
    Web: ClientDeviceStatus option
}

// TODO: Should this just define the invis/offline in the type instead of as option (which discord does)

// https://discord.com/developers/docs/topics/gateway-events#activity-object-activity-structure
type Activity = {
    Name: string
    Type: ActivityType
    Url: string option option
    CreatedAt: DateTime
    Timestamps: ActivityTimestamps option
    ApplicationId: string option
    Details: string option option
    State: string option option
    Emoji: ActivityEmoji option option
    Party: ActivityParty option
    Assets: ActivityAssets option
    Secrets: ActivitySecrets option
    Instance: bool option
    Flags: ActivityFlag list option
    Buttons: ActivityButton list option
}

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-timestamps
type ActivityTimestamps = {
    Start: DateTime option
    End: DateTime option
}

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-emoji
type ActivityEmoji = {
    Name: string
    Id: string option
    Animated: bool option
}

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-party
type ActivityParty = {
    Id: string option
    Size: ActivityPartySize option
}

type ActivityPartySize = {
    Current: int
    Maximum: int
}

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-assets
type ActivityAssets = {
    LargeImage: string option
    LargeText: string option
    SmallImage: string option
    SmallText: string option
}

// TODO: Handle activity asset images as documented

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-secrets
type ActivitySecrets = {
    Join: string option
    Spectate: string option
    Match: string option
}

// https://discord.com/developers/docs/events/gateway-events#activity-object-activity-buttons
type ActivityButton = {
    Label: string
    Url: string
}

// https://discord.com/developers/docs/topics/gateway-events#typing-start-typing-start-event-fields
type TypingStartReceiveEvent = {
    ChannelId: string
    GuildId: string option
    UserId: string
    Timestamp: DateTime
    Member: GuildMember option
}

// https://discord.com/developers/docs/events/gateway-events#user-update
type UserUpdateReceiveEvent = User

// https://discord.com/developers/docs/events/gateway-events#voice-channel-effect-send
type VoiceChannelEffectSendReceiveEvent = {
    ChannelId: string
    GuildId: string
    UserId: string
    Emoji: Emoji option option
    AnimationType: AnimationType option option
    AnimationId: string option
    SoundId: string option // TODO: This can be a snowflake OR integer, need to test if an int will cast to string for this (if so, message nonce should be changed)
    SoundVolume: double option
}

// https://discord.com/developers/docs/events/gateway-events#voice-state-update
type VoiceStateUpdateReceiveEvent = VoiceState

// https://discord.com/developers/docs/events/gateway-events#voice-server-update
type VoiceServerUpdateReceiveEvent = {
    Token: string
    GuildId: string
    Endpoint: string option
}

// https://discord.com/developers/docs/events/gateway-events#webhooks-update
type WebhooksUpdateReceiveEvent = {
    GuildId: string
    ChannelId: string
}

// https://discord.com/developers/docs/events/gateway-events#interaction-create
type InteractionCreateReceiveEvent = Interaction

// https://discord.com/developers/docs/events/gateway-events#stage-instance-create
type StageInstanceCreateReceiveEvent = StageInstance

// https://discord.com/developers/docs/events/gateway-events#stage-instance-update
type StageInstanceUpdateReceiveEvent = StageInstance

// https://discord.com/developers/docs/events/gateway-events#stage-instance-delete
type StageInstanceDeleteReceiveEvent = StageInstance

// https://discord.com/developers/docs/events/gateway-events#subscription-create
type SubscriptionCreateReceiveEvent = Subscription

// https://discord.com/developers/docs/events/gateway-events#subscription-update
type SubscriptionUpdateReceiveEvent = Subscription

// https://discord.com/developers/docs/events/gateway-events#subscription-delete
type SubscriptionDeleteReceiveEvent = Subscription

// https://discord.com/developers/docs/events/gateway-events#message-poll-vote-add
type MessagePollVoteAddReceiveEvent = {
    UserId: string
    ChannelId: string
    MessageId: string
    GuildId: string option
    AnswerId: int
}

// https://discord.com/developers/docs/events/gateway-events#message-poll-vote-remove
type MessagePollVoteRemoveReceiveEvent = {
    UserId: string
    ChannelId: string
    MessageId: string
    GuildId: string option
    AnswerId: int
}

// ----- Events: Webhook Events -----

// https://discord.com/developers/docs/events/webhook-events#event-body-object
type WebhookEventPayload<'a> = {
    Version: int
    ApplicationId: string
    Type: WebhookPayloadType
    Event: WebhookEventBody<'a> option
}

// https://discord.com/developers/docs/events/webhook-events#payload-structure
type WebhookEventBody<'a> = {
    Type: WebhookEventType
    Timestamp: DateTime
    Data: 'a option
}

// https://discord.com/developers/docs/events/webhook-events#application-authorized-application-authorized-structure
type ApplicationAuthorizedEvent = {
    IntegrationType: ApplicationIntegrationType option
    User: User
    Scopes: OAuthScope list
    Guild: Guild option
}

// https://discord.com/developers/docs/events/webhook-events#entitlement-create-entitlement-create-structure
type EntitlementCreateEvent = Entitlement

// ----- Resources: Application -----

// https://discord.com/developers/docs/resources/application#application-object-application-structure
type Application = {
    Id: string
    Name: string
    Icon: string option
    Description: string
    RpcOrigins: string list option
    BotPublic: bool
    BotRequireCodeGrant: bool
    Bot: PartialUser option
    TermsOfServiceUrl: string option
    PrivacyPolicyUrl: string option
    Owner: PartialUser option
    VerifyKey: string
    Team: Team option
    GuildId: string option
    Guild: PartialGuild option
    PrimarySkuId: string option
    Slug: string option
    CoverImage: string option
    Flags: ApplicationFlag list option
    ApproximateGuildCount: int option
    ApproximateUserInstallCount: int option
    RedirectUris: string list option
    InteractionsEndpointUrl: string option option
    RoleConnectionsVerificationUrl: string option option
    EventWebhooksUrl: string option option
    EventWebhooksStatus: WebhookEventStatus
    EventWebhooksTypes: WebhookEventType list option
    Tags: string list option
    InstallParams: InstallParams option
    IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option
    CustomInstallUrl: string option
}

and PartialApplication = {
    Id: string
    Name: string option
    Icon: string option option
    Description: string option
    RpcOrigins: string list option
    BotPublic: bool option
    BotRequireCodeGrant: bool option
    Bot: PartialUser option
    TermsOfServiceUrl: string option
    PrivacyPolicyUrl: string option
    Owner: PartialUser option
    VerifyKey: string option
    Team: Team option option
    GuildId: string option
    Guild: PartialGuild option
    PrimarySkuId: string option
    Slug: string option
    CoverImage: string option
    Flags: ApplicationFlag list option
    ApproximateGuildCount: int option
    ApproximateUserInstallCount: int option
    RedirectUris: string list option
    InteractionsEndpointUrl: string option option
    RoleConnectionsVerificationUrl: string option option
    EventWebhooksUrl: string option option
    EventWebhooksStatus: WebhookEventStatus option
    EventWebhooksTypes: WebhookEventType list option
    Tags: string list option
    InstallParams: InstallParams option
    IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option
    CustomInstallUrl: string option
}

// https://discord.com/developers/docs/resources/application#application-object-application-integration-type-configuration-object
type ApplicationIntegrationTypeConfiguration = {
    OAuth2InstallParams: InstallParams option
}

// https://discord.com/developers/docs/resources/application#install-params-object-install-params-structure
type InstallParams = {
    Scopes: OAuthScope list
    Permissions: string // TODO: Serialize bitfield into permission list
}

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-instance-object
type ActivityInstance = {
    ApplicationId: string
    InstanceId: string
    LaunchId: string
    Location: ActivityLocation
    Users: string list
}

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-location-object
type ActivityLocation = {
    Id: string
    Kind: ActivityLocationKind
    ChannelId: string
    GuildId: string option option
}

// TODO: Should above optional + nullable properties be stored as `option option`? What alternative would there be?

// ----- Resources: Application Role Connection Metadata -----

// https://discord.com/developers/docs/resources/application-role-connection-metadata#application-role-connection-metadata-object-application-role-connection-metadata-structure
type ApplicationRoleConnectionMetadata = {
    Type: ApplicationRoleConnectionMetadataType
    Key: string
    Name: string
    NameLocalizations: Map<string, string> option
    Description: string
    DescriptionLocalizations: Map<string, string> option
}

// ----- Resources: Audit Log -----

// https://discord.com/developers/docs/resources/audit-log#audit-log-object-audit-log-structure
type AuditLog = {
    ApplicationCommands: ApplicationCommand list
    AuditLogEntries: AuditLogEntry list
    AutoModerationRules: AutoModerationRule list
    GuildScheduledEvents: GuildScheduledEvent list
    Integrations: PartialIntegration list
    Threads: Channel list
    Users: User list
    Webhooks: Webhook list
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-audit-log-entry-structure
type AuditLogEntry = {
    TargetId: string option
    Changes: AuditLogChange list option
    UserId: string option
    Id: string
    ActionType: AuditLogEventType
    Options: AuditLogEntryOptionalInfo option
    Reason: string option
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-optional-audit-entry-info
type AuditLogEntryOptionalInfo = {
    ApplicationId: string option
    AutoModerationRuleName: string option
    AutoModerationRuleTriggerType: string option
    ChannelId: string option
    Count: string option
    DeleteMemberDays: string option
    Id: string option
    MembersRemoved: string option
    MessageId: string option
    RoleName: string option
    Type: string option
    IntegrationType: string option
}

// TODO: Create DU for above for specific event types

// https://discord.com/developers/docs/resources/audit-log#audit-log-change-object
type AuditLogChange = {
    NewValue: obj option
    OldValue: obj option
    Key: string
}

// TODO: Handle exceptions for keys as documented, and consider if obj is most appropriate for old and new value (DU for all json types?)

// ----- Resources: Auto Moderation -----

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-auto-moderation-rule-structure
type AutoModerationRule = {
    Id: string
    GuildId: string
    Name: string
    CreatorId: string
    EventType: AutoModerationEventType
    TriggerType: AutoModerationTriggerType
    TriggerMetadata: AutoModerationTriggerMetadata
    Actions: AutoModerationAction list
    Enabled: bool
    ExemptRoles: string list
    ExemptChannels: string list
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-trigger-metadata
type AutoModerationTriggerMetadata = {
    KeywordFilter: string list option
    RegexPatterns: string list option
    Presets: AutoModerationKeywordPreset list option
    AllowList: string list option
    MentionTotalLimit: int option
    MentionRaidProtectionEnabled: bool option
}

// TODO: DU for trigger metadata types
// TODO: Handle trigger metadata field limits?

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object
type AutoModerationAction = {
    Type: AutoModerationActionType
    Metadata: AutoModerationActionMetadata option
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object-action-metadata
type AutoModerationActionMetadata = {
    ChannelId: string option
    DurationSeconds: int option
    CustomMessage: string option
}

// ----- Resources: Channel -----

// https://discord.com/developers/docs/resources/channel#channel-object-channel-structure
type Channel = {
    Id: string
    Type: ChannelType
    GuildId: string option
    Position: int option
    PermissionOverwrites: PermissionOverwrite list option
    Name: string option option
    Topic: string option option
    Nsfw: bool option
    LastMessageId: string option option
    Bitrate: int option
    UserLimit: int option
    RateLimitPerUser: int option
    Recipients: User list option
    Icon: string option option
    OwnerId: string option
    ApplicationId: string option
    Managed: bool option
    ParentId: string option option
    LastPinTimestamp: DateTime option option
    RtcRegion: string option option
    VideoQualityMode: VideoQualityMode option
    MessageCount: int option
    MemberCount: int option
    ThreadMetadata: ThreadMetadata option
    Member: ThreadMember option // only in certain endpoints (likely should be an ExtraField)
    DefaultAutoArchiveDuration: AutoArchiveDuration option
    Permissions: string option // TODO: Convert bitfield into permission list
    Flags: ChannelFlag list option
    TotalMessagesSent: int option
    AvailableTags: ForumTag list option
    AppliedTags: string list option
    DefaultReactionEmoji: DefaultReaction option option
    DefaultThreadRateLimitPerUser: int option
    DefaultSortOrder: ChannelSortOrder option option
    DefaultForumLayout: ForumLayout option
}

and PartialChannel = {
    Id: string
    Type: ChannelType option
    GuildId: string option
    Position: int option
    PermissionOverwrites: PermissionOverwrite list option
    Name: string option option
    Topic: string option option
    Nsfw: bool option
    LastMessageId: string option option
    Bitrate: int option
    UserLimit: int option
    RateLimitPerUser: int option
    Recipients: User list option
    Icon: string option option
    OwnerId: string option
    ApplicationId: string option
    Managed: bool option
    ParentId: string option option
    LastPinTimestamp: DateTime option option
    RtcRegion: string option option
    VideoQualityMode: VideoQualityMode option
    MessageCount: int option
    MemberCount: int option
    ThreadMetadata: ThreadMetadata option
    Member: ThreadMember option // only in certain endpoints (likely should be an ExtraField)
    DefaultAutoArchiveDuration: AutoArchiveDuration option
    Permissions: string option // TODO: Convert bitfield into permission list
    Flags: ChannelFlag list option
    TotalMessagesSent: int option
    AvailableTags: ForumTag list option
    AppliedTags: string list option
    DefaultReactionEmoji: DefaultReaction option option
    DefaultThreadRateLimitPerUser: int option
    DefaultSortOrder: ChannelSortOrder option option
    DefaultForumLayout: ForumLayout option
}

//type GuildTextChannel = BaseChannel
//type DmChannel = BaseChannel
//type GuildVoiceChannel = BaseChannel
//type GroupDmChannel = BaseChannel
//type GuildCategoryChannel = BaseChannel
//type GuildAnnouncementChannel = BaseChannel
//type AnnouncementThreadChannel = BaseChannel
//type PublicThreadChannel = BaseChannel
//type PrivateThreadChannel = BaseChannel
//type GuildStageVoiceChannel = BaseChannel
//type GuildDirectoryChannel = BaseChannel
//type GuildForumChannel = BaseChannel
//type GuildMediaChannel = BaseChannel

//type Channel =
//    | GUILD_TEXT          of GuildTextChannel
//    | DM                  of DmChannel
//    | GUILD_VOICE         of GuildVoiceChannel
//    | GROUP_DM            of GroupDmChannel
//    | GUILD_CATEGORY      of GuildCategoryChannel
//    | GUILD_ANNOUNCEMENT  of GuildAnnouncementChannel
//    | ANNOUNCEMENT_THREAD of AnnouncementThreadChannel
//    | PUBLIC_THREAD       of PublicThreadChannel
//    | PRIVATE_THREAD      of PrivateThreadChannel
//    | GUILD_STAGE_VOICE   of GuildStageVoiceChannel
//    | GUILD_DIRECTORY     of GuildDirectoryChannel
//    | GUILD_FORUM         of GuildForumChannel
//    | GUILD_MEDIA         of GuildMediaChannel

// https://discord.com/developers/docs/resources/channel#followed-channel-object-followed-channel-structure
type FollowedChannel = {
    ChannelId: string
    WebhookId: string
}

// https://discord.com/developers/docs/resources/channel#overwrite-object-overwrite-structure
type PermissionOverwrite = {
    Id: string
    Type: PermissionOverwriteType
    Allow: string
    Deny: string
}

and PartialPermissionOverwrite = {
    Id: string
    Type: PermissionOverwriteType option
    Allow: string option
    Deny: string option
}

// https://discord.com/developers/docs/resources/channel#thread-metadata-object-thread-metadata-structure
type ThreadMetadata = {
    Archived: bool
    AutoArchiveDuration: AutoArchiveDuration
    ArchiveTimestamp: DateTime
    Locked: bool
    Invitable: bool option
    CreateTimestamp: DateTime option option
}

// https://discord.com/developers/docs/resources/channel#thread-member-object-thread-member-structure
type ThreadMember = {
    Id: string option
    UserId: string option
    JoinTimestamp: DateTime
    Flags: int // TODO: What kind of values are here? "any user-thread setting, currently only used for notifications"
    Member: GuildMember option
}

// https://discord.com/developers/docs/resources/channel#default-reaction-object-default-reaction-structure
type DefaultReaction = {
    EmojiId: string option
    EmojiName: string option
}

// https://discord.com/developers/docs/resources/channel#forum-tag-object-forum-tag-structure
type ForumTag = {
    Id: string
    Name: string
    Moderated: bool
    EmojiId: string option
    EmojiName: string option
}

// TODO: "When updating a GUILD_FORUM or a GUILD_MEDIA channel, tag objects in available_tags only require the name field."

// ----- Resources: Emoji -----

// https://discord.com/developers/docs/resources/emoji#emoji-object-emoji-structure
type Emoji = {
    Id: string option
    Name: string option
    Roles: string list option
    User: User option
    RequireColons: bool option
    Managed: bool option
    Animated: bool option
    Available: bool option
}

and PartialEmoji = Emoji // All emoji properties are already optional

// ----- Resources: Entitlement -----

// https://discord.com/developers/docs/resources/entitlement#entitlement-object-entitlement-structure
type Entitlement = {
    Id: string
    SkuId: string
    ApplicationId: string
    UserId: string option
    Type: EntitlementType
    Deleted: bool
    StartsAt: DateTime option
    EndsAt: DateTime option
    GuildId: string option
    Consumed: bool option
}

// ----- Resources: Guild -----

// https://discord.com/developers/docs/resources/guild#guild-object-guild-structure
type Guild = {
    Id: string
    Name: string
    Icon: string option
    IconHash: string option option
    Splash: string option
    DiscoverySplash: string option
    Owner: bool option
    OwnerId: string
    Permissions: string option
    AfkChannelId: string option
    AfkTimeout: int
    WidgetEnabled: bool option
    WidgetChannelId: string option option
    VerificationLevel: VerificationLevel
    DefaultMessageNotifications: MessageNotificationLevel
    ExplicitContentFilter: ExplicitContentFilterLevel
    Roles: Role list
    Emojis: Emoji list
    Features: GuildFeature list
    MfaLevel: MfaLevel
    ApplicationId: string option
    SystemChannelId: string option
    SystemChannelFlags: SystemChannelFlag list
    RulesChannelId: string option
    MaxPresences: int option option
    MaxMembers: int option
    VanityUrlCode: string option
    Description: string option
    Banner: string option
    PremiumTier: GuildPremiumTier
    PremiumSubscriptionCount: int option
    PreferredLocale: string
    PublicUpdatesChannelId: string option
    MaxVideoChannelUsers: int option
    MaxStageVideoChannelUsers: int option
    ApproximateMemberCount: int option
    ApproximatePresenceCount: int option
    WelcomeScreen: WelcomeScreen option
    NsfwLevel: NsfwLevel
    Stickers: Sticker list option
    PremiumProgressBarEnabled: bool
    SafetyAlertsChannelId: string option
    IncidentsData: IncidentsData option
}

and PartialGuild = {
    Id: string
    Name: string option
    Icon: string option option
    IconHash: string option option
    Splash: string option option
    DiscoverySplash: string option option
    Owner: bool option
    OwnerId: string option
    Permissions: string option
    AfkChannelId: string option option
    AfkTimeout: int option
    WidgetEnabled: bool option
    WidgetChannelId: string option option
    VerificationLevel: VerificationLevel option
    DefaultMessageNotifications: MessageNotificationLevel option
    ExplicitContentFilter: ExplicitContentFilterLevel option
    Roles: Role list option
    Emojis: Emoji list option
    Features: GuildFeature list option
    MfaLevel: MfaLevel option
    ApplicationId: string option option
    SystemChannelId: string option option
    SystemChannelFlags: SystemChannelFlag list option
    RulesChannelId: string option option
    MaxPresences: int option option
    MaxMembers: int option
    VanityUrlCode: string option option
    Description: string option option
    Banner: string option option
    PremiumTier: GuildPremiumTier option
    PremiumSubscriptionCount: int option
    PreferredLocale: string option
    PublicUpdatesChannelId: string option option
    MaxVideoChannelUsers: int option
    MaxStageVideoChannelUsers: int option
    ApproximateMemberCount: int option
    ApproximatePresenceCount: int option
    WelcomeScreen: WelcomeScreen option
    NsfwLevel: NsfwLevel option
    Stickers: Sticker list option
    PremiumProgressBarEnabled: bool option
    SafetyAlertsChannelId: string option option
    IncidentsData: IncidentsData option option
}

// https://discord.com/developers/docs/resources/guild#unavailable-guild-object
type UnavailableGuild = {
    Id: string
    Unavailable: bool
}

// https://discord.com/developers/docs/resources/guild#guild-preview-object-guild-preview-structure
type GuildPreview = {
    Id: string
    Name: string
    Icon: string option
    Splash: string option
    DiscoverySplash: string option
    Emojis: Emoji list
    Features: GuildFeature list
    ApproximateMemberCount: int
    ApproximatePresenceCount: int
    Description: string option
    Stickers: Sticker list
}

// https://discord.com/developers/docs/resources/guild#guild-widget-settings-object-guild-widget-settings-structure
type GuildWidgetSettings = {
    Enabled: bool
    ChannelId: string option
}

// https://discord.com/developers/docs/resources/guild#guild-widget-object-guild-widget-structure
type GuildWidget = {
    Id: string
    Name: string
    InstantInvite: string option
    Channels: PartialChannel list
    Members: PartialUser list
    PresenceCount: int
}

// https://discord.com/developers/docs/resources/guild#guild-member-object-guild-member-structure
type GuildMember = {
    User: User option // TODO: Not included in MESSAGE_CREATE and MESSAGE_UPDATE (how do handle this?)
    Nick: string option option
    Avatar: string option option
    Banner: string option option
    Roles: string list
    JoinedAt: DateTime
    PremiumSince: DateTime option option
    Deaf: bool
    Mute: bool
    Flags: GuildMemberFlag list
    Pending: bool option // TODO: Only in GUILD_ events (should it be in ExtraFields?)
    Permissions: string option
    CommunicationDisabledUntil: DateTime option option
    AvatarDecorationData: AvatarDecorationData option option
}

and PartialGuildMember = {
    User: User option // TODO: Not included in MESSAGE_CREATE and MESSAGE_UPDATE (how do handle this?)
    Nick: string option option
    Avatar: string option option
    Banner: string option option
    Roles: string list option
    JoinedAt: DateTime option
    PremiumSince: DateTime option option
    Deaf: bool option
    Mute: bool option
    Flags: GuildMemberFlag list option
    Pending: bool option // TODO: Only in GUILD_ events (should it be in ExtraFields?)
    Permissions: string option
    CommunicationDisabledUntil: DateTime option option
    AvatarDecorationData: AvatarDecorationData option option
}

// https://discord.com/developers/docs/resources/guild#integration-object-integration-structure
type Integration = {
    Id: string
    Name: string
    Type: GuildIntegrationType
    Enabled: bool
    Syncing: bool option
    RoleId: string option
    EnableEmoticons: bool option
    ExpireBehavior: IntegrationExpireBehavior option
    ExpireGracePeriod: int option
    User: User option
    Account: IntegrationAccount
    SyncedAt: DateTime option
    SubscriberCount: int option
    Revoked: bool option
    Application: IntegrationApplication option
    Scopes: OAuthScope list option
}

and PartialIntegration = {
    Id: string
    Name: string option
    Type: GuildIntegrationType option
    Enabled: bool option
    Syncing: bool option
    RoleId: string option
    EnableEmoticons: bool option
    ExpireBehavior: IntegrationExpireBehavior option
    ExpireGracePeriod: int option
    User: User option
    Account: IntegrationAccount option
    SyncedAt: DateTime option
    SubscriberCount: int option
    Revoked: bool option
    Application: IntegrationApplication option
    Scopes: OAuthScope list option
}

// https://discord.com/developers/docs/resources/guild#integration-account-object-integration-account-structure
type IntegrationAccount = {
    Id: string
    Name: string
}

// https://discord.com/developers/docs/resources/guild#integration-application-object-integration-application-structure
type IntegrationApplication = {
    Id: string
    Name: string
    Icon: string option
    Description: string
    Bot: User option
}

// https://discord.com/developers/docs/resources/guild#ban-object-ban-structure
type Ban = {
    Reason: string option
    User: User
}

// https://discord.com/developers/docs/resources/guild#welcome-screen-object-welcome-screen-structure
type WelcomeScreen = {
    Description: string option
    WelcomeChannels: WelcomeScreenChannel list
}

// https://discord.com/developers/docs/resources/guild#welcome-screen-object-welcome-screen-channel-structure
type WelcomeScreenChannel = {
    ChannelId: string
    Description: string
    EmojiId: string option
    EmojiName: string option
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-guild-onboarding-structure
type GuildOnboarding = {
    GuildId: string
    Prompts: GuildOnboardingPrompt list
    DefaultChannelIds: string list
    Enabled: bool
    Mode: OnboardingMode
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-onboarding-prompt-structure
type GuildOnboardingPrompt = {
    Id: string
    Type: OnboardingPromptType
    Options: GuildOnboardingPromptOption list
    Title: string
    SingleSelect: bool
    Required: bool
    InOnboarding: bool
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-prompt-option-structure
type GuildOnboardingPromptOption = {
    Id: string
    ChannelIds: string list
    RoleIds: string list
    Emoji: Emoji option
    EmojiId: string option
    EmojiName: string option
    EmojiAnimated: bool option
    Title: string
    Description: string option
}

// https://discord.com/developers/docs/resources/guild#incidents-data-object-incidents-data-structure
type IncidentsData = {
    InvitesDisabledUntil: DateTime option
    DmsDisabledUntil: DateTime option
    DmSpamDetectedAt: DateTime option option
    RaidDetectedAt: DateTime option option
}

// ----- Resources: Guild Scheduled Event -----

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-structure
type GuildScheduledEvent = {
    Id: string
    GuildId: string
    ChannelId: string option
    CreatorId: string option option
    Name: string
    Description: string option option
    ScheduledStartTime: DateTime
    ScheduledEndTime: DateTime option
    PrivacyLevel: PrivacyLevel
    Status: EventStatus
    EntityType: ScheduledEntityType
    EntityId: string option
    EntityMetadata: EntityMetadata option
    Creator: User option
    UserCount: int option
    Image: string option option
    RecurrenceRule: RecurrenceRule option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-entity-metadata
type EntityMetadata = {
    Location: string option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-user-object-guild-scheduled-event-user-structure
type GuildScheduledEventUser = {
    GuildScheduledEventId: string
    User: User
    Member: GuildMember option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object
type RecurrenceRule = {
    Start: DateTime
    End: DateTime option
    Frequency: RecurrenceRuleFrequency
    Interval: int
    ByWeekday: RecurrenceRuleWeekday list option
    ByWeekend: RecurrenceRuleNWeekday list option
    ByMonth: RecurrenceRuleMonth list option
    ByMonthDay: int list option
    ByYearDay: int list option
    Count: int option
}

// TODO: Handle documented limitations?

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-nweekday-structure
type RecurrenceRuleNWeekday = {
    N: int
    Day: RecurrenceRuleWeekday
}

// ----- Resources: Guild Template -----

// https://discord.com/developers/docs/resources/guild-template#guild-template-object-guild-template-structure
type GuildTemplate = {
    Code: string
    Name: string
    Description: string option
    UsageCount: int
    CreatorId: string
    Creator: User
    CreatedAt: DateTime
    UpdatedAt: DateTime
    SourceGuildId: string
    SerializedSourceGuild: PartialGuild
    IsDirty: bool option
}

// ----- Resources: Invite -----

// https://discord.com/developers/docs/resources/invite#invite-object-invite-structure
type Invite = {
    Type: InviteType
    Code: string
    Guild: PartialGuild option
    Channel: PartialChannel option
    Inviter: PartialUser option
    TargetType: InviteTargetType option
    TargetUser: User option
    TargetApplication: PartialApplication option
    ApproximatePresenceCount: int option
    ApproximateMemberCount: int option
    ExpiresAt: DateTime option option
    GuildScheduledEvent: GuildScheduledEvent option
}

// TODO: ApproximatePresenceCount ApproximateMemberCount ExpiresAt all only returned in certain endpoints (should they be ExtraFields?)

// https://discord.com/developers/docs/resources/invite#invite-metadata-object-invite-metadata-structure
type InviteMetadata = {
    Uses: int
    MaxUses: int
    MaxAge: int
    Temporary: bool
    CreatedAt: DateTime
}

type InviteWithMetadata = {
    Invite: Invite
    Metadata: InviteMetadata
}

// ----- Resources: Lobby -----

// https://discord.com/developers/docs/resources/lobby#lobby-object-lobby-structure
type Lobby = {
    Id: string
    ApplicationId: string
    Metadata: Map<string, string> option
    Members: LobbyMember list
    LinkedChannel: Channel option
}

// https://discord.com/developers/docs/resources/lobby#lobby-member-object-lobby-member-structure
type LobbyMember = {
    Id: string
    Metadata: Map<string, string> option option
    Flags: LobbyMemberFlag list option
}

// ----- Resources: Message -----

// https://discord.com/developers/docs/resources/message#message-object-message-structure
type Message = {
    Id: string
    ChannelId: string
    Author: User
    Content: string option
    Timestamp: DateTime
    EditedTimestamp: DateTime option
    Tts: bool
    MentionEveryone: bool
    Mentions: User list
    MentionRoles: string list
    MentionChannels: ChannelMention list option
    Attachments: Attachment list
    Embeds: Embed list
    Reactions: Reaction list option
    Nonce: MessageNonce option
    Pinned: bool
    WebhookId: string option
    Type: MessageType
    Activity: MessageActivity option
    Application: PartialApplication option
    ApplicationId: string option
    Flags: MessageFlag list option
    MessageReference: MessageReference option
    MessageSnapshots: MessageSnapshot list option
    ReferencedMessage: Message option option
    InteractionMetadata: MessageInteractionMetadata option
    Interaction: MessageInteraction option
    Thread: Channel option
    Components: Component list option
    StickerItems: StickerItem list option
    Position: int option
    RoleSubscriptionData: RoleSubscriptionData option
    Resolved: ResolvedData option
    Poll: Poll option
    Call: MessageCall option
}

and PartialMessage = {
    Id: string
    ChannelId: string option
    Author: User option
    Content: string option option
    Timestamp: DateTime option
    EditedTimestamp: DateTime option option
    Tts: bool option
    MentionEveryone: bool option
    Mentions: User list option
    MentionRoles: string list option
    MentionChannels: ChannelMention list option
    Attachments: Attachment list option
    Embeds: Embed list option
    Reactions: Reaction list option
    Nonce: MessageNonce option
    Pinned: bool option
    WebhookId: string option
    Type: MessageType option
    Activity: MessageActivity option
    Application: PartialApplication option
    ApplicationId: string option
    Flags: MessageFlag list option
    MessageReference: MessageReference option
    MessageSnapshots: MessageSnapshot list option
    ReferencedMessage: Message option option
    InteractionMetadata: MessageInteractionMetadata option
    Interaction: MessageInteraction option
    Thread: Channel option
    Components: Component list option
    StickerItems: StickerItem list option
    Position: int option
    RoleSubscriptionData: RoleSubscriptionData option
    Resolved: ResolvedData option
    Poll: Poll option
    Call: MessageCall option
}

/// A partial message specifically for message snapshots
and SnapshotPartialMessage = {
    Content: string option
    Timestamp: DateTime
    EditedTimestamp: DateTime option
    Mentions: User list
    MentionRoles: string list
    Attachments: Attachment list
    Embeds: Embed list
    Type: MessageType
    Flags: MessageFlag list option
    Components: Component list option
    StickerItems: StickerItem list option
}

[<RequireQualifiedAccess>]
type MessageNonce =
    | INT of int
    | STRING of string

// TODO: Handle documented conditions?

// https://discord.com/developers/docs/resources/message#message-object-message-activity-structure
type MessageActivity = {
    Type: MessageActivityType
    PartyId: string option
}

// https://discord.com/developers/docs/resources/message#message-interaction-metadata-object-application-command-interaction-metadata-structure
type ApplicationCommandInteractionMetadata = {
    Id: string
    Type: InteractionType
    User: User
    AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, string>
    OriginalResponseMessageId: string option
    TargetUser: User option
    TargetMessageId: string option
}

// https://discord.com/developers/docs/resources/message#message-interaction-metadata-object-message-component-interaction-metadata-structure
type MessageComponentInteractionMetadata = {
    Id: string
    Type: InteractionType
    User: User
    AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, string>
    OriginalResponseMessageId: string option
    InteractedMessageId: string
}

// https://discord.com/developers/docs/resources/message#message-interaction-metadata-object-modal-submit-interaction-metadata-structure
type ModalSubmitInteractionMetadata = {
    Id: string
    Type: InteractionType
    User: User
    AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, string>
    OriginalResponseMessageId: string option
    TriggeringInteractionMetadata: MessageInteractionMetadata // TODO: Make this not allowed to be a ModalSubmitInteractionMetadata
}

type MessageInteractionMetadata =
    | APPLICATION_COMMAND of ApplicationCommandInteractionMetadata
    | MESSAGE_COMPONENT of MessageComponentInteractionMetadata
    | MODAL_SUBMIT of ModalSubmitInteractionMetadata

// https://discord.com/developers/docs/resources/message#message-call-object-message-call-object-structure
type MessageCall = {
    Participants: string list
    EndedTimestamp: DateTime option option
}

// https://discord.com/developers/docs/resources/message#message-reference-structure
type MessageReference = {
    Type: MessageReferenceType
    MessageId: string option
    ChannelId: string option
    GuildId: string option
    FailIfNotExists: bool option
}

// TODO: Handle documented conditions?

// https://discord.com/developers/docs/resources/message#message-snapshot-structure
type MessageSnapshot = {
    Message: SnapshotPartialMessage
}

// https://discord.com/developers/docs/resources/message#reaction-object-reaction-structure
type Reaction = {
    Count: int
    CountDetails: ReactionCountDetails
    Me: bool
    MeBurst: bool
    Emoji: PartialEmoji
    BurstColors: int list
}

// https://discord.com/developers/docs/resources/message#reaction-count-details-object-reaction-count-details-structure
type ReactionCountDetails = {
    Burst: int
    Normal: int
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-structure
type Embed = {
    Title: string option
    Type: EmbedType option
    Description: string option
    Url: string option
    Timestamp: DateTime option
    Color: int option
    Footer: EmbedFooter option
    Image: EmbedImage option
    Thumbnail: EmbedThumbnail option
    Video: EmbedVideo option
    Provider: EmbedProvider option
    Author: EmbedAuthor option
    Fields: EmbedField list option
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-thumbnail-structure
type EmbedThumbnail = {
    Url: string
    ProxyUrl: string option
    Height: int option
    Width: int option
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-video-structure
type EmbedVideo = {
    Url: string option
    ProxyUrl: string option
    Height: int option
    Width: int option
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-image-structure
type EmbedImage = {
    Url: string
    ProxyUrl: string option
    Height: int option
    Width: int option
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-provider-structure
type EmbedProvider = {
    Name: string option
    Url: string option
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-author-structure
type EmbedAuthor = {
    Name: string
    Url: string option
    IconUrl: string option
    ProxyIconUrl: string option
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-footer-structure
type EmbedFooter = {
    Text: string
    IconUrl: string option
    ProxyIconUrl: string option
}

// https://discord.com/developers/docs/resources/message#embed-object-embed-field-structure
type EmbedField = {
    Name: string
    Value: string
    Inline: bool option
}

// TODO: Handle documented embed limits
// TODO: Add poll result embed fields from https://discord.com/developers/docs/resources/message#embed-fields-by-embed-type-poll-result-embed-fields

// https://discord.com/developers/docs/resources/message#attachment-object-attachment-structure
type Attachment = {
    Id: string
    Filename: string
    Title: string option
    Description: string option
    ContentType: string option
    Size: int
    Url: string
    ProxyUrl: string
    Height: int option option
    Width: int option option
    Ephemeral: bool option
    DurationSecs: float option
    Waveform: string option
    Flags: AttachmentFlag list option
}

and PartialAttachment = {
    Id: string
    Filename: string option
    Title: string option
    Description: string option
    ContentType: string option
    Size: int option
    Url: string option
    ProxyUrl: string option
    Height: int option option
    Width: int option option
    Ephemeral: bool option
    DurationSecs: float option
    Waveform: string option
    Flags: AttachmentFlag list option
}

// https://discord.com/developers/docs/resources/message#channel-mention-object-channel-mention-structure
type ChannelMention = {
    Id: string
    GuildId: string
    Type: ChannelType
    Name: string
}

// https://discord.com/developers/docs/resources/message#allowed-mentions-object-allowed-mentions-structure
type AllowedMentions = {
    Parse: AllowedMentionsParseType list option
    Roles: string list option
    Users: string list option
    RepliedUser: bool option
}

// https://discord.com/developers/docs/resources/message#role-subscription-data-object-role-subscription-data-object-structure
type RoleSubscriptionData = {
    RoleSubscriptionListingId: string
    TierName: string
    TotalMonthsSubscribed: int
    IsRenewal: bool
}

// ----- Resources: Poll -----

// https://discord.com/developers/docs/resources/poll#poll-object-poll-object-structure
type Poll = {
    Question: PollMedia
    Answers: PollAnswer list
    Expiry: DateTime option
    AllowMultiselect: bool
    LayoutType: PollLayout
    Results: PollResults option
}

// https://discord.com/developers/docs/resources/poll#poll-create-request-object-poll-create-request-object-structure
type PollCreateRequest = {
    Question: PollMedia
    Answers: PollAnswer list
    Duration: int // TODO: up to 32 days
    AllowMultiselect: bool
    LayoutType: PollLayout
}

// https://discord.com/developers/docs/resources/poll#poll-media-object-poll-media-object-structure
type PollMedia = {
    Text: string option
    Emoji: PartialEmoji option
}

// https://discord.com/developers/docs/resources/poll#poll-answer-object
type PollAnswer = {
    AnswerId: int // TODO: "Only sent as part of responses from Discord's API/Gateway"
    PollMedia: PollMedia
}

// https://discord.com/developers/docs/resources/poll#poll-results-object-poll-results-object-structure
type PollResults = {
    IsFinalized: bool
    AnswerCounts: PollAnswerCount list
}

// https://discord.com/developers/docs/resources/poll#poll-results-object-poll-answer-count-object-structure
type PollAnswerCount = {
    Id: int
    Count: int
    MeVoted: bool
}

// ----- Resources: SKU -----

// https://discord.com/developers/docs/resources/sku#sku-object-sku-structure
type Sku = {
    Id: string
    Type: SkuType
    ApplicationId: string
    Name: string
    Slug: string
    Flags: SkuFlag list
}

// ----- Resources: Soundboard -----

// https://discord.com/developers/docs/resources/soundboard#soundboard-sound-object-soundboard-sound-structure
type SoundboardSound = {
    Name: string
    SoundId: string
    Volume: double
    EmojiId: string option
    EmojiName: string option
    GuildId: string option
    Available: bool
    User: User option
}

// ----- Resources: Stage Instance -----

// https://discord.com/developers/docs/resources/stage-instance#stage-instance-object-stage-instance-structure
type StageInstance = {
    Id: string
    GuildId: string
    ChannelId: string
    Topic: string
    PrivacyLevel: PrivacyLevel
    DiscoverableEnabled: bool
    GuildScheduledEventId: string option
}

// ----- Resources: Sticker -----

// https://discord.com/developers/docs/resources/sticker#sticker-object-sticker-structure
type Sticker = {
    Id: string
    PackId: string option
    Name: string
    Description: string option
    Tags: string // TODO: Convert to list based on comma separated delimitation
    Type: StickerType
    FormatType: StickerFormat
    Available: bool option
    GuildId: string option
    User: User option
    SortValue: int option
}

// https://discord.com/developers/docs/resources/sticker#sticker-item-object-sticker-item-structure
type StickerItem = {
    Id: string
    Name: string
    FormatType: StickerFormat
}

// https://discord.com/developers/docs/resources/sticker#sticker-pack-object
type StickerPack = {
    Id: string
    Stickers: Sticker list
    Name: string
    SkuId: string
    CoverStickerId: string option
    Description: string
    BannerAssetId: string option
}

// ----- Resources: Subscription -----

// https://discord.com/developers/docs/resources/subscription#subscription-object
type Subscription = {
    Id: string
    UserId: string
    SkuIds: string list
    EntitlementIds: string list
    RenewalSkuIds: string list option
    CurrentPeriodStart: DateTime
    CurrentPeriodEnd: DateTime
    Status: SubscriptionStatus
    CanceledAt: DateTime option
    Country: string option
}

// ----- Resources: User -----

// https://discord.com/developers/docs/resources/user#user-object-user-structure
type User = {
    Id: string
    Username: string
    Discriminator: string
    GlobalName: string option
    Avatar: string option
    Bot: bool option
    System: bool option
    MfaEnabled: bool option
    Banner: string option option
    AccentColor: int option option
    Locale: string option
    Verified: bool option
    Email: string option option
    Flags: UserFlag list option
    PremiumType: UserPremiumTier option
    PublicFlags: UserFlag list option
    AvatarDecorationData: AvatarDecorationData option option
}

and PartialUser = {
    Id: string
    Username: string option
    Discriminator: string option
    GlobalName: string option option
    Avatar: string option option
    Bot: bool option
    System: bool option
    MfaEnabled: bool option
    Banner: string option option
    AccentColor: int option option
    Locale: string option
    Verified: bool option
    Email: string option option
    Flags: UserFlag list option
    PremiumType: UserPremiumTier option
    PublicFlags: UserFlag list option
    AvatarDecorationData: AvatarDecorationData option option
}

module User =
    /// Get the avatar index for the user to fetch their default avatar if no custom avatar is set
    let getAvatarIndex (user: User) =
        match int user.Discriminator, Int64.Parse user.Id with
        | 0, id -> (int (id >>> 22)) % 6
        | discriminator, _ -> discriminator % 5

// https://discord.com/developers/docs/resources/user#avatar-decoration-data-object-avatar-decoration-data-structure
type AvatarDecorationData = {
    Asset: string
    SkuId: string
}

// https://discord.com/developers/docs/resources/user#connection-object-connection-structure
type Connection = {
    Id: string
    Name: string
    Type: ConnectionServiceType
    Revoked: bool option
    Integrations: PartialIntegration list option
    Verified: bool
    FriendSync: bool
    ShowActivity: bool
    TwoWayLink: bool
    Visibility: ConnectionVisibility
}

// https://discord.com/developers/docs/resources/user#application-role-connection-object-application-role-connection-structure
type ApplicationRoleConnection = {
    PlatformName: string option
    PlatformUsername: string option
    Metadata: Map<string, string> // value is the "stringified value" of the metadata
}

// ----- Resources: Voice -----

// https://discord.com/developers/docs/resources/voice#voice-state-object-voice-state-structure
type VoiceState = {
    GuildId: string option
    ChannelId: string option
    UserId: string
    Member: GuildMember option
    SessionId: string
    Deaf: bool
    Mute: bool
    SelfDeaf: bool
    SelfMute: bool
    SelfStream: bool option
    SelfVideo: bool
    Suppress: bool
    RequestToSpeakTimestamp: DateTime option
}

and PartialVoiceState = {
    GuildId: string option
    ChannelId: string option option
    UserId: string option
    Member: GuildMember option
    SessionId: string option
    Deaf: bool option
    Mute: bool option
    SelfDeaf: bool option
    SelfMute: bool option
    SelfStream: bool option
    SelfVideo: bool option
    Suppress: bool option
    RequestToSpeakTimestamp: DateTime option option
}

// https://discord.com/developers/docs/resources/voice#voice-region-object-voice-region-structure
type VoiceRegion = {
    Id: string
    Name: string
    Optimal: bool
    Deprecated: bool
    Custom: bool
}

// ----- Resources: Webhook -----

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-structure
type Webhook = {
    Id: string
    Type: WebhookType
    GuildId: string option option
    ChannelId: string option
    User: User option
    Name: string option
    Avatar: string option
    Token: string option
    ApplicationId: string option
    SourceGuild: PartialGuild option
    SourceChannel: PartialChannel option
    Url: string option
}

// ----- Topics: Permissions -----

// TODO: Create modules for computing permissions as documented

// https://discord.com/developers/docs/topics/permissions#role-object-role-structure
type Role = {
    Id: string
    Name: string
    Color: int
    Hoist: bool
    Icon: string option option
    UnicodeEmoji: string option option
    Position: int
    Permissions: string
    Managed: bool
    Mentionable: bool
    Tags: RoleTags option
    Flags: RoleFlag list
}

// https://discord.com/developers/docs/topics/permissions#role-object-role-tags-structure
type RoleTags = {
    BotId: string option
    IntegrationId: string option
    PremiumSubscriber: bool
    SubscriptionListingId: string option
    AvailableForPurchase: bool
    GuildConnections: bool
}

// ----- Topics: Teams -----

// https://discord.com/developers/docs/topics/teams#data-models-team-object
type Team = {
    Icon: string option
    Id: string
    Members: TeamMember list
    Name: string
    OwnerUserId: string
}

// https://discord.com/developers/docs/topics/teams#data-models-team-member-object
type TeamMember = {
    MembershipState: MembershipState
    TeamId: string
    User: PartialUser // avatar, discriminator, id, username
    Role: TeamMemberRoleType
}

// TODO: Convert url strings to Uri type (?)
// TODO: Create single DUs for values with range/size/etc rules
// TODO: Add comments describing properties as per docs
