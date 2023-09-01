using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiEndpoints : ISerilogUiOptionsSetter
    {
        Task GetApiKeys(HttpContext httpContext);

        Task GetLogs(HttpContext httpContext);
    }
}
