using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
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
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            var builder = new SerilogUiOptionsBuilder(services);
            optionsBuilder.Invoke(builder);

            return services;
        }
    }
}