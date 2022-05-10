using System.Net.Http.Headers;

namespace Replacement.Client;

//public partial interface IReplacementClient { }

public partial class ReplacementClient : IReplacementClient {

    public System.Net.Http.HttpClient HttpClient { get => this._httpClient; }

    //partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder) { }
    public Func<HttpClient, HttpRequestMessage, string, AuthenticationHeaderValue>? OnGetAuthorizationHeader;

    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url) {
        if (this.OnGetAuthorizationHeader is not null) {
            AuthenticationHeaderValue authenticationHeaderValue = this.OnGetAuthorizationHeader(client, request, url);
            request.Headers.Authorization = authenticationHeaderValue;
        }
    }

    // partial void ProcessResponse(HttpClient client, HttpResponseMessage response) { }

    // partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings) { }
}