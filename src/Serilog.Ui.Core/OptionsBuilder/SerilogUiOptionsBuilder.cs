using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    /// <summary>
    /// SerilogUi OptionsBuilder class, used during app services registration.
    /// Implements <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    public class SerilogUiOptionsBuilder : ISerilogUiOptionsBuilder
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// It creates an instance of <see cref="SerilogUiOptionsBuilder"/>.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public SerilogUiOptionsBuilder(IServiceCollection services)
        {
            _services = services;
        }

        IServiceCollection ISerilogUiOptionsBuilder.Services => _services;
    }
}