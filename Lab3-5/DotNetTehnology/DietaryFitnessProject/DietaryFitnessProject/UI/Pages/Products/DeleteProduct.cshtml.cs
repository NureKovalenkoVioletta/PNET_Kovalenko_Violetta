using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.Products;

[Authorize]
public class DeleteProductModel : PageModel
{
    private readonly IProductService _productService;

    public DeleteProductModel(IProductService productService)
    {
        _productService = productService;
    }

    [BindProperty]
    public int Id { get; set; }

    public Product? Product { get; private set; }

    public bool IsUsedInRecipes { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Id = id;
        Product = await _productService.GetByIdAsync(id, cancellationToken);
        if (Product is null)
        {
            return NotFound();
        }

        IsUsedInRecipes = await _productService.IsUsedInRecipesAsync(id, cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(Id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        IsUsedInRecipes = await _productService.IsUsedInRecipesAsync(Id, cancellationToken);

        try
        {
            await _productService.DeleteAsync(Id, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            Product = product;
            return Page();
        }

        return LocalRedirect(Url.Content("~/Products")!);
    }
}
