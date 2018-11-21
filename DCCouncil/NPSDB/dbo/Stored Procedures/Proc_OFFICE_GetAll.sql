
CREATE PROCEDURE Proc_OFFICE_GetAll   
(        
    @SearchText nvarchar(MAX),        
    @FiscalYearID [bigint],        
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
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [O].[Name] END,        
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [O].[Name] END DESC,        
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [O].[ActiveFrom] END,        
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [O].[ActiveFrom] END DESC,        
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN O.[ActiveTo] END,        
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN O.[ActiveTo] END DESC,        
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN O.[PCA] END,        
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN O.[PCA] END DESC,                        
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN O.[PCATitle] END,        
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN O.[PCATitle] END DESC,                        
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN O.[IndexCode] END,        
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN O.[IndexCode] END DESC,                        
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN O.[IndexTitle] END,        
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN O.[IndexTitle] END DESC,                                        
                CASE WHEN @SortField = 7 AND @SortDirection = 0 THEN [O].[CreatedDate] END,        
                CASE WHEN @SortField = 7 AND @SortDirection = 1 THEN [O].[CreatedDate] END DESC,        
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [O].[UpdatedDate] END,        
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [O].[UpdatedDate] END DESC                        
                ) AS RowNumber,        
                [O].[OfficeID],        
                [O].[Name],        
                [O].[ActiveFrom],        
                [O].[ActiveTo],        
                [O].[IndexCode],        
                [O].[IndexTitle],        
                [O].[PCA],        
                [O].[PCATitle],        
                [O].[IsDeleted],        
                [O].[CreatedDate],        
                [O].[UpdatedDate],
                [O].CompCode
            FROM [dbo].[Offices] O                    
            WHERE [O].[IsDeleted]=0        
            AND ([dbo].[udf_CheckIfOfficeActiveInFiscalYear]([O].[OfficeID],@FiscalYearID)=1 OR @FiscalYearID IS NULL)        
            AND                     
            (LEN(@SearchText) = 0 OR @SearchText IS NULL         
            OR [O].[Name] LIKE '%'+@SearchText+'%'        
            OR [O].[PCA] LIKE '%'+@SearchText+'%'        
            OR [O].[PCATitle] LIKE '%'+@SearchText+'%'        
            OR [O].[IndexCode] LIKE '%'+@SearchText+'%'        
            OR [O].[IndexTitle] LIKE '%'+@SearchText+'%')        
        )           
        SELECT          
            [OfficeID],        
            [Name],        
            [ActiveFrom],        
            [ActiveTo],        
            [IndexCode],        
            [IndexTitle],        
            [PCA],        
            [PCATitle],        
            [CreatedDate],        
            [UpdatedDate] ,
           CompCode                       
        FROM CTEUsers        
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;        
                
        WITH CTEUsers AS        
        (        
            SELECT        
                ROW_NUMBER() OVER(ORDER BY         
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [O].[Name] END,        
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [O].[Name] END DESC,        
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [O].[ActiveFrom] END,        
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [O].[ActiveFrom] END DESC,        
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN O.[ActiveTo] END,        
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN O.[ActiveTo] END DESC,        
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN O.[PCA] END,        
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN O.[PCA] END DESC,                        
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN O.[PCATitle] END,        
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN O.[PCATitle] END DESC,                        
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN O.[IndexCode] END,        
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN O.[IndexCode] END DESC,                        
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN O.[IndexTitle] END,        
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN O.[IndexTitle] END DESC,                                        
                CASE WHEN @SortField = 7 AND @SortDirection = 0 THEN [O].[CreatedDate] END,        
                CASE WHEN @SortField = 7 AND @SortDirection = 1 THEN [O].[CreatedDate] END DESC,        
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [O].[UpdatedDate] END,        
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [O].[UpdatedDate] END DESC                        
                ) AS RowNumber,        
                [O].[OfficeID],        
                [O].[Name],        
                [O].[ActiveFrom],        
                [O].[ActiveTo],        
                [O].[IndexCode],        
                [O].[IndexTitle],        
                [O].[PCA],        
                [O].[PCATitle],        
                [O].[IsDeleted],        
                [O].[CreatedDate],        
                [O].[UpdatedDate],
                [O].CompCode          
            FROM [dbo].[Offices] O        
            WHERE [O].[IsDeleted]=0        
            AND ([dbo].[udf_CheckIfOfficeActiveInFiscalYear]([O].[OfficeID],@FiscalYearID)=1 OR @FiscalYearID IS NULL)        
            AND                     
            (LEN(@SearchText) = 0 OR @SearchText IS NULL         
            OR [O].[Name] LIKE '%'+@SearchText+'%'        
            OR [O].[PCA] LIKE '%'+@SearchText+'%'        
            OR [O].[PCATitle] LIKE '%'+@SearchText+'%'        
            OR [O].[IndexCode] LIKE '%'+@SearchText+'%'        
            OR [O].[IndexTitle] LIKE '%'+@SearchText+'%')        
        )        
        SELECT MAX(RowNumber) AS TotalRowCount        
        FROM CTEUsers         
    END        
ELSE        
    BEGIN        
       
        SELECT        
            [O].[OfficeID],        
            [O].[Name],        
            [O].[ActiveFrom],        
            [O].[ActiveTo],        
            [O].[IndexCode],        
            [O].[IndexTitle],        
            [O].[PCA],        
            [O].[PCATitle],        
            [O].[IsDeleted],        
            [O].[CreatedDate],         
            [O].[UpdatedDate],
            [O].CompCode          
        FROM [dbo].[Offices] O        
        WHERE O.IsDeleted = 0        
        AND ([dbo].[udf_CheckIfOfficeActiveInFiscalYear]([O].[OfficeID],@FiscalYearID)=1 OR @FiscalYearID IS NULL)        
        AND      
        (LEN(@SearchText) = 0 OR @SearchText IS NULL         
        OR [O].[Name] LIKE '%'+@SearchText+'%'        
        OR [O].[PCA] LIKE '%'+@SearchText+'%'        
        OR [O].[PCATitle] LIKE '%'+@SearchText+'%'        
        OR [O].[IndexCode] LIKE '%'+@SearchText+'%'        
OR [O].[IndexTitle] LIKE '%'+@SearchText+'%')   
ORDER BY O.Name        
    END         
END
