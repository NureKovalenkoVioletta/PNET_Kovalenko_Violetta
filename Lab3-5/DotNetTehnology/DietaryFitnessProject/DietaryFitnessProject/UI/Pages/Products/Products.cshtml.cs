using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietaryFitnessProject.UI.Pages.Products;

[Authorize]
public class ProductsModel : PageModel
{
    private readonly IProductService _productService;

    public ProductsModel(IProductService productService)
    {
        _productService = productService;
    }

    public IReadOnlyList<Product> Products { get; private set; } = Array.Empty<Product>();

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var list = await _productService.GetAllAsync(cancellationToken);
        Products = list.OrderBy(p => p.ProductName, StringComparer.OrdinalIgnoreCase).ToList();
    }
}
