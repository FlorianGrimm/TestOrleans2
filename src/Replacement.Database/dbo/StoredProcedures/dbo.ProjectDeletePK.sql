CREATE PROCEDURE [dbo].[ProjectDeletePK]
    @ProjectId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @SerialVersion bigint
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentProjectId uniqueidentifier;
    DECLARE @CurrentTitle nvarchar(50);
    DECLARE @CurrentOperationId uniqueidentifier;
    DECLARE @CurrentCreatedAt datetimeoffset;
    DECLARE @CurrentCreatedBy uniqueidentifier;
    DECLARE @CurrentModifiedAt datetimeoffset;
    DECLARE @CurrentModifiedBy uniqueidentifier;
    DECLARE @CurrentSerialVersion BIGINT;
    DECLARE @Result AS TABLE (
        [ProjectId] uniqueidentifier
    );

    SELECT TOP(1)
            @CurrentProjectId = [ProjectId],
            @CurrentTitle = [Title],
            @CurrentOperationId = [OperationId],
            @CurrentCreatedAt = [CreatedAt],
            @CurrentCreatedBy = [CreatedBy],
            @CurrentModifiedAt = [ModifiedAt],
            @CurrentModifiedBy = [ModifiedBy],
            @CurrentSerialVersion = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@ProjectId = [ProjectId])
        ;

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
                    ([OperationId] = @CurrentOperationId)
                    AND ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND (@ProjectId = [ProjectId])
        ;
        INSERT INTO [history].[ProjectHistory] (
            [ProjectId],
            [Title],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy],
            [ValidFrom],
            [ValidTo]
        ) Values (
            @CurrentProjectId,
            @CurrentTitle,
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
        [ProjectId]
        FROM @Result
        ;
END;
