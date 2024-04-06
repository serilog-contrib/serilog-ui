using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.Core.OptionsBuilder
{
    /// <summary>
    /// SerilogUi OptionsBuilder class, used during app services registration.
    /// Implements <see cref="ISerilogUiOptionsBuilder"/>.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public class SerilogUiOptionsBuilder(IServiceCollection services) : ISerilogUiOptionsBuilder
    {
        IServiceCollection ISerilogUiOptionsBuilder.Services { get; } = services;
    }
}