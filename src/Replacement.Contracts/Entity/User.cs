namespace Replacement.Contracts.Entity;
/*
    public partial class User {
        public User() {
            this.ToDo = new HashSet<ToDo>();
        }

        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string UserName { get; set; } = null!;
        public Guid? OperationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public long DataVersion { get; set; }

        [ForeignKey("ModifiedAt,OperationId")]
        [InverseProperty("User")]
        public virtual Operation? Operation { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ToDo> ToDo { get; set; }
    }
*/
public record class UserEntity(
    // [property:Key]
    Guid UserId,
    // [property: StringLength(50)]
    string UserName,
    Guid OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    long EntityVersion
) : IOperationRelatedEntity {
    public UserPK GetPrimaryKey() => new UserPK(this.UserId);
    public UserPK? GetCreatedByUserPK() => this.CreatedBy.HasValue ? new UserPK(this.CreatedBy.Value) : null;
    public UserPK? GetModifiedByUserPK() => this.ModifiedBy.HasValue ? new UserPK(this.ModifiedBy.Value) : null;

    public static UserEntity Create(
        OperationEntity operation,
        Guid userId,
        string userName
        ) {
        return new UserEntity(
            UserId: userId,
            UserName: userName,
            OperationId: operation.OperationId,
            CreatedAt: operation.CreatedAt,
            CreatedBy: operation.UserId,
            ModifiedAt: operation.CreatedAt,
            ModifiedBy: operation.UserId,
            EntityVersion: 0
            );
    }
    public UserEntity SetOperation(OperationEntity value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = this.EntityVersion == 0 ? value.CreatedAt : this.CreatedAt,
            CreatedBy = this.EntityVersion == 0 ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }

    public User ToAPI() {
        throw new NotImplementedException();
    }
}

public record class UserSelectByUserNameArg(
    [property: StringLength(50)]
    string UserName
    );