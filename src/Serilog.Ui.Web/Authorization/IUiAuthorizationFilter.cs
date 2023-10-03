using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Authorization
{
    public interface IUiAuthorizationFilter
    {
        bool Authorize(HttpContext httpContext);
    }

    public interface IUiAsyncAuthorizationFilter
    {
        Task<bool> AuthorizeAsync(HttpContext httpContext);
    }
}