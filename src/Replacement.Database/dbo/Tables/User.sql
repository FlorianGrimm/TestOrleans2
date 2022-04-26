CREATE TABLE [dbo].[User] (
    [UserId]        UNIQUEIDENTIFIER   NOT NULL,
    [UserName]      NVARCHAR (50)      NOT NULL,
    [OperationId]   UNIQUEIDENTIFIER   NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [CreatedBy]     UNIQUEIDENTIFIER   NULL,
    [ModifiedAt]    DATETIMEOFFSET (7) NOT NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER   NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_dbo_User_dbo_Operation] FOREIGN KEY ([ModifiedAt], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [OperationId])
);
GO
CREATE UNIQUE INDEX [UX_dbo_User_UserName] ON [dbo].[User] ([UserName]) INCLUDE ([UserId]);
GO