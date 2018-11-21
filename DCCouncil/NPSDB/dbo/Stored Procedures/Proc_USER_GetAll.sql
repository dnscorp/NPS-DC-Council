CREATE PROCEDURE [dbo].[Proc_USER_GetAll]
(
    @SearchText nvarchar(MAX),
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
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [U].[Username] END,
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [U].[Username] END DESC,
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [U].[IsActive] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [U].[IsActive] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [UP].[FirstName] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [UP].[FirstName] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [UP].[LastName] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [UP].[LastName] END DESC,
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [U].[CreatedDate] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [U].[CreatedDate] END DESC,
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [U].[UpdatedDate] END,
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [U].[UpdatedDate] END DESC                    
                ) AS RowNumber,                   
        		[U].[UserID],
                [U].[UserGuid],
                [U].[Username],
                [U].[PasswordHash],
                [U].[IsActive],
                [U].[IsDeleted],
                [U].[LastFiscalYearSelectedID] AS FiscalYearID,
                [FY].[CreatedDate] AS FiscalYearCreatedDate,
                [FY].[Name] AS FiscalYearName,
                [FY].[StartDate] AS FiscalYearStartDate,
                [FY].[EndDate] AS FiscalYearEndDate,
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
                [FY].[Year] AS FiscalYearYear,
                [U].[CreatedDate],
                [U].[UpdatedDate],
                [UP].[UserProfileID],
                [UP].[FirstName] AS UserProfileFirstName,
                [UP].[LastName] AS UserProfileLastName,
                [UP].[CreatedDate] AS UserProfileCreatedDate,
                [UP].[UpdatedDate] AS UserProfileUpdatedDate
            FROM Users U
            INNER JOIN [dbo].[UserProfiles] UP ON [UP].[UserID] = [U].[UserID]
            AND U.IsDeleted = 0
            AND 
            (LEN(@SearchText) = 0 OR @SearchText IS NULL OR [U].[Username] LIKE '%'+@SearchText+'%' OR [UP].[FirstName] LIKE '%'+@SearchText+'%' OR [UP].[LastName] LIKE '%'+@SearchText+'%')
            LEFT JOIN [dbo].[FiscalYears] FY ON [U].[LastFiscalYearSelectedID] = [FY].[FiscalYearID]
        )            
        SELECT 
            [UserID],
            [UserGuid],
            [Username],
            [PasswordHash],
            [IsActive],
            [IsDeleted],
            FiscalYearID,
            FiscalYearCreatedDate,
            FiscalYearName,
            FiscalYearStartDate,
            FiscalYearEndDate,
            FiscalYearUpdatedDate,
            FiscalYearYear,
            [CreatedDate],
            [UpdatedDate],
            [UserProfileID],
            UserProfileFirstName,
            UserProfileLastName,
            UserProfileCreatedDate,
            UserProfileUpdatedDate
        FROM CTEUsers
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;
        
        WITH CTEUsers AS
        (
            SELECT
                ROW_NUMBER() OVER(ORDER BY 
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [U].[Username] END,
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [U].[Username] END DESC,
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [U].[IsActive] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [U].[IsActive] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [UP].[FirstName] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [UP].[FirstName] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [UP].[LastName] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [UP].[LastName] END DESC,
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [U].[CreatedDate] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [U].[CreatedDate] END DESC,
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [U].[UpdatedDate] END,
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [U].[UpdatedDate] END DESC                    
                ) AS RowNumber,                   
        		[U].[UserID],
                [U].[UserGuid],
                [U].[Username],
                [U].[PasswordHash],
                [U].[IsActive],
                [U].[IsDeleted],
                [U].[LastFiscalYearSelectedID] AS FiscalYearID,
                [FY].[CreatedDate] AS FiscalYearCreatedDate,
                [FY].[Name] AS FiscalYearName,
                [FY].[StartDate] AS FiscalYearStartDate,
                [FY].[EndDate] AS FiscalYearEndDate,
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
                [FY].[Year] AS FiscalYearYear,
                [U].[CreatedDate],
                [U].[UpdatedDate],
                [UP].[UserProfileID],
                [UP].[FirstName] AS UserProfileFirstName,
                [UP].[LastName] AS UserProfileLastName,
                [UP].[CreatedDate] AS UserProfileCreatedDate,
                [UP].[UpdatedDate] AS UserProfileUpdatedDate
            FROM Users U
            INNER JOIN [dbo].[UserProfiles] UP ON [UP].[UserID] = [U].[UserID]
            AND U.IsDeleted = 0
            AND 
            (LEN(@SearchText) = 0 OR @SearchText IS NULL OR [U].[Username] LIKE '%'+@SearchText+'%' OR [UP].[FirstName] LIKE '%'+@SearchText+'%' OR [UP].[LastName] LIKE '%'+@SearchText+'%')
            LEFT JOIN [dbo].[FiscalYears] FY ON [U].[LastFiscalYearSelectedID] = [FY].[FiscalYearID]
        )            
        
        SELECT MAX(RowNumber) AS TotalRowCount
        FROM CTEUsers 
    END
    ELSE
    BEGIN
    	SELECT
    		[U].[UserID],
            [U].[UserGuid],
            [U].[Username],
            [U].[PasswordHash],
            [U].[IsActive],
            [U].[IsDeleted],
            [U].[LastFiscalYearSelectedID] AS FiscalYearID,
            [FY].[CreatedDate] AS FiscalYearCreatedDate,
            [FY].[Name] AS FiscalYearName,
            [FY].[StartDate] AS FiscalYearStartDate,
            [FY].[EndDate] AS FiscalYearEndDate,
            [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
            [FY].[Year] AS FiscalYearYear,
            [U].[CreatedDate],
            [U].[UpdatedDate],
            [UP].[UserProfileID],
            [UP].[FirstName] AS UserProfileFirstName,
            [UP].[LastName] AS UserProfileLastName,
            [UP].[CreatedDate] AS UserProfileCreatedDate,
            [UP].[UpdatedDate] AS UserProfileUpdatedDate
        FROM Users U
        INNER JOIN [dbo].[UserProfiles] UP ON [UP].[UserID] = [U].[UserID]
        AND U.IsDeleted = 0
        AND 
        (LEN(@SearchText) = 0 OR @SearchText IS NULL OR [U].[Username] LIKE '%'+@SearchText+'%' OR [UP].[FirstName] LIKE '%'+@SearchText+'%' OR [UP].[LastName] LIKE '%'+@SearchText+'%')
        
        LEFT JOIN [dbo].[FiscalYears] FY ON [U].[LastFiscalYearSelectedID] = [FY].[FiscalYearID]
    END 
END






