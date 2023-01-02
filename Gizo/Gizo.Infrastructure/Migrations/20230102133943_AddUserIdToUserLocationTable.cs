using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class AddUserIdToUserLocationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "User",
                table: "UserLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_UserId",
                schema: "User",
                table: "UserLocations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocations_AspNetUsers_UserId",
                schema: "User",
                table: "UserLocations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLocations_AspNetUsers_UserId",
                schema: "User",
                table: "UserLocations");

            migrationBuilder.DropIndex(
                name: "IX_UserLocations_UserId",
                schema: "User",
                table: "UserLocations");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "User",
                table: "UserLocations");
        }
    }
}
