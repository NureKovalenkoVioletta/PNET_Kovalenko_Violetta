CREATE PROCEDURE usp_User_Insert
    @FirstName       NVARCHAR(100),
    @LastName        NVARCHAR(100),
    @Email           NVARCHAR(100),
    @Gender          INT,            
    @Age             INT,
    @Height          FLOAT,     
    @Weight          FLOAT,    
    @ActivityLevel   INT,         
    @Goal            INT,        
    @PasswordHash    NVARCHAR(512),
    @PasswordSalt    NVARCHAR(256),
    @CreatedAtUtc    DATETIME2(7) = NULL,  
AS
BEGIN

    IF @CreatedAtUtc IS NULL
        SET @CreatedAtUtc = SYSUTCDATETIME();

    IF EXISTS (SELECT 1 FROM dbo.[User] WHERE Email = @Email)
    BEGIN
        PRINT(N'Користувач із такою електронною поштою вже існує.');
        RETURN;
    END;

    INSERT INTO dbo.[User] (
        FirstName,
        LastName,
        Email,
        Gender,
        Age,
        Height,
        Weight,
        ActivityLevel,
        Goal,
        PasswordHash,
        PasswordSalt,
        CreatedAtUtc,
        LastLoginAtUtc
    )
    VALUES (
        @FirstName,
        @LastName,
        @Email,
        @Gender,
        @Age,
        @Height,
        @Weight,
        @ActivityLevel,
        @Goal,
        @PasswordHash,
        @PasswordSalt,
        @CreatedAtUtc,
        NULL
    );
END;
GO
