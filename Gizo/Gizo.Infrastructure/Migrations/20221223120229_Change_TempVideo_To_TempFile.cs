using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Change_TempVideo_To_TempFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripTempVideos",
                schema: "Trip");

            migrationBuilder.AddColumn<string>(
                name: "TempFileName",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TripTempFiles",
                schema: "Trip",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChunkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripFileType = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTempFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripTempFiles_Trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "Trip",
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripTempFiles_TripId",
                schema: "Trip",
                table: "TripTempFiles",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripTempFiles",
                schema: "Trip");

            migrationBuilder.DropColumn(
                name: "TempFileName",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.CreateTable(
                name: "TripTempVideos",
                schema: "Trip",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<long>(type: "bigint", nullable: false),
                    ChunkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripFileType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTempVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripTempVideos_Trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "Trip",
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripTempVideos_TripId",
                schema: "Trip",
                table: "TripTempVideos",
                column: "TripId");
        }
    }
}
