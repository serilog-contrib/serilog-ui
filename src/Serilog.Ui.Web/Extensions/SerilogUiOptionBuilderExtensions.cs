using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.Web
{
    /// <summary>
    ///     Extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///     In order to give appropriate rights for production use, you need to enables
        ///     configuring authorization. By default only local requests have access to the log
        ///     dashboard page.
        /// </summary>
        /// <param name="optionsBuilder"> The builder being used to configure the SerilogUI. </param>
        /// <param name="options"> An action to allow configure authorization. </param>
        /// <returns> SerilogUiOptionsBuilder. </returns>
        /// <exception cref="ArgumentNullException"> Throw if optionsBuilder is null </exception>
        /// <exception cref="ArgumentNullException"> Throw if options is null </exception>
        public static SerilogUiOptionsBuilder EnableAuthorization(this SerilogUiOptionsBuilder optionsBuilder, Action<AuthorizationOptions> options)
        {
            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var authorizationOptions = new AuthorizationOptions { Enabled = true };
            options(authorizationOptions);

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(authorizationOptions);

            return optionsBuilder;
        }
    }
}