using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Enums;

namespace DietaryFitnessProject.BLL.Services;

public static class DailyDietTargetCalculator
{
    private const double MinHeightCm = 50;
    private const double MaxHeightCm = 260;
    private const double MinWeightKg = 20;
    private const double MaxWeightKg = 400;
    private const int MinAge = 10;
    private const int MaxAge = 120;

    private const double ProteinGramsPerKg = 1.8;

    private const double FatCalorieFraction = 0.28;

    private const double LoseWeightCalorieFraction = 0.10;

    private const double GainWeightCalorieFraction = 0.10;

    private const double MinDailyCalories = 1200;

    public static DailyDietTargets Calculate(User user)
    {
        Validate(user);

        var bmr = CalculateBmrMifflinStJeor(user.Weight, user.Height, user.Age, user.Gender);
        var tdee = bmr * GetActivityMultiplier(user.ActivityLevel);
        var targetCalories = ApplyGoalDelta(tdee, user.Goal);
        targetCalories = Math.Max(MinDailyCalories, targetCalories);

        var proteinG = user.Weight * ProteinGramsPerKg;
        var fatKcal = targetCalories * FatCalorieFraction;
        var fatG = fatKcal / 9.0;
        var carbKcal = targetCalories - proteinG * 4.0 - fatG * 9.0;

        if (carbKcal < 50)
        {
            proteinG = user.Weight * 1.6;
            fatKcal = targetCalories * 0.25;
            fatG = fatKcal / 9.0;
            carbKcal = targetCalories - proteinG * 4.0 - fatG * 9.0;
        }

        var carbG = Math.Max(0, carbKcal / 4.0);

        return new DailyDietTargets(
            DailyCalories: Math.Round(targetCalories, 0),
            ProteinsGrams: Math.Round(proteinG, 1),
            FatsGrams: Math.Round(fatG, 1),
            CarbsGrams: Math.Round(carbG, 1));
    }

    public static double CalculateBmrMifflinStJeor(double weightKg, double heightCm, int ageYears, Gender gender)
    {
        var basePart = 10.0 * weightKg + 6.25 * heightCm - 5.0 * ageYears;
        return gender switch
        {
            Gender.Male => basePart + 5.0,
            Gender.Female => basePart - 161.0,
            _ => throw new InvalidOperationException("Невідоме значення статі."),
        };
    }

    public static double GetActivityMultiplier(ActivityLevel level) => level switch
    {
        ActivityLevel.Sedentary => 1.2,
        ActivityLevel.Light => 1.375,
        ActivityLevel.Moderate => 1.55,
        ActivityLevel.Active => 1.725,
        ActivityLevel.VeryActive => 1.9,
        _ => throw new InvalidOperationException("Невідомий рівень активності."),
    };

    public static double ApplyGoalDelta(double tdeeKcal, GoalType goal) => goal switch
    {
        GoalType.LoseWeight => tdeeKcal * (1.0 - LoseWeightCalorieFraction),
        GoalType.MaintainWeight => tdeeKcal,
        GoalType.GainWeight => tdeeKcal * (1.0 + GainWeightCalorieFraction),
        _ => throw new InvalidOperationException("Невідома мета."),
    };

    private static void Validate(User user)
    {
        if (user.Age < MinAge || user.Age > MaxAge)
        {
            throw new InvalidOperationException($"Вік має бути в діапазоні {MinAge}–{MaxAge} років.");
        }

        if (user.Height is < MinHeightCm or > MaxHeightCm)
        {
            throw new InvalidOperationException($"Зріст має бути в діапазоні {MinHeightCm}–{MaxHeightCm} см.");
        }

        if (user.Weight is < MinWeightKg or > MaxWeightKg)
        {
            throw new InvalidOperationException($"Вага має бути в діапазоні {MinWeightKg}–{MaxWeightKg} кг.");
        }
    }
}
