using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Ui.Web.Endpoints;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serilog.Ui.Web
{
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
            var path = httpContext.Request.Path.Value;
            var httpMethod = httpContext.Request.Method;
            var isGet = httpMethod == "GET";

            if (!isGet)
            {
                return _staticFileMiddleware.Invoke(httpContext);
            }

            uiAppRoutes.SetOptions(_options);
            uiEndpoints.SetOptions(_options);

            if (CheckPath(path, "/api/keys/?")) return uiEndpoints.GetApiKeysAsync(httpContext);
            if (CheckPath(path, "/api/logs/?")) return uiEndpoints.GetLogsAsync(httpContext);
            if (CheckPath(path, "/index.html")) return uiAppRoutes.RedirectHomeAsync(httpContext);
            if (CheckPath(path, "/(?:.*(.*/))(?:.*(assets/)).*"))
            {
                ChangeAssetRequestPath(httpContext);
            }

            return CheckPath(path, "/(?!.*(assets/)).*") ? uiAppRoutes.GetHomeAsync(httpContext) : _staticFileMiddleware.Invoke(httpContext);
        }

        private StaticFileMiddleware CreateStaticFileMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory)
        {
            var staticFileOptions = new StaticFileOptions
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
        private void ChangeAssetRequestPath(HttpContext httpContext)
        {
            var from = $"{_options.RoutePrefix}/";
            const string to = "assets/";
            
            var startOfWrongAssetSubPath = httpContext.Request.Path.Value.IndexOf(from, StringComparison.Ordinal) + from.Length;
            var endOfWrongAssetSubPath = httpContext.Request.Path.Value.IndexOf(to, StringComparison.OrdinalIgnoreCase);
            var pathToRemove = httpContext.Request.Path.Value.Substring(startOfWrongAssetSubPath, endOfWrongAssetSubPath - startOfWrongAssetSubPath);
            
            httpContext.Request.Path = httpContext.Request.Path.Value.Replace(pathToRemove, "");
        }
    }
}