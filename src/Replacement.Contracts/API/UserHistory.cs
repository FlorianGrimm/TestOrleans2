namespace Replacement.Contracts.API;
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
public record class UserHistory(
    [property:Key]
    Guid OperationId,
    [property: Key]
    Guid Id,
    [property: StringLength(50)]
    string UserName,
    [property: Key]
    DateTimeOffset ValidFrom,
    [property: Key]
    DateTimeOffset ValidTo,
    long SerialVersion
);