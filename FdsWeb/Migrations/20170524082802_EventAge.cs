using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FdsWeb.Migrations
{
    public partial class EventAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "AgeMax",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "AgeMin",
                table: "Events",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeMax",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AgeMin",
                table: "Events");
        }
    }
}
