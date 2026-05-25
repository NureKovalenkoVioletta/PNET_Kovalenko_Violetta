using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Interfaces;

public interface IMealRecipeService
{
    Task<IReadOnlyCollection<MealRecipe>> GetByMealIdAsync(int mealId, CancellationToken cancellationToken = default);
    Task AddAsync(MealRecipe relation, CancellationToken cancellationToken = default);
    Task DeleteAsync(int mealId, int recipeId, CancellationToken cancellationToken = default);
}
