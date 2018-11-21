CREATE PROCEDURE Proc_STAFF_GetAll  
(  
    @OfficeId bigint,  
    @blnOnlyStaffWithStaffLevelExpenditure bit,  
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
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [S].[FirstName] END,  
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [S].[FirstName] END DESC,  
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN S.[LastName] END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN S.[LastName] END DESC,  
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN S.[ActiveFrom] END,  
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN S.[ActiveFrom] END DESC,  
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN S.[ActiveTo] END,  
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN S.[ActiveTo] END DESC,  
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [S].[CreatedDate] END,  
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [S].[CreatedDate] END DESC,  
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [S].[UpdatedDate] END,  
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [S].[UpdatedDate] END DESC,  
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN [S].[HasStaffLevelExpenditures] END,  
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN [S].[HasStaffLevelExpenditures] END DESC                     
                ) AS RowNumber,                     
                [S].[StaffID],  
                [S].[FirstName],  
                [S].[LastName],  
                [S].[HasStaffLevelExpenditures],  
                [S].[ActiveTo],  
                [S].[ActiveFrom],  
                [S].[IsDeleted],  
                [S].[OfficeID] AS StaffOfficeId,  
                [S].[CreatedDate],  
                [S].[UpdatedDate], 
                [S].WirelessNumber, 
                O.[OfficeID] AS OfficeID,  
                [O].[Name] AS OfficeName,  
                O.[ActiveFrom] AS OfficeActiveFrom,  
                [O].[ActiveTo] AS OfficeActiveTo,  
                [O].[IndexCode] AS OfficeIndexCode,  
                [O].[IndexTitle] AS OfficeIndexTitle,  
                [O].[PCA] AS OfficePCA,  
                [O].[PCATitle] AS OfficePCATitle,  
                [O].[IsDeleted] AS OfficeIsDeleted,  
                [O].[CreatedDate] AS OfficeCreatedDate,  
                [O].[UpdatedDate] AS OfficeUpdatedDate,
                [O].CompCode AS OfficeCompCode
            FROM  [dbo].[Staffs] S   
            INNER JOIN [dbo].[Offices] O ON O.[OfficeID] = [S].[OfficeID]   
            WHERE [O].[OfficeID] = @OfficeId  AND   
            (@blnOnlyStaffWithStaffLevelExpenditure = 0 or (@blnOnlyStaffWithStaffLevelExpenditure = 1 AND [S].[HasStaffLevelExpenditures] = 1))  
            AND [S].[IsDeleted]=0  
        )   
        SELECT   
            [StaffID],  
            [FirstName],  
            [LastName],  
            [HasStaffLevelExpenditures],  
            [ActiveTo],  
            [ActiveFrom],  
            [IsDeleted],  
            StaffOfficeId,  
            [CreatedDate],  
            [UpdatedDate],
            WirelessNumber,       
            OfficeID,  
            OfficeName,  
            OfficeActiveFrom,  
            OfficeActiveTo,  
            OfficeIndexCode,  
            OfficeIndexTitle,  
            OfficePCA,  
            OfficePCATitle,  
            OfficeIsDeleted,  
            OfficeCreatedDate,  
            OfficeUpdatedDate,
            OfficeCompCode 
        FROM CTEUsers  
        WHERE [RowNumber] BETWEEN  @intStartRow AND @intEndRow;  
          
        WITH CTEUsers AS  
        (  
            SELECT  
                ROW_NUMBER() OVER(ORDER BY   
                CASE WHEN @SortField = 0 AND @SortDirection = 0 THEN [S].[FirstName] END,  
                CASE WHEN @SortField = 0 AND @SortDirection = 1 THEN [S].[FirstName] END DESC,  
                CASE WHEN @SortField = 1 AND @SortDirection = 0 THEN S.[LastName] END,  
                CASE WHEN @SortField = 1 AND @SortDirection = 1 THEN S.[LastName] END DESC,  
                CASE WHEN @SortField = 2 AND @SortDirection = 0 THEN S.[ActiveFrom] END,  
                CASE WHEN @SortField = 2 AND @SortDirection = 1 THEN S.[ActiveFrom] END DESC,  
                CASE WHEN @SortField = 3 AND @SortDirection = 0 THEN S.[ActiveTo] END,  
                CASE WHEN @SortField = 3 AND @SortDirection = 1 THEN S.[ActiveTo] END DESC,  
                CASE WHEN @SortField = 4 AND @SortDirection = 0 THEN [S].[CreatedDate] END,  
                CASE WHEN @SortField = 4 AND @SortDirection = 1 THEN [S].[CreatedDate] END DESC,  
                CASE WHEN @SortField = 5 AND @SortDirection = 0 THEN [S].[UpdatedDate] END,  
                CASE WHEN @SortField = 5 AND @SortDirection = 1 THEN [S].[UpdatedDate] END DESC,  
                CASE WHEN @SortField = 6 AND @SortDirection = 0 THEN [S].[HasStaffLevelExpenditures] END,  
                CASE WHEN @SortField = 6 AND @SortDirection = 1 THEN [S].[HasStaffLevelExpenditures] END DESC                        
                ) AS RowNumber,                     
                [S].[StaffID],  
                [S].[FirstName],  
                [S].[LastName],  
                [S].[HasStaffLevelExpenditures],  
                [S].[ActiveTo],  
                [S].[ActiveFrom],  
                [S].[IsDeleted],  
                [S].[OfficeID] AS StaffOfficeId,  
                [S].[CreatedDate],  
                [S].[UpdatedDate],
                [S].WirelessNumber,   
                O.[OfficeID] AS OfficeID,  
                [O].[Name] AS OfficeName,  
                O.[ActiveFrom] AS OfficeActiveFrom,  
                [O].[ActiveTo] AS OfficeActiveTo,  
                [O].[IndexCode] AS OfficeIndexCode,  
                [O].[IndexTitle] AS OfficeIndexTitle,  
                [O].[PCA] AS OfficePCA,  
                [O].[PCATitle] AS OfficePCATitle,  
                [O].[IsDeleted] AS OfficeIsDeleted,  
                [O].[CreatedDate] AS OfficeCreatedDate,  
                [O].[UpdatedDate] AS OfficeUpdatedDate,
                [O].CompCode AS OfficeCompCode                 
            FROM  [dbo].[Staffs] S   
            INNER JOIN [dbo].[Offices] O ON O.[OfficeID] = [S].[OfficeID]   
            WHERE [O].[OfficeID] = @OfficeId AND   
            (@blnOnlyStaffWithStaffLevelExpenditure = 0 or (@blnOnlyStaffWithStaffLevelExpenditure = 1 AND [S].[HasStaffLevelExpenditures] = 1))   
            AND [S].[IsDeleted]=0  
        )              
  
        SELECT MAX(RowNumber) AS TotalRowCount  
        FROM CTEUsers   
    END  
    ELSE  
    BEGIN  
        SELECT  
            [S].[StaffID],                  
            [S].[FirstName],  
            [S].[LastName],  
            [S].[HasStaffLevelExpenditures],  
            [S].[ActiveTo],  
            [S].[ActiveFrom],  
            [S].[IsDeleted],  
            [S].[OfficeID] AS StaffOfficeId,  
            [S].[CreatedDate],  
            [S].[UpdatedDate],
            [S].WirelessNumber,   
            O.[OfficeID] AS OfficeID,  
            [O].[Name] AS OfficeName,  
            O.[ActiveFrom] AS OfficeActiveFrom,  
            [O].[ActiveTo] AS OfficeActiveTo,  
            [O].[IndexCode] AS OfficeIndexCode,  
            [O].[IndexTitle] AS OfficeIndexTitle,  
            [O].[PCA] AS OfficePCA,  
            [O].[PCATitle] AS OfficePCATitle,  
            [O].[IsDeleted] AS OfficeIsDeleted,  
            [O].[CreatedDate] AS OfficeCreatedDate,  
            [O].[UpdatedDate] AS OfficeUpdatedDate,
            [O].CompCode AS OfficeCompCode 
        FROM  [dbo].[Staffs] S   
        INNER JOIN [dbo].[Offices] O ON O.[OfficeID] = [S].[OfficeID]   
        WHERE [O].[OfficeID] = @OfficeId AND   
            (@blnOnlyStaffWithStaffLevelExpenditure = 0 or (@blnOnlyStaffWithStaffLevelExpenditure = 1 AND [S].[HasStaffLevelExpenditures] = 1))  
        AND [S].[IsDeleted]=0  
    END   
END
