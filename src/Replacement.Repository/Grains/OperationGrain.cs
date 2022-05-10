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

#warning HERE
    public Task<List<OperationEntity>> GetAllOperation(OperationFilter filter, OperationEntity operation) {
        throw new NotImplementedException();
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