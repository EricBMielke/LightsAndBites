using Microsoft.EntityFrameworkCore.Migrations;

namespace LightsAndBites.Data.Migrations
{
    public partial class ChangedBarTypeToName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Bars",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Bars",
                newName: "Type");
        }
    }
}
