CREATE PROCEDURE [dbo].[ToDoDeletePK]
    @ToDoId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @SerialVersion bigint
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @Result AS TABLE (
        [ToDoId] uniqueidentifier
    );

    DELETE FROM [dbo].[ToDo]
        OUTPUT
            DELETED.[ToDoId]
        INTO @Result
        WHERE (@ToDoId = [ToDoId])
        ;

    IF (EXISTS(
        SELECT
            [ToDoId]
            FROM @Result
        )
    ) BEGIN
        UPDATE TOP(1) [history].[ToDoHistory]
            SET
                [ValidTo] = @ModifiedAt
            WHERE
                    ([OperationId] = @OperationId)
                    AND ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND (@ToDoId = [ToDoId])
        ;
    END;
    SELECT
        [ToDoId]
        FROM @Result
        ;
END;
