namespace Replacement.Contracts.Entity;
public record class OperationEntity(
    Guid OperationId,
    string OperationName,
    string EntityType,
    string EntityId,
    Guid? UserId,
    DateTimeOffset CreatedAt,
    long SerialVersion
) : IDataEntity {
    public static OperationEntity Create(
        Guid OperationId,
        string OperationName,
        string EntityType,
        string EntityId,
        Guid? UserId
    ) {
        return new OperationEntity(
            OperationId: OperationId,
            OperationName: OperationName,
            EntityType: EntityType,
            EntityId: EntityId,
            UserId: UserId,
            CreatedAt: DateTimeOffset.UtcNow,
            SerialVersion: 0
        );
    }
    public OperationEntity Renew() {
        return this with {
            OperationId = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            SerialVersion = 0
        };
    }
}
