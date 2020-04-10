using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    public interface ISerilogUiOptionsBuilder
    {
        IServiceCollection Services { get; }
    }
}