using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Trip_Change_Structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoFilePath",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.AddColumn<string>(
                name: "ChunkId",
                schema: "Trip",
                table: "TripTempVideos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ChunkSize",
                schema: "Trip",
                table: "Trips",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "FilesPath",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GpuFileName",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImuFileName",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChunkId",
                schema: "Trip",
                table: "TripTempVideos");

            migrationBuilder.DropColumn(
                name: "FilesPath",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "GpuFileName",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ImuFileName",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChunkSize",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "VideoFilePath",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
