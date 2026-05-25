using System.Security.Claims;
using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.DailyPlans;

[Authorize]
public class DailyPlansModel : PageModel
{
    private readonly IDailyDietPlanService _dailyDietPlanService;

    public DailyPlansModel(IDailyDietPlanService dailyDietPlanService)
    {
        _dailyDietPlanService = dailyDietPlanService;
    }

    public IReadOnlyList<DailyDietPlan> Plans { get; private set; } = Array.Empty<DailyDietPlan>();

    public bool HasPlanForToday { get; private set; }

    public string PlanDayPageUrl { get; private set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        var list = await _dailyDietPlanService.GetByUserIdAsync(userId.Value, cancellationToken);
        Plans = list.ToList();

        var today = DateOnly.FromDateTime(DateTime.Today);
        HasPlanForToday = Plans.Any(p => p.DailyPlanDay == today);
        PlanDayPageUrl = BuildPlanDayPageUrl(today);

        return Page();
    }

    private string BuildPlanDayPageUrl(DateOnly today)
    {
        if (!HasPlanForToday)
        {
            return Url.Page("/DailyPlans/Plan", new { date = today.ToString("yyyy-MM-dd") })
                   ?? Url.Content("~/DailyPlans/Plan")!;
        }

        DateOnly? firstFree = null;
        for (var i = 1; i <= 366; i++)
        {
            var d = today.AddDays(i);
            if (!Plans.Any(p => p.DailyPlanDay == d))
            {
                firstFree = d;
                break;
            }
        }

        var target = firstFree ?? today.AddDays(1);
        return Url.Page("/DailyPlans/Plan", new { date = target.ToString("yyyy-MM-dd") })
               ?? Url.Content("~/DailyPlans/Plan")!;
    }

    private int? GetCurrentUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
