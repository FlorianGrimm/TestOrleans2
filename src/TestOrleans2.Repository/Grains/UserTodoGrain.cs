namespace TestOrleans2.Repository.Grains;


public interface IUserToDoGrain : IGrainWithGuidKey {
    Task<List<ToDoEntity>> GetUsersToDos(OperationEntity operation);
    Task<List<ToDoEntity>> GetAllToDos(OperationEntity operation);
}

public class UserToDoGrain : GrainBase<UserEntity>, IUserToDoGrain, IProjectGrainObserver {
    private CachedValue<List<ToDoEntity>> _GetAllToDos;
    private readonly ILogger _Logger;

    public UserToDoGrain(
        IDBContext dBContext,
        ILogger<UserGrain> logger
        ) : base(dBContext) {
        this._GetAllToDos = new CachedValue<List<ToDoEntity>>();
        this._Logger = logger;
    }

    public override async Task OnActivateAsync() {
        //this.GrainFactory.GetProjectCollectionGrain().AsReference<IProjectGrainObserver>
        await this.GrainFactory.GetProjectCollectionGrain().Subscripe(this.AsReference<IProjectGrainObserver>());
        await base.OnActivateAsync();
    }
    public override async Task OnDeactivateAsync() {
        await this.GrainFactory.GetProjectCollectionGrain().Unsubscripe(this.AsReference<IProjectGrainObserver>());
        await base.OnDeactivateAsync();
    }

    public async Task<List<ToDoEntity>> GetAllToDos(OperationEntity operation) {
        if (!this._GetAllToDos.TryGetValue(out var result)) {
#warning TODO check user            
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                result = await sqlAccess.ExecuteToDoSelectAllAsync();
            }
            if (result.Count < 1000) {
                this._GetAllToDos = this._GetAllToDos.SetStatus(result);
                //if (result.Any()) {
                //    var projects = result.Select(i => i.ProjectId).Distinct().ToList();
                //    this.GrainFactory.GetProjectCollectionGrain().
                //}
            } else {
                this._GetAllToDos = new CachedValue<List<ToDoEntity>>();
            }
            return result;
        }
        return result;
    }

    public async Task<List<ToDoEntity>> GetUsersToDos(OperationEntity operation) {
        using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
#warning TODO check user
            var result = await sqlAccess.ExecuteToDoSelectAllAsync();
            return result;
        }
    }

    public void ReceiveDirtyProject(ProjectPK projectPK) {
        this._GetAllToDos = new CachedValue<List<ToDoEntity>>();
        this._Logger.LogInformation("UserToDoGrain.ReceiveDirty {0}", projectPK);
    }

    public void ReceiveDirtyTodo(ToDoPK toDoPK) {
        this._GetAllToDos = new CachedValue<List<ToDoEntity>>();
        this._Logger.LogInformation("UserToDoGrain.ReceiveDirty {0}", toDoPK);
    }
}

//

public static partial class GrainExtensions {
    public static IUserToDoGrain GetUserToDoGrain(this /*IClusterClient*/ IGrainFactory client, Guid id) {
        var grain = client.GetGrain<IUserToDoGrain>(id);
        return grain;
    }
}

//