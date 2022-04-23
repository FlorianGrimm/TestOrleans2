CREATE TABLE [dbo].[ToDo] (
    [Id]            UNIQUEIDENTIFIER   NOT NULL,
    [ProjectId]     UNIQUEIDENTIFIER   NULL,
    [UserId]        UNIQUEIDENTIFIER   NULL,
    [Title]         NVARCHAR (50)      NOT NULL,
    [Done]          BIT                NOT NULL,
    [OperationId]    UNIQUEIDENTIFIER   NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [ModifiedAt]    DATETIMEOFFSET (7) NOT NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_ToDo] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo_ToDo_dbo_Operation] FOREIGN KEY ([ModifiedAt], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [Id]),
    CONSTRAINT [FK_dbo_ToDo_dbo_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id]),
    CONSTRAINT [FK_dbo_ToDo_dbo_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



