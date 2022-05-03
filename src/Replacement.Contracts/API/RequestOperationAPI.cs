namespace Replacement.Contracts.API;

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
