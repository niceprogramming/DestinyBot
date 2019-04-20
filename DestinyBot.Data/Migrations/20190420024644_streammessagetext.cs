using Microsoft.EntityFrameworkCore.Migrations;

namespace DestinyBot.Data.Migrations
{
    public partial class streammessagetext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "MessageText",
                "TwitchSubscriptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "MessageText",
                "TwitchSubscriptions");
        }
    }
}