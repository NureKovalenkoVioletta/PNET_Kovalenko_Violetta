using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.BLL.Models;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Enums;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class MealService : IMealService
{
    private readonly IMealRepository _mealRepository;
    private readonly IDailyDietPlanRepository _dailyDietPlanRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMealRecipeRepository _mealRecipeRepository;
    private readonly IMealRecipeService _mealRecipeService;
    private readonly IRecipeService _recipeService;

    public MealService(
        IMealRepository mealRepository,
        IDailyDietPlanRepository dailyDietPlanRepository,
        IRecipeRepository recipeRepository,
        IMealRecipeRepository mealRecipeRepository,
        IMealRecipeService mealRecipeService,
        IRecipeService recipeService)
    {
        _mealRepository = mealRepository;
        _dailyDietPlanRepository = dailyDietPlanRepository;
        _recipeRepository = recipeRepository;
        _mealRecipeRepository = mealRecipeRepository;
        _mealRecipeService = mealRecipeService;
        _recipeService = recipeService;
    }

    public Task<IReadOnlyCollection<Meal>> GetAllAsync(CancellationToken cancellationToken = default)
        => _mealRepository.GetAllAsync(cancellationToken);

    public Task<Meal?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _mealRepository.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyCollection<Meal>> GetByPlanIdAsync(int dailyDietPlanId, CancellationToken cancellationToken = default)
        => _mealRepository.GetByPlanIdAsync(dailyDietPlanId, cancellationToken);

    public Task AddAsync(Meal meal, CancellationToken cancellationToken = default)
        => _mealRepository.AddAsync(meal, cancellationToken);

    public Task UpdateAsync(Meal meal, CancellationToken cancellationToken = default)
        => _mealRepository.UpdateAsync(meal, cancellationToken);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        => _mealRepository.DeleteAsync(id, cancellationToken);

    public async Task<IReadOnlyCollection<Meal>> GenerateForPlanAsync(
        int dailyDietPlanId,
        CancellationToken cancellationToken = default)
    {
        var plan = await _dailyDietPlanRepository.GetByIdAsync(dailyDietPlanId, cancellationToken);
        if (plan is null)
        {
            throw new InvalidOperationException($"Денний план з ідентифікатором {dailyDietPlanId} не знайдено.");
        }

        if (plan.DailyPlanNumberOfMeals is < 1 or > 5)
        {
            throw new InvalidOperationException("Кількість прийомів їжі в плані має бути від 1 до 5.");
        }

        var existing = await _mealRepository.GetByPlanIdAsync(dailyDietPlanId, cancellationToken);
        if (existing.Count > 0)
        {
            throw new InvalidOperationException(
                $"Прийоми їжі для плану на {plan.DailyPlanDay:dd.MM.yyyy} вже згенеровано. Повторна генерація неможлива. " +
                "Щоб згенерувати знову, спочатку видаліть згенеровані прийоми їжі.");
        }

        var recipes = await _recipeRepository.GetAllAsync(cancellationToken);
        if (recipes.Count == 0)
        {
            throw new InvalidOperationException("Немає рецептів для генерації прийомів їжі.");
        }

        var dailyTargets = new DailyDietTargets(
            DailyCalories: plan.DailyPlanCalories,
            ProteinsGrams: plan.DailyPlanProteins,
            FatsGrams: plan.DailyPlanFats,
            CarbsGrams: plan.DailyPlanCarbs);

        var slots = MealGenerationPlanBuilder.BuildSlots(plan.DailyPlanNumberOfMeals, dailyTargets);
        var drafts = slots
            .Select(slot => MealRecipeCandidateSelector.BuildDraftForSlot(
                slot,
                recipes,
                selectionSeed: BuildSelectionSeed(plan, slot.MealOrder)))
            .ToList();

        foreach (var draft in drafts)
        {
            var meal = new Meal
            {
                DailyDietPlanId = dailyDietPlanId,
                MealOrder = draft.MealOrder,
                MealType = draft.MealType,
                MealTime = draft.MealTime,
                MealCalories = draft.MealCalories,
                MealProteins = draft.MealProteins,
                MealFats = draft.MealFats,
                MealCarbs = draft.MealCarbs,
            };

            await _mealRepository.AddAsync(meal, cancellationToken);
            await _mealRecipeRepository.AddAsync(
                new MealRecipe
                {
                    MealId = meal.MealId,
                    RecipeId = draft.RecipeIds[0],
                },
                cancellationToken);

            foreach (var recipeId in draft.RecipeIds.Skip(1))
            {
                await _mealRecipeRepository.AddAsync(
                    new MealRecipe
                    {
                        MealId = meal.MealId,
                        RecipeId = recipeId,
                    },
                    cancellationToken);
            }
        }

        return await _mealRepository.GetByPlanIdAsync(dailyDietPlanId, cancellationToken);
    }

    public async Task DeleteAllForPlanAsync(int dailyDietPlanId, CancellationToken cancellationToken = default)
    {
        var meals = await _mealRepository.GetByPlanIdAsync(dailyDietPlanId, cancellationToken);
        foreach (var meal in meals)
        {
            await _mealRepository.DeleteAsync(meal.MealId, cancellationToken);
        }
    }

    public async Task<IReadOnlyList<MealDisplayItem>> GetDisplayItemsForPlanAsync(
        int dailyDietPlanId,
        CancellationToken cancellationToken = default)
    {
        var meals = await _mealRepository.GetByPlanIdAsync(dailyDietPlanId, cancellationToken);
        var rows = new List<MealDisplayItem>();

        foreach (var meal in meals.OrderBy(x => x.MealOrder))
        {
            var links = await _mealRecipeService.GetByMealIdAsync(meal.MealId, cancellationToken);
            var recipes = new List<MealRecipeLineItem>();
            var portions = 0;
            var grams = 0.0;

            foreach (var link in links.OrderBy(x => x.RecipeId))
            {
                if (link.RecipeId <= 0)
                {
                    continue;
                }

                var recipe = await _recipeService.GetByIdAsync(link.RecipeId, cancellationToken);
                if (recipe is null)
                {
                    continue;
                }

                recipes.Add(new MealRecipeLineItem(recipe.RecipeId, recipe.RecipeName));
                portions += 1;

                if (recipe.PortionCount > 0)
                {
                    grams += recipe.RecipeTotalGrams / recipe.PortionCount;
                }
            }

            if (recipes.Count == 0)
            {
                recipes.Add(new MealRecipeLineItem(0, "—"));
            }

            rows.Add(new MealDisplayItem(
                MealOrder: meal.MealOrder,
                MealTypeLabel: MealTypeLabel(meal.MealType),
                MealTime: meal.MealTime,
                Calories: meal.MealCalories,
                Proteins: meal.MealProteins,
                Fats: meal.MealFats,
                Carbs: meal.MealCarbs,
                Recipes: recipes,
                Portions: portions,
                Grams: grams));
        }

        return rows;
    }

    private static int BuildSelectionSeed(DailyDietPlan plan, int mealOrder)
    {
        unchecked
        {
            var seed = 17;
            seed = seed * 31 + plan.DailyDietPlanId;
            seed = seed * 31 + plan.UserId;
            seed = seed * 31 + plan.DailyPlanDay.DayNumber;
            seed = seed * 31 + mealOrder;
            return seed;
        }
    }

    private static string MealTypeLabel(MealType type) => type switch
    {
        MealType.Breakfast => "Сніданок",
        MealType.MorningSnack => "Перекус (ранковий)",
        MealType.Lunch => "Обід",
        MealType.AfternoonSnack => "Перекус (денний)",
        MealType.Dinner => "Вечеря",
        MealType.LateSnack => "Пізній перекус",
        _ => type.ToString(),
    };
}
