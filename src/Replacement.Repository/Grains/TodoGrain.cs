using Replacement.Contracts.API;

using System.Security.Claims;

namespace Replacement.Repository.Grains;

public interface IToDoCollectionGrain : IGrainWithGuidKey {
    Task<ToDo[]> GetAllToDos(User user, Operation operation);
    Task SetDirty();
}

public interface IToDoGrain : IGrainWithGuidKey {
    Task<ToDo?> GetToDo(User user, Operation operation);
    Task<bool> UpsertToDo(ToDo value, User user, Operation operation);
    Task<bool> DeleteToDo(ToDo value, User user, Operation operation);
}

//

public class ToDoCollectionGrain : Grain, IToDoCollectionGrain {
    private readonly IDBContext _DBContext;
    private bool _IsDirty;
    private ToDo[]? _GetAllToDos;

    public ToDoCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<ToDo[]> GetAllToDos(User user, Operation operation) {
        if (this._IsDirty || this._GetAllToDos is null) {
            //using var sqlAccess = this._DBContext.GetSqlAccess();
            //sqlAccess.ExecuteToDoSelectPKAsync()
            var result = await this._DBContext.ReadAllToDoAsync();
            this._GetAllToDos = result;
            this._IsDirty = false;
            return result;
        } else {
            return this._GetAllToDos;
        }
    }

    public Task SetDirty() {
        this._IsDirty = true;
        return Task.CompletedTask;
    }
}

public class ToDoGrain : Grain, IToDoGrain {
    private readonly IDBContext _DBContext;
    private ToDo? _State;

    public ToDoGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public override async Task OnActivateAsync() {
        this._State = await this._DBContext.ReadToDoAsync(this.GetGrainIdentity().PrimaryKey);
    }

    public Task<ToDo?> GetToDo(User user, Operation operation) {
        var state = this._State;
        if (state is null) {
            this.DeactivateOnIdle();
        } else if (state.UserId == user.Id) {
            return Task.FromResult(this._State);
        }

        {
            return Task.FromResult<ToDo?>(null);
        }
    }

    public async Task<bool> UpsertToDo(ToDo value, User user, Operation operation) {
        //var operation = new Operation(Guid.NewGuid(), "UpsertToDo", "ToDo", value.Id.ToString(), null, DateTimeOffset.UtcNow, 0);
        value = value with {
            OperationId = operation.Id,
            UserId = user.Id,
            ModifiedAt = operation.CreatedAt
        };
        this._DBContext.Operation.Add(operation);
        var to = this._DBContext.ToDo.Upsert(value);
        await this._DBContext.ApplyChangesAsync();
        this._State = to.Value;
        await this.PopulateDirty();

        return true;
    }


    public async Task<bool> DeleteToDo(ToDo value, User user, Operation operation) {
        //var operation = new Operation(Guid.NewGuid(), "DeleteToDo", "ToDo", value.Id.ToString(), null, DateTimeOffset.UtcNow, 0);
        value = value with {
            OperationId = operation.Id,
            UserId = user.Id,
            ModifiedAt = operation.CreatedAt
        };
        this._DBContext.ToDo.Delete(value);
        await this._DBContext.ApplyChangesAsync();
        this._State = null;
        await this.PopulateDirty();
        this.DeactivateOnIdle();
        return true;
    }

    private async Task PopulateDirty() {
        var todoCollectionGrain = this.GrainFactory.GetGrain<IToDoCollectionGrain>(Guid.Empty);
        await todoCollectionGrain.SetDirty();
    }
}

//

public static partial class GrainExtensions {
    public static IToDoCollectionGrain GetToDoCollectionGrain(this IClusterClient client) {
        var grain = client.GetGrain<IToDoCollectionGrain>(Guid.Empty);
        return grain;
    }

    public static IToDoGrain GetToDoGrain(this IClusterClient client, Guid id) {
        var grain = client.GetGrain<IToDoGrain>(id);
        return grain;
    }
}

//