namespace Replacement.Contracts.API;

public record class RequestLogFilter(
    Guid? RequestLogId,
    Guid? OperationId,
    string? ActivityId,
    string? OperationName,
    string? EntityType,
    string? EntityId,
    string? Argument,
    Guid? UserId,
    DateTimeOffset? CreatedAtLow,
    DateTimeOffset? CreatedAtHigh
);
