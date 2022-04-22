namespace Replacement.Contracts.API;
/*
    [Table("ToDoHistory", Schema = "history")]
    public partial class ToDoHistory {
        [Key]
        public Guid ActivityId { get; set; }
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

        [ForeignKey("ValidFrom,ActivityId")]
        [InverseProperty("ToDoHistory")]
        public virtual Activity Activity { get; set; } = null!;
    }
*/

[Table("ToDoHistory", Schema = "history")]
public record class ToDoHistory (
    [property:Key]
    Guid ActivityId,
    [property:Key]
    Guid Id,
    Guid? ProjectId,
    Guid? UserId,
    [property:StringLength(50)]
    string Title,
    bool Done,
    [property:Key]
    DateTimeOffset ValidFrom,
    [property:Key]
    DateTimeOffset ValidTo,
    long SerialVersion
);