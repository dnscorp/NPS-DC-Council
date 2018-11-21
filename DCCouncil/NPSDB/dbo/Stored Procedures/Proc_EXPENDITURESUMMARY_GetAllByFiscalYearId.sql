
CREATE PROCEDURE Proc_EXPENDITURESUMMARY_GetAllByFiscalYearId  
(  
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
          
        WITH CTEExpenditureSummary AS  
        (  
            SELECT  
                ROW_NUMBER() OVER(ORDER BY   
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [O].[Name] END,  
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [O].[Name] END DESC,  
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0) END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0) END DESC,  
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0) END,  
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0) END DESC,  
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [dbo].udf_GetBurnRate(ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0),ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0)) END,  
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [dbo].udf_GetBurnRate(ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0),ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0)) END DESC  
                ) AS RowNumber,  
                O.[OfficeID],  
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
                ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0) AS TotalExpenditureAmount,  
                ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0) AS TotalBudgetAMount,  
                [dbo].udf_GetBurnRate(ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0),ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0)) AS BurnRate  
            FROM [dbo].[Offices] O              
            WHERE [O].[IsDeleted] = 0  
            AND [dbo].[udf_CheckIfOfficeActiveInFiscalYear]([O].[OfficeID],@FiscalYearId) = 1  
        )  
          
        SELECT  
            OfficeID,  
            OfficeActiveFrom,  
            OfficeActiveTo,  
            OfficeCreatedDate,  
            OfficeIndexCode,  
            OfficeIndexTitle,  
            OfficeIsDeleted,  
            OfficeName,  
            OfficePCA,  
            OfficePCATitle,  
            OfficeUpdatedDate,
             OfficeCompCode,  
            [TotalBudgetAmount],  
            [TotalExpenditureAmount],  
            [BurnRate]  
        FROM CTEExpenditureSummary  
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;  
          
        WITH CTEExpenditureSummary AS  
        (  
            SELECT  
                ROW_NUMBER() OVER(ORDER BY   
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [O].[Name] END,  
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [O].[Name] END DESC,  
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0) END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0) END DESC,  
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0) END,  
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0) END DESC,  
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [dbo].udf_GetBurnRate(ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0),ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0)) END,  
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [dbo].udf_GetBurnRate(ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0),ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0)) END DESC          
        
                ) AS RowNumber,  
                O.[OfficeID],  
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
                ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0) AS TotalExpenditureAmount,  
                ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0) AS TotalBudgetAMount,  
                [dbo].udf_GetBurnRate(ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0),ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0)) AS BurnRate  
            FROM [dbo].[Offices] O              
            WHERE [O].[IsDeleted] = 0   
            AND [dbo].[udf_CheckIfOfficeActiveInFiscalYear]([O].[OfficeID],@FiscalYearId) = 1  
        )          
        SELECT MAX(RowNumber) AS TotalRowCount  
        FROM CTEExpenditureSummary   
          
        END  
    ELSE  
    BEGIN  
     SELECT  
            O.[OfficeID],  
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
            ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0) AS TotalExpenditureAmount,  
            ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0) AS TotalBudgetAMount,  
            [dbo].udf_GetBurnRate(ISNULL(dbo.udf_GetTotalExpenditureAmount([O].[OfficeID],@FiscalYearId),0),ISNULL([dbo].udf_GetTotalBudgetAmount([O].[OfficeID],@FiscalYearId),0)) AS BurnRate  
        FROM [dbo].[Offices] O              
        WHERE [O].[IsDeleted] = 0    
        AND [dbo].[udf_CheckIfOfficeActiveInFiscalYear]([O].[OfficeID],@FiscalYearId) = 1  
    END   
END
