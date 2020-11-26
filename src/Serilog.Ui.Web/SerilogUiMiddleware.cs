using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Ui.Core;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Serilog.Ui.Web
{
    public class SerilogUiMiddleware
    {
        private const string EmbeddedFileNamespace = "Serilog.Ui.Web.wwwroot.dist";
        private readonly UiOptions _options;
        private readonly StaticFileMiddleware _staticFileMiddleware;
        private readonly JsonSerializerSettings _jsonSerializerOptions;

        public SerilogUiMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            UiOptions options)
        {
            _options = options;
            _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory);
            _jsonSerializerOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/api/logs/?$", RegexOptions.IgnoreCase))
            {
                var result = await FetchLogsAsync(httpContext);
                httpContext.Response.StatusCode = 200;
                httpContext.Response.ContentType = "application/json;charset=utf-8";
                await httpContext.Response.WriteAsync(result);
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$", RegexOptions.IgnoreCase))
            {
                var indexUrl = httpContext.Request.GetEncodedUrl().TrimEnd('/') + "/index.html";

                RespondWithRedirect(httpContext.Response, indexUrl);
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?index.html$", RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            await _staticFileMiddleware.Invoke(httpContext);
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

        private void RespondWithRedirect(HttpResponse response, string location)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = location;
        }

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";

            await using var stream = IndexStream();
            var htmlBuilder = new StringBuilder(await new StreamReader(stream).ReadToEndAsync());
            htmlBuilder.Replace("%(Configs)", JsonConvert.SerializeObject(_options, _jsonSerializerOptions));

            await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        }

        private Func<Stream> IndexStream { get; } = () => typeof(AuthorizationOptions).GetTypeInfo().Assembly
            .GetManifestResourceStream("Serilog.Ui.Web.wwwroot.index.html");

        private async Task<string> FetchLogsAsync(HttpContext httpContext)
        {
            httpContext.Request.Query.TryGetValue("page", out var pageStr);
            httpContext.Request.Query.TryGetValue("count", out var countStr);

            int.TryParse(pageStr, out var page);
            int.TryParse(countStr, out var count);
            page = page == default ? 1 : page;
            count = count == default ? 10 : count;

            var provider = httpContext.RequestServices.GetService<IDataProvider>();
            var (logs, total) = await provider.FetchDataAsync(page, count);

            //var result = JsonSerializer.Serialize(logs, _jsonSerializerOptions);
            var result = JsonConvert.SerializeObject(new { logs, total, page, count }, _jsonSerializerOptions);
            return result;
        }
    }
}