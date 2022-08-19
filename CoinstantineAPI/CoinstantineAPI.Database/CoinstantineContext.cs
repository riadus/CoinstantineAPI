using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoinstantineAPI.Database
{
    public class CoinstantineContext : DbContext, IContext
    {
        public CoinstantineContext(DbContextOptions<CoinstantineContext> options)
                                : base(options)
        {
             Database.Migrate();
        }

        public CoinstantineContext(DbContextOptions<CoinstantineContext> options, bool migrate)
                        : base(options)
        {
            if (migrate)
            {
                Database.Migrate();
            }
            else
            {
                Database.EnsureCreated();
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmartContract>()
                        .HasOne(s => s.Token)
                        .WithOne(t => t.SmartContract)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApiUser>()
                        .HasOne(u => u.BctProfile)
                        .WithOne(p => p.ApiUser)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ApiUser>()
                        .HasOne(u => u.Telegram)
                        .WithOne(p => p.ApiUser)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ApiUser>()
                        .HasOne(u => u.TwitterProfile)
                        .WithOne(p => p.ApiUser)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ApiUser>()
                        .HasOne(u => u.BlockchainInfo)
                        .WithOne(p => p.ApiUser)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<UserIdentity>()
                        .HasOne(u => u.RelatedUser)
                        .WithOne(u => u.UserIdentity)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserIdentity>()
                       .HasMany(u => u.RefreshTokens)
                       .WithOne(u => u.User)
                       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BuyTokensResult>()
                        .HasOne(b => b.TransactionReceipt)
                        .WithOne(t => t.BuyTokensResult)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Airdrop>()
                        .HasOne(a => a.Creator)
                        .WithMany(u => u.Airdrops)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Airdrop>()
                        .HasOne(a => a.Token)
                        .WithOne(t => t.Airdrop)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Airdrop>()
                        .HasMany(a => a.Subscribers)
                        .WithOne(s => s.Airdrop)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AirdropSubscription>()
                        .HasMany(s => s.Subscribers)
                        .WithOne(s => s.AirdropSubscription)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AirdropDefinition>()
                        .HasOne(a => a.TwitterAirdropRequirement)
                        .WithOne(t => t.AirdropDefinition)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AirdropDefinition>()
                        .HasOne(a => a.TelegramAirdropRequirement)
                        .WithOne(t => t.AirdropDefinition)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AirdropDefinition>()
                        .HasOne(a => a.BitcoinTalkAirdropRequirement)
                        .WithOne(t => t.AirdropDefinition)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApiUser>()
                .HasMany(c => c.DiscordProfiles)
                .WithOne(e => e.ApiUser);

            modelBuilder.Entity<DiscordProfile>()
                .HasOne(c => c.ApiUser)
                .WithMany(e => e.DiscordProfiles);

            modelBuilder.Entity<Game>()
                .HasOne(x => x.AirdropDefinition);

            modelBuilder.Entity<Game>()
                .HasMany(x => x.Achievements);

            modelBuilder.Entity<Achievements>()
                .HasOne(x => x.ApiUser);

            modelBuilder.Entity<Achievements>()
                .HasMany(x => x.UserAchievements);

            modelBuilder.Entity<Referral>()
               .HasOne(x => x.GodFather);

            modelBuilder.Entity<Referral>()
                .HasMany(x => x.Users);

            CreateIndexes(modelBuilder);
        }

        private void CreateIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiUser>()
                            .HasIndex(u => u.Email)
                            .IsUnique();

            modelBuilder.Entity<UserIdentity>()
                        .HasIndex(u => u.EmailAddress)
                        .IsUnique();

            modelBuilder.Entity<BlockchainUser>()
                        .HasIndex(u => u.Username)
                        .IsUnique();

            modelBuilder.Entity<BlockchainInfo>()
                        .HasIndex(u => u.Address)
                        .IsUnique();

            modelBuilder.Entity<BitcoinTalkProfile>()
                        .HasIndex(u => u.BctId)
                        .IsUnique();

            modelBuilder.Entity<BitcoinTalkProfile>()
                        .HasIndex(u => u.Location)
                        .IsUnique();

            modelBuilder.Entity<TelegramProfile>()
                        .HasIndex(u => u.TelegramId)
                        .IsUnique();

            modelBuilder.Entity<TwitterProfile>()
                        .HasIndex(u => u.TwitterId)
                        .IsUnique();

            modelBuilder.Entity<Referral>()
                       .HasIndex(x => x.Code)
                       .IsUnique();
        }

        public Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public void SetModified(Entity entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public DbSet<BlockchainUser> BlockchainUsers { get; private set; }
        public DbSet<BlockchainInfo> BlockchainInfos { get; private set; }
        public DbSet<Airdrop> Airdrops { get; private set; }
        public DbSet<Token> Tokens { get; private set; }
        public DbSet<SmartContract> SmartContracts { get; private set; }
        public DbSet<Subscriber> Subscribers { get; private set; }
        public DbSet<ApiUser> ApiUsers { get; private set; }
        public DbSet<UserIdentity> UserIdentities { get; private set; }
        public DbSet<BitcoinTalkProfile> BitcoinTalkProfiles { get; private set; }
        public DbSet<Translation> Translations { get; private set; }
        public DbSet<BuyTokensResult> BuyTokensResults { get; private set; }
        public DbSet<TransactionReceiptData> TransactionReceipts { get; private set; }
        public DbSet<UserAirdrops> UserAirdrops { get; private set; }
        public DbSet<AirdropSubscription> AirdropSubscriptions { get; private set; }
        public DbSet<AirdropSubscriber> AirdropSubscribers { get; private set; }
        public DbSet<AirdropDefinition> AirdropDefinitions { get; private set; }
        public DbSet<TwitterAirdropRequirement> TwitterAirdropRequirements { get; private set; }
        public DbSet<TelegramAirdropRequirement> TelegramAirdropRequirements { get; private set; }
        public DbSet<BitcoinTalkAirdropRequirement> BitcoinTalkAirdropRequirements { get; private set; }
        public DbSet<DiscordAirdropRequirement> DiscordAirdropRequirements { get; private set; }
        public DbSet<TwitterProfile> TwitterProfiles { get; private set; }
        public DbSet<TelegramProfile> TelegramProfiles { get; private set; }
        public DbSet<DiscordProfile> DiscordProfiles { get; private set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Achievements> Achievements { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<TwitterConfig> TwitterConfigs { get; set; }

        public bool Disposed { get; private set; }

        public override void Dispose()
        {
            Disposed = true;
            base.Dispose();
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CoinstantineContext>
    {
        public CoinstantineContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CoinstantineContext>();
             //builder.UseSqlite("Data Source=CoinstantineDB.db");
            builder.UseSqlServer("Server=tcp:coinstantine.database.windows.net,1433;Initial Catalog=CoinstantineDBAcc;Persist Security Info=False;User ID=cirta;Password=$Oleil12ZA!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            return new CoinstantineContext(builder.Options);
        }
    }
}

