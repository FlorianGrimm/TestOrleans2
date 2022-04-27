namespace Replacement.Repository.Grains;

public interface IUserCollectionGrain : IGrainWithGuidKey {
    Task<List<User>> GetAllUsers(Operation operation);
    Task<(User? user, bool created)> GetUserByUserName(string username, bool createIfNeeded, Operation operation);
    Task SetDirty(User? user);
}

public interface IUserGrain : IGrainWithGuidKey {
    Task<User?> GetUser(Operation operation);
    Task<User?> UpsertUser(User value, User? currentUser, Operation operation);
    Task<bool> DeleteUser(User? currentUser, Operation operation);
}

public class UserCollectionGrain : Grain, IUserCollectionGrain {
    private readonly IDBContext _DBContext;
    private Dictionary<string, User>? _Cache;

    public UserCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public Task<List<User>> GetAllUsers(Operation operation) {
        throw new NotImplementedException();
    }

    public async Task<(User? user, bool created)> GetUserByUserName(string username, bool createIfNeeded, Operation operation) {
        var cache = this._Cache ??= new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        if (cache.TryGetValue(username, out var user)) {
            return (user: user, created: false);
        } else {
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                user = await sqlAccess.ExecuteUserSelectByUserNameAsync(new UserSelectByUserNameArg(username));
            }
            if (user is not null) {
                cache[username] = user;
                return (user: user, created: false);
            }
            if (createIfNeeded) {
                user = User.Create(
                    operation: operation,
                    userId: Guid.NewGuid(),
                    userName: username
                    );
                await this.GrainFactory.GetUserGrain(user.UserId).UpsertUser(user, null, operation);
                return (user: user, created: true);
            } else {
                return (user: user, created: false);
            }
        }
    }

    public Task SetDirty(User? user) {
        if (user is null) {
            this._Cache = null;
        } else if (this._Cache is not null) {
            this._Cache[user.UserName] = user;
        }
        return Task.CompletedTask;
    }
}

public class UserGrain : Grain, IUserGrain {
    private readonly IDBContext _DBContext;
    private User? _User;

    public UserGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public override async Task OnActivateAsync() {
        var pk = new UserPK(
                this.GetGrainIdentity().PrimaryKey
            );
        using (var dataAccess = await this._DBContext.GetDataAccessAsync()) {
            var result = await dataAccess.ExecuteUserSelectPKAsync(pk);
            if (result is null) {
                this._User = null;
            } else {
                this._User = result;
                this._DBContext.User.Attach(result);
            }
        }
    }

    public Task<User?> GetUser(Operation operation) {
        if (this._User is null) {
            this.DeactivateOnIdle();
        }
        return Task.FromResult(this._User);
    }

    public async Task<User?> UpsertUser(User value, User? currentUser, Operation operation) {
        value = value.SetOperation(operation);
        this._DBContext.Operation.Add(operation);
        var resultTO = this._DBContext.User.Upsert(value);
        await this._DBContext.ApplyChangesAsync();

        var result = resultTO.Value;
        this._User = result;
        await this.PopulateDirty(result);
        return result;
    }

    public async Task<bool> DeleteUser(User? currentUser, Operation operation) {
        if (this._User is null) {
            return false;
        } else {
            var value = this._User.SetOperation(operation);
            this._DBContext.Operation.Add(operation);
            this._DBContext.User.Delete(value);
            await this._DBContext.ApplyChangesAsync();

            this._User = null;
            await this.PopulateDirty(null);
            this.DeactivateOnIdle();
            return true;
        }
    }

    private async Task PopulateDirty(User? user) {
        var userCollectionGrain = this.GrainFactory.GetUserCollectionGrain();
        await userCollectionGrain.SetDirty(user);
    }
}

//

public static partial class GrainExtensions {
    public static IUserCollectionGrain GetUserCollectionGrain(this /*IClusterClient*/ IGrainFactory client) {
        var grain = client.GetGrain<IUserCollectionGrain>(Guid.Empty);
        return grain;
    }

    public static IUserGrain GetUserGrain(this /*IClusterClient*/ IGrainFactory client, Guid id) {
        var grain = client.GetGrain<IUserGrain>(id);
        return grain;
    }

#warning here
    public static async Task<(User? user, bool created)> GetUserByUserName(this IClusterClient client, string username, bool createIfNeeded, Operation operation) {
        return await client.GetUserCollectionGrain().GetUserByUserName(username, createIfNeeded, operation);
    }
}

//