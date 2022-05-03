CREATE TABLE [dbo].[Operation] (
    [OperationId]   UNIQUEIDENTIFIER   NOT NULL,
    [OperationName] VARCHAR (100)      NOT NULL,
    [EntityType]    VARCHAR (50)       NOT NULL,
    [EntityId]      NVARCHAR (100)     NOT NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [UserId]        UNIQUEIDENTIFIER   NULL,
    [SerialVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_Operation] PRIMARY KEY CLUSTERED ([CreatedAt] ASC,[OperationId] ASC)
);

