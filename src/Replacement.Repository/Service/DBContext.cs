namespace Replacement.Repository.Service;

#warning weichei

public class TodoRepo {
    private static Dictionary<Guid, ToDo>? _Todos;
    internal static Dictionary<Guid, ToDo> GetTodos() {
        if (_Todos == null) {
            _Todos = new Dictionary<Guid, ToDo>();
            var item = new ToDo(
                Id: new Guid("9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A"),
                ProjectId: new Guid("9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A"),
                UserId: new Guid("9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A"),
                Title: "1",
                Done: false,
                ActivityId: new Guid("9C4490D6-9FC9-4A91-A3C1-98D5CE9A7B7A"),
                CreatedAt: DateTimeOffset.Now,
                ModifiedAt: DateTimeOffset.Now,
                SerialVersion: 1
                );
            _Todos.Add(item.Id, item);
        }
        return _Todos;

    }
}

[Transient]
public class DBContext : Brimborium.Tracking.TrackingContext, IDBContext {
    private string? _ConnectionString;

    public ITrackingSet<ActivityPK, Activity> Activity { get; }
    public ITrackingSet<UserPK, User> User { get; }
    public ITrackingSet<ProjectPK, Project> Project { get; }
    public ITrackingSet<ToDoPK, ToDo> Todo { get; }

    public string? ConnectionString { get => this._ConnectionString; set => this._ConnectionString = value; }

    public DBContext(IOptions<DBContextOption> options) {
        this._ConnectionString = options.Value.ConnectionString;
        this.Activity = new TrackingSetActivity(this, TrackingSetApplyChangesActivity.Instance);
        this.User = new TrackingSetUser(this, TrackingSetApplyChangesUser.Instance);
        this.Project = new TrackingSetProject(this, TrackingSetApplyChangesProject.Instance);
        this.Todo = new TrackingSetToDo(this, TrackingSetApplyChangesToDo.Instance);
    }

    public SqlAccess GetSqlAccess() {
        if (string.IsNullOrEmpty(this._ConnectionString)) { throw new InvalidOperationException("ConnectionString is empty"); }
        return new SqlAccess(this._ConnectionString);
    }

    public async Task ApplyChangesAsync() {
        using var sqlAccess = this.GetSqlAccess();
        var sqlAccessConnection = new TrackingSqlAccessConnection(this.GetSqlAccess());
        await this.ApplyChangesAsync(sqlAccessConnection);
    }

    public async Task<ToDo[]> ReadAllTodoAsync() {
        using var sqlAccess = this.GetSqlAccess();
        var result = new List<ToDo>();
#warning TODO
        var item = await sqlAccess.ExecuteToDoSelectPKAsync(null!, null);
        if (item is not null) {
            result.Add(item);
        }
        return this.Todo.AttachRange(result).Select(to => to.Value).ToArray();
    }

    public Task<ToDo?> ReadTodoAsync(Guid id) {
        TodoRepo.GetTodos().TryGetValue(id, out var result);
        return Task.FromResult(result);
    }

}

public class DBContextOption {
    public string? ConnectionString { get; set; }
}
