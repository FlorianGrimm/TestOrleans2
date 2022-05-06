namespace Replacement.Contracts.API;
public record class ToDo(
    Guid ToDoId,
    Guid ProjectId,
    Guid UserId,
    string Title,
    bool Done,
    Guid OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    long DataVersion
) : IOperationRelatedAPI {
    public ToDoPK GetPrimaryKey() => new ToDoPK(this.ProjectId, this.ToDoId);
    public OperationPK GetOperationPK() => new OperationPK(this.ModifiedAt, this.OperationId);
    public ProjectPK GetProjectPK() => new ProjectPK(this.ProjectId);
    public UserPK? GetCreatedByUserPK() => this.CreatedBy.HasValue ? new UserPK(this.CreatedBy.Value) : null;
    public UserPK? GetModifiedByUserPK() => this.ModifiedBy.HasValue ? new UserPK(this.ModifiedBy.Value) : null;

    public ToDo SetOperation(OperationEntity value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = this.DataVersion == 0 ? value.CreatedAt : this.CreatedAt,
            CreatedBy = this.DataVersion == 0 ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }

    public ToDo SetProject(ProjectPK value) {
        return this with {
            ProjectId = value.ProjectId
        };
    }

}
