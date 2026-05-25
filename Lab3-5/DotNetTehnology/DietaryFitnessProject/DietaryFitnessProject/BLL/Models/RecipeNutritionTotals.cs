namespace DietaryFitnessProject.BLL.Models;

public readonly record struct RecipeNutritionTotals(
    double TotalCalories,
    double TotalProteins,
    double TotalFats,
    double TotalCarbs,
    double TotalGrams);
