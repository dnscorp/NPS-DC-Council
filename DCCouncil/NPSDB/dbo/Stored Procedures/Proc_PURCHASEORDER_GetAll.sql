
CREATE PROCEDURE [dbo].[Proc_PURCHASEORDER_GetAll]      
(      
    @SearchText nvarchar(MAX),      
    @OfficeId [bigint],      
    @AsOfDate [datetime],      
    @FiscalYearId [bigint],          
    @PageSize [int] = -1,      
    @PageNumber [int] = 0,      
    @SortField [int] = 0,      
    @SortDirection [int] = 0       
)      
AS      
BEGIN      
    IF @PageSize <> -1      
    BEGIN      
        DECLARE @intStartRow int;      
        DECLARE @intEndRow int;      
              
        SET @intStartRow = (@PageNumber -1) * @PageSize + 1;      
        SET @intEndRow = @PageNumber * @PageSize;       
              
        WITH CTEPurchaseOrders AS      
        (      
            SELECT      
                ROW_NUMBER() OVER(ORDER BY      
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [PO].[DateOfTransaction] END,      
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [PO].[DateOfTransaction] END DESC,      
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [PO].[VendorName] END,      
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [PO].[VendorName] END DESC,                      
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [PO].OBJCode END,      
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN PO.OBJCode END DESC,      
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN PO.[PONumber] END,      
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN PO.[PONumber] END,      
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN PO.[POAmtSum] END,      
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN PO.[POAmtSum] END,      
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN PO.[POAdjAmtSum] END,      
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN PO.[POAdjAmtSum] END,      
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN PO.[VoucherAmtSum] END,      
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN PO.[VoucherAmtSum] END,      
                CASE WHEN @SortField = 7 AND @SortDirection = 0 THEN PO.[POBalSum] END,      
                CASE WHEN @SortField = 7 AND @SortDirection = 1 THEN PO.[POBalSum] END,      
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [O].[Name] END,      
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [O].[Name] END DESC,      
                CASE WHEN @SortField = 9 AND @SortDirection = 0 THEN [FY].[Year] END,      
                CASE WHEN @SortField = 9 AND @SortDirection = 1 THEN [FY].[Year] END DESC,    
                CASE WHEN @SortField = 10 AND @SortDirection = 0 THEN [POD].[DescriptionText] END,      
                CASE WHEN @SortField = 10 AND @SortDirection = 1 THEN [POD].[DescriptionText] END DESC)      
                AS RowNumber,                      
                [PO].[BudgetID],      
                [B].[Amount] AS BudgetAmount,      
                [B].[CreatedDate] AS BudgetCreatedDate,      
                [B].[IsDefault] AS BudgetIsDefault,      
                [B].[IsDeleted] AS BudgetIsDeleted,      
                [B].[Name] AS BudgetName,      
                [B].[UpdatedDate] AS BudgetUpdatedDate,    
                [B].IsDeduct AS BudgetIsDeduct,    
                PO.[CreatedDate],      
                [PO].[DateOfTransaction],      
                [PO].[FiscalYearID],      
                [FY].[CreatedDate] AS FiscalYearCreatedDate,      
                [FY].[Name] AS FiscalYearName,      
                [FY].[StartDate] AS FiscalYearStartDate,      
                [FY].[EndDate] AS FiscalYearEndDate,      
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,      
                [FY].[Year] AS FiscalYearYear,      
                [PO].[ImportID],      
                [POI].[CreatedDate] AS PurchaseOrderImportCreatedDate,      
                [POI].[FileName] AS PurchaseOrderImportFileName,      
                [POI].[ImportGUID] AS PurchaseOrderImportGUID,      
                [POI].[UpdatedDate] AS PurchaseOrderImportUpdatedDate,      
   [PO].[IsDeleted],      
                [PO].[OBJCode],      
                [PO].[OfficeID],      
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
                [PO].[POAdjAmtSum],      
                [PO].[POAmtSum],      
                [PO].[POBalSum],      
                [PO].[PONumber],      
                [PO].[PurchaseOrderID],      
                [PO].[UpdatedDate],      
                [PO].[VendorName],      
                [PO].[VoucherAmtSum],    
                [POD].[CreatedDate] AS PurchaseOrderDescriptionCreatedDate,    
                [POD].[DescriptionID] AS PurchaseOrderDescriptionDescriptionID,    
                [POD].[DescriptionText] AS PurchaseOrderDescriptionDescriptionText,    
                [POD].[PONumber] AS PurchaseOrderDescriptionPONumber,    
                [POD].[UpdatedDate] AS PurchaseOrderDescriptionUpdatedDate,
				isnull(POAO.AlternateOfficeID,0) AS AlternateOfficeID
            FROM [dbo].PurchaseOrders PO      
            INNER JOIN [dbo].[Budgets] B ON [PO].[BudgetID] = [B].[BudgetID]      
            INNER JOIN [dbo].[FiscalYears] FY ON [PO].[FiscalYearID] = [FY].[FiscalYearID]      
            INNER JOIN [dbo].[Offices] O ON [PO].[OfficeID] = [O].[OfficeID]      
            LEFT JOIN [dbo].[PurchaseOrderImports] POI ON [PO].[ImportID] = [POI].[ImportID]      
            LEFT JOIN [dbo].[PurchaseOrderDescriptions] POD ON PO.[PONumber] = [POD].[PONumber]
			left join PurchaseOrdersAlternateOffice POAO on POAO.PONumber=po.PONumber    
            WHERE [PO].[IsDeleted] = 0      
            AND      
            (      
                (LEN(@SearchText) = 0 OR (@SearchText IS NULL))       
                OR      
                (      
                    [PO].[OBJCode] LIKE '%'+@SearchText+'%' OR [PO].[PONumber] LIKE '%'+@SearchText+'%' OR      
                    [PO].[VendorName] LIKE '%'+@SearchText+'%' OR [FY].[Name] LIKE '%'+@SearchText+'%' OR      
                    [O].[Name] LIKE '%'+@SearchText+'%'      
                )      
            )      
            AND      
            ([O].[OfficeID] = @OfficeId OR @OfficeId IS NULL)      
            AND      
            ([FY].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)      
            AND      
            ([PO].[DateOfTransaction] <= @AsOfDate OR @AsOfDate IS NULL)      
        )              
        SELECT      
            BudgetID,      
            BudgetAmount,      
            BudgetCreatedDate,      
            BudgetIsDefault,      
            BudgetIsDeleted,      
            BudgetName,      
            BudgetUpdatedDate,   
            BudgetIsDeduct ,    
            [CreatedDate],      
            [DateOfTransaction],      
            [FiscalYearID],      
            FiscalYearCreatedDate,      
            FiscalYearName,      
            FiscalYearStartDate,      
            FiscalYearEndDate,      
            FiscalYearUpdatedDate,      
            FiscalYearYear,      
            [ImportID],      
            PurchaseOrderImportCreatedDate,      
            PurchaseOrderImportFileName,      
            PurchaseOrderImportGUID,      
            PurchaseOrderImportUpdatedDate,      
            [IsDeleted],      
            [OBJCode],      
            [OfficeID],      
            OfficeActiveFrom,      
            OfficeActiveTo,      
            OfficeCreatedDate,      
            OfficeIndexCode,      
            OfficeIndexTitle,      
            OfficeIsDeleted,     
            OfficeUpdatedDate,      
            OfficeName,      
            OfficePCA,      
            OfficePCATitle, 
            OfficeCompCode,     
            [POAdjAmtSum],      
            [POAmtSum],      
            [POBalSum],      
            [PONumber],      
            [PurchaseOrderID],      
            [UpdatedDate],      
            [VendorName],      
            [VoucherAmtSum],    
   [PurchaseOrderDescriptionCreatedDate],    
            [PurchaseOrderDescriptionDescriptionID],    
            [PurchaseOrderDescriptionDescriptionText],    
            [PurchaseOrderDescriptionPONumber],    
            [PurchaseOrderDescriptionUpdatedDate],
			AlternateOfficeID
        FROM CTEPurchaseOrders      
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;      
              
        WITH CTEPurchaseOrders AS      
        (      
            SELECT      
                ROW_NUMBER() OVER(ORDER BY      
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [PO].[DateOfTransaction] END,      
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [PO].[DateOfTransaction] END DESC,      
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [PO].[VendorName] END,      
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [PO].[VendorName] END DESC,                      
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [PO].OBJCode END,      
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN PO.OBJCode END DESC,      
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN PO.[PONumber] END,      
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN PO.[PONumber] END,      
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN PO.[POAmtSum] END,      
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN PO.[POAmtSum] END,      
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN PO.[POAdjAmtSum] END,      
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN PO.[POAdjAmtSum] END,      
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN PO.[VoucherAmtSum] END,      
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN PO.[VoucherAmtSum] END,      
                CASE WHEN @SortField = 7 AND @SortDirection = 0 THEN PO.[POBalSum] END,      
                CASE WHEN @SortField = 7 AND @SortDirection = 1 THEN PO.[POBalSum] END,      
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [O].[Name] END,      
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [O].[Name] END DESC,      
                CASE WHEN @SortField = 9 AND @SortDirection = 0 THEN [FY].[Year] END,      
                CASE WHEN @SortField = 9 AND @SortDirection = 1 THEN [FY].[Year] END DESC,    
                CASE WHEN @SortField = 10 AND @SortDirection = 0 THEN [POD].[DescriptionText] END,      
                CASE WHEN @SortField = 10 AND @SortDirection = 1 THEN [POD].[DescriptionText] END DESC)    
                AS RowNumber,                      
                [PO].[BudgetID],      
                [B].[Amount] AS BudgetAmount,      
                [B].[CreatedDate] AS BudgetCreatedDate,      
                [B].[IsDefault] AS BudgetIsDefault,      
                [B].[IsDeleted] AS BudgetIsDeleted,      
                [B].[Name] AS BudgetName,      
                [B].[UpdatedDate] AS BudgetUpdatedDate,  
                [B].IsDeduct AS BudgetIsDeduct,      
                PO.[CreatedDate],      
                [PO].[DateOfTransaction],      
                [PO].[FiscalYearID],      
                [FY].[CreatedDate] AS FiscalYearCreatedDate,      
                [FY].[Name] AS FiscalYearName,      
                [FY].[StartDate] AS FiscalYearStartDate,      
                [FY].[EndDate] AS FiscalYearEndDate,      
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,      
                [FY].[Year] AS FiscalYearYear,      
  [PO].[ImportID],      
                [POI].[CreatedDate] AS PurchaseOrderImportCreatedDate,      
                [POI].[FileName] AS PurchaseOrderImportFileName,      
                [POI].[ImportGUID] AS PurchaseOrderImportGUID,      
                [POI].[UpdatedDate] AS PurchaseOrderImportUpdatedDate,      
                [PO].[IsDeleted],      
                [PO].[OBJCode],      
                [PO].[OfficeID],      
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
                [PO].[POAdjAmtSum],      
                [PO].[POAmtSum],      
                [PO].[POBalSum],      
                [PO].[PONumber],      
                [PO].[PurchaseOrderID],      
                [PO].[UpdatedDate],      
                [PO].[VendorName],      
                [PO].[VoucherAmtSum],    
                [POD].[CreatedDate] AS PurchaseOrderDescriptionCreatedDate,    
                [POD].[DescriptionID] AS PurchaseOrderDescriptionDescriptionID,    
                [POD].[DescriptionText] AS PurchaseOrderDescriptionDescriptionText,    
                [POD].[PONumber] AS PurchaseOrderDescriptionPONumber,    
                [POD].[UpdatedDate] AS PurchaseOrderDescriptionUpdatedDate,
				isnull(POAO.AlternateOfficeID,0) AS AlternateOfficeID    
            FROM [dbo].PurchaseOrders PO      
            INNER JOIN [dbo].[Budgets] B ON [PO].[BudgetID] = [B].[BudgetID]      
            INNER JOIN [dbo].[FiscalYears] FY ON [PO].[FiscalYearID] = [FY].[FiscalYearID]      
            INNER JOIN [dbo].[Offices] O ON [PO].[OfficeID] = [O].[OfficeID]      
            LEFT JOIN [dbo].[PurchaseOrderImports] POI ON [PO].[ImportID] = [POI].[ImportID]      
            LEFT JOIN [dbo].[PurchaseOrderDescriptions] POD ON PO.[PONumber] = [POD].[PONumber]
			left join PurchaseOrdersAlternateOffice POAO on POAO.PONumber=po.PONumber        
            WHERE [PO].[IsDeleted] = 0      
            AND      
            (      
                (LEN(@SearchText) = 0 OR (@SearchText IS NULL))       
                OR      
                (      
                    [PO].[OBJCode] LIKE '%'+@SearchText+'%' OR [PO].[PONumber] LIKE '%'+@SearchText+'%' OR      
                    [PO].[VendorName] LIKE '%'+@SearchText+'%' OR [FY].[Name] LIKE '%'+@SearchText+'%' OR      
                    [O].[Name] LIKE '%'+@SearchText+'%'      
                )      
            )      
            AND      
         ([O].[OfficeID] = @OfficeId OR @OfficeId IS NULL)      
            AND      
            ([FY].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)      
            AND      
            ([PO].[DateOfTransaction] <= @AsOfDate OR @AsOfDate IS NULL)      
        )      
        SELECT MAX(RowNumber) AS TotalRowCount      
        FROM CTEPurchaseOrders      
    END      
    ELSE      
    BEGIN      
        SELECT      
            [PO].[BudgetID],      
            [B].[Amount] AS BudgetAmount,      
            [B].[CreatedDate] AS BudgetCreatedDate,      
            [B].[IsDefault] AS BudgetIsDefault,      
            [B].[IsDeleted] AS BudgetIsDeleted,      
            [B].[Name] AS BudgetName,      
            [B].[UpdatedDate] AS BudgetUpdatedDate,  
            [B].IsDeduct AS BudgetIsDeduct,      
            PO.[CreatedDate],      
            [PO].[DateOfTransaction],      
            [PO].[FiscalYearID],      
            [FY].[CreatedDate] AS FiscalYearCreatedDate,      
            [FY].[Name] AS FiscalYearName,      
            [FY].[StartDate] AS FiscalYearStartDate,      
            [FY].[EndDate] AS FiscalYearEndDate,      
            [FY].[UpdatedDate] AS FiscalYearUpdatedDate,      
            [FY].[Year] AS FiscalYearYear,      
            [PO].[ImportID],      
            [POI].[CreatedDate] AS PurchaseOrderImportCreatedDate,      
            [POI].[FileName] AS PurchaseOrderImportFileName,      
            [POI].[ImportGUID] AS PurchaseOrderImportGUID,      
            [POI].[UpdatedDate] AS PurchaseOrderImportUpdatedDate,      
            [PO].[IsDeleted],      
            [PO].[OBJCode],      
            [PO].[OfficeID],      
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
            [PO].[POAdjAmtSum],      
            [PO].[POAmtSum],      
            [PO].[POBalSum],      
            [PO].[PONumber],      
            [PO].[PurchaseOrderID],      
            [PO].[UpdatedDate],      
            [PO].[VendorName],      
            [PO].[VoucherAmtSum],    
            [POD].[CreatedDate] AS PurchaseOrderDescriptionCreatedDate,    
            [POD].[DescriptionID] AS PurchaseOrderDescriptionDescriptionID,    
            [POD].[DescriptionText] AS PurchaseOrderDescriptionDescriptionText,    
            [POD].[PONumber] AS PurchaseOrderDescriptionPONumber,    
            [POD].[UpdatedDate] AS PurchaseOrderDescriptionUpdatedDate,
			isnull(POAO.AlternateOfficeID,0) AS AlternateOfficeID    
        FROM [dbo].PurchaseOrders PO      
        INNER JOIN [dbo].[Budgets] B ON [PO].[BudgetID] = [B].[BudgetID]      
        INNER JOIN [dbo].[FiscalYears] FY ON [PO].[FiscalYearID] = [FY].[FiscalYearID]      
        INNER JOIN [dbo].[Offices] O ON [PO].[OfficeID] = [O].[OfficeID]      
        LEFT JOIN [dbo].[PurchaseOrderImports] POI ON [PO].[ImportID] = [POI].[ImportID]      
        LEFT JOIN [dbo].[PurchaseOrderDescriptions] POD ON PO.[PONumber] = [POD].[PONumber]
		left join PurchaseOrdersAlternateOffice POAO on POAO.PONumber=po.PONumber        
        WHERE [PO].[IsDeleted] = 0      
        AND      
        (      
            (LEN(@SearchText) = 0 OR (@SearchText IS NULL))       
            OR      
            (      
                [PO].[OBJCode] LIKE '%'+@SearchText+'%' OR [PO].[PONumber] LIKE '%'+@SearchText+'%' OR      
                [PO].[VendorName] LIKE '%'+@SearchText+'%' OR [FY].[Name] LIKE '%'+@SearchText+'%' OR      
                [O].[Name] LIKE '%'+@SearchText+'%'      
            )      
        )      
        AND      
        ([O].[OfficeID] = @OfficeId OR @OfficeId IS NULL)      
        AND      
        ([FY].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)      
        AND      
        ([PO].[DateOfTransaction] <= @AsOfDate OR @AsOfDate IS NULL)      
    END      
END





