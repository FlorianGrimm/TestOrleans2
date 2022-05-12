using TestOrleans2.Contracts.Entity;

namespace TestOrleans2.Repository.Service;

public interface IDBContext : ITrackingContext {
    ITrackingSet<OperationPK, OperationEntity> Operation { get; }
    ITrackingSet<UserPK, UserEntity> User { get; }
    ITrackingSet<ProjectPK, ProjectEntity> Project { get; }
    ITrackingSet<ToDoPK, ToDoEntity> ToDo { get; }

    Task<ISqlAccess> GetDataAccessAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task ApplyChangesAsync(ISqlAccess? sqlAccess = default, CancellationToken cancellationToken = default(CancellationToken));
    void Clear();
}