SET NOCOUNT ON;

DECLARE @RecipeId   INT = 1;
DECLARE @ProductId  INT = 41;

DELETE FROM dbo.[RecipeProduct]
WHERE [RecipeId] = @RecipeId
  AND [ProductId] = @ProductId;

PRINT N'Видалено рядок RecipeProduct для RecipeId=' + CAST(@RecipeId AS NVARCHAR(10))
    + N', ProductId=' + CAST(@ProductId AS NVARCHAR(10))
    + N'. Рядків видалено: ' + CAST(@@ROWCOUNT AS NVARCHAR(10));

UPDATE dbo.[Recipe]
SET [RecipeInstructions] =
    TRIM(
        REPLACE(
            REPLACE(
                ISNULL([RecipeInstructions], N''),
                N' Рецепт містить 10+ продуктів',
                N''
            ),
            N'Рецепт містить 10+ продуктів',
            N''
        )
    )
WHERE [RecipeId] = @RecipeId
  AND CHARINDEX(N'Рецепт містить 10+ продуктів', ISNULL([RecipeInstructions], N'')) > 0;

IF @@ROWCOUNT > 0
    PRINT N'З RecipeInstructions прибрано маркер «Рецепт містить 10+ продуктів» для RecipeId=' + CAST(@RecipeId AS NVARCHAR(10)) + N'.';
