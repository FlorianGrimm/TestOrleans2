CREATE PROCEDURE [dbo].[ToDoSelectAll]
AS BEGIN
    SET NOCOUNT ON;

    SELECT
            -- Replace=SelectTableColumns.[dbo].[ToDo] --
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
            -- Replace#SelectTableColumns.[dbo].[ToDo] --
        FROM
            [dbo].[ToDo]
        ;
END;
