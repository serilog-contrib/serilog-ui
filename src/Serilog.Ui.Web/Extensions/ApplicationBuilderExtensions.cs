using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Serilog.Ui.Web
{
    /// <summary>
    ///     Contains extensions for configuring routing on an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds a <see cref="SerilogUiMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="applicationBuilder">
        ///     The <see cref="IApplicationBuilder"/> to add the middleware to.
        /// </param>
        /// <param name="options"> The options to configure SerilogUI dashboard. </param>
        /// <returns> IApplicationBuilder. </returns>
        /// <exception cref="ArgumentNullException"> throw if applicationBuilder if null </exception>
        public static IApplicationBuilder UseSerilogUi(this IApplicationBuilder applicationBuilder, Action<UiOptions> options = null)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            var uiOptions = new UiOptions();
            options?.Invoke(uiOptions);

            var scope = applicationBuilder.ApplicationServices.CreateScope();
            var authOptions = scope.ServiceProvider.GetService<AuthorizationOptions>();
            uiOptions.AuthType = authOptions.AuthenticationType.ToString();

            scope.Dispose();

            return applicationBuilder.UseMiddleware<SerilogUiMiddleware>(uiOptions);
        }
    }
}