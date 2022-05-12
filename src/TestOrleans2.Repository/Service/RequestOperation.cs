namespace TestOrleans2.Repository.Service;

public record class RequestOperation(
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
) {
    public static RequestOperation Create(
        Guid RequestLogId,
        Guid OperationId,
        string ActivityId,
        string OperationName,
        string EntityType,
        string EntityId,
        string? Argument,
        string? UserName,
        Guid? UserId
    ) {
        return new RequestOperation(
                RequestLogId: RequestLogId,
                OperationId: OperationId,
                ActivityId: ActivityId,
                OperationName: OperationName,
                EntityType: EntityType,
                EntityId: EntityId,
                Argument: Argument,
                UserName: UserName,
                UserId: UserId,
                CreatedAt: System.DateTimeOffset.UtcNow,
                SerialVersion: 0
            );
    }
}
