SET NOCOUNT ON;

SELECT
    d.[DailyDietPlanId],
    d.[UserId],
    d.[DailyPlanDay],
    d.[DailyPlanNumberOfMeals],
    (SELECT COUNT(*) FROM dbo.[Meal] AS m WHERE m.[DailyDietPlanId] = d.[DailyDietPlanId]) AS [MealRows]
FROM dbo.[DailyDietPlan] AS d
ORDER BY d.[DailyDietPlanId];

DECLARE @DailyDietPlanId INT;
DECLARE @PlanDate        DATE;

SELECT TOP (1)
    @DailyDietPlanId = d.[DailyDietPlanId],
    @PlanDate = d.[DailyPlanDay]
FROM dbo.[DailyDietPlan] AS d
WHERE EXISTS (
    SELECT 1
    FROM dbo.[Meal] AS m
    WHERE m.[DailyDietPlanId] = d.[DailyDietPlanId]
)
ORDER BY d.[DailyDietPlanId];

IF @DailyDietPlanId IS NULL
BEGIN
    PRINT N'У таблиці DailyDietPlan немає планів з рядками в Meal — створіть план у застосунку, потім повторіть скрипт.';
END
ELSE
BEGIN
    PRINT N'Виклик функції: DailyDietPlanId = ' + CAST(@DailyDietPlanId AS NVARCHAR(20))
        + N', DailyPlanDay = ' + CONVERT(NVARCHAR(10), @PlanDate, 23) + N'.';

    SELECT *
    FROM dbo.fn_LowestCalorieMealsByPlanAndDate(@DailyDietPlanId, @PlanDate);

    SELECT *
    FROM dbo.fn_LowestCalorieMealsByPlanAndDate(@DailyDietPlanId, DATEADD(DAY, 400, @PlanDate));
END;
