using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.Recipes;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IRecipeService _recipeService;

    public DetailsModel(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public Recipe? Recipe { get; private set; }

    public double PerPortionCalories { get; private set; }
    public double PerPortionProteins { get; private set; }
    public double PerPortionFats { get; private set; }
    public double PerPortionCarbs { get; private set; }
    public double PerPortionGrams { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var recipe = await _recipeService.GetByIdWithIngredientsAsync(id, cancellationToken);
        if (recipe is null)
        {
            return NotFound();
        }

        Recipe = recipe;
        var portions = recipe.PortionCount >= 1 ? recipe.PortionCount : 1;
        PerPortionCalories = recipe.RecipeCalories / portions;
        PerPortionProteins = recipe.RecipeProteins / portions;
        PerPortionFats = recipe.RecipeFats / portions;
        PerPortionCarbs = recipe.RecipeCarbs / portions;
        PerPortionGrams = recipe.RecipeTotalGrams / portions;

        return Page();
    }
}
