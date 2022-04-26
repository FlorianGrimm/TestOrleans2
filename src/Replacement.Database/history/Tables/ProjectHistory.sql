CREATE TABLE [history].[ProjectHistory] (
    [OperationId]   UNIQUEIDENTIFIER   NOT NULL,
    [ProjectId]     UNIQUEIDENTIFIER   NOT NULL,
    [Title]         NVARCHAR (50)      NOT NULL,
    [ValidFrom]     DATETIMEOFFSET (7) NOT NULL,
    [ValidTo]       DATETIMEOFFSET (7) NOT NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_history_ProjectHistory] PRIMARY KEY CLUSTERED ([ValidTo] ASC, [ValidFrom] ASC, [OperationId] ASC, [ProjectId] ASC),
    CONSTRAINT [FK_history_ProjectHistory_dbo_Operation] FOREIGN KEY ([ValidFrom], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [OperationId])
);

