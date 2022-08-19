using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class Source : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Province",
                table: "UserIdentities",
                newName: "Source");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Source",
                table: "UserIdentities",
                newName: "Province");
        }
    }
}
