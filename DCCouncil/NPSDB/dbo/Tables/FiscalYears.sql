CREATE TABLE [dbo].[FiscalYears] (
    [FiscalYearID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (500) NOT NULL,
    [Year]         INT            NOT NULL,
    [StartDate]    DATETIME       NOT NULL,
    [EndDate]      DATETIME       NOT NULL,
    [CreatedDate]  DATETIME       NOT NULL,
    [UpdatedDate]  DATETIME       NULL,
    CONSTRAINT [PK_FiscalYears] PRIMARY KEY CLUSTERED ([FiscalYearID] ASC),
    CONSTRAINT [UX_FiscalYears] UNIQUE NONCLUSTERED ([Year] ASC)
);

