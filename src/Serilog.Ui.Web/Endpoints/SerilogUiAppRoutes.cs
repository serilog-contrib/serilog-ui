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
            RoutePrefix = ConstructRoutesPrefix(options),
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
        // If ServerSubPath is empty, just return the RoutePrefix
        if (string.IsNullOrWhiteSpace(options.ServerSubPath)) return options.RoutePrefix;

        // Create a span to avoid allocations when slicing
        ReadOnlySpan<char> path = options.ServerSubPath.AsSpan();
        // Skip leading slash if present
        if (path.Length > 0 && path[0] == '/')
        {
            path = path[1..];
        }

        // If the path is empty after removing the slash, just return the RoutePrefix
        if (path.Length == 0)
        {
            return options.RoutePrefix;
        }

        // Check if we need a trailing slash
        bool needsTrailingSlash = path.Length > 0 && path[^1] != '/';
        // Build the final string with minimal allocations
        return needsTrailingSlash ? string.Concat(path.ToString(), "/", options.RoutePrefix) : string.Concat(path.ToString(), options.RoutePrefix);
    }
}