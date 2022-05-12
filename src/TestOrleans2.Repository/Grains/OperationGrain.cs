namespace Replacement.Repository.Grains;

public interface IOperationCollectionGrain : IGrainWithGuidKey {
    Task<List<OperationEntity>> GetAllOperation(OperationFilter filter, OperationEntity operation);
    Task<OperationRelatedEntity?> GetOperation(OperationEntity operation);
}

public class OperationCollectionGrain : Grain, IOperationCollectionGrain {
    private readonly IDBContext _DBContext;

    public OperationCollectionGrain(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
    }

    public async Task<List<OperationEntity>> GetAllOperation(OperationFilter filter, OperationEntity operation) {
        List<OperationEntity> result;
        using (var dataAccess = await this._DBContext.GetDataAccessAsync()) {
            result = await dataAccess.ExecuteOperationSelectAllAsync(filter);
        }
        return result;

    }

#warning HERE
    public Task<OperationRelatedEntity?> GetOperation(OperationEntity operation) {
        throw new NotImplementedException();
    }
}

//

public static partial class GrainExtensions {
    public static IOperationCollectionGrain GetOperationCollectionGrain(this /*IClusterClient*/ IGrainFactory client) {
        var grain = client.GetGrain<IOperationCollectionGrain>(Guid.Empty);
        return grain;
    }
}

//