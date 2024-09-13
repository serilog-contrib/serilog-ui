using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Web.Extensions;

namespace Serilog.Ui.Web.Authorization.Filters;

internal class LocalRequestsOnlyAuthorizationFilter(IHttpContextAccessor httpContextAccessor) : IUiAuthorizationFilter
{
    public bool Authorize() =>
         httpContextAccessor.HttpContext is not null &&
         httpContextAccessor.HttpContext.Request.IsLocal();
}