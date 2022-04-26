CREATE TABLE [dbo].[ToDo] (
    [ToDoId]            UNIQUEIDENTIFIER   NOT NULL,
    [ProjectId]     UNIQUEIDENTIFIER   NULL,
    [UserId]        UNIQUEIDENTIFIER   NULL,
    [Title]         NVARCHAR (50)      NOT NULL,
    [Done]          BIT                NOT NULL,
    [OperationId]    UNIQUEIDENTIFIER   NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [ModifiedAt]    DATETIMEOFFSET (7) NOT NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_ToDo] PRIMARY KEY CLUSTERED ([ToDoId] ASC),
    CONSTRAINT [FK_dbo_ToDo_dbo_Operation] FOREIGN KEY ([ModifiedAt], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [OperationId]),
    CONSTRAINT [FK_dbo_ToDo_dbo_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo_ToDo_dbo_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);



