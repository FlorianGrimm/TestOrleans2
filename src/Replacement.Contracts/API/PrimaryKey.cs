#if true
using System;

namespace Replacement.Contracts.API {
    public sealed record OperationPK(
        DateTimeOffset CreatedAt,
        Guid OperationId
        ) : IPrimaryKey;

    public sealed record ProjectPK(
        Guid ProjectId
        ) : IPrimaryKey;

    public sealed record ToDoPK(
        Guid ToDoId
        ) : IPrimaryKey;

    public sealed record UserPK(
        Guid UserId
        ) : IPrimaryKey;

    public sealed record ProjectHistoryPK(
        DateTimeOffset ValidTo,
        DateTimeOffset ValidFrom,
        Guid OperationId,
        Guid ProjectHistoryId
        ) : IPrimaryKey;

    public sealed record ToDoHistoryPK(
        DateTimeOffset ValidTo,
        DateTimeOffset ValidFrom,
        Guid OperationId,
        Guid ToDoId
        ) : IPrimaryKey;

    public sealed record UserHistoryPK(
        DateTimeOffset ValidTo,
        DateTimeOffset ValidFrom,
        Guid OperationId,
        Guid UserId
        ) : IPrimaryKey;
}

#endif
