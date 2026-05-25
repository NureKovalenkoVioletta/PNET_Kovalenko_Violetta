using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Repositories;

public class MealRecipeRepository : IMealRecipeRepository
{
    private readonly AppDbContext _dbContext;

    public MealRecipeRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task<IReadOnlyCollection<MealRecipe>> GetByMealIdAsync(int mealId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MealRecipes
            .AsNoTracking()
            .Where(x => x.MealId == mealId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(MealRecipe entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.MealRecipes.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int mealId, int recipeId, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.MealRecipes.FindAsync([mealId, recipeId], cancellationToken);
        if (entity is null)
        {
            return;
        }

        _dbContext.MealRecipes.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
