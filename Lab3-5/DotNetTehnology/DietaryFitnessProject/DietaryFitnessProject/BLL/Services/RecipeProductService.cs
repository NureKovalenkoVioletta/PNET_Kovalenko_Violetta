using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class RecipeProductService : IRecipeProductService
{
    private readonly IRecipeProductRepository _recipeProductRepository;

    public RecipeProductService(IRecipeProductRepository recipeProductRepository)
    {
        _recipeProductRepository = recipeProductRepository;
    }

    public Task<IReadOnlyCollection<RecipeProduct>> GetByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default)
        => _recipeProductRepository.GetByRecipeIdAsync(recipeId, cancellationToken);

    public Task AddAsync(RecipeProduct relation, CancellationToken cancellationToken = default)
        => _recipeProductRepository.AddAsync(relation, cancellationToken);

    public Task DeleteAsync(int recipeId, int productId, CancellationToken cancellationToken = default)
        => _recipeProductRepository.DeleteAsync(recipeId, productId, cancellationToken);
}
