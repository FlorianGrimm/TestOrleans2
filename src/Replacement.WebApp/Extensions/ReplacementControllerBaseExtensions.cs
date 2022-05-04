namespace Replacement.WebApp.Extensions;
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
        var result = RequestOperation.Create(
            RequestLogId: Guid.NewGuid(),
            OperationId: Guid.NewGuid(),
            ActivityId: activityId,
            OperationName: operationName,
            EntityType: typeof(T).Name,
            EntityId: pk?.ToString() ?? string.Empty,
            Argument: argumentJSON,
            UserName: username,
            UserId: null
        );
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
        return await requestOperation.InitializeOperation(
            canModifyState: canModifyState,
            createUserIfNeeded: createUserIfNeeded,
            client: that.Client,
            requestLogService: that.HttpContext.RequestServices.GetService<IRequestLogService>()
            );
    }
}
