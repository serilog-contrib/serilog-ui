using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints;

internal class AppStreamLoader : IAppStreamLoader
{
    private const string AppManifest = "Serilog.Ui.Web.wwwroot.dist.index.html";

    public Stream? GetIndex() =>
        typeof(AuthorizationOptions)
            .Assembly
            .GetManifestResourceStream(AppManifest);
}