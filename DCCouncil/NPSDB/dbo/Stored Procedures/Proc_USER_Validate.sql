CREATE PROCEDURE [dbo].[Proc_USER_Validate]
(
    @Username nvarchar(50),
    @PasswordHash nvarchar(MAX)
)
AS
BEGIN
    DECLARE @userID [bigint]
    
    SELECT @userID=UserID FROM [dbo].[Users]
    WHERE [Username]=@Username AND [PasswordHash]=@PasswordHash AND [IsDeleted]=0
    
    IF (@userID IS NOT NULL)
    BEGIN
        EXEC Proc_USER_GetByUserName @Username
    END   
    
END
