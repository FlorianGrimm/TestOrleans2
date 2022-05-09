CREATE PROCEDURE [dbo].[UserSelectByUserName]
	 @UserName      NVARCHAR (50) 
AS BEGIN
    SET NOCOUNT ON;
    DECLARE @UserId uniqueidentifier;
    SELECT TOP(1)
            @UserId=[UserId]         
        FROM
            [dbo].[User]
        WHERE
            (@UserName = [UserName])
        ;
    -- Replace=SelectPKTempateBody.[dbo].[User] --        
    SELECT TOP(1)
            [UserId] = [UserId],
            [UserName] = [UserName],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [dbo].[User]
        WHERE
            (@UserId = [UserId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[User] --
END;