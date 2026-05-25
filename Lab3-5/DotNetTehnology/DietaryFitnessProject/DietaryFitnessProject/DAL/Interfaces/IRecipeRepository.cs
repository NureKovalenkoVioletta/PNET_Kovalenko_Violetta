using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.DAL.Interfaces;

public interface IRecipeRepository : IRepository<Recipe>
{
    Task<Recipe?> GetByIdWithIngredientsAsync(int id, CancellationToken cancellationToken = default);

    Task UpdateRecipeAndReplaceIngredientsAsync(
        Recipe recipe,
        IReadOnlyCollection<RecipeProduct> newRecipeProducts,
        CancellationToken cancellationToken = default);

    Task AddRecipeWithIngredientsAsync(
        Recipe recipe,
        IReadOnlyCollection<RecipeProduct> newRecipeProducts,
        CancellationToken cancellationToken = default);
}
