using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using System.Text.Encodings.Web;

namespace Replacement.TestExtensions;

public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions {
}

public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationSchemeOptions> {
    private const string Prefix = "UnitTest:";

    public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) {

    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        if (Request.Headers.TryGetValue(HeaderNames.Authorization, out StringValues authorizationHeader)) {
            var authorizationHeaderValue = authorizationHeader.First(s => s.StartsWith(Prefix));
            if (authorizationHeaderValue is not null) {
                var username = authorizationHeaderValue.Substring(Prefix.Length);
                if (!string.IsNullOrEmpty(username)) {
                    var claims = new System.Security.Claims.Claim[] {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username)
                    };
                    var identity = new System.Security.Claims.ClaimsIdentity(claims, this.Scheme.Name);
                    var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
