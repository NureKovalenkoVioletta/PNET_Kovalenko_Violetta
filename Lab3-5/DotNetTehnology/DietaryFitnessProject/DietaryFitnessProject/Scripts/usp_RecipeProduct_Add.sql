IF EXISTS (
    SELECT * 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[usp_RecipeProduct_Add]') 
      AND type IN (N'P', N'PC')
)
DROP PROCEDURE [dbo].[usp_RecipeProduct_Add]
GO

CREATE PROCEDURE dbo.usp_RecipeProduct_Add
    @RecipeId   INT,
    @ProductId  INT,
    @Grams      FLOAT
AS
BEGIN
    IF @Grams IS NULL OR @Grams < 0
    BEGIN
        PRINT(N'Кількість у грамах має бути невід''ємним числом.');
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.[Recipe] r
        WHERE r.RecipeId = @RecipeId
    )
    BEGIN
        PRINT(N'Рецепт з вказаним ідентифікатором відсутній.');
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.[Product] p
        WHERE p.ProductId = @ProductId
    )
    BEGIN
        PRINT(N'Продукт з вказаним ідентифікатором відсутній у довіднику.');
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.[RecipeProduct] rp
        WHERE rp.RecipeId = @RecipeId
          AND rp.ProductId = @ProductId
    )
    BEGIN
        UPDATE dbo.[RecipeProduct]
        SET Grams = Grams + @Grams
        WHERE RecipeId = @RecipeId
          AND ProductId = @ProductId;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.[RecipeProduct] (RecipeId, ProductId, Grams)
        VALUES (@RecipeId, @ProductId, @Grams);
    END;
END;
GO