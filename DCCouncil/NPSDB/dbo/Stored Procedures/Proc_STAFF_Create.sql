CREATE PROCEDURE Proc_STAFF_Create
(
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
 INSERT INTO [dbo].[Staffs] ( [FirstName], [LastName], [HasStaffLevelExpenditures], [ActiveFrom], [ActiveTo], [OfficeID], [IsDeleted], [CreatedDate],WirelessNumber)
 VALUES
 (@FirstName,@LastName,@HasStaffLevelExpenditures,@ActiveFrom,@ActiveTo,@OfficeID,@IsDeleted,GETDATE(),@WirelessNumber)
 
END
