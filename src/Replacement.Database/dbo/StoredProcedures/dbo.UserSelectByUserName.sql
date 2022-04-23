CREATE PROCEDURE [dbo].[UserSelectByUserName]
	 @UserName      NVARCHAR (50) 
AS BEGIN
    SET NOCOUNT ON;
    DECLARE @Id uniqueidentifier;
    SELECT TOP(1)
            @Id=[Id]         
        FROM
            [dbo].[User]
        WHERE
            (@UserName = [UserName])
        ;
    -- Replace=SelectPKTempateBody.[dbo].[User] --        
    SELECT TOP(1)
            [Id],
            [UserName],
            [OperationId],
            [CreatedAt],
            [ModifiedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[User]
        WHERE
            (@Id = [Id])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[User] --
END;