CREATE TABLE [dbo].[OfficeAttributeLookups] (
    [OfficeAttributeLookupID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                    NVARCHAR (500) NOT NULL,
    [Description]             NVARCHAR (MAX) NOT NULL,
    [CreatedDate]             DATETIME       NOT NULL,
    [UpdatedDate]             DATETIME       NULL,
    CONSTRAINT [PK_OfficeAttributeLookups] PRIMARY KEY CLUSTERED ([OfficeAttributeLookupID] ASC)
);

