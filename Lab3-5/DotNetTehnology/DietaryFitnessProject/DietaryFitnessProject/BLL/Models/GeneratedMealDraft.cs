using DietaryFitnessProject.DAL.Enums;

namespace DietaryFitnessProject.BLL.Models;

internal readonly record struct GeneratedMealDraft(
    int MealOrder,
    MealType MealType,
    TimeOnly MealTime,
    double MealCalories,
    double MealProteins,
    double MealFats,
    double MealCarbs,
    IReadOnlyList<int> RecipeIds);
