using DietaryFitnessProject.DAL.Enums;

namespace DietaryFitnessProject.DAL.Entities;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public GoalType Goal { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastLoginAtUtc { get; set; }

    public ICollection<DailyDietPlan> DailyDietPlans { get; set; } = new List<DailyDietPlan>();
}
