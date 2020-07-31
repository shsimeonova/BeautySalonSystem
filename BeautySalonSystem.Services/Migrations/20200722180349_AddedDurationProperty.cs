using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalonSystem.Products.Migrations
{
    public partial class AddedDurationProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Products");
        }
    }
}
