
CREATE PROCEDURE Proc_BUDGET_GetAll     
(      
    @SearchText [nvarchar](MAX),      
    @OfficeId [bigint],      
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
              
        WITH CTEBudget AS      
        (      
            SELECT      
                ROW_NUMBER() OVER(ORDER BY       
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [B].[Name] END,      
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [B].[Name] END DESC,      
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [B].[IsDefault] END,      
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [B].[IsDefault] END DESC,      
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [B].[Amount] END,      
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [B].[Amount] END DESC,      
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [FY].[Name] END,      
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [FY].[Name] END DESC,      
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [B].[CreatedDate] END,      
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [B].[CreatedDate] END DESC,      
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [B].[UpdatedDate] END,      
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [B].[UpdatedDate] END DESC                          
                ) AS RowNumber,      
                [B].[Amount],      
                [B].[BudgetID],      
                [B].[CreatedDate],      
                [B].[FiscalYearID],      
                [FY].[CreatedDate] AS FiscalYearCreatedDate,      
                [FY].[Name] AS FiscalYearName,                      
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,      
                [FY].[Year] AS FiscalYearYear,      
                [FY].[StartDate] AS FiscalYearStartDate,      
                [FY].[EndDate] AS FiscalYearEndDate,      
                [B].[IsDefault],      
                [B].[IsDeleted],      
                [B].[Name],      
                [B].[OfficeID],    
                [B].[IsDeduct],      
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
            WHERE      
            ([B].[OfficeID] = @OfficeId OR @OfficeId IS NULL)                  
            AND       
            ([B].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)      
            AND       
            (LEN(@SearchText) = 0 OR @SearchText IS NULL OR [B].[Name] LIKE '%'+@SearchText+'%')      
            AND [B].[IsDeleted] = 0      
        )      
              
        SELECT      
            [Amount],      
            [BudgetID],      
            [CreatedDate],      
            [FiscalYearID],      
            FiscalYearCreatedDate,      
            FiscalYearName,      
            FiscalYearStartDate,      
            FiscalYearEndDate,      
            FiscalYearUpdatedDate,      
            FiscalYearYear,                    
            [IsDefault],      
            [IsDeleted],      
            [Name],      
            [OfficeID],     
            [IsDeduct] ,    
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
            [UpdatedDate]      
        FROM [CTEBudget]      
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;      
                  
        WITH CTEBudget AS      
        (      
            SELECT      
                ROW_NUMBER() OVER(ORDER BY       
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [B].[Name] END,      
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [B].[Name] END DESC,      
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [B].[IsDefault] END,      
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [B].[IsDefault] END DESC,      
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [B].[Amount] END,      
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [B].[Amount] END DESC,      
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [FY].[Name] END,      
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [FY].[Name] END DESC,      
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [B].[CreatedDate] END,      
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [B].[CreatedDate] END DESC,      
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [B].[UpdatedDate] END,      
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [B].[UpdatedDate] END DESC                          
                ) AS RowNumber,      
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
                [B].[IsDeduct],      
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
            WHERE       
            ([B].[OfficeID] = @OfficeId OR @OfficeId IS NULL)                  
            AND       
            ([B].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)      
            AND       
            (LEN(@SearchText) = 0 OR @SearchText IS NULL OR [B].[Name] LIKE '%'+@SearchText+'%')      
          AND [B].[IsDeleted] = 0      
        )      
              
        SELECT MAX(RowNumber) AS TotalRowCount      
        FROM CTEBudget      
              
         END      
    ELSE      
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
            [B].[IsDeduct],    
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
        WHERE       
        ([B].[OfficeID] = @OfficeId OR @OfficeId IS NULL)                  
        AND       
        ([B].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)      
        AND       
        (LEN(@SearchText) = 0 OR @SearchText IS NULL OR [B].[Name] LIKE '%'+@SearchText+'%')      
        AND [B].[IsDeleted] = 0    
        ORDER BY [B].IsDefault DESC  
    END          
END
