using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IProductRepository _productRepository;

    public RecipeService(IRecipeRepository recipeRepository, IProductRepository productRepository)
    {
        _recipeRepository = recipeRepository;
        _productRepository = productRepository;
    }

    public Task<IReadOnlyCollection<Recipe>> GetAllAsync(CancellationToken cancellationToken = default)
        => _recipeRepository.GetAllAsync(cancellationToken);

    public Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _recipeRepository.GetByIdAsync(id, cancellationToken);

    public Task<Recipe?> GetByIdWithIngredientsAsync(int id, CancellationToken cancellationToken = default)
        => _recipeRepository.GetByIdWithIngredientsAsync(id, cancellationToken);

    public Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default)
        => _recipeRepository.AddAsync(recipe, cancellationToken);

    public Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default)
        => _recipeRepository.UpdateAsync(recipe, cancellationToken);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        => _recipeRepository.DeleteAsync(id, cancellationToken);

    public async Task<Recipe> CreateWithIngredientsAsync(
        string recipeName,
        string recipeInstructions,
        int portionCount,
        IReadOnlyList<RecipeIngredientInput> ingredientLines,
        CancellationToken cancellationToken = default)
    {
        ValidateRecipeInput(portionCount, ingredientLines);

        var productsById = await LoadProductsForLinesAsync(ingredientLines, cancellationToken);
        var totals = RecipeNutritionCalculator.Calculate(
            ingredientLines.Select(line => (productsById[line.ProductId], line.Grams)));

        var recipe = new Recipe
        {
            RecipeName = recipeName,
            RecipeInstructions = recipeInstructions,
            PortionCount = portionCount,
            RecipeTotalGrams = totals.TotalGrams,
            RecipeCalories = totals.TotalCalories,
            RecipeProteins = totals.TotalProteins,
            RecipeFats = totals.TotalFats,
            RecipeCarbs = totals.TotalCarbs,
        };

        var recipeProducts = ingredientLines
            .Select(line => new RecipeProduct { ProductId = line.ProductId, Grams = line.Grams })
            .ToList();

        await _recipeRepository.AddRecipeWithIngredientsAsync(recipe, recipeProducts, cancellationToken);

        return await _recipeRepository.GetByIdWithIngredientsAsync(recipe.RecipeId, cancellationToken)
               ?? throw new InvalidOperationException("Не вдалося завантажити створений рецепт.");
    }

    public async Task<Recipe> UpdateWithIngredientsAsync(
        int recipeId,
        string recipeName,
        string recipeInstructions,
        int portionCount,
        IReadOnlyList<RecipeIngredientInput> ingredientLines,
        CancellationToken cancellationToken = default)
    {
        ValidateRecipeInput(portionCount, ingredientLines);

        var existing = await _recipeRepository.GetByIdAsync(recipeId, cancellationToken);
        if (existing is null)
        {
            throw new InvalidOperationException($"Рецепт з ідентифікатором {recipeId} не знайдено.");
        }

        var productsById = await LoadProductsForLinesAsync(ingredientLines, cancellationToken);
        var totals = RecipeNutritionCalculator.Calculate(
            ingredientLines.Select(line => (productsById[line.ProductId], line.Grams)));

        var recipe = new Recipe
        {
            RecipeId = recipeId,
            RecipeName = recipeName,
            RecipeInstructions = recipeInstructions,
            PortionCount = portionCount,
            RecipeTotalGrams = totals.TotalGrams,
            RecipeCalories = totals.TotalCalories,
            RecipeProteins = totals.TotalProteins,
            RecipeFats = totals.TotalFats,
            RecipeCarbs = totals.TotalCarbs,
        };

        var recipeProducts = ingredientLines
            .Select(line => new RecipeProduct { ProductId = line.ProductId, Grams = line.Grams })
            .ToList();

        await _recipeRepository.UpdateRecipeAndReplaceIngredientsAsync(recipe, recipeProducts, cancellationToken);

        return await _recipeRepository.GetByIdWithIngredientsAsync(recipeId, cancellationToken)
               ?? throw new InvalidOperationException("Не вдалося завантажити оновлений рецепт.");
    }

    public async Task RecalculateNutritionFromIngredientsAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        var recipe = await _recipeRepository.GetByIdWithIngredientsAsync(recipeId, cancellationToken);
        if (recipe is null)
        {
            return;
        }

        if (recipe.RecipeProducts.Count == 0)
        {
            return;
        }

        var totals = RecipeNutritionCalculator.Calculate(
            recipe.RecipeProducts.Select(rp => (rp.Product, rp.Grams)));

        recipe.RecipeTotalGrams = totals.TotalGrams;
        recipe.RecipeCalories = totals.TotalCalories;
        recipe.RecipeProteins = totals.TotalProteins;
        recipe.RecipeFats = totals.TotalFats;
        recipe.RecipeCarbs = totals.TotalCarbs;

        await _recipeRepository.UpdateAsync(recipe, cancellationToken);
    }

    private static void ValidateRecipeInput(int portionCount, IReadOnlyList<RecipeIngredientInput> ingredientLines)
    {
        if (portionCount < 1)
        {
            throw new InvalidOperationException("Кількість порцій має бути не менше 1.");
        }

        if (ingredientLines.Count < 1)
        {
            throw new InvalidOperationException("Додайте хоча б один інгредієнт.");
        }

        var productIds = ingredientLines.Select(l => l.ProductId).ToList();
        if (productIds.Count != productIds.Distinct().Count())
        {
            throw new InvalidOperationException(
                "Кожен продукт у рецепті може бути лише в одному рядку. Об’єднайте грами для одного продукту.");
        }

        for (var i = 0; i < ingredientLines.Count; i++)
        {
            var line = ingredientLines[i];
            if (line.Grams <= 0 || double.IsNaN(line.Grams) || double.IsInfinity(line.Grams))
            {
                throw new InvalidOperationException($"Маса інгредієнта в рядку {i + 1} має бути додатною.");
            }
        }
    }

    private async Task<Dictionary<int, Product>> LoadProductsForLinesAsync(
        IReadOnlyList<RecipeIngredientInput> ingredientLines,
        CancellationToken cancellationToken)
    {
        var distinctIds = ingredientLines.Select(l => l.ProductId).Distinct().ToArray();
        var products = await _productRepository.GetByIdsAsync(distinctIds, cancellationToken);
        var map = products.ToDictionary(p => p.ProductId);

        if (map.Count != distinctIds.Length)
        {
            var missing = distinctIds.Where(id => !map.ContainsKey(id)).ToArray();
            throw new InvalidOperationException(
                $"Не знайдено продукти з ідентифікаторами: {string.Join(", ", missing)}.");
        }

        return map;
    }
}
