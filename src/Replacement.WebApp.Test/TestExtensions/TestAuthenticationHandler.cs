using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using System.Net.Http.Headers;
using System.Text.Encodings.Web;

namespace Replacement.TestExtensions;

public class TestAuthenticationDefaults {
    public const string AuthenticationScheme = "Test";
}

public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions {
    public TestAuthenticationSchemeOptions() {
        this.TestUsersClaims = new Dictionary<string, System.Security.Claims.Claim[]>();
        this.TestUsers = new Dictionary<string, System.Security.Claims.ClaimsPrincipal>();
    }

    public bool AcceptAllUsers { get; set; }
    public Dictionary<string, System.Security.Claims.Claim[]> TestUsersClaims;
    public Dictionary<string, System.Security.Claims.ClaimsPrincipal> TestUsers;

    public TestAuthenticationSchemeOptions AddTestUser(string username) {
        var claims = new System.Security.Claims.Claim[] {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username)
            };
        this.TestUsersClaims.Add(username, claims);
        return this;
    }
}

public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationSchemeOptions> {

    public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) {

    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        if (Request.Headers.TryGetValue(HeaderNames.Authorization, out StringValues authorizationHeader)) {
            if (AuthenticationHeaderValue.TryParse(authorizationHeader.First(), out var authenticationHeaderValue)) {
                if (string.Equals(authenticationHeaderValue.Scheme, this.Scheme.Name, StringComparison.Ordinal)) {
                    var username = authenticationHeaderValue.Parameter;
                    if (!string.IsNullOrEmpty(username)) {
                        var options = this.OptionsMonitor.Get(this.Scheme.Name);
                        if (options.TestUsers.TryGetValue(username, out var principal)) {
                            //OK
                        } else if (options.TestUsersClaims.TryGetValue(username, out var claims)) {
                            var identity = new System.Security.Claims.ClaimsIdentity(claims, this.Scheme.Name);
                            principal = new System.Security.Claims.ClaimsPrincipal(identity);
                        } else if (options.AcceptAllUsers) {
                            claims = new System.Security.Claims.Claim[] {
                                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username)
                            };
                            var identity = new System.Security.Claims.ClaimsIdentity(claims, this.Scheme.Name);
                            principal = new System.Security.Claims.ClaimsPrincipal(identity);
                        } else {
                            return Task.FromResult(AuthenticateResult.Fail("Unknown User"));
                        }
                        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                }
            }
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}

public static class TestAuthenticationHandlerExtensions {

    public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder)
        => builder.AddTestAuthentication(TestAuthenticationDefaults.AuthenticationScheme, _ => { });

    public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, Action<TestAuthenticationSchemeOptions> configureOptions)
    => builder.AddTestAuthentication(TestAuthenticationDefaults.AuthenticationScheme, configureOptions);

    public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, string authenticationScheme, Action<TestAuthenticationSchemeOptions> configureOptions)
        => builder.AddTestAuthentication(authenticationScheme, displayName: null, configureOptions: configureOptions);

    public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<TestAuthenticationSchemeOptions> configureOptions) {
        //builder.Services.AddOptions<TestAuthenticationSchemeOptions>(authenticationScheme).Configure(configureOptions);
        return builder.AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }

    public static AuthenticationHeaderValue CreateAuthorizationHeader(string username) {
        return CreateAuthorizationHeader(TestAuthenticationDefaults.AuthenticationScheme, username);
    }
    public static AuthenticationHeaderValue CreateAuthorizationHeader(string scheme, string username) {
        return new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, username);
    }

}
