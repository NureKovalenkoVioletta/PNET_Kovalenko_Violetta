using DietaryFitnessProject.DAL.Enums;

namespace DietaryFitnessProject.DAL.Entities;

public class Meal
{
    public int MealId { get; set; }
    public int DailyDietPlanId { get; set; }
    public int MealOrder { get; set; }
    public MealType MealType { get; set; }
    public TimeOnly MealTime { get; set; }
    public double MealCalories { get; set; }
    public double MealProteins { get; set; }
    public double MealFats { get; set; }
    public double MealCarbs { get; set; }

    public DailyDietPlan DailyDietPlan { get; set; } = null!;
    public ICollection<MealRecipe> MealRecipes { get; set; } = new List<MealRecipe>();
}
