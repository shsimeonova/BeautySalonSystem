using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalonSystem.Appointments.Migrations
{
    public partial class RenamedOfferId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductOfferId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "OfferId",
                table: "Appointments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "ProductOfferId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
