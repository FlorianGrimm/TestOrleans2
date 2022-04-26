CREATE PROCEDURE [dbo].[ProjectUpsert]
    @ProjectId uniqueidentifier,
    @Title nvarchar(50),
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @ModifiedAt datetimeoffset,
    @SerialVersion BIGINT
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentProjectId uniqueidentifier;
    DECLARE @CurrentTitle nvarchar(50);
    DECLARE @CurrentOperationId uniqueidentifier;
    DECLARE @CurrentCreatedAt datetimeoffset;
    DECLARE @CurrentModifiedAt datetimeoffset;
    DECLARE @CurrentSerialVersion BIGINT;
    DECLARE @ResultValue INT;

    IF (@CurrentSerialVersion > 0) BEGIN
        SELECT TOP(1)
                @CurrentProjectId = [ProjectId],
                @CurrentTitle = [Title],
                @CurrentOperationId = [OperationId],
                @CurrentCreatedAt = [CreatedAt],
                @CurrentModifiedAt = [ModifiedAt],
                @CurrentSerialVersion = CAST([SerialVersion] as BIGINT)
            FROM
                [dbo].[Project]
            WHERE
                (@ProjectId = [ProjectId])
            ;
    END ELSE BEGIN
        SELECT TOP(1)
                @CurrentSerialVersion = CAST([SerialVersion] as BIGINT)
            FROM
                [dbo].[Project]
            WHERE
                (@ProjectId = [ProjectId])
            ;
    END;
    IF ((@CurrentSerialVersion IS NULL)) BEGIN
        INSERT INTO [dbo].[Project] (
            [ProjectId],
            [Title],
            [OperationId],
            [CreatedAt],
            [ModifiedAt]
        ) Values (
            @ProjectId,
            @Title,
            @OperationId,
            @CreatedAt,
            @ModifiedAt
        );
        SET @ResultValue = 1; /* Inserted */
        INSERT INTO [history].[ProjectHistory] (
            [ProjectId],
            [Title],
            [OperationId],
            [ValidFrom],
            [ValidTo]
        ) Values (
            @ProjectId,
            @Title,
            @OperationId,
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
                        @Title
                    EXCEPT
                    SELECT
                        @CurrentProjectId,
                        @CurrentTitle
                )) BEGIN
                UPDATE TOP(1) [dbo].[Project]
                    SET
                        [Title] = @Title,
                        [OperationId] = @OperationId,
                        [ModifiedAt] = @ModifiedAt
                    WHERE
                        ([ProjectId] = @ProjectId)
                ;
                SET @ResultValue = 2; /* Updated */
                UPDATE TOP(1) [history].[ProjectHistory]
                    SET
                        [ValidTo] = @ModifiedAt
                    WHERE
                        ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND ([OperationId] = @OperationId)
                        AND ([ProjectId] = @ProjectId)
                ;
                INSERT INTO [history].[ProjectHistory] (
                    [ProjectId],
                    [Title],
                    [OperationId],
                    [ValidFrom],
                    [ValidTo]
                ) Values (
                    @ProjectId,
                    @Title,
                    @OperationId,
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
            [ModifiedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@ProjectId = [ProjectId])
        ;
    SELECT ResultValue = @ResultValue, Message='';
END;
