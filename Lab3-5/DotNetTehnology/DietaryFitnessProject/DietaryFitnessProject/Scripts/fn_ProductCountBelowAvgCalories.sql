IF EXISTS (
    SELECT *
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'[dbo].[fn_ProductCountBelowAvgCalories]')
      AND type IN (N'FN', N'FS', N'FT')
)
    DROP FUNCTION [dbo].[fn_ProductCountBelowAvgCalories];
GO

CREATE FUNCTION dbo.fn_ProductCountBelowAvgCalories()
RETURNS INT
AS
BEGIN
    DECLARE @ROW_COUNT INT;

    SET @ROW_COUNT = (
        SELECT COUNT(*)
        FROM dbo.[Product] AS p
        WHERE p.[ProductCaloriesPer100g] < (
                SELECT AVG(p2.[ProductCaloriesPer100g])
                FROM dbo.[Product] AS p2
            )
    );

    RETURN @ROW_COUNT;
END;
GO

SELECT dbo.fn_ProductCountBelowAvgCalories();
GO
