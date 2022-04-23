CREATE PROCEDURE [dbo].[ToDoSelectPK]
    @Id uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [Id],
            [ProjectId],
            [UserId],
            [Title],
            [Done],
            [OperationId],
            [CreatedAt],
            [ModifiedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[ToDo]
        WHERE
            (@Id = [Id])
        ;
END;
