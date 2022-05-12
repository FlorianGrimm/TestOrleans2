namespace Replacement.Contracts.Entity;
/*
    [Table("ProjectHistory", Schema = "history")]
    public partial class ProjectHistory {
        [Key]
        public Guid OperationId { get; set; }
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string Title { get; set; } = null!;
        [Key]
        public DateTimeOffset ValidFrom { get; set; }
        [Key]
        public DateTimeOffset ValidTo { get; set; }
        public long DataVersion { get; set; }

        [ForeignKey("ValidFrom,OperationId")]
        [InverseProperty("ProjectHistory")]
        public virtual Operation Operation { get; set; } = null!;
    }
*/

[Table("ProjectHistory", Schema = "history")]
public record class ProjectHistoryEntity(
    // [property:Key]
    Guid OperationId,
    // [property:Key]
    Guid ProjectId,
    // [property:StringLength(50)]
    string Title,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    // [property:Key]
    DateTimeOffset ValidFrom,
    // [property:Key]
    DateTimeOffset ValidTo,
    long EntityVersion
) : IHistoryEntity;