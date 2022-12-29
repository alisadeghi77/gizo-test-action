using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Rename_CarModel_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarBrands_CarId",
                schema: "CarBrand",
                table: "CarModels");

            migrationBuilder.RenameColumn(
                name: "ModelName",
                schema: "CarBrand",
                table: "CarModels",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CarId",
                schema: "CarBrand",
                table: "CarModels",
                newName: "CarBrandId");

            migrationBuilder.RenameIndex(
                name: "IX_CarModels_CarId",
                schema: "CarBrand",
                table: "CarModels",
                newName: "IX_CarModels_CarBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                schema: "CarBrand",
                table: "CarModels",
                column: "CarBrandId",
                principalSchema: "CarBrand",
                principalTable: "CarBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                schema: "CarBrand",
                table: "CarModels");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "CarBrand",
                table: "CarModels",
                newName: "ModelName");

            migrationBuilder.RenameColumn(
                name: "CarBrandId",
                schema: "CarBrand",
                table: "CarModels",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_CarModels_CarBrandId",
                schema: "CarBrand",
                table: "CarModels",
                newName: "IX_CarModels_CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarBrands_CarId",
                schema: "CarBrand",
                table: "CarModels",
                column: "CarId",
                principalSchema: "CarBrand",
                principalTable: "CarBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
