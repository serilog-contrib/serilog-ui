using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.Models;
using Serilog.Ui.Core.Models.Options;

namespace Serilog.Ui.Core.OptionsBuilder;

/// <summary>
/// SerilogUi OptionsBuilder class, used during app services registration.
/// Implements <see cref="ISerilogUiOptionsBuilder"/>.
/// </summary>
/// <param name="services">Service collection.</param>
public class SerilogUiOptionsBuilder(IServiceCollection services) : ISerilogUiOptionsBuilder
{
    private readonly ProvidersOptions _providersOptions = new();

    /// <inheritdoc />
    public IServiceCollection Services { get; } = services;

    /// <inheritdoc />
    public void RegisterDisabledSortForProviderKey(string providerKey)
    {
        _providersOptions.RegisterDisabledSortName(providerKey);
    }

    /// <inheritdoc />
    public void RegisterColumnsInfo<T>(string providerKey) where T : LogModel
    {
        _providersOptions.RegisterType<T>(providerKey);
    }

    /// <summary>
    /// Register the <see cref="ProvidersOptions"/> in <see cref="IServiceCollection"/>.
    /// </summary>
    public void RegisterProviderServices()
    {
        Services.AddSingleton(_providersOptions);
    }
}