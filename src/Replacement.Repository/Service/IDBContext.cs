namespace Replacement.Repository.Service;

public interface IDBContext : ITrackingContext {
    ITrackingSet<OperationPK, Operation> Operation { get; }
    ITrackingSet<UserPK, User> User { get; }
    ITrackingSet<ProjectPK, Project> Project { get; }
    ITrackingSet<ToDoPK, ToDo> ToDo { get; }

    Task<ISqlAccess> GetDataAccessAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task ApplyChangesAsync(ISqlAccess? sqlAccess = default, CancellationToken cancellationToken = default(CancellationToken));
}