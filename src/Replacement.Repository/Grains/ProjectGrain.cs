using Orleans.Concurrency;

using Replacement.Contracts.Entity;

namespace Replacement.Repository.Grains;
public interface IProjectCollectionGrain : IGrainWithGuidKey {
    Task<List<ProjectEntity>> GetAllProjects(UserEntity user, OperationEntity operation);
    Task<List<ProjectEntity>> GetUsersProjects(UserEntity user, OperationEntity operation);

    Task<Guid?> GetProjectIdFromToDoId(Guid toDoId);

    Task Subscripe(IProjectGrainObserver projectGrainObserver);
    Task Unsubscripe(IProjectGrainObserver projectGrainObserver);
    Task SetDirty();
    Task SetDirtyProject(ProjectPK projectPK);
    Task SetDirtyToDo(ToDoPK toDoPK);
}

public interface IProjectGrainObserver : IGrainObserver {
    void ReceiveDirtyProject(ProjectPK projectPK);
    void ReceiveDirtyTodo(ToDoPK toDoPK);
    //    void ReceiveActivate(Guid projectId);
    //    void ReceiveDeactivate(Guid guid);
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

public partial class ProjectCollectionGrain : GrainCollectionBase, IProjectCollectionGrain {
    private readonly ObserverManager<IProjectGrainObserver> _ProjectGrainObservers;
    private readonly ILogger _Logger;
    private CachedValue<List<ProjectEntity>> _AllProjects;

    public ProjectCollectionGrain(
        IDBContext dbContext,
        ILogger<ProjectCollectionGrain> logger
        ) : base(dbContext) {
        this._AllProjects = new CachedValue<List<ProjectEntity>>();
        this._Logger = logger;
        this._ProjectGrainObservers = new ObserverManager<IProjectGrainObserver>(TimeSpan.Zero, logger, "ProjectGrainObserver");
    }


    [LoggerMessage(
        EventId = (int)LogEventId.ProjectCollectionGrain_LogSubscripe,
        Level = LogLevel.Trace,
        Message = "Subscripe ProjectGrainObserver:{grainId};")]
    private partial void LogSubscripe(string grainId);

    [AlwaysInterleave]
    public Task Subscripe(IProjectGrainObserver projectGrainObserver) {
        LogSubscripe(projectGrainObserver.GetGrainId()?.ToString() ?? "-");

        this._ProjectGrainObservers.Subscribe(projectGrainObserver, projectGrainObserver);
        return Task.CompletedTask;
    }

    [LoggerMessage(
       EventId = (int)LogEventId.ProjectCollectionGrain_LogUnsubscripe,
       Level = LogLevel.Trace,
       Message = "Subscripe ProjectGrainObserver:{grainId};")]
    private partial void LogUnsubscripe(string grainId);

    [AlwaysInterleave]
    public Task Unsubscripe(IProjectGrainObserver projectGrainObserver) {
        LogUnsubscripe(projectGrainObserver.GetGrainId()?.ToString() ?? "-");

        this._ProjectGrainObservers.Unsubscribe(projectGrainObserver);
        return Task.CompletedTask;
    }

    [AlwaysInterleave]
    public Task SetDirty() {
        this._AllProjects = new CachedValue<List<ProjectEntity>>();
        return Task.CompletedTask;
    }

    [LoggerMessage(
    EventId = (int)LogEventId.ProjectCollectionGrain_LogSetDirtyProject,
    Level = LogLevel.Trace,
    Message = "SetDirtyProject projectPK:{projectPK};")]
    private partial void LogSetDirtyProject(string projectPK);


    [AlwaysInterleave]
    public Task SetDirtyProject(ProjectPK projectPK) {
        this.LogSetDirtyProject(projectPK.ToString());
        this._ProjectGrainObservers.Notify((o) => { o.ReceiveDirtyProject(projectPK); });
        this._AllProjects = new CachedValue<List<ProjectEntity>>();
        return Task.CompletedTask;
    }

    [LoggerMessage(
    EventId = (int)LogEventId.ProjectCollectionGrain_LogSetDirtyToDo,
    Level = LogLevel.Trace,
    Message = "SetDirtyToDo toDoPK:{toDoPK};")]
    private partial void LogSetDirtyToDo(string toDoPK);

    [AlwaysInterleave]
    public Task SetDirtyToDo(ToDoPK toDoPK) {
        this._ProjectGrainObservers.Notify((o) => { o.ReceiveDirtyTodo(toDoPK); });
        //this._AllProjects = new CachedValue<List<ProjectEntity>>();
        return Task.CompletedTask;
    }

    [LoggerMessage(
    EventId = (int)LogEventId.ProjectCollectionGrain_GetAllProjects,
    Level = LogLevel.Trace,
    Message = "SetDirtyToDo userId:{userId}; resultCount:{resultCount}; cached:{cached}")]
    private partial void LogGetAllProjects(Guid userId, int resultCount, bool cached);

    public async Task<List<ProjectEntity>> GetAllProjects(UserEntity user, OperationEntity operation) {
        if (this._AllProjects.TryGetValue(out var result)) {
            this.LogGetAllProjects(user.UserId, result.Count, true);
            return result;
        } else {
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                result = await sqlAccess.ExecuteProjectSelectAllAsync();
            }
            this._AllProjects = this._AllProjects.SetStatus(result);
            this.LogGetAllProjects(user.UserId, result.Count, false);
            return result;
        }
    }

    [LoggerMessage(
    EventId = (int)LogEventId.ProjectCollectionGrain_GetUsersProjects,
    Level = LogLevel.Trace,
    Message = "GetUsersProjects userId:{userId}; resultCount:{resultCount}; cached:{cached}")]
    private partial void LogGetUsersProjects(Guid userId, int resultCount, bool cached);

    public async Task<List<ProjectEntity>> GetUsersProjects(UserEntity user, OperationEntity operation) {
        List<ProjectEntity> result;
        using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
#warning TODO
            result = await sqlAccess.ExecuteProjectSelectAllAsync();
        }
        this.LogGetUsersProjects(user.UserId, result.Count, false);
        return result;
    }

    public Task<Guid?> GetProjectIdFromToDoId(Guid toDoId) {
#warning TODO
        throw new NotImplementedException();
    }
}

public sealed partial class ProjectGrain : GrainBase<ProjectEntity>, IProjectGrain {
    private readonly ILogger<ProjectGrain> _Logger;

    public ProjectGrain(
        IDBContext dbContext,
        ILogger<ProjectGrain> logger
        ) : base(dbContext) {
        this._Logger = logger;
    }

    //public override Task OnActivateAsync() {
    //    this.GrainFactory.GetProjectCollectionGrain().AsReference<IProjectGrainObserver>().ReceiveActivate(this.GetPrimaryKey());
    //    return base.OnActivateAsync();
    //}

    //public override Task OnDeactivateAsync() {
    //    this.GrainFactory.GetProjectCollectionGrain().AsReference<IProjectGrainObserver>().ReceiveDeactivate(this.GetPrimaryKey());
    //    return base.OnDeactivateAsync();
    //}

    private ProjectPK GetProjectPrimaryKey()
        => new ProjectPK(this.GetGrainIdentity().PrimaryKey);

    [LoggerMessage(
        EventId = (int)LogEventId.ProjectGrain_Load,
        Level = LogLevel.Trace,
        Message = "Load projectPK:{projectPK}; found:{found}; toDosCount:{toDosCount};")]
    private partial void LogLoad(ProjectPK projectPK, bool found, int toDosCount);

    protected override async Task<ProjectEntity?> Load() {
        ProjectPK? projectPK = GetProjectPrimaryKey();
        List<ProjectEntity> projects;
        List<ToDoEntity> lstToDos;
        using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
            (projects, lstToDos) = await sqlAccess.ExecuteProjectSelectPKAsync(projectPK);
        }
        var project = projects.SingleOrDefault();
        if (project is null) {
            this.LogLoad(projectPK, false, 0);

            return null;
        } else {
            this._DBContext.Project.Attach(project);
            this._DBContext.ToDo.AttachRange(lstToDos);
            this.LogLoad(projectPK, true, lstToDos.Count);
            return project;
        }
    }

    [LoggerMessage(
        EventId = (int)LogEventId.ProjectGrain_GetProject,
        Level = LogLevel.Trace,
        Message = "GetProject projectPK:{projectPK}; found:{found};")]
    private partial void LogGetProject(ProjectPK projectPK, bool found);

    public async Task<ProjectEntity?> GetProject(UserEntity user, OperationEntity operation) {
        var state = await this.GetState();
        if (state is null) {
            this.DeactivateOnIdle();
            this.LogGetProject(this.GetProjectPrimaryKey(), false);
            return state;
        } else { 
            this.LogGetProject(state.GetPrimaryKey(), true);
            return state;
        }
    }

    public async Task<ProjectEntity?> UpsertProject(ProjectEntity value, UserEntity user, OperationEntity operation) {
        try {

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

            this._State = projectTO.Value;
            this._DBContext.Operation.Detach(operationTO);
            await this.PopulateDirtyProject();

            return projectTO.Value;
        } catch {
            this.SetStateDirty();
            throw;
        }
    }

    public async Task<bool> DeleteProject(UserEntity user, OperationEntity operation) {
        try {
            var state = await this.GetState();
            if (state is null) { return false; }
            //        
            var lstToDo = this._DBContext.GetToDoOfProject(state.GetPrimaryKey()).ToList();
            var operationTO = this._DBContext.Operation.Add(operation);
            foreach (var toDo in lstToDo) {
                this._DBContext.ToDo.Delete(toDo.SetOperation(operation));
            }
            this._DBContext.Project.Delete(state.SetOperation(operation));
            this._State = null;
            await this.ApplyChangesAsync();
            this._DBContext.Operation.Detach(operationTO);
            await this.PopulateDirtyProject();
            this.DeactivateOnIdle();
            return true;
        } catch {
            this.SetStateDirty();
            throw;
        }
    }

    public async Task<ToDoEntity?> GetToDo(ToDoPK toDoPK, UserEntity user, OperationEntity operation) {
        try {
            var state = await this.GetState();
            if (state is not null) {
                if (this._DBContext.ToDo.TryGetValue(toDoPK, out var result)) {
                    return result;
                }
            }
            return null;
        } catch {
            this.SetStateDirty();
            throw;
        }
    }

    public async Task<ToDoEntity?> UpsertToDo(ToDoEntity value, UserEntity user, OperationEntity operation) {
        try {
            var state = await this.GetState();
            if (state is null) { return null; }

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
            await this.PopulateDirtyTodo(value.GetPrimaryKey());
            return result.Value;
        } catch {
            this.SetStateDirty();
            throw;
        }
    }

    public async Task<bool> DeleteToDo(ToDoPK toDoPK, UserEntity user, OperationEntity operation) {
        try {
            var state = await this.GetState();
            if (state is null) { return false; }

            if (this._DBContext.ToDo.TryGetValue(toDoPK, out var value)) {
                var operationTO = this._DBContext.Operation.Add(operation);
                this._DBContext.ToDo.Delete(value);
                await this.ApplyChangesAsync();
                this._DBContext.Operation.Detach(operationTO);
                return true;
            } else {
                return false;
            }
        } catch {
            this.SetStateDirty();
            throw;
        }
    }

    private async Task PopulateDirtyProject() {
        var ProjectCollectionGrain = this.GrainFactory.GetGrain<IProjectCollectionGrain>(Guid.Empty);
        var projectId = this.GetPrimaryKey();
        await ProjectCollectionGrain.SetDirtyProject(new ProjectPK(projectId));
    }

    private async Task PopulateDirtyTodo(ToDoPK toDoPK) {
        var ProjectCollectionGrain = this.GrainFactory.GetGrain<IProjectCollectionGrain>(Guid.Empty);
        var projectId = this.GetPrimaryKey();
        await ProjectCollectionGrain.SetDirtyToDo(toDoPK);
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