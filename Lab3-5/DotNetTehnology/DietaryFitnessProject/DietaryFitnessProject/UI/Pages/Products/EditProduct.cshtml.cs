using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.Products;

[Authorize]
public class EditProductModel : PageModel
{
    private readonly IProductService _productService;

    public EditProductModel(IProductService productService)
    {
        _productService = productService;
    }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public ProductInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Id = id;
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        Input = MapFromEntity(product);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var product = await _productService.GetByIdAsync(Id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        product.ProductName = Input.Name.Trim();
        product.ProductCaloriesPer100g = Input.CaloriesPer100g;
        product.ProductProteinsPer100g = Input.ProteinsPer100g;
        product.ProductFatsPer100g = Input.FatsPer100g;
        product.ProductCarbsPer100g = Input.CarbsPer100g;

        await _productService.UpdateAsync(product, cancellationToken);
        return LocalRedirect(Url.Content("~/Products")!);
    }

    private static ProductInputModel MapFromEntity(Product p) => new()
    {
        Name = p.ProductName,
        CaloriesPer100g = p.ProductCaloriesPer100g,
        ProteinsPer100g = p.ProductProteinsPer100g,
        FatsPer100g = p.ProductFatsPer100g,
        CarbsPer100g = p.ProductCarbsPer100g
    };
}
