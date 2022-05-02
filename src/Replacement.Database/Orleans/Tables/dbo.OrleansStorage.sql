CREATE TABLE dbo.OrleansStorage
(
    -- These are for the book keeping. Orleans calculates
    -- these hashes (see RelationalStorageProvide implementation),
    -- which are signed 32 bit integers mapped to the *Hash fields.
    -- The mapping is done in the code. The
    -- *String columns contain the corresponding clear name fields.
    --
    -- If there are duplicates, they are resolved by using GrainIdN0,
    -- GrainIdN1, GrainIdExtensionString and GrainTypeString fields.
    -- It is assumed these would be rarely needed.
    GrainIdHash                INT NOT NULL,
    GrainIdN0                BIGINT NOT NULL,
    GrainIdN1                BIGINT NOT NULL,
    GrainTypeHash            INT NOT NULL,
    GrainTypeString            NVARCHAR(512) NOT NULL,
    GrainIdExtensionString    NVARCHAR(512) NULL,
    ServiceId                NVARCHAR(150) NOT NULL,

    -- The usage of the Payload records is exclusive in that
    -- only one should be populated at any given time and two others
    -- are NULL. The types are separated to advantage on special
    -- processing capabilities present on database engines (not all might
    -- have both JSON and XML types.
    --
    -- One is free to alter the size of these fields.
    PayloadBinary    VARBINARY(MAX) NULL,
    PayloadXml        XML NULL,
    PayloadJson        NVARCHAR(MAX) NULL,

    -- Informational field, no other use.
    ModifiedOn DATETIME2(3) NOT NULL,

    -- The version of the stored payload.
    Version INT NULL

    -- The following would in principle be the primary key, but it would be too thick
    -- to be indexed, so the values are hashed and only collisions will be solved
    -- by using the fields. That is, after the indexed queries have pinpointed the right
    -- rows down to [0, n] relevant ones, n being the number of collided value pairs.
);
GO

CREATE NONCLUSTERED INDEX IX_OrleansStorage ON dbo.OrleansStorage(GrainIdHash, GrainTypeHash);
GO
-- This ensures lock escalation will not lock the whole table, which can potentially be enormous.
-- See more information at https://www.littlekendra.com/2016/02/04/why-rowlock-hints-can-make-queries-slower-and-blocking-worse-in-sql-server/.
ALTER TABLE dbo.OrleansStorage SET(LOCK_ESCALATION = DISABLE);
GO
-- A feature with ID is compression. If it is supported, it is used for OrleansStorage table. This is an Enterprise feature.
-- This consumes more processor cycles, but should save on space on GrainIdString, GrainTypeString and ServiceId, which
-- contain mainly the same values. Also the payloads will be compressed.
IF EXISTS (SELECT 1 FROM sys.dm_db_persisted_sku_features WHERE feature_id = 100)
BEGIN
    ALTER TABLE dbo.OrleansStorage REBUILD PARTITION = ALL WITH(DATA_COMPRESSION = PAGE);
END
GO
