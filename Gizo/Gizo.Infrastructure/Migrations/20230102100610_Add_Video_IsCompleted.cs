using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gizo.Infrastructure.Migrations
{
    public partial class Add_Video_IsCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                schema: "Trip",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                schema: "Trip",
                table: "Trips");
        }
    }
}
