using System.Threading.Tasks;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiEndpoints : ISerilogUiOptionsSetter
    {
        Task GetApiKeysAsync();

        Task GetLogsAsync();
    }
}
