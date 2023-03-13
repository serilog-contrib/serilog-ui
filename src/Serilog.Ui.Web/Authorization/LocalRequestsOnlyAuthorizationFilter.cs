using Microsoft.AspNetCore.Http;

namespace Serilog.Ui.Web.Authorization
{
    public class LocalRequestsOnlyAuthorizationFilter : IUiAuthorizationFilter
    {
        public bool Authorize(HttpContext httpContext)
        {
            return httpContext.Request.IsLocal();
        }
    }
}