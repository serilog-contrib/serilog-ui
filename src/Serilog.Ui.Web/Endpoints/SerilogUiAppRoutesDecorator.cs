using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints;

internal class SerilogUiAppRoutesDecorator(
    IHttpContextAccessor httpContextAccessor,
    ISerilogUiAppRoutes decoratedService,
    IAuthorizationFilterService authFilterService
    ) : ISerilogUiAppRoutes
{
    private readonly HttpContext _httpContext = Guard.Against.Null(httpContextAccessor.HttpContext);

    public bool BlockHomeAccess { get; set; }

    public UiOptions? Options { get; private set; }

    public void SetOptions(UiOptions options)
    {
        Options = options;
        decoratedService.SetOptions(options);
    }

    public async Task GetHomeAsync()
    {
        Guard.Against.Null(Options, nameof(Options));

        if (Options.Authorization.RunAuthorizationFilterOnAppRoutes)
        {
            await authFilterService.CheckAccessAsync(() => Task.CompletedTask, OnAccessFailure);
        }

        await decoratedService.GetHomeAsync();

        Task OnAccessFailure()
        {
            _httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            decoratedService.BlockHomeAccess = true;

            return Task.CompletedTask;
        }
    }

    public Task RedirectHomeAsync()
    {
        Guard.Against.Null(Options, nameof(Options));

        return decoratedService.RedirectHomeAsync();
    }
}