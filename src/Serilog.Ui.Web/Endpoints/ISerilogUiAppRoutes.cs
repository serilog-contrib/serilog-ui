using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiAppRoutes: ISerilogUiOptionsSetter
    {
        Task GetHomeAsync();

        Task RedirectHomeAsync();
    }
}
