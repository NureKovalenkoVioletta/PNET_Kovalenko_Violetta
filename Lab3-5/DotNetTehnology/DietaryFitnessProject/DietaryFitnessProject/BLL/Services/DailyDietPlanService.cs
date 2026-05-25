using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class DailyDietPlanService : IDailyDietPlanService
{
    private readonly IDailyDietPlanRepository _dailyDietPlanRepository;
    private readonly IUserRepository _userRepository;

    public DailyDietPlanService(
        IDailyDietPlanRepository dailyDietPlanRepository,
        IUserRepository userRepository)
    {
        _dailyDietPlanRepository = dailyDietPlanRepository;
        _userRepository = userRepository;
    }

    public Task<IReadOnlyCollection<DailyDietPlan>> GetAllAsync(CancellationToken cancellationToken = default)
        => _dailyDietPlanRepository.GetAllAsync(cancellationToken);

    public Task<DailyDietPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _dailyDietPlanRepository.GetByIdAsync(id, cancellationToken);

    public Task<DailyDietPlan?> GetByIdForUserAsync(int planId, int userId, CancellationToken cancellationToken = default)
        => GetOwnedPlanAsync(planId, userId, cancellationToken);

    public Task<IReadOnlyCollection<DailyDietPlan>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        => _dailyDietPlanRepository.GetByUserIdAsync(userId, cancellationToken);

    public Task<DailyDietPlan?> GetByUserAndDateAsync(int userId, DateOnly day, CancellationToken cancellationToken = default)
        => _dailyDietPlanRepository.GetByUserAndDateAsync(userId, day, cancellationToken);

    public Task AddAsync(DailyDietPlan plan, CancellationToken cancellationToken = default)
        => _dailyDietPlanRepository.AddAsync(plan, cancellationToken);

    public Task UpdateAsync(DailyDietPlan plan, CancellationToken cancellationToken = default)
        => _dailyDietPlanRepository.UpdateAsync(plan, cancellationToken);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        => _dailyDietPlanRepository.DeleteAsync(id, cancellationToken);

    public async Task<DailyDietPlan> CreatePlanForUserAsync(
        int userId,
        DateOnly date,
        int? numberOfMeals = null,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException($"Користувача з ідентифікатором {userId} не знайдено.");
        }

        var existing = await _dailyDietPlanRepository.GetByUserAndDateAsync(userId, date, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException(FormatPlanAlreadyExistsMessage(date));
        }

        var targets = DailyDietTargetCalculator.Calculate(user);
        var meals = NormalizeMealCount(numberOfMeals);

        var created = new DailyDietPlan
        {
            UserId = userId,
            DailyPlanDay = date,
            DailyPlanCalories = targets.DailyCalories,
            DailyPlanProteins = targets.ProteinsGrams,
            DailyPlanFats = targets.FatsGrams,
            DailyPlanCarbs = targets.CarbsGrams,
            DailyPlanNumberOfMeals = meals,
        };

        await _dailyDietPlanRepository.AddAsync(created, cancellationToken);

        var saved = await _dailyDietPlanRepository.GetByUserAndDateAsync(userId, date, cancellationToken);
        return saved
               ?? throw new InvalidOperationException("Не вдалося завантажити збережений денний план.");
    }

    public static string FormatPlanAlreadyExistsMessage(DateOnly date) =>
        $"Денний план на {date:dd.MM.yyyy} вже згенеровано. Повторно створити план на цю дату неможливо. " +
        "Щоб змінити параметри, спочатку видаліть план у розділі «Усі плани».";

    private async Task<DailyDietPlan?> GetOwnedPlanAsync(int planId, int userId, CancellationToken cancellationToken)
    {
        var plan = await _dailyDietPlanRepository.GetByIdAsync(planId, cancellationToken);
        if (plan is null || plan.UserId != userId)
        {
            return null;
        }

        return plan;
    }

    private static int NormalizeMealCount(int? numberOfMeals)
    {
        var n = numberOfMeals ?? 3;
        if (n < 1)
        {
            return 1;
        }

        if (n > 5)
        {
            return 5;
        }

        return n;
    }
}
