CREATE PROCEDURE Proc_EXPENDITURECATEGORY_Update
@ExpenditureCategoryID [BIGINT], @Name NVARCHAR (250), @Code NVARCHAR (50), @IsStaffLevel bit, @IsFixed [bit],@IsActive [bit], @IsDeleted [bit], @IsVendorStaff [bit],@IsMonthly [bit], @AppendMonth [bit],@IsSystemDefined [bit]
AS
BEGIN
    UPDATE [dbo].[ExpenditureCategories]
       SET [Name] = @Name
          ,[Code] = @Code
          ,[IsStaffLevel] = @IsStaffLevel
          ,[IsFixed] = @IsFixed
          ,[IsActive] = @IsActive
          ,[IsDeleted] = @IsDeleted
          ,[UpdatedDate] = GETDATE()
          ,[IsVendorStaff] = @IsVendorStaff
          ,[IsMonthly] = @IsMonthly
          ,[AppendMonth] = @AppendMonth
          ,[IsSystemDefined] = @IsSystemDefined          
    WHERE  [ExpenditureCategoryID] = @ExpenditureCategoryID;
END
