using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.Products;

[Authorize]
public class CreateProductModel : PageModel
{
    private readonly IProductService _productService;

    public CreateProductModel(IProductService productService)
    {
        _productService = productService;
    }

    [BindProperty]
    public ProductInputModel Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var product = new Product
        {
            ProductName = Input.Name.Trim(),
            ProductCaloriesPer100g = Input.CaloriesPer100g,
            ProductProteinsPer100g = Input.ProteinsPer100g,
            ProductFatsPer100g = Input.FatsPer100g,
            ProductCarbsPer100g = Input.CarbsPer100g
        };

        await _productService.AddAsync(product, cancellationToken);
        return LocalRedirect(Url.Content("~/Products")!);
    }
}
