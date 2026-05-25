using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<DailyDietPlan> DailyDietPlans => Set<DailyDietPlan>();
    public DbSet<Meal> Meals => Set<Meal>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<MealRecipe> MealRecipes => Set<MealRecipe>();
    public DbSet<RecipeProduct> RecipeProducts => Set<RecipeProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
