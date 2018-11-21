
CREATE PROCEDURE Proc_OFFICE_GetByOfficeId  
@OfficeId BIGINT  
AS  
BEGIN  
    SELECT [O].[OfficeID],  
           [O].[Name],  
           [O].[ActiveFrom],  
           [O].[ActiveTo],  
           [O].[IndexCode],  
           [O].[IndexTitle],  
           [O].[PCA],  
           [O].[PCATitle],  
           [O].[CreatedDate],  
           [O].[UpdatedDate],
           [O].CompCode
    FROM   [dbo].[Offices] AS O  
    WHERE  O.IsDeleted = 0  
    AND [O].[OfficeID] = @OfficeId  
END
