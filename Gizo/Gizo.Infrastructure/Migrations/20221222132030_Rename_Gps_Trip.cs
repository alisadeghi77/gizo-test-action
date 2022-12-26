using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Rename_Gps_Trip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GpuFileName",
                schema: "Trip",
                table: "Trips",
                newName: "GpsFileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GpsFileName",
                schema: "Trip",
                table: "Trips",
                newName: "GpuFileName");
        }
    }
}
