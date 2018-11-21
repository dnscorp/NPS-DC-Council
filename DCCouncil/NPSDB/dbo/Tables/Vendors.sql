CREATE TABLE [dbo].[Vendors] (
    [VendorID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (MAX) NOT NULL,
    [DefaultDescription] NVARCHAR (MAX) NULL,
    [OfficeID]           BIGINT         NOT NULL,
    [FiscalYearID]       BIGINT         NOT NULL,
    [IsRolledUp]         BIT            NOT NULL,
    [IsDeleted]          BIT            NOT NULL,
    [CreatedDate]        DATETIME       NOT NULL,
    [UpdatedDate]        DATETIME       NULL,
    CONSTRAINT [PK_Vendors] PRIMARY KEY CLUSTERED ([VendorID] ASC),
    CONSTRAINT [FK_Vendors_FiscalYears] FOREIGN KEY ([FiscalYearID]) REFERENCES [dbo].[FiscalYears] ([FiscalYearID]),
    CONSTRAINT [FK_Vendors_Offices] FOREIGN KEY ([OfficeID]) REFERENCES [dbo].[Offices] ([OfficeID])
);

