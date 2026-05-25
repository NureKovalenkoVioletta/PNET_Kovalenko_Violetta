using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Services;

public static class RecipeNutritionCalculator
{
    public static RecipeNutritionTotals Calculate(IEnumerable<(Product Product, double Grams)> lines)
    {
        double totalGrams = 0;
        double calories = 0;
        double proteins = 0;
        double fats = 0;
        double carbs = 0;

        foreach (var (product, grams) in lines)
        {
            var factor = grams / 100.0;
            totalGrams += grams;
            calories += product.ProductCaloriesPer100g * factor;
            proteins += product.ProductProteinsPer100g * factor;
            fats += product.ProductFatsPer100g * factor;
            carbs += product.ProductCarbsPer100g * factor;
        }

        return new RecipeNutritionTotals(calories, proteins, fats, carbs, totalGrams);
    }
}
