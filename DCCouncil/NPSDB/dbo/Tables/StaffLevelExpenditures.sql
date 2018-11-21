CREATE TABLE [dbo].[StaffLevelExpenditures] (
    [StaffLevelExpenditureID] BIGINT     IDENTITY (1, 1) NOT NULL,
    [ExpenditureID]           BIGINT     NOT NULL,
    [StaffID]                 BIGINT     NOT NULL,
    [Amount]                  FLOAT (53) NOT NULL,
    [CreatedDate]             DATETIME   NOT NULL,
    [UpdatedDate]             DATETIME   NULL,
    CONSTRAINT [PK_StaffLevelExpenditures] PRIMARY KEY CLUSTERED ([StaffLevelExpenditureID] ASC),
    CONSTRAINT [FK_StaffLevelExpenditures_Expenditures] FOREIGN KEY ([ExpenditureID]) REFERENCES [dbo].[Expenditures] ([ExpenditureID]),
    CONSTRAINT [FK_StaffLevelExpenditures_Staffs] FOREIGN KEY ([StaffID]) REFERENCES [dbo].[Staffs] ([StaffID])
);

