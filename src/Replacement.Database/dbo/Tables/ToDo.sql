CREATE TABLE [dbo].[ToDo] (
    [ProjectId]     UNIQUEIDENTIFIER   NOT NULL,
    [ToDoId]        UNIQUEIDENTIFIER   NOT NULL,
    [UserId]        UNIQUEIDENTIFIER   NULL,
    [Title]         NVARCHAR (50)      NOT NULL,
    [Done]          BIT                NOT NULL,
    [OperationId]    UNIQUEIDENTIFIER   NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [CreatedBy]     UNIQUEIDENTIFIER   NULL,
    [ModifiedAt]    DATETIMEOFFSET (7) NOT NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER   NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_ToDo] PRIMARY KEY CLUSTERED ([ProjectId] ASC, [ToDoId] ASC),
    CONSTRAINT [FK_dbo_ToDo_dbo_Operation] FOREIGN KEY ([ModifiedAt], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [OperationId]),
    CONSTRAINT [FK_dbo_ToDo_dbo_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_dbo_ToDo_dbo_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);



