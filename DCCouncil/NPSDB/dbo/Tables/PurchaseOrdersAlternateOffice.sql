CREATE TABLE [dbo].[PurchaseOrdersAlternateOffice] (
    [POOfficeID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [PONumber]          NVARCHAR (50) NOT NULL,
    [AlternateOfficeID] BIGINT        NOT NULL,
    [CreatedDate]       DATETIME      NOT NULL,
    [UpdatedDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_PurchaseOrdersAlternateOffice] PRIMARY KEY CLUSTERED ([POOfficeID] ASC)
);

