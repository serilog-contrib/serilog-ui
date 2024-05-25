using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization
{
    internal sealed class AuthorizationFilterService(
        IHttpContextAccessor httpContextAccessor,
        IEnumerable<IUiAuthorizationFilter> syncFilters,
        IEnumerable<IUiAsyncAuthorizationFilter> asyncFilters) : IAuthorizationFilterService
    {
        public async Task CheckAccessAsync(
            Func<Task> onSuccess,
            Func<Task>? onFailure = null)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext is null) return;

            var accessCheck = await CanAccessAsync();

            if (accessCheck)
            {
                await onSuccess();
                return;
            }

            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            if (onFailure != null)
            {
                await onFailure.Invoke();
            }
        }

        private async Task<bool> CanAccessAsync()
        {
            var syncFilterResult = syncFilters.Any(filter => !filter.Authorize());

            var asyncFilter = await Task.WhenAll(asyncFilters.Select(filter => filter.AuthorizeAsync()));
            var asyncFilterResult = Array.Exists(asyncFilter, filter => !filter);

            return !syncFilterResult && !asyncFilterResult;
        }
    }
}