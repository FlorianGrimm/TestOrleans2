﻿using Replacement.Contracts.Entity;

namespace Replacement.Repository.Grains;

public interface IUserCollectionGrain : IGrainWithGuidKey {
    Task<List<UserEntity>> GetAllUsers(OperationEntity operation);
    Task<(UserEntity? user, bool created)> GetUserByUserName(string username, bool createIfNeeded, OperationEntity operation);
    Task SetDirty(UserEntity? user);
}

public interface IUserGrain : IGrainWithGuidKey {
    Task<UserEntity?> GetUser(OperationEntity operation);
    Task<UserEntity?> UpsertUser(UserEntity value, UserEntity? currentUser, OperationEntity operation);
    Task<bool> DeleteUser(UserEntity? currentUser, OperationEntity operation);
}

public class UserCollectionGrain : Grain, IUserCollectionGrain {
    private readonly IDBContext _DBContext;
    private Dictionary<string, UserEntity>? _Cache;

    public UserCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public Task<List<UserEntity>> GetAllUsers(OperationEntity operation) {
        throw new NotImplementedException();
    }

    public async Task<(UserEntity? user, bool created)> GetUserByUserName(string username, bool createIfNeeded, OperationEntity operation) {
        var cache = this._Cache ??= new Dictionary<string, UserEntity>(StringComparer.OrdinalIgnoreCase);
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
                user = UserEntity.Create(
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

    public Task SetDirty(UserEntity? user) {
        if (user is null) {
            this._Cache = null;
        } else if (this._Cache is not null) {
            this._Cache[user.UserName] = user;
        }
        return Task.CompletedTask;
    }
}

public class UserGrain : GrainBase<UserEntity>, IUserGrain {

    public UserGrain(
        IDBContext dbContext
        ):base(dbContext) {
    }

    protected override async Task<UserEntity?> Load() {
        var pk = new UserPK(
                this.GetGrainIdentity().PrimaryKey
            );
        using (var dataAccess = await this._DBContext.GetDataAccessAsync()) {
            var result = await dataAccess.ExecuteUserSelectPKAsync(pk);
            if (result is null) {
                return null;
            } else {
                this._DBContext.User.Attach(result);
                return result;
            }
        }
    }

    public async Task<UserEntity?> GetUser(OperationEntity operation) {
        var user = await this.GetState();
        if (user is null) {
            this.DeactivateOnIdle();
        }
        return user;
    }

    public async Task<UserEntity?> UpsertUser(UserEntity value, UserEntity? currentUser, OperationEntity operation) {
        var user = await this.GetState();
        value = value.SetOperation(operation);
        this._DBContext.Operation.Add(operation);
        var resultTO = this._DBContext.User.Upsert(value);
        await this._DBContext.ApplyChangesAsync();

        var result = resultTO.Value;
        this._State = result;
        await this.PopulateDirty(result);
        return result;
    }

    public async Task<bool> DeleteUser(UserEntity? currentUser, OperationEntity operation) {
        var user = await this.GetState();
        if (user is null) {
            return false;
        } else {
            var value = user.SetOperation(operation);
            this._DBContext.Operation.Add(operation);
            this._DBContext.User.Delete(value);
            await this._DBContext.ApplyChangesAsync();

            this._State = null;
            await this.PopulateDirty(null);
            this.DeactivateOnIdle();
            return true;
        }
    }

    private async Task PopulateDirty(UserEntity? user) {
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
    public static async Task<(UserEntity? user, bool created)> GetUserByUserName(this IClusterClient client, string username, bool createIfNeeded, OperationEntity operation) {
        return await client.GetUserCollectionGrain().GetUserByUserName(username, createIfNeeded, operation);
    }
}

//