using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietaryFitnessProject.DAL.Data.Configurations;

public class MealRecipeConfiguration : IEntityTypeConfiguration<MealRecipe>
{
    public void Configure(EntityTypeBuilder<MealRecipe> builder)
    {
        builder.ToTable("MealRecipe");

        builder.HasKey(x => new { x.MealId, x.RecipeId });
        builder.Property(x => x.MealId).HasColumnName("MealId");
        builder.Property(x => x.RecipeId).HasColumnName("RecipeId");

        builder.HasOne(x => x.Meal)
            .WithMany(x => x.MealRecipes)
            .HasForeignKey(x => x.MealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Recipe)
            .WithMany(x => x.MealRecipes)
            .HasForeignKey(x => x.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
