using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "BlockchainUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "BlockchainInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "BitcoinTalkProfiles",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitterProfiles_TwitterId",
                table: "TwitterProfiles",
                column: "TwitterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramProfiles_TelegramId",
                table: "TelegramProfiles",
                column: "TelegramId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainUsers_Username",
                table: "BlockchainUsers",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainInfo_Address",
                table: "BlockchainInfo",
                column: "Address",
                unique: true,
                filter: "[Address] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinTalkProfiles_BctId",
                table: "BitcoinTalkProfiles",
                column: "BctId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinTalkProfiles_Location",
                table: "BitcoinTalkProfiles",
                column: "Location",
                unique: true,
                filter: "[Location] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TwitterProfiles_TwitterId",
                table: "TwitterProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TelegramProfiles_TelegramId",
                table: "TelegramProfiles");

            migrationBuilder.DropIndex(
                name: "IX_BlockchainUsers_Username",
                table: "BlockchainUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlockchainInfo_Address",
                table: "BlockchainInfo");

            migrationBuilder.DropIndex(
                name: "IX_BitcoinTalkProfiles_BctId",
                table: "BitcoinTalkProfiles");

            migrationBuilder.DropIndex(
                name: "IX_BitcoinTalkProfiles_Location",
                table: "BitcoinTalkProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "BlockchainUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "BlockchainInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "BitcoinTalkProfiles",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
