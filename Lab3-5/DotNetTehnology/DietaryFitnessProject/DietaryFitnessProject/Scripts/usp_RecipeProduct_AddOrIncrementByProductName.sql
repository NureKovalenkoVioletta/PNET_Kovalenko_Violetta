CREATE PROCEDURE dbo.usp_RecipeProduct_AddOrIncrementByProductName
    @RecipeId    INT,
    @ProductName NVARCHAR(100),
    @Grams       FLOAT
AS
BEGIN
    IF @Grams IS NULL OR @Grams < 0
    BEGIN
        PRINT(N'Кількість у грамах має бути невід''ємним числом.');
        RETURN;
    END;

    DECLARE @ProductId INT;

    SELECT TOP (1) @ProductId = p.ProductId
    FROM dbo.[Product] AS p
    WHERE p.ProductName = @ProductName
    ORDER BY p.ProductId;

    IF @ProductId IS NULL
    BEGIN
        PRINT(N'Продукт з вказаною назвою відсутній у довіднику.');
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM dbo.[RecipeProduct] AS rp
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
