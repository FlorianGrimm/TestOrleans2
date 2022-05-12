CREATE PROCEDURE [dbo].[ToDoSelectPK]
    @ProjectId uniqueidentifier,
    @ToDoId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [ProjectId] = [ProjectId],
            [ToDoId] = [ToDoId],
            [UserId] = [UserId],
            [Title] = [Title],
            [Done] = [Done],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [dbo].[ToDo]
        WHERE
            (@ProjectId = [ProjectId])
             AND (@ToDoId = [ToDoId])
        ;
END;
