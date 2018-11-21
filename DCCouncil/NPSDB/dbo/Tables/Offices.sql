CREATE TABLE [dbo].[Offices] (
    [OfficeID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500) NOT NULL,
    [ActiveFrom]  DATETIME       NOT NULL,
    [ActiveTo]    DATETIME       NULL,
    [PCA]         NVARCHAR (50)  NOT NULL,
    [PCATitle]    NVARCHAR (250) NULL,
    [IndexCode]   NVARCHAR (50)  NOT NULL,
    [IndexTitle]  NVARCHAR (250) NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_Offices_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate] DATETIME       NOT NULL,
    [UpdatedDate] DATETIME       NULL,
    [CompCode]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Offices] PRIMARY KEY CLUSTERED ([OfficeID] ASC)
);

