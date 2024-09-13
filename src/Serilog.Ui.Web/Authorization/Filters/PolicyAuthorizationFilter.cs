using Microsoft.AspNetCore.Authorization;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization.Filters;

internal class PolicyAuthorizationFilter(
    IHttpContextAccessor httpContextAccessor,
    IAuthorizationService authorizationService,
    string policy
    ) : IUiAsyncAuthorizationFilter
{
    private readonly HttpContext _httpContext = Guard.Against.Null(httpContextAccessor.HttpContext);

    public async Task<bool> AuthorizeAsync()
    {
        AuthorizationResult result = await authorizationService.AuthorizeAsync(_httpContext.User, policy);
        return result.Succeeded;
    }
}