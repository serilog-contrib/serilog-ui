using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Ui.Core
{
    /// <summary>
    /// SerilogUi OptionsBuilder class, used during app services registration.
    /// Implements <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public class SerilogUiOptionsBuilder(IServiceCollection services) : ISerilogUiOptionsBuilder
    {
        private readonly IServiceCollection _services = services;

        IServiceCollection ISerilogUiOptionsBuilder.Services => _services;
    }
}