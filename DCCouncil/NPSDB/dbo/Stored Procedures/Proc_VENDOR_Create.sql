
create PROCEDURE Proc_VENDOR_Create 
(
@VendorName nvarchar(MAX),
@Description nvarchar(MAX),
@OfficeID bigint,
@FiscalYearId bigint,
@IsRolledUp bit,
@IsDeleted bit
)
AS
BEGIN
IF NOT EXISTS(SELECT Name from Vendors where Name = @VendorName AND OfficeID =@OfficeID)
 BEGIN

INSERT INTO VENDORS
 VALUES
 
(@VendorName,@Description,@OfficeID,@FiscalYearId,@IsRolledUp,@IsDeleted,GETDATE(),NULL)
 END

END
