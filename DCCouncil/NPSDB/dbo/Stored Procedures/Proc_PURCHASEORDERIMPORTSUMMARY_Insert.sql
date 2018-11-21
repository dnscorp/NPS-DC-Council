CREATE PROCEDURE [dbo].[Proc_PURCHASEORDERIMPORTSUMMARY_Insert]
(
    @ImportGUID [uniqueidentifier]
)
AS
BEGIN
    WITH POSumCTE
    AS
    (
        SELECT 
            [POI].[ImportID],            
            [POII].[PONumber],
            [POII].[VendorName],
            MAX([POII].[CompObject]) AS OBJCode,
            MAX([POII].[EffDate]) AS DateOfTransaction,
            SUM([POII].[POAMT]) AS POAmtSum,
            SUM([POII].[POADJAMT]) AS POAdjAmtSum,
            SUM([POII].[VOUCHERAMT]) AS VoucherAmtSum,
            SUM([POII].[POBAL]) AS POBalSum,
            [POII].[IndexCode] AS IndexCode,
            [POII].[PCA] AS PCA,
            MAX([POII].[FISCAL_YEAR]) AS FiscalYear
        FROM [dbo].[PurchaseOrderImportItems] POII
        INNER JOIN [dbo].[PurchaseOrderImports] POI ON [POII].[ImportID] = [POI].[ImportID] AND [POI].[ImportGUID] = @ImportGUID
        GROUP BY [POII].[PONumber],[POI].[ImportID],[POII].[VendorName],[POII].[IndexCode],[POII].[PCA]            
    ),
    PONewAllDetailsCTE
    AS
    (
        SELECT 
            CTE1.PONumber,
            CTE1.VendorName,
            CTE1.OBJCode,
            CTE1.DateOfTransaction,
            CTE1.POAmtSum,
            CTE1.POAdjAmtSum,
            CTE1.VoucherAmtSum,
            CTE1.POBalSum,
            [O].[OfficeID],                        
            [FY].[FiscalYearID],            
            [B].[BudgetID],            
            [CTE1].[ImportID]                       
        FROM POSumCTE CTE1
        INNER JOIN [dbo].[Offices] O ON CTE1.PCA = [O].[PCA] AND CTE1.IndexCode = [O].[IndexCode] AND [O].[IsDeleted]=0
        INNER JOIN [dbo].[FiscalYears] FY ON CTE1.[FiscalYear] = [FY].[Year]
        INNER JOIN [dbo].[Budgets] B ON [B].[FiscalYearID] = [FY].[FiscalYearID] AND B.[OfficeID] = [O].[OfficeID] AND B.[IsDefault] = 1 AND [B].[IsDeleted] = 0
        INNER JOIN [dbo].[PurchaseOrderImports] POI ON CTE1.[ImportID] = [POI].[ImportID]
    )
    INSERT INTO [dbo].[PurchaseOrderImportSummary] 
        ([VendorName],
        [OBJCode],
        [DateOfTransaction],
        [PONumber],
        [POAmtSum],
        [POAdjAmtSum],
        [VoucherAmtSum],
        [POBalSum],
        [OfficeID],
        [FiscalYearID],
        [BudgetID],
        [IsDeleted],
        [ImportID],
        [CreatedDate],
        [UpdatedDate],
        [ImportStatus])
    SELECT
         [CTE].[VendorName],
         [CTE].[OBJCode],
         [CTE].[DateOfTransaction],
         [CTE].[PONumber],
         [CTE].[POAmtSum],
         [CTE].[POAdjAmtSum],
         [CTE].[VoucherAmtSum],
         [CTE].[POBalSum],
         [CTE].[OfficeID],
         [CTE].[FiscalYearID],
         [CTE].[BudgetID],
         0,
         [CTE].[ImportID],
         GETDATE(),
         NULL,
         0
    FROM PONewAllDetailsCTE CTE
END
