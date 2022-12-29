using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Add_Relation_between_trip_and_UserCarModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserCarModelId",
                schema: "Trip",
                table: "Trips",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserCarModelId",
                schema: "Trip",
                table: "Trips",
                column: "UserCarModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_UserCarModels_UserCarModelId",
                schema: "Trip",
                table: "Trips",
                column: "UserCarModelId",
                principalSchema: "User",
                principalTable: "UserCarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_UserCarModels_UserCarModelId",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_UserCarModelId",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "UserCarModelId",
                schema: "Trip",
                table: "Trips");
        }
    }
}
