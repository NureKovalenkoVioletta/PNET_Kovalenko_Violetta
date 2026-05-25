using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.Recipes;

[Authorize]
public class RecipesModel : PageModel
{
    private readonly IRecipeService _recipeService;

    public RecipesModel(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public IReadOnlyList<Recipe> RecipeList { get; private set; } = Array.Empty<Recipe>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var list = await _recipeService.GetAllAsync(cancellationToken);
        RecipeList = list.OrderBy(r => r.RecipeName, StringComparer.OrdinalIgnoreCase).ToList();
    }
}
