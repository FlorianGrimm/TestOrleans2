namespace Replacement.Contracts.Entity;

public record class RequestOperationEntity(
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
