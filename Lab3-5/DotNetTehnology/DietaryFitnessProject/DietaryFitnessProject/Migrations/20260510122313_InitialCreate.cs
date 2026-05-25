using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietaryFitnessProject.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductCaloriesPer100g = table.Column<double>(type: "float", nullable: false),
                    ProductProteinsPer100g = table.Column<double>(type: "float", nullable: false),
                    ProductFatsPer100g = table.Column<double>(type: "float", nullable: false),
                    ProductCarbsPer100g = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.CheckConstraint("CK_Product_CaloriesPer100g_NonNegative", "[ProductCaloriesPer100g] >= 0");
                    table.CheckConstraint("CK_Product_CarbsPer100g_NonNegative", "[ProductCarbsPer100g] >= 0");
                    table.CheckConstraint("CK_Product_FatsPer100g_NonNegative", "[ProductFatsPer100g] >= 0");
                    table.CheckConstraint("CK_Product_ProteinsPer100g_NonNegative", "[ProductProteinsPer100g] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RecipeCalories = table.Column<double>(type: "float", nullable: false),
                    RecipeProteins = table.Column<double>(type: "float", nullable: false),
                    RecipeFats = table.Column<double>(type: "float", nullable: false),
                    RecipeCarbs = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.RecipeId);
                    table.CheckConstraint("CK_Recipe_Calories_NonNegative", "[RecipeCalories] >= 0");
                    table.CheckConstraint("CK_Recipe_Carbs_NonNegative", "[RecipeCarbs] >= 0");
                    table.CheckConstraint("CK_Recipe_Fats_NonNegative", "[RecipeFats] >= 0");
                    table.CheckConstraint("CK_Recipe_Proteins_NonNegative", "[RecipeProteins] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    ActivityLevel = table.Column<int>(type: "int", nullable: false),
                    Goal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.CheckConstraint("CK_User_Height_NonNegative", "[Height] >= 0");
                    table.CheckConstraint("CK_User_Weight_NonNegative", "[Weight] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "RecipeProduct",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Grams = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeProduct", x => new { x.RecipeId, x.ProductId });
                    table.CheckConstraint("CK_RecipeProduct_Grams_NonNegative", "[Grams] >= 0");
                    table.ForeignKey(
                        name: "FK_RecipeProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeProduct_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyDietPlan",
                columns: table => new
                {
                    DailyDietPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DailyPlanCalories = table.Column<double>(type: "float", nullable: false),
                    DailyPlanProteins = table.Column<double>(type: "float", nullable: false),
                    DailyPlanFats = table.Column<double>(type: "float", nullable: false),
                    DailyPlanCarbs = table.Column<double>(type: "float", nullable: false),
                    DailyPlanDay = table.Column<DateOnly>(type: "date", nullable: false),
                    DailyPlanNumberOfMeals = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyDietPlan", x => x.DailyDietPlanId);
                    table.CheckConstraint("CK_DailyDietPlan_Calories_NonNegative", "[DailyPlanCalories] >= 0");
                    table.CheckConstraint("CK_DailyDietPlan_Carbs_NonNegative", "[DailyPlanCarbs] >= 0");
                    table.CheckConstraint("CK_DailyDietPlan_Fats_NonNegative", "[DailyPlanFats] >= 0");
                    table.CheckConstraint("CK_DailyDietPlan_Proteins_NonNegative", "[DailyPlanProteins] >= 0");
                    table.ForeignKey(
                        name: "FK_DailyDietPlan_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyDietPlanId = table.Column<int>(type: "int", nullable: false),
                    MealOrder = table.Column<int>(type: "int", nullable: false),
                    MealTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    MealCalories = table.Column<double>(type: "float", nullable: false),
                    MealProteins = table.Column<double>(type: "float", nullable: false),
                    MealFats = table.Column<double>(type: "float", nullable: false),
                    MealCarbs = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.MealId);
                    table.CheckConstraint("CK_Meal_Calories_NonNegative", "[MealCalories] >= 0");
                    table.CheckConstraint("CK_Meal_Carbs_NonNegative", "[MealCarbs] >= 0");
                    table.CheckConstraint("CK_Meal_Fats_NonNegative", "[MealFats] >= 0");
                    table.CheckConstraint("CK_Meal_Proteins_NonNegative", "[MealProteins] >= 0");
                    table.ForeignKey(
                        name: "FK_Meal_DailyDietPlan_DailyDietPlanId",
                        column: x => x.DailyDietPlanId,
                        principalTable: "DailyDietPlan",
                        principalColumn: "DailyDietPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealRecipe",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealRecipe", x => new { x.MealId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_MealRecipe_Meal_MealId",
                        column: x => x.MealId,
                        principalTable: "Meal",
                        principalColumn: "MealId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealRecipe_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyDietPlan_UserId_DailyPlanDay",
                table: "DailyDietPlan",
                columns: new[] { "UserId", "DailyPlanDay" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meal_DailyDietPlanId_MealOrder",
                table: "Meal",
                columns: new[] { "DailyDietPlanId", "MealOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealRecipe_RecipeId",
                table: "MealRecipe",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeProduct_ProductId",
                table: "RecipeProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealRecipe");

            migrationBuilder.DropTable(
                name: "RecipeProduct");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "DailyDietPlan");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
