
CREATE TABLE dbo.DailyDietPlanLogs
(
    Id               INT            NOT NULL IDENTITY(1, 1),
    DailyDietPlanId  INT            NOT NULL,
    ModifyDate       DATETIME       NOT NULL,
    CONSTRAINT PK_DailyDietPlanLogs PRIMARY KEY (Id)
);
GO

CREATE TRIGGER dbo.tr_DailyDietPlanLogging
ON dbo.DailyDietPlan
AFTER UPDATE
AS
BEGIN
    INSERT INTO dbo.DailyDietPlanLogs (DailyDietPlanId, ModifyDate)
    SELECT
        i.DailyDietPlanId,
        GETDATE()
    FROM inserted AS i;
END;
GO
