CREATE TABLE [dbo].[Users] (
    [UserID]                   BIGINT           IDENTITY (1, 1) NOT NULL,
    [UserGuid]                 UNIQUEIDENTIFIER NOT NULL,
    [Username]                 NVARCHAR (50)    NOT NULL,
    [PasswordHash]             NVARCHAR (MAX)   NOT NULL,
    [IsActive]                 BIT              CONSTRAINT [DF_Users_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDeleted]                BIT              CONSTRAINT [DF_Users_IsDeleted] DEFAULT ((0)) NOT NULL,
    [LastFiscalYearSelectedID] BIGINT           NULL,
    [CreatedDate]              DATETIME         NOT NULL,
    [UpdatedDate]              DATETIME         NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_Users_FiscalYears] FOREIGN KEY ([LastFiscalYearSelectedID]) REFERENCES [dbo].[FiscalYears] ([FiscalYearID]),
    CONSTRAINT [IX_Username] UNIQUE NONCLUSTERED ([Username] ASC),
    CONSTRAINT [UX_UserGuid] UNIQUE NONCLUSTERED ([UserGuid] ASC)
);

