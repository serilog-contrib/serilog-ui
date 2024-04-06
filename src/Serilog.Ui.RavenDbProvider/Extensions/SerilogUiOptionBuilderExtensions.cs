using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Interfaces;

namespace Serilog.Ui.RavenDbProvider.Extensions;

/// <summary>
///   RavenDB's data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
/// </summary>
public static class SerilogUiOptionBuilderExtensions
{
    /// <summary>
    ///   Configures the SerilogUi to connect to a RavenDB database.
    ///   It supports multiple registration by collection. 
    /// </summary>
    /// <param name="optionsBuilder">The Serilog UI options builder.</param>
    /// <param name="setupOptions">The RavenDb options action.</param>
    /// <exception cref="ArgumentNullException">throw if documentStore is null</exception>
    public static ISerilogUiOptionsBuilder UseRavenDb(this ISerilogUiOptionsBuilder optionsBuilder, Action<RavenDbOptions> setupOptions)
    {
        var dbOptions = new RavenDbOptions();
        setupOptions(dbOptions);
        dbOptions.Validate();

        optionsBuilder.Services.AddSingleton(dbOptions.DocumentStore);
        optionsBuilder.Services.AddScoped<IDataProvider>(serviceProvider =>
            new RavenDbDataProvider(serviceProvider.GetRequiredService<IDocumentStore>(), dbOptions.CollectionName));
        
        return optionsBuilder;
    }
}