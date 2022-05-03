namespace Replacement.Contracts.Entity;
/*
    [Table("UserHistory", Schema = "history")]
    public partial class UserHistory {
        [Key]
        public Guid OperationId { get; set; }
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string UserName { get; set; } = null!;
        [Key]
        public DateTimeOffset ValidFrom { get; set; }
        [Key]
        public DateTimeOffset ValidTo { get; set; }
        public long SerialVersion { get; set; }

        [ForeignKey("ValidFrom,OperationId")]
        [InverseProperty("UserHistory")]
        public virtual Operation Operation { get; set; } = null!;
    }
*/
[Table("UserHistory", Schema = "history")]
public record class UserHistoryEntity(
    // [property:Key]
    Guid OperationId,
    // [property: Key]
    Guid UserId,
    // [property: StringLength(50)]
    string UserName,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    // [property: Key]
    DateTimeOffset ValidFrom,
    // [property: Key]
    DateTimeOffset ValidTo,
    long SerialVersion
) : IDataHistory;

partial class ConverterToAPI {
    [return: NotNullIfNotNull("that")]
    public static UserHistoryAPI? ToUserHistoryAPI(this UserHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new UserHistoryAPI(
                OperationId: that.OperationId,
                UserId: that.UserId,
                UserName: that.UserName,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                ValidFrom: that.ValidFrom,
                ValidTo: that.ValidTo,
                SerialVersion: that.SerialVersion
                );
        }
    }
}
