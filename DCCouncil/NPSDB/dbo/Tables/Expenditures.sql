CREATE TABLE [dbo].[Expenditures] (
    [ExpenditureID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [ExpenditureCategoryID] BIGINT         NOT NULL,
    [VendorName]            NVARCHAR (500) NULL,
    [Description]           NVARCHAR (MAX) NULL,
    [OBJCode]               NVARCHAR (50)  NOT NULL,
    [DateOfTransaction]     DATETIME       NOT NULL,
    [Amount]                FLOAT (53)     NOT NULL,
    [OfficeID]              BIGINT         NOT NULL,
    [Comments]              NVARCHAR (MAX) NULL,
    [FiscalYearID]          BIGINT         NOT NULL,
    [BudgetID]              BIGINT         NOT NULL,
    [IsDeleted]             BIT            NOT NULL,
    [CreatedDate]           DATETIME       NOT NULL,
    [UpdatedDate]           DATETIME       NULL,
    CONSTRAINT [PK_Expenditures] PRIMARY KEY CLUSTERED ([ExpenditureID] ASC),
    CONSTRAINT [FK_Expenditures_Budgets] FOREIGN KEY ([BudgetID]) REFERENCES [dbo].[Budgets] ([BudgetID]),
    CONSTRAINT [FK_Expenditures_ExpenditureCategory] FOREIGN KEY ([ExpenditureCategoryID]) REFERENCES [dbo].[ExpenditureCategories] ([ExpenditureCategoryID]),
    CONSTRAINT [FK_Expenditures_FiscalYears] FOREIGN KEY ([FiscalYearID]) REFERENCES [dbo].[FiscalYears] ([FiscalYearID]),
    CONSTRAINT [FK_Expenditures_Offices] FOREIGN KEY ([OfficeID]) REFERENCES [dbo].[Offices] ([OfficeID])
);

