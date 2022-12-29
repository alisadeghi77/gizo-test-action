using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Rename_Cars_To_CarBrands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_Cars_CarId",
                schema: "Car",
                table: "CarModels");

            migrationBuilder.DropTable(
                name: "Cars",
                schema: "Car");

            migrationBuilder.DropTable(
                name: "UserCars",
                schema: "User");

            migrationBuilder.EnsureSchema(
                name: "CarBrand");

            migrationBuilder.RenameTable(
                name: "CarModels",
                schema: "Car",
                newName: "CarModels",
                newSchema: "CarBrand");

            migrationBuilder.CreateTable(
                name: "CarBrands",
                schema: "CarBrand",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCarModels",
                schema: "User",
                columns: table => new
                {
                    CarModelId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    License = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCarModels", x => x.CarModelId);
                    table.ForeignKey(
                        name: "FK_UserCarModels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCarModels_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalSchema: "CarBrand",
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCarModels_UserId",
                schema: "User",
                table: "UserCarModels",
                column: "UserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarBrands_CarId",
                schema: "CarBrand",
                table: "CarModels");

            migrationBuilder.DropTable(
                name: "CarBrands",
                schema: "CarBrand");

            migrationBuilder.DropTable(
                name: "UserCarModels",
                schema: "User");

            migrationBuilder.EnsureSchema(
                name: "Car");

            migrationBuilder.RenameTable(
                name: "CarModels",
                schema: "CarBrand",
                newName: "CarModels",
                newSchema: "Car");

            migrationBuilder.CreateTable(
                name: "Cars",
                schema: "Car",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCars",
                schema: "User",
                columns: table => new
                {
                    CarModelId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCars", x => x.CarModelId);
                    table.ForeignKey(
                        name: "FK_UserCars_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCars_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalSchema: "Car",
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCars_UserId",
                schema: "User",
                table: "UserCars",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_Cars_CarId",
                schema: "Car",
                table: "CarModels",
                column: "CarId",
                principalSchema: "Car",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
