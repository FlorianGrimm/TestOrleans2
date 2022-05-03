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
        public long SerialVersion { get; set; }

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
    long SerialVersion
) : IDataHistory;

partial class ConverterToAPI {
    [return: NotNullIfNotNull("that")]
    public static ProjectHistoryAPI? ToProjectHistoryAPI(this ProjectHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ProjectHistoryAPI(
                OperationId:that.OperationId,
                ProjectId:that.ProjectId,
                Title:that.Title,
                CreatedAt:that.CreatedAt,
                CreatedBy:that.CreatedBy,
                ModifiedAt:that.ModifiedAt,
                ModifiedBy:that.ModifiedBy,
                ValidFrom:that.ValidFrom,
                ValidTo:that.ValidTo,
                SerialVersion: that.SerialVersion
                );
        }
    }
}
