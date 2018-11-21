
CREATE PROCEDURE Proc_VENDOR_GetAll  
(  
    @SearchText nvarchar(MAX),  
    @OfficeIds XML,  
    @PageSize [int] = -1,  
    @PageNumber [int] = 0,  
    @SortField [int] = 0,  
    @SortDirection [int] = 0   
)  
AS  
BEGIN  
    Declare @XmlDataHandle int  
 Exec sp_xml_preparedocument @XmlDataHandle OUTPUT,@OfficeIds          
      
    SELECT officeid  
 INTO #TmpOfficeIds  
 FROM OpenXml(@XmlDataHandle,'/officeids/officeid',2)  
 WITH  
 (  
     officeid bigint 'text()'   
 )  
    IF @PageSize <> -1  
    BEGIN  
        DECLARE @intStartRow int;  
        DECLARE @intEndRow int;  
          
        SET @intStartRow = (@PageNumber -1) * @PageSize + 1;  
        SET @intEndRow = @PageNumber * @PageSize;      
        WITH CTEVendor AS  
        (  
            SELECT  
                ROW_NUMBER() OVER(ORDER BY   
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [V].[Name] END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [V].[Name] END DESC,  
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [O].[Name] END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [O].[Name]  END DESC,  
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [V].[IsRolledUp] END,  
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [V].[IsRolledUp] END DESC                  
                ) AS RowNumber,  
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
                [V].[CreatedDate],  
                [V].[DefaultDescription],  
                [V].[IsDeleted],  
                [V].[IsRolledUp],  
                [V].[Name],  
                [V].[OfficeID],  
                [V].[UpdatedDate],  
                [V].[VendorID],  
                [V].[FiscalYearID],  
                [FY].[CreatedDate] AS FiscalYearCreatedDate,  
                [FY].[Name] AS FiscalYearName,  
                [FY].[StartDate] AS FiscalYearStartDate,  
                [FY].[EndDate] AS FiscalYearEndDate,  
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,  
                [FY].[Year] AS FiscalYearYear                
            FROM [dbo].[Vendors] V  
            INNER JOIN [dbo].[Offices] O ON [V].[OfficeID] = [O].[OfficeID]                          
            INNER JOIN [dbo].[FiscalYears] FY ON [V].[FiscalYearID] = [FY].[FiscalYearID]  
            WHERE                           
            ([O].[OfficeID] IN (SELECT officeid FROM #TmpOfficeIds) OR @OfficeIds IS NULL)              
            AND  
            (LEN(@SearchText) = 0 OR (@SearchText IS NULL) OR  
            ([V].[Name] LIKE '%'+@SearchText+'%' OR [O].[Name] LIKE '%'+@SearchText+'%')  
            )  
            AND [V].[IsDeleted] = 0  
        )              
        SELECT   
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
            [CreatedDate],  
            [DefaultDescription],  
            [IsDeleted],  
            [IsRolledUp],  
            [Name],  
            [OfficeID],  
            [UpdatedDate],  
            [VendorID],  
            [FiscalYearID],  
            FiscalYearCreatedDate,  
            FiscalYearName,  
            FiscalYearStartDate,  
            FiscalYearEndDate,  
            FiscalYearUpdatedDate,  
            FiscalYearYear  
        FROM CTEVendor  
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;  
          
        WITH CTEVendor AS  
        (  
            SELECT  
                ROW_NUMBER() OVER(ORDER BY   
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [V].[Name] END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [V].[Name] END DESC,  
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [O].[Name] END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [O].[Name]  END DESC,  
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [V].[IsRolledUp] END,  
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [V].[IsRolledUp] END DESC                  
                ) AS RowNumber,  
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
                [V].[CreatedDate],  
                [V].[DefaultDescription],  
                [V].[IsDeleted],  
                [V].[IsRolledUp],  
                [V].[Name],  
                [V].[OfficeID],  
                [V].[UpdatedDate],  
                [V].[VendorID],  
                [V].[FiscalYearID],  
                [FY].[CreatedDate] AS FiscalYearCreatedDate,  
                [FY].[Name] AS FiscalYearName,  
                [FY].[StartDate] AS FiscalYearStartDate,  
                [FY].[EndDate] AS FiscalYearEndDate,  
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,  
                [FY].[Year] AS FiscalYearYear                
            FROM [dbo].[Vendors] V  
            INNER JOIN [dbo].[Offices] O ON [V].[OfficeID] = [O].[OfficeID]                          
            INNER JOIN [dbo].[FiscalYears] FY ON [V].[FiscalYearID] = [FY].[FiscalYearID]  
            WHERE                           
            ([O].[OfficeID] IN (SELECT officeid FROM #TmpOfficeIds) OR @OfficeIds IS NULL)              
            AND  
            (LEN(@SearchText) = 0 OR (@SearchText IS NULL) OR  
            ([V].[Name] LIKE '%'+@SearchText+'%' OR [O].[Name] LIKE '%'+@SearchText+'%')  
            )  
            AND [V].[IsDeleted] = 0  
        )              
        SELECT MAX(RowNumber) AS TotalRowCount  
        FROM CTEVendor   
    END  
    ELSE  
    BEGIN  
      SELECT                  
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
            [V].[CreatedDate],  
            [V].[DefaultDescription],  
            [V].[IsDeleted],  
            [V].[IsRolledUp],  
            [V].[Name],  
            [V].[OfficeID],  
            [V].[UpdatedDate],  
            [V].[VendorID],  
            [V].[FiscalYearID],  
            [FY].[CreatedDate] AS FiscalYearCreatedDate,  
            [FY].[Name] AS FiscalYearName,  
            [FY].[StartDate] AS FiscalYearStartDate,  
            [FY].[EndDate] AS FiscalYearEndDate,  
            [FY].[UpdatedDate] AS FiscalYearUpdatedDate,  
            [FY].[Year] AS FiscalYearYear   
        FROM [dbo].[Vendors] V  
        INNER JOIN [dbo].[Offices] O ON [V].[OfficeID] = [O].[OfficeID]                          
        INNER JOIN [dbo].[FiscalYears] FY ON [V].[FiscalYearID] = [FY].[FiscalYearID]  
        WHERE                           
        ([O].[OfficeID] IN (SELECT officeid FROM #TmpOfficeIds) OR @OfficeIds IS NULL)              
        AND  
        (LEN(@SearchText) = 0 OR (@SearchText IS NULL) OR  
        ([V].[Name] LIKE '%'+@SearchText+'%' OR [O].[Name] LIKE '%'+@SearchText+'%')  
        )  
        AND [V].[IsDeleted] = 0  
    END   
END
