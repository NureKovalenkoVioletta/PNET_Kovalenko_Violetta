namespace DietaryFitnessProject.DAL.Entities;

public class Recipe
{
    public int RecipeId { get; set; }
    public string RecipeName { get; set; } = string.Empty;
    public string RecipeInstructions { get; set; } = string.Empty;
    public int PortionCount { get; set; } = 1;
    public double RecipeTotalGrams { get; set; }
    public double RecipeCalories { get; set; }
    public double RecipeProteins { get; set; }
    public double RecipeFats { get; set; }
    public double RecipeCarbs { get; set; }

    public ICollection<MealRecipe> MealRecipes { get; set; } = new List<MealRecipe>();
    public ICollection<RecipeProduct> RecipeProducts { get; set; } = new List<RecipeProduct>();
}
