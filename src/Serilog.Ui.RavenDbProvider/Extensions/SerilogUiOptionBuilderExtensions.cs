using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Serilog.Ui.Core;

namespace Serilog.Ui.RavenDbProvider.Extensions;

/// <summary>
///   RavenDB's data provider specific extension methods for <see cref="ISerilogUiOptionsBuilder"/>.
/// </summary>
public static class SerilogUiOptionBuilderExtensions
{
    /// <summary>
    ///   Configures the SerilogUi to connect to a RavenDB database.
    /// </summary>
    /// <param name="optionsBuilder">The Serilog UI options builder.</param>
    /// <param name="setupOptions">The RavenDb options action.</param>
    /// <exception cref="ArgumentNullException">throw if documentStore is null</exception>
    public static void UseRavenDb(this ISerilogUiOptionsBuilder optionsBuilder, Action<RavenDbOptions> setupOptions)
    {
        var dbOptions = new RavenDbOptions();
        setupOptions(dbOptions);
        dbOptions.Validate();

        // TODO: Fix up RavenDB to allow multiple registrations. Think about multiple RavenDB clients
        // (singletons) used in data providers (scoped)
        if (optionsBuilder.Services.Any(c => c.ImplementationType == typeof(RavenDbDataProvider)))
        {
            throw new NotSupportedException($"Adding multiple registrations of '{typeof(RavenDbDataProvider).FullName}' is not (yet) supported.");
        }

        optionsBuilder.Services.AddSingleton(dbOptions.DocumentStore);
        optionsBuilder.Services.AddScoped<IDataProvider>(serviceProvider =>
            new RavenDbDataProvider(serviceProvider.GetRequiredService<IDocumentStore>(), dbOptions.CollectionName));
    }
}