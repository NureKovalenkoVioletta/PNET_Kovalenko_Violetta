using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DietaryFitnessProject.UI.Pages.Recipes;

[Authorize]
public class EditRecipeModel : PageModel
{
    private readonly IRecipeService _recipeService;
    private readonly IProductService _productService;

    public EditRecipeModel(IRecipeService recipeService, IProductService productService)
    {
        _recipeService = recipeService;
        _productService = productService;
    }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public RecipeFormInputModel Input { get; set; } = new();

    [BindProperty]
    public List<RecipeIngredientLineInputModel> IngredientLines { get; set; } = RecipeFormSlots.CreateInitial();

    public IReadOnlyList<SelectListItem> ProductOptions { get; private set; } = Array.Empty<SelectListItem>();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Id = id;
        ProductOptions = await RecipeFormSlots.BuildProductSelectAsync(_productService, cancellationToken);

        var recipe = await _recipeService.GetByIdWithIngredientsAsync(id, cancellationToken);
        if (recipe is null)
        {
            return NotFound();
        }

        Input = new RecipeFormInputModel
        {
            RecipeName = recipe.RecipeName,
            Instructions = recipe.RecipeInstructions,
            PortionCount = recipe.PortionCount,
        };

        var fromDb = recipe.RecipeProducts
            .OrderBy(rp => rp.ProductId)
            .Select(rp => new RecipeIngredientLineInputModel
            {
                ProductId = rp.ProductId,
                Grams = rp.Grams,
            });

        IngredientLines = RecipeFormSlots.FromRecipeLines(fromDb);

        if (IngredientLines.Count > RecipeFormSlots.MaxLineCount)
        {
            IngredientLines = IngredientLines.Take(RecipeFormSlots.MaxLineCount).ToList();
        }

        return Page();
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
            await _recipeService.UpdateWithIngredientsAsync(
                Id,
                Input.RecipeName.Trim(),
                Input.Instructions.Trim(),
                Input.PortionCount,
                lines,
                cancellationToken);

            return RedirectToPage("/Recipes/Details", new { id = Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
