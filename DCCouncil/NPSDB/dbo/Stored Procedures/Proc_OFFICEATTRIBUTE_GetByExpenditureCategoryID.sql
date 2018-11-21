CREATE PROCEDURE Proc_OFFICEATTRIBUTE_GetByExpenditureCategoryID
(
    @OfficeID [bigint]
)
AS
BEGIN
    SELECT 
        [OA].[AttributeValue],
        [OA].[CreatedDate],
        [OA].[OfficeAttributeID],
        [OA].[OfficeAttributeLookupID],
        [OAL].[CreatedDate] AS OfficeAttributeLookupCreatedDate,
        [OAL].[Name] AS OfficeAttributeLookupName,
        [OAL].[Description] AS OfficeAttributeLookupDescription,
        [OAL].[UpdatedDate] AS OfficeAttributeLookupUpdatedDate,
        [OA].[UpdatedDate]        
    FROM [dbo].[OfficeAttributes] OA
    INNER JOIN [dbo].[OfficeAttributeLookups] OAL ON [OA].[OfficeAttributeLookupID] = [OAL].[OfficeAttributeLookupID]
    WHERE [OA].[OfficeID] = @OfficeID
END
