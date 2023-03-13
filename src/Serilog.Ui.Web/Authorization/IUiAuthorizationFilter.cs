using Microsoft.AspNetCore.Http;

namespace Serilog.Ui.Web.Authorization
{
    public interface IUiAuthorizationFilter
    {
        bool Authorize(HttpContext httpContext);
    }
}