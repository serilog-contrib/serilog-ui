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

        public Task GetApiKeysAsync(HttpContext httpContext)
        {
            return _authFilterService.CheckAccessAsync(httpContext, Options, _decoratedService.GetApiKeysAsync);
        }

        public Task GetLogsAsync(HttpContext httpContext)
        {
            return _authFilterService.CheckAccessAsync(httpContext, Options, _decoratedService.GetLogsAsync);
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
            _decoratedService.SetOptions(options);
        }
    }
}
