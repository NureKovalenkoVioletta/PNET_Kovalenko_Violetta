using System.Security.Claims;
using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.UI.Localization;
using DietaryFitnessProject.UI.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DietaryFitnessProject.UI.Pages.Auth;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly IAuthService _authService;

    public RegisterModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public RegisterInputModel Input { get; set; } = new();

    public IReadOnlyCollection<SelectListItem> GenderOptions { get; private set; } = Array.Empty<SelectListItem>();
    public IReadOnlyCollection<SelectListItem> ActivityLevelOptions { get; private set; } = Array.Empty<SelectListItem>();
    public IReadOnlyCollection<SelectListItem> GoalOptions { get; private set; } = Array.Empty<SelectListItem>();

    public void OnGet()
    {
        PopulateOptions();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        PopulateOptions();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = new User
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
            };

            var createdUser = await _authService.RegisterAsync(user, Input.Password, cancellationToken);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                BuildPrincipal(createdUser));

            return LocalRedirect(Url.Content("~/Profile")!);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private static ClaimsPrincipal BuildPrincipal(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
            new(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }

    private void PopulateOptions()
    {
        GenderOptions = UkFormOptions.GenderSelectList();
        ActivityLevelOptions = UkFormOptions.ActivityLevelSelectList();
        GoalOptions = UkFormOptions.GoalTypeSelectList();
    }
}
