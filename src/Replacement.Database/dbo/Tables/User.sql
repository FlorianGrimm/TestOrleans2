CREATE TABLE [dbo].[User] (
    [Id]            UNIQUEIDENTIFIER   NOT NULL,
    [UserName]      NVARCHAR (50)      NOT NULL,
    [OperationId]    UNIQUEIDENTIFIER   NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [ModifiedAt]    DATETIMEOFFSET (7) NOT NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo_User_dbo_Operation] FOREIGN KEY ([ModifiedAt], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [Id])
);


GO

CREATE UNIQUE INDEX [UX_dbo_User_UserName] ON [dbo].[User] ([UserName]) INCLUDE ([Id])
