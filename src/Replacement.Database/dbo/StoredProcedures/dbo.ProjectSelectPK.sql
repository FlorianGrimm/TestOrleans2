CREATE PROCEDURE [dbo].[ProjectSelectPK]
    @ProjectId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    -- Replace=SelectPKTempateBody.[dbo].[Project] --        
    SELECT TOP(1)
            [ProjectId],
            [Title],
            [OperationId],
            [CreatedAt],
            [ModifiedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@ProjectId = [ProjectId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[Project] --
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
    FROM [dbo].[ToDO]
    WHERE (@ProjectId = [ProjectId])
        ;
END;
