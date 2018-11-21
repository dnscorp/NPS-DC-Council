CREATE PROCEDURE Proc_EXPENDITURECATEGORYATTRIBUTE_GetByExpenditureCategoryID
(
    @ExpenditureCategoryID [bigint]
)
AS
BEGIN
    SELECT 
        [ECA].[ExpenditureCategoryAttributeID],
        [ECA].[ExpenditureCategoryAttributeLookupID],
        [ECA].[ExpenditureCategoryID],
        [ECA].[AttributeValue],
        [ECA].[CreatedDate],
        [ECA].[UpdatedDate],
        [ECAL].[ExpenditureCategoryAttributeLookupID],
        [ECAL].[Name] AS ExpenditureCategoryAttributeLookupName,
        [ECAL].[Description] AS ExpenditureCategoryAttributeLookupDescription,
        [ECAL].[CreatedDate] AS ExpenditureCategoryAttributeLookupCreatedDate,
        [ECAL].[UpdatedDate] AS ExpenditureCategoryAttributeLookupUpdatedDate
    FROM [dbo].[ExpenditureCategoryAttributes] ECA
    INNER JOIN [dbo].[ExpenditureCategoryAttributeLookups] ECAL ON [ECA].[ExpenditureCategoryAttributeLookupID] = [ECAL].[ExpenditureCategoryAttributeLookupID]
    WHERE [ECA].[ExpenditureCategoryID] = @ExpenditureCategoryID
END
