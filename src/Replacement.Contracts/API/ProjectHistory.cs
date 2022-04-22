namespace Replacement.Contracts.API;
/*
    [Table("ProjectHistory", Schema = "history")]
    public partial class ProjectHistory {
        [Key]
        public Guid ActivityId { get; set; }
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string Title { get; set; } = null!;
        [Key]
        public DateTimeOffset ValidFrom { get; set; }
        [Key]
        public DateTimeOffset ValidTo { get; set; }
        public long SerialVersion { get; set; }

        [ForeignKey("ValidFrom,ActivityId")]
        [InverseProperty("ProjectHistory")]
        public virtual Activity Activity { get; set; } = null!;
    }
*/

[Table("ProjectHistory", Schema = "history")]
public record class ProjectHistory(
        [property:Key]
        Guid ActivityId,
        [property:Key]
        Guid Id,
        [property:StringLength(50)]
        string Title, 
        [property:Key]
        DateTimeOffset ValidFrom,
        [property:Key]
        DateTimeOffset ValidTo,
        long SerialVersion
);
        