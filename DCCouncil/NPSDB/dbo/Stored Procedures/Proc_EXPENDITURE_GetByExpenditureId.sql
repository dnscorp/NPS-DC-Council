
CREATE PROCEDURE Proc_EXPENDITURE_GetByExpenditureId    
(    
    @ExpenditureId bigint    
)    
AS    
BEGIN    
    SELECT               
      [E].[Amount],    
            [E].[BudgetID],    
            [E].[OfficeID],    
            [E].[VendorName],    
            [O].[ActiveFrom] AS OfficeActiveFrom,    
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
            [B].[Amount] AS BudgetAmount,    
            [B].[CreatedDate] AS BudgetCreatedDate,    
            [B].[IsDefault] AS BudgetIsDefault,    
            [B].[IsDeleted] AS BudgetIsDeleted,    
            [B].[Name] AS BudgetName,    
            [B].[UpdatedDate] AS BudgetUpdatedDate,   
            [B].IsDeduct AS BudgetIsDeduct,   
            [E].[FiscalYearID],    
            [FY].[CreatedDate] AS FiscalYearCreatedDate,    
            [FY].[Name] AS FiscalYearName,    
            [FY].[StartDate] AS FiscalYearStartDate,    
            [FY].[EndDate] AS FiscalYearEndDate,    
            [FY].[UpdatedDate] AS FiscalYearUpdatedDate,    
            [FY].[Year] AS FiscalYearYear,                    
            [E].[Comments],    
            [E].[CreatedDate],    
            [E].[DateOfTransaction],    
            [E].[Description],    
            [E].[ExpenditureCategoryID],    
            [EC].[Code] AS ExpenditureCategoryCode,    
            [EC].[CreatedDate] AS ExpenditureCategoryCreatedDate,    
            [EC].[IsActive] AS ExpenditureCategoryIsActive,    
            [EC].[IsDeleted] AS ExpenditureCategoryIsDeleted,    
            [EC].[IsStaffLevel] AS ExpenditureCategoryIsStaffLevel,    
            [EC].[Name] AS ExpenditureCategoryName,    
            [EC].[UpdatedDate] AS ExpenditureCategoryUpdatedDate,    
            [EC].[IsFixed] AS ExpenditureCategoryIsFixed,                
            [EC].[IsMonthly] AS ExpenditureCategoryIsMonthly,    
            [EC].[IsVendorStaff] AS ExpenditureCategoryIsVendorStaff,    
            [EC].[IsSystemDefined] AS ExpenditureCategoryIsSystemDefined,    
            [EC].[AppendMonth] AS ExpenditureCategoryAppendMonth,    
            [EC].[IsVendorStaffAndOther] AS ExpenditureCategoryIsVendorStaffAndOther,    
            [E].[ExpenditureID],    
            [E].[IsDeleted],    
            E.[OBJCode],    
            [E].[UpdatedDate]                
        FROM [dbo].[Expenditures] E    
        INNER JOIN [dbo].[Offices] O ON [E].[OfficeID] = [O].[OfficeID]                
        INNER JOIN [dbo].[Budgets] B ON [E].[OfficeID] = [O].[OfficeID]    
        INNER JOIN [dbo].[FiscalYears] FY ON [E].[FiscalYearID] = [FY].[FiscalYearID]    
        INNER JOIN [dbo].[ExpenditureCategories] EC ON [E].[ExpenditureCategoryID] = [EC].[ExpenditureCategoryID]                
        WHERE [E].[ExpenditureID] = @ExpenditureId    
END
