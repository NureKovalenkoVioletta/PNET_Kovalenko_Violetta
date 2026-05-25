using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Repositories;

public class RecipeRepository : GenericRepository<Recipe>, IRecipeRepository
{
    public RecipeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Recipe?> GetByIdWithIngredientsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Recipes
            .Include(r => r.RecipeProducts)
            .ThenInclude(rp => rp.Product)
            .FirstOrDefaultAsync(r => r.RecipeId == id, cancellationToken);
    }

    public async Task UpdateRecipeAndReplaceIngredientsAsync(
        Recipe recipe,
        IReadOnlyCollection<RecipeProduct> newRecipeProducts,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var tracked = await _dbContext.Recipes
                .Include(r => r.RecipeProducts)
                .FirstOrDefaultAsync(r => r.RecipeId == recipe.RecipeId, cancellationToken);

            if (tracked is null)
            {
                throw new InvalidOperationException($"Рецепт з ідентифікатором {recipe.RecipeId} не знайдено.");
            }

            tracked.RecipeName = recipe.RecipeName;
            tracked.RecipeInstructions = recipe.RecipeInstructions;
            tracked.PortionCount = recipe.PortionCount;
            tracked.RecipeTotalGrams = recipe.RecipeTotalGrams;
            tracked.RecipeCalories = recipe.RecipeCalories;
            tracked.RecipeProteins = recipe.RecipeProteins;
            tracked.RecipeFats = recipe.RecipeFats;
            tracked.RecipeCarbs = recipe.RecipeCarbs;

            _dbContext.RecipeProducts.RemoveRange(tracked.RecipeProducts);

            var lines = newRecipeProducts.Select(line => new RecipeProduct
            {
                RecipeId = tracked.RecipeId,
                ProductId = line.ProductId,
                Grams = line.Grams,
            });

            await _dbContext.RecipeProducts.AddRangeAsync(lines, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task AddRecipeWithIngredientsAsync(
        Recipe recipe,
        IReadOnlyCollection<RecipeProduct> newRecipeProducts,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _dbContext.Recipes.AddAsync(recipe, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var lines = newRecipeProducts.Select(line => new RecipeProduct
            {
                RecipeId = recipe.RecipeId,
                ProductId = line.ProductId,
                Grams = line.Grams,
            });

            await _dbContext.RecipeProducts.AddRangeAsync(lines, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
