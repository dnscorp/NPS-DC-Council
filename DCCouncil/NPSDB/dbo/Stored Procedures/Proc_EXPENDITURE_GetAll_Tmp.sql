CREATE PROCEDURE [dbo].[Proc_EXPENDITURE_GetAll_Tmp]
(
    @SearchText nvarchar(MAX),
    @OfficeIds xml,
    @AsOfDate [datetime],
    @FiscalYearId [bigint],
    @ExpenditureCategoryIds [xml],
    @PageSize [int] = -1,
    @PageNumber [int] = 0,
    @SortField [int] = 0,
    @SortDirection [int] = 0    
)
AS
BEGIN

    --TEST Values
    --    DECLARE @SearchText nvarchar(MAX)
    --    DECLARE @OfficeIds XML
    --    DECLARE @AsOfDate [datetime]
    --    DECLARE @FiscalYearId [bigint]
    --    DECLARE @ExpenditureCategoryIds [xml]
    --    DECLARE @PageSize [int] = -1
    --    DECLARE @PageNumber [int] = 0
    --    DECLARE @SortField [int] = 0
    --    DECLARE @SortDirection [int] = 0 
    --    SET	@SearchText = NULL
    --	SET	@OfficeIds = N'<officeids><officeid>6</officeid></officeids>'
    --	SET	@AsOfDate = N'09/15/2013'
    --	SET	@FiscalYearId = NULL
    --	SET	@ExpenditureCategoryIds = NULL
    --	SET	@PageSize = -1
    --	SET	@PageNumber = NULL
    --	SET	@SortField = 0
    --	SET	@SortDirection = 0
    --Test Values END
    
    Declare @XmlDataHandle int
	Exec sp_xml_preparedocument @XmlDataHandle OUTPUT,@OfficeIds        
    
    SELECT officeid
	INTO #TmpOfficeIds
	FROM OpenXml(@XmlDataHandle,'/officeids/officeid',2)
	WITH
	(
	    officeid bigint	'text()'	
	)
    
    Exec sp_xml_preparedocument @XmlDataHandle OUTPUT,@ExpenditureCategoryIds        
    
    SELECT expenditurecategoryid
	INTO #TmpExpenditureCategoryIds
	FROM OpenXml(@XmlDataHandle,'/expenditurecategoryids/expenditurecategoryid',2)
	WITH
	(
	    expenditurecategoryid bigint 'text()'	
	)
    
    
    IF @PageSize <> -1
    BEGIN
        DECLARE @intStartRow int;
        DECLARE @intEndRow int;
        
        SET @intStartRow = (@PageNumber -1) * @PageSize + 1;
        SET @intEndRow = @PageNumber * @PageSize;    
        
        WITH CTEExpenditureCategory AS
        (
            SELECT
                ROW_NUMBER() OVER(ORDER BY 
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [E].[DateOfTransaction] END,
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [E].[DateOfTransaction] END DESC,
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [E].[VendorName] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [E].[VendorName] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [E].[Description] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [E].[Description] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [E].OBJCode END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN E.OBJCode END DESC,
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN O.[IndexCode] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN O.[IndexCode] END DESC,
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [O].[PCA] END,
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [O].[PCA] END DESC,
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN [E].[Amount] END,
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN [E].[Amount] END DESC,                
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [E].[Comments] END,
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [E].[Comments] END DESC
                ) AS RowNumber,                   
        		[E].[Amount],
                [E].[BudgetID],
                [E].[OfficeID],
                [E].[VendorName],
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
                [B].[Amount] AS BudgetAmount,
                [B].[CreatedDate] AS BudgetCreatedDate,
                [B].[IsDefault] AS BudgetIsDefault,
                [B].[IsDeleted] AS BudgetIsDeleted,
                [B].[Name] AS BudgetName,
                [B].[UpdatedDate] AS BudgetUpdatedDate,
                [E].[FiscalYearID],
                [FY].[CreatedDate] AS FiscalYearCreatedDate,
                [FY].[Name] AS FiscalYearName,
                [FY].[StartDate] AS FiscalYearStartDate,
                [FY].[EndDate] AS FiscalYearEndDate,
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
                [FY].[Year] AS FiscalYearYear,                
                [E].[Comments],
                [E].[CreatedDate],
                [E].[DateOfTransaction],
                [E].[Description],
                [E].[ExpenditureCategoryID],
                [EC].[Code] AS ExpenditureCategoryCode,
                [EC].[CreatedDate] AS ExpenditureCategoryCreatedDate,
                [EC].[IsActive] AS ExpenditureCategoryIsActive,
                [EC].[IsDeleted] AS ExpenditureCategoryIsDeleted,
                [EC].[IsStaffLevel] AS ExpenditureCategoryIsStaffLevel,
                [EC].[Name] AS ExpenditureCategoryName,
                [EC].[UpdatedDate] AS ExpenditureCategoryUpdatedDate,
                [EC].[IsFixed] AS ExpenditureCategoryIsFixed,                
                [E].[ExpenditureID],
                [E].[IsDeleted],
                [E].[OBJCode],
                [E].[UpdatedDate]
            FROM [dbo].[Expenditures] E
            INNER JOIN [dbo].[Offices] O ON [E].[OfficeID] = [O].[OfficeID]            
            INNER JOIN [dbo].[Budgets] B ON [E].[BudgetID] = [B].[BudgetID]
            INNER JOIN [dbo].[FiscalYears] FY ON [E].[FiscalYearID] = [FY].[FiscalYearID]
            INNER JOIN [dbo].[ExpenditureCategories] EC ON [E].[ExpenditureCategoryID] = [EC].[ExpenditureCategoryID]            
            WHERE 
            ([E].[ExpenditureCategoryID] IN (SELECT expenditurecategoryid FROM #TmpExpenditureCategoryIds) OR @ExpenditureCategoryIds IS NULL)
            AND 
            ([E].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)
            AND
            ([O].[OfficeID] IN (SELECT officeid FROM #TmpOfficeIds) OR @OfficeIds IS NULL)
            AND
            ([E].[DateOfTransaction] <= @AsOfDate OR @AsOfDate IS NULL)
            AND
            (LEN(@SearchText) = 0 OR (@SearchText IS NULL) OR
            ([E].[VendorName] LIKE '%'+@SearchText+'%' OR [E].[VendorName] LIKE '%'+@SearchText+'%' OR [E].[Description] LIKE '%'+@SearchText+'%' OR [E].[OBJCode] LIKE '%'+@SearchText+'%' OR [E].[Comments] LIKE '%'+@SearchText+'%'
            OR [O].[IndexCode] LIKE '%' + @SearchText +'%' OR [O].[PCA] LIKE '%' +@SearchText +'%')
            )
            AND [E].[IsDeleted] = 0
        )            
        SELECT 
            [Amount],
            [BudgetID],
            [OfficeID],
            [VendorName],
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
            BudgetAmount,
            BudgetCreatedDate,
            BudgetIsDefault,
            BudgetIsDeleted,
            BudgetName,
            BudgetUpdatedDate,
            [FiscalYearID],
            FiscalYearCreatedDate,
            FiscalYearName,
            FiscalYearStartDate,
            FiscalYearEndDate,
            FiscalYearUpdatedDate,
            FiscalYearYear,                
            [Comments],
            [CreatedDate],
            [DateOfTransaction],
            [Description],
            [ExpenditureCategoryID],
            ExpenditureCategoryCode,
            ExpenditureCategoryCreatedDate,
            ExpenditureCategoryIsActive,
            ExpenditureCategoryIsDeleted,
            ExpenditureCategoryIsStaffLevel,
            ExpenditureCategoryName,
            ExpenditureCategoryUpdatedDate,
            ExpenditureCategoryIsFixed,            
            [ExpenditureID],
            [IsDeleted],
            [OBJCode],
            [UpdatedDate]
        FROM CTEExpenditureCategory
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;
        
        WITH CTEExpenditureCategory AS
        (
            SELECT
                ROW_NUMBER() OVER(ORDER BY 
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [E].[DateOfTransaction] END,
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [E].[DateOfTransaction] END DESC,
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN [E].[VendorName] END,
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN [E].[VendorName] END DESC,
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN [E].[Description] END,
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN [E].[Description] END DESC,
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN [E].OBJCode END,
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN E.OBJCode END DESC,
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN O.[IndexCode] END,
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN O.[IndexCode] END DESC,
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [O].[PCA] END,
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [O].[PCA] END DESC,                    
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN [E].[Amount] END,
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN [E].[Amount] END DESC,                
                CASE WHEN @SortField = 8 AND @SortDirection = 0 THEN [E].[Comments] END,
                CASE WHEN @SortField = 8 AND @SortDirection = 1 THEN [E].[Comments] END DESC
                ) AS RowNumber,                   
        		[E].[Amount],
                [E].[BudgetID],
                [E].[OfficeID],
                [E].[VendorName],
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
                [B].[Amount] AS BudgetAmount,
                [B].[CreatedDate] AS BudgetCreatedDate,
                [B].[IsDefault] AS BudgetIsDefault,
                [B].[IsDeleted] AS BudgetIsDeleted,
                [B].[Name] AS BudgetName,
                [B].[UpdatedDate] AS BudgetUpdatedDate,
                [E].[FiscalYearID],
                [FY].[CreatedDate] AS FiscalYearCreatedDate,
                [FY].[Name] AS FiscalYearName,
                [FY].[StartDate] AS FiscalYearStartDate,
                [FY].[EndDate] AS FiscalYearEndDate,
                [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
                [FY].[Year] AS FiscalYearYear,                
                [E].[Comments],
                [E].[CreatedDate],
                [E].[DateOfTransaction],
                [E].[Description],
                [E].[ExpenditureCategoryID],
                [EC].[Code] AS ExpenditureCategoryCode,
                [EC].[CreatedDate] AS ExpenditureCategoryCreatedDate,
                [EC].[IsActive] AS ExpenditureCategoryIsActive,
                [EC].[IsDeleted] AS ExpenditureCategoryIsDeleted,
                [EC].[IsStaffLevel] AS ExpenditureCategoryIsStaffLevel,
                [EC].[Name] AS ExpenditureCategoryName,
                [EC].[UpdatedDate] AS ExpenditureCategoryUpdatedDate,
                [EC].[IsFixed] AS ExpenditureCategoryIsFixed,                                
                [E].[ExpenditureID],
                [E].[IsDeleted],
                [E].[OBJCode],
                [E].[UpdatedDate]
            FROM [dbo].[Expenditures] E
            INNER JOIN [dbo].[Offices] O ON [E].[OfficeID] = [O].[OfficeID]            
            INNER JOIN [dbo].[Budgets] B ON [E].[BudgetID] = [B].[BudgetID]
            INNER JOIN [dbo].[FiscalYears] FY ON [E].[FiscalYearID] = [FY].[FiscalYearID]
            INNER JOIN [dbo].[ExpenditureCategories] EC ON [E].[ExpenditureCategoryID] = [EC].[ExpenditureCategoryID]            
            WHERE            
            ([E].[ExpenditureCategoryID] IN (SELECT expenditurecategoryid FROM #TmpExpenditureCategoryIds) OR @ExpenditureCategoryIds IS NULL)
            AND 
            ([E].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)
            AND
            ([O].[OfficeID] IN (SELECT officeid FROM #TmpOfficeIds) OR @OfficeIds IS NULL)
            AND
            ([E].[DateOfTransaction] <= @AsOfDate OR @AsOfDate IS NULL)
            AND
            (LEN(@SearchText) = 0 OR (@SearchText IS NULL) OR
            ([E].[VendorName] LIKE '%'+@SearchText+'%' OR [E].[VendorName] LIKE '%'+@SearchText+'%' OR [E].[Description] LIKE '%'+@SearchText+'%' OR [E].[OBJCode] LIKE '%'+@SearchText+'%' OR [E].[Comments] LIKE '%'+@SearchText+'%'
            OR [O].[IndexCode] LIKE '%' + @SearchText +'%' OR [O].[PCA] LIKE '%' +@SearchText +'%')
            )
            AND [E].[IsDeleted] = 0
        )            
        SELECT MAX(RowNumber) AS TotalRowCount
        FROM CTEExpenditureCategory 
    END
    ELSE
    BEGIN
    	SELECT           
    		[E].[Amount],
            [E].[BudgetID],
            [E].[OfficeID],
            [E].[VendorName],
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
            [B].[Amount] AS BudgetAmount,
            [B].[CreatedDate] AS BudgetCreatedDate,
            [B].[IsDefault] AS BudgetIsDefault,
            [B].[IsDeleted] AS BudgetIsDeleted,
            [B].[Name] AS BudgetName,
            [B].[UpdatedDate] AS BudgetUpdatedDate,
            [E].[FiscalYearID],
            [FY].[CreatedDate] AS FiscalYearCreatedDate,
            [FY].[Name] AS FiscalYearName,
            [FY].[StartDate] AS FiscalYearStartDate,
            [FY].[EndDate] AS FiscalYearEndDate,
            [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
            [FY].[Year] AS FiscalYearYear,                
            [E].[Comments],
            [E].[CreatedDate],
            [E].[DateOfTransaction],
            [E].[Description],
            [E].[ExpenditureCategoryID],
            [EC].[Code] AS ExpenditureCategoryCode,
            [EC].[CreatedDate] AS ExpenditureCategoryCreatedDate,
            [EC].[IsActive] AS ExpenditureCategoryIsActive,
            [EC].[IsDeleted] AS ExpenditureCategoryIsDeleted,
            [EC].[IsStaffLevel] AS ExpenditureCategoryIsStaffLevel,
            [EC].[Name] AS ExpenditureCategoryName,
            [EC].[UpdatedDate] AS ExpenditureCategoryUpdatedDate,
            [EC].[IsFixed] AS ExpenditureCategoryIsFixed,                            
            [E].[ExpenditureID],
            [E].[IsDeleted],
            E.[OBJCode],
            [E].[UpdatedDate]
        FROM [dbo].[Expenditures] E
        INNER JOIN [dbo].[Offices] O ON [E].[OfficeID] = [O].[OfficeID]            
        INNER JOIN [dbo].[Budgets] B ON [E].[BudgetID] = [B].[BudgetID]
        INNER JOIN [dbo].[FiscalYears] FY ON [E].[FiscalYearID] = [FY].[FiscalYearID]
        INNER JOIN [dbo].[ExpenditureCategories] EC ON [E].[ExpenditureCategoryID] = [EC].[ExpenditureCategoryID]            
        WHERE 
        ([E].[ExpenditureCategoryID] IN (SELECT expenditurecategoryid FROM #TmpExpenditureCategoryIds) OR @ExpenditureCategoryIds IS NULL)
        AND 
        ([E].[FiscalYearID] = @FiscalYearId OR @FiscalYearId IS NULL)
        AND
        ([O].[OfficeID] IN (SELECT officeid FROM #TmpOfficeIds) OR @OfficeIds IS NULL)
        AND
        ([E].[DateOfTransaction] <= @AsOfDate OR @AsOfDate IS NULL)
        AND
        (LEN(@SearchText) = 0 OR (@SearchText IS NULL) OR
        ([E].[VendorName] LIKE '%'+@SearchText+'%' OR [E].[VendorName] LIKE '%'+@SearchText+'%' OR [E].[Description] LIKE '%'+@SearchText+'%' OR [E].[OBJCode] LIKE '%'+@SearchText+'%' OR [E].[Comments] LIKE '%'+@SearchText+'%'
        OR [O].[IndexCode] LIKE '%' + @SearchText +'%' OR [O].[PCA] LIKE '%' +@SearchText +'%'))
        AND [E].[IsDeleted] = 0
    END 
END

















