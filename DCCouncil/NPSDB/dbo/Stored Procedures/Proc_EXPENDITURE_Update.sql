
CREATE PROCEDURE Proc_EXPENDITURE_Update    
(    
    @ExpenditureId [bigint],    
    @ExpenditureCategoryId [bigint],    
    @VendorName nvarchar(500),    
    @Description nvarchar(MAX),    
    @OBJCode nvarchar(500),    
    @DateOfTransaction [datetime],    
    @Amount [float],  
    @OfficeId [BIGINT]  ,  
    @Comments nvarchar(MAX),    
    @FiscalYearId [bigint],    
    @BudgetId [bigint],    
    @IsDeleted [bit],    
    @StaffLevelExpenditures XML    
)    
AS    
BEGIN    
    
    UPDATE [dbo].[Expenditures]    
    SET    
        [ExpenditureCategoryID] = @ExpenditureCategoryId,    
        [VendorName] = @VendorName,    
        [Description] = @Description,    
        [OBJCode] = @OBJCode,    
        [DateOfTransaction] = @DateOfTransaction,    
        [Amount] = @Amount,   
        OfficeId=@OfficeId,   
        [Comments] = @Comments,    
        [FiscalYearID] = @FiscalYearId,    
        [BudgetID] = @BudgetId,    
        [IsDeleted] = @IsDeleted,            
        [UpdatedDate] = GETDATE()            
    WHERE [ExpenditureID] = @ExpenditureId        
        
        
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
		
		IF @StaffLevelExpenditures IS NULL
		BEGIN
			DELETE FROM StaffLevelExpenditures WHERE ExpenditureID = @ExpenditureId
		END  
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
        UPDATE [dbo].[StaffLevelExpenditures]    
        SET [dbo].[StaffLevelExpenditures].[Amount] = TMP.amount,[UpdatedDate]=GETDATE()    
        FROM #TmpStaffLevelExpenditures TMP    
        WHERE [dbo].[StaffLevelExpenditures].[StaffID] = TMP.staffid    
        AND [ExpenditureID] = @ExpenditureId                    
    END       
END
