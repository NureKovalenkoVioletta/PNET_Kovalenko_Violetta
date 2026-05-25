using System.Security.Claims;
using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.UI.Localization;
using DietaryFitnessProject.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DietaryFitnessProject.UI.Pages;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IUserProfileService _userProfileService;

    public ProfileModel(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [BindProperty]
    public UserProfileFormInputModel Input { get; set; } = new();

    public IReadOnlyCollection<SelectListItem> GenderOptions { get; private set; } = Array.Empty<SelectListItem>();
    public IReadOnlyCollection<SelectListItem> ActivityLevelOptions { get; private set; } = Array.Empty<SelectListItem>();
    public IReadOnlyCollection<SelectListItem> GoalOptions { get; private set; } = Array.Empty<SelectListItem>();

    public bool IsEditMode { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        IsEditMode = false;

        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        if (!await TryLoadProfileAsync(userId.Value, cancellationToken))
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnGetEditAsync(CancellationToken cancellationToken)
    {
        IsEditMode = true;
        PopulateOptions();

        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        if (!await TryLoadProfileAsync(userId.Value, cancellationToken))
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        IsEditMode = true;
        PopulateOptions();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Challenge();
        }

        var success = await _userProfileService.UpdateCurrentProfileAsync(
            userId.Value,
            new User
            {
                FirstName = Input.FirstName.Trim(),
                LastName = Input.LastName.Trim(),
                Email = Input.Email.Trim(),
                Gender = Input.Gender,
                Age = Input.Age,
                Height = Input.Height,
                Weight = Input.Weight,
                ActivityLevel = Input.ActivityLevel,
                Goal = Input.Goal
            },
            cancellationToken);

        if (!success)
        {
            return NotFound();
        }

        return LocalRedirect(Url.Content("~/Profile")!);
    }

    private async Task<bool> TryLoadProfileAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _userProfileService.GetCurrentProfileAsync(userId, cancellationToken);
        if (user is null)
        {
            return false;
        }

        Input = new UserProfileFormInputModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Age = user.Age,
            Height = user.Height,
            Weight = user.Weight,
            ActivityLevel = user.ActivityLevel,
            Goal = user.Goal
        };

        return true;
    }

    private int? GetCurrentUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(raw, out var id) ? id : null;
    }

    private void PopulateOptions()
    {
        GenderOptions = UkFormOptions.GenderSelectList();
        ActivityLevelOptions = UkFormOptions.ActivityLevelSelectList();
        GoalOptions = UkFormOptions.GoalTypeSelectList();
    }
}
