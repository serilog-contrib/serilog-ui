using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Web.Authorization;

internal sealed class AuthorizationFilterService(
    IHttpContextAccessor httpContextAccessor,
    IEnumerable<IUiAuthorizationFilter> syncFilters,
    IEnumerable<IUiAsyncAuthorizationFilter> asyncFilters
    ) : IAuthorizationFilterService
{
    private readonly HttpContext _httpContext = Guard.Against.Null(httpContextAccessor.HttpContext);

    public async Task CheckAccessAsync(Func<Task> onSuccess, Func<Task>? onFailure = null)
    {
        bool accessCheck = await CanAccessAsync();
        if (accessCheck)
        {
            await onSuccess();
            return;
        }

        _httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        if (onFailure != null)
        {
            await onFailure.Invoke();
        }
    }

    private async Task<bool> CanAccessAsync()
    {
        // Evaluate all synchronous filters and check if any of them deny access.
        bool syncAuthorizeResult = syncFilters.Any(filter => !filter.Authorize());

        // Evaluate all asynchronous filters and check if any of them deny access.
        bool[] asyncFilter = await Task.WhenAll(asyncFilters.Select(filter => filter.AuthorizeAsync()));
        bool asyncAuthorizeResult = Array.Exists(asyncFilter, filter => !filter);

        // Return true if all filters grant access, otherwise return false.
        return !syncAuthorizeResult && !asyncAuthorizeResult;
    }
}