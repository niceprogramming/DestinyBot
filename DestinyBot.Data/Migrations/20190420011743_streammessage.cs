using Microsoft.EntityFrameworkCore.Migrations;

namespace DestinyBot.Data.Migrations
{
    public partial class streammessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "Message",
                "TwitchSubscriptions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Message",
                "TwitchSubscriptions");
        }
    }
}