using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Interfaces;

public interface IRecipeProductService
{
    Task<IReadOnlyCollection<RecipeProduct>> GetByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default);
    Task AddAsync(RecipeProduct relation, CancellationToken cancellationToken = default);
    Task DeleteAsync(int recipeId, int productId, CancellationToken cancellationToken = default);
}
