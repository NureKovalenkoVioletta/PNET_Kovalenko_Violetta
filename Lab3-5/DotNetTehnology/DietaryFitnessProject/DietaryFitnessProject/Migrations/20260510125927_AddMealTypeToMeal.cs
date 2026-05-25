using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietaryFitnessProject.Migrations
{
    public partial class AddMealTypeToMeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealType",
                table: "Meal",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealType",
                table: "Meal");
        }
    }
}
