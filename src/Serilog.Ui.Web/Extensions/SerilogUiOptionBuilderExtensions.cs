using Microsoft.AspNetCore.Builder;
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
            options(authorizationOptions);

            ((ISerilogUiOptionsBuilder)optionsBuilder).Services.AddSingleton(authorizationOptions);

            return optionsBuilder;
        }

        public static IApplicationBuilder UseSerilogUi(this IApplicationBuilder applicationBuilder, Action<UiOptions> options = null)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            var uiOptions = new UiOptions();
            options?.Invoke(uiOptions);

            var authOptions = applicationBuilder.ApplicationServices.GetService<AuthorizationOptions>();
            if (authOptions != null)
                uiOptions.AuthType = authOptions.AuthenticationType;

            return applicationBuilder.UseMiddleware<SerilogUiMiddleware>(uiOptions);
        }
    }
}