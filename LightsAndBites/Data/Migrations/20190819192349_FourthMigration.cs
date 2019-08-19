using Microsoft.EntityFrameworkCore.Migrations;

namespace LightsAndBites.Data.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BarCategoryIdOne",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BarCategoryIdTwo",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserProfile",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventCategoryIdOne",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventCategoryIdThree",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventCategoryIdTwo",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "UserProfile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hometown",
                table: "UserProfile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "UserProfile",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantCategoryIdOne",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantCategoryIdThree",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantCategoryIdTwo",
                table: "UserProfile",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCategoryIdOne",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "BarCategoryIdTwo",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "EventCategoryIdOne",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "EventCategoryIdThree",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "EventCategoryIdTwo",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Hometown",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "RestaurantCategoryIdOne",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "RestaurantCategoryIdThree",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "RestaurantCategoryIdTwo",
                table: "UserProfile");
        }
    }
}
