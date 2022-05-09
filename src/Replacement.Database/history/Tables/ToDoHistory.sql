CREATE TABLE [history].[ToDoHistory] (
    [OperationId]   UNIQUEIDENTIFIER   NOT NULL,
    [ProjectId]     UNIQUEIDENTIFIER   NOT NULL,
    [ToDoId]        UNIQUEIDENTIFIER   NOT NULL,
    [UserId]        UNIQUEIDENTIFIER   NOT NULL,
    [Title]         NVARCHAR (50)      NOT NULL,
    [Done]          BIT                NOT NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [CreatedBy]     UNIQUEIDENTIFIER   NULL,
    [ModifiedAt]    DATETIMEOFFSET (7) NOT NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER   NULL,
    [ValidFrom]     DATETIMEOFFSET (7) NOT NULL,
    [ValidTo]       DATETIMEOFFSET (7) NOT NULL,
    [EntityVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_history_ToDoistory] PRIMARY KEY CLUSTERED ([ValidTo] ASC, [ValidFrom] ASC, [OperationId] ASC, [ProjectId] ASC, [ToDoId] ASC),
    CONSTRAINT [FK_history_ToDoHistory_dbo_Operation] FOREIGN KEY ([ValidFrom], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [OperationId])
);

