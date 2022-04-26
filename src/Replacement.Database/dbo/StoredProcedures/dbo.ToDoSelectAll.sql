CREATE PROCEDURE [dbo].[ToDoSelectAll]
AS BEGIN
    SET NOCOUNT ON;

    SELECT
            -- Replace=SelectTableColumns.[dbo].[ToDo] --
            [ToDoId],
            [ProjectId],
            [UserId],
            [Title],
            [Done],
            [OperationId],
            [CreatedAt],
            [ModifiedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
            -- Replace#SelectTableColumns.[dbo].[ToDo] --
        FROM
            [dbo].[ToDo]
        ;
END;
