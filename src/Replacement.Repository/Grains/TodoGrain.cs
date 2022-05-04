using Replacement.Contracts.Entity;

namespace Replacement.Repository.Grains;

public interface IToDoCollectionGrain : IGrainWithGuidKey {
    Task<List<ToDoEntity>> GetAllToDos(UserEntity user, OperationEntity operation);
    Task<List<ToDoEntity>> GetUsersToDos(UserEntity user, OperationEntity operation);
    Task SetDirty();
}

public interface IToDoGrain : IGrainWithGuidCompoundKey {
    Task<ToDoEntity?> GetToDo(UserEntity user, OperationEntity operation);
    Task<ToDoEntity?> UpsertToDo(ToDoEntity value, UserEntity user, OperationEntity operation);
    Task<bool> DeleteToDo(UserEntity user, OperationEntity operation);
}

//

public class ToDoCollectionGrain : Grain, IToDoCollectionGrain {
    private readonly IDBContext _DBContext;
    private bool _IsDirty;
    private List<ToDoEntity>? _GetAllToDos;

    public ToDoCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<List<ToDoEntity>> GetAllToDos(UserEntity user, OperationEntity operation) {
        var result = this._GetAllToDos;
        if (this._IsDirty || result is null) {
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                result = await sqlAccess.ExecuteToDoSelectAllAsync();
            }
            if (result.Count < 1000) {
                this._GetAllToDos = result;
                this._IsDirty = false;
            }
            return result;
        } else {
            return result;
        }
    }

    public async Task<List<ToDoEntity>> GetUsersToDos(UserEntity user, OperationEntity operation) {
        List<ToDoEntity> result;
        using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
            result = await sqlAccess.ExecuteToDoSelectAllAsync();
        }
        return result;
    }

    public Task SetDirty() {
        this._IsDirty = true;
        return Task.CompletedTask;
    }
}

public class ToDoGrain : GrainBase<ToDoEntity>, IToDoGrain {
    public ToDoGrain(
        IDBContext dbContext
        ) :base(dbContext){
        this._DBContext = dbContext;
    }

    private ToDoPK? GetGrainIdentityAsPK() {
        var toDoId = this.GetGrainIdentity().GetPrimaryKey(out var keyProjectId);
        if ((toDoId == Guid.Empty)
            && Guid.TryParse(keyProjectId, out var projectId)) {
            return new ToDoPK(projectId, toDoId);
        } else {
            return null;
        }
    }
    public override async Task OnActivateAsync() {
        var pk = this.GetGrainIdentityAsPK();
        if (pk is not null) {
            ToDoEntity? state;
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                state = await sqlAccess.ExecuteToDoSelectPKAsync(pk);
            }
            if (state is not null) {
                this._DBContext.ToDo.Attach(state);
                this._State = state;
                return;
            }
        }
        this._State = null;
    }

    public Task<ToDoEntity?> GetToDo(UserEntity user, OperationEntity operation) {
        var state = this._State;
        if (state is null) {
            this.DeactivateOnIdle();
            return Task.FromResult<ToDoEntity?>(null);
        } else if (state.UserId == user.UserId) {
            return Task.FromResult<ToDoEntity?>(state);
        } else {
            return Task.FromResult<ToDoEntity?>(null);
        }
    }

    public async Task<ToDoEntity?> UpsertToDo(ToDoEntity value, UserEntity user, OperationEntity operation) {
        //var operation = new Operation(Guid.NewGuid(), "UpsertToDo", "ToDo", value.Id.ToString(), null, DateTimeOffset.UtcNow, 0);
        value = value with {
            OperationId = operation.OperationId,
            UserId = user.UserId,
            CreatedAt = operation.CreatedAt,
            ModifiedAt = operation.CreatedAt
        };
        this._DBContext.Operation.Add(operation);
        var to = this._DBContext.ToDo.Upsert(value);
        await this._DBContext.ApplyChangesAsync();
        this._State = to.Value;
        await this.PopulateDirty();

        return this._State;
    }


    public async Task<bool> DeleteToDo(UserEntity user, OperationEntity operation) {
        var state = this._State;
        if (state is null) {
            return false;
        } else {
            var value = state with {
                OperationId = operation.OperationId,
                UserId = user.UserId,
                CreatedAt = operation.CreatedAt,
                ModifiedAt = operation.CreatedAt
            };
            this._DBContext.Operation.Add(operation);
            this._DBContext.ToDo.Delete(value);
            await this._DBContext.ApplyChangesAsync();
            this._State = null;
            await this.PopulateDirty();
            this.DeactivateOnIdle();
            return true;
        }
    }

    private async Task PopulateDirty() {
        var todoCollectionGrain = this.GrainFactory.GetGrain<IToDoCollectionGrain>(Guid.Empty);
        await todoCollectionGrain.SetDirty();
    }
}

//

public static partial class GrainExtensions {
    public static IToDoCollectionGrain GetToDoCollectionGrain(this /*IClusterClient*/ IGrainFactory client) {
        var grain = client.GetGrain<IToDoCollectionGrain>(Guid.Empty);
        return grain;
    }

    public static IToDoGrain GetToDoGrain(this /*IClusterClient*/ IGrainFactory client, ToDoPK toDoPK) {
        var grain = client.GetGrain<IToDoGrain>(toDoPK.ToDoId, toDoPK.ProjectId.ToString());
        return grain;
    }
}

//