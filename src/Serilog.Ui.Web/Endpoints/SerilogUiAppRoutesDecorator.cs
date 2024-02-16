using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiAppRoutesDecorator(ISerilogUiAppRoutes decoratedService, IAuthorizationFilterService authFilterService) : ISerilogUiAppRoutes
    {
        private readonly ISerilogUiAppRoutes _decoratedService = decoratedService;
        private readonly IAuthorizationFilterService _authFilterService = authFilterService;

        public UiOptions Options { get; private set; }

        public Task GetHomeAsync(HttpContext httpContext)
        {
            Guard.Against.Null(Options, nameof(Options));

            static Task ChangeResponseAsync(HttpResponse response)
            {
                response.ContentType = "text/html;charset=utf-8";
                return response.WriteAsync("<p>You don't have enough permission to access this page!</p>", Encoding.UTF8);
            }

            return Options.Authorization.RunAuthorizationFilterOnAppRoutes ?
                _authFilterService.CheckAccessAsync(httpContext, Options, _decoratedService.GetHomeAsync, ChangeResponseAsync) :
                _decoratedService.GetHomeAsync(httpContext);
        }

        public Task RedirectHomeAsync(HttpContext httpContext)
        {
            Guard.Against.Null(Options, nameof(Options));

            return Options.Authorization.RunAuthorizationFilterOnAppRoutes ?
                _authFilterService.CheckAccessAsync(httpContext, Options, _decoratedService.RedirectHomeAsync) :
                _decoratedService.RedirectHomeAsync(httpContext);
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
            _decoratedService.SetOptions(options);
        }
    }
}
