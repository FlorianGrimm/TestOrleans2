CREATE PROCEDURE [dbo].[ProjectUpsert]
    @ProjectId uniqueidentifier,
    @Title nvarchar(50),
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @SerialVersion BIGINT
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
    DECLARE @ResultValue INT;

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
    IF ((@CurrentSerialVersion IS NULL)) BEGIN
        INSERT INTO [dbo].[Project] (
            [ProjectId],
            [Title],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy]
        )
        VALUES (
            @ProjectId,
            @Title,
            @OperationId,
            @CreatedAt,
            @CreatedBy,
            @ModifiedAt,
            @ModifiedBy
        );
        SET @ResultValue = 1; /* Inserted */
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
        )
        VALUES (
            @ProjectId,
            @Title,
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
                        @Title,
                        @CreatedBy,
                        @ModifiedBy
                    EXCEPT
                    SELECT
                        @CurrentProjectId,
                        @CurrentTitle,
                        @CurrentCreatedBy,
                        @CurrentModifiedBy
                )) BEGIN
                UPDATE TOP(1) [dbo].[Project]
                    SET
                        [Title] = @Title,
                        [OperationId] = @OperationId,
                        [CreatedBy] = @CreatedBy,
                        [ModifiedAt] = @ModifiedAt,
                        [ModifiedBy] = @ModifiedBy
                    WHERE
                        ([ProjectId] = @ProjectId)
                ;
                SET @ResultValue = 2; /* Updated */
                UPDATE TOP(1) [history].[ProjectHistory]
                    SET
                        [ValidTo] = @ModifiedAt
                    WHERE
                        ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND ([OperationId] = @CurrentOperationId)
                        AND ([ProjectId] = @ProjectId)
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
                )
                VALUES (
                    @ProjectId,
                    @Title,
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
            [Title],
            [OperationId],
            [CreatedAt],
            [CreatedBy],
            [ModifiedAt],
            [ModifiedBy],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@ProjectId = [ProjectId])
        ;
    SELECT ResultValue = @ResultValue, [Message] = '';
END;
