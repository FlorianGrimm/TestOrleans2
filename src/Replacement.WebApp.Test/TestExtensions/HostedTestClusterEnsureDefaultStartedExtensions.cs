namespace Replacement.TestExtensions;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static class HostedTestBaseClusterStartedExtensions {
    public static RequestOperation CreateRequestOperation<PK, T>(
        this TestClusterTestingBase that,
        [AllowNull] PK pk,
        [AllowNull] T argument,
        string username,
        [CallerMemberName] string? callerMemberName = default
        ) {
        var activityId = System.Diagnostics.Activity.Current?.Id ?? String.Empty;
        var operationName = GetOperationName(that, callerMemberName);
        var argumentJSON = (argument is null)
            ? null
            : System.Text.Json.JsonSerializer.Serialize<T>(argument);
        var result = RequestOperation.Create(
            RequestLogId: Guid.NewGuid(),
            OperationId: Guid.NewGuid(),
            ActivityId: activityId,
            OperationName: operationName,
            EntityType: nameof(T),
            EntityId: pk?.ToString() ?? string.Empty,
            Argument: argumentJSON,
            UserName: username,
            UserId: null
        );
        return result;
    }

    public static string GetOperationName(
        this TestClusterTestingBase that,
        [CallerMemberName] string? callerMemberName = default
        ) {
        var controllerName = that.GetType().Name;
        return $"{controllerName}.{callerMemberName}";
    }

    public static async Task<(OperationEntity operation, UserEntity? user)> InitializeOperation(
            this TestClusterTestingBase that,
            RequestOperation requestOperation,
            IRequestLogService? requestLogService=default,
            bool canModifyState=true,
            bool createUserIfNeeded=true) {

        return await requestOperation.InitializeOperation(
            canModifyState: canModifyState,
            createUserIfNeeded: createUserIfNeeded,
            client: that.Client,
            requestLogService: requestLogService  ?? that.CreateTestRequestLogService()
            //?? that.ClusterClient.ServiceProvider.GetService<IRequestLogService>()
            );
    }

    public static IRequestLogService CreateTestRequestLogService(
        this TestClusterTestingBase that
    ) {
        return new TestRequestLogService(that.Fixture.TestOutputHelper);
    }
}
