using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LittleSteve.Data.Migrations
{
    public partial class AddedYoutuberAndAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "YoutuberId",
                "GuildOwners",
                nullable: true);

            migrationBuilder.CreateTable(
                "Youtubers",
                table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LatestVideoDate = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Youtubers", x => x.Id); });

            migrationBuilder.CreateTable(
                "YoutubeAlertSubscriptions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    MessageId = table.Column<long>(nullable: false),
                    YoutuberId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeAlertSubscriptions", x => x.Id);
                    table.ForeignKey(
                        "FK_YoutubeAlertSubscriptions_Youtubers_YoutuberId",
                        x => x.YoutuberId,
                        "Youtubers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_GuildOwners_YoutuberId",
                "GuildOwners",
                "YoutuberId");

            migrationBuilder.CreateIndex(
                "IX_YoutubeAlertSubscriptions_YoutuberId",
                "YoutubeAlertSubscriptions",
                "YoutuberId");

            migrationBuilder.AddForeignKey(
                "FK_GuildOwners_Youtubers_YoutuberId",
                "GuildOwners",
                "YoutuberId",
                "Youtubers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_GuildOwners_Youtubers_YoutuberId",
                "GuildOwners");

            migrationBuilder.DropTable(
                "YoutubeAlertSubscriptions");

            migrationBuilder.DropTable(
                "Youtubers");

            migrationBuilder.DropIndex(
                "IX_GuildOwners_YoutuberId",
                "GuildOwners");

            migrationBuilder.DropColumn(
                "YoutuberId",
                "GuildOwners");
        }
    }
}