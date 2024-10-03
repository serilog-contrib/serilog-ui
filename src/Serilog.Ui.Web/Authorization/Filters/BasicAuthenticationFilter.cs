using System.Net.Http.Headers;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization.Filters;

internal class BasicAuthenticationFilter(
    IHttpContextAccessor httpContextAccessor,
    IBasicAuthenticationService basicAuthenticationService)
    : IUiAsyncAuthorizationFilter
{
    private const string AuthenticationScheme = "Basic";
    private readonly HttpContext _httpContext = Guard.Against.Null(httpContextAccessor.HttpContext);

    public async Task<bool> AuthorizeAsync()
    {
        StringValues header = _httpContext.Request.Headers.Authorization;
        AuthenticationHeaderValue authValues = AuthenticationHeaderValue.Parse(header!);

        return
            // not basic, thus no evaluation should happen
            !IsBasicAuthentication(authValues) ||
            // if basic, evaluate header
            await basicAuthenticationService.CanAccessAsync(authValues);
    }

    private static bool IsBasicAuthentication(AuthenticationHeaderValue authValues)
    {
        return AuthenticationScheme.Equals(authValues.Scheme, StringComparison.OrdinalIgnoreCase);
    }
}