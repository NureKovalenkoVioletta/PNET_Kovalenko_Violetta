using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.UI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DietaryFitnessProject.UI.Pages.Recipes;

internal static class RecipeFormSlots
{
    public const int InitialLineCount = 2;

    public const int MaxLineCount = 40;

    public static List<RecipeIngredientLineInputModel> CreateInitial()
    {
        return Enumerable.Range(0, InitialLineCount).Select(_ => new RecipeIngredientLineInputModel()).ToList();
    }


    public static List<RecipeIngredientLineInputModel> CoerceIngredientLines(List<RecipeIngredientLineInputModel>? list)
    {
        list ??= new List<RecipeIngredientLineInputModel>();
        while (list.Count < InitialLineCount)
        {
            list.Add(new RecipeIngredientLineInputModel());
        }

        return list;
    }
    
    public static List<RecipeIngredientLineInputModel> FromRecipeLines(IEnumerable<RecipeIngredientLineInputModel> lines)
    {
        var list = lines.ToList();
        return CoerceIngredientLines(list);
    }

    public static IReadOnlyList<RecipeIngredientInput> ToBLLLines(IReadOnlyList<RecipeIngredientLineInputModel> slots)
    {
        return slots
            .Where(l => l.ProductId > 0 && l.Grams > 0)
            .Select(l => new RecipeIngredientInput(l.ProductId, l.Grams))
            .ToList();
    }

    public static async Task<IReadOnlyList<SelectListItem>> BuildProductSelectAsync(
        IProductService productService,
        CancellationToken cancellationToken)
    {
        var products = await productService.GetAllAsync(cancellationToken);
        var items = products
            .OrderBy(p => p.ProductName, StringComparer.OrdinalIgnoreCase)
            .Select(p => new SelectListItem { Value = p.ProductId.ToString(), Text = p.ProductName })
            .ToList();
        items.Insert(0, new SelectListItem { Value = "0", Text = "— оберіть продукт —" });
        return items;
    }
}
