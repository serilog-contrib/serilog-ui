using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Authorization
{
    internal class AuthorizationFilterService : IAuthorizationFilterService
    {
        public async Task CheckAccessAsync(HttpContext httpContext,
            UiOptions options,
            Func<HttpContext, Task> onSuccess,
            Func<HttpResponse, Task> onFailure = null)
        {
            var accessCheck = await CanAccessAsync(httpContext, options);
            if (!accessCheck)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                if (onFailure != null)
                {
                    await onFailure.Invoke(httpContext.Response);
                }
                return;
            }

            await onSuccess(httpContext);
        }

        private static async Task<bool> CanAccessAsync(HttpContext httpContext, UiOptions options)
        {
            var syncFilterResult = options.Authorization.Filters.Any(filter => !filter.Authorize(httpContext));

            var asyncFilter = await Task.WhenAll(options.Authorization.AsyncFilters.Select(filter => filter.AuthorizeAsync(httpContext)));
            var asyncFilterResult = Array.Exists(asyncFilter, filter => !filter);

            return !syncFilterResult && !asyncFilterResult;
        }
    }
}
