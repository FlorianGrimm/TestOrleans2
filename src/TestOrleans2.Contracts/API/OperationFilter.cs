namespace TestOrleans2.Contracts.API;

public record class OperationFilter(
    Guid? OperationId,
    string? OperationName,
    string? EntityType,
    string? EntityId,
    Guid? UserId,
    DateTimeOffset? CreatedAtLow,
    DateTimeOffset? CreatedAtHigh
);