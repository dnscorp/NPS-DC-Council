CREATE PROCEDURE Proc_BUDGET_Create  
(  
    @Name [nvarchar] (250),  
    @Amount [float],  
    @FiscalYearId [bigint],  
    @OfficeId [bigint],  
    @IsDefault [bit],  
    @IsDeleted [bit] ,
     @IsDeduct [bit] 
     
)  
AS  
BEGIN  
  
    INSERT INTO [dbo].[Budgets] ([Name], [IsDefault], [Amount], [OfficeID], [FiscalYearID], [IsDeleted], [CreatedDate], [UpdatedDate],IsDeduct)  
    VALUES(@Name,@IsDefault,@Amount,@OfficeId,@FiscalYearId,@IsDeleted,GETUTCDATE(),NULL,@IsDeduct)  
      
END
