using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization.Filters;

internal class BasicAuthenticationFilter(
    IHttpContextAccessor httpContextAccessor,
    IBasicAuthenticationService basicAuthenticationService)
    : IUiAsyncAuthorizationFilter
{
    private const string AuthenticationScheme = "Basic";

    public async Task<bool> AuthorizeAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return false;

        var header = httpContext.Request.Headers.Authorization;

        var authValues = AuthenticationHeaderValue.Parse(header);

        return
            // not basic, thus no evaluation should happen
            !IsBasicAuthentication(authValues) ||
            // if basic, evaluate header
            await basicAuthenticationService.CanAccessAsync(authValues);
    }

    private static bool IsBasicAuthentication(AuthenticationHeaderValue authValues)
    {
        return AuthenticationScheme.Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase);
    }
}