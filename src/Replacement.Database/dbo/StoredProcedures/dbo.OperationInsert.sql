CREATE PROCEDURE [dbo].[OperationInsert]
    @OperationId uniqueidentifier,
    @Title nvarchar(20),
    @Data nvarchar(MAX),
    @CreatedAt datetimeoffset
AS BEGIN
    SET NOCOUNT ON;

    IF (@CreatedAt IS NULL) BEGIN
        SET @CreatedAt = GETUTCDATE();
    END;

    INSERT INTO [dbo].[Operation] (
        [OperationId],
        [Title],
        [Data],
        [CreatedAt]
    ) Values (
        @OperationId,
        @Title,
        @Data,
        @CreatedAt
    );

    -- Replace=SelectPKTempateBody.[dbo].[Operation] --        
    SELECT TOP(1)
            [OperationId],
            [Title],
            [EntityType],
            [EntityId],
            [Data],
            [CreatedAt],
            [UserId],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Operation]
        WHERE
            (@CreatedAt = [CreatedAt])
             AND (@OperationId = [OperationId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[Operation] --
END;
