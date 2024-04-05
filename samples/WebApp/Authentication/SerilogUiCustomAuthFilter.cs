using Serilog.Ui.Core.Interfaces;

namespace WebApp.Authentication;

public class SerilogUiCustomAuthFilter(IHttpContextAccessor httpContextAccessor) : IUiAuthorizationFilter
{
    public bool Authorize()
    {
        return httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true };
    }
}