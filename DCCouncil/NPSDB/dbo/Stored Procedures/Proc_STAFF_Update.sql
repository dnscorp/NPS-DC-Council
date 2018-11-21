CREATE PROCEDURE Proc_STAFF_Update
(
  @StaffId bigint,
  @FirstName nvarchar(250) ,
  @LastName nvarchar(250) ,
  @HasStaffLevelExpenditures bit ,
  @ActiveFrom datetime ,
  @ActiveTo datetime ,
  @OfficeID bigint ,
  @IsDeleted bit,
  @WirelessNumber nvarchar(50) 
)

AS
BEGIN
UPDATE [dbo].[Staffs] SET

 [FirstName] = @FirstName,
 [LastName]=@LastName,
 [HasStaffLevelExpenditures] = @HasStaffLevelExpenditures,
 [ActiveFrom] = @ActiveFrom,
 [ActiveTo] = @ActiveTo,
 OfficeID = @OfficeID,
 [IsDeleted]= @IsDeleted,
 [UpdatedDate] = GETDATE(),
 WirelessNumber = @WirelessNumber

 WHERE [StaffID] = @StaffId

END
