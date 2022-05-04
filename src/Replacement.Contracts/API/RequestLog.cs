namespace Replacement.Contracts.API;

public record class RequestLog(
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
) : IDataAPI;
