using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Add_UserRideScore_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RideScore");

            migrationBuilder.CreateTable(
                name: "UserRideScores",
                schema: "RideScore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsVideoUploaded = table.Column<bool>(type: "bit", nullable: false),
                    IsImuUploaded = table.Column<bool>(type: "bit", nullable: false),
                    IsGpsUploaded = table.Column<bool>(type: "bit", nullable: false),
                    ChunkSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VideoFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRideScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRideScores_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRideVideosSplits",
                schema: "RideScore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRideScoreId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUploaded = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRideVideosSplits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRideVideosSplits_UserRideScores_UserRideScoreId",
                        column: x => x.UserRideScoreId,
                        principalSchema: "RideScore",
                        principalTable: "UserRideScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRideScores_UserId",
                schema: "RideScore",
                table: "UserRideScores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRideVideosSplits_UserRideScoreId",
                schema: "RideScore",
                table: "UserRideVideosSplits",
                column: "UserRideScoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRideVideosSplits",
                schema: "RideScore");

            migrationBuilder.DropTable(
                name: "UserRideScores",
                schema: "RideScore");
        }
    }
}
