CREATE VIEW [dbo].[ExpenditureSummary]
AS
    SELECT 
        O.[OfficeID],      
        [FY].[FiscalYearID],
        SUM(ISNULL([E].[Amount],0)) AS TotalExpenditureAmount,
        SUM(ISNULL([B].[Amount],0)) AS TotalBudgetAmount,
        (SUM(ISNULL([E].[Amount],0))/SUM(ISNULL([B].[Amount],0)))*100 AS BurnRate
    FROM [dbo].[Offices] O
    LEFT JOIN [dbo].[Expenditures] E ON [E].[OfficeID] = [O].[OfficeID] AND [E].[IsDeleted] = 0        
    LEFT JOIN [dbo].[Budgets] B ON [B].[OfficeID] = [O].[OfficeID] AND [B].[IsDeleted] = 0
    LEFT JOIN [dbo].[FiscalYears] FY ON [B].[FiscalYearID] = [FY].[FiscalYearID]
    WHERE 
        dbo.udf_CheckIfOfficeActiveInFiscalYear([O].[OfficeID],[FY].[FiscalYearID]) = 1        
        AND [O].[IsDeleted] = 0        
    GROUP BY [O].[OfficeID],[FY].[FiscalYearID]


