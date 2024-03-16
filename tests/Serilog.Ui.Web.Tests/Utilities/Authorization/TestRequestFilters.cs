using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Tests.Utilities.Authorization
{
    internal class ForbidLocalRequestFilter(IHttpContextAccessor contextAccessor) : IUiAuthorizationFilter
    {
        public bool Authorize()
        {
            return !contextAccessor.HttpContext!.Request.IsLocal();
        }
    }

    internal class AdmitRequestFilter(IHttpContextAccessor contextAccessor) : IUiAuthorizationFilter
    {
        public bool Authorize()
        {
            return !contextAccessor.HttpContext!.Request.IsLocal();
        }
    }

    internal class ForbidLocalRequestAsyncFilter(IHttpContextAccessor contextAccessor) : IUiAsyncAuthorizationFilter
    {
        public Task<bool> AuthorizeAsync()
        {
            return Task.FromResult(!contextAccessor.HttpContext!.Request.IsLocal());
        }
    }

    internal class AdmitRequestAsyncFilter : IUiAsyncAuthorizationFilter
    {
        public Task<bool> AuthorizeAsync()
        {
            return Task.FromResult(true);
        }
    }
}
