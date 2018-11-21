CREATE TABLE [dbo].[PurchaseOrderImports] (
    [ImportID]    BIGINT           IDENTITY (1, 1) NOT NULL,
    [ImportGUID]  UNIQUEIDENTIFIER NOT NULL,
    [FileName]    NVARCHAR (500)   NOT NULL,
    [ImportFile]  VARBINARY (MAX)  NULL,
    [CreatedDate] DATETIME         NOT NULL,
    [UpdatedDate] DATETIME         NULL,
    CONSTRAINT [PK_PurchaseOrderImport] PRIMARY KEY CLUSTERED ([ImportID] ASC)
);

