using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Authorization
{
    internal interface IAuthorizationFilterService
    {
        Task CheckAccessAsync(HttpContext httpContext,
            UiOptions options,
            Func<HttpContext, Task> onSuccess,
            Func<HttpResponse, Task> onFailure = null);
    }
}
