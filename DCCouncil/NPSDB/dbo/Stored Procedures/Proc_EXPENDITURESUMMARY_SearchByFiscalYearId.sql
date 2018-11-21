CREATE PROCEDURE [dbo].[Proc_EXPENDITURESUMMARY_SearchByFiscalYearId]
(
    @SearchText [nvarchar](MAX),
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
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [ES].[TotalBudgetAmount] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [ES].[TotalBudgetAmount] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [ES].[TotalExpenditureAmount] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [ES].[TotalExpenditureAmount] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [ES].[BurnRate] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [ES].[BurnRate] END DESC                
                ) AS RowNumber,
                ES.[OfficeID],
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
                [FY].[FiscalYearID],
                [FY].[CreatedDate] AS FiscalYearCreatedDate,
                [FY].[Name] AS FiscalYearName,
                [FY].[StartDate] AS FiscalYearStartDate,
                [FY].[EndDate] AS FiscalYearEndDate,
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
                [FY].[Year] AS FiscalYearYear, 
                [ES].[TotalBudgetAmount],
                [ES].[TotalExpenditureAmount],
                [ES].[BurnRate]
            FROM [dbo].[ExpenditureSummary] ES
            INNER JOIN [dbo].[Offices] O ON [O].[OfficeID] = [ES].[OfficeID]
            INNER JOIN [dbo].[FiscalYears] FY ON [FY].[FiscalYearID] = [ES].[FiscalYearID]
            WHERE [FY].[FiscalYearID] = @FiscalYearId        
            AND 
            (O.[Name] LIKE '%'+@SearchText+'%')
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
            [FiscalYearID],
            FiscalYearCreatedDate,
            FiscalYearName,
            FiscalYearStartDate,
            FiscalYearEndDate,
            FiscalYearUpdatedDate,
            FiscalYearYear,
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
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [ES].[TotalBudgetAmount] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [ES].[TotalBudgetAmount] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [ES].[TotalExpenditureAmount] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [ES].[TotalExpenditureAmount] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [ES].[BurnRate] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [ES].[BurnRate] END DESC                
                ) AS RowNumber,
                ES.[OfficeID],
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
                [FY].[FiscalYearID],
                [FY].[CreatedDate] AS FiscalYearCreatedDate,
                [FY].[Name] AS FiscalYearName,
                [FY].[StartDate] AS FiscalYearStartDate,
                [FY].[EndDate] AS FiscalYearEndDate,
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
                [FY].[Year] AS FiscalYearYear, 
                [ES].[TotalBudgetAmount],
                [ES].[TotalExpenditureAmount],
                [ES].[BurnRate]
            FROM [dbo].[ExpenditureSummary] ES
            INNER JOIN [dbo].[Offices] O ON [O].[OfficeID] = [ES].[OfficeID]
            INNER JOIN [dbo].[FiscalYears] FY ON [FY].[FiscalYearID] = [ES].[FiscalYearID]
            WHERE [FY].[FiscalYearID] = @FiscalYearId        
            AND 
            (O.[Name] LIKE '%'+@SearchText+'%')
        )
        
        SELECT MAX(RowNumber) AS TotalRowCount
        FROM CTEExpenditureSummary 
        
        END
    ELSE
    BEGIN
    	SELECT
            ES.[OfficeID],
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
            [FY].[FiscalYearID],
            [FY].[CreatedDate] AS FiscalYearCreatedDate,
            [FY].[Name] AS FiscalYearName,
            [FY].[StartDate] AS FiscalYearStartDate,
            [FY].[EndDate] AS FiscalYearEndDate,
            [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
            [FY].[Year] AS FiscalYearYear, 
            [ES].[TotalBudgetAmount],
            [ES].[TotalExpenditureAmount],
            [ES].[BurnRate]
        FROM [dbo].[ExpenditureSummary] ES
        INNER JOIN [dbo].[Offices] O ON [O].[OfficeID] = [ES].[OfficeID]
        INNER JOIN [dbo].[FiscalYears] FY ON [FY].[FiscalYearID] = [ES].[FiscalYearID]
        WHERE [FY].[FiscalYearID] = @FiscalYearId
        AND 
        (O.[Name] LIKE '%'+@SearchText+'%')
    END 
END


