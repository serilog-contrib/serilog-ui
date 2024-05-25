using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiAppRoutes : ISerilogUiOptionsSetter
    {
        protected internal bool BlockHomeAccess { get; set; }

        Task GetHomeAsync();

        Task RedirectHomeAsync();
    }
}