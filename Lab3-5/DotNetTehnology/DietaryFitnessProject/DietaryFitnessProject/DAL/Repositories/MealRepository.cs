using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Repositories;

public class MealRepository : GenericRepository<Meal>, IMealRepository
{
    public MealRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<Meal>> GetByPlanIdAsync(int dailyDietPlanId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Meals
            .AsNoTracking()
            .Where(x => x.DailyDietPlanId == dailyDietPlanId)
            .OrderBy(x => x.MealOrder)
            .ToListAsync(cancellationToken);
    }
}
