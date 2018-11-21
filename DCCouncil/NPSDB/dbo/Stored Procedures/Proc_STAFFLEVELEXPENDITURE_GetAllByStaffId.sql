CREATE PROCEDURE Proc_STAFFLEVELEXPENDITURE_GetAllByStaffId    
(    
    @StaffId [bigint]    
)    
AS    
BEGIN    
    SELECT    
        [SLE].[Amount],    
        [SLE].[CreatedDate],    
        [SLE].[ExpenditureID],    
        [E].[Amount] AS ExpenditureAmount,    
        [E].[BudgetID],    
        [B].[Amount] AS BudgetAmount,    
        [B].[CreatedDate] AS BudgetCreatedDate,    
        [B].[FiscalYearID],    
        [FY].[CreatedDate] AS FiscalYearCreatedDate,    
        [FY].[Name] AS FiscalYearName,    
        [FY].[StartDate] AS FiscalYearStartDate,    
        [FY].[EndDate] AS FiscalYearEndDate,    
        [FY].[UpdatedDate] AS FiscalYearUpdatedDate,    
        [FY].[Year] AS FiscalYearYear,    
        [B].[IsDefault] AS BudgetIsDefault,    
        [B].[IsDeleted] AS BudgetIsDeleted,    
        [B].[Name] AS BudgetName,    
        [B].[OfficeID],    
        [B].IsDeduct AS BudgetIsDeduct,  
        O.[ActiveFrom] AS OfficeActiveFrom,    
        [O].[ActiveTo] AS OfficeActiveTo,    
        [O].[CreatedDate] AS OfficeCreatedDate,    
        [O].[IndexCode] AS OfficeIndexCode,    
        [O].[IndexTitle] AS OfficeIndexTitle,    
        [O].[IsDeleted] AS OfficeIsDeleted,    
        [O].[Name] AS OfficeName,    
        [O].[PCA] AS OfficePCA,    
        [O].[PCATitle] AS OfficePCATitle,    
        [O].[UpdatedDate] AS OfficeUpdatedDate,
        [O].CompCode AS OfficeCompCode,    
        [B].[UpdatedDate] AS BudgetUpdatedDate,    
        E.[Comments] AS ExpenditureComments,    
        [E].[CreatedDate] AS ExpenditureCreatedDate,    
        [E].[DateOfTransaction] AS ExpenditureDateOfTransaction,    
        [E].[Description] AS ExpenditureDescription,    
        [E].[ExpenditureCategoryID],    
        [EC].[Code] AS ExpenditureCategoryCode,    
        [EC].[CreatedDate] AS ExpenditureCategoryCreatedDate,    
        [EC].[IsActive] AS ExpenditureCategoryIsActive,    
        [EC].[IsDeleted] AS ExpenditureCategoryIsDeleted,    
        [EC].[IsFixed] AS ExpenditureCategoryIsFixed,                                  
        [EC].[IsStaffLevel] AS ExpenditureCategoryIsStaffLevel,    
        [EC].[Name] AS ExpenditureCategoryName,    
        [EC].[UpdatedDate] AS ExpenditureCategoryUpdatedDate,    
        [EC].[IsMonthly] AS ExpenditureCategoryIsMonthly,    
        [EC].[IsVendorStaff] AS ExpenditureCategoryIsVendorStaff,    
        [EC].[IsSystemDefined] AS ExpenditureCategoryIsSystemDefined,    
        [EC].[AppendMonth] AS ExpenditureCategoryAppendMonth,    
        [EC].[IsVendorStaffAndOther] AS ExpenditureCategoryIsVendorStaffAndOther,    
        [E].[IsDeleted] AS ExpenditureIsDeleted,    
        [E].[OBJCode] AS ExpenditureOBJCode,    
        [E].[UpdatedDate] AS ExpenditureUpdatedDate,    
        [E].[VendorName] AS ExpenditureVendorName,    
        [SLE].[StaffID],    
        [S].[ActiveFrom] AS StaffActiveFrom,    
        [S].[ActiveTo] AS StaffActiveTo,    
        [S].[HasStaffLevelExpenditures] AS HasStaffLevelExpenditures,    
        [S].[CreatedDate] AS StaffCreatedDate,    
        [S].[FirstName] AS StaffFirstName,    
        [S].[IsDeleted] AS StaffIsDeleted,    
        [S].[LastName] AS StaffLastName,    
        [S].[UpdatedDate] AS StaffUpdatedDate, 
        [S].WirelessNumber as StaffWirelessNumber ,  
        [SLE].[StaffLevelExpenditureID],    
        [SLE].[UpdatedDate]            
    FROM  [dbo].[StaffLevelExpenditures] [SLE]    
    INNER JOIN [dbo].[Expenditures] E ON [SLE].[ExpenditureID] = [E].[ExpenditureID]    
    INNER JOIN [dbo].[Budgets] B ON [E].[BudgetID] = [B].[BudgetID]    
    INNER JOIN [dbo].[FiscalYears] FY ON [B].[FiscalYearID] = [FY].[FiscalYearID]    
    INNER JOIN [dbo].[Offices] O ON [B].[OfficeID] = [O].[OfficeID]    
    INNER JOIN [dbo].[ExpenditureCategories] EC ON [E].[ExpenditureCategoryID] = [EC].[ExpenditureCategoryID]    
    INNER JOIN [dbo].[Staffs] S ON S.[StaffID] = [SLE].[StaffID]    
    WHERE [SLE].[StaffId] = @StaffId    
END
