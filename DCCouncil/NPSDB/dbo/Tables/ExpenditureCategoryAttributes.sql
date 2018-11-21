CREATE TABLE [dbo].[ExpenditureCategoryAttributes] (
    [ExpenditureCategoryAttributeID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [ExpenditureCategoryAttributeLookupID] BIGINT         NOT NULL,
    [ExpenditureCategoryID]                BIGINT         NOT NULL,
    [AttributeValue]                       NVARCHAR (MAX) NOT NULL,
    [CreatedDate]                          DATETIME       NOT NULL,
    [UpdatedDate]                          DATETIME       NULL,
    CONSTRAINT [PK_ExpenditureCategoryAttributes] PRIMARY KEY CLUSTERED ([ExpenditureCategoryAttributeID] ASC),
    CONSTRAINT [FK_ExpenditureCategoryAttributes_ExpenditureCategories] FOREIGN KEY ([ExpenditureCategoryID]) REFERENCES [dbo].[ExpenditureCategories] ([ExpenditureCategoryID]),
    CONSTRAINT [FK_ExpenditureCategoryAttributes_ExpenditureCategoryAttributeLookups] FOREIGN KEY ([ExpenditureCategoryAttributeLookupID]) REFERENCES [dbo].[ExpenditureCategoryAttributeLookups] ([ExpenditureCategoryAttributeLookupID])
);

