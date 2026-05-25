using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class MealRecipeService : IMealRecipeService
{
    private readonly IMealRecipeRepository _mealRecipeRepository;

    public MealRecipeService(IMealRecipeRepository mealRecipeRepository)
    {
        _mealRecipeRepository = mealRecipeRepository;
    }

    public Task<IReadOnlyCollection<MealRecipe>> GetByMealIdAsync(int mealId, CancellationToken cancellationToken = default)
        => _mealRecipeRepository.GetByMealIdAsync(mealId, cancellationToken);

    public Task AddAsync(MealRecipe relation, CancellationToken cancellationToken = default)
        => _mealRecipeRepository.AddAsync(relation, cancellationToken);

    public Task DeleteAsync(int mealId, int recipeId, CancellationToken cancellationToken = default)
        => _mealRecipeRepository.DeleteAsync(mealId, recipeId, cancellationToken);
}
