create PROCEDURE Proc_VENDOR_Update
(
    @VendorID [bigint],
    @OfficeID [bigint],
    @DefaultDescription nvarchar(MAX),
    @FiscalYearID [bigint],
    @Name nvarchar(MAX),
    @IsRolledUp [bit],
    @IsDeleted [bit]    
)
AS
BEGIN
    UPDATE [dbo].[Vendors]
    SET    
        [Name]= @Name,
        [DefaultDescription] = @DefaultDescription,
        [FiscalYearID] = @FiscalYearID,
        [IsDeleted]   = @IsDeleted,
        [UpdatedDate] = GETDATE(),
        [IsRolledUp] = @IsRolledUp,
        [OfficeID] = @OfficeId
    WHERE  
        [VendorID] = @VendorID
END
