using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

[Transient]
public class DBContext : Brimborium.Tracking.TrackingContext, IDBContext {
    private ISqlAccessFactory _SqlAccessFactory;
    private DBContextOption _Options;

    public ITrackingSet<OperationPK, OperationEntity> Operation { get; }
    public ITrackingSet<UserPK, UserEntity> User { get; }
    public ITrackingSet<ProjectPK, ProjectEntity> Project { get; }
    public ITrackingSet<ToDoPK, ToDoEntity> ToDo { get; }


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
