using Microsoft.AspNetCore.Mvc;

namespace Replacement.Repository.Grains;

public interface IProjectCollectionGrain : IGrainWithGuidKey {
    Task<List<Project>> GetAllProjects(User user, Operation operation);
    Task<List<Project>> GetUsersProjects(User user, Operation operation);
    
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
    private List<Project>? _GetAllProjects;

    public ProjectCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<List<Project>> GetAllProjects(User user, Operation operation) {
        if (this._IsDirty || this._GetAllProjects is null) {
            List<Replacement.Contracts.API.Project> projects;
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                projects = await sqlAccess.ExecuteProjectSelectAllAsync();
            }
            this._GetAllProjects = projects;
            this._IsDirty = false;
            return projects;
        } else {
            return this._GetAllProjects;
        }
    }

    public async Task<List<Project>> GetUsersProjects(User user, Operation operation) {
        if (this._IsDirty || this._GetAllProjects is null) {
            List<Replacement.Contracts.API.Project> projects;
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
#warning TODO
                projects = await sqlAccess.ExecuteProjectSelectAllAsync();
            }
            this._GetAllProjects = projects;
            this._IsDirty = false;
            return projects;
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
        Project? project;
        List<ToDo> lstToDos;
        using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
            (project, lstToDos) = await sqlAccess.ExecuteProjectSelectPKAsync(pk);
        }
        if (project is null) {
            this.DeactivateOnIdle();
        } else {
            this._DBContext.Project.Attach(project);
            this._DBContext.ToDo.AttachRange(lstToDos);
            this._State = project;
        }
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
            OperationId = operation.OperationId,
            CreatedAt = operation.CreatedAt,
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
            OperationId = operation.OperationId
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
    public static IProjectCollectionGrain GetProjectCollectionGrain(this /*IClusterClient*/ IGrainFactory client) {
        var grain = client.GetGrain<IProjectCollectionGrain>(Guid.Empty);
        return grain;
    }

    public static IProjectGrain GetProjectGrain(this /*IClusterClient*/ IGrainFactory client, Guid id) {
        var grain = client.GetGrain<IProjectGrain>(id);
        return grain;
    }
}

//