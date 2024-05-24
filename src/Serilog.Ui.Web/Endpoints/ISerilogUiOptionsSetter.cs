using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Endpoints
{
    public interface ISerilogUiOptionsSetter
    {
        UiOptions? Options { get; }

        void SetOptions(UiOptions options);
    }
}