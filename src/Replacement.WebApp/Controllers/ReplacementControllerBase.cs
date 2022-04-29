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
    public static string GetOperationData(this ReplacementControllerBase that)
        => GetOperationData<object?>(that, null);

    public static string GetOperationData<T>(
        this ReplacementControllerBase that,
        T body
        ) {

        var identity = that.HttpContext.User.Identity;
        var username = (identity is not null && identity.IsAuthenticated) ? (identity?.Name) : null;
        var request = that.HttpContext.Request;
        var method = request.Method;
        var path = request.Path.Value ?? string.Empty;

        List<KeyValuePair<string, List<string>>>? lstForm = null;
        try {
            IFormCollection? requestForm = null;
            var contentType = request.ContentType;
            if ((!string.IsNullOrEmpty(contentType))
                && (contentType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)
                    || contentType.Equals("multipart/form-data", StringComparison.OrdinalIgnoreCase)
                )) {
                requestForm = request.Form;
                if (requestForm.Any()) {
                    lstForm = HttpContextInfo.ConvertRequestForm(requestForm);
                }
            }
        } catch {
            lstForm = null;
        }

        //string argumentType;
        //string argument;
        //try {
        //    argumentType = typeof(T).FullName ?? String.Empty;
        //    argument = (body is null)
        //        ? String.Empty
        //        : System.Text.Json.JsonSerializer.Serialize<T>(body);
        //} catch {
        //    argumentType = string.Empty;
        //    argument = string.Empty;
        //}

        var argumentType = typeof(T).FullName;

        var httpContextInfo = new HttpContextInfo(
                Username: username,
                Method: method,
                Path: path,
                Form: lstForm,
                ArgumentType: argumentType,
                Argument: body
            );
        var result = System.Text.Json.JsonSerializer.Serialize<HttpContextInfo>(httpContextInfo);
        return result;
    }

    public static string GetOperationTitle(
        this ReplacementControllerBase that,
        [CallerMemberName] string? callerMemberName = default
        ) {
        var controllerName = that.GetType().Name;
        return $"{controllerName}.{callerMemberName}";
    }

    public static async Task<(Operation operation, User? user)> GetUserByUserName(
        this ReplacementControllerBase that,
        Operation operation,
        bool createIfNeeded = true) {
        var identity = that.HttpContext.User.Identity;
        if (identity is null) {
            return (operation, null);
        }
        if (!identity.IsAuthenticated) {
            return (operation, null);
        }
        var username = identity.Name;
        if (username is null) {
            return (operation, null);
        }
        var userCollectionGrain = that.Client.GetUserCollectionGrain();
        var (user, created) = await userCollectionGrain.GetUserByUserName(username, createIfNeeded, operation);
        if (created) {
            var operationNext = operation with {
                OperationId = Guid.NewGuid(),
                UserId = user?.UserId,
                CreatedAt = DateTimeOffset.Now
            };
            return (operationNext, user);
        } else {
            var operationNext = operation with {
                UserId = user?.UserId
            };
            return (operationNext, user);
        }
    }
}