using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.DAL.Interfaces;

public interface IDailyDietPlanRepository : IRepository<DailyDietPlan>
{
    Task<IReadOnlyCollection<DailyDietPlan>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<DailyDietPlan?> GetByUserAndDateAsync(int userId, DateOnly day, CancellationToken cancellationToken = default);
}
