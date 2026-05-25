CREATE PROCEDURE dbo.usp_RecipeProduct_AddByName
    @ProductName NVARCHAR(100),
    @RecipeId     INT,
    @Grams        FLOAT = 100
AS
BEGIN
    DECLARE @ProductId INT;
    DECLARE @Name NVARCHAR(100) = NULLIF(LTRIM(RTRIM(@ProductName)), N'');

    IF @Name IS NULL
    BEGIN
        PRINT(N'Назва продукту не може бути порожньою.');
        RETURN;
    END;

    IF @Grams IS NULL OR @Grams <= 0
    BEGIN
        PRINT(N'Кількість у грамах має бути більша за нуль.');
        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.[Recipe] AS r
        WHERE r.RecipeId = @RecipeId
    )
    BEGIN
        PRINT(N'Рецепт з вказаним ідентифікатором відсутній.');
        RETURN;
    END;

    SELECT TOP (1)
        @ProductId = p.ProductId
    FROM dbo.[Product] AS p
    WHERE p.ProductName = @Name
    ORDER BY p.ProductName ASC;

    IF @ProductId IS NULL
    BEGIN
        PRINT(N'Продукт з такою назвою відсутній у довіднику.');
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
