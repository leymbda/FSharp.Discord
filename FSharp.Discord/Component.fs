namespace rec FSharp.Discord

open FSharp.Discord.Types

module ActionRow =
    let create () =
        {
            ActionRow.Id = None
            Components = []
        }

    let setId id (v: ActionRow) =
        { v with Id = Some id }

    let addComponent component' (v: ActionRow) =
        { v with Components = v.Components @ [component'] }

module PrimaryButton =
    let create customId =
        {
            PrimaryButton.Id = None
            Label = None
            Emoji = None
            CustomId = customId
            Disabled = false
        }

    let setId id (v: PrimaryButton) =
        { v with Id = Some id }

    let setLabel label (v: PrimaryButton) =
        { v with Label = Some label }

    let setEmoji emoji (v: PrimaryButton) =
        { v with Emoji = Some emoji }

    let setCustomId customId (v: PrimaryButton) =
        { v with CustomId = customId }

    let setDisabled disabled (v: PrimaryButton) =
        { v with Disabled = disabled }

module SecondaryButton =
    let create customId =
        {
            SecondaryButton.Id = None
            Label = None
            Emoji = None
            CustomId = customId
            Disabled = false
        }

    let setId id (v: SecondaryButton) =
        { v with Id = Some id }

    let setLabel label (v: SecondaryButton) =
        { v with Label = Some label }

    let setEmoji emoji (v: SecondaryButton) =
        { v with Emoji = Some emoji }

    let setCustomId customId (v: SecondaryButton) =
        { v with CustomId = customId }

    let setDisabled disabled (v: SecondaryButton) =
        { v with Disabled = disabled }
        
module SuccessButton =
    let create customId =
        {
            SuccessButton.Id = None
            Label = None
            Emoji = None
            CustomId = customId
            Disabled = false
        }

    let setId id (v: SuccessButton) =
        { v with Id = Some id }

    let setLabel label (v: SuccessButton) =
        { v with Label = Some label }

    let setEmoji emoji (v: SuccessButton) =
        { v with Emoji = Some emoji }

    let setCustomId customId (v: SuccessButton) =
        { v with CustomId = customId }

    let setDisabled disabled (v: SuccessButton) =
        { v with Disabled = disabled }
        
module DangerButton =
    let create customId =
        {
            DangerButton.Id = None
            Label = None
            Emoji = None
            CustomId = customId
            Disabled = false
        }

    let setId id (v: DangerButton) =
        { v with Id = Some id }

    let setLabel label (v: DangerButton) =
        { v with Label = Some label }

    let setEmoji emoji (v: DangerButton) =
        { v with Emoji = Some emoji }

    let setCustomId customId (v: DangerButton) =
        { v with CustomId = customId }

    let setDisabled disabled (v: DangerButton) =
        { v with Disabled = disabled }
        
module LinkButton =
    let create url =
        {
            LinkButton.Id = None
            Label = None
            Emoji = None
            Url = url
            Disabled = false
        }

    let setId id (v: LinkButton) =
        { v with Id = Some id }

    let setLabel label (v: LinkButton) =
        { v with Label = Some label }

    let setEmoji emoji (v: LinkButton) =
        { v with Emoji = Some emoji }

    let setUrl url (v: LinkButton) =
        { v with Url = url }

    let setDisabled disabled (v: LinkButton) =
        { v with Disabled = disabled }
        
module PremiumButton =
    let create skuId =
        {
            PremiumButton.Id = None
            Label = None
            Emoji = None
            SkuId = skuId
            Disabled = false
        }

    let setId id (v: PremiumButton) =
        { v with Id = Some id }

    let setLabel label (v: PremiumButton) =
        { v with Label = Some label }

    let setEmoji emoji (v: PremiumButton) =
        { v with Emoji = Some emoji }

    let setSkuId skuId (v: PremiumButton) =
        { v with SkuId = skuId }

    let setDisabled disabled (v: PremiumButton) =
        { v with Disabled = disabled }

module StringSelect =
    let create customId =
        {
            StringSelect.Id = None
            CustomId = customId
            Options = []
            Placeholder = None
            MinValues = 1
            MaxValues = 1
            Disabled = false
        }

    let setId id (v: StringSelect) =
        { v with Id = Some id }

    let setCustomId customId (v: StringSelect) =
        { v with CustomId = customId }

    let addOption option (v: StringSelect) =
        { v with Options = v.Options @ [option] }

    let setPlaceholder placeholder (v: StringSelect) =
        { v with Placeholder = Some placeholder }

    let setMinValues minValues (v: StringSelect) =
        { v with MinValues = minValues }

    let setMaxValues maxValues (v: StringSelect) =
        { v with MaxValues = maxValues }

    let setDisaabled disabled (v: StringSelect) =
        { v with Disabled = disabled }

module SelectOption =
    let create label value =
        {
            SelectOption.Label = label
            Value = value
            Description = None
            Emoji = None
            Default = None
        }

    let setLabel label (v: SelectOption) =
        { v with Label = label }

    let setValue value (v: SelectOption) =
        { v with Value = value }

    let setDescription description (v: SelectOption) =
        { v with Description = Some description }

    let setEmoji emoji (v: SelectOption) =
        { v with Emoji = Some emoji }

    let setIsDefault default' (v: SelectOption) =
        { v with Default = default' }

module ShortTextInput =
    let create customId label =
        {
            ShortTextInput.Id = None
            CustomId = customId
            Label = label
            Placeholder = None
            MinLength = None
            MaxLength = None
            Required = false
            Value = None
        }

    let setId id (v: ShortTextInput) =
        { v with Id = Some id }

    let setCustomId customId (v: ShortTextInput) =
        { v with CustomId = customId }

    let setLabel label (v: ShortTextInput) =
        { v with Label = label }

    let setMinLength minLength (v: ShortTextInput) =
        { v with MinLength = Some minLength }

    let setMaxLength maxLength (v: ShortTextInput) =
        { v with MaxLength = Some maxLength }

    let setRequired required (v: ShortTextInput) =
        { v with Required = required }

    let setValue value (v: ShortTextInput) =
        { v with Value = Some value }

    let setPlaceholder placeholder (v: ShortTextInput) =
        { v with Placeholder = Some placeholder }

module ParagraphTextInput =
    let create customId label =
        {
            ParagraphTextInput.Id = None
            CustomId = customId
            Label = label
            Placeholder = None
            MinLength = None
            MaxLength = None
            Required = false
            Value = None
        }

    let setId id (v: ParagraphTextInput) =
        { v with Id = Some id }

    let setCustomId customId (v: ParagraphTextInput) =
        { v with CustomId = customId }

    let setLabel label (v: ParagraphTextInput) =
        { v with Label = label }

    let setMinLength minLength (v: ParagraphTextInput) =
        { v with MinLength = Some minLength }

    let setMaxLength maxLength (v: ParagraphTextInput) =
        { v with MaxLength = Some maxLength }

    let setRequired required (v: ParagraphTextInput) =
        { v with Required = required }

    let setValue value (v: ParagraphTextInput) =
        { v with Value = Some value }

    let setPlaceholder placeholder (v: ParagraphTextInput) =
        { v with Placeholder = Some placeholder }
        
module UserSelect =
    let create customId =
        {
            UserSelect.Id = None
            CustomId = customId
            DefaultValues = None
            Placeholder = None
            MinValues = 1
            MaxValues = 1
            Disabled = false
        }

    let setId id (v: UserSelect) =
        { v with Id = Some id }

    let setCustomId customId (v: UserSelect) =
        { v with CustomId = customId }

    let addDefaultUserValue defaultValue (v: UserSelect) =
        { v with DefaultValues = Some ((Option.defaultValue [] v.DefaultValues) @ [SelectDefaultValue.USER defaultValue]) }

    let setPlaceholder placeholder (v: UserSelect) =
        { v with Placeholder = Some placeholder }

    let setMinValues minValues (v: UserSelect) =
        { v with MinValues = minValues }

    let setMaxValues maxValues (v: UserSelect) =
        { v with MaxValues = maxValues }

    let setDisabled disabled (v: UserSelect) =
        { v with Disabled = disabled }
        
module RoleSelect =
    let create customId =
        {
            RoleSelect.Id = None
            CustomId = customId
            DefaultValues = None
            Placeholder = None
            MinValues = 1
            MaxValues = 1
            Disabled = false
        }

    let setId id (v: RoleSelect) =
        { v with Id = Some id }

    let setCustomId customId (v: RoleSelect) =
        { v with CustomId = customId }

    let addDefaultRoleValue defaultValue (v: RoleSelect) =
        { v with DefaultValues = Some ((Option.defaultValue [] v.DefaultValues) @ [SelectDefaultValue.ROLE defaultValue]) }

    let setPlaceholder placeholder (v: RoleSelect) =
        { v with Placeholder = Some placeholder }

    let setMinValues minValues (v: RoleSelect) =
        { v with MinValues = minValues }

    let setMaxValues maxValues (v: RoleSelect) =
        { v with MaxValues = maxValues }

    let setDisabled disabled (v: RoleSelect) =
        { v with Disabled = disabled }
        
module MentionableSelect =
    let create customId =
        {
            MentionableSelect.Id = None
            CustomId = customId
            DefaultValues = None
            Placeholder = None
            MinValues = 1
            MaxValues = 1
            Disabled = false
        }

    let setId id (v: MentionableSelect) =
        { v with Id = Some id }

    let setCustomId customId (v: MentionableSelect) =
        { v with CustomId = customId }

    let addDefaultRoleValue defaultValue (v: MentionableSelect) =
        { v with DefaultValues = Some ((Option.defaultValue [] v.DefaultValues) @ [SelectDefaultValue.ROLE defaultValue]) }
        
    let addDefaultUserValue defaultValue (v: MentionableSelect) =
        { v with DefaultValues = Some ((Option.defaultValue [] v.DefaultValues) @ [SelectDefaultValue.USER defaultValue]) }

    let setPlaceholder placeholder (v: MentionableSelect) =
        { v with Placeholder = Some placeholder }

    let setMinValues minValues (v: MentionableSelect) =
        { v with MinValues = minValues }

    let setMaxValues maxValues (v: MentionableSelect) =
        { v with MaxValues = maxValues }

    let setDisabled disabled (v: MentionableSelect) =
        { v with Disabled = disabled }
        
module ChannelSelect =
    let create customId =
        {
            ChannelSelect.Id = None
            CustomId = customId
            ChannelTypes = None
            DefaultValues = None
            Placeholder = None
            MinValues = 1
            MaxValues = 1
            Disabled = false
        }

    let setId id (v: ChannelSelect) =
        { v with Id = Some id }

    let setCustomId customId (v: ChannelSelect) =
        { v with CustomId = customId }

    let addChannelType channelType (v: ChannelSelect) =
        { v with ChannelTypes = Some ((Option.defaultValue [] v.ChannelTypes) @ [channelType]) }

    let addDefaultChannelValue defaultValue (v: ChannelSelect) =
        { v with DefaultValues = Some ((Option.defaultValue [] v.DefaultValues) @ [SelectDefaultValue.CHANNEL defaultValue]) }
        
    let setPlaceholder placeholder (v: ChannelSelect) =
        { v with Placeholder = Some placeholder }

    let setMinValues minValues (v: ChannelSelect) =
        { v with MinValues = minValues }

    let setMaxValues maxValues (v: ChannelSelect) =
        { v with MaxValues = maxValues }

    let setDisabled disabled (v: ChannelSelect) =
        { v with Disabled = disabled }

module Section =
    let create () =
        {
            Section.Id = None
            Components = []
            Accessory = None
        }
        
    let setId id (v: Section) =
        { v with Id = Some id }

    let addComponent component' (v: Section) =
        { v with Components = v.Components @ [component'] }

    let setAccessory accessory (v: Section) =
        { v with Accessory = Some accessory }

module TextDisplay =
    let create content =
        {
            TextDisplay.Id = None
            Content = content
        }
        
    let setId id (v: TextDisplay) =
        { v with Id = Some id }

    let setContent content (v: TextDisplay) =
        { v with Content = content }

module Thumbnail =
    let create url =
        {
            Thumbnail.Id = None
            Media = {
                Url = url
                ProxyUrl = None
                Height = None
                Width = None
                ContentType = None
            }
            Description = None
            Spoiler = false
        }

    let setId id (v: Thumbnail) =
        { v with Id = Some id }

    let setMedia media (v: Thumbnail) =
        { v with Media = media }

    let setDescription description (v: Thumbnail) =
        { v with Description = Some description }

    let setSpoiler spoiler (v: Thumbnail) =
        { v with Spoiler = spoiler }

module MediaGallery =
    let create () =
        {
            MediaGallery.Id = None
            Items = []
        }

    let setId id (v: MediaGallery) =
        { v with Id = Some id }

    let addMedia media (v: MediaGallery) =
        { v with Items = v.Items @ [media] }

module MediaGalleryItem =
    let create url =
        {
            Media = {
                Url = url
                ProxyUrl = None
                Height = None
                Width = None
                ContentType = None
            }
            Description = None
            Spoiler = false
        }
        
    let setMedia media (v: MediaGalleryItem) =
        { v with Media = media }

    let setDescription description (v: MediaGalleryItem) =
        { v with Description = Some description }

    let setSpoiler spoiler (v: MediaGalleryItem) =
        { v with Spoiler = spoiler }

module File =
    let create url =
        {
            File.Id = None
            File = {
                Url = url
                ProxyUrl = None
                Height = None
                Width = None
                ContentType = None
            }
            Spoiler = false
        }

    let setId id (v: File) =
        { v with Id = Some id }
        
    let setFile file (v: File) =
        { v with File = file }

    let setSpoiler spoiler (v: File) =
        { v with Spoiler = spoiler }

module Separator =
    let create () =
        {
            Separator.Id = None
            Divider = true
            Spacing = SeparatorPaddingType.SMALL
        }

    let setId id (v: Separator) =
        { v with Id = Some id }

    let setDivider divider (v: Separator) =
        { v with Divider = divider }

    let setSpacing spacing (v: Separator) =
        { v with Spacing = spacing }

module Container =
    let create () =
        {
            Container.Id = None
            Components = []
            AccentColor = None
            Spoiler = false
        }

    let setId id (v: Container) =
        { v with Id = Some id }

    let addComponent component' (v: Container) =
        { v with Components = v.Components @ [component'] }

    let setAccentColor color (v: Container) =
        { v with AccentColor = Some color }

    let setSpoiler spoiler (v: Container) =
        { v with Spoiler = spoiler }

module UnfurledMediaItem =
    let create url =
        {
            Url = url
            ProxyUrl = None
            Height = None
            Width = None
            ContentType = None
        }

    let setUrl url (v: UnfurledMediaItem) =
        { v with Url = url }

    let setProxyUrl proxyUrl (v: UnfurledMediaItem) =
        { v with ProxyUrl = Some proxyUrl }

    let setHeight height (v: UnfurledMediaItem) =
        { v with Height = Some height }

    let setWidth width (v: UnfurledMediaItem) =
        { v with Width = Some width }

    let setContentType contentType (v: UnfurledMediaItem) =
        { v with ContentType = Some contentType }
