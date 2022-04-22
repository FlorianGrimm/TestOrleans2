using Replacement.Contracts.API;

using System.Security.Claims;

namespace Replacement.Repository.Grains;

public interface ITodoCollectionGrain : IGrainWithGuidKey {
    Task<ToDo[]> GetAllTodos(User user);
    Task SetDirty();
}

public interface ITodoGrain : IGrainWithGuidKey {
    Task<ToDo?> GetTodo(User user);
    Task<bool> UpsertTodo(ToDo value, User user);
    Task<bool> DeleteTodo(ToDo value, User user);
}

public class TodoCollectionGrain : Grain, ITodoCollectionGrain {
    private readonly IDBContext _DBContext;
    private bool _IsDirty;
    private ToDo[]? _GetAllTodos;

    public TodoCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<ToDo[]> GetAllTodos(User user) {
        if (this._IsDirty || this._GetAllTodos is null) {
            //using var sqlAccess = this._DBContext.GetSqlAccess();
            //sqlAccess.ExecuteToDoSelectPKAsync()
            var result = await this._DBContext.ReadAllTodoAsync();
            this._GetAllTodos = result;
            this._IsDirty = false;
            return result;
        } else {
            return this._GetAllTodos;
        }
    }

    public Task SetDirty() {
        this._IsDirty = true;
        return Task.CompletedTask;
    }
}

public class TodoGrain : Grain, ITodoGrain {
    private readonly IDBContext _DBContext;
    private ToDo? _State;

    public TodoGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public override async Task OnActivateAsync() {
        this._State = await this._DBContext.ReadTodoAsync(this.GetGrainIdentity().PrimaryKey);
    }

    public Task<ToDo?> GetTodo(User user) {
        if (this._State is null) {
            this.DeactivateOnIdle();
        }
        return Task.FromResult(this._State);
    }

    public async Task<bool> UpsertTodo(ToDo value, User user) {
        var activity = new Activity(Guid.NewGuid(), "UpsertTodo", "Todo", value.Id.ToString(), null, DateTimeOffset.UtcNow, 0);
        value = value with {
            ActivityId = activity.Id,
            UserId = user.Id,
            ModifiedAt = activity.CreatedAt
        };
        this._DBContext.Activity.Add(activity);
        var to = this._DBContext.Todo.Upsert(value);
        await this._DBContext.ApplyChangesAsync();
        this._State = to.Value;
        await this.PopulateDirty();

        return true;
    }


    public async Task<bool> DeleteTodo(ToDo value, User user) {
        var activity = new Activity(Guid.NewGuid(), "DeleteTodo", "Todo", value.Id.ToString(), null, DateTimeOffset.UtcNow, 0);
        value = value with {
            ActivityId = activity.Id,
            UserId = user.Id,
            ModifiedAt = activity.CreatedAt
        };
        this._DBContext.Todo.Delete(value);
        await this._DBContext.ApplyChangesAsync();
        this._State = null;
        await this.PopulateDirty();
        this.DeactivateOnIdle();
        return true;
    }

    private async Task PopulateDirty() {
        var todoCollectionGrain = this.GrainFactory.GetGrain<ITodoCollectionGrain>(Guid.Empty);
        await todoCollectionGrain.SetDirty();
    }
}
