using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LittleSteve.Data.Migrations
{
    public partial class AlternateStreamLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlternateStreamLink",
                table: "TwitchStreamers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlternateStreamLink",
                table: "TwitchStreamers");
        }
    }
}
