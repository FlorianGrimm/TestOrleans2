namespace Replacement.Contracts.Entity;

public record class RequestLogEntity(
    Guid RequestLogId,
    Guid OperationId,
    string ActivityId,
    string OperationName,
    string EntityType,
    string EntityId,
    string? Argument,
    Guid? UserId,
    DateTimeOffset CreatedAt,
    long EntityVersion
) : IDataEntity;
