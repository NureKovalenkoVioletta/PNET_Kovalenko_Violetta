CREATE TRIGGER dbo.tr_RecipeProduct_AfterInsert_TenProductsNote
ON dbo.RecipeProduct
AFTER INSERT
AS
BEGIN
    DECLARE @Marker NVARCHAR(100) = N'Рецепт містить 10+ продуктів';

    UPDATE r
    SET r.RecipeInstructions =
        CONCAT(
            ISNULL(r.RecipeInstructions, N''),
            CASE 
                WHEN LEN(ISNULL(r.RecipeInstructions, N'')) > 0 THEN N' '
                ELSE N''
            END,
            @Marker
        )
    FROM dbo.Recipe r
    WHERE r.RecipeId IN (
        SELECT i.RecipeId FROM inserted i
    )
    AND (
        SELECT COUNT(DISTINCT rp.ProductId)
        FROM dbo.RecipeProduct rp
        WHERE rp.RecipeId = r.RecipeId
    ) >= 10
    AND CHARINDEX(@Marker, ISNULL(r.RecipeInstructions, N'')) = 0;
END;