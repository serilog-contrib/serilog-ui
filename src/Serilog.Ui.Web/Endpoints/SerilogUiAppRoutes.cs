using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace Serilog.Ui.Web.Endpoints
{
    internal class SerilogUiAppRoutes(IAppStreamLoader appStreamLoader) : ISerilogUiAppRoutes
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public UiOptions Options { get; private set; }

        public async Task GetHomeAsync(HttpContext httpContext)
        {
            Guard.Against.Null(Options, nameof(Options));

            var response = httpContext.Response;

            await using var stream = appStreamLoader.GetIndex();
            if (stream is null)
            {
                response.StatusCode = 500;
                await response.WriteAsync("<div>Server error while loading assets. Please contact administration.</div>", Encoding.UTF8);
                return;
            }

            var htmlString = await LoadStream(stream, Options);
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";

            await response.WriteAsync(htmlString, Encoding.UTF8);
        }

        public Task RedirectHomeAsync(HttpContext httpContext)
        {
            var indexUrl = httpContext.Request.GetEncodedUrl().Replace("index.html", "");

            httpContext.Response.StatusCode = 301;
            httpContext.Response.Headers["Location"] = indexUrl;

            return Task.CompletedTask;
        }

        public void SetOptions(UiOptions options)
        {
            Options = options;
        }

        private static async Task<string> LoadStream(Stream stream, UiOptions options)
        {
            var htmlStringBuilder = new StringBuilder(await new StreamReader(stream).ReadToEndAsync());
            var authType = options.Authorization.AuthenticationType.ToString();
            var encodeAuthOpts = Uri.EscapeDataString(JsonSerializer.Serialize(new { options.RoutePrefix, authType, options.HomeUrl }, JsonSerializerOptions));

            htmlStringBuilder
                .Replace("%(Configs)", encodeAuthOpts)
                .Replace("<meta name=\"dummy\" content=\"%(HeadContent)\">", options.HeadContent)
                .Replace("<meta name=\"dummy\" content=\"%(BodyContent)\">", options.BodyContent);

            return htmlStringBuilder.ToString();
        }
    }
}
