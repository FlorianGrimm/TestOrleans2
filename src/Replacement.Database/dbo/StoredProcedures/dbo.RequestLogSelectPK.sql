CREATE PROCEDURE [dbo].[RequestLogSelectPK]
    @RequestLogId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [RequestLogId] = [RequestLogId],
            [OperationId] = [OperationId],
            [ActivityId] = [ActivityId],
            [OperationName] = [OperationName],
            [EntityType] = [EntityType],
            [EntityId] = [EntityId],
            [Argument] = [Argument],
            [CreatedAt] = [CreatedAt],
            [UserId] = [UserId],
            [SerialVersion] = CAST([SerialVersion] AS BIGINT)
        FROM
            [dbo].[RequestLog]
        WHERE
            (@RequestLogId = [RequestLogId])
        ;
END;
