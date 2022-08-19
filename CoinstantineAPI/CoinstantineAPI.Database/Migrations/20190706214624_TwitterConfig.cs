using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinstantineAPI.Database.Migrations
{
    public partial class TwitterConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwitterConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Follow = table.Column<bool>(nullable: false),
                    AccountToFollow = table.Column<long>(nullable: false),
                    Tweet = table.Column<bool>(nullable: false),
                    Retweet = table.Column<bool>(nullable: false),
                    TweetIdToRetweet = table.Column<long>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    AccessTokenSecret = table.Column<string>(nullable: true),
                    UseIntents = table.Column<bool>(nullable: false),
                    UseFollowIntent = table.Column<bool>(nullable: false),
                    UseTweetIntent = table.Column<bool>(nullable: false),
                    UseRetweetIntent = table.Column<bool>(nullable: false),
                    FollowIntent = table.Column<string>(nullable: true),
                    TweetIntent = table.Column<string>(nullable: true),
                    RetweetIntent = table.Column<string>(nullable: true),
                    BaseFollowIntent = table.Column<string>(nullable: true),
                    BaseTweetIntent = table.Column<string>(nullable: true),
                    BaseRetweetIntent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterConfigs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitterConfigs");
        }
    }
}
