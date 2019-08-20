using Microsoft.EntityFrameworkCore.Migrations;

namespace LightsAndBites.Data.Migrations
{
    public partial class ChangedTypeToNameOnRestaurantModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Restaurants",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Restaurants",
                newName: "Type");
        }
    }
}
