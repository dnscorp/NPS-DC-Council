CREATE TABLE [dbo].[Staffs] (
    [StaffID]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName]                 NVARCHAR (250) NOT NULL,
    [LastName]                  NVARCHAR (250) NULL,
    [ActiveFrom]                DATETIME       NOT NULL,
    [ActiveTo]                  DATETIME       NULL,
    [OfficeID]                  BIGINT         NOT NULL,
    [HasStaffLevelExpenditures] BIT            NULL,
    [IsDeleted]                 BIT            CONSTRAINT [DF_Staffs_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]               DATETIME       NOT NULL,
    [UpdatedDate]               DATETIME       NULL,
    [WirelessNumber]            NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Staffs] PRIMARY KEY CLUSTERED ([StaffID] ASC),
    CONSTRAINT [FK_Staffs_Offices] FOREIGN KEY ([OfficeID]) REFERENCES [dbo].[Offices] ([OfficeID])
);

