using System;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core.OptionsBuilder;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Extensions
{
    /// <summary>
    ///   Contains extensions for configuring routing on an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///   Adds a <see cref="SerilogUiMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="applicationBuilder">
        ///   The <see cref="IApplicationBuilder"/> to add the middleware to.
        /// </param>
        /// <param name="options">The options to configure Serilog UI dashboard.</param>
        /// <returns>IApplicationBuilder.</returns>
        /// <exception cref="ArgumentNullException">throw if applicationBuilder is null</exception>
        /// <exception cref="ArgumentException">if <see cref="UiOptions"/> validation fails</exception>
        public static IApplicationBuilder UseSerilogUi(this IApplicationBuilder applicationBuilder, Action<UiOptions> options = null)
        {
            Guard.Against.Null(applicationBuilder);

            var providerOptions = applicationBuilder.ApplicationServices.GetRequiredService<ProvidersOptions>();
            var uiOptions = new UiOptions(providerOptions);
            options?.Invoke(uiOptions);
            uiOptions.Validate();

            return applicationBuilder.UseMiddleware<SerilogUiMiddleware>(uiOptions);
        }
    }
}