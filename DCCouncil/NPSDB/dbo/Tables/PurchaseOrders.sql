CREATE TABLE [dbo].[PurchaseOrders] (
    [PurchaseOrderID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [VendorName]        NVARCHAR (500) NOT NULL,
    [OBJCode]           NVARCHAR (50)  NOT NULL,
    [DateOfTransaction] DATETIME       NOT NULL,
    [PONumber]          NVARCHAR (50)  NOT NULL,
    [POAmtSum]          FLOAT (53)     NOT NULL,
    [POAdjAmtSum]       FLOAT (53)     NOT NULL,
    [VoucherAmtSum]     FLOAT (53)     NOT NULL,
    [POBalSum]          FLOAT (53)     NOT NULL,
    [OfficeID]          BIGINT         NOT NULL,
    [FiscalYearID]      BIGINT         NOT NULL,
    [BudgetID]          BIGINT         NOT NULL,
    [IsDeleted]         BIT            NOT NULL,
    [ImportID]          BIGINT         NULL,
    [CreatedDate]       DATETIME       NOT NULL,
    [UpdatedDate]       DATETIME       NULL,
    CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY CLUSTERED ([PurchaseOrderID] ASC),
    CONSTRAINT [FK_PurchaseOrders_Budgets] FOREIGN KEY ([BudgetID]) REFERENCES [dbo].[Budgets] ([BudgetID]),
    CONSTRAINT [FK_PurchaseOrders_FiscalYears] FOREIGN KEY ([FiscalYearID]) REFERENCES [dbo].[FiscalYears] ([FiscalYearID]),
    CONSTRAINT [FK_PurchaseOrders_Offices] FOREIGN KEY ([OfficeID]) REFERENCES [dbo].[Offices] ([OfficeID]),
    CONSTRAINT [FK_PurchaseOrders_PurchaseOrderImports] FOREIGN KEY ([ImportID]) REFERENCES [dbo].[PurchaseOrderImports] ([ImportID])
);

