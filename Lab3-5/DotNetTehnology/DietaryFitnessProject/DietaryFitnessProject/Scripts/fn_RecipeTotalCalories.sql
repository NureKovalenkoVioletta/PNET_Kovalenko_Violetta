IF EXISTS (
    SELECT *
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'[dbo].[fn_RecipeTotalCalories]')
      AND type IN (N'IF', N'TF', N'FS', N'FT')
)
    DROP FUNCTION [dbo].[fn_RecipeTotalCalories];
GO

CREATE FUNCTION dbo.fn_RecipeTotalCalories()
RETURNS TABLE
AS
RETURN
(
    SELECT
        r.[RecipeId],
        r.[RecipeName],
            SUM(p.[ProductCaloriesPer100g] * rp.[Grams] / 100.0) AS [TotalCalories]
    FROM dbo.[Recipe] AS r
    INNER JOIN dbo.[RecipeProduct] AS rp ON rp.[RecipeId] = r.[RecipeId]
    INNER JOIN dbo.[Product] AS p ON p.[ProductId] = rp.[ProductId]
    GROUP BY r.[RecipeId], r.[RecipeName]
);
GO

SELECT *
FROM dbo.fn_RecipeTotalCalories();
GO
