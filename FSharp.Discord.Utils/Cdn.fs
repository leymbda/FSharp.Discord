namespace FSharp.Discord.Utils

open FSharp.Discord.Types

module Cdn =
    [<RequireQualifiedAccess>]
    type ImageSize =
        | x16
        | x32
        | x64
        | x128
        | x256
        | x512
        | x1024
        | x2048
        | x4096

    module ImageSize =
        let toInt (size: ImageSize) =
            match size with
            | ImageSize.x16 -> 16
            | ImageSize.x32 -> 32
            | ImageSize.x64 -> 64
            | ImageSize.x128 -> 128
            | ImageSize.x256 -> 256
            | ImageSize.x512 -> 512
            | ImageSize.x1024 -> 1024
            | ImageSize.x2048 -> 2048
            | ImageSize.x4096 -> 4096

        let asParamSuffix (size: ImageSize option) =
            match size with
            | None -> ""
            | Some size -> $"?size={toInt size}"

    type CustomEmojiFileType = PNG | JPEG | WEBP | GIF

    module CustomEmojiFileType =
        let toString (fileType: CustomEmojiFileType) =
            match fileType with
            | CustomEmojiFileType.PNG -> "png"
            | CustomEmojiFileType.JPEG -> "jpeg"
            | CustomEmojiFileType.WEBP -> "webp"
            | CustomEmojiFileType.GIF -> "gif"

    let customEmoji (emojiId: string) (fileType: CustomEmojiFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"emojis/{emojiId}.{CustomEmojiFileType.toString fileType}" + ImageSize.asParamSuffix size

    type GuildIconFileType = PNG | JPEG | WEBP | GIF

    module GuildIconFileType =
        let toString (fileType: GuildIconFileType) =
            match fileType with
            | GuildIconFileType.PNG -> "png"
            | GuildIconFileType.JPEG -> "jpeg"
            | GuildIconFileType.WEBP -> "webp"
            | GuildIconFileType.GIF -> "gif"

        let withAnimatedPrefix (hash: string) (fileType: GuildIconFileType) =
            match fileType with
            | GuildIconFileType.GIF -> "a_" + hash
            | _ -> hash

    let guildIcon (guildId: string) (guildIconHash: string) (fileType: GuildIconFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"icons/{guildId}/{GuildIconFileType.withAnimatedPrefix guildIconHash fileType}.{GuildIconFileType.toString fileType}" + ImageSize.asParamSuffix size

    type GuildSplashFileType = PNG | JPEG | WEBP

    module GuildSplashFileType =
        let toString (fileType: GuildSplashFileType) =
            match fileType with
            | GuildSplashFileType.PNG -> "png"
            | GuildSplashFileType.JPEG -> "jpeg"
            | GuildSplashFileType.WEBP -> "webp"

    let guildSplash (guildId: string) (guildSplashHash: string) (fileType: GuildSplashFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"splashes/{guildId}/{guildSplashHash}.{GuildSplashFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type GuildDiscoverySplashFileType = PNG | JPEG | WEBP

    module GuildDiscoverySplashFileType =
        let toString (fileType: GuildDiscoverySplashFileType) =
            match fileType with
            | GuildDiscoverySplashFileType.PNG -> "png"
            | GuildDiscoverySplashFileType.JPEG -> "jpeg"
            | GuildDiscoverySplashFileType.WEBP -> "webp"

    let guildDiscoverySplash (guildId: string) (guildDiscoverySplashHash: string) (fileType: GuildDiscoverySplashFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"discovery-splashes/{guildId}/{guildDiscoverySplashHash}.{GuildDiscoverySplashFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type GuildBannerFileType = PNG | JPEG | WEBP | GIF

    module GuildBannerFileType =
        let toString (fileType: GuildBannerFileType) =
            match fileType with
            | GuildBannerFileType.PNG -> "png"
            | GuildBannerFileType.JPEG -> "jpeg"
            | GuildBannerFileType.WEBP -> "webp"
            | GuildBannerFileType.GIF -> "gif"

        let withAnimatedPrefix (hash: string) (fileType: GuildBannerFileType) =
            match fileType with
            | GuildBannerFileType.GIF -> "a_" + hash
            | _ -> hash

    let guildBanner (guildId: string) (guildBannerHash: string) (fileType: GuildBannerFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"banners/{guildId}/{GuildBannerFileType.withAnimatedPrefix guildBannerHash fileType}.{GuildBannerFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type UserBannerFileType = PNG | JPEG | WEBP | GIF

    module UserBannerFileType =
        let toString (fileType: UserBannerFileType) =
            match fileType with
            | UserBannerFileType.PNG -> "png"
            | UserBannerFileType.JPEG -> "jpeg"
            | UserBannerFileType.WEBP -> "webp"
            | UserBannerFileType.GIF -> "gif"

        let withAnimatedPrefix (hash: string) (fileType: UserBannerFileType) =
            match fileType with
            | UserBannerFileType.GIF -> "a_" + hash
            | _ -> hash

    let userBanner (userId: string) (userBannerHash: string) (fileType: UserBannerFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"banners/{userId}/{UserBannerFileType.withAnimatedPrefix userBannerHash fileType}.{UserBannerFileType.toString fileType}" + ImageSize.asParamSuffix size

    let userDefaultAvatar (index: int) =
        IMAGE_BASE_URL + $"embed/avatars/{index}.png"
        
    type UserAvatarFileType = PNG | JPEG | WEBP | GIF

    module UserAvatarFileType =
        let toString (fileType: UserAvatarFileType) =
            match fileType with
            | UserAvatarFileType.PNG -> "png"
            | UserAvatarFileType.JPEG -> "jpeg"
            | UserAvatarFileType.WEBP -> "webp"
            | UserAvatarFileType.GIF -> "gif"

        let withAnimatedPrefix (hash: string) (fileType: UserAvatarFileType) =
            match fileType with
            | UserAvatarFileType.GIF -> "a_" + hash
            | _ -> hash

    let userAvatar (userId: string) (userAvatarHash: string) (fileType: UserAvatarFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"avatars/{userId}/{UserAvatarFileType.withAnimatedPrefix userAvatarHash fileType}.{UserAvatarFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type GuildMemberAvatarFileType = PNG | JPEG | WEBP | GIF

    module GuildMemberAvatarFileType =
        let toString (fileType: GuildMemberAvatarFileType) =
            match fileType with
            | GuildMemberAvatarFileType.PNG -> "png"
            | GuildMemberAvatarFileType.JPEG -> "jpeg"
            | GuildMemberAvatarFileType.WEBP -> "webp"
            | GuildMemberAvatarFileType.GIF -> "gif"

        let withAnimatedPrefix (hash: string) (fileType: GuildMemberAvatarFileType) =
            match fileType with
            | GuildMemberAvatarFileType.GIF -> "a_" + hash
            | _ -> hash

    let guildMemberAvatar (guildId: string) (userId: string) (guildMemberAvatarHash: string) (fileType: GuildMemberAvatarFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"guilds/{guildId}/users/{userId}/avatars/{GuildMemberAvatarFileType.withAnimatedPrefix guildMemberAvatarHash fileType}.{GuildMemberAvatarFileType.toString fileType}" + ImageSize.asParamSuffix size

    let avatarDecoration (assetHash: string) =
        IMAGE_BASE_URL + $"avatar-decoration-presets/{assetHash}.png"
        
    type ApplicationIconFileType = PNG | JPEG | WEBP

    module ApplicationIconFileType =
        let toString (fileType: ApplicationIconFileType) =
            match fileType with
            | ApplicationIconFileType.PNG -> "png"
            | ApplicationIconFileType.JPEG -> "jpeg"
            | ApplicationIconFileType.WEBP -> "webp"

    let applicationIcon (applicationId: string) (applicationIconHash: string) (fileType: ApplicationIconFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"app-icons/{applicationId}/{applicationIconHash}.{ApplicationIconFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type ApplicationCoverFileType = PNG | JPEG | WEBP

    module ApplicationCoverFileType =
        let toString (fileType: ApplicationCoverFileType) =
            match fileType with
            | ApplicationCoverFileType.PNG -> "png"
            | ApplicationCoverFileType.JPEG -> "jpeg"
            | ApplicationCoverFileType.WEBP -> "webp"

    let applicationCover (applicationId: string) (applicationCoverHash: string) (fileType: ApplicationCoverFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"app-icons/{applicationId}/{applicationCoverHash}.{ApplicationCoverFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type ApplicationAssetFileType = PNG | JPEG | WEBP

    module ApplicationAssetFileType =
        let toString (fileType: ApplicationAssetFileType) =
            match fileType with
            | ApplicationAssetFileType.PNG -> "png"
            | ApplicationAssetFileType.JPEG -> "jpeg"
            | ApplicationAssetFileType.WEBP -> "webp"

    let applicationAsset (applicationId: string) (applicationAssetId: string) (fileType: ApplicationAssetFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"app-assets/{applicationId}/{applicationAssetId}.{ApplicationAssetFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type AchievementIconFileType = PNG | JPEG | WEBP

    module AchievementIconFileType =
        let toString (fileType: AchievementIconFileType) =
            match fileType with
            | AchievementIconFileType.PNG -> "png"
            | AchievementIconFileType.JPEG -> "jpeg"
            | AchievementIconFileType.WEBP -> "webp"

    let achievementIcon (applicationId: string) (achievementId: string) (achievementIconHash: string) (fileType: AchievementIconFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"app-assets/{applicationId}/achievements/{achievementId}/icons/{achievementIconHash}.{AchievementIconFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type StorePageAssetFileType = PNG | JPEG | WEBP

    module StorePageAssetFileType =
        let toString (fileType: StorePageAssetFileType) =
            match fileType with
            | StorePageAssetFileType.PNG -> "png"
            | StorePageAssetFileType.JPEG -> "jpeg"
            | StorePageAssetFileType.WEBP -> "webp"

    let storePageAsset (applicationId: string) (assetId: string) (fileType: StorePageAssetFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"app-assets/{applicationId}/store/{assetId}.{StorePageAssetFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type StickerPackBannerFileType = PNG | JPEG | WEBP

    module StickerPackBannerFileType =
        let toString (fileType: StickerPackBannerFileType) =
            match fileType with
            | StickerPackBannerFileType.PNG -> "png"
            | StickerPackBannerFileType.JPEG -> "jpeg"
            | StickerPackBannerFileType.WEBP -> "webp"

    let stickerPackBanner (stickerPackBannerAssetId: string) (fileType: StickerPackBannerFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"app-assets/710982414301790216/store/{stickerPackBannerAssetId}.{StickerPackBannerFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type TeamIconFileType = PNG | JPEG | WEBP

    module TeamIconFileType =
        let toString (fileType: TeamIconFileType) =
            match fileType with
            | TeamIconFileType.PNG -> "png"
            | TeamIconFileType.JPEG -> "jpeg"
            | TeamIconFileType.WEBP -> "webp"

    let teamIcon (teamId: string) (teamIconHash: string) (fileType: TeamIconFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"team-icons/{teamId}/{teamIconHash}.{TeamIconFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type StickerFileType = PNG | LOTTIE | GIF

    module StickerFileType =
        let fromStickerFormat (format: StickerFormat) =
            match format with
            | StickerFormat.PNG -> StickerFileType.PNG
            | StickerFormat.APNG -> StickerFileType.PNG
            | StickerFormat.LOTTIE -> StickerFileType.LOTTIE
            | StickerFormat.GIF -> StickerFileType.GIF
            | _ -> StickerFileType.PNG // Should never occur

        let toString (fileType: StickerFileType) =
            match fileType with
            | StickerFileType.PNG -> "png"
            | StickerFileType.LOTTIE -> "lottie"
            | StickerFileType.GIF -> "gif"

    /// Only works if the correct file type for the sticker is provided. `Cdn.sticker` ensures this by using the sticker object itself.
    let stickerRaw (stickerId: string) (fileType: StickerFileType) =
        IMAGE_BASE_URL + $"stickers/{stickerId}.{StickerFileType.toString fileType}"

    let sticker (sticker: Sticker) =
        stickerRaw sticker.Id (StickerFileType.fromStickerFormat sticker.FormatType)
        
    type RoleIconFileType = PNG | JPEG | WEBP

    module RoleIconFileType =
        let toString (fileType: RoleIconFileType) =
            match fileType with
            | RoleIconFileType.PNG -> "png"
            | RoleIconFileType.JPEG -> "jpeg"
            | RoleIconFileType.WEBP -> "webp"

    let roleIcon (roleId: string) (roleIconHash: string) (fileType: RoleIconFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"role-icons/{roleId}/{roleIconHash}.{RoleIconFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type GuildScheduledEventCoverFileType = PNG | JPEG | WEBP

    module GuildScheduledEventCoverFileType =
        let toString (fileType: GuildScheduledEventCoverFileType) =
            match fileType with
            | GuildScheduledEventCoverFileType.PNG -> "png"
            | GuildScheduledEventCoverFileType.JPEG -> "jpeg"
            | GuildScheduledEventCoverFileType.WEBP -> "webp"

    let guildScheduledEventCover (scheduledEventId: string) (scheduledEventCoverImageHash: string) (fileType: GuildScheduledEventCoverFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"guild-events/{scheduledEventId}/{scheduledEventCoverImageHash}.{GuildScheduledEventCoverFileType.toString fileType}" + ImageSize.asParamSuffix size
        
    type GuildMemberBannerFileType = PNG | JPEG | WEBP | GIF

    module GuildMemberBannerFileType =
        let toString (fileType: GuildMemberBannerFileType) =
            match fileType with
            | GuildMemberBannerFileType.PNG -> "png"
            | GuildMemberBannerFileType.JPEG -> "jpeg"
            | GuildMemberBannerFileType.WEBP -> "webp"
            | GuildMemberBannerFileType.GIF -> "gif"

        let withAnimatedPrefix (hash: string) (fileType: GuildMemberBannerFileType) =
            match fileType with
            | GuildMemberBannerFileType.GIF -> "a_" + hash
            | _ -> hash

    let guildMemberBanner (guildId: string) (userId: string) (guildMemberBannerHash: string) (fileType: GuildMemberBannerFileType) (size: ImageSize option) =
        IMAGE_BASE_URL + $"guilds/{guildId}/users/{userId}/banners/{GuildMemberBannerFileType.withAnimatedPrefix guildMemberBannerHash fileType}.{GuildMemberBannerFileType.toString fileType}" + ImageSize.asParamSuffix size
