using System;
using DietaryFitnessProject.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DietaryFitnessProject.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260510151836_AddRecipeNamePortionCountTotalGrams")]
    partial class AddRecipeNamePortionCountTotalGrams
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.DailyDietPlan", b =>
                {
                    b.Property<int>("DailyDietPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DailyDietPlanId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DailyDietPlanId"));

                    b.Property<double>("DailyPlanCalories")
                        .HasColumnType("float")
                        .HasColumnName("DailyPlanCalories");

                    b.Property<double>("DailyPlanCarbs")
                        .HasColumnType("float")
                        .HasColumnName("DailyPlanCarbs");

                    b.Property<DateOnly>("DailyPlanDay")
                        .HasColumnType("date")
                        .HasColumnName("DailyPlanDay");

                    b.Property<double>("DailyPlanFats")
                        .HasColumnType("float")
                        .HasColumnName("DailyPlanFats");

                    b.Property<int>("DailyPlanNumberOfMeals")
                        .HasColumnType("int")
                        .HasColumnName("DailyPlanNumberOfMeals");

                    b.Property<double>("DailyPlanProteins")
                        .HasColumnType("float")
                        .HasColumnName("DailyPlanProteins");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserId");

                    b.HasKey("DailyDietPlanId");

                    b.HasIndex("UserId", "DailyPlanDay")
                        .IsUnique();

                    b.ToTable("DailyDietPlan", null, t =>
                        {
                            t.HasCheckConstraint("CK_DailyDietPlan_Calories_NonNegative", "[DailyPlanCalories] >= 0");

                            t.HasCheckConstraint("CK_DailyDietPlan_Carbs_NonNegative", "[DailyPlanCarbs] >= 0");

                            t.HasCheckConstraint("CK_DailyDietPlan_Fats_NonNegative", "[DailyPlanFats] >= 0");

                            t.HasCheckConstraint("CK_DailyDietPlan_Proteins_NonNegative", "[DailyPlanProteins] >= 0");
                        });
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.Meal", b =>
                {
                    b.Property<int>("MealId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MealId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MealId"));

                    b.Property<int>("DailyDietPlanId")
                        .HasColumnType("int")
                        .HasColumnName("DailyDietPlanId");

                    b.Property<double>("MealCalories")
                        .HasColumnType("float")
                        .HasColumnName("MealCalories");

                    b.Property<double>("MealCarbs")
                        .HasColumnType("float")
                        .HasColumnName("MealCarbs");

                    b.Property<double>("MealFats")
                        .HasColumnType("float")
                        .HasColumnName("MealFats");

                    b.Property<int>("MealOrder")
                        .HasColumnType("int")
                        .HasColumnName("MealOrder");

                    b.Property<double>("MealProteins")
                        .HasColumnType("float")
                        .HasColumnName("MealProteins");

                    b.Property<TimeOnly>("MealTime")
                        .HasColumnType("time")
                        .HasColumnName("MealTime");

                    b.Property<int>("MealType")
                        .HasColumnType("int")
                        .HasColumnName("MealType");

                    b.HasKey("MealId");

                    b.HasIndex("DailyDietPlanId", "MealOrder")
                        .IsUnique();

                    b.ToTable("Meal", null, t =>
                        {
                            t.HasCheckConstraint("CK_Meal_Calories_NonNegative", "[MealCalories] >= 0");

                            t.HasCheckConstraint("CK_Meal_Carbs_NonNegative", "[MealCarbs] >= 0");

                            t.HasCheckConstraint("CK_Meal_Fats_NonNegative", "[MealFats] >= 0");

                            t.HasCheckConstraint("CK_Meal_Proteins_NonNegative", "[MealProteins] >= 0");
                        });
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.MealRecipe", b =>
                {
                    b.Property<int>("MealId")
                        .HasColumnType("int")
                        .HasColumnName("MealId");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int")
                        .HasColumnName("RecipeId");

                    b.HasKey("MealId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("MealRecipe", (string)null);
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProductId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<double>("ProductCaloriesPer100g")
                        .HasColumnType("float")
                        .HasColumnName("ProductCaloriesPer100g");

                    b.Property<double>("ProductCarbsPer100g")
                        .HasColumnType("float")
                        .HasColumnName("ProductCarbsPer100g");

                    b.Property<double>("ProductFatsPer100g")
                        .HasColumnType("float")
                        .HasColumnName("ProductFatsPer100g");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ProductName");

                    b.Property<double>("ProductProteinsPer100g")
                        .HasColumnType("float")
                        .HasColumnName("ProductProteinsPer100g");

                    b.HasKey("ProductId");

                    b.ToTable("Product", null, t =>
                        {
                            t.HasCheckConstraint("CK_Product_CaloriesPer100g_NonNegative", "[ProductCaloriesPer100g] >= 0");

                            t.HasCheckConstraint("CK_Product_CarbsPer100g_NonNegative", "[ProductCarbsPer100g] >= 0");

                            t.HasCheckConstraint("CK_Product_FatsPer100g_NonNegative", "[ProductFatsPer100g] >= 0");

                            t.HasCheckConstraint("CK_Product_ProteinsPer100g_NonNegative", "[ProductProteinsPer100g] >= 0");
                        });
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RecipeId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecipeId"));

                    b.Property<int>("PortionCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1)
                        .HasColumnName("PortionCount");

                    b.Property<double>("RecipeCalories")
                        .HasColumnType("float")
                        .HasColumnName("RecipeCalories");

                    b.Property<double>("RecipeCarbs")
                        .HasColumnType("float")
                        .HasColumnName("RecipeCarbs");

                    b.Property<double>("RecipeFats")
                        .HasColumnType("float")
                        .HasColumnName("RecipeFats");

                    b.Property<string>("RecipeInstructions")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("RecipeInstructions");

                    b.Property<string>("RecipeName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("RecipeName");

                    b.Property<double>("RecipeProteins")
                        .HasColumnType("float")
                        .HasColumnName("RecipeProteins");

                    b.Property<double>("RecipeTotalGrams")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0)
                        .HasColumnName("RecipeTotalGrams");

                    b.HasKey("RecipeId");

                    b.ToTable("Recipe", null, t =>
                        {
                            t.HasCheckConstraint("CK_Recipe_Calories_NonNegative", "[RecipeCalories] >= 0");

                            t.HasCheckConstraint("CK_Recipe_Carbs_NonNegative", "[RecipeCarbs] >= 0");

                            t.HasCheckConstraint("CK_Recipe_Fats_NonNegative", "[RecipeFats] >= 0");

                            t.HasCheckConstraint("CK_Recipe_PortionCount_MinOne", "[PortionCount] >= 1");

                            t.HasCheckConstraint("CK_Recipe_Proteins_NonNegative", "[RecipeProteins] >= 0");

                            t.HasCheckConstraint("CK_Recipe_TotalGrams_NonNegative", "[RecipeTotalGrams] >= 0");
                        });
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.RecipeProduct", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("int")
                        .HasColumnName("RecipeId");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("ProductId");

                    b.Property<double>("Grams")
                        .HasColumnType("float")
                        .HasColumnName("Grams");

                    b.HasKey("RecipeId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("RecipeProduct", null, t =>
                        {
                            t.HasCheckConstraint("CK_RecipeProduct_Grams_NonNegative", "[Grams] >= 0");
                        });
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<int>("ActivityLevel")
                        .HasColumnType("int")
                        .HasColumnName("ActivityLevel");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAtUtc");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("FirstName");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("Gender");

                    b.Property<int>("Goal")
                        .HasColumnType("int")
                        .HasColumnName("Goal");

                    b.Property<double>("Height")
                        .HasColumnType("float")
                        .HasColumnName("Height");

                    b.Property<DateTime?>("LastLoginAtUtc")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastLoginAtUtc");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("LastName");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)")
                        .HasColumnName("PasswordHash");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("PasswordSalt");

                    b.Property<double>("Weight")
                        .HasColumnType("float")
                        .HasColumnName("Weight");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User", null, t =>
                        {
                            t.HasCheckConstraint("CK_User_Height_NonNegative", "[Height] >= 0");

                            t.HasCheckConstraint("CK_User_Weight_NonNegative", "[Weight] >= 0");
                        });
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.DailyDietPlan", b =>
                {
                    b.HasOne("DietaryFitnessProject.DAL.Entities.User", "User")
                        .WithMany("DailyDietPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.Meal", b =>
                {
                    b.HasOne("DietaryFitnessProject.DAL.Entities.DailyDietPlan", "DailyDietPlan")
                        .WithMany("Meals")
                        .HasForeignKey("DailyDietPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DailyDietPlan");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.MealRecipe", b =>
                {
                    b.HasOne("DietaryFitnessProject.DAL.Entities.Meal", "Meal")
                        .WithMany("MealRecipes")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DietaryFitnessProject.DAL.Entities.Recipe", "Recipe")
                        .WithMany("MealRecipes")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meal");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.RecipeProduct", b =>
                {
                    b.HasOne("DietaryFitnessProject.DAL.Entities.Product", "Product")
                        .WithMany("RecipeProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DietaryFitnessProject.DAL.Entities.Recipe", "Recipe")
                        .WithMany("RecipeProducts")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.DailyDietPlan", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.Meal", b =>
                {
                    b.Navigation("MealRecipes");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.Product", b =>
                {
                    b.Navigation("RecipeProducts");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.Recipe", b =>
                {
                    b.Navigation("MealRecipes");

                    b.Navigation("RecipeProducts");
                });

            modelBuilder.Entity("DietaryFitnessProject.DAL.Entities.User", b =>
                {
                    b.Navigation("DailyDietPlans");
                });
#pragma warning restore 612, 618
        }
    }
}
