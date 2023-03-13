using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;

namespace SampleWebApp.Authentication;

public class SerilogUiCustomAuthFilter : IUiAuthorizationFilter
{
    public bool Authorize(HttpContext httpContext)
    {
        return httpContext.User.Identity is { IsAuthenticated: true };
    }
}