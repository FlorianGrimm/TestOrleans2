﻿namespace Replacement.Contracts.Entity;
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
    long SerialVersion
) : IDataOperationRelated {
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
            SerialVersion: 0
            );
    }
    public UserEntity SetOperation(OperationEntity value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = this.SerialVersion == 0 ? value.CreatedAt : this.CreatedAt,
            CreatedBy = this.SerialVersion == 0 ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }

    public UserAPI ToAPI() {
        throw new NotImplementedException();
    }
}

partial class ConverterToAPI {
    [return:NotNullIfNotNull("that")]
    public static UserAPI? ToUserAPI(this UserEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new UserAPI(
                UserId: that.UserId,
                UserName: that.UserName,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                SerialVersion: that.SerialVersion
                );
        }
    }
}

public record class UserSelectByUserNameArg(
    [property: StringLength(50)]
    string UserName
    );