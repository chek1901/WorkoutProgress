using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AwoAppServices.Migrations
{
    public partial class extended_identityrole_datetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OriginDate",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginDate",
                table: "AspNetRoles");
        }
    }
}
