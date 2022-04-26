namespace Replacement.Contracts.API;
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
        public long SerialVersion { get; set; }

        [ForeignKey("ModifiedAt,OperationId")]
        [InverseProperty("User")]
        public virtual Operation? Operation { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ToDo> ToDo { get; set; }
    }
*/
public record class User(
    [property:Key]
    Guid UserId,
    [property: StringLength(50)]
    string UserName,
    Guid? OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    long SerialVersion
) : IDataOperationRelated {
    public UserPK GetPrimaryKey() => new UserPK(this.UserId);

    public User SetOperation(Operation value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = (this.SerialVersion == 0) ? value.CreatedAt : this.CreatedAt,
            CreatedBy = (this.SerialVersion == 0) ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }
}


public record class UserSelectByUserNameArg(
    [property: StringLength(50)]
    string UserName
    );