
CREATE PROCEDURE [dbo].[Proc_PURCHASEORDER_GetbyPurchaseId]    
@PurchaseId [bigint]    
AS    
BEGIN    
    
SELECT [P].[BudgetID],    
        [B].[Amount] AS BudgetAmount,    
        [B].[CreatedDate] AS BudgetCreatedDate,    
        [B].[IsDefault] AS BudgetIsDefault,    
        [B].[IsDeleted] AS BudgetIsDeleted,    
        [B].[Name] AS BudgetName,    
        [B].[UpdatedDate] AS BudgetUpdatedDate,  
        [B].IsDeduct AS BudgetIsDeduct,    
        P.[CreatedDate],    
        P.[DateOfTransaction],    
        P.[FiscalYearID],    
        [FY].[CreatedDate] AS FiscalYearCreatedDate,    
        [FY].[Name] AS FiscalYearName,    
        [FY].[StartDate] AS FiscalYearStartDate,    
        [FY].[EndDate] AS FiscalYearEndDate,    
        [FY].[UpdatedDate] AS FiscalYearUpdatedDate,    
        [FY].[Year] AS FiscalYearYear,    
        P.[ImportID],    
        [POI].[CreatedDate] AS PurchaseOrderImportCreatedDate,    
        [POI].[FileName] AS PurchaseOrderImportFileName,    
        [POI].[ImportGUID] AS PurchaseOrderImportGUID,    
        [POI].[UpdatedDate] AS PurchaseOrderImportUpdatedDate,    
        P.[IsDeleted],    
        P.[OBJCode],    
        P.[OfficeID],    
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
        P.[POAdjAmtSum],    
        P.[POAmtSum],    
        P.[POBalSum],    
        P.[PONumber],    
        P.[PurchaseOrderID],    
        P.[UpdatedDate],    
        P.[UpdatedDate],    
        P.[VendorName],    
        P.[VoucherAmtSum],    
        [POD].[CreatedDate] AS PurchaseOrderDescriptionCreatedDate,    
        [POD].[DescriptionID] AS PurchaseOrderDescriptionDescriptionID,    
        [POD].[DescriptionText] AS PurchaseOrderDescriptionDescriptionText,    
        [POD].[PONumber] AS PurchaseOrderDescriptionPONumber,    
        [POD].[UpdatedDate] AS PurchaseOrderDescriptionUpdatedDate,
		isnull(POAO.AlternateOfficeID,0) 'AlternateOfficeID'
        FROM [dbo].[PurchaseOrders] P INNER JOIN [dbo].[Budgets] B ON P.[BudgetID] = [B].[BudgetID]    
        INNER JOIN [dbo].[FiscalYears] FY ON FY.[FiscalYearID] = [P].[FiscalYearID]    
        LEFT JOIN [dbo].[PurchaseOrderImports] POI ON P.[ImportID] = [POI].[ImportID]    
        LEFT JOIN [dbo].[PurchaseOrderDescriptions] POD ON P.[PONumber] = [POD].[PONumber]    
        INNER JOIN [dbo].[Offices] O ON O.[OfficeID] = [P].[OfficeID]     
		left outer join PurchaseOrdersAlternateOffice POAO on POAO.[PONumber]=p.[PONumber]
        WHERE [P].[PurchaseOrderID] = @PurchaseId    
        END
