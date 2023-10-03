using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiEndpoints : ISerilogUiOptionsSetter
    {
        Task GetApiKeysAsync(HttpContext httpContext);

        Task GetLogsAsync(HttpContext httpContext);
    }
}
