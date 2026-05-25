SET NOCOUNT ON;

EXEC dbo.usp_RecipeProduct_Add
    @RecipeId   = 1,
    @ProductId  = 41,
    @Grams      = 150;

EXEC dbo.usp_RecipeProduct_Add
    @RecipeId   = 1,
    @ProductId  = 19,
    @Grams      = 25;

EXEC dbo.usp_RecipeProduct_Add
    @RecipeId   = 4,
    @ProductId  = 73,
    @Grams      = 40;

SELECT rp.RecipeId, rp.ProductId, p.ProductName, rp.Grams
FROM dbo.[RecipeProduct] AS rp
INNER JOIN dbo.[Product] AS p ON p.ProductId = rp.ProductId
WHERE rp.RecipeId = 1 AND rp.ProductId IN (19, 41)
ORDER BY rp.ProductId;

SELECT rp.RecipeId, rp.ProductId, p.ProductName, rp.Grams
FROM dbo.[RecipeProduct] AS rp
INNER JOIN dbo.[Product] AS p ON p.ProductId = rp.ProductId
WHERE rp.RecipeId = 4 AND rp.ProductId = 73;
