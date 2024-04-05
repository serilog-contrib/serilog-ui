using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;
using Serilog.Ui.Core.OptionsBuilder;

namespace Serilog.Ui.Web.Tests.Utilities.InMemoryDataProvider
{
    internal static class SerilogUiOptionBuilderExtensions
    {
        /// <summary>
        ///   Configures the SerilogUi to connect to a InMemory sink.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        public static ISerilogUiOptionsBuilder UseInMemory(this ISerilogUiOptionsBuilder optionsBuilder)
        {
            optionsBuilder.Services.AddScoped<IDataProvider, SerilogInMemoryDataProvider>();
            return optionsBuilder;
        }
    }
}