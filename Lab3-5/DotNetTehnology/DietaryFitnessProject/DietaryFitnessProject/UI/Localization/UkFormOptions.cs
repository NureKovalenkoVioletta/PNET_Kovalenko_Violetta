using DietaryFitnessProject.DAL.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DietaryFitnessProject.UI.Localization;

public static class UkFormOptions
{
    public static IReadOnlyList<SelectListItem> GenderSelectList() =>
        Enum.GetValues<Gender>()
            .Select(v => new SelectListItem(GenderLabel(v), v.ToString()))
            .ToList();

    public static IReadOnlyList<SelectListItem> ActivityLevelSelectList() =>
        Enum.GetValues<ActivityLevel>()
            .Select(v => new SelectListItem(ActivityLevelLabel(v), v.ToString()))
            .ToList();

    public static IReadOnlyList<SelectListItem> GoalTypeSelectList() =>
        Enum.GetValues<GoalType>()
            .Select(v => new SelectListItem(GoalTypeLabel(v), v.ToString()))
            .ToList();

    public static string GenderLabel(Gender value) => value switch
    {
        Gender.Female => "Жінка",
        Gender.Male => "Чоловік",
        _ => value.ToString()
    };

    public static string ActivityLevelLabel(ActivityLevel value) => value switch
    {
        ActivityLevel.Sedentary => "Малорухливий спосіб життя",
        ActivityLevel.Light => "Легка активність",
        ActivityLevel.Moderate => "Помірна активність",
        ActivityLevel.Active => "Висока активність",
        ActivityLevel.VeryActive => "Дуже висока активність",
        _ => value.ToString()
    };

    public static string GoalTypeLabel(GoalType value) => value switch
    {
        GoalType.LoseWeight => "Схуднення",
        GoalType.MaintainWeight => "Підтримка ваги",
        GoalType.GainWeight => "Набір маси",
        _ => value.ToString()
    };
}
