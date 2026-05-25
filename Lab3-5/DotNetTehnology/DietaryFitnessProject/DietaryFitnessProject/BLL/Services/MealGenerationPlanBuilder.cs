using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Enums;

namespace DietaryFitnessProject.BLL.Services;

internal static class MealGenerationPlanBuilder
{
    private readonly record struct SlotTemplate(int MealOrder, MealType MealType, TimeOnly MealTime, double Ratio);

    private static readonly IReadOnlyDictionary<int, SlotTemplate[]> Templates = new Dictionary<int, SlotTemplate[]>
    {
        [1] =
        [
            new SlotTemplate(1, MealType.Lunch, new TimeOnly(13, 0), 1.00),
        ],
        [2] =
        [
            new SlotTemplate(1, MealType.Breakfast, new TimeOnly(8, 0), 0.45),
            new SlotTemplate(2, MealType.Dinner, new TimeOnly(19, 0), 0.55),
        ],
        [3] =
        [
            new SlotTemplate(1, MealType.Breakfast, new TimeOnly(8, 0), 0.25),
            new SlotTemplate(2, MealType.Lunch, new TimeOnly(13, 0), 0.40),
            new SlotTemplate(3, MealType.Dinner, new TimeOnly(19, 0), 0.35),
        ],
        [4] =
        [
            new SlotTemplate(1, MealType.Breakfast, new TimeOnly(8, 0), 0.25),
            new SlotTemplate(2, MealType.MorningSnack, new TimeOnly(11, 0), 0.10),
            new SlotTemplate(3, MealType.Lunch, new TimeOnly(14, 0), 0.35),
            new SlotTemplate(4, MealType.Dinner, new TimeOnly(19, 0), 0.30),
        ],
        [5] =
        [
            new SlotTemplate(1, MealType.Breakfast, new TimeOnly(8, 0), 0.25),
            new SlotTemplate(2, MealType.MorningSnack, new TimeOnly(11, 0), 0.10),
            new SlotTemplate(3, MealType.Lunch, new TimeOnly(14, 0), 0.35),
            new SlotTemplate(4, MealType.AfternoonSnack, new TimeOnly(17, 0), 0.10),
            new SlotTemplate(5, MealType.Dinner, new TimeOnly(20, 0), 0.20),
        ],
    };

    public static IReadOnlyList<MealGenerationSlot> BuildSlots(int mealCount, DailyDietTargets dailyTargets)
    {
        if (!Templates.TryGetValue(mealCount, out var template))
        {
            throw new InvalidOperationException("Кількість прийомів їжі має бути від 1 до 5.");
        }

        return template
            .Select(slot => new MealGenerationSlot(
                MealOrder: slot.MealOrder,
                MealType: slot.MealType,
                MealTime: slot.MealTime,
                TargetCalories: dailyTargets.DailyCalories * slot.Ratio,
                TargetProteins: dailyTargets.ProteinsGrams * slot.Ratio,
                TargetFats: dailyTargets.FatsGrams * slot.Ratio,
                TargetCarbs: dailyTargets.CarbsGrams * slot.Ratio))
            .ToList();
    }
}
