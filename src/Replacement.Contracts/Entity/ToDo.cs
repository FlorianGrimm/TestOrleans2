namespace Replacement.Contracts.Entity;
/*
    public partial class ToDo {
        [Key]
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
        [StringLength(50)]
        public string Title { get; set; } = null!;
        public bool Done { get; set; }
        public Guid? OperationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public long SerialVersion { get; set; }

        [ForeignKey("ModifiedAt,OperationId")]
        [InverseProperty("ToDo")]
        public virtual Operation? Operation { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("ToDo")]
        public virtual Project? Project { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ToDo")]
        public virtual User? User { get; set; }
    }
*/
public record class ToDoEntity(
    // [property: Key]
    Guid ToDoId,
    // [property: Key]
    Guid ProjectId,
    Guid UserId,
    // [property: StringLength(50)]
    string Title,
    bool Done,
    Guid OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    long SerialVersion
) : IDataOperationRelated {
    public ToDoPK GetPrimaryKey() => new ToDoPK(this.ProjectId, this.ToDoId);
    public OperationPK GetOperationPK() => new OperationPK(this.ModifiedAt, this.OperationId);
    public ProjectPK GetProjectPK() => new ProjectPK(this.ProjectId);
    public UserPK? GetCreatedByUserPK() => this.CreatedBy.HasValue ? new UserPK(this.CreatedBy.Value) : null;
    public UserPK? GetModifiedByUserPK() => this.ModifiedBy.HasValue ? new UserPK(this.ModifiedBy.Value) : null;

    public ToDoEntity SetOperation(OperationEntity value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = this.SerialVersion == 0 ? value.CreatedAt : this.CreatedAt,
            CreatedBy = this.SerialVersion == 0 ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }

    public ToDoEntity SetProject(ProjectPK value) {
        return this with {
            ProjectId = value.ProjectId
        };
    }

}

partial class ConverterToAPI {
    [return: NotNullIfNotNull("that")]
    public static ToDoAPI? ToToDoAPI(this ToDoEntity? that) {
        if (that is null) {
            return default;
        } else {
            return new ToDoAPI(
                ToDoId: that.ToDoId,
                ProjectId: that.ProjectId,
                UserId: that.UserId,
                Title: that.Title,
                Done: that.Done,
                OperationId: that.OperationId,
                CreatedAt: that.CreatedAt,
                CreatedBy: that.CreatedBy,
                ModifiedAt: that.ModifiedAt,
                ModifiedBy: that.ModifiedBy,
                SerialVersion: that.SerialVersion
                );
        }
    }

    public static List<ToDoAPI> ToListToDoAPI(
        this IEnumerable<ToDoEntity> that) {
        var result = new List<ToDoAPI>();
        foreach (var e in that) {
            if (e is not null) {
                var a = e.ToToDoAPI();
                result.Add(a);
            }
        }
        return result;
    }
}
