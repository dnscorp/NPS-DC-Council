CREATE TABLE [dbo].[ExpenditureCategoryAttributeLookups] (
    [ExpenditureCategoryAttributeLookupID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                                 NVARCHAR (500) NOT NULL,
    [Description]                          NVARCHAR (MAX) NOT NULL,
    [CreatedDate]                          DATETIME       NOT NULL,
    [UpdatedDate]                          DATETIME       NULL,
    CONSTRAINT [PK_ExpenditureCategoryAttributeLookups] PRIMARY KEY CLUSTERED ([ExpenditureCategoryAttributeLookupID] ASC)
);

