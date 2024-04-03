using System;
using System.Linq;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Web.Models;

namespace Serilog.Ui.Web.Extensions
{
    /// <summary>
    ///   Contains extensions for configuring routing on an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private static readonly string[] DisabledSortProviders = ["ElasticSearch"];

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
            Guard.Against.Null(applicationBuilder);

            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var providers = scope.ServiceProvider.GetServices<IDataProvider>();
            var disabledSortKeys = providers
                .Where(sd => Array.Exists(DisabledSortProviders, pr => sd.GetType().Name.StartsWith(pr)))
                .Select(p => p.Name);

            var uiOptions = new UiOptions
            {
                DisabledSortOnKeys = disabledSortKeys
            };
            options?.Invoke(uiOptions);

            return applicationBuilder.UseMiddleware<SerilogUiMiddleware>(uiOptions);
        }
    }
}