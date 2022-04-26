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
            [UserId],
            [UserName],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[User]
        WHERE
            (@UserId = [UserId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[User] --
END;