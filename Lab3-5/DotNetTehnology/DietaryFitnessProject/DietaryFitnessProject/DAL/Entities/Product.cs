namespace DietaryFitnessProject.DAL.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public double ProductCaloriesPer100g { get; set; }
    public double ProductProteinsPer100g { get; set; }
    public double ProductFatsPer100g { get; set; }
    public double ProductCarbsPer100g { get; set; }

    public ICollection<RecipeProduct> RecipeProducts { get; set; } = new List<RecipeProduct>();
}
