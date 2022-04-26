CREATE PROCEDURE [dbo].[ToDoUpsert]
    @ProjectId uniqueidentifier,
    @ToDoId uniqueidentifier,
    @UserId uniqueidentifier,
    @Title nvarchar(50),
    @Done bit,
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @SerialVersion BIGINT
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
    DECLARE @CurrentSerialVersion BIGINT;
    DECLARE @ResultValue INT;

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
            @CurrentSerialVersion = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[ToDo]
        WHERE
            (@ProjectId = [ProjectId])
             AND (@ToDoId = [ToDoId])
        ;
    IF ((@CurrentSerialVersion IS NULL)) BEGIN
        INSERT INTO [dbo].[ToDo] (
            [ProjectId],
            [ToDoId],
            [UserId],
            [Title],
            [Done],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy]
        ) Values (
            @ProjectId,
            @ToDoId,
            @UserId,
            @Title,
            @Done,
            @OperationId,
            @CreatedAt,
            @CreatedBy,
            @ModifiedAt,
            @ModifiedBy
        );
        SET @ResultValue = 1; /* Inserted */
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
        ) Values (
            @ProjectId,
            @ToDoId,
            @UserId,
            @Title,
            @Done,
            @OperationId,
            @CreatedAt,
            @CreatedBy,
            @ModifiedAt,
            @ModifiedBy,
            @ModifiedAt,
            CAST('3141-05-09T00:00:00Z' as datetimeoffset)
        );
    END ELSE BEGIN
        IF ((@SerialVersion <= 0)
            OR ((0 < @SerialVersion) AND (@SerialVersion = @CurrentSerialVersion))
        ) BEGIN
            IF (EXISTS(
                    SELECT
                        @ProjectId,
                        @ToDoId,
                        @UserId,
                        @Title,
                        @Done,
                        @CreatedBy,
                        @ModifiedBy
                    EXCEPT
                    SELECT
                        @CurrentProjectId,
                        @CurrentToDoId,
                        @CurrentUserId,
                        @CurrentTitle,
                        @CurrentDone,
                        @CurrentCreatedBy,
                        @CurrentModifiedBy
                )) BEGIN
                UPDATE TOP(1) [dbo].[ToDo]
                    SET
                        [UserId] = @UserId,
                        [Title] = @Title,
                        [Done] = @Done,
                        [OperationId] = @OperationId,
                        [CreatedBy] = @CreatedBy,
                        [ModifiedAt] = @ModifiedAt,
                        [ModifiedBy] = @ModifiedBy
                    WHERE
                        ([ProjectId] = @ProjectId)
                         AND ([ToDoId] = @ToDoId)
                ;
                SET @ResultValue = 2; /* Updated */
                UPDATE TOP(1) [history].[ToDoHistory]
                    SET
                        [ValidTo] = @ModifiedAt
                    WHERE
                        ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND ([OperationId] = @CurrentOperationId)
                        AND ([ProjectId] = @ProjectId)
                        AND ([ToDoId] = @ToDoId)
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
                ) Values (
                    @ProjectId,
                    @ToDoId,
                    @UserId,
                    @Title,
                    @Done,
                    @OperationId,
                    @CreatedAt,
                    @CreatedBy,
                    @ModifiedAt,
                    @ModifiedBy,
                    @ModifiedAt,
                    CAST('3141-05-09T00:00:00Z' as datetimeoffset)
                );
            END ELSE BEGIN
                SET @ResultValue = 0; /* NoNeedToUpdate */
            END;
        END ELSE BEGIN
            SET @ResultValue = -1 /* RowVersionMismatch */;
        END;
    END;
    SELECT TOP(1)
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
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[ToDo]
        WHERE
            (@ProjectId = [ProjectId])
             AND (@ToDoId = [ToDoId])
        ;
    SELECT ResultValue = @ResultValue, [Message] = '';
END;
