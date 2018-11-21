CREATE FUNCTION [dbo].[udf_GetTotalExpenditureAmount]
(
    @OfficeID [bigint],
    @FiscalYearID [bigint]
)
RETURNS [float]
AS
BEGIN
    DECLARE @TotalExpenditureAmount [float]
    
    SELECT @TotalExpenditureAmount=SUM(ISNULL([E].[Amount],0)) FROM [dbo].[Offices] [O]
    INNER JOIN [dbo].[Expenditures] E ON [E].[OfficeID] = [O].[OfficeID]
    INNER JOIN [dbo].[Budgets] B ON [E].[BudgetID] = [B].[BudgetID]
    INNER JOIN [dbo].[FiscalYears] FY ON [E].[FiscalYearID] = [FY].[FiscalYearID]
    WHERE 
        dbo.udf_CheckIfOfficeActiveInFiscalYear([O].[OfficeID],[FY].[FiscalYearID]) = 1
        AND [B].[IsDeleted] = 0
        AND [O].[IsDeleted] = 0
        AND [E].[IsDeleted] = 0
        AND [O].[OfficeID] = @OfficeID  
        AND [FY].[FiscalYearID] = @FiscalYearID
    GROUP BY [O].[OfficeID]
    
    
    DECLARE @TotalPOAmountVoucher [float]
    DECLARE @TotalPOAmountPOBal [float]
    
    SELECT @TotalPOAmountVoucher=SUM(ISNULL([PO].[VoucherAmtSum],0)),@TotalPOAmountPOBal=SUM(ISNULL([PO].[POBalSum],0))  
    FROM [dbo].[Offices] [O]
    INNER JOIN [dbo].[PurchaseOrders] PO ON [PO].[OfficeID] = [O].[OfficeID]
    INNER JOIN [dbo].[Budgets] B ON [PO].[BudgetID] = [B].[BudgetID]
    INNER JOIN [dbo].[FiscalYears] FY ON [PO].[FiscalYearID] = [FY].[FiscalYearID]
    WHERE 
        dbo.udf_CheckIfOfficeActiveInFiscalYear([O].[OfficeID],[FY].[FiscalYearID]) = 1
        AND [B].[IsDeleted] = 0
        AND [O].[IsDeleted] = 0
        AND [PO].[IsDeleted] = 0
        AND [O].[OfficeID] = @OfficeID  
        AND [FY].[FiscalYearID] = @FiscalYearID
    GROUP BY [O].[OfficeID]
    
    RETURN ISNULL(@TotalExpenditureAmount,0) + ISNULL(@TotalPOAmountVoucher,0)+ isnull(@TotalPOAmountPOBal,0)
END

