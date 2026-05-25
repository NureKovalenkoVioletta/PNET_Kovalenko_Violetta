using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Interfaces;

public interface IMealService
{
    Task<IReadOnlyCollection<Meal>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Meal?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Meal>> GetByPlanIdAsync(int dailyDietPlanId, CancellationToken cancellationToken = default);
    Task AddAsync(Meal meal, CancellationToken cancellationToken = default);
    Task UpdateAsync(Meal meal, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Meal>> GenerateForPlanAsync(int dailyDietPlanId, CancellationToken cancellationToken = default);
    Task DeleteAllForPlanAsync(int dailyDietPlanId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MealDisplayItem>> GetDisplayItemsForPlanAsync(int dailyDietPlanId, CancellationToken cancellationToken = default);
}
