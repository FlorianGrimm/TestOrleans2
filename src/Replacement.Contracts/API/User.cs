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
        public Guid? ActivityId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public long SerialVersion { get; set; }

        [ForeignKey("ModifiedAt,ActivityId")]
        [InverseProperty("User")]
        public virtual Activity? Activity { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ToDo> ToDo { get; set; }
    }
*/
public record class User(
    [property:Key]
    Guid Id,
    [property: StringLength(50)]
    string UserName,
    Guid? ActivityId,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    long SerialVersion
);