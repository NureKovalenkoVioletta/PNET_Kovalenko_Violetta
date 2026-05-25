using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.DailyPlans;

[Authorize]
public class PlanModel : PageModel
{
    private readonly IDailyDietPlanService _dailyDietPlanService;
    private readonly IMealService _mealService;

    public PlanModel(
        IDailyDietPlanService dailyDietPlanService,
        IMealService mealService)
    {
        _dailyDietPlanService = dailyDietPlanService;
        _mealService = mealService;
    }

    [BindProperty]
    [Display(Name = "Дата плану")]
    public DateOnly PlanDate { get; set; }

    [BindProperty]
    [Display(Name = "Кількість прийомів їжі")]
    [Range(1, 5, ErrorMessage = "Оберіть від {1} до {2} прийомів їжі.")]
    public int MealCount { get; set; } = 3;

    public DailyDietPlan? SavedPlan { get; private set; }

    public IReadOnlyList<MealDisplayItem> GeneratedMeals { get; private set; } = Array.Empty<MealDisplayItem>();

    public bool ShowSavedSummary => SavedPlan is not null;

    public async Task<IActionResult> OnGetAsync([FromQuery] string? date, CancellationToken cancellationToken)
    {
        PlanDate = ParseDateQueryOrToday(date);

        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        await LoadPageStateAsync(userId.Value, cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            await LoadPageStateAsync(userId.Value, cancellationToken);
            return Page();
        }

        try
        {
            await _dailyDietPlanService.CreatePlanForUserAsync(
                userId.Value,
                PlanDate,
                MealCount,
                cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadPageStateAsync(userId.Value, cancellationToken);
            return Page();
        }

        return RedirectToPage("/DailyPlans/Plan", new { date = PlanDate.ToString("yyyy-MM-dd") });
    }

    public async Task<IActionResult> OnPostGenerateMealsAsync(int dailyDietPlanId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        var plan = await _dailyDietPlanService.GetByIdForUserAsync(dailyDietPlanId, userId.Value, cancellationToken);
        if (plan is null)
        {
            return NotFound();
        }

        try
        {
            await _mealService.GenerateForPlanAsync(dailyDietPlanId, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await BindPlanStateAsync(plan, cancellationToken);
            return Page();
        }

        return RedirectToPage("/DailyPlans/Plan", new { date = plan.DailyPlanDay.ToString("yyyy-MM-dd") });
    }

    public async Task<IActionResult> OnPostDeleteMealsAsync(int dailyDietPlanId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        var plan = await _dailyDietPlanService.GetByIdForUserAsync(dailyDietPlanId, userId.Value, cancellationToken);
        if (plan is null)
        {
            return NotFound();
        }

        await _mealService.DeleteAllForPlanAsync(dailyDietPlanId, cancellationToken);

        return RedirectToPage("/DailyPlans/Plan", new { date = plan.DailyPlanDay.ToString("yyyy-MM-dd") });
    }

    private async Task LoadPageStateAsync(int userId, CancellationToken cancellationToken)
    {
        SavedPlan = await _dailyDietPlanService.GetByUserAndDateAsync(userId, PlanDate, cancellationToken);
        if (SavedPlan is null)
        {
            GeneratedMeals = Array.Empty<MealDisplayItem>();
            return;
        }

        await BindPlanStateAsync(SavedPlan, cancellationToken);
    }

    private async Task BindPlanStateAsync(DailyDietPlan plan, CancellationToken cancellationToken)
    {
        SavedPlan = plan;
        MealCount = plan.DailyPlanNumberOfMeals;
        PlanDate = plan.DailyPlanDay;
        GeneratedMeals = await _mealService.GetDisplayItemsForPlanAsync(plan.DailyDietPlanId, cancellationToken);
    }

    private static DateOnly ParseDateQueryOrToday(string? date)
    {
        if (!string.IsNullOrWhiteSpace(date) && DateOnly.TryParse(date, out var parsed))
        {
            return parsed;
        }

        return DateOnly.FromDateTime(DateTime.Today);
    }

    private int? GetCurrentUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
