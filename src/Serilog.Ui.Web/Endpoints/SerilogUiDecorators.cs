using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiAppRoutesDecorator : ISerilogUiAppRoutes
    {
        private readonly ISerilogUiAppRoutes _decoratedService;
        private readonly IAuthorizationFilterService _authFilterService;

        public SerilogUiAppRoutesDecorator(ISerilogUiAppRoutes decoratedService, IAuthorizationFilterService authFilterService)
        {
            _decoratedService = decoratedService;
            _authFilterService = authFilterService;
        }
        public UiOptions Options { get; private set; }

        public Task GetHome(HttpContext httpContext)
        {
#if NET6_0_OR_GREATER
            Guard.Against.Null(Options);
#else
            Guard.Against.Null(Options, nameof(Options));
#endif

            var changeResponse = (HttpResponse response) =>
            {
                response.ContentType = "text/html;charset=utf-8";
                return response.WriteAsync("<p>You don't have enough permission to access this page!</p>", Encoding.UTF8);
            };

            return Options.Authorization.RunAuthorizationFilterOnAppRoutes ?
                _authFilterService.CheckAccess(httpContext, Options, _decoratedService.GetHome, changeResponse) :
                _decoratedService.GetHome(httpContext);
        }

        public Task RedirectHome(HttpContext httpContext)
        {
#if NET6_0_OR_GREATER
            Guard.Against.Null(Options);
#else
            Guard.Against.Null(Options, nameof(Options));
#endif

            return Options.Authorization.RunAuthorizationFilterOnAppRoutes ?
                _authFilterService.CheckAccess(httpContext, Options, _decoratedService.RedirectHome) :
                _decoratedService.RedirectHome(httpContext);
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
            _decoratedService.SetOptions(options);
        }
    }

    internal class SerilogUiEndpointsDecorator : ISerilogUiEndpoints
    {
        private readonly ISerilogUiEndpoints _decoratedService;
        private readonly IAuthorizationFilterService _authFilterService;

        public SerilogUiEndpointsDecorator(ISerilogUiEndpoints decoratedService, IAuthorizationFilterService authFilterService)
        {
            _decoratedService = decoratedService;
            _authFilterService = authFilterService;
        }

        public UiOptions Options { get; private set; }

        public Task GetApiKeys(HttpContext httpContext)
        {
            return _authFilterService.CheckAccess(httpContext, Options, _decoratedService.GetApiKeys);
        }

        public Task GetLogs(HttpContext httpContext)
        {
            return _authFilterService.CheckAccess(httpContext, Options, _decoratedService.GetLogs);
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
            _decoratedService.SetOptions(options);
        }
    }
}
