using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class Referral2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiUsers_Referrals_ReferralId",
                table: "ApiUsers");

            migrationBuilder.RenameColumn(
                name: "ReferralId",
                table: "ApiUsers",
                newName: "ApiUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApiUsers_ReferralId",
                table: "ApiUsers",
                newName: "IX_ApiUsers_ApiUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiUsers_Referrals_ApiUserId",
                table: "ApiUsers",
                column: "ApiUserId",
                principalTable: "Referrals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiUsers_Referrals_ApiUserId",
                table: "ApiUsers");

            migrationBuilder.RenameColumn(
                name: "ApiUserId",
                table: "ApiUsers",
                newName: "ReferralId");

            migrationBuilder.RenameIndex(
                name: "IX_ApiUsers_ApiUserId",
                table: "ApiUsers",
                newName: "IX_ApiUsers_ReferralId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiUsers_Referrals_ReferralId",
                table: "ApiUsers",
                column: "ReferralId",
                principalTable: "Referrals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
