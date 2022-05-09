CREATE TABLE [dbo].[Project] (
    [ProjectId]     UNIQUEIDENTIFIER   NOT NULL,
    [Title]         NVARCHAR (50)      NOT NULL,
    [OperationId]   UNIQUEIDENTIFIER   NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [CreatedBy]     UNIQUEIDENTIFIER   NULL,
    [ModifiedAt]    DATETIMEOFFSET (7) NOT NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER   NULL,
    [EntityVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_Project] PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    CONSTRAINT [FK_Project_Operation] FOREIGN KEY ([ModifiedAt], [OperationId]) REFERENCES [dbo].[Operation] ([CreatedAt], [OperationId])
);

