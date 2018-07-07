using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LittleSteve.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "TwitterUsers",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LastTweetId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ScreenName = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_TwitterUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                "GuildOwners",
                table => new
                {
                    DiscordId = table.Column<long>(nullable: false),
                    GuildId = table.Column<long>(nullable: false),
                    TwitterUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildOwners", x => new {x.DiscordId, x.GuildId});
                    table.ForeignKey(
                        "FK_GuildOwners_TwitterUsers_TwitterUserId",
                        x => x.TwitterUserId,
                        "TwitterUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TwitterAlerts",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    TwitterUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterAlerts", x => x.Id);
                    table.ForeignKey(
                        "FK_TwitterAlerts_TwitterUsers_TwitterUserId",
                        x => x.TwitterUserId,
                        "TwitterUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_GuildOwners_TwitterUserId",
                "GuildOwners",
                "TwitterUserId");

            migrationBuilder.CreateIndex(
                "IX_TwitterAlerts_TwitterUserId",
                "TwitterAlerts",
                "TwitterUserId");

            migrationBuilder.CreateIndex(
                "IX_TwitterUsers_LastTweetId",
                "TwitterUsers",
                "LastTweetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "GuildOwners");

            migrationBuilder.DropTable(
                "TwitterAlerts");

            migrationBuilder.DropTable(
                "TwitterUsers");
        }
    }
}