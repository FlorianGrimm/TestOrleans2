CREATE PROCEDURE [dbo].[OperationInsert]
    @OperationId uniqueidentifier,
    @Title nvarchar(20),
    @EntityType nvarchar(100),
    @EntityId nvarchar(100),
    @Data nvarchar(MAX),
    @CreatedAt datetimeoffset,
    @UserId uniqueidentifier
AS BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Operation](
        [OperationId]
        ,[Title]
        ,[EntityType]
        ,[EntityId]
        ,[Data]
        ,[CreatedAt]
        ,[UserId]
    ) Values (
        @OperationId
        ,@Title
        ,@EntityType
        ,@EntityId
        ,@Data
        ,@CreatedAt
        ,@UserId
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
