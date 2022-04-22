namespace Replacement.Repository.Grains;

public interface IUserCollectionGrain : IGrainWithGuidKey {
    Task<User?> GetUserByName(string name);
    Task SetDirty();
}

public interface IUserGrain : IGrainWithGuidKey {
    Task<ToDo?> GetUser(User user);
    Task<bool> UpsertUser(User value, User user);
    Task<bool> DeleteUser(User value, User user);
}

public class UserCollectionGrain : Grain, IUserCollectionGrain {
    private readonly IDBContext _DBContext;
    private Dictionary<string, User>? _Cache;

    public UserCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<User?> GetUserByName(string name) {
        var cache = this._Cache ??= new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        if (cache.TryGetValue(name, out var result)) {
            return result;
        } else {
            using var sqlAccess = this._DBContext.GetSqlAccess();
#warning TODO
            result = await sqlAccess.ExecuteUserSelectPKAsync(null!, null);
            if (result is not null) { 
                cache[name] = result;
            }
            return result;
        }
    }

    public Task SetDirty() {
        this._Cache = null;
        return Task.CompletedTask;
    }
}

public class UserGrain : Grain, IUserGrain {
    private readonly IDBContext _DBContext;

    public UserGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public Task<ToDo?> GetUser(User user) {
        throw new NotImplementedException();
    }

    public Task<bool> UpsertUser(User value, User user) {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUser(User value, User user) {
        throw new NotImplementedException();
    }
}

