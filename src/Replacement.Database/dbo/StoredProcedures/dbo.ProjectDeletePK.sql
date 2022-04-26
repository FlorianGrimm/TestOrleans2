CREATE PROCEDURE [dbo].[ProjectDeletePK]
    @ProjectId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @SerialVersion bigint
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @Result AS TABLE (
        [ProjectId] uniqueidentifier
    );

    DELETE FROM [dbo].[Project]
        OUTPUT
            DELETED.[ProjectId]
        INTO @Result
        WHERE (@ProjectId = [ProjectId])
        ;

    IF (EXISTS(
        SELECT
            [ProjectId]
            FROM @Result
        )
    ) BEGIN
        UPDATE TOP(1) [history].[ProjectHistory]
            SET
                [ValidTo] = @ModifiedAt
            WHERE
                    ([OperationId] = @OperationId)
                    AND ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND (@ProjectId = [ProjectId])
        ;
    END;
    SELECT
        [ProjectId]
        FROM @Result
        ;
END;
