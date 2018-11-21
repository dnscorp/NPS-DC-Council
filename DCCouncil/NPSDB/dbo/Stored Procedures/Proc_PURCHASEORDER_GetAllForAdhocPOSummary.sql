CREATE PROCEDURE [dbo].[Proc_PURCHASEORDER_GetAllForAdhocPOSummary]      
(      
    @SearchText nvarchar(MAX),      
	@OfficeID [bigint],      
    @AsOfDate [datetime],      
	@StartDate [datetime],      
    @FiscalYearId [bigint]          
)      
AS      
BEGIN      
        
        SELECT      
                 
            SUM([PO].[VoucherAmtSum]  ) 'POExpended', SUM([PO].[POBalSum]) 'POObligated'
        FROM [dbo].[PurchaseOrders] PO      
        INNER JOIN [dbo].[Budgets] B ON [PO].[BudgetID] = [B].[BudgetID]      
        INNER JOIN [dbo].[FiscalYears] FY ON [PO].[FiscalYearID] = [FY].[FiscalYearID]      
        INNER JOIN [dbo].[Offices] O ON [PO].[OfficeID] = [O].[OfficeID]      
        LEFT JOIN [dbo].[PurchaseOrderImports] POI ON [PO].[ImportID] = [POI].[ImportID]      
        LEFT JOIN [dbo].[PurchaseOrderDescriptions] POD ON PO.[PONumber] = [POD].[PONumber]    
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
		AND ([O].OfficeID in (@OfficeID))
        AND      
        ([FY].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)      
        AND  
				([PO].[DateOfTransaction] <= @AsOfDate OR @AsOfDate IS NULL)      
		AND
				(([PO].[DateOfTransaction]>=@StartDate and [PO].[DateOfTransaction]<=@AsOfDate) or @StartDate IS NULL)

END