CREATE PROCEDURE [dbo].[ToDoSelectPK]
    @ToDoId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [ToDoId],
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
            (@ToDoId = [ToDoId])
        ;
END;
