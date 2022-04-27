namespace Replacement.Contracts.API;
/*
public partial class Operation {
    public Operation() {
        this.Project = new HashSet<Project>();
        this.ProjectHistory = new HashSet<ProjectHistory>();
        this.ToDo = new HashSet<ToDo>();
        this.ToDoHistory = new HashSet<ToDoHistory>();
        this.User = new HashSet<User>();
        this.UserHistory = new HashSet<UserHistory>();
    }

    [Key]
    public Guid Id { get; set; }
    [StringLength(100)]
    public string Title { get; set; } = null!;
    [StringLength(100)]
    public string EntityType { get; set; } = null!;
    [StringLength(100)]
    public string EntityId { get; set; } = null!;
    public string? Data { get; set; }
    [Key]
    public DateTimeOffset CreatedAt { get; set; }
    public long SerialVersion { get; set; }

    [InverseProperty("Operation")]
    public virtual ICollection<Project> Project { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<ProjectHistory> ProjectHistory { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<ToDo> ToDo { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<ToDoHistory> ToDoHistory { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<User> User { get; set; }
    [InverseProperty("Operation")]
    public virtual ICollection<UserHistory> UserHistory { get; set; }
}
*/
public record class Operation(
    Guid OperationId,
    // [property:StringLength(100)]
    string Title,
    // [property: StringLength(100)]
    string EntityType,
    // [property: StringLength(100)]
    string EntityId,
    string? Data,
    Guid? UserId,
    // [property: Key]
    DateTimeOffset CreatedAt,
    long SerialVersion
);
