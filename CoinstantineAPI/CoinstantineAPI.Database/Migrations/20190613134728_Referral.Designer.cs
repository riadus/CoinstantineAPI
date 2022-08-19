﻿// <auto-generated />
using System;
using CoinstantineAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoinstantineAPI.Database.Migrations
{
    [DbContext(typeof(CoinstantineContext))]
    [Migration("20190613134728_Referral")]
    partial class Referral
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoinstantineAPI.Data.Achievements", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApiUserId");

                    b.Property<int?>("GameId");

                    b.HasKey("Id");

                    b.HasIndex("ApiUserId");

                    b.HasIndex("GameId");

                    b.ToTable("Achievements");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Airdrop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AirdropId");

                    b.Property<byte[]>("AirdropIdBytes");

                    b.Property<int>("Amount");

                    b.Property<int?>("CreatorId");

                    b.Property<int>("NumberOfUsers");

                    b.Property<int?>("TokenId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TokenId")
                        .IsUnique()
                        .HasFilter("[TokenId] IS NOT NULL");

                    b.ToTable("Airdrops");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.AirdropDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AirdropName");

                    b.Property<int>("AirdropType");

                    b.Property<int>("Amount");

                    b.Property<int?>("BitcoinTalkAirdropRequirementId");

                    b.Property<int?>("DiscordAirdropRequirementId");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<int>("MaxLimit");

                    b.Property<string>("OtherInfoToDisplay");

                    b.Property<int>("ReferralAward");

                    b.Property<DateTime>("StartDate");

                    b.Property<int?>("TelegramAirdropRequirementId");

                    b.Property<string>("TokenName");

                    b.Property<int?>("TwitterAirdropRequirementId");

                    b.HasKey("Id");

                    b.HasIndex("BitcoinTalkAirdropRequirementId")
                        .IsUnique()
                        .HasFilter("[BitcoinTalkAirdropRequirementId] IS NOT NULL");

                    b.HasIndex("DiscordAirdropRequirementId")
                        .IsUnique()
                        .HasFilter("[DiscordAirdropRequirementId] IS NOT NULL");

                    b.HasIndex("TelegramAirdropRequirementId")
                        .IsUnique()
                        .HasFilter("[TelegramAirdropRequirementId] IS NOT NULL");

                    b.HasIndex("TwitterAirdropRequirementId")
                        .IsUnique()
                        .HasFilter("[TwitterAirdropRequirementId] IS NOT NULL");

                    b.ToTable("AirdropDefinitions");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.AirdropSubscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AirdropSubscriptionId");

                    b.Property<int>("Status");

                    b.Property<DateTime>("SubscribtionDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AirdropSubscriptionId");

                    b.ToTable("AirdropSubscribers");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.AirdropSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AirdropDefinitionId");

                    b.HasKey("Id");

                    b.HasIndex("AirdropDefinitionId")
                        .IsUnique();

                    b.ToTable("AirdropSubscriptions");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.ApiUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BctProfileId");

                    b.Property<int?>("BlockchainInfoId");

                    b.Property<string>("Culture");

                    b.Property<string>("DeviceId");

                    b.Property<string>("Email");

                    b.Property<string>("Phonenumber");

                    b.Property<int?>("ReferralId");

                    b.Property<int?>("TelegramId");

                    b.Property<int?>("TwitterProfileId");

                    b.Property<string>("UniqueId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("BctProfileId")
                        .IsUnique()
                        .HasFilter("[BctProfileId] IS NOT NULL");

                    b.HasIndex("BlockchainInfoId")
                        .IsUnique()
                        .HasFilter("[BlockchainInfoId] IS NOT NULL");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("ReferralId");

                    b.HasIndex("TelegramId")
                        .IsUnique()
                        .HasFilter("[TelegramId] IS NOT NULL");

                    b.HasIndex("TwitterProfileId")
                        .IsUnique()
                        .HasFilter("[TwitterProfileId] IS NOT NULL");

                    b.ToTable("ApiUsers");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApplicationId");

                    b.Property<string>("ApplicationSecret");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.BitcoinTalkAirdropRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ExactRank");

                    b.Property<bool>("HasAccount");

                    b.Property<int>("MinimumActivity");

                    b.Property<DateTime?>("MinimumCreationDate");

                    b.Property<int>("MinimumPosts");

                    b.Property<int?>("MinimumRank");

                    b.HasKey("Id");

                    b.ToTable("BitcoinTalkAirdropRequirements");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.BitcoinTalkProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Activity");

                    b.Property<int>("Age");

                    b.Property<string>("Aim");

                    b.Property<int>("BctId");

                    b.Property<string>("Email");

                    b.Property<string>("Icq");

                    b.Property<DateTime>("LastActive");

                    b.Property<string>("Location");

                    b.Property<string>("Msn");

                    b.Property<int>("Position");

                    b.Property<int>("Posts");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("Trust");

                    b.Property<string>("Username");

                    b.Property<bool>("Validated");

                    b.Property<DateTime?>("ValidationDate");

                    b.Property<string>("Website");

                    b.Property<string>("Yim");

                    b.HasKey("Id");

                    b.HasIndex("BctId")
                        .IsUnique();

                    b.HasIndex("Location")
                        .IsUnique()
                        .HasFilter("[Location] IS NOT NULL");

                    b.ToTable("BitcoinTalkProfiles");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.BlockchainInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<float>("Coinstantine");

                    b.Property<float>("Ether");

                    b.HasKey("Id");

                    b.HasIndex("Address")
                        .IsUnique()
                        .HasFilter("[Address] IS NOT NULL");

                    b.ToTable("BlockchainInfos");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.BlockchainUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("Json");

                    b.Property<string>("PassPhrase");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("BlockchainUsers");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.BuyTokensResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AmountBought");

                    b.Property<int>("BuyerId");

                    b.Property<DateTime>("PurchaseDate");

                    b.Property<int>("SaleType");

                    b.Property<int?>("TransactionReceiptId");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.HasIndex("TransactionReceiptId")
                        .IsUnique()
                        .HasFilter("[TransactionReceiptId] IS NOT NULL");

                    b.ToTable("BuyTokensResults");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.DiscordAirdropRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("NeedsToJoinServer");

                    b.Property<string>("ServerName");

                    b.Property<string>("ServerUrl");

                    b.HasKey("Id");

                    b.ToTable("DiscordAirdropRequirements");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.DiscordProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApiUserId");

                    b.Property<string>("DiscordChannelUrl");

                    b.Property<string>("DiscordServerName");

                    b.Property<string>("DiscordUserIdentifier");

                    b.Property<DateTime?>("JoinedDate");

                    b.Property<string>("Username");

                    b.Property<bool>("Validated");

                    b.Property<DateTime?>("ValidationDate");

                    b.HasKey("Id");

                    b.HasIndex("ApiUserId");

                    b.ToTable("DiscordProfiles");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AzureFilename");

                    b.Property<bool>("DocumentAvailableOnline");

                    b.Property<DateTime>("DownloadDate");

                    b.Property<string>("Filename");

                    b.Property<DateTime>("LastCheck");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Path");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AirdropDefinitionId");

                    b.HasKey("Id");

                    b.HasIndex("AirdropDefinitionId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Referral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<int?>("GodFatherId");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasFilter("[Code] IS NOT NULL");

                    b.HasIndex("GodFatherId");

                    b.ToTable("Referrals");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.RefreshTokens", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApplicationId");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<DateTime>("GenerationDate");

                    b.Property<string>("RefreshToken");

                    b.Property<string>("RefreshTokenSalt");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.SmartContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abi");

                    b.Property<string>("Address");

                    b.Property<string>("Name");

                    b.Property<int?>("TokenId");

                    b.HasKey("Id");

                    b.HasIndex("TokenId")
                        .IsUnique()
                        .HasFilter("[TokenId] IS NOT NULL");

                    b.ToTable("SmartContracts");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AirdropId");

                    b.Property<bool>("Airdroped");

                    b.Property<int>("AmountToReceive");

                    b.Property<int?>("BlockchainUserId");

                    b.Property<bool>("Blocked");

                    b.Property<bool>("HasWithdrawn");

                    b.Property<string>("Identifier");

                    b.Property<byte[]>("IdentifierBytes");

                    b.Property<bool>("Validated");

                    b.HasKey("Id");

                    b.HasIndex("AirdropId");

                    b.HasIndex("BlockchainUserId");

                    b.ToTable("Subscribers");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.TelegramAirdropRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("HasAccount");

                    b.HasKey("Id");

                    b.ToTable("TelegramAirdropRequirements");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.TelegramProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<long>("TelegramId");

                    b.Property<string>("Username");

                    b.Property<bool>("Validated");

                    b.Property<DateTime?>("ValidationDate");

                    b.HasKey("Id");

                    b.HasIndex("TelegramId")
                        .IsUnique();

                    b.ToTable("TelegramProfiles");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int>("Decimals");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerAddress");

                    b.Property<int>("Supply");

                    b.Property<string>("Symbol");

                    b.HasKey("Id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.TransactionReceiptData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BlockHash");

                    b.Property<string>("BlockNumber");

                    b.Property<string>("ContractAddress");

                    b.Property<string>("CumulativeGasUsed");

                    b.Property<string>("GasUsed");

                    b.Property<string>("Status");

                    b.Property<string>("TransactionHash");

                    b.Property<string>("TransactionIndex");

                    b.HasKey("Id");

                    b.ToTable("TransactionReceipts");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Translation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments");

                    b.Property<string>("Key");

                    b.Property<string>("Language");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Translations");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Tweet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AvailableFrom");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("IsTranslationKey");

                    b.Property<string>("Language");

                    b.Property<string>("Text");

                    b.Property<int>("TweetType");

                    b.HasKey("Id");

                    b.ToTable("Tweets");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.TwitterAirdropRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("HasAccount");

                    b.Property<DateTime?>("MinimumCreationDate");

                    b.Property<int>("MinimumFollowers");

                    b.HasKey("Id");

                    b.ToTable("TwitterAirdropRequirements");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.TwitterProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("NumberOfFollower");

                    b.Property<string>("ScreenName");

                    b.Property<long>("TwitterId");

                    b.Property<string>("Username");

                    b.Property<bool>("Validated");

                    b.Property<DateTime?>("ValidationDate");

                    b.HasKey("Id");

                    b.HasIndex("TwitterId")
                        .IsUnique();

                    b.ToTable("TwitterProfiles");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.UserAchievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Achieved");

                    b.Property<DateTime>("AchievementDate");

                    b.Property<string>("AchievementName");

                    b.Property<int?>("AchievementsId");

                    b.Property<string>("Description");

                    b.Property<string>("Giver");

                    b.Property<int>("PercentageDone");

                    b.Property<string>("Source");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("AchievementsId");

                    b.ToTable("UserAchievements");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.UserAirdrops", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AirdropIdsAsString")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserAirdrops");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.UserIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int?>("ApiUserId");

                    b.Property<string>("City");

                    b.Property<string>("ConfirmationCode");

                    b.Property<DateTime>("ConfirmationDate");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DoB");

                    b.Property<string>("EmailAddress");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("PasswordSalt");

                    b.Property<string>("PostalCode");

                    b.Property<string>("Province");

                    b.Property<string>("ReferralCode");

                    b.Property<int>("Role");

                    b.Property<DateTime>("SubscriptionDate");

                    b.Property<string>("UserId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("ApiUserId")
                        .IsUnique()
                        .HasFilter("[ApiUserId] IS NOT NULL");

                    b.HasIndex("EmailAddress")
                        .IsUnique()
                        .HasFilter("[EmailAddress] IS NOT NULL");

                    b.ToTable("UserIdentities");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Achievements", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.ApiUser", "ApiUser")
                        .WithMany()
                        .HasForeignKey("ApiUserId");

                    b.HasOne("CoinstantineAPI.Data.Game")
                        .WithMany("Achievements")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Airdrop", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.BlockchainUser", "Creator")
                        .WithMany("Airdrops")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CoinstantineAPI.Data.Token", "Token")
                        .WithOne("Airdrop")
                        .HasForeignKey("CoinstantineAPI.Data.Airdrop", "TokenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoinstantineAPI.Data.AirdropDefinition", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.BitcoinTalkAirdropRequirement", "BitcoinTalkAirdropRequirement")
                        .WithOne("AirdropDefinition")
                        .HasForeignKey("CoinstantineAPI.Data.AirdropDefinition", "BitcoinTalkAirdropRequirementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CoinstantineAPI.Data.DiscordAirdropRequirement", "DiscordAirdropRequirement")
                        .WithOne("AirdropDefinition")
                        .HasForeignKey("CoinstantineAPI.Data.AirdropDefinition", "DiscordAirdropRequirementId");

                    b.HasOne("CoinstantineAPI.Data.TelegramAirdropRequirement", "TelegramAirdropRequirement")
                        .WithOne("AirdropDefinition")
                        .HasForeignKey("CoinstantineAPI.Data.AirdropDefinition", "TelegramAirdropRequirementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CoinstantineAPI.Data.TwitterAirdropRequirement", "TwitterAirdropRequirement")
                        .WithOne("AirdropDefinition")
                        .HasForeignKey("CoinstantineAPI.Data.AirdropDefinition", "TwitterAirdropRequirementId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoinstantineAPI.Data.AirdropSubscriber", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.AirdropSubscription", "AirdropSubscription")
                        .WithMany("Subscribers")
                        .HasForeignKey("AirdropSubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoinstantineAPI.Data.AirdropSubscription", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.AirdropDefinition")
                        .WithOne("AirdropSubscription")
                        .HasForeignKey("CoinstantineAPI.Data.AirdropSubscription", "AirdropDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoinstantineAPI.Data.ApiUser", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.BitcoinTalkProfile", "BctProfile")
                        .WithOne("ApiUser")
                        .HasForeignKey("CoinstantineAPI.Data.ApiUser", "BctProfileId");

                    b.HasOne("CoinstantineAPI.Data.BlockchainInfo", "BlockchainInfo")
                        .WithOne("ApiUser")
                        .HasForeignKey("CoinstantineAPI.Data.ApiUser", "BlockchainInfoId");

                    b.HasOne("CoinstantineAPI.Data.Referral")
                        .WithMany("Users")
                        .HasForeignKey("ReferralId");

                    b.HasOne("CoinstantineAPI.Data.TelegramProfile", "Telegram")
                        .WithOne("ApiUser")
                        .HasForeignKey("CoinstantineAPI.Data.ApiUser", "TelegramId");

                    b.HasOne("CoinstantineAPI.Data.TwitterProfile", "TwitterProfile")
                        .WithOne("ApiUser")
                        .HasForeignKey("CoinstantineAPI.Data.ApiUser", "TwitterProfileId");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.BuyTokensResult", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.TransactionReceiptData", "TransactionReceipt")
                        .WithOne("BuyTokensResult")
                        .HasForeignKey("CoinstantineAPI.Data.BuyTokensResult", "TransactionReceiptId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoinstantineAPI.Data.DiscordProfile", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.ApiUser", "ApiUser")
                        .WithMany("DiscordProfiles")
                        .HasForeignKey("ApiUserId");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Game", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.AirdropDefinition", "AirdropDefinition")
                        .WithMany()
                        .HasForeignKey("AirdropDefinitionId");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Referral", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.ApiUser", "GodFather")
                        .WithMany()
                        .HasForeignKey("GodFatherId");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.RefreshTokens", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.HasOne("CoinstantineAPI.Data.UserIdentity", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoinstantineAPI.Data.SmartContract", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.Token", "Token")
                        .WithOne("SmartContract")
                        .HasForeignKey("CoinstantineAPI.Data.SmartContract", "TokenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoinstantineAPI.Data.Subscriber", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.Airdrop", "Airdrop")
                        .WithMany("Subscribers")
                        .HasForeignKey("AirdropId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CoinstantineAPI.Data.BlockchainUser", "User")
                        .WithMany()
                        .HasForeignKey("BlockchainUserId");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.UserAchievement", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.Achievements")
                        .WithMany("UserAchievements")
                        .HasForeignKey("AchievementsId");
                });

            modelBuilder.Entity("CoinstantineAPI.Data.UserIdentity", b =>
                {
                    b.HasOne("CoinstantineAPI.Data.ApiUser", "RelatedUser")
                        .WithOne("UserIdentity")
                        .HasForeignKey("CoinstantineAPI.Data.UserIdentity", "ApiUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
