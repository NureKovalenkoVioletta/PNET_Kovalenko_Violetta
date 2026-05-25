using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.DAL.Interfaces;

public interface IMealRepository : IRepository<Meal>
{
    Task<IReadOnlyCollection<Meal>> GetByPlanIdAsync(int dailyDietPlanId, CancellationToken cancellationToken = default);
}
