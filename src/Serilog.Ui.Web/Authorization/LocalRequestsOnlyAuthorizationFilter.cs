using Microsoft.AspNetCore.Http;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization
{
    public class LocalRequestsOnlyAuthorizationFilter(IHttpContextAccessor httpContextAccessor) : IUiAuthorizationFilter
    {
        public bool Authorize()
        {
            var httpContext = httpContextAccessor.HttpContext;

            return httpContext is not null && httpContext.Request.IsLocal();
        }
    }
}