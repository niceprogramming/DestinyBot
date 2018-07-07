using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LittleSteve.Data.Migrations
{
    public partial class AddedTwitchEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "TwitterAlerts");

            migrationBuilder.AddColumn<long>(
                "TwitchStreamerId",
                "GuildOwners",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                "TwitchStreamers",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    SteamStartTime = table.Column<DateTimeOffset>(nullable: false),
                    StreamEndTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_TwitchStreamers", x => x.Id); });

            migrationBuilder.CreateTable(
                "TwitterAlertSubscriptions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    TwitterUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterAlertSubscriptions", x => x.Id);
                    table.ForeignKey(
                        "FK_TwitterAlertSubscriptions_TwitterUsers_TwitterUserId",
                        x => x.TwitterUserId,
                        "TwitterUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TwitchAlertSubscriptions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    TwitchStreamerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchAlertSubscriptions", x => x.Id);
                    table.ForeignKey(
                        "FK_TwitchAlertSubscriptions_TwitchStreamers_TwitchStreamerId",
                        x => x.TwitchStreamerId,
                        "TwitchStreamers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_GuildOwners_TwitchStreamerId",
                "GuildOwners",
                "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                "IX_TwitchAlertSubscriptions_TwitchStreamerId",
                "TwitchAlertSubscriptions",
                "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                "IX_TwitterAlertSubscriptions_TwitterUserId",
                "TwitterAlertSubscriptions",
                "TwitterUserId");

            migrationBuilder.AddForeignKey(
                "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                "GuildOwners",
                "TwitchStreamerId",
                "TwitchStreamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                "GuildOwners");

            migrationBuilder.DropTable(
                "TwitchAlertSubscriptions");

            migrationBuilder.DropTable(
                "TwitterAlertSubscriptions");

            migrationBuilder.DropTable(
                "TwitchStreamers");

            migrationBuilder.DropIndex(
                "IX_GuildOwners_TwitchStreamerId",
                "GuildOwners");

            migrationBuilder.DropColumn(
                "TwitchStreamerId",
                "GuildOwners");

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
                "IX_TwitterAlerts_TwitterUserId",
                "TwitterAlerts",
                "TwitterUserId");
        }
    }
}