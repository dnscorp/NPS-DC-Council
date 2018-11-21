CREATE TABLE [dbo].[ExpenditureCategories] (
    [ExpenditureCategoryID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (250) NOT NULL,
    [Code]                  NVARCHAR (50)  NOT NULL,
    [IsStaffLevel]          BIT            CONSTRAINT [DF_ExpenditureCategory_IsStaffLevel] DEFAULT ((0)) NOT NULL,
    [IsFixed]               BIT            NOT NULL,
    [IsMonthly]             BIT            NOT NULL,
    [IsVendorStaff]         BIT            NOT NULL,
    [IsSystemDefined]       BIT            NOT NULL,
    [AppendMonth]           BIT            NOT NULL,
    [IsVendorStaffAndOther] BIT            NOT NULL,
    [IsActive]              BIT            NOT NULL,
    [IsDeleted]             BIT            CONSTRAINT [DF_ExpenditureCategory_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]           DATETIME       NOT NULL,
    [UpdatedDate]           DATETIME       NULL,
    CONSTRAINT [PK_ExpenditureCategory] PRIMARY KEY CLUSTERED ([ExpenditureCategoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Is the category be captured at a staff level or the office level', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExpenditureCategories', @level2type = N'COLUMN', @level2name = N'IsStaffLevel';

