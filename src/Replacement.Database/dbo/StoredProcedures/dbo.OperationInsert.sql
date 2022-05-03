CREATE PROCEDURE [dbo].[OperationInsert]
    -- Replace=TableColumnsAsParameter.[dbo].[Operation] --
    @OperationId uniqueidentifier,
    @OperationName varchar(100),
    @EntityType varchar(50),
    @EntityId nvarchar(100),
    @CreatedAt datetimeoffset,
    @UserId uniqueidentifier
    -- Replace#TableColumnsAsParameter.[dbo].[Operation] --
AS BEGIN
    SET NOCOUNT ON;

    -- Replace=AtTableResultTempate.[dbo].[Operation] --
    DECLARE @Result_dbo_Operation AS TABLE (
        [OperationId] uniqueidentifier NOT NULL,
        [OperationName] varchar(100) NOT NULL,
        [EntityType] varchar(50) NOT NULL,
        [EntityId] nvarchar(100) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UserId] uniqueidentifier NULL,
        [SerialVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [CreatedAt],
            [OperationId]
        ));
    -- Replace#AtTableResultTempate.[dbo].[Operation] --
    
    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[Operation] --
    INSERT INTO [dbo].[Operation] (
        [OperationId],
        [OperationName],
        [EntityType],
        [EntityId],
        [CreatedAt],
        [UserId]
    )
    OUTPUT
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[OperationName] as [OperationName],
        INSERTED.[EntityType] as [EntityType],
        INSERTED.[EntityId] as [EntityId],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[UserId] as [UserId],
        CAST(INSERTED.[SerialVersion] AS BIGINT) as [SerialVersion]
    INTO @Result_dbo_Operation
    VALUES (
        @OperationId,
        @OperationName,
        @EntityType,
        @EntityId,
        @CreatedAt,
        @UserId
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[Operation] --

    -- Replace=SelectAtTableResultTemplate.[dbo].[Operation] --
    SELECT
            [OperationId] = [OperationId],
            [OperationName] = [OperationName],
            [EntityType] = [EntityType],
            [EntityId] = [EntityId],
            [CreatedAt] = [CreatedAt],
            [UserId] = [UserId],
            [SerialVersion] = CAST([SerialVersion] AS BIGINT)
        FROM
            @Result_dbo_Operation
        ;
    -- Replace#SelectAtTableResultTemplate.[dbo].[Operation] --
END;
