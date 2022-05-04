namespace Replacement.Repository.Grains;
#if false
public interface IUserToDoCollectionGrain : IGrainWithGuidKey {
}
#endif

public interface IUserToDoGrain : IGrainWithGuidKey {
    Task<List<ToDoEntity>> GetUsersToDos(OperationEntity operation);
    Task<List<ToDoEntity>> GetAllToDos(OperationEntity operation);
}

#if false
public class UserToDoCollectionGrain : Grain, IUserToDoCollectionGrain {
    public UserToDoCollectionGrain() : base() {
    }
}
#endif

public class UserToDoGrain : GrainBase<UserEntity>, IUserToDoGrain {
    private CachedValue<List<ToDoEntity>> _GetAllToDos;

    public UserToDoGrain(IDBContext dBContext) : base(dBContext) {
        this._GetAllToDos = new CachedValue<List<ToDoEntity>>();
    }

    public async Task<List<ToDoEntity>> GetAllToDos(OperationEntity operation) {
        if (!this._GetAllToDos.TryGetValue(out var result)) {
#warning TODO check user            
            using (var sqlAccess = await this._DBContext.GetDataAccessAsync()) {
                result = await sqlAccess.ExecuteToDoSelectAllAsync();
            }
            if (result.Count < 1000) {
                this._GetAllToDos = this._GetAllToDos.SetStatus(result);
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
}

//

public static partial class GrainExtensions {
#if false
    public static IUserToDoCollectionGrain GetUserToDoCollectionGrain(this /*IClusterClient*/ IGrainFactory client) {
        var grain = client.GetGrain<IUserToDoCollectionGrain>(Guid.Empty);
        return grain;
    }
#endif

    public static IUserToDoGrain GetUserToDoGrain(this /*IClusterClient*/ IGrainFactory client, Guid id) {
        var grain = client.GetGrain<IUserToDoGrain>(id);
        return grain;
    }
}

//