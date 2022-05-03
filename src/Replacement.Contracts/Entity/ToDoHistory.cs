namespace Replacement.Contracts.Entity;
/*
    [Table("ToDoHistory", Schema = "history")]
    public partial class ToDoHistory {
        [Key]
        public Guid OperationId { get; set; }
        [Key]
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
        [StringLength(50)]
        public string Title { get; set; } = null!;
        public bool Done { get; set; }
        [Key]
        public DateTimeOffset ValidFrom { get; set; }
        [Key]
        public DateTimeOffset ValidTo { get; set; }
        public long SerialVersion { get; set; }

        [ForeignKey("ValidFrom,OperationId")]
        [InverseProperty("ToDoHistory")]
        public virtual Operation Operation { get; set; } = null!;
    }
*/

[Table("ToDoHistory", Schema = "history")]
public record class ToDoHistoryEntity(
    // [property:Key]
    Guid OperationId,
    // [property:Key]
    Guid ToDoId,
    Guid? ProjectId,
    Guid? UserId,
    // [property:StringLength(50)]
    string Title,
    bool Done,
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
    public static ToDoHistoryAPI? ToToDoHistoryAPI(this ToDoHistoryEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDoHistoryAPI(
                OperationId:that.OperationId,
                ToDoId:that.ToDoId,
                ProjectId:that.ProjectId,
                UserId:that.UserId,
                Title:that.Title,
                Done:that.Done,
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
