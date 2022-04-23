CREATE TABLE [history].[UserHistory] (
    [OperationId]    UNIQUEIDENTIFIER   NOT NULL,
    [Id]            UNIQUEIDENTIFIER   NOT NULL,
    [UserName]      NVARCHAR (50)      NOT NULL,
    [ValidFrom]     DATETIMEOFFSET (7) NOT NULL,
    [ValidTo]       DATETIMEOFFSET (7) NOT NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_history_UserHistory] PRIMARY KEY CLUSTERED ([ValidTo] ASC, [ValidFrom] ASC, [OperationId] ASC, [Id] ASC),
    CONSTRAINT [FK_history_UserHistory_dbo_Operation] FOREIGN KEY ([ValidFrom], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [Id])
);

