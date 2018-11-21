
CREATE PROCEDURE Proc_Vendor_GetById  
(  
    @VendorID bigint  
)  
AS  
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
    WHERE [V].[VendorID] = @VendorID  
    AND [V].[IsDeleted] = 0          
      
END
