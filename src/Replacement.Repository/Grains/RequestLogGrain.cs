using Orleans.Concurrency;

namespace Replacement.Repository.Grains;
public interface IRequestLogCollectionGrain : IGrainWithGuidKey {
    Task<List<RequestLogEntity>> GetAllRequestLogs(UserEntity user, OperationEntity operation);
    Task<List<RequestLogEntity>> GetUsersRequestLogs(UserEntity user, OperationEntity operation);
}

/*
public interface IRequestLogGrain : IGrainWithGuidKey {
    Task<RequestLogEntity?> GetRequestLog(UserEntity user, OperationEntity operation);
    Task<RequestLogContext?> GetRequestLogContext(UserEntity user, OperationEntity operation);
    Task<RequestLogEntity?> UpsertRequestLog(RequestLogEntity value, UserEntity user, OperationEntity operation);
    Task<bool> DeleteRequestLog(UserEntity user, OperationEntity operation);
}
*/

public partial class RequestLogCollectionGrain : GrainCollectionBase, IRequestLogCollectionGrain {
    private readonly ILogger _Logger;

    public RequestLogCollectionGrain(
        IDBContext dbContext,
        ILogger<RequestLogCollectionGrain> logger
        ) : base(dbContext) {
        this._Logger = logger;
    }

#warning here
    public Task<List<RequestLogEntity>> GetAllRequestLogs(UserEntity user, OperationEntity operation) {
        throw new NotImplementedException();
    }

#warning here
    public Task<List<RequestLogEntity>> GetUsersRequestLogs(UserEntity user, OperationEntity operation) {
        throw new NotImplementedException();
    }


    //[LoggerMessage(
    //    EventId = (int)LogEventId.RequestLogCollectionGrain_LogSubscripe,
    //    Level = LogLevel.Trace,
    //    Message = "Subscripe RequestLogGrainObserver:{grainId};")]
    //private partial void LogSubscripe(string grainId);

}

//

public static partial class GrainExtensions {
    public static IRequestLogCollectionGrain GetRequestLogCollectionGrain(this /*IClusterClient*/ IGrainFactory client) {
        var grain = client.GetGrain<IRequestLogCollectionGrain>(Guid.Empty);
        return grain;
    }

    //public static IRequestLogGrain GetRequestLogGrain(this /*IClusterClient*/ IGrainFactory client, Guid id) {
    //    var grain = client.GetGrain<IRequestLogGrain>(id);
    //    return grain;
    //}
}

//