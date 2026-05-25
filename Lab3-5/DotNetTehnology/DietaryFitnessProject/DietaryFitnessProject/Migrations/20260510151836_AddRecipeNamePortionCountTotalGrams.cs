using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietaryFitnessProject.Migrations
{
    public partial class AddRecipeNamePortionCountTotalGrams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortionCount",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "RecipeName",
                table: "Recipe",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "RecipeTotalGrams",
                table: "Recipe",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Recipe_PortionCount_MinOne",
                table: "Recipe",
                sql: "[PortionCount] >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Recipe_TotalGrams_NonNegative",
                table: "Recipe",
                sql: "[RecipeTotalGrams] >= 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Recipe_PortionCount_MinOne",
                table: "Recipe");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Recipe_TotalGrams_NonNegative",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "PortionCount",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "RecipeName",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "RecipeTotalGrams",
                table: "Recipe");
        }
    }
}
