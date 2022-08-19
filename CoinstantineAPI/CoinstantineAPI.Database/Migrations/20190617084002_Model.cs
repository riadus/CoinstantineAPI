using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "UserIdentities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OsVersion",
                table: "UserIdentities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model",
                table: "UserIdentities");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "UserIdentities");
        }
    }
}
