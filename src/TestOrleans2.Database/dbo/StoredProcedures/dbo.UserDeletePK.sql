CREATE PROCEDURE [dbo].[UserDeletePK]
    @UserId uniqueidentifier,
    @OperationId uniqueidentifier,
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
    DECLARE @Result AS TABLE (
        [UserId] uniqueidentifier
    );

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

    DELETE FROM [dbo].[User]
        OUTPUT
            DELETED.[UserId]
        INTO @Result
        WHERE (@UserId = [UserId])
        ;

    IF (EXISTS(
        SELECT
            [UserId]
            FROM @Result
        )
    ) BEGIN
        UPDATE TOP(1) [history].[UserHistory]
            SET
                [ValidTo] = @ModifiedAt
            WHERE
                    ([OperationId] = @CurrentOperationId)
                    AND ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND (@UserId = [UserId])
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
            @CurrentUserId,
            @CurrentUserName,
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
        [UserId]
        FROM @Result
        ;
END;
