CREATE PROCEDURE [dbo].[Proc_USER_Create]  
(  
@Username [nvarchar] (50),  
@PasswordHash [nvarchar] (MAX),  
@IsActive [bit],  
@IsDeleted [bit],  
@FirstName [nvarchar](250),  
@LastName [nvarchar](250)  
)  
  
AS  
BEGIN  
	
    INSERT INTO [dbo].[Users] ([UserGuid], [Username], [PasswordHash], [LastFiscalYearSelectedID],  [IsActive], [IsDeleted], [CreatedDate], [UpdatedDate]) 
    SELECT TOP 1 NEWID(), @Username, @PasswordHash ,FiscalYearID , @IsActive,@IsDeleted,GETDATE(), GETDATE() FROM [dbo].[FiscalYears]
      
    DECLARE @UserId [int]  
    SET @UserId= @@IDENTITY  
   
      
    INSERT INTO [dbo].[UserProfiles] ([UserID], [FirstName], [LastName], [CreatedDate], [UpdatedDate]) 
    VALUES (@UserId,@FirstName,@LastName,GETDATE(), GETDATE())  
  
END  
  
  
  