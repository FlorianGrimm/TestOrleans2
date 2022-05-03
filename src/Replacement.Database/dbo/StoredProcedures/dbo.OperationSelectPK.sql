CREATE PROCEDURE [dbo].[OperationSelectPK]
    @CreatedAt datetimeoffset,
    @OperationId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [OperationId] = [OperationId],
            [OperationName] = [OperationName],
            [EntityType] = [EntityType],
            [EntityId] = [EntityId],
            [CreatedAt] = [CreatedAt],
            [UserId] = [UserId],
            [SerialVersion] = CAST([SerialVersion] AS BIGINT)
        FROM
            [dbo].[Operation]
        WHERE
            (@CreatedAt = [CreatedAt])
             AND (@OperationId = [OperationId])
        ;
END;
