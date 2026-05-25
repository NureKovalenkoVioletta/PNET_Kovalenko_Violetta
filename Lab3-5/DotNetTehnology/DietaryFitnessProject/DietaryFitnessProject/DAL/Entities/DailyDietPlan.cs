namespace DietaryFitnessProject.DAL.Entities;

public class DailyDietPlan
{
    public int DailyDietPlanId { get; set; }
    public int UserId { get; set; }
    public double DailyPlanCalories { get; set; }
    public double DailyPlanProteins { get; set; }
    public double DailyPlanFats { get; set; }
    public double DailyPlanCarbs { get; set; }
    public DateOnly DailyPlanDay { get; set; }
    public int DailyPlanNumberOfMeals { get; set; } = 3;

    public User User { get; set; } = null!;
    public ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
