CREATE TABLE [dbo].[PurchaseOrderDescriptions] (
    [DescriptionID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [PONumber]        NVARCHAR (50)  NOT NULL,
    [DescriptionText] NVARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [UpdatedDate]     DATETIME       NULL,
    CONSTRAINT [PK_PurchaseOrderDescriptions] PRIMARY KEY CLUSTERED ([DescriptionID] ASC)
);

