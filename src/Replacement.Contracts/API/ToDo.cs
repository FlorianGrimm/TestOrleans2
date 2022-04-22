namespace Replacement.Contracts.API;
/*
    public partial class ToDo {
        [Key]
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
        [StringLength(50)]
        public string Title { get; set; } = null!;
        public bool Done { get; set; }
        public Guid? ActivityId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public long SerialVersion { get; set; }

        [ForeignKey("ModifiedAt,ActivityId")]
        [InverseProperty("ToDo")]
        public virtual Activity? Activity { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("ToDo")]
        public virtual Project? Project { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ToDo")]
        public virtual User? User { get; set; }
    }
*/
public record class ToDo(
        [property: Key]
        Guid Id,
        Guid? ProjectId,
        Guid? UserId,
        [property: StringLength(50)]
        string Title,
        bool Done,
        Guid? ActivityId,
        DateTimeOffset CreatedAt,
        DateTimeOffset ModifiedAt,
        long SerialVersion
    );