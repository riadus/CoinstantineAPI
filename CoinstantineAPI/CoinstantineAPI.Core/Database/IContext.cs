using System;
using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Core.Database
{
    public interface IContext : IDisposable
    {
        DbSet<BlockchainUser> BlockchainUsers { get; }
        DbSet<Airdrop> Airdrops { get; }
        DbSet<Token> Tokens { get; }
        DbSet<SmartContract> SmartContracts { get; }
        DbSet<Subscriber> Subscribers { get; }
        DbSet<ApiUser> ApiUsers { get; }
        DbSet<Tweet> Tweets { get; }
        DbSet<UserIdentity> UserIdentities { get; }
        DbSet<BitcoinTalkProfile> BitcoinTalkProfiles { get; }
        DbSet<Translation> Translations { get; }
        DbSet<BuyTokensResult> BuyTokensResults { get; }
        DbSet<TransactionReceiptData> TransactionReceipts { get; }
        DbSet<UserAirdrops> UserAirdrops { get; }
        DbSet<AirdropSubscription> AirdropSubscriptions { get; }
        DbSet<AirdropSubscriber> AirdropSubscribers { get; }
        DbSet<AirdropDefinition> AirdropDefinitions { get; }
        DbSet<TwitterAirdropRequirement> TwitterAirdropRequirements { get; }
        DbSet<TelegramAirdropRequirement> TelegramAirdropRequirements { get; }
        DbSet<BitcoinTalkAirdropRequirement> BitcoinTalkAirdropRequirements { get; }
        DbSet<TwitterProfile> TwitterProfiles { get; }
        DbSet<TelegramProfile> TelegramProfiles { get; }
        DbSet<DiscordProfile> DiscordProfiles { get; }
        DbSet<Application> Applications { get; }
        DbSet<RefreshTokens> RefreshTokens { get; }
        DbSet<Document> Documents { get; }
        DbSet<Game> Games { get; }
        DbSet<UserAchievement> UserAchievements { get; }
        DbSet<Achievements> Achievements { get; }
        DbSet<BlockchainInfo> BlockchainInfos { get; }
        DbSet<Referral> Referrals { get; set; }
        DbSet<TwitterConfig> TwitterConfigs { get; set; }
        Task SaveChangesAsync();
        void SetModified(Entity entity);
        bool Disposed { get; }
    }    
}
