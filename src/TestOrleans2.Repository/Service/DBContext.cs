namespace TestOrleans2.Repository.Service;

[Transient]
public class DBContext : Brimborium.Tracking.TrackingContext, IDBContext {
    private ISqlAccessFactory _SqlAccessFactory;
    private readonly DBContextApplyChangesSequencializer _DbContextApplyChangesSequencializer;
    private DBContextOption _Options;

    public ITrackingSet<OperationPK, OperationEntity> Operation { get; }
    public ITrackingSet<UserPK, UserEntity> User { get; }
    public ITrackingSet<ProjectPK, ProjectEntity> Project { get; }
    public ITrackingSet<ToDoPK, ToDoEntity> ToDo { get; }

    public DBContext(
        ISqlAccessFactory sqlAccessFactory,
        DBContextApplyChangesSequencializer dbContextApplyChangesSequencializer,
        IOptions<DBContextOption> options
        ) {
        this._SqlAccessFactory = sqlAccessFactory;
        this._DbContextApplyChangesSequencializer = dbContextApplyChangesSequencializer;
        this._Options = options.Value;
        this.UseSequencializer = true;

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

    public bool UseSequencializer { get; set; }

    public Task<ISqlAccess> GetDataAccessAsync(
            CancellationToken cancellationToken = default(CancellationToken)
        ) {
        return this._SqlAccessFactory.CreateDataAccessAsync(cancellationToken);
    }

    public async Task ApplyChangesAsync(
            ISqlAccess? sqlAccess = default,
            CancellationToken cancellationToken = default(CancellationToken)
        ) {
        if (this.UseSequencializer) {
            if (sqlAccess is null) {
                using (sqlAccess = await this.GetDataAccessAsync()) {
                    await this._DbContextApplyChangesSequencializer.ApplyChangesAsync(this.TrackingChanges, sqlAccess, cancellationToken);
                }
            } else {
                await this._DbContextApplyChangesSequencializer.ApplyChangesAsync(this.TrackingChanges, sqlAccess, cancellationToken);
            }
        } else {
            if (sqlAccess is null) {
                using (sqlAccess = await this.GetDataAccessAsync()) {
                    await this.TrackingChanges.ApplyChangesAsync(sqlAccess, cancellationToken);
                }
            } else {
                await this.TrackingChanges.ApplyChangesAsync(sqlAccess, cancellationToken);
            }
        }
    }

    public void Clear() {
        this.Operation.Clear();
        this.User.Clear();
        this.Project.Clear();
        this.ToDo.Clear();
        this.TrackingChanges.Clear();
    }
}

public class DBContextOption
    : TrackingSqlConnectionOption {
    // public string? ConnectionString { get; set; }
}

[Singleton]
public class DBContextApplyChangesSequencializer {    
    private Task _LastTask;

    public DBContextApplyChangesSequencializer() {
        this._LastTask = Task.CompletedTask;
    }

    public async Task ApplyChangesAsync(TrackingChanges trackingChanges, ISqlAccess sqlAccess, CancellationToken cancellationToken) {
        Task currentTask;
        lock (this) {
            currentTask = this._LastTask.ContinueWith((task) => {
                return trackingChanges.ApplyChangesAsync(sqlAccess, cancellationToken);
            }, TaskContinuationOptions.RunContinuationsAsynchronously).Unwrap();
            this._LastTask = currentTask;
        }
        await currentTask;
        lock (this) {
            System.Threading.Interlocked.CompareExchange(ref this._LastTask, Task.CompletedTask, currentTask);
        }
    }
}