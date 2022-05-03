namespace Replacement.Contracts.API;
public record class OperationAPI(
    Guid OperationId,
    string OperationName,
    string EntityType,
    string EntityId,
    Guid? UserId,
    DateTimeOffset CreatedAt,
    long SerialVersion
);

public record class RequestLogAPI(
    Guid RequestLogId,
    Guid OperationId,
    string ActivityId,
    string OperationName,
    string EntityType,
    string EntityId,
    string? Argument,
    Guid? UserId,
    DateTimeOffset CreatedAt,
    long SerialVersion
);

public record class RequestOperationAPI(
    Guid RequestLogId,
    Guid OperationId,
    string ActivityId,
    string OperationName,
    string EntityType,
    string EntityId,
    string? Argument,
    string? UserName,
    Guid? UserId,
    DateTimeOffset CreatedAt,
    long SerialVersion
);
