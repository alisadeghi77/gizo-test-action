using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Remove_IsVideoUploaded_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUploaded",
                schema: "RideScore",
                table: "UserRideVideosSplits");

            migrationBuilder.AddColumn<string>(
                name: "VideoFilePath",
                schema: "RideScore",
                table: "UserRideScores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoFilePath",
                schema: "RideScore",
                table: "UserRideScores");

            migrationBuilder.AddColumn<bool>(
                name: "IsUploaded",
                schema: "RideScore",
                table: "UserRideVideosSplits",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
