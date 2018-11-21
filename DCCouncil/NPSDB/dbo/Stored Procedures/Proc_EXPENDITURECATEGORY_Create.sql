CREATE PROCEDURE Proc_EXPENDITURECATEGORY_Create
(
  @Name [nvarchar] (250),
  @Code [nvarchar] (50),
  @IsStaffLevel [bit] ,
  @IsFixed [bit] ,
  @IsActive [bit] ,
  @IsDeleted bit,
  @IsVendorStaff bit,
  @IsMonthly [bit],
  @AppendMonth [bit],
  @IsSystemDefined bit  
)
AS
BEGIN

INSERT INTO [dbo].[ExpenditureCategories] ([Name], [Code], [IsStaffLevel], [IsFixed], [IsActive], [IsDeleted], [CreatedDate], [UpdatedDate],[IsVendorStaff],[IsMonthly],[AppendMonth],[IsSystemDefined])
VALUES
(@Name,@Code,@IsStaffLevel,@IsFixed,@IsActive,@IsDeleted,GETDATE(),NULL,@IsVendorStaff,@IsMonthly,@AppendMonth,@IsSystemDefined)

DECLARE @ExpenditureCategoryID [bigint]
SET @ExpenditureCategoryID = @@IDENTITY

INSERT INTO [dbo].[ExpenditureCategoryAttributes] ([ExpenditureCategoryAttributeLookupID], [ExpenditureCategoryID], [AttributeValue], [CreatedDate], [UpdatedDate])
SELECT 
    [ECAL].[ExpenditureCategoryAttributeLookupID],@ExpenditureCategoryID,'',GETDATE(),null
FROM [dbo].[ExpenditureCategoryAttributeLookups] ECAL

END
