namespace DietaryFitnessProject.DAL.Entities;

public class RecipeProduct
{
    public int RecipeId { get; set; }
    public int ProductId { get; set; }
    public double Grams { get; set; }

    public Recipe Recipe { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
