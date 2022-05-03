namespace Replacement.Contracts.API;

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
