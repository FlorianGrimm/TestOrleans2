namespace Replacement.Contracts.Service;

public record class RequestOperation(
    Guid RequestLogId,
    Guid OperationId,
    // [property:StringLength(200)]
    string ActivityId,
    // [property:StringLength(100)]
    string OperationName,
    // [property: StringLength(50)]
    string EntityType,
    // [property: StringLength(100)]
    string EntityId,
    string? Argument,
    string? UserName,
    Guid? UserId,
    // [property: Key]
    DateTimeOffset CreatedAt,
    long SerialVersion
);
