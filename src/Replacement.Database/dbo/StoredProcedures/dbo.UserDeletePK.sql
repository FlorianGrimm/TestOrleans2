CREATE PROCEDURE [dbo].[UserDeletePK]
    @UserId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @SerialVersion bigint
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @Result AS TABLE (
        [UserId] uniqueidentifier
    );

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
                    ([OperationId] = @OperationId)
                    AND ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND (@UserId = [UserId])
        ;
    END;
    SELECT
        [UserId]
        FROM @Result
        ;
END;
