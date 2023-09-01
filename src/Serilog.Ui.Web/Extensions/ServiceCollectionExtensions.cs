using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog.Ui.Core;
using Serilog.Ui.Web.Authorization;
using Serilog.Ui.Web.Endpoints;
using System;

namespace Serilog.Ui.Web
{
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
        ///   An action to configure the <see cref="SerilogUiOptionsBuilder"/> for the SerilogUI.
        /// </param>
        /// <exception cref="ArgumentNullException">services</exception>
        /// <exception cref="ArgumentNullException">optionsBuilder</exception>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddSerilogUi(this IServiceCollection services, Action<SerilogUiOptionsBuilder> optionsBuilder)
        {
#if NET6_0_OR_GREATER
            Guard.Against.Null(services);
            Guard.Against.Null(optionsBuilder);
#else
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(optionsBuilder, nameof(optionsBuilder));
#endif

            var builder = new SerilogUiOptionsBuilder(services);
            optionsBuilder.Invoke(builder);

            services.AddScoped<IAuthorizationFilterService, AuthorizationFilterService>();
            services.AddSingleton<IAppStreamLoader, AppStreamLoader>();

            services.AddScoped<ISerilogUiEndpoints, SerilogUiEndpoints>();
            services.Decorate<ISerilogUiEndpoints, SerilogUiEndpointsDecorator>();

            services.AddScoped<ISerilogUiAppRoutes, SerilogUiAppRoutes>();
            services.Decorate<ISerilogUiAppRoutes, SerilogUiAppRoutesDecorator>();
            
            services.TryAddScoped(typeof(AggregateDataProvider));

            return services;
        }
    }
}