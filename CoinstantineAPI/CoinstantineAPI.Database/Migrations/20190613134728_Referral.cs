using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class Referral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGame",
                table: "AirdropDefinitions");

            migrationBuilder.AddColumn<string>(
                name: "ReferralCode",
                table: "UserIdentities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TweetType",
                table: "Tweets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReferralId",
                table: "ApiUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AirdropType",
                table: "AirdropDefinitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReferralAward",
                table: "AirdropDefinitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    GodFatherId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referrals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Referrals_ApiUsers_GodFatherId",
                        column: x => x.GodFatherId,
                        principalTable: "ApiUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiUsers_ReferralId",
                table: "ApiUsers",
                column: "ReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_Code",
                table: "Referrals",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_GodFatherId",
                table: "Referrals",
                column: "GodFatherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiUsers_Referrals_ReferralId",
                table: "ApiUsers",
                column: "ReferralId",
                principalTable: "Referrals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiUsers_Referrals_ReferralId",
                table: "ApiUsers");

            migrationBuilder.DropTable(
                name: "Referrals");

            migrationBuilder.DropIndex(
                name: "IX_ApiUsers_ReferralId",
                table: "ApiUsers");

            migrationBuilder.DropColumn(
                name: "ReferralCode",
                table: "UserIdentities");

            migrationBuilder.DropColumn(
                name: "TweetType",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "ReferralId",
                table: "ApiUsers");

            migrationBuilder.DropColumn(
                name: "AirdropType",
                table: "AirdropDefinitions");

            migrationBuilder.DropColumn(
                name: "ReferralAward",
                table: "AirdropDefinitions");

            migrationBuilder.AddColumn<bool>(
                name: "IsGame",
                table: "AirdropDefinitions",
                nullable: false,
                defaultValue: false);
        }
    }
}
