namespace Replacement.Repository.Extensions;
public static class RequestOperationExtensions {
    public static async Task<(OperationEntity operation, UserEntity? user)> InitializeOperation(
        this RequestOperation requestOperation,
        bool canModifyState,
        bool createUserIfNeeded,
        IGrainFactory client,
        IRequestLogService? requestLogService
        ) {

        var operation = new OperationEntity(
            OperationId: requestOperation.OperationId,
            OperationName: requestOperation.OperationName,
            EntityType: requestOperation.EntityType,
            EntityId: requestOperation.EntityId,
            UserId: requestOperation.UserId,
            CreatedAt: requestOperation.CreatedAt,
            SerialVersion: 0);
        if (operation.UserId.HasValue
            || string.IsNullOrEmpty(requestOperation.UserName)) {
            var requestLog = new RequestLogEntity(
                RequestLogId: requestOperation.RequestLogId,
                OperationId: requestOperation.OperationId,
                ActivityId: requestOperation.ActivityId,
                OperationName: requestOperation.OperationName,
                EntityType: requestOperation.EntityType,
                EntityId: requestOperation.EntityId,
                Argument: requestOperation.Argument,
                UserId: requestOperation.UserId,
                CreatedAt: requestOperation.CreatedAt,
                SerialVersion: 0);
            if (requestLogService is not null) {
                await requestLogService.InsertAsync(requestLog, canModifyState);
            }
            return (operation, null);
        } else {
            var userCollectionGrain = client.GetUserCollectionGrain();
            var (user, created) = await userCollectionGrain.GetUserByUserName(requestOperation.UserName, createUserIfNeeded, operation);
            if (created) {
                var operationNext = operation with {
                    OperationId = Guid.NewGuid(),
                    UserId = user?.UserId,
                    CreatedAt = DateTimeOffset.Now
                };
                var requestLog = new RequestLogEntity(
                    RequestLogId: requestOperation.RequestLogId,
                    OperationId: operationNext.OperationId,
                    ActivityId: requestOperation.ActivityId,
                    OperationName: requestOperation.OperationName,
                    EntityType: requestOperation.EntityType,
                    EntityId: requestOperation.EntityId,
                    Argument: requestOperation.Argument,
                    UserId: user?.UserId,
                    CreatedAt: operationNext.CreatedAt,
                    SerialVersion: 0);
                if (requestLogService is not null) {
                    await requestLogService.InsertAsync(requestLog, canModifyState);
                }
                return (operationNext, user);
            } else {
                var operationNext = operation with {
                    UserId = user?.UserId
                };
                var requestLog = new RequestLogEntity(
                    RequestLogId: requestOperation.RequestLogId,
                    OperationId: requestOperation.OperationId,
                    ActivityId: requestOperation.ActivityId,
                    OperationName: requestOperation.OperationName,
                    EntityType: requestOperation.EntityType,
                    EntityId: requestOperation.EntityId,
                    Argument: requestOperation.Argument,
                    UserId: user?.UserId,
                    CreatedAt: requestOperation.CreatedAt,
                    SerialVersion: 0);
                if (requestLogService is not null) {
                    await requestLogService.InsertAsync(requestLog, canModifyState);
                }
                return (operationNext, user);
            }
        }
    }
    // private static async Task InsertRequestLog(
    //         IRequestLogService? requestLogService,
    //         RequestLogEntity requestLog,
    //         bool canModifyState) {
    //     //var service = that.HttpContext.RequestServices.GetService<IRequestLogService>();
    //     if (requestLogService is not null) {
    //         await requestLogService.InsertAsync(requestLog, canModifyState);
    //     }
    // }
}
