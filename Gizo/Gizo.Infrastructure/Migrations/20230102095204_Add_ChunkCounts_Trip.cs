using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Add_ChunkCounts_Trip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GpsChunkCount",
                schema: "Trip",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImuChunkCount",
                schema: "Trip",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VideoChunkCount",
                schema: "Trip",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GpsChunkCount",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ImuChunkCount",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "VideoChunkCount",
                schema: "Trip",
                table: "Trips");
        }
    }
}
