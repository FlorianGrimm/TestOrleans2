// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


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
    public static string GetOperationData(
        this ReplacementControllerBase that
        ) {

        var identity = that.HttpContext.User.Identity;
        var username = (identity is not null && identity.IsAuthenticated) ? (identity?.Name) : null;
        var request = that.HttpContext.Request;
        var method = request.Method;
        var path = request.Path.Value ?? string.Empty;

        //HttpContextInfo httpContextInfo;
        //if (string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase)) {
        //    httpContextInfo = HttpContextInfo.ConvertFrom(username, method, path, null);
        //} else if (string.Equals(request.Method, "POST", StringComparison.OrdinalIgnoreCase)) {
        //    httpContextInfo = HttpContextInfo.ConvertFrom(username, method, path, request.Form);
        //} else if (string.Equals(request.Method, "PUT", StringComparison.OrdinalIgnoreCase)) {
        //    httpContextInfo = HttpContextInfo.ConvertFrom(username, method, path, request.Form);
        //} else if (string.Equals(request.Method, "DELETE", StringComparison.OrdinalIgnoreCase)) {
        //    httpContextInfo = HttpContextInfo.ConvertFrom(username, method, path, null);
        //} else {
        //    httpContextInfo = HttpContextInfo.ConvertFrom(username, method, path, null);
        //}
        var httpContextInfo = HttpContextInfo.ConvertFrom(username, method, path, request.Form);
        var result = System.Text.Json.JsonSerializer.Serialize(httpContextInfo);
        return result;
    }
    public static string GetOperationTitle(
        this ReplacementControllerBase that,
        [CallerMemberName] string? callerMemberName = default
        ) {
        var controllerName = that.GetType().Name;
        return $"{controllerName}.{callerMemberName}";
    }
    public static async Task<User?> GetUserByUserName(this ReplacementControllerBase that, Operation operation) {
        var identity = that.HttpContext.User.Identity;
        if (identity is null) {
            return null;
        }
        if (!identity.IsAuthenticated) {
            return null;
        }
        var username = identity.Name;
        if (username is null) {
            return null;
        }
        var user = await that.Client.GetUserCollectionGrain().GetUserByUserName(username, operation);
        return user;
    }
}