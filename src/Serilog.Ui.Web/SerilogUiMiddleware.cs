using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Serilog.Ui.Web.Endpoints;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web;

public class SerilogUiMiddleware
{
    private const string EmbeddedFileNamespace = "Serilog.Ui.Web.wwwroot.dist";
    private readonly UiOptions _options;
    private readonly StaticFileMiddleware _staticFileMiddleware;

    public SerilogUiMiddleware(
        RequestDelegate next,
        IWebHostEnvironment hostingEnv,
        ILoggerFactory loggerFactory,
        UiOptions options)
    {
        _options = options;
        _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory);
    }

    public Task Invoke(HttpContext httpContext, ISerilogUiAppRoutes uiAppRoutes, ISerilogUiEndpoints uiEndpoints)
    {
        string? path = httpContext.Request.Path.Value;
        string httpMethod = httpContext.Request.Method;
        bool isGet = httpMethod == "GET";

        if (!isGet || path is null)
        {
            return _staticFileMiddleware.Invoke(httpContext);
        }

        uiAppRoutes.SetOptions(_options);
        uiEndpoints.SetOptions(_options);

        if (CheckPath(path, "/api/keys/?"))
        {
            return uiEndpoints.GetApiKeysAsync();
        }

        if (CheckPath(path, "/api/logs/?"))
        {
            return uiEndpoints.GetLogsAsync();
        }

        if (CheckPath(path, "/api/dashboard/?"))
        {
            return uiEndpoints.GetDashboardAsync();
        }

        // prefix without trailing slash or old index.html routing
        if (CheckPath(path, "/index.html") || CheckPath(path, ""))
        {
            return uiAppRoutes.RedirectHomeAsync();
        }

        // asset request, we remove any extra path part since it's always served from the serilog-ui root
        if (CheckPath(path, "/(?:.*(.*/))(?:(assets/)).*"))
        {
            return ChangeAssetRequestPath(httpContext);
        }

        return CheckPath(path, "/(?!.*(assets/)).*") ? uiAppRoutes.GetHomeAsync() : _staticFileMiddleware.Invoke(httpContext);
    }

    private StaticFileMiddleware CreateStaticFileMiddleware(
        RequestDelegate next,
        IWebHostEnvironment hostingEnv,
        ILoggerFactory loggerFactory)
    {
        StaticFileOptions staticFileOptions = new()
        {
            RequestPath = $"/{_options.RoutePrefix}",
            FileProvider = new EmbeddedFileProvider(typeof(SerilogUiMiddleware).GetTypeInfo().Assembly, EmbeddedFileNamespace),
        };

        return new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
    }

    private bool CheckPath(string currentPath, string onPath)
        => Regex.IsMatch(currentPath, $"^/{Regex.Escape(_options.RoutePrefix)}{onPath}$", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));

    /// <summary>
    /// Since the FE app includes compiled js assets as relative urls, when the page has sub-paths the app tries to access the asset
    /// from the whole sub-paths url.
    /// <br/>
    /// The StaticFileMiddleware always expose it from the serilog-ui route prefix. We remove the sub-paths from the path value and let the middleware do its job.
    /// <br />
    /// <br />
    /// Example: if user opens [...]/serilog-ui/other-route/my-log-id,
    /// the FE would search for main.js into [...]/serilog-ui/other-route/main.js instead of [...]/serilog-ui/main.js.
    /// </summary>
    private Task ChangeAssetRequestPath(HttpContext httpContext)
    {
        string from = $"{_options.RoutePrefix}/";
        const string to = "assets/";

        string requestPath = httpContext.Request.GetEncodedUrl();
        int startOfWrongAssetSubPath = requestPath.IndexOf(from, StringComparison.Ordinal) + from.Length;
        int endOfWrongAssetSubPath = requestPath.IndexOf(to, StringComparison.OrdinalIgnoreCase);
        string pathToRemove = requestPath.Substring(startOfWrongAssetSubPath, endOfWrongAssetSubPath - startOfWrongAssetSubPath);
        string newPath = requestPath.Replace(pathToRemove, string.Empty);

        httpContext.Response.Redirect(newPath, true);

        return Task.CompletedTask;
    }
}