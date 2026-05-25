namespace DietaryFitnessProject.BLL.Models;

internal readonly record struct RecipePortionCandidate(
    int RecipeId,
    string RecipeName,
    int Portions,
    double Calories,
    double Proteins,
    double Fats,
    double Carbs,
    double Score);
