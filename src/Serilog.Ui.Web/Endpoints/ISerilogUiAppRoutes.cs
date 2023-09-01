using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiAppRoutes: ISerilogUiOptionsSetter
    {
        Task GetHome(HttpContext httpContext);

        Task RedirectHome(HttpContext httpContext);
    }
}
