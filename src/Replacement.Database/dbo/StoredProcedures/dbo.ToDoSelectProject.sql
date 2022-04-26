CREATE PROCEDURE [dbo].[ToDoSelectProject]
    @ToDoId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT
            -- Replace=SelectTableColumns.[dbo].[ToDo] --
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
            -- Replace#SelectTableColumns.[dbo].[ToDo] --
        FROM
            [dbo].[ToDo]
        WHERE
             (@ToDoId = [ToDoId])
        ;

END;