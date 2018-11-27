using Microsoft.EntityFrameworkCore.Migrations;

namespace DestinyBot.Data.Migrations
{
    public partial class current : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuildOwners",
                columns: table => new
                {
                    GuildId = table.Column<string>(nullable: false),
                    ChannelHub = table.Column<string>(nullable: true),
                    Snowflake = table.Column<string>(nullable: true),
                    YoutubeId = table.Column<string>(nullable: true),
                    TwitterId = table.Column<long>(nullable: false),
                    TwitchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildOwners", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeSubscriptions",
                columns: table => new
                {
                    DiscordChannelId = table.Column<string>(nullable: false),
                    ChannelId = table.Column<string>(nullable: false),
                    ChannelName = table.Column<string>(nullable: true),
                    LatestVideoDate = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeSubscriptions", x => new { x.ChannelId, x.DiscordChannelId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildOwners");

            migrationBuilder.DropTable(
                name: "YoutubeSubscriptions");
        }
    }
}
