using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationId = table.Column<string>(nullable: true),
                    ApplicationSecret = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinTalkAirdropRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HasAccount = table.Column<bool>(nullable: false),
                    MinimumPosts = table.Column<int>(nullable: false),
                    MinimumActivity = table.Column<int>(nullable: false),
                    MinimumRank = table.Column<int>(nullable: true),
                    ExactRank = table.Column<int>(nullable: true),
                    MinimumCreationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinTalkAirdropRequirements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinTalkProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BctId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Posts = table.Column<int>(nullable: false),
                    Activity = table.Column<int>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    LastActive = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Icq = table.Column<string>(nullable: true),
                    Aim = table.Column<string>(nullable: true),
                    Msn = table.Column<string>(nullable: true),
                    Yim = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Trust = table.Column<string>(nullable: true),
                    Validated = table.Column<bool>(nullable: false),
                    ValidationDate = table.Column<DateTime>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinTalkProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockchainInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ether = table.Column<float>(nullable: false),
                    Coinstantine = table.Column<float>(nullable: false),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockchainInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockchainUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    PassPhrase = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockchainUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    DownloadDate = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Filename = table.Column<string>(nullable: true),
                    LastCheck = table.Column<DateTime>(nullable: false),
                    DocumentAvailableOnline = table.Column<bool>(nullable: false),
                    AzureFilename = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelegramAirdropRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HasAccount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramAirdropRequirements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelegramProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    TelegramId = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Validated = table.Column<bool>(nullable: false),
                    ValidationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Symbol = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Supply = table.Column<int>(nullable: false),
                    Decimals = table.Column<int>(nullable: false),
                    OwnerAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionHash = table.Column<string>(nullable: true),
                    TransactionIndex = table.Column<string>(nullable: true),
                    BlockHash = table.Column<string>(nullable: true),
                    BlockNumber = table.Column<string>(nullable: true),
                    CumulativeGasUsed = table.Column<string>(nullable: true),
                    GasUsed = table.Column<string>(nullable: true),
                    ContractAddress = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionReceipts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterAirdropRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HasAccount = table.Column<bool>(nullable: false),
                    MinimumFollowers = table.Column<int>(nullable: false),
                    MinimumCreationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterAirdropRequirements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScreenName = table.Column<string>(nullable: true),
                    TwitterId = table.Column<long>(nullable: false),
                    NumberOfFollower = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Validated = table.Column<bool>(nullable: false),
                    ValidationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAirdrops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    AirdropIdsAsString = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAirdrops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Airdrops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: true),
                    TokenId = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    NumberOfUsers = table.Column<int>(nullable: false),
                    AirdropId = table.Column<string>(nullable: true),
                    AirdropIdBytes = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airdrops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airdrops_BlockchainUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "BlockchainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Airdrops_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartContracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abi = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    TokenId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartContracts_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyTokensResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountBought = table.Column<decimal>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    TransactionReceiptId = table.Column<int>(nullable: true),
                    PurchaseDate = table.Column<DateTime>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    SaleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyTokensResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyTokensResults_TransactionReceipts_TransactionReceiptId",
                        column: x => x.TransactionReceiptId,
                        principalTable: "TransactionReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirdropDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AirdropName = table.Column<string>(nullable: true),
                    TokenName = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    MaxLimit = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    OtherInfoToDisplay = table.Column<string>(nullable: true),
                    TwitterAirdropRequirementId = table.Column<int>(nullable: true),
                    TelegramAirdropRequirementId = table.Column<int>(nullable: true),
                    BitcoinTalkAirdropRequirementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirdropDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirdropDefinitions_BitcoinTalkAirdropRequirements_BitcoinTalkAirdropRequirementId",
                        column: x => x.BitcoinTalkAirdropRequirementId,
                        principalTable: "BitcoinTalkAirdropRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirdropDefinitions_TelegramAirdropRequirements_TelegramAirdropRequirementId",
                        column: x => x.TelegramAirdropRequirementId,
                        principalTable: "TelegramAirdropRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirdropDefinitions_TwitterAirdropRequirements_TwitterAirdropRequirementId",
                        column: x => x.TwitterAirdropRequirementId,
                        principalTable: "TwitterAirdropRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    DeviceId = table.Column<string>(nullable: true),
                    UniqueId = table.Column<string>(nullable: true),
                    TwitterProfileId = table.Column<int>(nullable: true),
                    TelegramId = table.Column<int>(nullable: true),
                    BctProfileId = table.Column<int>(nullable: true),
                    BlockchainInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiUsers_BitcoinTalkProfiles_BctProfileId",
                        column: x => x.BctProfileId,
                        principalTable: "BitcoinTalkProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiUsers_BlockchainInfo_BlockchainInfoId",
                        column: x => x.BlockchainInfoId,
                        principalTable: "BlockchainInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiUsers_TelegramProfiles_TelegramId",
                        column: x => x.TelegramId,
                        principalTable: "TelegramProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiUsers_TwitterProfiles_TwitterProfileId",
                        column: x => x.TwitterProfileId,
                        principalTable: "TwitterProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlockchainUserId = table.Column<int>(nullable: true),
                    AirdropId = table.Column<int>(nullable: true),
                    Validated = table.Column<bool>(nullable: false),
                    Blocked = table.Column<bool>(nullable: false),
                    Airdroped = table.Column<bool>(nullable: false),
                    HasWithdrawn = table.Column<bool>(nullable: false),
                    AmountToReceive = table.Column<int>(nullable: false),
                    IdentifierBytes = table.Column<byte[]>(nullable: true),
                    Identifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribers_Airdrops_AirdropId",
                        column: x => x.AirdropId,
                        principalTable: "Airdrops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscribers_BlockchainUsers_BlockchainUserId",
                        column: x => x.BlockchainUserId,
                        principalTable: "BlockchainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AirdropSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AirdropDefinitionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirdropSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirdropSubscriptions_AirdropDefinitions_AirdropDefinitionId",
                        column: x => x.AirdropDefinitionId,
                        principalTable: "AirdropDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIdentities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiUserId = table.Column<int>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    DoB = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    SubscriptionDate = table.Column<DateTime>(nullable: false),
                    ConfirmationCode = table.Column<string>(nullable: true),
                    ConfirmationDate = table.Column<DateTime>(nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIdentities_ApiUsers_ApiUserId",
                        column: x => x.ApiUserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirdropSubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    SubscribtionDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    AirdropSubscriptionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirdropSubscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirdropSubscribers_AirdropSubscriptions_AirdropSubscriptionId",
                        column: x => x.AirdropSubscriptionId,
                        principalTable: "AirdropSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenSalt = table.Column<string>(nullable: true),
                    ApplicationId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    GenerationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_UserIdentities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirdropDefinitions_BitcoinTalkAirdropRequirementId",
                table: "AirdropDefinitions",
                column: "BitcoinTalkAirdropRequirementId",
                unique: true,
                filter: "[BitcoinTalkAirdropRequirementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropDefinitions_TelegramAirdropRequirementId",
                table: "AirdropDefinitions",
                column: "TelegramAirdropRequirementId",
                unique: true,
                filter: "[TelegramAirdropRequirementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropDefinitions_TwitterAirdropRequirementId",
                table: "AirdropDefinitions",
                column: "TwitterAirdropRequirementId",
                unique: true,
                filter: "[TwitterAirdropRequirementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Airdrops_CreatorId",
                table: "Airdrops",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Airdrops_TokenId",
                table: "Airdrops",
                column: "TokenId",
                unique: true,
                filter: "[TokenId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropSubscribers_AirdropSubscriptionId",
                table: "AirdropSubscribers",
                column: "AirdropSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropSubscriptions_AirdropDefinitionId",
                table: "AirdropSubscriptions",
                column: "AirdropDefinitionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiUsers_BctProfileId",
                table: "ApiUsers",
                column: "BctProfileId",
                unique: true,
                filter: "[BctProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApiUsers_BlockchainInfoId",
                table: "ApiUsers",
                column: "BlockchainInfoId",
                unique: true,
                filter: "[BlockchainInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApiUsers_TelegramId",
                table: "ApiUsers",
                column: "TelegramId",
                unique: true,
                filter: "[TelegramId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApiUsers_TwitterProfileId",
                table: "ApiUsers",
                column: "TwitterProfileId",
                unique: true,
                filter: "[TwitterProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BuyTokensResults_TransactionReceiptId",
                table: "BuyTokensResults",
                column: "TransactionReceiptId",
                unique: true,
                filter: "[TransactionReceiptId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ApplicationId",
                table: "RefreshTokens",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartContracts_TokenId",
                table: "SmartContracts",
                column: "TokenId",
                unique: true,
                filter: "[TokenId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_AirdropId",
                table: "Subscribers",
                column: "AirdropId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_BlockchainUserId",
                table: "Subscribers",
                column: "BlockchainUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentities_ApiUserId",
                table: "UserIdentities",
                column: "ApiUserId",
                unique: true,
                filter: "[ApiUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirdropSubscribers");

            migrationBuilder.DropTable(
                name: "BuyTokensResults");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SmartContracts");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "UserAirdrops");

            migrationBuilder.DropTable(
                name: "AirdropSubscriptions");

            migrationBuilder.DropTable(
                name: "TransactionReceipts");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "UserIdentities");

            migrationBuilder.DropTable(
                name: "Airdrops");

            migrationBuilder.DropTable(
                name: "AirdropDefinitions");

            migrationBuilder.DropTable(
                name: "ApiUsers");

            migrationBuilder.DropTable(
                name: "BlockchainUsers");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "BitcoinTalkAirdropRequirements");

            migrationBuilder.DropTable(
                name: "TelegramAirdropRequirements");

            migrationBuilder.DropTable(
                name: "TwitterAirdropRequirements");

            migrationBuilder.DropTable(
                name: "BitcoinTalkProfiles");

            migrationBuilder.DropTable(
                name: "BlockchainInfo");

            migrationBuilder.DropTable(
                name: "TelegramProfiles");

            migrationBuilder.DropTable(
                name: "TwitterProfiles");
        }
    }
}
