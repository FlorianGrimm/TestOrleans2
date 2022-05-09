CREATE PROCEDURE [dbo].[ToDoDeletePK]
    @ProjectId uniqueidentifier,
    @ToDoId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @EntityVersion BIGINT
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentProjectId uniqueidentifier;
    DECLARE @CurrentToDoId uniqueidentifier;
    DECLARE @CurrentUserId uniqueidentifier;
    DECLARE @CurrentTitle nvarchar(50);
    DECLARE @CurrentDone bit;
    DECLARE @CurrentOperationId uniqueidentifier;
    DECLARE @CurrentCreatedAt datetimeoffset;
    DECLARE @CurrentCreatedBy uniqueidentifier;
    DECLARE @CurrentModifiedAt datetimeoffset;
    DECLARE @CurrentModifiedBy uniqueidentifier;
    DECLARE @CurrentEntityVersion BIGINT;
    DECLARE @Result AS TABLE (
        [ProjectId] uniqueidentifier,
        [ToDoId] uniqueidentifier
    );

    SELECT TOP(1)
            @CurrentProjectId = [ProjectId],
            @CurrentToDoId = [ToDoId],
            @CurrentUserId = [UserId],
            @CurrentTitle = [Title],
            @CurrentDone = [Done],
            @CurrentOperationId = [OperationId],
            @CurrentCreatedAt = [CreatedAt],
            @CurrentCreatedBy = [CreatedBy],
            @CurrentModifiedAt = [ModifiedAt],
            @CurrentModifiedBy = [ModifiedBy],
            @CurrentEntityVersion = CAST([EntityVersion] as BIGINT)
        FROM
            [dbo].[ToDo]
        WHERE
            (@ProjectId = [ProjectId])
             AND (@ToDoId = [ToDoId])
        ;

    DELETE FROM [dbo].[ToDo]
        OUTPUT
            DELETED.[ProjectId],
            DELETED.[ToDoId]
        INTO @Result
        WHERE (@ProjectId = [ProjectId])
            AND (@ToDoId = [ToDoId])
        ;

    IF (EXISTS(
        SELECT
            [ProjectId],
            [ToDoId]
            FROM @Result
        )
    ) BEGIN
        UPDATE TOP(1) [history].[ToDoHistory]
            SET
                [ValidTo] = @ModifiedAt
            WHERE
                    ([OperationId] = @CurrentOperationId)
                    AND ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND (@ProjectId = [ProjectId])
                        AND (@ToDoId = [ToDoId])
        ;
        INSERT INTO [history].[ToDoHistory] (
            [ProjectId],
            [ToDoId],
            [UserId],
            [Title],
            [Done],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy],
            [ValidFrom],
            [ValidTo]
        )
        VALUES (
            @CurrentProjectId,
            @CurrentToDoId,
            @CurrentUserId,
            @CurrentTitle,
            @CurrentDone,
            @OperationId,
            @CurrentCreatedAt,
            @CurrentCreatedBy,
            @ModifiedAt,
            @ModifiedBy,
            @ModifiedAt,
            @ModifiedAt
        );
    END;
    SELECT
        [ProjectId],
        [ToDoId]
        FROM @Result
        ;
END;
