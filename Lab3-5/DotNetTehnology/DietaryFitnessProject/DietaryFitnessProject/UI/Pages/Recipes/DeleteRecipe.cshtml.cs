using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.Recipes;

[Authorize]
public class DeleteRecipeModel : PageModel
{
    private readonly IRecipeService _recipeService;

    public DeleteRecipeModel(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [BindProperty]
    public int Id { get; set; }

    public Recipe? Recipe { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Id = id;
        Recipe = await _recipeService.GetByIdAsync(id, cancellationToken);
        if (Recipe is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var recipe = await _recipeService.GetByIdAsync(Id, cancellationToken);
        if (recipe is null)
        {
            return NotFound();
        }

        await _recipeService.DeleteAsync(Id, cancellationToken);
        return LocalRedirect(Url.Content("~/Recipes")!);
    }
}
