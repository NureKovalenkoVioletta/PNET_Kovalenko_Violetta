using DietaryFitnessProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietaryFitnessProject.DAL.Data.Configurations;

public class DailyDietPlanConfiguration : IEntityTypeConfiguration<DailyDietPlan>
{
    public void Configure(EntityTypeBuilder<DailyDietPlan> builder)
    {
        builder.ToTable("DailyDietPlan", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_DailyDietPlan_Calories_NonNegative", "[DailyPlanCalories] >= 0");
            tableBuilder.HasCheckConstraint("CK_DailyDietPlan_Proteins_NonNegative", "[DailyPlanProteins] >= 0");
            tableBuilder.HasCheckConstraint("CK_DailyDietPlan_Fats_NonNegative", "[DailyPlanFats] >= 0");
            tableBuilder.HasCheckConstraint("CK_DailyDietPlan_Carbs_NonNegative", "[DailyPlanCarbs] >= 0");
            tableBuilder.HasCheckConstraint(
                "CK_DailyDietPlan_NumberOfMeals_Range",
                "[DailyPlanNumberOfMeals] >= 1 AND [DailyPlanNumberOfMeals] <= 5");
        });

        builder.HasKey(x => x.DailyDietPlanId);
        builder.Property(x => x.DailyDietPlanId).HasColumnName("DailyDietPlanId").UseIdentityColumn();
        builder.Property(x => x.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(x => x.DailyPlanCalories).HasColumnName("DailyPlanCalories").HasColumnType("float").IsRequired();
        builder.Property(x => x.DailyPlanProteins).HasColumnName("DailyPlanProteins").HasColumnType("float").IsRequired();
        builder.Property(x => x.DailyPlanFats).HasColumnName("DailyPlanFats").HasColumnType("float").IsRequired();
        builder.Property(x => x.DailyPlanCarbs).HasColumnName("DailyPlanCarbs").HasColumnType("float").IsRequired();
        builder.Property(x => x.DailyPlanDay).HasColumnName("DailyPlanDay").HasColumnType("date").IsRequired();
        builder.Property(x => x.DailyPlanNumberOfMeals).HasColumnName("DailyPlanNumberOfMeals").IsRequired();

        builder.HasIndex(x => new { x.UserId, x.DailyPlanDay }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.DailyDietPlans)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
