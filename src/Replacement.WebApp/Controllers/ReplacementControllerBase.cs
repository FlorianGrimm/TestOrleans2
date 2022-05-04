// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Replacement.Contracts.Service;

using System.Diagnostics.CodeAnalysis;

namespace Replacement.WebApp.Controllers;

public class ReplacementControllerBase : ControllerBase {
    public readonly IClusterClient Client;
    public readonly ILogger Logger;

    protected ReplacementControllerBase(
        IClusterClient client,
        ILogger logger
        ) {
        this.Client = client;
        this.Logger = logger;
    }
}

public static class ReplacementControllerBaseExtensions {
    public static RequestOperation CreateRequestOperation<PK, T>(
        this ReplacementControllerBase that,
        [AllowNull] PK pk,
        [AllowNull] T argument,
        [CallerMemberName] string? callerMemberName = default
        ) {
        var activityId = System.Diagnostics.Activity.Current?.Id ?? String.Empty;
        var operationName = GetOperationName(that, callerMemberName);
        var identity = that.HttpContext.User.Identity;
        var username = (identity is not null && identity.IsAuthenticated) ? identity.Name : null;
        var argumentJSON = (argument is null)
            ? null
            : System.Text.Json.JsonSerializer.Serialize<T>(argument);
        var result = new RequestOperation(
            RequestLogId: Guid.NewGuid(),
            OperationId: Guid.NewGuid(),
            ActivityId: activityId,
            OperationName: operationName,
            EntityType: nameof(T),
            EntityId: pk?.ToString() ?? string.Empty,
            Argument: argumentJSON,
            UserName: username,
            UserId: null,
            CreatedAt: DateTimeOffset.Now,
            SerialVersion: 0);
        return result;
    }

    public static string GetOperationName(
        this ReplacementControllerBase that,
        [CallerMemberName] string? callerMemberName = default
        ) {
        var controllerName = that.GetType().Name;
        return $"{controllerName}.{callerMemberName}";
    }

    public static async Task<(OperationEntity operation, UserEntity? user)> InitializeOperation(
        this ReplacementControllerBase that,
        RequestOperation requestOperation,
        bool canModifyState,
        bool createUserIfNeeded) {

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
            await InsertRequestLog(that, requestLog, canModifyState);
            return (operation, null);
        } else {
            var userCollectionGrain = that.Client.GetUserCollectionGrain();
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
                await InsertRequestLog(that, requestLog, canModifyState);
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
                await InsertRequestLog(that, requestLog, canModifyState);
                return (operationNext, user);
            }
        }
    }

    private static async Task InsertRequestLog(
        this ReplacementControllerBase that,
        RequestLogEntity requestLog, 
        bool canModifyState) {
        var service = that.HttpContext.RequestServices.GetService<IRequestLogService>();
        if (service is not null) {
            await service.InsertAsync(requestLog, canModifyState);
        }
    }
}
