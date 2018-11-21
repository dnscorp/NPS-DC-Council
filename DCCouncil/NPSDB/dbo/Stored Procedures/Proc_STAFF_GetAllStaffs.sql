CREATE PROCEDURE Proc_STAFF_GetAllStaffs  

AS  
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
        O.[OfficeID],  
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
  WHERE S.IsDeleted = 0 AND O.IsDeleted=0  
  
  
END
