CREATE PROCEDURE [dbo].[RequestLogInsert]
    -- Replace=TableColumnsAsParameter.[dbo].[RequestLog] --
    @RequestLogId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ActivityId varchar(200),
    @OperationName varchar(100),
    @EntityType varchar(50),
    @EntityId nvarchar(100),
    @Argument nvarchar(MAX),
    @CreatedAt datetimeoffset,
    @UserId uniqueidentifier
    -- Replace#TableColumnsAsParameter.[dbo].[RequestLog] --
AS BEGIN
	SET NOCOUNT ON;
    -- Replace=InsertIntoTableValuesParameterTemplate.[dbo].[RequestLog] --
    INSERT INTO [dbo].[RequestLog] (
        [RequestLogId],
        [OperationId],
        [ActivityId],
        [OperationName],
        [EntityType],
        [EntityId],
        [Argument],
        [CreatedAt],
        [UserId]
    )
    VALUES (
        @RequestLogId,
        @OperationId,
        @ActivityId,
        @OperationName,
        @EntityType,
        @EntityId,
        @Argument,
        @CreatedAt,
        @UserId
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[dbo].[RequestLog] --
END
