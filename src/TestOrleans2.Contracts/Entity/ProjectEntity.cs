namespace TestOrleans2.Contracts.Entity;

public record class ProjectEntity(
    // [property: Key]
    Guid ProjectId,
    // [property: StringLength(50)]
    string Title,
    Guid OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    long EntityVersion
) : IOperationRelatedEntity {
    public ProjectPK GetPrimaryKey() => new ProjectPK(this.ProjectId);
    public OperationPK GetOperationPK() => new OperationPK(this.ModifiedAt, this.OperationId);
    public UserPK? GetCreatedByUserPK() => this.CreatedBy.HasValue ? new UserPK(this.CreatedBy.Value) : null;
    public UserPK? GetModifiedByUserPK() => this.ModifiedBy.HasValue ? new UserPK(this.ModifiedBy.Value) : null;

    public ProjectEntity SetOperation(OperationEntity value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = this.EntityVersion == 0 ? value.CreatedAt : this.CreatedAt,
            CreatedBy = this.EntityVersion == 0 ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }
}

public record class ProjectSelectPKResult(
    List<ProjectEntity> Projects,
    List<ToDoEntity> ToDos
    );
