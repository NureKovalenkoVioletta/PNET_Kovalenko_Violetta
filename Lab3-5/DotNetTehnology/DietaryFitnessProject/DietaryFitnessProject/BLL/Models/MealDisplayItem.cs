namespace DietaryFitnessProject.BLL.Models;

public readonly record struct MealDisplayItem(
    int MealOrder,
    string MealTypeLabel,
    TimeOnly MealTime,
    double Calories,
    double Proteins,
    double Fats,
    double Carbs,
    IReadOnlyList<MealRecipeLineItem> Recipes,
    int Portions,
    double Grams);

public readonly record struct MealRecipeLineItem(int RecipeId, string RecipeName);
