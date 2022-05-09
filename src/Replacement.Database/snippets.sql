/*
-- Replace=SNIPPETS --
    -- Replace=AtTableResultPKTempate.[dbo].[Operation] --
    DECLARE @ResultPK_dbo_Operation AS TABLE (
        [CreatedAt] datetimeoffset NOT NULL,
        [OperationId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [CreatedAt],
            [OperationId]
        ));
    -- Replace#AtTableResultPKTempate.[dbo].[Operation] --

    -- Replace=AtTableResultPKTempate.[dbo].[Project] --
    DECLARE @ResultPK_dbo_Project AS TABLE (
        [ProjectId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [ProjectId]
        ));
    -- Replace#AtTableResultPKTempate.[dbo].[Project] --

    -- Replace=AtTableResultPKTempate.[dbo].[RequestLog] --
    DECLARE @ResultPK_dbo_RequestLog AS TABLE (
        [RequestLogId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [RequestLogId]
        ));
    -- Replace#AtTableResultPKTempate.[dbo].[RequestLog] --

    -- Replace=AtTableResultPKTempate.[dbo].[ToDo] --
    DECLARE @ResultPK_dbo_ToDo AS TABLE (
        [ProjectId] uniqueidentifier NOT NULL,
        [ToDoId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [ProjectId],
            [ToDoId]
        ));
    -- Replace#AtTableResultPKTempate.[dbo].[ToDo] --

    -- Replace=AtTableResultPKTempate.[dbo].[User] --
    DECLARE @ResultPK_dbo_User AS TABLE (
        [UserId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [UserId]
        ));
    -- Replace#AtTableResultPKTempate.[dbo].[User] --

    -- Replace=AtTableResultPKTempate.[history].[ProjectHistory] --
    DECLARE @ResultPK_history_ProjectHistory AS TABLE (
        [ValidTo] datetimeoffset NOT NULL,
        [ValidFrom] datetimeoffset NOT NULL,
        [OperationId] uniqueidentifier NOT NULL,
        [ProjectId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [ValidTo],
            [ValidFrom],
            [OperationId],
            [ProjectId]
        ));
    -- Replace#AtTableResultPKTempate.[history].[ProjectHistory] --

    -- Replace=AtTableResultPKTempate.[history].[ToDoHistory] --
    DECLARE @ResultPK_history_ToDoHistory AS TABLE (
        [ValidTo] datetimeoffset NOT NULL,
        [ValidFrom] datetimeoffset NOT NULL,
        [OperationId] uniqueidentifier NOT NULL,
        [ProjectId] uniqueidentifier NOT NULL,
        [ToDoId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [ValidTo],
            [ValidFrom],
            [OperationId],
            [ProjectId],
            [ToDoId]
        ));
    -- Replace#AtTableResultPKTempate.[history].[ToDoHistory] --

    -- Replace=AtTableResultPKTempate.[history].[UserHistory] --
    DECLARE @ResultPK_history_UserHistory AS TABLE (
        [ValidTo] datetimeoffset NOT NULL,
        [ValidFrom] datetimeoffset NOT NULL,
        [OperationId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL
        PRIMARY KEY CLUSTERED (
            [ValidTo],
            [ValidFrom],
            [OperationId],
            [UserId]
        ));
    -- Replace#AtTableResultPKTempate.[history].[UserHistory] --

    -- Replace=AtTableResultTempate.[dbo].[Operation] --
    DECLARE @Result_dbo_Operation AS TABLE (
        [OperationId] uniqueidentifier NOT NULL,
        [OperationName] varchar(100) NOT NULL,
        [EntityType] varchar(50) NOT NULL,
        [EntityId] nvarchar(100) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UserId] uniqueidentifier NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [CreatedAt],
            [OperationId]
        ));
    -- Replace#AtTableResultTempate.[dbo].[Operation] --

    -- Replace=AtTableResultTempate.[dbo].[Project] --
    DECLARE @Result_dbo_Project AS TABLE (
        [ProjectId] uniqueidentifier NOT NULL,
        [Title] nvarchar(50) NOT NULL,
        [OperationId] uniqueidentifier NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [ModifiedAt] datetimeoffset NOT NULL,
        [ModifiedBy] uniqueidentifier NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [ProjectId]
        ));
    -- Replace#AtTableResultTempate.[dbo].[Project] --

    -- Replace=AtTableResultTempate.[dbo].[RequestLog] --
    DECLARE @Result_dbo_RequestLog AS TABLE (
        [RequestLogId] uniqueidentifier NOT NULL,
        [OperationId] uniqueidentifier NOT NULL,
        [ActivityId] varchar(200) NOT NULL,
        [OperationName] varchar(100) NOT NULL,
        [EntityType] varchar(50) NOT NULL,
        [EntityId] nvarchar(100) NOT NULL,
        [Argument] nvarchar(MAX) NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UserId] uniqueidentifier NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [RequestLogId]
        ));
    -- Replace#AtTableResultTempate.[dbo].[RequestLog] --

    -- Replace=AtTableResultTempate.[dbo].[ToDo] --
    DECLARE @Result_dbo_ToDo AS TABLE (
        [ProjectId] uniqueidentifier NOT NULL,
        [ToDoId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [Title] nvarchar(50) NOT NULL,
        [Done] bit NOT NULL,
        [OperationId] uniqueidentifier NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [ModifiedAt] datetimeoffset NOT NULL,
        [ModifiedBy] uniqueidentifier NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [ProjectId],
            [ToDoId]
        ));
    -- Replace#AtTableResultTempate.[dbo].[ToDo] --

    -- Replace=AtTableResultTempate.[dbo].[User] --
    DECLARE @Result_dbo_User AS TABLE (
        [UserId] uniqueidentifier NOT NULL,
        [UserName] nvarchar(50) NOT NULL,
        [OperationId] uniqueidentifier NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [ModifiedAt] datetimeoffset NOT NULL,
        [ModifiedBy] uniqueidentifier NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [UserId]
        ));
    -- Replace#AtTableResultTempate.[dbo].[User] --

    -- Replace=AtTableResultTempate.[history].[ProjectHistory] --
    DECLARE @Result_history_ProjectHistory AS TABLE (
        [OperationId] uniqueidentifier NOT NULL,
        [ProjectId] uniqueidentifier NOT NULL,
        [Title] nvarchar(50) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [ModifiedAt] datetimeoffset NOT NULL,
        [ModifiedBy] uniqueidentifier NULL,
        [ValidFrom] datetimeoffset NOT NULL,
        [ValidTo] datetimeoffset NOT NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [ValidTo],
            [ValidFrom],
            [OperationId],
            [ProjectId]
        ));
    -- Replace#AtTableResultTempate.[history].[ProjectHistory] --

    -- Replace=AtTableResultTempate.[history].[ToDoHistory] --
    DECLARE @Result_history_ToDoHistory AS TABLE (
        [OperationId] uniqueidentifier NOT NULL,
        [ProjectId] uniqueidentifier NOT NULL,
        [ToDoId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Title] nvarchar(50) NOT NULL,
        [Done] bit NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [ModifiedAt] datetimeoffset NOT NULL,
        [ModifiedBy] uniqueidentifier NULL,
        [ValidFrom] datetimeoffset NOT NULL,
        [ValidTo] datetimeoffset NOT NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [ValidTo],
            [ValidFrom],
            [OperationId],
            [ProjectId],
            [ToDoId]
        ));
    -- Replace#AtTableResultTempate.[history].[ToDoHistory] --

    -- Replace=AtTableResultTempate.[history].[UserHistory] --
    DECLARE @Result_history_UserHistory AS TABLE (
        [OperationId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [UserName] nvarchar(50) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [ModifiedAt] datetimeoffset NOT NULL,
        [ModifiedBy] uniqueidentifier NULL,
        [ValidFrom] datetimeoffset NOT NULL,
        [ValidTo] datetimeoffset NOT NULL,
        [EntityVersion] BIGINT NOT NULL
        PRIMARY KEY CLUSTERED (
            [ValidTo],
            [ValidFrom],
            [OperationId],
            [UserId]
        ));
    -- Replace#AtTableResultTempate.[history].[UserHistory] --

    -- Replace=ColumnRowversion.[dbo].[Operation] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[dbo].[Operation] --

    -- Replace=ColumnRowversion.[dbo].[Project] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[dbo].[Project] --

    -- Replace=ColumnRowversion.[dbo].[RequestLog] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[dbo].[RequestLog] --

    -- Replace=ColumnRowversion.[dbo].[ToDo] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[dbo].[ToDo] --

    -- Replace=ColumnRowversion.[dbo].[User] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[dbo].[User] --

    -- Replace=ColumnRowversion.[history].[ProjectHistory] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[history].[ProjectHistory] --

    -- Replace=ColumnRowversion.[history].[ToDoHistory] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[history].[ToDoHistory] --

    -- Replace=ColumnRowversion.[history].[UserHistory] --
    [EntityVersion] = CAST([EntityVersion] as BIGINT)
    -- Replace#ColumnRowversion.[history].[UserHistory] --

    -- Replace=ConditionColumnsParameterPKTempate.[dbo].[Operation] --
    (@CreatedAt = [CreatedAt])
     AND (@OperationId = [OperationId])
    -- Replace#ConditionColumnsParameterPKTempate.[dbo].[Operation] --

    -- Replace=ConditionColumnsParameterPKTempate.[dbo].[Project] --
    (@ProjectId = [ProjectId])
    -- Replace#ConditionColumnsParameterPKTempate.[dbo].[Project] --

    -- Replace=ConditionColumnsParameterPKTempate.[dbo].[RequestLog] --
    (@RequestLogId = [RequestLogId])
    -- Replace#ConditionColumnsParameterPKTempate.[dbo].[RequestLog] --

    -- Replace=ConditionColumnsParameterPKTempate.[dbo].[ToDo] --
    (@ProjectId = [ProjectId])
     AND (@ToDoId = [ToDoId])
    -- Replace#ConditionColumnsParameterPKTempate.[dbo].[ToDo] --

    -- Replace=ConditionColumnsParameterPKTempate.[dbo].[User] --
    (@UserId = [UserId])
    -- Replace#ConditionColumnsParameterPKTempate.[dbo].[User] --

    -- Replace=ConditionColumnsParameterPKTempate.[history].[ProjectHistory] --
    (@ValidTo = [ValidTo])
     AND (@ValidFrom = [ValidFrom])
     AND (@OperationId = [OperationId])
     AND (@ProjectId = [ProjectId])
    -- Replace#ConditionColumnsParameterPKTempate.[history].[ProjectHistory] --

    -- Replace=ConditionColumnsParameterPKTempate.[history].[ToDoHistory] --
    (@ValidTo = [ValidTo])
     AND (@ValidFrom = [ValidFrom])
     AND (@OperationId = [OperationId])
     AND (@ProjectId = [ProjectId])
     AND (@ToDoId = [ToDoId])
    -- Replace#ConditionColumnsParameterPKTempate.[history].[ToDoHistory] --

    -- Replace=ConditionColumnsParameterPKTempate.[history].[UserHistory] --
    (@ValidTo = [ValidTo])
     AND (@ValidFrom = [ValidFrom])
     AND (@OperationId = [OperationId])
     AND (@UserId = [UserId])
    -- Replace#ConditionColumnsParameterPKTempate.[history].[UserHistory] --

    -- Replace=DeletePKTempateParameter.[dbo].[Project] --
    @ProjectId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @EntityVersion BIGINT
    -- Replace#DeletePKTempateParameter.[dbo].[Project] --

    -- Replace=DeletePKTempateParameter.[dbo].[ToDo] --
    @ProjectId uniqueidentifier,
    @ToDoId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @EntityVersion BIGINT
    -- Replace#DeletePKTempateParameter.[dbo].[ToDo] --

    -- Replace=DeletePKTempateParameter.[dbo].[User] --
    @UserId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @EntityVersion BIGINT
    -- Replace#DeletePKTempateParameter.[dbo].[User] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[Operation] --
    INSERT INTO [dbo].[Operation] (
        [OperationId],
        [OperationName],
        [EntityType],
        [EntityId],
        [CreatedAt],
        [UserId]
    )
    OUTPUT
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[OperationName] as [OperationName],
        INSERTED.[EntityType] as [EntityType],
        INSERTED.[EntityId] as [EntityId],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[UserId] as [UserId],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_dbo_Operation
    VALUES (
        @OperationId,
        @OperationName,
        @EntityType,
        @EntityId,
        @CreatedAt,
        @UserId
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[Operation] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[Project] --
    INSERT INTO [dbo].[Project] (
        [ProjectId],
        [Title],
        [OperationId],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy]
    )
    OUTPUT
        INSERTED.[ProjectId] as [ProjectId],
        INSERTED.[Title] as [Title],
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[CreatedBy] as [CreatedBy],
        INSERTED.[ModifiedAt] as [ModifiedAt],
        INSERTED.[ModifiedBy] as [ModifiedBy],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_dbo_Project
    VALUES (
        @ProjectId,
        @Title,
        @OperationId,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[Project] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[RequestLog] --
    INSERT INTO [dbo].[RequestLog] (
        [RequestLogId],
        [OperationId],
        [ActivityId],
        [OperationName],
        [EntityType],
        [EntityId],
        [Argument],
        [CreatedAt],
        [UserId]
    )
    OUTPUT
        INSERTED.[RequestLogId] as [RequestLogId],
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[ActivityId] as [ActivityId],
        INSERTED.[OperationName] as [OperationName],
        INSERTED.[EntityType] as [EntityType],
        INSERTED.[EntityId] as [EntityId],
        INSERTED.[Argument] as [Argument],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[UserId] as [UserId],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_dbo_RequestLog
    VALUES (
        @RequestLogId,
        @OperationId,
        @ActivityId,
        @OperationName,
        @EntityType,
        @EntityId,
        @Argument,
        @CreatedAt,
        @UserId
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[RequestLog] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[ToDo] --
    INSERT INTO [dbo].[ToDo] (
        [ProjectId],
        [ToDoId],
        [UserId],
        [Title],
        [Done],
        [OperationId],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy]
    )
    OUTPUT
        INSERTED.[ProjectId] as [ProjectId],
        INSERTED.[ToDoId] as [ToDoId],
        INSERTED.[UserId] as [UserId],
        INSERTED.[Title] as [Title],
        INSERTED.[Done] as [Done],
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[CreatedBy] as [CreatedBy],
        INSERTED.[ModifiedAt] as [ModifiedAt],
        INSERTED.[ModifiedBy] as [ModifiedBy],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_dbo_ToDo
    VALUES (
        @ProjectId,
        @ToDoId,
        @UserId,
        @Title,
        @Done,
        @OperationId,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[ToDo] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[User] --
    INSERT INTO [dbo].[User] (
        [UserId],
        [UserName],
        [OperationId],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy]
    )
    OUTPUT
        INSERTED.[UserId] as [UserId],
        INSERTED.[UserName] as [UserName],
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[CreatedBy] as [CreatedBy],
        INSERTED.[ModifiedAt] as [ModifiedAt],
        INSERTED.[ModifiedBy] as [ModifiedBy],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_dbo_User
    VALUES (
        @UserId,
        @UserName,
        @OperationId,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[dbo].[User] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[history].[ProjectHistory] --
    INSERT INTO [history].[ProjectHistory] (
        [OperationId],
        [ProjectId],
        [Title],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy],
        [ValidFrom],
        [ValidTo]
    )
    OUTPUT
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[ProjectId] as [ProjectId],
        INSERTED.[Title] as [Title],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[CreatedBy] as [CreatedBy],
        INSERTED.[ModifiedAt] as [ModifiedAt],
        INSERTED.[ModifiedBy] as [ModifiedBy],
        INSERTED.[ValidFrom] as [ValidFrom],
        INSERTED.[ValidTo] as [ValidTo],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_history_ProjectHistory
    VALUES (
        @OperationId,
        @ProjectId,
        @Title,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy,
        @ValidFrom,
        @ValidTo
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[history].[ProjectHistory] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[history].[ToDoHistory] --
    INSERT INTO [history].[ToDoHistory] (
        [OperationId],
        [ProjectId],
        [ToDoId],
        [UserId],
        [Title],
        [Done],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy],
        [ValidFrom],
        [ValidTo]
    )
    OUTPUT
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[ProjectId] as [ProjectId],
        INSERTED.[ToDoId] as [ToDoId],
        INSERTED.[UserId] as [UserId],
        INSERTED.[Title] as [Title],
        INSERTED.[Done] as [Done],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[CreatedBy] as [CreatedBy],
        INSERTED.[ModifiedAt] as [ModifiedAt],
        INSERTED.[ModifiedBy] as [ModifiedBy],
        INSERTED.[ValidFrom] as [ValidFrom],
        INSERTED.[ValidTo] as [ValidTo],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_history_ToDoHistory
    VALUES (
        @OperationId,
        @ProjectId,
        @ToDoId,
        @UserId,
        @Title,
        @Done,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy,
        @ValidFrom,
        @ValidTo
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[history].[ToDoHistory] --

    -- Replace=InsertIntoTableOutputAtTableResultValuesParameterTemplate.[history].[UserHistory] --
    INSERT INTO [history].[UserHistory] (
        [OperationId],
        [UserId],
        [UserName],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy],
        [ValidFrom],
        [ValidTo]
    )
    OUTPUT
        INSERTED.[OperationId] as [OperationId],
        INSERTED.[UserId] as [UserId],
        INSERTED.[UserName] as [UserName],
        INSERTED.[CreatedAt] as [CreatedAt],
        INSERTED.[CreatedBy] as [CreatedBy],
        INSERTED.[ModifiedAt] as [ModifiedAt],
        INSERTED.[ModifiedBy] as [ModifiedBy],
        INSERTED.[ValidFrom] as [ValidFrom],
        INSERTED.[ValidTo] as [ValidTo],
        CAST(INSERTED.[EntityVersion] AS BIGINT) as [EntityVersion]
    INTO @Result_history_UserHistory
    VALUES (
        @OperationId,
        @UserId,
        @UserName,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy,
        @ValidFrom,
        @ValidTo
    );
    -- Replace#InsertIntoTableOutputAtTableResultValuesParameterTemplate.[history].[UserHistory] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[dbo].[Operation] --
    INSERT INTO [dbo].[Operation] (
        [OperationId],
        [OperationName],
        [EntityType],
        [EntityId],
        [CreatedAt],
        [UserId]
    )
    VALUES (
        @OperationId,
        @OperationName,
        @EntityType,
        @EntityId,
        @CreatedAt,
        @UserId
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[dbo].[Operation] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[dbo].[Project] --
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
    -- Replace#InsertIntoTableValuesParameterTemplate.[dbo].[Project] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[dbo].[RequestLog] --
    INSERT INTO [dbo].[RequestLog] (
        [RequestLogId],
        [OperationId],
        [ActivityId],
        [OperationName],
        [EntityType],
        [EntityId],
        [Argument],
        [CreatedAt],
        [UserId]
    )
    VALUES (
        @RequestLogId,
        @OperationId,
        @ActivityId,
        @OperationName,
        @EntityType,
        @EntityId,
        @Argument,
        @CreatedAt,
        @UserId
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[dbo].[RequestLog] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[dbo].[ToDo] --
    INSERT INTO [dbo].[ToDo] (
        [ProjectId],
        [ToDoId],
        [UserId],
        [Title],
        [Done],
        [OperationId],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy]
    )
    VALUES (
        @ProjectId,
        @ToDoId,
        @UserId,
        @Title,
        @Done,
        @OperationId,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[dbo].[ToDo] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[dbo].[User] --
    INSERT INTO [dbo].[User] (
        [UserId],
        [UserName],
        [OperationId],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy]
    )
    VALUES (
        @UserId,
        @UserName,
        @OperationId,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[dbo].[User] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[history].[ProjectHistory] --
    INSERT INTO [history].[ProjectHistory] (
        [OperationId],
        [ProjectId],
        [Title],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy],
        [ValidFrom],
        [ValidTo]
    )
    VALUES (
        @OperationId,
        @ProjectId,
        @Title,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy,
        @ValidFrom,
        @ValidTo
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[history].[ProjectHistory] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[history].[ToDoHistory] --
    INSERT INTO [history].[ToDoHistory] (
        [OperationId],
        [ProjectId],
        [ToDoId],
        [UserId],
        [Title],
        [Done],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy],
        [ValidFrom],
        [ValidTo]
    )
    VALUES (
        @OperationId,
        @ProjectId,
        @ToDoId,
        @UserId,
        @Title,
        @Done,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy,
        @ValidFrom,
        @ValidTo
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[history].[ToDoHistory] --

    -- Replace=InsertIntoTableValuesParameterTemplate.[history].[UserHistory] --
    INSERT INTO [history].[UserHistory] (
        [OperationId],
        [UserId],
        [UserName],
        [CreatedAt],
        [CreatedBy],
        [ModifiedAt],
        [ModifiedBy],
        [ValidFrom],
        [ValidTo]
    )
    VALUES (
        @OperationId,
        @UserId,
        @UserName,
        @CreatedAt,
        @CreatedBy,
        @ModifiedAt,
        @ModifiedBy,
        @ValidFrom,
        @ValidTo
    );
    -- Replace#InsertIntoTableValuesParameterTemplate.[history].[UserHistory] --

    -- Replace=SelectAtTableResultTemplate.[dbo].[Operation] --
    SELECT
            [OperationId] = [OperationId],
            [OperationName] = [OperationName],
            [EntityType] = [EntityType],
            [EntityId] = [EntityId],
            [CreatedAt] = [CreatedAt],
            [UserId] = [UserId],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_dbo_Operation
        ;
    -- Replace#SelectAtTableResultTemplate.[dbo].[Operation] --

    -- Replace=SelectAtTableResultTemplate.[dbo].[Project] --
    SELECT
            [ProjectId] = [ProjectId],
            [Title] = [Title],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_dbo_Project
        ;
    -- Replace#SelectAtTableResultTemplate.[dbo].[Project] --

    -- Replace=SelectAtTableResultTemplate.[dbo].[RequestLog] --
    SELECT
            [RequestLogId] = [RequestLogId],
            [OperationId] = [OperationId],
            [ActivityId] = [ActivityId],
            [OperationName] = [OperationName],
            [EntityType] = [EntityType],
            [EntityId] = [EntityId],
            [Argument] = [Argument],
            [CreatedAt] = [CreatedAt],
            [UserId] = [UserId],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_dbo_RequestLog
        ;
    -- Replace#SelectAtTableResultTemplate.[dbo].[RequestLog] --

    -- Replace=SelectAtTableResultTemplate.[dbo].[ToDo] --
    SELECT
            [ProjectId] = [ProjectId],
            [ToDoId] = [ToDoId],
            [UserId] = [UserId],
            [Title] = [Title],
            [Done] = [Done],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_dbo_ToDo
        ;
    -- Replace#SelectAtTableResultTemplate.[dbo].[ToDo] --

    -- Replace=SelectAtTableResultTemplate.[dbo].[User] --
    SELECT
            [UserId] = [UserId],
            [UserName] = [UserName],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_dbo_User
        ;
    -- Replace#SelectAtTableResultTemplate.[dbo].[User] --

    -- Replace=SelectAtTableResultTemplate.[history].[ProjectHistory] --
    SELECT
            [OperationId] = [OperationId],
            [ProjectId] = [ProjectId],
            [Title] = [Title],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [ValidFrom] = [ValidFrom],
            [ValidTo] = [ValidTo],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_history_ProjectHistory
        ;
    -- Replace#SelectAtTableResultTemplate.[history].[ProjectHistory] --

    -- Replace=SelectAtTableResultTemplate.[history].[ToDoHistory] --
    SELECT
            [OperationId] = [OperationId],
            [ProjectId] = [ProjectId],
            [ToDoId] = [ToDoId],
            [UserId] = [UserId],
            [Title] = [Title],
            [Done] = [Done],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [ValidFrom] = [ValidFrom],
            [ValidTo] = [ValidTo],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_history_ToDoHistory
        ;
    -- Replace#SelectAtTableResultTemplate.[history].[ToDoHistory] --

    -- Replace=SelectAtTableResultTemplate.[history].[UserHistory] --
    SELECT
            [OperationId] = [OperationId],
            [UserId] = [UserId],
            [UserName] = [UserName],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [ValidFrom] = [ValidFrom],
            [ValidTo] = [ValidTo],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            @Result_history_UserHistory
        ;
    -- Replace#SelectAtTableResultTemplate.[history].[UserHistory] --

    -- Replace=SelectColumnsParameterPKTempate.[dbo].[Operation] --
    [CreatedAt],
    [OperationId]
    -- Replace#SelectColumnsParameterPKTempate.[dbo].[Operation] --

    -- Replace=SelectColumnsParameterPKTempate.[dbo].[Project] --
    [ProjectId]
    -- Replace#SelectColumnsParameterPKTempate.[dbo].[Project] --

    -- Replace=SelectColumnsParameterPKTempate.[dbo].[RequestLog] --
    [RequestLogId]
    -- Replace#SelectColumnsParameterPKTempate.[dbo].[RequestLog] --

    -- Replace=SelectColumnsParameterPKTempate.[dbo].[ToDo] --
    [ProjectId],
    [ToDoId]
    -- Replace#SelectColumnsParameterPKTempate.[dbo].[ToDo] --

    -- Replace=SelectColumnsParameterPKTempate.[dbo].[User] --
    [UserId]
    -- Replace#SelectColumnsParameterPKTempate.[dbo].[User] --

    -- Replace=SelectColumnsParameterPKTempate.[history].[ProjectHistory] --
    [ValidTo],
    [ValidFrom],
    [OperationId],
    [ProjectId]
    -- Replace#SelectColumnsParameterPKTempate.[history].[ProjectHistory] --

    -- Replace=SelectColumnsParameterPKTempate.[history].[ToDoHistory] --
    [ValidTo],
    [ValidFrom],
    [OperationId],
    [ProjectId],
    [ToDoId]
    -- Replace#SelectColumnsParameterPKTempate.[history].[ToDoHistory] --

    -- Replace=SelectColumnsParameterPKTempate.[history].[UserHistory] --
    [ValidTo],
    [ValidFrom],
    [OperationId],
    [UserId]
    -- Replace#SelectColumnsParameterPKTempate.[history].[UserHistory] --

    -- Replace=SelectPKTempateBody.[dbo].[Operation] --
    SELECT TOP(1)
            [OperationId] = [OperationId],
            [OperationName] = [OperationName],
            [EntityType] = [EntityType],
            [EntityId] = [EntityId],
            [CreatedAt] = [CreatedAt],
            [UserId] = [UserId],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [dbo].[Operation]
        WHERE
            (@CreatedAt = [CreatedAt])
             AND (@OperationId = [OperationId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[Operation] --

    -- Replace=SelectPKTempateBody.[dbo].[Project] --
    SELECT TOP(1)
            [ProjectId] = [ProjectId],
            [Title] = [Title],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [dbo].[Project]
        WHERE
            (@ProjectId = [ProjectId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[Project] --

    -- Replace=SelectPKTempateBody.[dbo].[RequestLog] --
    SELECT TOP(1)
            [RequestLogId] = [RequestLogId],
            [OperationId] = [OperationId],
            [ActivityId] = [ActivityId],
            [OperationName] = [OperationName],
            [EntityType] = [EntityType],
            [EntityId] = [EntityId],
            [Argument] = [Argument],
            [CreatedAt] = [CreatedAt],
            [UserId] = [UserId],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [dbo].[RequestLog]
        WHERE
            (@RequestLogId = [RequestLogId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[RequestLog] --

    -- Replace=SelectPKTempateBody.[dbo].[ToDo] --
    SELECT TOP(1)
            [ProjectId] = [ProjectId],
            [ToDoId] = [ToDoId],
            [UserId] = [UserId],
            [Title] = [Title],
            [Done] = [Done],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [dbo].[ToDo]
        WHERE
            (@ProjectId = [ProjectId])
             AND (@ToDoId = [ToDoId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[ToDo] --

    -- Replace=SelectPKTempateBody.[dbo].[User] --
    SELECT TOP(1)
            [UserId] = [UserId],
            [UserName] = [UserName],
            [OperationId] = [OperationId],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [dbo].[User]
        WHERE
            (@UserId = [UserId])
        ;
    -- Replace#SelectPKTempateBody.[dbo].[User] --

    -- Replace=SelectPKTempateBody.[history].[ProjectHistory] --
    SELECT TOP(1)
            [OperationId] = [OperationId],
            [ProjectId] = [ProjectId],
            [Title] = [Title],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [ValidFrom] = [ValidFrom],
            [ValidTo] = [ValidTo],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [history].[ProjectHistory]
        WHERE
            (@ValidTo = [ValidTo])
             AND (@ValidFrom = [ValidFrom])
             AND (@OperationId = [OperationId])
             AND (@ProjectId = [ProjectId])
        ;
    -- Replace#SelectPKTempateBody.[history].[ProjectHistory] --

    -- Replace=SelectPKTempateBody.[history].[ToDoHistory] --
    SELECT TOP(1)
            [OperationId] = [OperationId],
            [ProjectId] = [ProjectId],
            [ToDoId] = [ToDoId],
            [UserId] = [UserId],
            [Title] = [Title],
            [Done] = [Done],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [ValidFrom] = [ValidFrom],
            [ValidTo] = [ValidTo],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [history].[ToDoHistory]
        WHERE
            (@ValidTo = [ValidTo])
             AND (@ValidFrom = [ValidFrom])
             AND (@OperationId = [OperationId])
             AND (@ProjectId = [ProjectId])
             AND (@ToDoId = [ToDoId])
        ;
    -- Replace#SelectPKTempateBody.[history].[ToDoHistory] --

    -- Replace=SelectPKTempateBody.[history].[UserHistory] --
    SELECT TOP(1)
            [OperationId] = [OperationId],
            [UserId] = [UserId],
            [UserName] = [UserName],
            [CreatedAt] = [CreatedAt],
            [CreatedBy] = [CreatedBy],
            [ModifiedAt] = [ModifiedAt],
            [ModifiedBy] = [ModifiedBy],
            [ValidFrom] = [ValidFrom],
            [ValidTo] = [ValidTo],
            [EntityVersion] = CAST([EntityVersion] AS BIGINT)
        FROM
            [history].[UserHistory]
        WHERE
            (@ValidTo = [ValidTo])
             AND (@ValidFrom = [ValidFrom])
             AND (@OperationId = [OperationId])
             AND (@UserId = [UserId])
        ;
    -- Replace#SelectPKTempateBody.[history].[UserHistory] --

    -- Replace=SelectTableColumns.[dbo].[Operation] --
    [OperationId] = [OperationId],
    [OperationName] = [OperationName],
    [EntityType] = [EntityType],
    [EntityId] = [EntityId],
    [CreatedAt] = [CreatedAt],
    [UserId] = [UserId],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[dbo].[Operation] --

    -- Replace=SelectTableColumns.[dbo].[Project] --
    [ProjectId] = [ProjectId],
    [Title] = [Title],
    [OperationId] = [OperationId],
    [CreatedAt] = [CreatedAt],
    [CreatedBy] = [CreatedBy],
    [ModifiedAt] = [ModifiedAt],
    [ModifiedBy] = [ModifiedBy],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[dbo].[Project] --

    -- Replace=SelectTableColumns.[dbo].[RequestLog] --
    [RequestLogId] = [RequestLogId],
    [OperationId] = [OperationId],
    [ActivityId] = [ActivityId],
    [OperationName] = [OperationName],
    [EntityType] = [EntityType],
    [EntityId] = [EntityId],
    [Argument] = [Argument],
    [CreatedAt] = [CreatedAt],
    [UserId] = [UserId],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[dbo].[RequestLog] --

    -- Replace=SelectTableColumns.[dbo].[ToDo] --
    [ProjectId] = [ProjectId],
    [ToDoId] = [ToDoId],
    [UserId] = [UserId],
    [Title] = [Title],
    [Done] = [Done],
    [OperationId] = [OperationId],
    [CreatedAt] = [CreatedAt],
    [CreatedBy] = [CreatedBy],
    [ModifiedAt] = [ModifiedAt],
    [ModifiedBy] = [ModifiedBy],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[dbo].[ToDo] --

    -- Replace=SelectTableColumns.[dbo].[User] --
    [UserId] = [UserId],
    [UserName] = [UserName],
    [OperationId] = [OperationId],
    [CreatedAt] = [CreatedAt],
    [CreatedBy] = [CreatedBy],
    [ModifiedAt] = [ModifiedAt],
    [ModifiedBy] = [ModifiedBy],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[dbo].[User] --

    -- Replace=SelectTableColumns.[history].[ProjectHistory] --
    [OperationId] = [OperationId],
    [ProjectId] = [ProjectId],
    [Title] = [Title],
    [CreatedAt] = [CreatedAt],
    [CreatedBy] = [CreatedBy],
    [ModifiedAt] = [ModifiedAt],
    [ModifiedBy] = [ModifiedBy],
    [ValidFrom] = [ValidFrom],
    [ValidTo] = [ValidTo],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[history].[ProjectHistory] --

    -- Replace=SelectTableColumns.[history].[ToDoHistory] --
    [OperationId] = [OperationId],
    [ProjectId] = [ProjectId],
    [ToDoId] = [ToDoId],
    [UserId] = [UserId],
    [Title] = [Title],
    [Done] = [Done],
    [CreatedAt] = [CreatedAt],
    [CreatedBy] = [CreatedBy],
    [ModifiedAt] = [ModifiedAt],
    [ModifiedBy] = [ModifiedBy],
    [ValidFrom] = [ValidFrom],
    [ValidTo] = [ValidTo],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[history].[ToDoHistory] --

    -- Replace=SelectTableColumns.[history].[UserHistory] --
    [OperationId] = [OperationId],
    [UserId] = [UserId],
    [UserName] = [UserName],
    [CreatedAt] = [CreatedAt],
    [CreatedBy] = [CreatedBy],
    [ModifiedAt] = [ModifiedAt],
    [ModifiedBy] = [ModifiedBy],
    [ValidFrom] = [ValidFrom],
    [ValidTo] = [ValidTo],
    [EntityVersion] = CAST([EntityVersion] AS BIGINT)
    -- Replace#SelectTableColumns.[history].[UserHistory] --

    -- Replace=TableColumnsAsParameter.[dbo].[Operation] --
    @OperationId uniqueidentifier,
    @OperationName varchar(100),
    @EntityType varchar(50),
    @EntityId nvarchar(100),
    @CreatedAt datetimeoffset,
    @UserId uniqueidentifier
    -- Replace#TableColumnsAsParameter.[dbo].[Operation] --

    -- Replace=TableColumnsAsParameter.[dbo].[Project] --
    @ProjectId uniqueidentifier,
    @Title nvarchar(50),
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier
    -- Replace#TableColumnsAsParameter.[dbo].[Project] --

    -- Replace=TableColumnsAsParameter.[dbo].[RequestLog] --
    @RequestLogId uniqueidentifier,
    @OperationId uniqueidentifier,
    @ActivityId varchar(200),
    @OperationName varchar(100),
    @EntityType varchar(50),
    @EntityId nvarchar(100),
    @Argument nvarchar(MAX),
    @CreatedAt datetimeoffset,
    @UserId uniqueidentifier
    -- Replace#TableColumnsAsParameter.[dbo].[RequestLog] --

    -- Replace=TableColumnsAsParameter.[dbo].[ToDo] --
    @ProjectId uniqueidentifier,
    @ToDoId uniqueidentifier,
    @UserId uniqueidentifier,
    @Title nvarchar(50),
    @Done bit,
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier
    -- Replace#TableColumnsAsParameter.[dbo].[ToDo] --

    -- Replace=TableColumnsAsParameter.[dbo].[User] --
    @UserId uniqueidentifier,
    @UserName nvarchar(50),
    @OperationId uniqueidentifier,
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier
    -- Replace#TableColumnsAsParameter.[dbo].[User] --

    -- Replace=TableColumnsAsParameter.[history].[ProjectHistory] --
    @OperationId uniqueidentifier,
    @ProjectId uniqueidentifier,
    @Title nvarchar(50),
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @ValidFrom datetimeoffset,
    @ValidTo datetimeoffset
    -- Replace#TableColumnsAsParameter.[history].[ProjectHistory] --

    -- Replace=TableColumnsAsParameter.[history].[ToDoHistory] --
    @OperationId uniqueidentifier,
    @ProjectId uniqueidentifier,
    @ToDoId uniqueidentifier,
    @UserId uniqueidentifier,
    @Title nvarchar(50),
    @Done bit,
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @ValidFrom datetimeoffset,
    @ValidTo datetimeoffset
    -- Replace#TableColumnsAsParameter.[history].[ToDoHistory] --

    -- Replace=TableColumnsAsParameter.[history].[UserHistory] --
    @OperationId uniqueidentifier,
    @UserId uniqueidentifier,
    @UserName nvarchar(50),
    @CreatedAt datetimeoffset,
    @CreatedBy uniqueidentifier,
    @ModifiedAt datetimeoffset,
    @ModifiedBy uniqueidentifier,
    @ValidFrom datetimeoffset,
    @ValidTo datetimeoffset
    -- Replace#TableColumnsAsParameter.[history].[UserHistory] --


-- Replace#SNIPPETS --
*/