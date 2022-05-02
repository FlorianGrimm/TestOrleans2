#if true

namespace Replacement.Contracts.API {
    public sealed partial record __EFMigrationsHistoryPK (
        string MigrationId
        ) : IPrimaryKey;

    public sealed partial record OperationPK (
        System.DateTimeOffset CreatedAt,
        Guid OperationId
        ) : IPrimaryKey;

    public sealed partial record OrleansMembershipTablePK (
        string DeploymentId,
        string Address,
        int Port,
        int Generation
        ) : IPrimaryKey;

    public sealed partial record OrleansMembershipVersionTablePK (
        string DeploymentId
        ) : IPrimaryKey;

    public sealed partial record OrleansQueryPK (
        string QueryKey
        ) : IPrimaryKey;

    public sealed partial record OrleansRemindersTablePK (
        string ServiceId,
        string GrainId,
        string ReminderName
        ) : IPrimaryKey;

    public sealed partial record ProjectPK (
        Guid ProjectId
        ) : IPrimaryKey;

    public sealed partial record ToDoPK (
        Guid ProjectId,
        Guid ToDoId
        ) : IPrimaryKey;

    public sealed partial record UserPK (
        Guid UserId
        ) : IPrimaryKey;

    public sealed partial record ProjectHistoryPK (
        System.DateTimeOffset ValidTo,
        System.DateTimeOffset ValidFrom,
        Guid OperationId,
        Guid ProjectId
        ) : IPrimaryKey;

    public sealed partial record ToDoHistoryPK (
        System.DateTimeOffset ValidTo,
        System.DateTimeOffset ValidFrom,
        Guid OperationId,
        Guid ProjectId,
        Guid ToDoId
        ) : IPrimaryKey;

    public sealed partial record UserHistoryPK (
        System.DateTimeOffset ValidTo,
        System.DateTimeOffset ValidFrom,
        Guid OperationId,
        Guid UserId
        ) : IPrimaryKey;

}

#endif
