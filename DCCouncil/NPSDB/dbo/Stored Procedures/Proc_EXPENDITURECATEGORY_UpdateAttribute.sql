CREATE PROCEDURE [dbo].[Proc_EXPENDITURECATEGORY_UpdateAttribute]
@ExpenditureCategoryId [BIGINT], @ExpenditureCategoryAttributeLookupId [BIGINT], @AttributeValue NVARCHAR (250)
AS
BEGIN
    UPDATE [dbo].[ExpenditureCategoryAttributes]
    SET    [AttributeValue] = @AttributeValue,
           [UpdatedDate] = GETDATE()
    WHERE  [ExpenditureCategoryID] = @ExpenditureCategoryId AND [ExpenditureCategoryAttributeLookupID] = @ExpenditureCategoryAttributeLookupId;
END

