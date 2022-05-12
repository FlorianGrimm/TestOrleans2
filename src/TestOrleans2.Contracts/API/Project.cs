using Brimborium.RowVersion.Extensions;

namespace TestOrleans2.Contracts.API;
public record class Project(
    Guid ProjectId,
    string Title,
    Guid OperationId,
    DateTimeOffset CreatedAt,
    Guid? CreatedBy,
    DateTimeOffset ModifiedAt,
    Guid? ModifiedBy,
    string DataVersion
) : IOperationRelatedAPI {
    public ProjectPK GetPrimaryKey() => new ProjectPK(this.ProjectId);
    public OperationPK GetOperationPK() => new OperationPK(this.ModifiedAt, this.OperationId);
    public UserPK? GetCreatedByUserPK() => this.CreatedBy.HasValue ? new UserPK(this.CreatedBy.Value) : null;
    public UserPK? GetModifiedByUserPK() => this.ModifiedBy.HasValue ? new UserPK(this.ModifiedBy.Value) : null;

    public Project SetOperation(Operation value) {
        return this with {
            OperationId = value.OperationId,
            CreatedAt = DataVersionExtensions.DataVersionIsEmptyOrZero(this.DataVersion) ? value.CreatedAt : this.CreatedAt,
            CreatedBy = DataVersionExtensions.DataVersionIsEmptyOrZero(this.DataVersion) ? value.UserId : this.CreatedBy,
            ModifiedAt = value.CreatedAt,
            ModifiedBy = value.UserId
        };
    }
}