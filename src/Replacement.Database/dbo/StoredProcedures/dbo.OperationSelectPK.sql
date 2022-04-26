CREATE PROCEDURE [dbo].[OperationSelectPK]
    @CreatedAt datetimeoffset,
    @OperationId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [OperationId],
            [Title],
            [EntityType],
            [EntityId],
            [Data],
            [CreatedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Operation]
        WHERE
            (@CreatedAt = [CreatedAt])
             AND (@OperationId = [OperationId])
        ;
END;
