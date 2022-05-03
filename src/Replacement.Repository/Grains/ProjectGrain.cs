using Replacement.Contracts.Entity;

namespace Replacement.Repository.Grains;
public interface IProjectCollectionGrain : IGrainWithGuidKey {
    Task<List<ProjectEntity>> GetAllProjects(UserEntity user, OperationEntity operation);
    Task<List<ProjectEntity>> GetUsersProjects(UserEntity user, OperationEntity operation);

    Task<Guid?> GetProjectIdFromToDoId(Guid toDoId);

    Task SetDirty();
}

public interface IProjectGrain : IGrainWithGuidKey {
    Task<ProjectEntity?> GetProject(UserEntity user, OperationEntity operation);
    Task<ProjectContext?> GetProjectContext(UserEntity user, OperationEntity operation);
    Task<ProjectEntity?> UpsertProject(ProjectEntity value, UserEntity user, OperationEntity operation);
    Task<bool> DeleteProject(UserEntity user, OperationEntity operation);

    Task<ToDoEntity?> GetToDo(ToDoPK toDoPK, UserEntity user, OperationEntity operation);
    Task<ToDoEntity?> UpsertToDo(ToDoEntity value, UserEntity user, OperationEntity operation);
    Task<bool> DeleteToDo(ToDoPK toDoPK, UserEntity user, OperationEntity operation);
}

public class ProjectCollectionGrain : GrainCollectionBase, IProjectCollectionGrain {
    private LazyValue<List<ProjectEntity>> _AllProjects;

    private List<ProjectEntity>? _GetAllProjects;

    public ProjectCollectionGrain(
        IDBContext dbContext
        ) : base(dbContext) {
        this._AllProjects = new LazyValue<List<ProjectEntity>>();
    }

    public async Task<List<ProjectEntity>> GetAllProjects(UserEntity user, OperationEntity operation) {
        if (this._AllProjects.TryGetValue(out var result)) {
            return result;
        } else {
            List<ProjectEntity> projects;
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                projects = await sqlAccess.ExecuteProjectSelectAllAsync();
            }
            this._AllProjects = this._AllProjects.SetStatus(projects ?? new List<ProjectEntity>());
            return this._AllProjects.Value;
        }
    }

    public async Task<List<ProjectEntity>> GetUsersProjects(UserEntity user, OperationEntity operation) {
        if (this._IsDirty || this._GetAllProjects is null) {
            List<ProjectEntity> projects;
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

public sealed class ProjectGrain : GrainBase<ProjectEntity>, IProjectGrain {
    private readonly ILogger<ProjectGrain> _Logger;

    public ProjectGrain(
        IDBContext dbContext,
        ILogger<ProjectGrain> logger

        ) : base(dbContext) {
        this._Logger = logger;
    }
    protected override async Task<ProjectEntity?> Load() {
        var projectPK = new ProjectPK(this.GetGrainIdentity().PrimaryKey);
        List<ProjectEntity> projects;
        List<ToDoEntity> lstToDos;
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

    public async Task<ProjectEntity?> GetProject(UserEntity user, OperationEntity operation) {
        var state = await this.GetState();
        if (state is null) {
            this.DeactivateOnIdle();
        }
        return state;
    }

    public async Task<ProjectEntity?> UpsertProject(ProjectEntity value, UserEntity user, OperationEntity operation) {
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

        await this.ApplyChangesAsync();
#if false
        sqlException.ErrorCode
        -2146232060
        sqlException.Number
        1205

        try {
            // when ((uint)sqlException.ErrorCode == 0x80131904)
            //} catch (Microsoft.Data.SqlClient.SqlException sqlException)  {
        } catch (Microsoft.Data.SqlClient.SqlException sqlException) {
            this._Logger.LogError(sqlException, "x");
            if (!System.Diagnostics.Debugger.IsAttached) {
                System.Diagnostics.Debugger.Launch();
            }
            System.Diagnostics.Debugger.Break();
            if (sqlException is not null) {
                throw;
            }
        } catch (System.Exception exception) {
            this._Logger.LogError(exception, "x");
            if (!System.Diagnostics.Debugger.IsAttached) {
                System.Diagnostics.Debugger.Launch();
            }
            System.Diagnostics.Debugger.Break();
            if (exception is not null) {
                throw;
            }
            //System.Console.Out.WriteLine($"ErrorCode:{sqlException.ErrorCode}; Number:{sqlException.Number};");

            await Task.Delay(50);
            //} catch (Microsoft.Data.SqlClient.SqlException sqlException) {
            //    await Task.Delay(50);
        }
#endif
        this._State = projectTO.Value;
        this._DBContext.Operation.Detach(operationTO);
        await this.PopulateDirty();

        return projectTO.Value;
    }

    public async Task<bool> DeleteProject(UserEntity user, OperationEntity operation) {
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
            await this.ApplyChangesAsync();
#if false
            try {
                await this._DBContext.ApplyChangesAsync();
                // when ((uint)sqlException.ErrorCode == 0x80131904)
                //} catch (Microsoft.Data.SqlClient.SqlException sqlException)  {
            } catch (Microsoft.Data.SqlClient.SqlException sqlException) {
                this._Logger.LogError(sqlException, "x");
                if (!System.Diagnostics.Debugger.IsAttached) {
                    System.Diagnostics.Debugger.Launch();
                }
                System.Diagnostics.Debugger.Break();
                if (sqlException is not null) {
                    throw;
                }
            } catch (System.Exception exception) {
                this._Logger.LogError(exception, "x");
                if (!System.Diagnostics.Debugger.IsAttached) {
                    System.Diagnostics.Debugger.Launch();
                }
                System.Diagnostics.Debugger.Break();
                if (exception is not null) {
                    throw;
                }
                //System.Console.Out.WriteLine($"ErrorCode:{sqlException.ErrorCode}; Number:{sqlException.Number};");

                await Task.Delay(50);
                //} catch (Microsoft.Data.SqlClient.SqlException sqlException) {
                //    await Task.Delay(50);
            }
#endif
            this._DBContext.Operation.Detach(operationTO);
            await this.PopulateDirty();
            this.DeactivateOnIdle();
            return true;
        }
    }

    public async Task<ToDoEntity?> GetToDo(ToDoPK toDoPK, UserEntity user, OperationEntity operation) {
        var state = await this.GetState();
        if (state is not null) {
            if (this._DBContext.ToDo.TryGetValue(toDoPK, out var result)) {
                return result;
            }
        }
        return null;
    }

    public async Task<ToDoEntity?> UpsertToDo(ToDoEntity value, UserEntity user, OperationEntity operation) {
        var state = await this.GetState();
        if (state is not null) {
            TrackingObject<OperationEntity> operationTO;
            TrackingObject<ToDoEntity> result;
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
            await this.ApplyChangesAsync();
            this._DBContext.Operation.Detach(operationTO);
            return result.Value;
        } else {
            return null;
        }
    }

    public async Task<bool> DeleteToDo(ToDoPK toDoPK, UserEntity user, OperationEntity operation) {
        var state = await this.GetState();
        if (state is not null) {
            if (this._DBContext.ToDo.TryGetValue(toDoPK, out var value)) {
                var operationTO = this._DBContext.Operation.Add(operation);
                this._DBContext.ToDo.Delete(value);
                await this.ApplyChangesAsync();
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

    public async Task<ProjectContext?> GetProjectContext(UserEntity user, OperationEntity operation) {
        var state = await this.GetState();
        if (state is null) {
            return new ProjectContext(new List<ProjectEntity>(), new List<ToDoEntity>());
        } else {
            var projectPK = state.GetPrimaryKey();
            return new ProjectContext(
                Project: new List<ProjectEntity>() { state }, 
                ToDo: new List<ToDoEntity>(
                    this._DBContext.ToDo.Values.Where(todo => todo.GetProjectPK() == projectPK)
                ));
        }
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