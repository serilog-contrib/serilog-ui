using System.Net;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiAppRoutesDecorator(
        IHttpContextAccessor httpContextAccessor,
        ISerilogUiAppRoutes decoratedService,
        IAuthorizationFilterService authFilterService) : ISerilogUiAppRoutes
    {
        public UiOptions Options { get; private set; }
        
        public bool BlockHomeAccess { get; set; }

        public async Task GetHomeAsync()
        {
            Guard.Against.Null(Options, nameof(Options));
            Guard.Against.Null(httpContextAccessor.HttpContext);

            if (Options.Authorization.RunAuthorizationFilterOnAppRoutes)
            {
                await authFilterService.CheckAccessAsync(() => Task.CompletedTask, OnAccessFailure);
            }

            await decoratedService.GetHomeAsync();

            Task OnAccessFailure()
            {
                httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                decoratedService.BlockHomeAccess = true;

                return Task.CompletedTask;
            }
        }

        public Task RedirectHomeAsync()
        {
            Guard.Against.Null(Options, nameof(Options));

            return decoratedService.RedirectHomeAsync();
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
            decoratedService.SetOptions(options);
        }
    }
}