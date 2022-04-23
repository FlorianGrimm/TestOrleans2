CREATE PROCEDURE [dbo].[ProjectSelectPK]
    @Id uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [Id],
            [Title],
            [OperationId],
            [CreatedAt],
            [ModifiedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@Id = [Id])
        ;
END;
