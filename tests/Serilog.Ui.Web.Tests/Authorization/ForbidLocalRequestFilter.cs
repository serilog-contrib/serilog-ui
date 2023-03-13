using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;

namespace Serilog.Ui.Web.Tests.Authorization;

internal class ForbidLocalRequestFilter : IUiAuthorizationFilter
{
    public bool Authorize(HttpContext httpContext)
    {
        return !httpContext.Request.IsLocal();
    }
}