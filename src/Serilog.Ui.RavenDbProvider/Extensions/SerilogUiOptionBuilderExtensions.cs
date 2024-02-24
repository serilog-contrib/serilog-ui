using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Serilog.Ui.Core;

namespace Serilog.Ui.RavenDbProvider.Extensions;

/// <summary>
///   RavenDB's data provider specific extension methods for <see cref="SerilogUiOptionsBuilder"/>.
/// </summary>
public static class SerilogUiOptionBuilderExtensions
{
    /// <summary>
    ///   Configures the SerilogUi to connect to a RavenDB database.
    /// </summary>
    /// <param name="optionsBuilder">The Serilog UI options builder.</param>
    /// <param name="documentStore">A DocumentStore for a RavenDB database.</param>
    /// <param name="collectionName"> Name of the collection to query logs. default value is <c>LogEvents</c>.</param>
    /// <exception cref="ArgumentNullException">throw if documentStore is null</exception>
    public static void UseRavenDb(this SerilogUiOptionsBuilder optionsBuilder, IDocumentStore documentStore, string collectionName = "LogEvents")
    {
        Guard.Against.Null(documentStore, nameof(documentStore));
        Guard.Against.Null(documentStore.Urls, nameof(documentStore.Urls));
        Guard.Against.NullOrEmpty(documentStore.Urls, nameof(documentStore.Urls));
        Guard.Against.Null(documentStore.Database, nameof(documentStore.Database));
        Guard.Against.Null(collectionName, nameof(collectionName));

        var builder = ((ISerilogUiOptionsBuilder)optionsBuilder);

        // TODO: Fix up RavenDB to allow multiple registrations. Think about multiple RavenDB clients
        // (singletons) used in data providers (scoped)
        if (builder.Services.Any(c => c.ImplementationType == typeof(RavenDbDataProvider)))
        {
            throw new NotSupportedException($"Adding multiple registrations of '{typeof(RavenDbDataProvider).FullName}' is not (yet) supported.");
        }

        builder.Services.AddSingleton(documentStore);
        builder.Services.AddScoped<IDataProvider>(_ => new RavenDbDataProvider(documentStore, collectionName));
    }
}