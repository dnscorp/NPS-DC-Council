CREATE TABLE [dbo].[UserProfiles] (
    [UserProfileID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserID]        BIGINT         NOT NULL,
    [FirstName]     NVARCHAR (250) NOT NULL,
    [LastName]      NVARCHAR (250) NULL,
    [CreatedDate]   DATETIME       NOT NULL,
    [UpdatedDate]   DATETIME       NULL,
    CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED ([UserProfileID] ASC),
    CONSTRAINT [FK_UserProfiles_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

