using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Update_UserCarModel_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCarModels",
                schema: "User",
                table: "UserCarModels");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                schema: "User",
                table: "UserCarModels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCarModels",
                schema: "User",
                table: "UserCarModels",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCarModels_CarModelId",
                schema: "User",
                table: "UserCarModels",
                column: "CarModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCarModels",
                schema: "User",
                table: "UserCarModels");

            migrationBuilder.DropIndex(
                name: "IX_UserCarModels_CarModelId",
                schema: "User",
                table: "UserCarModels");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "User",
                table: "UserCarModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCarModels",
                schema: "User",
                table: "UserCarModels",
                column: "CarModelId");
        }
    }
}
