using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;
using System.Linq;

namespace Serilog.Ui.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogUi(this IServiceCollection services, Action<SerilogUiOptionsBuilder> optionsBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            var builder = new SerilogUiOptionsBuilder(services);
            optionsBuilder.Invoke(builder);

            var isAuthorizationFilterExist = services.Any(s => s.ServiceType.FullName == typeof(AuthorizationOptions).FullName);
            if (!isAuthorizationFilterExist)
                services.AddScoped<AuthorizationOptions>();

            return services;
        }
    }
}