CREATE PROCEDURE [dbo].[OperationSelectPK]
    @CreatedAt datetimeoffset,
    @Id uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [Id],
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
             AND (@Id = [Id])
        ;
END;
