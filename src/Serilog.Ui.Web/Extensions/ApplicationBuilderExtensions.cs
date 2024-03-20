using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using System;

namespace Serilog.Ui.Web
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
        /// <exception cref="ArgumentNullException">throw if applicationBuilder if null</exception>
        public static IApplicationBuilder UseSerilogUi(this IApplicationBuilder applicationBuilder, Action<UiOptions> options = null)
        {
            Guard.Against.Null(applicationBuilder, nameof(applicationBuilder));
            // TODO disable timestamp in case of elastic...
            var uiOptions = new UiOptions();
            options?.Invoke(uiOptions);

            return applicationBuilder.UseMiddleware<SerilogUiMiddleware>(uiOptions);
        }
    }
}