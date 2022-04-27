namespace Replacement.Repository.Grains;

public interface IProjectCollectionGrain : IGrainWithGuidKey {
    Task<List<Project>> GetAllProjects(User user, Operation operation);
    Task<List<Project>> GetUsersProjects(User user, Operation operation);

    Task<Guid?> GetProjectIdFromToDoId(Guid toDoId);

    Task SetDirty();
}

public interface IProjectGrain : IGrainWithGuidKey {
    Task<Project?> GetProject(User user, Operation operation);
    Task<Project?> UpsertProject(Project value, User user, Operation operation);
    Task<bool> DeleteProject(User user, Operation operation);

    Task<ToDo?> GetToDo(ToDoPK toDoPK, User user, Operation operation);
    Task<ToDo?> UpsertToDo(ToDo value, User user, Operation operation);
    Task<bool> DeleteToDo(ToDoPK toDoPK, User user, Operation operation);
}

public class ProjectCollectionGrain : GrainCollectionBase, IProjectCollectionGrain {
    private LazyValue<List<Project>> _AllProjects;

    private List<Project>? _GetAllProjects;

    public ProjectCollectionGrain(
        IDBContext dbContext
        ) :base(dbContext) {
        this._AllProjects = new LazyValue<List<Project>>();
    }

    public async Task<List<Project>> GetAllProjects(User user, Operation operation) {
        if (this._AllProjects.TryGetValue(out var result)) {
            return result;
        } else {
            List<Project> projects;
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                projects = await sqlAccess.ExecuteProjectSelectAllAsync();
            }
            this._AllProjects = this._AllProjects.SetStatus(projects ?? new List<Project>());
            return this._AllProjects.Value;
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

    public Task<Guid?> GetProjectIdFromToDoId(Guid toDoId) {
#warning TODO
        throw new NotImplementedException();
    }

    public Task SetDirty() {
        this._IsDirty = true;
        return Task.CompletedTask;
    }
}

public sealed class ProjectGrain : GrainBase<Project>, IProjectGrain {

    public ProjectGrain(
        IDBContext dbContext
        ):base(dbContext) {
    }
    protected override async Task<Project?> Load() {
        var projectPK = new ProjectPK(this.GetGrainIdentity().PrimaryKey);
        List<Project> projects;
        List<ToDo> lstToDos;
        using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
            (projects, lstToDos) = await sqlAccess.ExecuteProjectSelectPKAsync(projectPK);
        }
        var project = projects.SingleOrDefault();
        if (project is null) {
            return null;
        } else {
            this._DBContext.Project.Attach(project);
            this._DBContext.ToDo.AttachRange(lstToDos);
            return project;
        }
    }

    public async Task<Project?> GetProject(User user, Operation operation) {
        var state = await this.GetState();
        if (state is null) {
            this.DeactivateOnIdle();
        }
        return state;
    }

    public async Task<Project?> UpsertProject(Project value, User user, Operation operation) {
        var state = await this.GetState();

        if (state is null) {
            // new
        } else {
            if (!state.SerialVersion.SerialVersionDoesMatch(value.SerialVersion)) {
                return null;
            }
        }

        var nextValue = value.SetOperation(operation);
        var operationTO = this._DBContext.Operation.Add(operation);
        var projectTO = this._DBContext.Project.Upsert(nextValue);
        await this._DBContext.ApplyChangesAsync();
        this._State = projectTO.Value;
        this._DBContext.Operation.Detach(operationTO);
        await this.PopulateDirty();

        return projectTO.Value;
    }

    public async Task<bool> DeleteProject(User user, Operation operation) {
        var state = await this.GetState();
        if (state is null) {
            return false;
        } else {
            //        
            var lstToDo = this._DBContext.GetToDoOfProject(state.GetPrimaryKey()).ToList();
            var operationTO = this._DBContext.Operation.Add(operation);
            foreach (var toDo in lstToDo) {
                this._DBContext.ToDo.Delete(toDo.SetOperation(operation));
            }
            this._DBContext.Project.Delete(state.SetOperation(operation));
            this._State = null;
            await this._DBContext.ApplyChangesAsync();
            this._DBContext.Operation.Detach(operationTO);
            await this.PopulateDirty();
            this.DeactivateOnIdle();
            return true;
        }
    }

    public async Task<ToDo?> GetToDo(ToDoPK toDoPK, User user, Operation operation) {
        var state = await this.GetState();
        if (state is not null) {
            if (this._DBContext.ToDo.TryGetValue(toDoPK, out var result)) {
                return Task.FromResult<ToDo?>(result);
            }
        }
        return Task.FromResult<ToDo?>(null);
    }

    public async Task<ToDo?> UpsertToDo(ToDo value, User user, Operation operation) {
        var state = await this.GetState();
        if (state is not null) {
            TrackingObject<Operation> operationTO;
            TrackingObject<ToDo> result;
            if (this._DBContext.ToDo.TryGetValue(value.GetPrimaryKey(), out var currentToDo)) {
                if (!currentToDo.SerialVersion.SerialVersionDoesMatch(value.SerialVersion)) {
                    return null;
                }
                operationTO = this._DBContext.Operation.Add(operation);
                value = value.SetOperation(operation);
                result = this._DBContext.ToDo.Update(value);
            } else {
                operationTO = this._DBContext.Operation.Add(operation);
                value = value.SetOperation(operation);
                result = this._DBContext.ToDo.Add(value);
            }
            await this._DBContext.ApplyChangesAsync();
            this._DBContext.Operation.Detach(operationTO);
            return result.Value;
        } else {
            return null;
        }
    }

    public async Task<bool> DeleteToDo(ToDoPK toDoPK, User user, Operation operation) {
        var state = await this.GetState();
        if (state is not null) {
            if (this._DBContext.ToDo.TryGetValue(toDoPK, out var value)) {
                var operationTO = this._DBContext.Operation.Add(operation);
                this._DBContext.ToDo.Delete(value);
                await this._DBContext.ApplyChangesAsync();
                this._DBContext.Operation.Detach(operationTO);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
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