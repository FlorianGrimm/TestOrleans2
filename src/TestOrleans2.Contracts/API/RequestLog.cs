namespace TestOrleans2.Contracts.API;

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
    string DataVersion
) : IDataAPI;
