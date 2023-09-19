using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
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
