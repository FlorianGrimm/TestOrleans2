namespace Replacement.Repository.Grains;

public interface IUserCollectionGrain : IGrainWithGuidKey {
    Task<User?> GetUserByUserName(string username, Operation operation);
    Task SetDirty();
}

public interface IUserGrain : IGrainWithGuidKey {
    Task<ToDo?> GetUser(User user, Operation operation);
    Task<bool> UpsertUser(User value, User user, Operation operation);
    Task<bool> DeleteUser(User value, User user, Operation operation);
}

public class UserCollectionGrain : Grain, IUserCollectionGrain {
    private readonly IDBContext _DBContext;
    private Dictionary<string, User>? _Cache;

    public UserCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<User?> GetUserByUserName(string name, Operation operation) {
        var cache = this._Cache ??= new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        if (cache.TryGetValue(name, out var result)) {
            return result;
        } else {
            using (var sqlAccess = this._DBContext.GetSqlAccess()) {
                using (sqlAccess.Connected()) {
                    result = await sqlAccess.ExecuteUserSelectByUserNameAsync(new UserSelectByUserNameArg(name), null);
                }
            }
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

    public Task<ToDo?> GetUser(User user, Operation operation) {
        throw new NotImplementedException();
    }

    public Task<bool> UpsertUser(User value, User user, Operation operation) {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUser(User value, User user, Operation operation) {
        throw new NotImplementedException();
    }
}

//

public static partial class GrainExtensions {
    public static IUserCollectionGrain GetUserCollectionGrain(this IClusterClient client) {
        var grain = client.GetGrain<IUserCollectionGrain>(Guid.Empty);
        return grain;
    }

    public static IUserGrain GetUserGrain(this IClusterClient client, Guid id) {
        var grain = client.GetGrain<IUserGrain>(id);
        return grain;
    }

    public static async Task<User?> GetUserByUserName(this IClusterClient client, string username, Operation operation) {
        return await client.GetUserCollectionGrain().GetUserByUserName(username, operation);
    }
}

//