using Microsoft.AspNetCore.Authorization;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization.Filters;

internal class PolicyAuthorizationFilter(
    IHttpContextAccessor httpContextAccessor,
    IAuthorizationService authorizationService,
    string policy
    ) : IUiAsyncAuthorizationFilter
{
    public async Task<bool> AuthorizeAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return false;
        }

        AuthorizationResult result = await authorizationService.AuthorizeAsync(httpContext.User, policy);
        return result.Succeeded;
    }
}