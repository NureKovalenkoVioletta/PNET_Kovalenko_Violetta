using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietaryFitnessProject.DAL.Data.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipe", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_Recipe_PortionCount_MinOne", "[PortionCount] >= 1");
            tableBuilder.HasCheckConstraint("CK_Recipe_TotalGrams_NonNegative", "[RecipeTotalGrams] >= 0");
            tableBuilder.HasCheckConstraint("CK_Recipe_Calories_NonNegative", "[RecipeCalories] >= 0");
            tableBuilder.HasCheckConstraint("CK_Recipe_Proteins_NonNegative", "[RecipeProteins] >= 0");
            tableBuilder.HasCheckConstraint("CK_Recipe_Fats_NonNegative", "[RecipeFats] >= 0");
            tableBuilder.HasCheckConstraint("CK_Recipe_Carbs_NonNegative", "[RecipeCarbs] >= 0");
        });

        builder.HasKey(x => x.RecipeId);
        builder.Property(x => x.RecipeId).HasColumnName("RecipeId").UseIdentityColumn();
        builder.Property(x => x.RecipeName).HasColumnName("RecipeName").HasMaxLength(200).IsRequired();
        builder.Property(x => x.RecipeInstructions).HasColumnName("RecipeInstructions").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.PortionCount).HasColumnName("PortionCount").IsRequired().HasDefaultValue(1);
        builder.Property(x => x.RecipeTotalGrams).HasColumnName("RecipeTotalGrams").HasColumnType("float").IsRequired().HasDefaultValue(0.0);
        builder.Property(x => x.RecipeCalories).HasColumnName("RecipeCalories").HasColumnType("float").IsRequired();
        builder.Property(x => x.RecipeProteins).HasColumnName("RecipeProteins").HasColumnType("float").IsRequired();
        builder.Property(x => x.RecipeFats).HasColumnName("RecipeFats").HasColumnType("float").IsRequired();
        builder.Property(x => x.RecipeCarbs).HasColumnName("RecipeCarbs").HasColumnType("float").IsRequired();
    }
}
