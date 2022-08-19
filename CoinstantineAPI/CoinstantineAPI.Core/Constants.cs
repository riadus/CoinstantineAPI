using System;
namespace CoinstantineAPI.Core
{
    public static class Constants
    {
        private static readonly string SqlPrefix = "SQLCONNSTR";
        private static readonly string AppSettingsPrefix = "APPSETTING";

        private static string ApiEnvironmentKey = "ASPNETCORE_ENVIRONMENT";
        private static string ConnectionDbKey = $"{SqlPrefix}_ConnectionDB";
        private static string AzureTenantKey => $"{AppSettingsPrefix}_AzureTenant";
        private static string AzurePolicyKey => $"{AppSettingsPrefix}_AzurePolicy";
        private static string AzureClientIdKey => $"{AppSettingsPrefix}_AzureClientId";
        private static string EtherscanUrlKey => $"{AppSettingsPrefix}_Etherscan";
        private static string Web3UrlKey => $"{AppSettingsPrefix}_Web3";
        private static string EthereumEnvironmentKey => $"{AppSettingsPrefix}_EthereumEnvironment";
        private static string TwitterAccessTokenKey => $"{AppSettingsPrefix}_TwitterAccessToken";
        private static string TwitterAccessTokenSecretKey => $"{AppSettingsPrefix}_TwitterAccessTokenSecret";
        private static string TwitterKeyKey => $"{AppSettingsPrefix}_TwitterKey";
        private static string TwitterSecretKey => $"{AppSettingsPrefix}_TwitterSecret";
        private static string AzureNotificationAccessSignatureKey => $"{AppSettingsPrefix}_AzureNotificationAccessSignature";
        private static string AzureNotificationHubsNameKey => $"{AppSettingsPrefix}_AzureNotificationHubsName";
        private static string TelegramBotTokenKey => $"{AppSettingsPrefix}_TelegramBotToken";
        private static string TelegramTimeoutKey => $"{AppSettingsPrefix}_TelegramTimeout";
        private static string TelegramUrlWebhookKey => $"{AppSettingsPrefix}_TelegramUrlWebhook";

        private static string VaultClientResourceKey => $"{AppSettingsPrefix}_VaultClientResource";
        private static string VaultClientCredKey => $"{AppSettingsPrefix}_VaultClientCred";
        private static string PassphraseKeyKey => $"{AppSettingsPrefix}_PassphraseKey";
        private static string PhonenumberKeyKey => $"{AppSettingsPrefix}_PhonenumberKey";
        private static string JsonKeyKey => $"{AppSettingsPrefix}_JsonKey";
        private static string OwnerPrivateKeyKey => $"{AppSettingsPrefix}_OwnerPrivateKey";
        private static string OwnerPasswordKey => $"{AppSettingsPrefix}_OwnerPassword";
        private static string UserPasswordKey => $"{AppSettingsPrefix}_UserPassword";
        private static string VaultUrlKey => $"{AppSettingsPrefix}_VaultUrl";
        private static string SendGridApiKeyKey => $"{AppSettingsPrefix}_SendGridApiKey";
        private static string AccountCreationTemplateKey => $"{AppSettingsPrefix}_AccountCreationTemplateKey";
        private static string ResetPasswordTemplateKey => $"{AppSettingsPrefix}_ResetPasswordTemplateKey";
        private static string SendUsernameTemplateKey => $"{AppSettingsPrefix}_SendUsernameTemplateKey";
        private static string JwtKey => $"{AppSettingsPrefix}_JwtKey"; 
        private static string WebsiteUrlKey => $"{AppSettingsPrefix}_WebsiteUrl";
        private static string AdminEmailKey => $"{AppSettingsPrefix}_AdminEmail";
        private static string TokenDurationInHoursKey => $"{AppSettingsPrefix}_TokenDurationInHours";
        private static string RefreshTokenDurationInHoursKey => $"{AppSettingsPrefix}_RefreshTokenDurationInHours";
        private static string ReCaptchaSecretKey => $"{AppSettingsPrefix}_ReCaptchaSecret";
        private static string AzureFilesEndpointKey => $"{AppSettingsPrefix}_AzureFilesEndpoint";
        private static string DiscordBotTokenKey => $"{AppSettingsPrefix}_DiscordBotToken";

        public static string ApiEnvironment => Environment.GetEnvironmentVariable(ApiEnvironmentKey);
        public static string ConnectionDb => Environment.GetEnvironmentVariable(ConnectionDbKey);
        public static string AzureTenant => Environment.GetEnvironmentVariable(AzureTenantKey);
        public static string AzurePolicy => Environment.GetEnvironmentVariable(AzurePolicyKey);
        public static string AzureClientId => Environment.GetEnvironmentVariable(AzureClientIdKey);
        public static string EtherscanUrl => Environment.GetEnvironmentVariable(EtherscanUrlKey);
        public static string Web3Url => Environment.GetEnvironmentVariable(Web3UrlKey);
        public static string EthereumEnvironment => Environment.GetEnvironmentVariable(EthereumEnvironmentKey);
        public static string TwitterAccessToken => Environment.GetEnvironmentVariable(TwitterAccessTokenKey);
        public static string TwitterAccessTokenSecret => Environment.GetEnvironmentVariable(TwitterAccessTokenSecretKey);
        public static string TwitterKey => Environment.GetEnvironmentVariable(TwitterKeyKey);
        public static string TwitterSecret => Environment.GetEnvironmentVariable(TwitterSecretKey);
        public static string AzureNotificationAccessSignature => Environment.GetEnvironmentVariable(AzureNotificationAccessSignatureKey);
        public static string AzureNotificationHubsName => Environment.GetEnvironmentVariable(AzureNotificationHubsNameKey);
        public static string TelegramBotToken => Environment.GetEnvironmentVariable(TelegramBotTokenKey);
        public static string TelegramTimeout => Environment.GetEnvironmentVariable(TelegramTimeoutKey);
        public static string TelegramUrlWebhook => Environment.GetEnvironmentVariable(TelegramUrlWebhookKey);

        public static string VaultClientResource => Environment.GetEnvironmentVariable(VaultClientResourceKey);
        public static string VaultClientCred => Environment.GetEnvironmentVariable(VaultClientCredKey);
        public static string PassphraseKey => Environment.GetEnvironmentVariable(PassphraseKeyKey);
        public static string PhonenumberKey => Environment.GetEnvironmentVariable(PhonenumberKeyKey);
        public static string JsonKey => Environment.GetEnvironmentVariable(JsonKeyKey);
        public static string OwnerPrivateKey => Environment.GetEnvironmentVariable(OwnerPrivateKeyKey);
        public static string OwnerPassword => Environment.GetEnvironmentVariable(OwnerPasswordKey);
        public static string UserPassword => Environment.GetEnvironmentVariable(UserPasswordKey);
        public static string VaultUrl => Environment.GetEnvironmentVariable(VaultUrlKey);
        public static string SendGridApiKey => Environment.GetEnvironmentVariable(SendGridApiKeyKey);
        public static string AccountCreationTemplate => Environment.GetEnvironmentVariable(AccountCreationTemplateKey);
        public static string ResetPasswordTemplate => Environment.GetEnvironmentVariable(ResetPasswordTemplateKey);
        public static string SendUsernameTemplate => Environment.GetEnvironmentVariable(SendUsernameTemplateKey);
        public static string Jwt => Environment.GetEnvironmentVariable(JwtKey);
        public static string WebsiteUrl => Environment.GetEnvironmentVariable(WebsiteUrlKey);
        public static string AdminEmail => Environment.GetEnvironmentVariable(AdminEmailKey);
        public static string ReCaptchaSecret => Environment.GetEnvironmentVariable(ReCaptchaSecretKey);

        public static int TokenDurationInHours => int.Parse(Environment.GetEnvironmentVariable(TokenDurationInHoursKey));
        public static int RefreshTokenDurationInHours => int.Parse(Environment.GetEnvironmentVariable(RefreshTokenDurationInHoursKey));

        public static string AzureFilesEndpoint => Environment.GetEnvironmentVariable(AzureFilesEndpointKey); 
        public static string DiscordBotToken => Environment.GetEnvironmentVariable(DiscordBotTokenKey); 
    }
}
