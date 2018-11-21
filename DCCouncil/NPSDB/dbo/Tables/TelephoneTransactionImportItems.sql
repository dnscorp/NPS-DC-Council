CREATE TABLE [dbo].[TelephoneTransactionImportItems] (
    [ItemID]              INT              IDENTITY (1, 1) NOT NULL,
    [ImportGuid]          UNIQUEIDENTIFIER NOT NULL,
    [FoundationAccount]   NVARCHAR (250)   NULL,
    [BillingAccount]      NVARCHAR (250)   NULL,
    [WirelessNumber]      NVARCHAR (250)   NULL,
    [UserName]            NVARCHAR (250)   NOT NULL,
    [MarketCycleEndDate]  DATETIME         NULL,
    [TotalKBUsage]        NVARCHAR (150)   NULL,
    [TotalNumberofEvents] NVARCHAR (50)    NULL,
    [TotalMOUUsage]       NVARCHAR (250)   NULL,
    [TotalCurrentCharges] FLOAT (53)       NOT NULL,
    [ImportStatus]        INT              NOT NULL,
    [CreatedDate]         DATETIME         NOT NULL,
    [UpdatedDate]         DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([ItemID] ASC)
);

