using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using System;
using System.Linq;

namespace Serilog.Ui.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogUi(
            this IServiceCollection services,
            IMvcBuilder mvcBuilder,
            Action<DataProviderOptionsBuilder> optionsBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            var builder = new DataProviderOptionsBuilder(services);
            optionsBuilder.Invoke(builder);

            //var assembly = typeof(ServiceCollectionExtensions).Assembly;
            // var location = assembly.Location;
            //var embeddedFileProvider = new EmbeddedFileProvider(assembly);
            //mvcBuilder.AddRazorRuntimeCompilation(options => options.FileProviders.Add(embeddedFileProvider));

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

            return services;
        }

        private static void ConfigureApplicationParts(ApplicationPartManager apm)
        {
            apm.ApplicationParts.Add(new AssemblyPart(typeof(ServiceCollectionExtensions).Assembly));
        }
    }
}