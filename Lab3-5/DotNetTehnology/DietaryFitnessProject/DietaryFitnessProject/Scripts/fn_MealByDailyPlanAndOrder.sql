CREATE FUNCTION dbo.fn_MealByDailyPlanAndOrder
(
    @DailyDietPlanId INT,
    @MealOrder INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        m.MealId,
        m.MealOrder,
        m.MealTime,
        m.MealCalories,
        m.MealProteins,
        m.MealFats,
        m.MealCarbs
    FROM dbo.Meal AS m
    WHERE m.DailyDietPlanId = @DailyDietPlanId
      AND m.MealOrder = @MealOrder
);
GO