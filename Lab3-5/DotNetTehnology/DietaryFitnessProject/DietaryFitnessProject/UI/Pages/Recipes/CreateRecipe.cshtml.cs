using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DietaryFitnessProject.UI.Pages.Recipes;

[Authorize]
public class CreateRecipeModel : PageModel
{
    private readonly IRecipeService _recipeService;
    private readonly IProductService _productService;

    public CreateRecipeModel(IRecipeService recipeService, IProductService productService)
    {
        _recipeService = recipeService;
        _productService = productService;
    }

    [BindProperty]
    public RecipeFormInputModel Input { get; set; } = new();

    [BindProperty]
    public List<RecipeIngredientLineInputModel> IngredientLines { get; set; } = RecipeFormSlots.CreateInitial();

    public IReadOnlyList<SelectListItem> ProductOptions { get; private set; } = Array.Empty<SelectListItem>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        IngredientLines = RecipeFormSlots.CreateInitial();
        ProductOptions = await RecipeFormSlots.BuildProductSelectAsync(_productService, cancellationToken);
    }

    public async Task<IActionResult> OnPostAddRowAsync(CancellationToken cancellationToken)
    {
        ModelState.Clear();
        IngredientLines = RecipeFormSlots.CoerceIngredientLines(IngredientLines);
        if (IngredientLines.Count >= RecipeFormSlots.MaxLineCount)
        {
            ModelState.AddModelError(
                string.Empty,
                $"Неможливо додати більше {RecipeFormSlots.MaxLineCount} рядків інгредієнтів.");
        }
        else
        {
            IngredientLines.Add(new RecipeIngredientLineInputModel());
        }

        ProductOptions = await RecipeFormSlots.BuildProductSelectAsync(_productService, cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveLastRowAsync(CancellationToken cancellationToken)
    {
        ModelState.Clear();
        IngredientLines = RecipeFormSlots.CoerceIngredientLines(IngredientLines);
        if (IngredientLines.Count > RecipeFormSlots.InitialLineCount)
        {
            IngredientLines.RemoveAt(IngredientLines.Count - 1);
        }

        ProductOptions = await RecipeFormSlots.BuildProductSelectAsync(_productService, cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        IngredientLines = RecipeFormSlots.CoerceIngredientLines(IngredientLines);
        ProductOptions = await RecipeFormSlots.BuildProductSelectAsync(_productService, cancellationToken);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var lines = RecipeFormSlots.ToBLLLines(IngredientLines);
        if (lines.Count == 0)
        {
            ModelState.AddModelError(
                string.Empty,
                "Додайте хоча б один інгредієнт: оберіть продукт і вкажіть масу більше 0.");
            return Page();
        }

        try
        {
            var recipe = await _recipeService.CreateWithIngredientsAsync(
                Input.RecipeName.Trim(),
                Input.Instructions.Trim(),
                Input.PortionCount,
                lines,
                cancellationToken);

            return RedirectToPage("/Recipes/Details", new { id = recipe.RecipeId });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
