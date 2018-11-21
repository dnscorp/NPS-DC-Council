CREATE PROCEDURE Proc_EXPENDITURECATEGORY_GetAll
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
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [EC].[Name] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [EC].[Name] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [EC].[Code] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [EC].[Code] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [EC].[IsStaffLevel] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [EC].[IsStaffLevel] END DESC,
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [EC].[IsFixed] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [EC].[IsFixed] END DESC,                                              
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [EC].[IsActive] END,
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [EC].[IsActive] END DESC,
                CASE WHEN @SortField = 10 AND @SortDirection = 0 THEN [EC].[CreatedDate] END,
                CASE WHEN @SortField = 10 AND @SortDirection = 1 THEN [EC].[CreatedDate] END DESC,          
                CASE WHEN @SortField = 11 AND @SortDirection = 0 THEN [EC].[UpdatedDate] END,
                CASE WHEN @SortField = 11 AND @SortDirection = 1 THEN [EC].[UpdatedDate] END DESC,    
                CASE WHEN @SortField = 12 AND @SortDirection = 0 THEN [EC].[IsMonthly] END,
                CASE WHEN @SortField = 12 AND @SortDirection = 1 THEN [EC].[IsMonthly] END DESC,    
                CASE WHEN @SortField = 13 AND @SortDirection = 0 THEN [EC].[IsVendorStaff] END,
                CASE WHEN @SortField = 13 AND @SortDirection = 1 THEN [EC].[IsVendorStaff] END DESC,    
                CASE WHEN @SortField = 14 AND @SortDirection = 0 THEN [EC].[IsSystemDefined] END,
                CASE WHEN @SortField = 14 AND @SortDirection = 1 THEN [EC].[IsSystemDefined] END DESC,    
                CASE WHEN @SortField = 15 AND @SortDirection = 0 THEN [EC].[AppendMonth] END,
                CASE WHEN @SortField = 15 AND @SortDirection = 1 THEN [EC].[AppendMonth] END DESC,    
                CASE WHEN @SortField = 16 AND @SortDirection = 0 THEN [EC].[IsVendorStaffAndOther] END,
                CASE WHEN @SortField = 16 AND @SortDirection = 1 THEN [EC].[IsVendorStaffAndOther] END DESC    
                ) AS RowNumber,
                [EC].[ExpenditureCategoryID],
                [EC].[Name],
                [EC].[Code],
                [EC].[IsStaffLevel],
                [EC].[IsFixed],                
                [EC].[IsActive],
                [EC].[IsMonthly],
                [EC].[IsVendorStaff],
                [EC].[IsVendorStaffAndOther],
                [EC].[IsSystemDefined],
                [EC].[AppendMonth],                
                [EC].[CreatedDate],
                [EC].[UpdatedDate]
            FROM [dbo].[ExpenditureCategories] EC            
            WHERE [EC].[IsDeleted]=0
            AND             
            (LEN(@SearchText) = 0 OR @SearchText IS NULL 
            OR [EC].[Name] LIKE '%'+@SearchText+'%'
            OR [EC].[Code] LIKE '%'+@SearchText+'%')
        )   
        SELECT  
            [ExpenditureCategoryID]
            ,[Name]
            ,[Code]
            ,[IsStaffLevel]
            ,[IsFixed]            
            ,[IsActive]
            ,[IsMonthly]   
            ,[IsVendorStaff]
            ,[IsSystemDefined]
            ,[AppendMonth]
            ,[IsVendorStaffAndOther]
            ,[CreatedDate]
            ,[UpdatedDate]               
        FROM CTEUsers
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;
        
        WITH CTEUsers AS
        (
            SELECT
                ROW_NUMBER() OVER(ORDER BY 
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [EC].[Name] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [EC].[Name] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [EC].[Code] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [EC].[Code] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [EC].[IsStaffLevel] END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN [EC].[IsStaffLevel] END DESC,                
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [EC].[IsFixed] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [EC].[IsFixed] END DESC,                
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [EC].[IsActive] END,
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [EC].[IsActive] END DESC,
                CASE WHEN @SortField = 10 AND @SortDirection = 0 THEN [EC].[CreatedDate] END,
                CASE WHEN @SortField = 10 AND @SortDirection = 1 THEN [EC].[CreatedDate] END DESC,         
                CASE WHEN @SortField = 11 AND @SortDirection = 0 THEN [EC].[UpdatedDate] END,
                CASE WHEN @SortField = 11 AND @SortDirection = 1 THEN [EC].[UpdatedDate] END DESC,          
                CASE WHEN @SortField = 12 AND @SortDirection = 0 THEN [EC].[IsMonthly] END,
                CASE WHEN @SortField = 12 AND @SortDirection = 1 THEN [EC].[IsMonthly] END DESC,    
                CASE WHEN @SortField = 13 AND @SortDirection = 0 THEN [EC].[IsVendorStaff] END,
                CASE WHEN @SortField = 13 AND @SortDirection = 1 THEN [EC].[IsVendorStaff] END DESC,    
                CASE WHEN @SortField = 14 AND @SortDirection = 0 THEN [EC].[IsSystemDefined] END,
                CASE WHEN @SortField = 14 AND @SortDirection = 1 THEN [EC].[IsSystemDefined] END DESC,    
                CASE WHEN @SortField = 15 AND @SortDirection = 0 THEN [EC].[AppendMonth] END,
                CASE WHEN @SortField = 15 AND @SortDirection = 1 THEN [EC].[AppendMonth] END DESC,    
                CASE WHEN @SortField = 16 AND @SortDirection = 0 THEN [EC].[IsVendorStaffAndOther] END,
                CASE WHEN @SortField = 16 AND @SortDirection = 1 THEN [EC].[IsVendorStaffAndOther] END DESC    
                ) AS RowNumber,
                [EC].[ExpenditureCategoryID],
                [EC].[Name],
                [EC].[Code],
                [EC].[IsStaffLevel],
                [EC].[IsFixed],                
                [EC].[IsActive],
                EC.[IsMonthly],
                [EC].[IsVendorStaff],
                [EC].[IsSystemDefined],
                [EC].[IsVendorStaffAndOther],
                [EC].[AppendMonth],
                [EC].[CreatedDate],
                [EC].[UpdatedDate]
            FROM [dbo].[ExpenditureCategories] EC
            WHERE [EC].[IsDeleted]= 0
            AND             
            (LEN(@SearchText) = 0 OR @SearchText IS NULL 
            OR [EC].[Name] LIKE '%'+@SearchText+'%'
            OR [EC].[Code] LIKE '%'+@SearchText+'%')
        )
        SELECT MAX(RowNumber) AS TotalRowCount
        FROM CTEUsers 
    END
ELSE
    BEGIN
        SELECT
            [EC].[ExpenditureCategoryID],
            [EC].[Name],
            [EC].[Code],
            [EC].[IsStaffLevel],
            [EC].[IsFixed],            
            [EC].[IsActive],
            [EC].[IsMonthly],
            [EC].[IsVendorStaff],
            [EC].[IsVendorStaffAndOther],
            [EC].[IsSystemDefined],
            [EC].[AppendMonth],
            [EC].[CreatedDate],
            [EC].[UpdatedDate]
        FROM [dbo].[ExpenditureCategories] EC
        WHERE [EC].IsDeleted = 0
        AND             
        (LEN(@SearchText) = 0 OR @SearchText IS NULL 
        OR [EC].[Name] LIKE '%'+@SearchText+'%'
        OR [EC].[Code] LIKE '%'+@SearchText+'%')
    END 
END
