CREATE PROCEDURE [dbo].[ToDoSelectPK]
    @ProjectId uniqueidentifier,
    @ToDoId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [ProjectId],
            [ToDoId],
            [UserId],
            [Title],
            [Done],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[ToDo]
        WHERE
            (@ProjectId = [ProjectId])
             AND (@ToDoId = [ToDoId])
        ;
END;
