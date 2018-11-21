CREATE TABLE [dbo].[OfficeAttributes] (
    [OfficeAttributeID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [OfficeAttributeLookupID] BIGINT         NOT NULL,
    [OfficeID]                BIGINT         NOT NULL,
    [AttributeValue]          NVARCHAR (MAX) NOT NULL,
    [CreatedDate]             DATETIME       NOT NULL,
    [UpdatedDate]             DATETIME       NULL,
    CONSTRAINT [PK_OfficeAttributes] PRIMARY KEY CLUSTERED ([OfficeAttributeID] ASC),
    CONSTRAINT [FK_OfficeAttributes_OfficeAttributeLookups] FOREIGN KEY ([OfficeAttributeLookupID]) REFERENCES [dbo].[OfficeAttributeLookups] ([OfficeAttributeLookupID]),
    CONSTRAINT [FK_OfficeAttributes_Offices] FOREIGN KEY ([OfficeID]) REFERENCES [dbo].[Offices] ([OfficeID])
);

