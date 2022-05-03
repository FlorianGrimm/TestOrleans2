CREATE PROCEDURE [dbo].[ProjectSelectAll]
AS BEGIN
    SET NOCOUNT ON;

    SELECT
        -- Replace=SelectTableColumns.[dbo].[Project] --
        [ProjectId] = [ProjectId],
        [Title] = [Title],
        [OperationId] = [OperationId],
        [CreatedAt] = [CreatedAt],
        [CreatedBy] = [CreatedBy],
        [ModifiedAt] = [ModifiedAt],
        [ModifiedBy] = [ModifiedBy],
        [SerialVersion] = CAST([SerialVersion] AS BIGINT)
        -- Replace#SelectTableColumns.[dbo].[Project] --
    FROM [dbo].[Project]
        ;
END;
