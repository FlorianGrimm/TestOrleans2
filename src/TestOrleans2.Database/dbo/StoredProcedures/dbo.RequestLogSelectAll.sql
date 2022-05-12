CREATE PROCEDURE [dbo].[RequestLogSelectAll]
	@RequestLogId uniqueidentifier,
	@OperationId uniqueidentifier,
	@ActivityId varchar(200),
	@OperationName varchar(100),
	@EntityType varchar(50),
	@EntityId nvarchar(100),
	@Argument nvarchar(MAX),
	@UserId uniqueidentifier,
	@CreatedAtLow datetimeoffset,
	@CreatedAtHigh datetimeoffset
AS BEGIN
	SET NOCOUNT ON;
	IF (@Argument IS NOT NULL) BEGIN SET @Argument = (N'%' + @Argument + N'%');	END;

	SELECT TOP (1000)
        -- Replace=SelectTableColumns.[dbo].[RequestLog] --
        [RequestLogId] = [RequestLogId],
        [OperationId] = [OperationId],
        [ActivityId] = [ActivityId],
        [OperationName] = [OperationName],
        [EntityType] = [EntityType],
        [EntityId] = [EntityId],	
        [Argument] = [Argument],
        [CreatedAt] = [CreatedAt],
        [UserId] = [UserId],
        [EntityVersion] = CAST([EntityVersion] AS BIGINT)
		-- Replace#SelectTableColumns.[dbo].[RequestLog] --
	FROM [dbo].[RequestLog]
	WHERE
		    (NULLIF(@RequestLogId, [RequestLogId]) IS NULL)
		AND (NULLIF(@OperationId, [OperationId]) IS NULL)
		AND (NULLIF(@ActivityId, [ActivityId]) IS NULL)
		AND (NULLIF(@OperationName, [OperationName]) IS NULL)
		AND (NULLIF(@EntityType, [EntityType]) IS NULL)
		AND (NULLIF(@EntityId, [EntityId]) IS NULL)
		AND (NULLIF(@Argument, [Argument]) IS NULL)
		AND (1 = CASE 
				WHEN @Argument IS NULL THEN 1 
				WHEN [Argument] LIKE @Argument THEN 1 
			END)
		AND (NULLIF(@UserId,[UserId]) IS NULL)  
		AND (1 = CASE 
				WHEN (@CreatedAtLow IS NOT NULL) AND (@CreatedAtHigh IS NOT NULL) THEN
					CASE 
						WHEN (@CreatedAtLow <= [CreatedAt]) AND ([CreatedAt] <= @CreatedAtHigh) THEN 1 
						ELSE 0
					END

				WHEN (@CreatedAtLow IS NOT NULL) AND (@CreatedAtHigh IS NULL) THEN
					CASE 
						WHEN (@CreatedAtLow <= [CreatedAt]) THEN 1 
						ELSE 0
					END

				WHEN (@CreatedAtHigh IS NOT NULL) THEN
					CASE 
						WHEN ([CreatedAt] <= @CreatedAtHigh) THEN 1 
						ELSE 0
					END

				ELSE
					1
				END
		)
	ORDER BY [CreatedAt] DESC
	;
END;