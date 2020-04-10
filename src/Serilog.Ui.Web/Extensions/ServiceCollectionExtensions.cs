using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Web.Filters;
using System;
using System.Linq;

namespace Serilog.Ui.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogUi(
            this IServiceCollection services,
            IMvcBuilder mvcBuilder,
            Action<SerilogUiOptionsBuilder> optionsBuilder
            )
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            var builder = new SerilogUiOptionsBuilder(services);
            optionsBuilder.Invoke(builder);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Serilog.Ui.Web")).ToList();
            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.Contains("Views"))
                    mvcBuilder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(assembly));
                else
                {
                    mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
                    var manifestResourceNames = assembly.GetManifestResourceNames();
                }
            }

            var isAuthorizationFilterExist = services.Any(s => s.ServiceType.FullName == typeof(AuthorizationOptions).FullName);
            if (!isAuthorizationFilterExist)
                services.AddScoped<AuthorizationOptions>();

            services.AddScoped<AuthorizationFilter>();

            return services;
        }
    }
}