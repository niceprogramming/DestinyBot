using Microsoft.EntityFrameworkCore.Migrations;

namespace DestinyBot.Data.Migrations
{
    public partial class twitter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LatestVideoDate = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildOwners",
                columns: table => new
                {
                    GuildId = table.Column<string>(nullable: false),
                    ChannelHub = table.Column<string>(nullable: true),
                    Snowflake = table.Column<string>(nullable: true),
                    YoutubeId = table.Column<string>(nullable: true),
                    TwitterUserId = table.Column<long>(nullable: false),
                    TwitchId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildOwners", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreated = table.Column<long>(nullable: false),
                    TimeToRemind = table.Column<long>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ChannelId = table.Column<long>(nullable: false),
                    GuildId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitchStreamers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    SteamStartTime = table.Column<long>(nullable: false),
                    StreamEndTime = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchStreamers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    ScreenName = table.Column<string>(nullable: true),
                    LastTweetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeSubscriptions",
                columns: table => new
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
                        name: "FK_YoutubeSubscriptions_Channels_YoutubeId",
                        column: x => x.YoutubeId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
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
                        name: "FK_Games_TwitchStreamers_TwitchStreamerId",
                        column: x => x.TwitchStreamerId,
                        principalTable: "TwitchStreamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitchSubscriptions",
                columns: table => new
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
                        name: "FK_TwitchSubscriptions_TwitchStreamers_TwitchStreamerId",
                        column: x => x.TwitchStreamerId,
                        principalTable: "TwitchStreamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitterSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    TwitterUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitterSubscriptions_TwitterUsers_TwitterUserId",
                        column: x => x.TwitterUserId,
                        principalTable: "TwitterUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_TwitchStreamerId",
                table: "Games",
                column: "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchSubscriptions_TwitchStreamerId",
                table: "TwitchSubscriptions",
                column: "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterSubscriptions_TwitterUserId",
                table: "TwitterSubscriptions",
                column: "TwitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterUsers_LastTweetId",
                table: "TwitterUsers",
                column: "LastTweetId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeSubscriptions_YoutubeId",
                table: "YoutubeSubscriptions",
                column: "YoutubeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GuildOwners");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "TwitchSubscriptions");

            migrationBuilder.DropTable(
                name: "TwitterSubscriptions");

            migrationBuilder.DropTable(
                name: "YoutubeSubscriptions");

            migrationBuilder.DropTable(
                name: "TwitchStreamers");

            migrationBuilder.DropTable(
                name: "TwitterUsers");

            migrationBuilder.DropTable(
                name: "Channels");
        }
    }
}
