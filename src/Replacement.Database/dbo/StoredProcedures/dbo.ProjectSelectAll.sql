CREATE PROCEDURE [dbo].[ProjectSelectAll]
AS BEGIN
    SET NOCOUNT ON;

    SELECT
        -- Replace=SelectTableColumns.[dbo].[Project] --
        [ProjectId],
        [Title],
        [OperationId],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy],
        [SerialVersion] = CAST([SerialVersion] as BIGINT)
        -- Replace#SelectTableColumns.[dbo].[Project] --
    FROM [dbo].[Project]
        ;
END;