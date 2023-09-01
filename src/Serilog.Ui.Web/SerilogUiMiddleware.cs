using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Ui.Web.Endpoints;
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

            if (isGet)
            {
                uiAppRoutes.SetOptions(_options);
                uiEndpoints.SetOptions(_options);

                if (CheckPath(path, "/api/keys/?")) return uiEndpoints.GetApiKeys(httpContext);
                if (CheckPath(path, "/api/logs/?")) return uiEndpoints.GetLogs(httpContext);
                if (CheckPath(path, "/?")) return uiAppRoutes.RedirectHome(httpContext);
                if (CheckPath(path, "/?index.html")) return uiAppRoutes.GetHome(httpContext);
            }

            return _staticFileMiddleware.Invoke(httpContext);
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

        private bool CheckPath(string currentPath, string OnPath)
            => Regex.IsMatch(currentPath, $"^/{Regex.Escape(_options.RoutePrefix)}{OnPath}$", RegexOptions.IgnoreCase);
    }
}