namespace Replacement.Repository.Service;

public interface IDBContext : ITrackingContext {
    //TrackingSetToDo Todo { get; }
    
    SqlAccess GetSqlAccess();
    
    ITrackingSet<ActivityPK, Activity> Activity { get; }
    ITrackingSet<UserPK, User> User { get; }
    ITrackingSet<ProjectPK, Project> Project { get; }
    ITrackingSet<ToDoPK, ToDo> Todo { get; }

    Task ApplyChangesAsync();

#warning weichei
    Task<ToDo?> ReadTodoAsync(Guid id);
    Task<ToDo[]> ReadAllTodoAsync();
    
}