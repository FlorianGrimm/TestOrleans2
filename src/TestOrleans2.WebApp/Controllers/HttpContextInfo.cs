// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace TestOrleans2.WebApp.Controllers;

public record HttpContextInfo(
    string? Username,
    string Method,
    string Path,
    List<KeyValuePair<string, List<string>>>? Form,
    string? ArgumentType,
    object? Argument
    //string Argument
    ) {

    public static List<KeyValuePair<string, List<string>>> ConvertRequestForm(
        IFormCollection? form
        ) {
        var lstForm = new List<KeyValuePair<string, List<string>>>();
        if (form is not null) {
            foreach (var kv in form) {
                var values = new List<string>();
                foreach (var value in kv.Value) {
                    values.Add(value);
                }
                lstForm.Add(new KeyValuePair<string, List<string>>(kv.Key, values));
            }
        }
        return lstForm;
    }

    /*
     * TODO
    public HttpContext ConvertToX() {
        var result = new DefaultHttpContext();
        result.Request.Method
        return result;
    }
    */

    public static string Serialize(HttpContextInfo httpContextInfo) {
        var result = System.Text.Json.JsonSerializer.Serialize(httpContextInfo);
        return result;
    }

    public HttpContextInfo? Deserialize(string json) {
        var httpContextInfo = System.Text.Json.JsonSerializer.Deserialize<HttpContextInfo>(json);
        return httpContextInfo;
    }
}
