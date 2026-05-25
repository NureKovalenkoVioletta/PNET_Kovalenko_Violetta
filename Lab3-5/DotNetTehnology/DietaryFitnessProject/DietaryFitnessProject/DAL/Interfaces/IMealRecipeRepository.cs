using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.DAL.Interfaces;

public interface IMealRecipeRepository
{
    Task<IReadOnlyCollection<MealRecipe>> GetByMealIdAsync(int mealId, CancellationToken cancellationToken = default);
    Task AddAsync(MealRecipe entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int mealId, int recipeId, CancellationToken cancellationToken = default);
}
