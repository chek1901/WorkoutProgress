using Microsoft.EntityFrameworkCore.Migrations;

namespace AwoAppServices.Migrations
{
    public partial class Extended_identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GymUserId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GymUserId",
                table: "AspNetUsers");
        }
    }
}
