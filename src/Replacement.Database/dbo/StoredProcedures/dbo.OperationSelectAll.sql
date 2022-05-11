CREATE PROCEDURE [dbo].[OperationSelectAll]
    @OperationId uniqueidentifier,
    @OperationName varchar(100),
    @EntityType varchar(50),
    @EntityId nvarchar(100),
    @UserId uniqueidentifier,
	@CreatedAtLow datetimeoffset,
	@CreatedAtHigh datetimeoffset
    
AS BEGIN
    SET NOCOUNT ON;
	
    SELECT TOP (1000)
        -- Replace=SelectTableColumns.[dbo].[Operation] --
        [OperationId] = [OperationId],
        [OperationName] = [OperationName],
        [EntityType] = [EntityType],
        [EntityId] = [EntityId],
        [CreatedAt] = [CreatedAt],
        [UserId] = [UserId],
        [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        -- Replace#SelectTableColumns.[dbo].[Operation] --
        FROM
            [dbo].[Operation]
	WHERE 
		    (NULLIF(@OperationId, [OperationId]) IS NULL)       
		AND (NULLIF(@OperationName, [OperationName]) IS NULL)         
		AND (NULLIF(@EntityType, [EntityType]) IS NULL)       
		AND (NULLIF(@EntityId, [EntityId]) IS NULL)    
		AND (NULLIF(@UserId, [UserId]) IS NULL)  
		AND (1 = CASE 
				WHEN (@CreatedAtLow IS NOT NULL) AND (@CreatedAtHigh IS NOT NULL) THEN
					CASE 
						WHEN (@CreatedAtLow <= [CreatedAt]) AND ([CreatedAt] <=@CreatedAtHigh) THEN 1 
						ELSE 0
					END

				WHEN (@CreatedAtLow IS NOT NULL) AND (@CreatedAtHigh IS NULL) THEN
					CASE 
						WHEN (@CreatedAtLow <= [CreatedAt]) THEN 1 
						ELSE 0
					END

				WHEN (@CreatedAtHigh IS NOT NULL) THEN
					CASE 
						WHEN ([CreatedAt] <=@CreatedAtHigh) THEN 1 
						ELSE 0
					END

				ELSE
					1
				END
		)
	ORDER BY [CreatedAt] DESC
	;
END;
