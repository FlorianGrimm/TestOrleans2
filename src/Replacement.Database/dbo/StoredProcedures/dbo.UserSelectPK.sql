CREATE PROCEDURE [dbo].[UserSelectPK]
    @UserId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    SELECT TOP(1)
            [UserId] = [UserId],
            [UserName] = [UserName],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [SerialVersion] = CAST([SerialVersion] AS BIGINT)
        FROM
            [dbo].[User]
        WHERE
            (@UserId = [UserId])
        ;
END;
