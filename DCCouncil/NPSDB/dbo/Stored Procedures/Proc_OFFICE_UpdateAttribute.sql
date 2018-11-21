CREATE PROCEDURE [dbo].[Proc_OFFICE_UpdateAttribute]
@OfficeId [BIGINT], @AttributeLookupId [BIGINT], @AttributeValue NVARCHAR (250)
AS
BEGIN
    UPDATE [dbo].[OfficeAttributes]
    SET    [AttributeValue] = @AttributeValue,
           [UpdatedDate] = GETDATE()
    WHERE  [OfficeID] = @OfficeId AND [OfficeAttributeLookupID] = @AttributeLookupId;
END

