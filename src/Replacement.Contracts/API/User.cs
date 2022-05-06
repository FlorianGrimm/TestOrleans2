namespace Replacement.Contracts.API;
public record class User(
    Guid UserId,
    string UserName,
    Guid OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    long DataVersion
) : IOperationRelatedAPI {
    public UserPK GetPrimaryKey() => new UserPK(this.UserId);
    public UserPK? GetCreatedByUserPK() => this.CreatedBy.HasValue ? new UserPK(this.CreatedBy.Value) : null;
    public UserPK? GetModifiedByUserPK() => this.ModifiedBy.HasValue ? new UserPK(this.ModifiedBy.Value) : null;

    public static User Create(
        Operation operation,
        Guid userId,
        string userName
        ) {
        return new User(
            UserId: userId,
            UserName: userName,
            OperationId: operation.OperationId,
            CreatedAt: operation.CreatedAt,
            CreatedBy: operation.UserId,
            ModifiedAt: operation.CreatedAt,
            ModifiedBy: operation.UserId,
            DataVersion: 0
            );
    }
    public User SetOperation(Operation value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = this.DataVersion == 0 ? value.CreatedAt : this.CreatedAt,
            CreatedBy = this.DataVersion == 0 ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }
}
