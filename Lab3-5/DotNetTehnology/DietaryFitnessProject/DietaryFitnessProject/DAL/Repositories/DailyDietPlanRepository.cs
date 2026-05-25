using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Repositories;

public class DailyDietPlanRepository : GenericRepository<DailyDietPlan>, IDailyDietPlanRepository
{
    public DailyDietPlanRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyCollection<DailyDietPlan>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DailyDietPlans
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.DailyPlanDay)
            .ToListAsync(cancellationToken);
    }

    public async Task<DailyDietPlan?> GetByUserAndDateAsync(int userId, DateOnly day, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DailyDietPlans
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.DailyPlanDay == day, cancellationToken);
    }
}
