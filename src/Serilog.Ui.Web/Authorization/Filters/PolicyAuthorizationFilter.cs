using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization.Filters;

public class PolicyAuthorizationFilter(
    IHttpContextAccessor httpContextAccessor,
    IAuthorizationService authorizationService,
    string policy)
    : IUiAsyncAuthorizationFilter
{
    public async Task<bool> AuthorizeAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return false;

        var result = await authorizationService.AuthorizeAsync(httpContext.User, policy);
        return result.Succeeded;
    }
}