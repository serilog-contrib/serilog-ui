using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Serilog.Ui.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsLocal(this HttpRequest request)
        {
            var ipAddress = request.Headers["X-forwarded-for"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                return false;
            }

            var connection = request.HttpContext.Connection;
            if (connection.RemoteIpAddress != null)
            {
                return connection.LocalIpAddress != null
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            // we know remote ip is null, thus it can be only be local
            return true;
        }
    }
}