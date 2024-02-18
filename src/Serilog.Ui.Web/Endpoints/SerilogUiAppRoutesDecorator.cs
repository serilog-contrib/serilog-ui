using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiAppRoutesDecorator(ISerilogUiAppRoutes decoratedService, IAuthorizationFilterService authFilterService) : ISerilogUiAppRoutes
    {
        public UiOptions Options { get; private set; }

        public Task GetHomeAsync(HttpContext httpContext)
        {
            Guard.Against.Null(Options, nameof(Options));

            return Options.Authorization.RunAuthorizationFilterOnAppRoutes ?
                authFilterService.CheckAccessAsync(httpContext, Options, decoratedService.GetHomeAsync, ChangeResponseAsync) :
                decoratedService.GetHomeAsync(httpContext);
            // await httpContext.ForbidAsync(); ?

            // https://stackoverflow.com/a/73555727/15129749
            static Task ChangeResponseAsync(HttpResponse response)
            {
                response.ContentType = "text/html;charset=utf-8";
                return response.WriteAsync("<p>You don't have enough permission to access this page!</p>", Encoding.UTF8);
            }
        }

        public Task RedirectHomeAsync(HttpContext httpContext)
        {
            Guard.Against.Null(Options, nameof(Options));

            return Options.Authorization.RunAuthorizationFilterOnAppRoutes ?
                authFilterService.CheckAccessAsync(httpContext, Options, decoratedService.RedirectHomeAsync) :
                decoratedService.RedirectHomeAsync(httpContext);
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
            decoratedService.SetOptions(options);
        }
    }
}
