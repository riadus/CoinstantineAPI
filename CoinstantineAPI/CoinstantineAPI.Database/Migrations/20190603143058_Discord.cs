using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class Discord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiUsers_BlockchainInfo_BlockchainInfoId",
                table: "ApiUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockchainInfo",
                table: "BlockchainInfo");

            migrationBuilder.RenameTable(
                name: "BlockchainInfo",
                newName: "BlockchainInfos");

            migrationBuilder.RenameIndex(
                name: "IX_BlockchainInfo_Address",
                table: "BlockchainInfos",
                newName: "IX_BlockchainInfos_Address");

            migrationBuilder.AddColumn<int>(
                name: "DiscordAirdropRequirementId",
                table: "AirdropDefinitions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGame",
                table: "AirdropDefinitions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockchainInfos",
                table: "BlockchainInfos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DiscordAirdropRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NeedsToJoinServer = table.Column<bool>(nullable: false),
                    ServerUrl = table.Column<string>(nullable: true),
                    ServerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordAirdropRequirements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    DiscordUserIdentifier = table.Column<string>(nullable: true),
                    Validated = table.Column<bool>(nullable: false),
                    ValidationDate = table.Column<DateTime>(nullable: true),
                    JoinedDate = table.Column<DateTime>(nullable: true),
                    DiscordChannelUrl = table.Column<string>(nullable: true),
                    DiscordServerName = table.Column<string>(nullable: true),
                    ApiUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordProfiles_ApiUsers_ApiUserId",
                        column: x => x.ApiUserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AirdropDefinitionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_AirdropDefinitions_AirdropDefinitionId",
                        column: x => x.AirdropDefinitionId,
                        principalTable: "AirdropDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiUserId = table.Column<int>(nullable: true),
                    GameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievements_ApiUsers_ApiUserId",
                        column: x => x.ApiUserId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Achievements_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAchievements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AchievementName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    AchievementDate = table.Column<DateTime>(nullable: false),
                    Achieved = table.Column<bool>(nullable: false),
                    PercentageDone = table.Column<int>(nullable: false),
                    Giver = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    AchievementsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements_AchievementsId",
                        column: x => x.AchievementsId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirdropDefinitions_DiscordAirdropRequirementId",
                table: "AirdropDefinitions",
                column: "DiscordAirdropRequirementId",
                unique: true,
                filter: "[DiscordAirdropRequirementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_ApiUserId",
                table: "Achievements",
                column: "ApiUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_GameId",
                table: "Achievements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordProfiles_ApiUserId",
                table: "DiscordProfiles",
                column: "ApiUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_AirdropDefinitionId",
                table: "Games",
                column: "AirdropDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_AchievementsId",
                table: "UserAchievements",
                column: "AchievementsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirdropDefinitions_DiscordAirdropRequirements_DiscordAirdropRequirementId",
                table: "AirdropDefinitions",
                column: "DiscordAirdropRequirementId",
                principalTable: "DiscordAirdropRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiUsers_BlockchainInfos_BlockchainInfoId",
                table: "ApiUsers",
                column: "BlockchainInfoId",
                principalTable: "BlockchainInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirdropDefinitions_DiscordAirdropRequirements_DiscordAirdropRequirementId",
                table: "AirdropDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiUsers_BlockchainInfos_BlockchainInfoId",
                table: "ApiUsers");

            migrationBuilder.DropTable(
                name: "DiscordAirdropRequirements");

            migrationBuilder.DropTable(
                name: "DiscordProfiles");

            migrationBuilder.DropTable(
                name: "UserAchievements");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_AirdropDefinitions_DiscordAirdropRequirementId",
                table: "AirdropDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockchainInfos",
                table: "BlockchainInfos");

            migrationBuilder.DropColumn(
                name: "DiscordAirdropRequirementId",
                table: "AirdropDefinitions");

            migrationBuilder.DropColumn(
                name: "IsGame",
                table: "AirdropDefinitions");

            migrationBuilder.RenameTable(
                name: "BlockchainInfos",
                newName: "BlockchainInfo");

            migrationBuilder.RenameIndex(
                name: "IX_BlockchainInfos_Address",
                table: "BlockchainInfo",
                newName: "IX_BlockchainInfo_Address");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockchainInfo",
                table: "BlockchainInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiUsers_BlockchainInfo_BlockchainInfoId",
                table: "ApiUsers",
                column: "BlockchainInfoId",
                principalTable: "BlockchainInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
