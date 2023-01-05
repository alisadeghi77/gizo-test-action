using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class AddGpsImuFieldsToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GPSJsonData",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IMUJsonData",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GPSJsonData",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "IMUJsonData",
                schema: "Trip",
                table: "Trips");
        }
    }
}
