using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IRecipeProductRepository _recipeProductRepository;
    private readonly IRecipeService _recipeService;

    public ProductService(
        IProductRepository productRepository,
        IRecipeProductRepository recipeProductRepository,
        IRecipeService recipeService)
    {
        _productRepository = productRepository;
        _recipeProductRepository = recipeProductRepository;
        _recipeService = recipeService;
    }

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => _productRepository.GetAllAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _productRepository.GetByIdAsync(id, cancellationToken);

    public Task<bool> IsUsedInRecipesAsync(int productId, CancellationToken cancellationToken = default)
        => _recipeProductRepository.IsProductUsedInRecipesAsync(productId, cancellationToken);

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
        => _productRepository.AddAsync(product, cancellationToken);

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _productRepository.UpdateAsync(product, cancellationToken);

        var recipeIds = await _recipeProductRepository.GetRecipeIdsByProductIdAsync(
            product.ProductId,
            cancellationToken);

        foreach (var recipeId in recipeIds)
        {
            await _recipeService.RecalculateNutritionFromIngredientsAsync(recipeId, cancellationToken);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _recipeProductRepository.IsProductUsedInRecipesAsync(id, cancellationToken))
        {
            throw new InvalidOperationException(
                "Неможливо видалити продукт: він входить до складу одного або кількох рецептів. " +
                "Спочатку приберіть продукт із рецептів.");
        }

        await _productRepository.DeleteAsync(id, cancellationToken);
    }
}
