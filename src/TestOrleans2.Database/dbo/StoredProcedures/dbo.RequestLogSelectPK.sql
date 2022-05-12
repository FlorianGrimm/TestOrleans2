CREATE PROCEDURE [dbo].[RequestLogSelectPK]
    @RequestLogId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
		-- Replace=SelectTableColumns.[dbo].[RequestLog] --
  [RequestLogId] = [RequestLogId],
  [OperationId] = [OperationId],
  [ActivityId] = [ActivityId],
  [OperationName] = [OperationName],
  [EntityType] = [EntityType],
  [EntityId] = [EntityId],
  [Argument] = [Argument],
  [CreatedAt] = [CreatedAt],
  [UserId] = [UserId],
  [EntityVersion] = CAST([EntityVersion] AS BIGINT)
		-- Replace#SelectTableColumns.[dbo].[RequestLog] --

        FROM
            [dbo].[RequestLog]
        WHERE
            (@RequestLogId = [RequestLogId])
        ;
END;
