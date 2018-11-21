

CREATE PROCEDURE [dbo].[Proc_PURCHASEORDERIMPORTSUMMARY_GetAllByGuid]  
(  
    @ImportGUID [uniqueidentifier]  
)  
AS  
BEGIN  
    SELECT  
        [POIS].[BudgetID],  
        [B].[Amount] AS BudgetAmount,  
        [B].[CreatedDate] AS BudgetCreatedDate,  
        [B].[IsDefault] AS BudgetIsDefault,  
        [B].[IsDeleted] AS BudgetIsDeleted,  
        [B].[Name] AS BudgetName,  
        [B].[UpdatedDate] AS BudgetUpdatedDate,  
        [B].IsDeduct AS BudgetIsDeduct,
        [POIS].[CreatedDate],  
        [POIS].[DateOfTransaction],  
        [POIS].[FiscalYearID],  
        [FY].[CreatedDate] AS FiscalYearCreatedDate,  
        [FY].[Name] AS FiscalYearName,  
        [FY].[StartDate] AS FiscalYearStartDate,  
        [FY].[EndDate] AS FiscalYearEndDate,  
        [FY].[UpdatedDate] AS FiscalYearUpdatedDate,  
        [FY].[Year] AS FiscalYearYear,   
        [POIS].[ImportID],  
        [POI].[CreatedDate] AS PurchaseOrderImportCreatedDate,  
        [POI].[FileName] AS PurchaseOrderImportFileName,  
        [POI].[ImportGUID] AS PurchaseOrderImportGUID,  
        [POI].[UpdatedDate] AS PurchaseOrderImportUpdatedDate,  
        [POIS].[ImportStatus],  
        [POIS].[IsDeleted],  
        [POIS].[OBJCode],  
        [POIS].[OfficeID],  
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
        O.CompCode AS OfficeCompCode,
        [POIS].[POAdjAmtSum],  
        [POIS].[POAmtSum],  
        [POIS].[POBalSum],  
        [POIS].[PONumber],  
        [POIS].[PurchaseOrderImportSummaryID],  
        [POIS].[UpdatedDate],  
        [POIS].[VendorName],  
        [POIS].[VoucherAmtSum]  
    FROM [dbo].[PurchaseOrderImportSummary] POIS  
    INNER JOIN [dbo].[Budgets] B ON [POIS].[BudgetID] = [B].[BudgetID]  
    INNER JOIN [dbo].[FiscalYears] FY ON [POIS].[FiscalYearID] = [FY].[FiscalYearID]  
    INNER JOIN [dbo].[PurchaseOrderImports] POI ON [POIS].[ImportID] = [POI].[ImportID] AND [POI].[ImportGUID] = @ImportGUID  
    INNER JOIN [dbo].[Offices] O ON [POIS].[OfficeID] = [O].[OfficeID]      
    ORDER BY [POIS].[DateOfTransaction]  
END
