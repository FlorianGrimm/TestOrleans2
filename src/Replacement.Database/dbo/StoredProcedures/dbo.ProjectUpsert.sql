CREATE PROCEDURE [dbo].[ProjectUpsert]
    @Id uniqueidentifier,
    @Title nvarchar(50),
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @ModifiedAt datetimeoffset,
    @SerialVersion BIGINT
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentId uniqueidentifier;
    DECLARE @CurrentTitle nvarchar(50);
    DECLARE @CurrentOperationId uniqueidentifier;
    DECLARE @CurrentCreatedAt datetimeoffset;
    DECLARE @CurrentModifiedAt datetimeoffset;
    DECLARE @CurrentSerialVersion BIGINT;
    DECLARE @ResultValue INT;

    IF (@CurrentSerialVersion > 0) BEGIN
        SELECT TOP(1)
                @CurrentId = [Id],
                @CurrentTitle = [Title],
                @CurrentOperationId = [OperationId],
                @CurrentCreatedAt = [CreatedAt],
                @CurrentModifiedAt = [ModifiedAt],
                @CurrentSerialVersion = CAST([SerialVersion] as BIGINT)
            FROM
                [dbo].[Project]
            WHERE
                (@Id = [Id])
            ;
    END ELSE BEGIN
        SELECT TOP(1)
                @CurrentSerialVersion = CAST([SerialVersion] as BIGINT)
            FROM
                [dbo].[Project]
            WHERE
                (@Id = [Id])
            ;
    END;
    IF ((@CurrentSerialVersion IS NULL)) BEGIN
        INSERT INTO [dbo].[Project] (
            [Id],
            [Title],
            [OperationId],
            [CreatedAt],
            [ModifiedAt]
        ) Values (
            @Id,
            @Title,
            @OperationId,
            @CreatedAt,
            @ModifiedAt
        );
        SET @ResultValue = 1; /* Inserted */
        INSERT INTO [history].[ProjectHistory] (
            [Id],
            [Title],
            [OperationId],
            [ValidFrom],
            [ValidTo]
        ) Values (
            @Id,
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
                        @Id,
                        @Title
                    EXCEPT
                    SELECT
                        @CurrentId,
                        @CurrentTitle
                )) BEGIN
                UPDATE TOP(1) [dbo].[Project]
                    SET
                        [Title] = @Title,
                        [OperationId] = @OperationId,
                        [CreatedAt] = @CreatedAt,
                        [ModifiedAt] = @ModifiedAt
                    WHERE
                        ([Id] = @Id)
                ;
                SET @ResultValue = 2; /* Updated */
                UPDATE TOP(1) [history].[ProjectHistory]
                    SET
                        [ValidTo] = @ModifiedAt
                    WHERE
                        ([ValidTo] = CAST('3141-05-09T00:00:00Z' as datetimeoffset))
                        AND ([OperationId] = @OperationId)
                        AND ([Id] = @Id)
                ;
                INSERT INTO [history].[ProjectHistory] (
                    [Id],
                    [Title],
                    [OperationId],
                    [ValidFrom],
                    [ValidTo]
                ) Values (
                    @Id,
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
            [Id],
            [Title],
            [OperationId],
            [CreatedAt],
            [ModifiedAt],
            [SerialVersion] = CAST([SerialVersion] as BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@Id = [Id])
        ;
    SELECT ResultValue = @ResultValue, Message='';
END;
