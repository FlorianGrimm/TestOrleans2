CREATE PROCEDURE [dbo].[UserUpsert]
    @UserId uniqueidentifier,
    @UserName nvarchar(50),
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @EntityVersion BIGINT
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentUserId uniqueidentifier;
    DECLARE @CurrentUserName nvarchar(50);
    DECLARE @CurrentOperationId uniqueidentifier;
    DECLARE @CurrentCreatedAt datetimeoffset;
    DECLARE @CurrentCreatedBy uniqueidentifier;
    DECLARE @CurrentModifiedAt datetimeoffset;
    DECLARE @CurrentModifiedBy uniqueidentifier;
    DECLARE @CurrentEntityVersion BIGINT;
    DECLARE @ResultValue INT;

    SELECT TOP(1)
            @CurrentUserId = [UserId],
            @CurrentUserName = [UserName],
            @CurrentOperationId = [OperationId],
            @CurrentCreatedAt = [CreatedAt],
            @CurrentCreatedBy = [CreatedBy],
            @CurrentModifiedAt = [ModifiedAt],
            @CurrentModifiedBy = [ModifiedBy],
            @CurrentEntityVersion = CAST([EntityVersion] as BIGINT)
        FROM
            [dbo].[User]
        WHERE
            (@UserId = [UserId])
        ;
    IF ((@CurrentEntityVersion IS NULL)) BEGIN
        INSERT INTO [dbo].[User] (
            [UserId],
            [UserName],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy]
        )
        VALUES (
            @UserId,
            @UserName,
            @OperationId,
            @CreatedAt,
            @CreatedBy,
            @ModifiedAt,
            @ModifiedBy
        );
        SET @ResultValue = 1; /* Inserted */
        INSERT INTO [history].[UserHistory] (
            [UserId],
            [UserName],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy],
            [ValidFrom],
            [ValidTo]
        )
        VALUES (
            @UserId,
            @UserName,
            @OperationId,
            @CreatedAt,
            @CreatedBy,
            @ModifiedAt,
            @ModifiedBy,
            @ModifiedAt,
            CAST('3141-05-09T00:00:00Z' as datetimeoffset)
        );
    END ELSE BEGIN
        IF ((@EntityVersion <= 0)
            OR ((0 < @EntityVersion) AND (@EntityVersion = @CurrentEntityVersion))
        ) BEGIN
            IF (EXISTS(
                    SELECT
                        @UserId,
                        @UserName,
                        @CreatedBy,
                        @ModifiedBy
                    EXCEPT
                    SELECT
                        @CurrentUserId,
                        @CurrentUserName,
                        @CurrentCreatedBy,
                        @CurrentModifiedBy
                )) BEGIN
                UPDATE TOP(1) [dbo].[User]
                    SET
                        [UserName] = @UserName,
                        [OperationId] = @OperationId,
                        [CreatedBy] = @CreatedBy,
                        [ModifiedAt] = @ModifiedAt,
                        [ModifiedBy] = @ModifiedBy
                    WHERE
                        ([UserId] = @UserId)
                ;
                SET @ResultValue = 2; /* Updated */
                UPDATE TOP(1) [history].[UserHistory]
                    SET
                        [ValidTo] = @ModifiedAt
                    WHERE
                        ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND ([OperationId] = @CurrentOperationId)
                        AND ([UserId] = @UserId)
                ;
                INSERT INTO [history].[UserHistory] (
                    [UserId],
                    [UserName],
                    [OperationId],
                    [CreatedAt],
                    [CreatedBy],
                    [ModifiedAt],
                    [ModifiedBy],
                    [ValidFrom],
                    [ValidTo]
                )
                VALUES (
                    @UserId,
                    @UserName,
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
            [UserId],
            [UserName],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] as BIGINT)
        FROM
            [dbo].[User]
        WHERE
            (@UserId = [UserId])
        ;
    SELECT ResultValue = @ResultValue, [Message] = '';
END;
