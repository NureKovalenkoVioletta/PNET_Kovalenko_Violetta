using System.Security.Claims;
using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.DailyPlans;

[Authorize]
public class DeletePlanModel : PageModel
{
    private readonly IDailyDietPlanService _dailyDietPlanService;
    private readonly IMealService _mealService;

    public DeletePlanModel(
        IDailyDietPlanService dailyDietPlanService,
        IMealService mealService)
    {
        _dailyDietPlanService = dailyDietPlanService;
        _mealService = mealService;
    }

    [BindProperty]
    public int Id { get; set; }

    public DailyDietPlan? Plan { get; private set; }

    public bool HasGeneratedMeals { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        Id = id;
        Plan = await _dailyDietPlanService.GetByIdForUserAsync(id, userId.Value, cancellationToken);
        if (Plan is null)
        {
            return NotFound();
        }

        HasGeneratedMeals = (await _mealService.GetByPlanIdAsync(id, cancellationToken)).Count > 0;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        var plan = await _dailyDietPlanService.GetByIdForUserAsync(Id, userId.Value, cancellationToken);
        if (plan is null)
        {
            return NotFound();
        }

        await _mealService.DeleteAllForPlanAsync(Id, cancellationToken);
        await _dailyDietPlanService.DeleteAsync(Id, cancellationToken);

        return RedirectToPage("/DailyPlans/DailyPlans");
    }

    private int? GetCurrentUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }
}
