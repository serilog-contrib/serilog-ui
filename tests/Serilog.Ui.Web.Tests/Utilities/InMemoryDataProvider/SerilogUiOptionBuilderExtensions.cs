using Microsoft.Extensions.DependencyInjection;
using Serilog.Ui.Core;

namespace Serilog.Ui.Web.Tests.Utilities.InMemoryDataProvider
{
    /// <summary>
    ///   SQL Server data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
    /// </summary>
    public static class SerilogUiOptionBuilderExtensions
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