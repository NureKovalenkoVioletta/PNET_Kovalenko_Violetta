using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Services;

internal static class MealRecipeCandidateSelector
{
    private const int DefaultMaxRecipesPerMeal = 3;
    private const int DefaultTopK = 5;
    private const double DefaultUnderfillThresholdRatio = 0.20;

    public static GeneratedMealDraft BuildDraftForSlot(
        MealGenerationSlot slot,
        IReadOnlyCollection<Recipe> recipes,
        int maxRecipesPerMeal = DefaultMaxRecipesPerMeal,
        int topK = DefaultTopK,
        double underfillThresholdRatio = DefaultUnderfillThresholdRatio,
        int? selectionSeed = null)
    {
        var selectedCandidates = SelectBestCandidatesForMeal(
            slot,
            recipes,
            maxRecipesPerMeal,
            topK,
            underfillThresholdRatio,
            selectionSeed);

        var calories = selectedCandidates.Sum(x => x.Calories);
        var proteins = selectedCandidates.Sum(x => x.Proteins);
        var fats = selectedCandidates.Sum(x => x.Fats);
        var carbs = selectedCandidates.Sum(x => x.Carbs);

        return new GeneratedMealDraft(
            MealOrder: slot.MealOrder,
            MealType: slot.MealType,
            MealTime: slot.MealTime,
            MealCalories: calories,
            MealProteins: proteins,
            MealFats: fats,
            MealCarbs: carbs,
            RecipeIds: selectedCandidates.Select(x => x.RecipeId).ToList());
    }

    public static IReadOnlyList<RecipePortionCandidate> SelectBestCandidatesForMeal(
        MealGenerationSlot slot,
        IReadOnlyCollection<Recipe> recipes,
        int maxRecipesPerMeal = DefaultMaxRecipesPerMeal,
        int topK = DefaultTopK,
        double underfillThresholdRatio = DefaultUnderfillThresholdRatio,
        int? selectionSeed = null)
    {
        if (recipes.Count == 0)
        {
            throw new InvalidOperationException("Немає рецептів для генерації прийомів їжі.");
        }

        if (maxRecipesPerMeal < 1)
        {
            throw new InvalidOperationException("Максимальна кількість рецептів має бути не менше 1.");
        }

        if (topK < 1)
        {
            throw new InvalidOperationException("Кількість кандидатів для вибору має бути не менше 1.");
        }

        if (underfillThresholdRatio < 0 || underfillThresholdRatio >= 1)
        {
            throw new InvalidOperationException("Поріг недобору калорій має бути від 0 до 1 (не включно 1).");
        }

        var candidates = new List<(RecipePortionCandidate Candidate, double MacroDeviation)>();

        foreach (var recipe in recipes)
        {
            if (recipe.PortionCount <= 0)
            {
                continue;
            }

            var onePortionCalories = recipe.RecipeCalories / recipe.PortionCount;
            var onePortionProteins = recipe.RecipeProteins / recipe.PortionCount;
            var onePortionFats = recipe.RecipeFats / recipe.PortionCount;
            var onePortionCarbs = recipe.RecipeCarbs / recipe.PortionCount;

            var score = Math.Abs(onePortionCalories - slot.TargetCalories);
            var macroDeviation =
                Math.Abs(onePortionProteins - slot.TargetProteins) +
                Math.Abs(onePortionFats - slot.TargetFats) +
                Math.Abs(onePortionCarbs - slot.TargetCarbs);

            var candidate = new RecipePortionCandidate(
                RecipeId: recipe.RecipeId,
                RecipeName: recipe.RecipeName,
                Portions: 1,
                Calories: onePortionCalories,
                Proteins: onePortionProteins,
                Fats: onePortionFats,
                Carbs: onePortionCarbs,
                Score: score);

            candidates.Add((candidate, macroDeviation));
        }

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException("Не знайдено валідних рецептів: у рецептів некоректний PortionCount.");
        }

        var orderedByAnchor = candidates
            .OrderBy(x => x.Candidate.Score)
            .ThenBy(x => x.MacroDeviation)
            .ThenBy(x => x.Candidate.RecipeId)
            .ToList();

        var take = Math.Min(topK, orderedByAnchor.Count);
        if (selectionSeed is null || take == 1)
        {
            return BuildMealByAnchorAndTopUp(slot, orderedByAnchor[0].Candidate, orderedByAnchor, maxRecipesPerMeal, underfillThresholdRatio);
        }

        var rng = new Random(selectionSeed.Value);
        var pickIndex = rng.Next(0, take);
        return BuildMealByAnchorAndTopUp(slot, orderedByAnchor[pickIndex].Candidate, orderedByAnchor, maxRecipesPerMeal, underfillThresholdRatio);
    }

    private static IReadOnlyList<RecipePortionCandidate> BuildMealByAnchorAndTopUp(
        MealGenerationSlot slot,
        RecipePortionCandidate anchor,
        IReadOnlyCollection<(RecipePortionCandidate Candidate, double MacroDeviation)> candidates,
        int maxRecipesPerMeal,
        double underfillThresholdRatio)
    {
        var selected = new List<RecipePortionCandidate> { anchor };
        var remaining = candidates
            .Select(x => x.Candidate)
            .Where(x => x.RecipeId != anchor.RecipeId)
            .ToList();

        while (selected.Count < maxRecipesPerMeal)
        {
            var currentCalories = selected.Sum(x => x.Calories);
            var currentProteins = selected.Sum(x => x.Proteins);
            var currentFats = selected.Sum(x => x.Fats);
            var currentCarbs = selected.Sum(x => x.Carbs);

            var caloriesDeficit = slot.TargetCalories - currentCalories;
            var underfillThreshold = slot.TargetCalories * underfillThresholdRatio;
            if (caloriesDeficit <= underfillThreshold)
            {
                break;
            }

            if (remaining.Count == 0)
            {
                break;
            }

            var lightCaloriesCap = Math.Max(caloriesDeficit * 1.15, slot.TargetCalories * 0.35);
            var lightweightCandidates = remaining
                .Where(x => x.Calories <= lightCaloriesCap)
                .ToList();

            var pool = lightweightCandidates.Count > 0 ? lightweightCandidates : remaining;

            var bestTopUp = pool
                .Select(candidate => new
                {
                    Candidate = candidate,
                    Score = Math.Abs((currentCalories + candidate.Calories) - slot.TargetCalories),
                    MacroDeviation =
                        Math.Abs((currentProteins + candidate.Proteins) - slot.TargetProteins) +
                        Math.Abs((currentFats + candidate.Fats) - slot.TargetFats) +
                        Math.Abs((currentCarbs + candidate.Carbs) - slot.TargetCarbs),
                })
                .OrderBy(x => x.Score)
                .ThenBy(x => x.MacroDeviation)
                .ThenBy(x => x.Candidate.Calories)
                .ThenBy(x => x.Candidate.RecipeId)
                .First()
                .Candidate;

            selected.Add(bestTopUp);
            remaining.RemoveAll(x => x.RecipeId == bestTopUp.RecipeId);
        }

        return selected;
    }
}
