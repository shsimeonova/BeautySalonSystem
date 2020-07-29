using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalonSystem.Appointments.Migrations
{
    public partial class AddedDateRequested : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateRequested",
                table: "Appointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRequested",
                table: "Appointments");
        }
    }
}
