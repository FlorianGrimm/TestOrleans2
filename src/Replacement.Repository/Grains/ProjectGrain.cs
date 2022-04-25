namespace Replacement.Repository.Grains;

public interface IProjectCollectionGrain : IGrainWithGuidKey {
    Task<Project[]> GetAllProjects(User user, Operation operation);
    Task SetDirty();
}

public interface IProjectGrain : IGrainWithGuidKey {
    Task<Project?> GetProject(User user, Operation operation);
    Task<bool> UpsertProject(Project value, User user, Operation operation);
    Task<bool> DeleteProject(User user, Operation operation);
}

//

public class ProjectCollectionGrain : Grain, IProjectCollectionGrain {
    private readonly IDBContext _DBContext;
    private bool _IsDirty;
    private Project[]? _GetAllProjects;

    public ProjectCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<Project[]> GetAllProjects(User user, Operation operation) {
        if (this._IsDirty || this._GetAllProjects is null) {
            using (var sqlAccess = this._DBContext.GetSqlAccess()) {
                using (sqlAccess.Connected()) {
#warning here
                    sqlAccess.ExecuteProjectSelectAllAsync();
                }
            }
                //sqlAccess.ExecuteProjectSelectPKAsync()
                var result = await this._DBContext.ReadAllProjectAsync();
            this._GetAllProjects = result;
            this._IsDirty = false;
            return result;
        } else {
            return this._GetAllProjects;
        }
    }

    public Task SetDirty() {
        this._IsDirty = true;
        return Task.CompletedTask;
    }
}

public class ProjectGrain : Grain, IProjectGrain {
    private readonly IDBContext _DBContext;
    private Project? _State;

    public ProjectGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public override async Task OnActivateAsync() {
        var pk = new ProjectPK(this.GetGrainIdentity().PrimaryKey);
        Project? result;
        using (var sqlAccess = this._DBContext.GetSqlAccess()) {
            using (sqlAccess.Connected()) {
                result = await sqlAccess.ExecuteProjectSelectPKAsync(pk);
            }
        }
        if (result is null) {
            this.DeactivateOnIdle();
        } else {
            this._DBContext.Project.Attach(result);
        }
        this._State = result;
    }

    public Task<Project?> GetProject(User user, Operation operation) {
        var state = this._State;
        return Task.FromResult<Project?>(state);
    }

    public async Task<bool> UpsertProject(Project value, User user, Operation operation) {
        var state = this._State;
        if (state is null) { return false; }
        //
#warning TODO
        if (state.SerialVersion != value.SerialVersion) {
            return false;
        }
        value = value with {
            OperationId = operation.Id,
            ModifiedAt = operation.CreatedAt
        };
        this._DBContext.Operation.Add(operation);
        var to = this._DBContext.Project.Upsert(value);
        await this._DBContext.ApplyChangesAsync();
        this._State = to.Value;
        await this.PopulateDirty();

        return true;
    }


    public async Task<bool> DeleteProject(User user, Operation operation) {
        var state = this._State;
        if (state is null) { return false; }
        //
        var value = state with {
            OperationId = operation.Id
        };
        this._DBContext.Operation.Add(operation);
        this._DBContext.Project.Delete(value);
        await this._DBContext.ApplyChangesAsync();
        this._State = null;
        await this.PopulateDirty();
        this.DeactivateOnIdle();
        return true;
    }

    private async Task PopulateDirty() {
        var ProjectCollectionGrain = this.GrainFactory.GetGrain<IProjectCollectionGrain>(Guid.Empty);
        await ProjectCollectionGrain.SetDirty();
    }
}

//

public static partial class GrainExtensions {
    public static IProjectCollectionGrain GetProjectCollectionGrain(this IClusterClient client) {
        var grain = client.GetGrain<IProjectCollectionGrain>(Guid.Empty);
        return grain;
    }

    public static IProjectGrain GetProjectGrain(this IClusterClient client, Guid id) {
        var grain = client.GetGrain<IProjectGrain>(id);
        return grain;
    }
}

//