using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.Web
{
    public static class SerilogUiOptionBuilderExtensions
    {
        public static SerilogUiOptionsBuilder EnableAuthorization(this SerilogUiOptionsBuilder optionsBuilder, Action<AuthorizationOptions> options)
        {
            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var authorizationOptions = new AuthorizationOptions { Enabled = true };
            options.Invoke(authorizationOptions);

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton<AuthorizationOptions>(authorizationOptions);

            return optionsBuilder;
        }
    }
}