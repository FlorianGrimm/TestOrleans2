CREATE TABLE [dbo].[RequestLog]
(
	[RequestLogId]  UNIQUEIDENTIFIER   NOT NULL,
	[OperationId]   UNIQUEIDENTIFIER   NOT NULL,
    [ActivityId]    VARCHAR (200)      NOT NULL,
    [OperationName] VARCHAR (100)      NOT NULL,
    [EntityType]    VARCHAR (50)       NOT NULL,
    [EntityId]      NVARCHAR (100)     NOT NULL,
    [Argument]      NVARCHAR (MAX)     NULL,
    [CreatedAt]     DATETIMEOFFSET (7) NOT NULL,
    [UserId]        UNIQUEIDENTIFIER   NULL,
    [EntityVersion] ROWVERSION         NOT NULL,
    CONSTRAINT [PK_dbo_RequestLog] PRIMARY KEY CLUSTERED ([RequestLogId] ASC)
)
