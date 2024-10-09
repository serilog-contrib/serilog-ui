using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Endpoints;

namespace Serilog.Ui.Web.Extensions;

/// <summary>
///   Extension methods for setting up SerilogUI related services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///   Registers the SerilogUI as a service in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="optionsBuilder">
    ///   An action to configure the <see cref="ISerilogUiOptionsBuilder"/> for the SerilogUI.
    /// </param>
    /// <exception cref="ArgumentNullException">services</exception>
    /// <exception cref="ArgumentNullException">optionsBuilder</exception>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddSerilogUi(this IServiceCollection services, Action<ISerilogUiOptionsBuilder> optionsBuilder)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(optionsBuilder, nameof(optionsBuilder));
        var isProviderAlreadyRegistered = services.Any(c => c.ImplementationType == typeof(AggregateDataProvider));
        if (isProviderAlreadyRegistered)
        {
            throw new InvalidOperationException($"{nameof(AddSerilogUi)} can be invoked one time per service registration.");
        }

        var builder = new SerilogUiOptionsBuilder(services);
        optionsBuilder.Invoke(builder);
        builder.RegisterProviderServices();

        services.AddHttpContextAccessor();
        services.AddScoped<IAuthorizationFilterService, AuthorizationFilterService>();
        services.AddSingleton<IAppStreamLoader, AppStreamLoader>();

        services.AddScoped<SerilogUiEndpoints>();
        services.AddScoped<ISerilogUiEndpoints>(sp => new SerilogUiEndpointsDecorator(
            sp.GetRequiredService<SerilogUiEndpoints>(),
            sp.GetRequiredService<IAuthorizationFilterService>()));

        services.AddScoped<SerilogUiAppRoutes>();
        services.AddScoped<ISerilogUiAppRoutes>(sp => new SerilogUiAppRoutesDecorator(
            sp.GetRequiredService<IHttpContextAccessor>(),
            sp.GetRequiredService<SerilogUiAppRoutes>(),
            sp.GetRequiredService<IAuthorizationFilterService>()));
        
        services.AddScoped<AggregateDataProvider>();

        return services;
    }
}