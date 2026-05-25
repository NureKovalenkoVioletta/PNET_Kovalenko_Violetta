using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Interfaces;

public interface IDailyDietPlanService
{
    Task<IReadOnlyCollection<DailyDietPlan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DailyDietPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DailyDietPlan?> GetByIdForUserAsync(int planId, int userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DailyDietPlan>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<DailyDietPlan?> GetByUserAndDateAsync(int userId, DateOnly day, CancellationToken cancellationToken = default);
    Task AddAsync(DailyDietPlan plan, CancellationToken cancellationToken = default);
    Task UpdateAsync(DailyDietPlan plan, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<DailyDietPlan> CreatePlanForUserAsync(
        int userId,
        DateOnly date,
        int? numberOfMeals = null,
        CancellationToken cancellationToken = default);
}
