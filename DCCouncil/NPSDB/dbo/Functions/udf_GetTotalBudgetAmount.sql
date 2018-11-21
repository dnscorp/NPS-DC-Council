CREATE FUNCTION udf_GetTotalBudgetAmount
(
    @OfficeID [bigint],
    @FiscalYearID [bigint]    
)
RETURNS [float]
AS
BEGIN
    DECLARE @TotalBudgetAmount [float]
    
    SELECT @TotalBudgetAmount=SUM(ISNULL([B].[Amount],0)) FROM [dbo].[Offices] [O]
    INNER JOIN [dbo].[Budgets] B ON [O].[OfficeID] = [B].[OfficeID] AND [B].[IsDeleted] = 0   
    INNER JOIN [dbo].[FiscalYears] FY ON [B].[FiscalYearID] = [FY].[FiscalYearID]
    WHERE 
        dbo.udf_CheckIfOfficeActiveInFiscalYear([O].[OfficeID],[FY].[FiscalYearID]) = 1        
        AND [O].[IsDeleted] = 0        
        AND [O].[OfficeID] = @OfficeID                
        AND [FY].[FiscalYearID] = @FiscalYearID
    GROUP BY [O].[OfficeID]
    
    RETURN @TotalBudgetAmount
END
