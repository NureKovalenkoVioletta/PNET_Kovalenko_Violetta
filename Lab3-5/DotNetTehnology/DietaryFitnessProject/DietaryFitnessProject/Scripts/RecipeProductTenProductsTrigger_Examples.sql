SET NOCOUNT ON;

DECLARE @RecipeId INT = 1;
DECLARE @NewProductId INT = 41;

IF EXISTS (
    SELECT 1
    FROM dbo.[RecipeProduct] AS rp
    WHERE rp.[RecipeId] = @RecipeId
      AND rp.[ProductId] = @NewProductId
)
BEGIN
    SELECT TOP (1)
        @NewProductId = p.[ProductId]
    FROM dbo.[Product] AS p
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.[RecipeProduct] AS rp
        WHERE rp.[RecipeId] = @RecipeId
          AND rp.[ProductId] = p.[ProductId]
    )
    ORDER BY p.[ProductId];
END;

IF @NewProductId IS NULL
BEGIN
    PRINT N'У рецепта ' + CAST(@RecipeId AS NVARCHAR(10)) + N' уже перелічені всі продукти з довідника — приклад не застосовний.';
    RETURN;
END;

PRINT N'Додаю до рецепта ' + CAST(@RecipeId AS NVARCHAR(10)) + N' продукт ProductId = ' + CAST(@NewProductId AS NVARCHAR(10));

SELECT
    r.[RecipeId],
    r.[RecipeName],
    (SELECT COUNT(DISTINCT rp.[ProductId]) FROM dbo.[RecipeProduct] AS rp WHERE rp.[RecipeId] = r.[RecipeId]) AS [DistinctProductsBefore],
    CASE
        WHEN CHARINDEX(N'Рецепт містить 10+ продуктів', ISNULL(r.[RecipeInstructions], N'')) > 0 THEN N'так'
        ELSE N'ні'
    END AS [MarkerAlreadyPresent],
    LEFT(r.[RecipeInstructions], 120) AS [InstructionsStart]
FROM dbo.[Recipe] AS r
WHERE r.[RecipeId] = @RecipeId;

INSERT INTO dbo.[RecipeProduct] ([RecipeId], [ProductId], [Grams])
VALUES (@RecipeId, @NewProductId, 100);

SELECT
    r.[RecipeId],
    r.[RecipeName],
    (SELECT COUNT(DISTINCT rp.[ProductId]) FROM dbo.[RecipeProduct] AS rp WHERE rp.[RecipeId] = r.[RecipeId]) AS [DistinctProductsAfter],
    CASE
        WHEN CHARINDEX(N'Рецепт містить 10+ продуктів', ISNULL(r.[RecipeInstructions], N'')) > 0 THEN N'так'
        ELSE N'ні'
    END AS [MarkerPresent],
    r.[RecipeInstructions]
FROM dbo.[Recipe] AS r
WHERE r.[RecipeId] = @RecipeId;
