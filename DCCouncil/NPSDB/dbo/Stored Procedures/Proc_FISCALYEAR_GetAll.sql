CREATE PROCEDURE [dbo].[Proc_FISCALYEAR_GetAll]
(
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
        
        WITH CTEUsers AS
        (
            SELECT
                ROW_NUMBER() OVER(ORDER BY 
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [FY].[Name] END,
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [FY].[Name] END DESC,
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [FY].[Year] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [FY].[Year] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [FY].[StartDate] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [FY].[StartDate] END DESC,                
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [FY].[EndDate] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [FY].[EndDate] END DESC,                
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [FY].[CreatedDate] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [FY].[CreatedDate] END DESC,
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [FY].[UpdatedDate] END,
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [FY].[UpdatedDate] END DESC                    
                ) AS RowNumber,
                [FY].[CreatedDate],
                [FY].[FiscalYearID],                
                [FY].[Name],
                [FY].[StartDate],
                [FY].[EndDate],
                [FY].[UpdatedDate],
                [FY].[Year]
            FROM [dbo].[FiscalYears] FY            
        )            
        SELECT 
            [FiscalYearID],            
            [Name],
            [StartDate],
            [EndDate],
            [Year],            
            [CreatedDate],
            [UpdatedDate]            
        FROM CTEUsers
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;
        
        WITH CTEUsers AS
        (
            SELECT
                ROW_NUMBER() OVER(ORDER BY 
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [FY].[Name] END,
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [FY].[Name] END DESC,
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [FY].[Year] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [FY].[Year] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [FY].[StartDate] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [FY].[StartDate] END DESC,                
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [FY].[EndDate] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [FY].[EndDate] END DESC,                
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [FY].[CreatedDate] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [FY].[CreatedDate] END DESC,
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [FY].[UpdatedDate] END,
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [FY].[UpdatedDate] END DESC                      
                ) AS RowNumber,                   
        		[FY].[CreatedDate],
                [FY].[FiscalYearID],                
                [FY].[Name],
                [FY].[StartDate],
                [FY].[EndDate],
                [FY].[UpdatedDate],
                [FY].[Year]
            FROM [dbo].[FiscalYears] FY            
        )            
        SELECT MAX(RowNumber) AS TotalRowCount
        FROM CTEUsers 
    END
    ELSE
    BEGIN
        SELECT
        	[FY].[CreatedDate],
            [FY].[FiscalYearID],            
            [FY].[Name],
            [FY].[StartDate],
            [FY].[EndDate],
            [FY].[UpdatedDate],
            [FY].[Year]
        FROM [dbo].[FiscalYears] FY        
    END 
END






