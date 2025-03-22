using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints;

internal class SerilogUiAppRoutes(IHttpContextAccessor httpContextAccessor, IAppStreamLoader appStreamLoader)
    : ISerilogUiAppRoutes
{
    private readonly HttpContext _httpContext = Guard.Against.Null(httpContextAccessor.HttpContext);

    public bool BlockHomeAccess { get; set; }

    public UiOptions? Options { get; private set; }

    public void SetOptions(UiOptions options)
    {
        Options = options;
    }

    public async Task GetHomeAsync()
    {
        Guard.Against.Null(Options, nameof(Options));

        var response = _httpContext.Response;

        await using Stream? stream = appStreamLoader.GetIndex();
        if (stream is null)
        {
            response.StatusCode = 500;
            await response.WriteAsync("<div>Server error while loading assets. Please contact administration.</div>", Encoding.UTF8);
            return;
        }

        response.StatusCode = 200;
        response.ContentType = "text/html;charset=utf-8";
        string htmlString = await LoadStream(stream, Options);

        await response.WriteAsync(htmlString, Encoding.UTF8);
    }

    public Task RedirectHomeAsync()
    {
        string indexUrl = _httpContext.Request.GetEncodedUrl().Replace("index.html", "");
        string indexUrlWithTrailingSlash = indexUrl.EndsWith('/') ? indexUrl : $"{indexUrl}/";

        _httpContext.Response.Redirect(indexUrlWithTrailingSlash, true);

        return Task.CompletedTask;
    }

    private async Task<string> LoadStream(Stream stream, UiOptions options)
    {
        StringBuilder htmlStringBuilder = new(await new StreamReader(stream).ReadToEndAsync());
        string authType = options.Authorization.AuthenticationType.ToString();

        var feOpts = new
        {
            authType,
            options.ColumnsInfo,
            options.DisabledSortOnKeys,
            options.RenderExceptionAsStringKeys,
            options.ShowBrand,
            options.HomeUrl,
            BlockHomeAccess,
            routePrefix = ConstructRoutesPrefix(options),
            options.ExpandDropdownsByDefault
        };
        string encodeAuthOpts = Uri.EscapeDataString(JsonSerializer.Serialize(feOpts, JsonSerializerOptionsFactory.GetDefaultOptions));

        htmlStringBuilder
            .Replace("%(Configs)", encodeAuthOpts)
            .Replace("<meta name=\"dummy\" content=\"%(HeadContent)\">", options.HeadContent)
            .Replace("<meta name=\"dummy\" content=\"%(BodyContent)\">", options.BodyContent);

        return htmlStringBuilder.ToString();
    }

    private static string ConstructRoutesPrefix(UiOptions options)
    {
        var safeHostPath = string.IsNullOrWhiteSpace(options.ServerSubPath) ? "" : options.ServerSubPath;
        var hostPathWithoutInitialSlash = safeHostPath.StartsWith("/", StringComparison.OrdinalIgnoreCase) ?
            safeHostPath[1..] : safeHostPath;
        var hostPathWithDivider = !string.IsNullOrWhiteSpace(hostPathWithoutInitialSlash) &&
            !hostPathWithoutInitialSlash.EndsWith('/') ?
            $"{hostPathWithoutInitialSlash}/" : hostPathWithoutInitialSlash;

        return $"{hostPathWithDivider}{options.RoutePrefix}";
    }
}