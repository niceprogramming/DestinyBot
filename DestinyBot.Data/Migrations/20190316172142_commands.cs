using Microsoft.EntityFrameworkCore.Migrations;

namespace DestinyBot.Data.Migrations
{
    public partial class commands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "CustomCommands",
                table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Aliases = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CustomCommands", x => x.Name); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "CustomCommands");
        }
    }
}