using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietaryFitnessProject.DAL.Data.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.ToTable("Meal", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_Meal_Calories_NonNegative", "[MealCalories] >= 0");
            tableBuilder.HasCheckConstraint("CK_Meal_Proteins_NonNegative", "[MealProteins] >= 0");
            tableBuilder.HasCheckConstraint("CK_Meal_Fats_NonNegative", "[MealFats] >= 0");
            tableBuilder.HasCheckConstraint("CK_Meal_Carbs_NonNegative", "[MealCarbs] >= 0");
        });

        builder.HasKey(x => x.MealId);
        builder.Property(x => x.MealId).HasColumnName("MealId").UseIdentityColumn();
        builder.Property(x => x.DailyDietPlanId).HasColumnName("DailyDietPlanId").IsRequired();
        builder.Property(x => x.MealOrder).HasColumnName("MealOrder").IsRequired();
        builder.Property(x => x.MealType).HasColumnName("MealType").HasConversion<int>().IsRequired();
        builder.Property(x => x.MealTime).HasColumnName("MealTime").HasColumnType("time").IsRequired();
        builder.Property(x => x.MealCalories).HasColumnName("MealCalories").HasColumnType("float").IsRequired();
        builder.Property(x => x.MealProteins).HasColumnName("MealProteins").HasColumnType("float").IsRequired();
        builder.Property(x => x.MealFats).HasColumnName("MealFats").HasColumnType("float").IsRequired();
        builder.Property(x => x.MealCarbs).HasColumnName("MealCarbs").HasColumnType("float").IsRequired();

        builder.HasIndex(x => new { x.DailyDietPlanId, x.MealOrder }).IsUnique();

        builder.HasOne(x => x.DailyDietPlan)
            .WithMany(x => x.Meals)
            .HasForeignKey(x => x.DailyDietPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
