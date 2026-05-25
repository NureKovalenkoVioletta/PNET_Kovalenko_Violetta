using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietaryFitnessProject.Migrations
{
    public partial class DailyDietPlanMealsCountCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE [DailyDietPlan]
                SET [DailyPlanNumberOfMeals] = CASE
                    WHEN [DailyPlanNumberOfMeals] < 1 THEN 1
                    WHEN [DailyPlanNumberOfMeals] > 5 THEN 5
                    ELSE [DailyPlanNumberOfMeals]
                END
                WHERE [DailyPlanNumberOfMeals] < 1 OR [DailyPlanNumberOfMeals] > 5;
                """);

            migrationBuilder.AddCheckConstraint(
                name: "CK_DailyDietPlan_NumberOfMeals_Range",
                table: "DailyDietPlan",
                sql: "[DailyPlanNumberOfMeals] >= 1 AND [DailyPlanNumberOfMeals] <= 5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DailyDietPlan_NumberOfMeals_Range",
                table: "DailyDietPlan");
        }
    }
}
