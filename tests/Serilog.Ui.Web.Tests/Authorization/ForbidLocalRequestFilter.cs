using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Tests.Authorization;

internal class ForbidLocalRequestFilter : IUiAuthorizationFilter
{
    public bool Authorize(HttpContext httpContext)
    {
        return !httpContext.Request.IsLocal();
    }
}

internal class AdmitRequestFilter : IUiAuthorizationFilter
{
    public bool Authorize(HttpContext httpContext)
    {
        return !httpContext.Request.IsLocal();
    }
}

internal class ForbidLocalRequestAsyncFilter : IUiAsyncAuthorizationFilter
{
    public Task<bool> AuthorizeAsync(HttpContext httpContext)
    {
        return Task.FromResult(!httpContext.Request.IsLocal());
    }
}

internal class AdmitRequestAsyncFilter : IUiAsyncAuthorizationFilter
{
    public Task<bool> AuthorizeAsync(HttpContext httpContext)
    {
        return Task.FromResult(true);
    }
}
