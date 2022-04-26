namespace Replacement.Repository.Service;

[Transient]
public class DBContext : Brimborium.Tracking.TrackingContext, IDBContext {
    private ISqlAccessFactory _SqlAccessFactory;
    private DBContextOption _Options;

    public ITrackingSet<OperationPK, Operation> Operation { get; }
    public ITrackingSet<UserPK, User> User { get; }
    public ITrackingSet<ProjectPK, Project> Project { get; }
    public ITrackingSet<ToDoPK, ToDo> ToDo { get; }


    public DBContext(
        ISqlAccessFactory sqlAccessFactory,
        IOptions<DBContextOption> options
        ) {
        this._SqlAccessFactory = sqlAccessFactory;
        this._Options = options.Value;

        this.Operation = new TrackingSetOperation(this, TrackingSetApplyChangesOperation.Instance);
        this.User = new TrackingSetUser(this, TrackingSetApplyChangesUser.Instance);
        this.Project = new TrackingSetProject(this, TrackingSetApplyChangesProject.Instance);
        this.ToDo = new TrackingSetToDo(this, TrackingSetApplyChangesToDo.Instance);
    }

    public DBContextOption Options {
        get {
            return this._Options;
        }
        set {
            this._Options = value;
            this._SqlAccessFactory.SetOptions(value);
        }
    }
    public ISqlAccessFactory SqlAccessFactory {
        get => this._SqlAccessFactory;
        set => this._SqlAccessFactory = value;
    }

    public Task<ISqlAccess> GetDataAccessAsync(
            CancellationToken cancellationToken = default(CancellationToken)
        ) {
        return this._SqlAccessFactory.CreateDataAccessAsync(cancellationToken);
    }

    public async Task ApplyChangesAsync(
            ISqlAccess? sqlAccess = default,
            CancellationToken cancellationToken = default(CancellationToken)
        ) {
        if (sqlAccess is null) {
            using (sqlAccess = await this.GetDataAccessAsync()) {
                await this.TrackingChanges.ApplyChangesAsync(sqlAccess, cancellationToken);
            }
        } else {
            await this.TrackingChanges.ApplyChangesAsync(sqlAccess, cancellationToken);
        }
    }

    //    public async Task<ToDo[]> ReadAllToDoAsync() {
    //        using (var sqlAccess = this.GetDataAccess()) {
    //            using (sqlAccess.Connected()) {
    //                var result = new List<ToDo>();
    //#warning TODO
    //                var item = await sqlAccess.ExecuteToDoSelectPKAsync(null!, null);
    //                if (item is not null) {
    //                    result.Add(item);
    //                }
    //                return this.ToDo.AttachRange(result).Select(to => to.Value).ToArray();
    //            }
    //        }
    //    }

    //    public Task<ToDo?> ReadToDoAsync(Guid id) {
    //        ToDoRepo.GetToDos().TryGetValue(id, out var result);
    //        return Task.FromResult(result);
    //    }

    //    public Task<ToDo?> ReadTodoAsync(Guid id) {
    //        throw new NotImplementedException();
    //    }

    //    public Task<ToDo[]> ReadAllTodoAsync() {
    //        throw new NotImplementedException();
    //    }
}

public class DBContextOption
    : TrackingSqlConnectionOption {
    // public string? ConnectionString { get; set; }
}
