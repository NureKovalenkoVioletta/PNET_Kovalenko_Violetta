using DietaryFitnessProject.DAL.Enums;

namespace DietaryFitnessProject.BLL.Models;

internal readonly record struct MealGenerationSlot(
    int MealOrder,
    MealType MealType,
    TimeOnly MealTime,
    double TargetCalories,
    double TargetProteins,
    double TargetFats,
    double TargetCarbs);
