namespace FSharp.Discord.Utils

open FSharp.Discord.Types
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type OAuthTests () =
    [<TestMethod>]
    member _.``authorizationUrl - Builds basic authorization url`` () =
        // Arrange
        let clientId = "1234567890"
        let redirectUri = "https://example.com"
        let scopes = [OAuth2Scope.IDENTIFY]

        // Act
        let url = OAuth.authorizationUrl clientId redirectUri scopes None OAuthConsent.None None

        // Assert
        Assert.AreEqual<string>("https://discord.com/oauth2/authorize?client_id=1234567890&redirect_uri=https%3a%2f%2fexample.com&response_type=code&scope=identify&prompt=none", url)

    [<TestMethod>]
    member _.``authorizationUrl - Builds complex authorization url`` () =
        // Arrange
        let clientId = "1234567890"
        let redirectUri = "https://example.com"
        let scopes = [OAuth2Scope.IDENTIFY; OAuth2Scope.BOT]
        let state = Some "state"
        let prompt = OAuthConsent.Consent
        let integrationType = Some ApplicationIntegrationType.GUILD_INSTALL

        // Act
        let url = OAuth.authorizationUrl clientId redirectUri scopes state prompt integrationType

        // Assert
        Assert.AreEqual<string>("https://discord.com/oauth2/authorize?client_id=1234567890&redirect_uri=https%3a%2f%2fexample.com&response_type=code&scope=identify+bot&state=state&prompt=consent&integration_type=0", url)
