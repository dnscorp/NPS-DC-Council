CREATE TABLE [dbo].[Budgets] (
    [BudgetID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (250) NOT NULL,
    [IsDefault]    BIT            NOT NULL,
    [Amount]       FLOAT (53)     NOT NULL,
    [OfficeID]     BIGINT         NOT NULL,
    [FiscalYearID] BIGINT         NOT NULL,
    [IsDeleted]    BIT            CONSTRAINT [DF_Budgets_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]  DATETIME       NOT NULL,
    [UpdatedDate]  DATETIME       NULL,
    [IsDeduct]     BIT            CONSTRAINT [DF_Budgets_IsDeduct] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Budgets] PRIMARY KEY CLUSTERED ([BudgetID] ASC),
    CONSTRAINT [FK_Budgets_FiscalYears] FOREIGN KEY ([FiscalYearID]) REFERENCES [dbo].[FiscalYears] ([FiscalYearID]),
    CONSTRAINT [FK_Budgets_Offices] FOREIGN KEY ([OfficeID]) REFERENCES [dbo].[Offices] ([OfficeID])
);

