using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Interfaces;

public interface IRecipeService
{
    Task<IReadOnlyCollection<Recipe>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Recipe?> GetByIdWithIngredientsAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<Recipe> CreateWithIngredientsAsync(
        string recipeName,
        string recipeInstructions,
        int portionCount,
        IReadOnlyList<RecipeIngredientInput> ingredientLines,
        CancellationToken cancellationToken = default);

    Task<Recipe> UpdateWithIngredientsAsync(
        int recipeId,
        string recipeName,
        string recipeInstructions,
        int portionCount,
        IReadOnlyList<RecipeIngredientInput> ingredientLines,
        CancellationToken cancellationToken = default);

    Task RecalculateNutritionFromIngredientsAsync(int recipeId, CancellationToken cancellationToken = default);
}
