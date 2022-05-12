CREATE PROCEDURE [dbo].[UserSelectAll]
    
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
        -- Replace=SelectTableColumns.[dbo].[User] --
        [UserId] = [UserId],
        [UserName] = [UserName],
        [OperationId] = [OperationId],
        [CreatedAt] = [CreatedAt],
        [CreatedBy] = [CreatedBy],
        [ModifiedAt] = [ModifiedAt],
        [ModifiedBy] = [ModifiedBy],
        [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        -- Replace#SelectTableColumns.[dbo].[User] --
        FROM
            [dbo].[User]    
        ;
END;
