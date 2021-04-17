using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    /// <summary>
    /// Serilog UI options builder
    /// </summary>
    public interface ISerilogUiOptionsBuilder
    {
        /// <summary>
        /// Gets the services collection.
        /// </summary>
        /// <value>The services.</value>
        IServiceCollection Services { get; }
    }
}