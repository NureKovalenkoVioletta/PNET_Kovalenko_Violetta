using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.DAL.Interfaces;

public interface IRecipeProductRepository
{
    Task<IReadOnlyCollection<RecipeProduct>> GetByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default);

    Task AddAsync(RecipeProduct entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<RecipeProduct> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(int recipeId, int productId, CancellationToken cancellationToken = default);

    Task DeleteByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default);

    Task<bool> IsProductUsedInRecipesAsync(int productId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<int>> GetRecipeIdsByProductIdAsync(int productId, CancellationToken cancellationToken = default);
}
