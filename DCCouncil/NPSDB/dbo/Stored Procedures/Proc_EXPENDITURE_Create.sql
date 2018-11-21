
CREATE PROCEDURE Proc_EXPENDITURE_Create 
(  
    @ExpenditureCategoryId [bigint],  
    @VendorName nvarchar(500),  
    @Description nvarchar(MAX),  
    @OBJCode nvarchar(500),  
    @DateOfTransaction [datetime],  
    @Amount [float],  
    @OfficeId [bigint],  
    @Comments nvarchar(MAX),  
    @FiscalYearId [bigint],  
    @BudgetId [bigint],  
    @IsDeleted [bit],  
    @StaffLevelExpenditures [xml]      
)  
AS  
BEGIN  
      
    INSERT INTO [dbo].[Expenditures] ([ExpenditureCategoryID], [VendorName], [Description], [OBJCode], [DateOfTransaction], [Amount], [OfficeID], [Comments], [FiscalYearID], [BudgetID], [IsDeleted], [CreatedDate], [UpdatedDate])  
    VALUES(@ExpenditureCategoryId,@VendorName,@Description,@OBJCode,@DateOfTransaction,@Amount,@OfficeId,@Comments,@FiscalYearId,@BudgetId,@IsDeleted,GETUTCDATE(),NULL);  
      
    DECLARE @ExpenditureId [bigint]  
    SET @ExpenditureId = @@IDENTITY  
      
    DECLARE @IsStaffLevel [bit]  
    SELECT @IsStaffLevel= [IsStaffLevel]  
    FROM [dbo].[ExpenditureCategories]  
    WHERE [ExpenditureCategoryID] = @ExpenditureCategoryId  
    
          
    DECLARE @IsVendorStaff [bit]  
    SELECT @IsVendorStaff= IsVendorStaff  
    FROM [dbo].[ExpenditureCategories]  
    WHERE [ExpenditureCategoryID] = @ExpenditureCategoryId 
      
    IF @IsStaffLevel = 1 OR @IsVendorStaff =1 
    BEGIN  
        Declare @XmlDataHandle int  
     Exec sp_xml_preparedocument @XmlDataHandle OUTPUT,@StaffLevelExpenditures          
          
        SELECT staffid,amount  
     INTO #TmpStaffLevelExpenditures  
     FROM OpenXml(@XmlDataHandle,'/stafflevelexpenditures/stafflevelexpenditure',2)  
     WITH  
     (  
         staffid [bigint],  
      amount [float]    
     )  
          
        INSERT INTO [dbo].[StaffLevelExpenditures] ([ExpenditureID], [StaffID], [Amount], [CreatedDate], [UpdatedDate])  
        SELECT @ExpenditureId,staffid,amount,GETDATE(),NULL  
        FROM #TmpStaffLevelExpenditures          
    END     
      
END
