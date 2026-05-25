using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Repositories;

public class RecipeProductRepository : IRecipeProductRepository
{
    private readonly AppDbContext _dbContext;

    public RecipeProductRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<IReadOnlyCollection<RecipeProduct>> GetByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RecipeProducts
            .AsNoTracking()
            .Where(x => x.RecipeId == recipeId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(RecipeProduct entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.RecipeProducts.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<RecipeProduct> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.RecipeProducts.AddRangeAsync(entities, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int recipeId, int productId, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.RecipeProducts.FindAsync([recipeId, productId], cancellationToken);
        if (entity is null)
        {
            return;
        }

        _dbContext.RecipeProducts.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        var toRemove = await _dbContext.RecipeProducts
            .Where(rp => rp.RecipeId == recipeId)
            .ToListAsync(cancellationToken);

        if (toRemove.Count == 0)
        {
            return;
        }

        _dbContext.RecipeProducts.RemoveRange(toRemove);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsProductUsedInRecipesAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RecipeProducts
            .AsNoTracking()
            .AnyAsync(rp => rp.ProductId == productId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<int>> GetRecipeIdsByProductIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.RecipeProducts
            .AsNoTracking()
            .Where(rp => rp.ProductId == productId)
            .Select(rp => rp.RecipeId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
