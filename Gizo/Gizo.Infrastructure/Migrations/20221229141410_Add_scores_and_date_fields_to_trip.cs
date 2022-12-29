using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Add_scores_and_date_fields_to_trip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                schema: "Trip",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HarshAccelerationScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HarshBrakingScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HarshCorneringScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RedLightCrossingScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RideDistanceScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RideDurationScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SpeedingScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                schema: "Trip",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StopSignCrossingScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TailGatingScore",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "HarshAccelerationScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "HarshBrakingScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "HarshCorneringScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "RedLightCrossingScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "RideDistanceScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "RideDurationScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "SpeedingScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "StopSignCrossingScore",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TailGatingScore",
                schema: "Trip",
                table: "Trips");
        }
    }
}
