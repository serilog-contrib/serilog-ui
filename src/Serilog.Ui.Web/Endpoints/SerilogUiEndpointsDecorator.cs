using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints;

internal class SerilogUiEndpointsDecorator(ISerilogUiEndpoints decoratedService, IAuthorizationFilterService authFilterService)
    : ISerilogUiEndpoints
{
    public UiOptions? Options { get; private set; }

    public void SetOptions(UiOptions options)
    {
        Options = options;
        decoratedService.SetOptions(options);
    }

    public Task GetApiKeysAsync()
    {
        return authFilterService.CheckAccessAsync(decoratedService.GetApiKeysAsync);
    }

    public Task GetLogsAsync()
    {
        return authFilterService.CheckAccessAsync(decoratedService.GetLogsAsync);
    }
}