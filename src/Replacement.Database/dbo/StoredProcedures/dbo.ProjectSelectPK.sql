CREATE PROCEDURE [dbo].[ProjectSelectPK]
    @ProjectId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Result AS TABLE (
        [UserId] uniqueidentifier
    );

    -- Replace=SelectPKTempateBody.[dbo].[Project] --        
    SELECT TOP(1)
            [ProjectId] = [ProjectId],
            [Title] = [Title],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [SerialVersion] = CAST([SerialVersion] AS BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@ProjectId = [ProjectId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[Project] --

    /*
    INSERT INTO @Result ([UserId]) SELECT [CreatedBy],[ModifiedBy]
    */

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
        [SerialVersion] = CAST([SerialVersion] AS BIGINT)
        -- Replace#SelectTableColumns.[dbo].[ToDo] --
    FROM [dbo].[ToDO]
    WHERE (@ProjectId = [ProjectId])
        ;

    --  SELECT TOP(1)
    --          [UserId],
    --          [UserName],
    --          [OperationId],
    --          [CreatedAt],
    --          [CreatedBy],
    --          [ModifiedAt],
    --          [ModifiedBy],
    --          [SerialVersion] = CAST([SerialVersion] as BIGINT)
    --      FROM
    --          [dbo].[User]
    --      WHERE
    --          (@UserId = [UserId])
    --      ;
END;
