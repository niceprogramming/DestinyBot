using Microsoft.EntityFrameworkCore.Migrations;

namespace DestinyBot.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Channels",
                table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LatestVideoDate = table.Column<long>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Channels", x => x.Id); });

            migrationBuilder.CreateTable(
                "GuildOwners",
                table => new
                {
                    GuildId = table.Column<string>(nullable: false),
                    ChannelHub = table.Column<string>(nullable: true),
                    Snowflake = table.Column<string>(nullable: true),
                    YoutubeId = table.Column<string>(nullable: true),
                    TwitterId = table.Column<long>(nullable: false),
                    TwitchId = table.Column<long>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_GuildOwners", x => x.GuildId); });

            migrationBuilder.CreateTable(
                "Reminders",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreated = table.Column<long>(nullable: false),
                    TimeToRemind = table.Column<long>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ChannelId = table.Column<long>(nullable: false),
                    GuildId = table.Column<long>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Reminders", x => x.Id); });

            migrationBuilder.CreateTable(
                "TwitchStreamers",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    SteamStartTime = table.Column<long>(nullable: false),
                    StreamEndTime = table.Column<long>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_TwitchStreamers", x => x.Id); });

            migrationBuilder.CreateTable(
                "YoutubeSubscriptions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordChannelId = table.Column<string>(nullable: true),
                    YoutubeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeSubscriptions", x => x.Id);
                    table.ForeignKey(
                        "FK_YoutubeSubscriptions_Channels_YoutubeId",
                        x => x.YoutubeId,
                        "Channels",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Games",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<long>(nullable: false),
                    EndTime = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TwitchStreamerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        "FK_Games_TwitchStreamers_TwitchStreamerId",
                        x => x.TwitchStreamerId,
                        "TwitchStreamers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TwitchSubscriptions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordChannelId = table.Column<string>(nullable: true),
                    TwitchStreamerId = table.Column<long>(nullable: false),
                    AlternateLink = table.Column<string>(nullable: true),
                    MessageId = table.Column<long>(nullable: false),
                    ShouldPin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchSubscriptions", x => x.Id);
                    table.ForeignKey(
                        "FK_TwitchSubscriptions_TwitchStreamers_TwitchStreamerId",
                        x => x.TwitchStreamerId,
                        "TwitchStreamers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Games_TwitchStreamerId",
                "Games",
                "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                "IX_TwitchSubscriptions_TwitchStreamerId",
                "TwitchSubscriptions",
                "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                "IX_YoutubeSubscriptions_YoutubeId",
                "YoutubeSubscriptions",
                "YoutubeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Games");

            migrationBuilder.DropTable(
                "GuildOwners");

            migrationBuilder.DropTable(
                "Reminders");

            migrationBuilder.DropTable(
                "TwitchSubscriptions");

            migrationBuilder.DropTable(
                "YoutubeSubscriptions");

            migrationBuilder.DropTable(
                "TwitchStreamers");

            migrationBuilder.DropTable(
                "Channels");
        }
    }
}