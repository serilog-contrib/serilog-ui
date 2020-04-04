using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;

namespace Serilog.Ui.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogUi(this IServiceCollection services, Action<DataProviderOptionBuilder> optionsBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            var builder = new DataProviderOptionBuilder(services);
            optionsBuilder.Invoke(builder);

            return services;
        }
    }
}