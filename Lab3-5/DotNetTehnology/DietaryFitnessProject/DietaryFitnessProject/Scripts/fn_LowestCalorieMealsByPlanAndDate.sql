CREATE FUNCTION dbo.fn_LowestCalorieMealsByPlanAndDate (
    @DailyDietPlanId INT,
    @PlanDate         DATE
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        m.[MealId],
        m.[MealOrder],
        (CASE m.[MealType]
            WHEN 0 THEN N'Сніданок'
            WHEN 1 THEN N'Перекус (ранковий)'
            WHEN 2 THEN N'Обід'
            WHEN 3 THEN N'Перекус (денний)'
            WHEN 4 THEN N'Вечеря'
            WHEN 5 THEN N'Перекус (пізній)'
            ELSE N'Невідомий тип'
        END) AS [MealName],
        m.[MealCalories],
        CAST(NULL AS NVARCHAR(500)) AS [Message]
    FROM dbo.[Meal] AS m
    INNER JOIN dbo.[DailyDietPlan] AS d
        ON d.[DailyDietPlanId] = m.[DailyDietPlanId]
    WHERE d.[DailyDietPlanId] = @DailyDietPlanId
      AND d.[DailyPlanDay] = @PlanDate
      AND m.[MealCalories] = (
            SELECT MIN(m2.[MealCalories])
            FROM dbo.[Meal] AS m2
            WHERE m2.[DailyDietPlanId] = @DailyDietPlanId
        )

    UNION ALL

    SELECT
        CAST(NULL AS INT),
        CAST(NULL AS INT),
        CAST(NULL AS NVARCHAR(100)),
        CAST(NULL AS FLOAT),
        N'Плани харчування не були сформовані у «'
            + CONVERT(NVARCHAR(10), @PlanDate, 104)
            + N'».'
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.[DailyDietPlan] AS d2
        WHERE d2.[DailyDietPlanId] = @DailyDietPlanId
          AND d2.[DailyPlanDay] = @PlanDate
    );
);
GO
