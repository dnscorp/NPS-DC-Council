﻿CREATE PROCEDURE Proc_EXPENDITURECATEGORY_GetByCode
(
    @Code nvarchar(50)
)
AS
BEGIN
    SELECT 
        [EC].[ExpenditureCategoryID],
        [EC].[Code],
        [EC].[CreatedDate],
        [EC].[IsActive],
        [EC].[IsDeleted],
        [EC].[IsFixed],
        [EC].[IsMonthly],
        [EC].[IsVendorStaff],
        [EC].[IsSystemDefined],
        [EC].[AppendMonth],
        [EC].[IsStaffLevel],
        [EC].[IsVendorStaffAndOther],
        [EC].[Name],
        [EC].[UpdatedDate]                
    FROM [dbo].[ExpenditureCategories] EC
    WHERE [EC].[Code] = @Code
    
END
