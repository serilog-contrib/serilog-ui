using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    public interface IDataProviderOptionsBuilder
    {
        IServiceCollection Services { get; }
    }
}