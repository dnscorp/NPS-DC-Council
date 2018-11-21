
CREATE PROCEDURE Proc_BUDGET_GetByBudgetId    
(    
    @BudgetId bigint    
)    
AS    
BEGIN    
    SELECT    
        [B].[Amount],    
        [B].[BudgetID],    
        [B].[CreatedDate],    
        [B].[FiscalYearID],    
        [FY].[CreatedDate] AS FiscalYearCreatedDate,    
        [FY].[Name] AS FiscalYearName,    
        [FY].[StartDate] AS FiscalYearStartDate,    
        [FY].[EndDate] AS FiscalYearEndDate,    
        [FY].[UpdatedDate] AS FiscalYearUpdatedDate,    
        [FY].[Year] AS FiscalYearYear,                  
        [B].[IsDefault],    
        [B].[IsDeleted],    
        [B].[Name],    
        [B].[OfficeID],   
        [B].IsDeduct,   
        [O].[ActiveFrom] AS OfficeActiveFrom,    
        [O].[ActiveTo] AS OfficeActiveTo,    
        [O].[CreatedDate] AS OfficeCreatedDate,    
        [O].[IndexCode] AS OfficeIndexCode,    
        [O].[IndexTitle] AS OfficeIndexTitle,    
        [O].[IsDeleted] AS OfficeIsDeleted,    
        [O].[UpdatedDate] AS OfficeUpdatedDate,    
        [O].[Name] AS OfficeName,    
        [O].[PCA] AS OfficePCA,    
        [O].[PCATitle] AS OfficePCATitle,
        [O].CompCode AS OfficeCompCode,
        [B].[UpdatedDate]    
    FROM [dbo].[Budgets] [B]    
    INNER JOIN [dbo].[Offices] O ON [B].[OfficeID] = [O].[OfficeID]    
    INNER JOIN [dbo].[FiscalYears] FY ON [B].[FiscalYearID] = [FY].[FiscalYearID]    
    WHERE [B].[BudgetID] = @BudgetId    
END
