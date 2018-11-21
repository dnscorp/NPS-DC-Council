CREATE PROCEDURE [dbo].[Proc_USER_Update]
(
    @UserId [bigint],
    @Username NVARCHAR (MAX),
    @PasswordHash NVARCHAR (MAX),
    @IsActive BIT,
    @IsDeleted BIT,
    @LastFiscalYearSelectedId [bigint],
    @FirstName NVARCHAR (250),
    @LastName NVARCHAR (250)
)
AS
BEGIN    
    UPDATE [dbo].[Users]
    SET    [Username]     = @Username, 
		   [PasswordHash] = @PasswordHash,	              
           [IsActive]     = @IsActive,
           [IsDeleted]    = @IsDeleted,
           [LastFiscalYearSelectedID] = @LastFiscalYearSelectedId,
           [UpdatedDate]  = GETUTCDATE()
    WHERE [UserID] = @UserId;
           
    UPDATE [dbo].[UserProfiles]
    SET    [FirstName]   = @FirstName,
           [LastName]    = @LastName,
           [UpdatedDate] = GETUTCDATE()   
    WHERE [UserID] = @UserId
    
END




